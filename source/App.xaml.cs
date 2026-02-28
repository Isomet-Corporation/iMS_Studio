using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using iMS_Studio.ViewModel;
using iMS;
using System.Windows.Input;
using Microsoft.Win32;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using ImsHwServer;
using Xceed.Wpf.DataGrid;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Threading;

namespace iMS_Studio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Grpc.Core.Channel channel;

        /* The iMS System we are connected to */
        private static ims_system thisIMS;

        private static Settings m_settings;
        private MainViewModel _MainViewModel = null;

        private Process HWServerApp = null;
        private static int lineCount = 0;

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            SplashScreen splash = new SplashScreen("resources\\Images\\isomet logo.jpg");
            splash.Show(false, true);

            // Global exception handling  
            Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(AppDispatcherUnhandledException);

            string startupErrorMsg = null;
            iMSNET.Init();

            string serverString = null;
            // Check for already running HW Servers on the local system
            Process[] localServer = Process.GetProcessesByName("ims_hw_server");
            if (localServer.Length > 0)
            {
                if ((e.Args.Count() > 0) && (Array.IndexOf(e.Args, "-nokill") >= e.Args.GetLowerBound(0)))
                {
                    serverString = "localhost";
                }
                else
                {
                    foreach (var proc in localServer)
                    {
                        proc.Kill();
                        proc.WaitForExit(5 * 1000);
                    }
                }

            }
            // If there are any command line arguments, try to connect to remote server specified otherwise start a local server.
            else if (e.Args.Count() > 0)
            {
                serverString = e.Args.FirstOrDefault(s => s.First() != '-');
            }

            if (string.IsNullOrEmpty(serverString))
            {
                // Start the H/W Server Process
                HWServerApp = new Process();
                HWServerApp.StartInfo.UseShellExecute = false;
                HWServerApp.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                HWServerApp.StartInfo.RedirectStandardOutput = true;
                HWServerApp.StartInfo.CreateNoWindow = true;
                HWServerApp.StartInfo.FileName = "ims_hw_server.exe";
                try
                {
                    HWServerApp.Start();
                }
                catch
                {
                    startupErrorMsg = "Can't Start H/W Server";
                }
                serverString = "localhost";
            }

            // Connect to the H/W server
            channel = new Channel(serverString + ":28241", ChannelCredentials.Insecure);
            var HWClient = new hw_list.hw_listClient(channel);

            if (startupErrorMsg == null)
            {
                bool client_ready = false;
                const int MAX_RETRIES = 10;
                int retries = 0;

                while (!client_ready && (retries++ != MAX_RETRIES))
                {
                    try
                    {
                        // Make sure we're starting clean
                        HWClient.reset(new Google.Protobuf.WellKnownTypes.Empty());
                        client_ready = true;
                    }
                    catch (Grpc.Core.RpcException ex)
                    {
                        if ( (ex.Status.StatusCode == Grpc.Core.StatusCode.Unavailable) ||
                            (ex.Status.StatusCode == Grpc.Core.StatusCode.Unknown) )
                        {
                            // Retry
                            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                        }
                        else
                        {
                            startupErrorMsg = "iMS Hardware Server not running or unable to start service\n(" + ex.Message + ")";
                            break;
                        }
                    }
                }

                if (startupErrorMsg == null && client_ready)
                {
                    using (var systems = HWClient.scan(new Empty()))
                    {
                        while (await systems.ResponseStream.MoveNext())
                        {
                            if (thisIMS == null)
                            {
                                ims_system ims = systems.ResponseStream.Current;
                                if ((ims.Ctlr.Model == "Not Available") || (ims.Synth.Model == "Not Available"))
                                    continue;
                                thisIMS = ims;
                                ConnectionStatus status = await HWClient.connectAsync(ims);
                            }
                        }
                    }
                    if (thisIMS == null)
                    {
                        // add error message if don't want to allow unconnected runtime
                    }
                }
                else
                {
                    splash.Close(TimeSpan.FromSeconds(0));
                    MessageBoxResult result = MessageBox.Show(startupErrorMsg, "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }

            }

            /* Create application command bindings */
            var new_binding = new CommandBinding(ApplicationCommands.New, NewCommandBinding_Executed, AlwaysEnabled_CanExecute);
            var open_binding = new CommandBinding(ApplicationCommands.Open, OpenCommandBinding_Executed, AlwaysEnabled_CanExecute);
            var save_binding = new CommandBinding(ApplicationCommands.Save, SaveCommandBinding_Executed, Save_CanExecute);
            var saveas_binding = new CommandBinding(ApplicationCommands.SaveAs, SaveAsCommandBinding_Executed, AlwaysEnabled_CanExecute);
            var dgc_paste_binding = new CommandBinding(ApplicationCommands.Paste, DataGrid_Paste, DataGrid_CanPaste);
            CommandManager.RegisterClassCommandBinding(typeof(Window), new_binding);
            CommandManager.RegisterClassCommandBinding(typeof(Window), open_binding);
            CommandManager.RegisterClassCommandBinding(typeof(Window), save_binding);
            CommandManager.RegisterClassCommandBinding(typeof(Window), saveas_binding);
            CommandManager.RegisterClassCommandBinding(typeof(DataGridControl), dgc_paste_binding);

            m_settings = new Settings(App.DirAppData, "iMSStudioSettings.xml");

            string proj_name = m_settings.Get("LastActiveProject", "");
            // Default Project
            m_imgproj = new ImageProject();
            bool proj_has_contents = false;
            if (string.IsNullOrWhiteSpace(proj_name) == false)
            {
                try
                {
                    if (m_imgproj.Load(proj_name))
                    {
                        proj_has_contents = true;
                    }
                }
                catch
                {}
            }

            //_saveFolder = _openFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            /* create the window */
            var mainWindow = new MainWindow();
            this.MainWindow = mainWindow;

            /* attach the mainViewModel */
            //if (startupErrorMsg == null)
            {

                _MainViewModel = new MainViewModel(m_imgproj, channel, thisIMS, m_settings);
                if (proj_has_contents == false)
                {
                    _MainViewModel.WindowName = "New Image Project";
                    // Create a default image
                    _MainViewModel.AddNewImageCommand.Execute(null);
                    _MainViewModel.IsDirty = false; // override flag
                }
                else
                {
                    _MainViewModel.WindowName = proj_name;
                    _MainViewModel.FileName = proj_name;
                }
                mainWindow.DataContext = _MainViewModel;
            }

            mainWindow.Show();
            splash.Close(TimeSpan.FromSeconds(2));

            //else
            {
                /* Update status bar with connection state */
                _MainViewModel.StatusBarText = (thisIMS == null) ? "Not Connected" : "Connected to iMS: " + thisIMS.ConnPort;

                if (HWServerApp != null)
                {
                    HWServerApp.OutputDataReceived += new DataReceivedEventHandler((hwsender, args) =>
                    {
                    // Prepend line numbers to each line of the output.
                    if (!String.IsNullOrEmpty(args.Data))
                        {
                            lineCount++;
                            _MainViewModel.ConsoleVM.ConsoleText += ("\n[" + lineCount + "]: " + args.Data);
                        }
                    });
                    HWServerApp.BeginOutputReadLine();
                }
                else
                {
                    if (serverString == "localhost")
                    {
                        _MainViewModel.ConsoleVM.ConsoleText = "Connected to Local Hardware Server Process (PID " + localServer[0].Id + ")";
                    }
                    else
                    {
                        _MainViewModel.ConsoleVM.ConsoleText = "Connected to Remote Hardware Server at " + serverString;
                    }
                }
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (thisIMS != null)
            {
                var HWClient = new hw_list.hw_listClient(channel);
                ConnectionStatus status = HWClient.disconnect(thisIMS);
            }

            // Close channel to H/W Server
            channel.ShutdownAsync().Wait();

            if (HWServerApp != null)
            {
                try
                {
                    HWServerApp.Kill();
                }
                catch
                {
                    //MessageBoxResult result = MessageBox.Show("Unable to stop Hardware service", "Internal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
#if DEBUG   // In debug mode do not custom-handle the exception, let Visual Studio handle it

            e.Handled = false;

#else

    ShowUnhandledException(e);    

#endif
        }

        void ShowUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            string errorMessage = string.Format("An application error occurred.\n\nError: {0}\n\nDo you want to continue?",

            e.Exception.Message + (e.Exception.InnerException != null ? "\n" +
            e.Exception.InnerException.Message : null));

            if (MessageBox.Show(errorMessage, "Application Error", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.No)
            {
                if (MessageBox.Show("WARNING: The application will close. Any changes will not be saved!\nDo you really want to close?", "Close the application!", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
            }
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_MainViewModel == null)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = _MainViewModel.IsDirty;
            }
        }

        private string _saveFolder;
        private void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Isomet Image Project|*.iip;*.xml|All Files|*.*";
            saveFileDialog.InitialDirectory = _saveFolder;
            if (saveFileDialog.ShowDialog() == true)
            {
                using (new WaitCursor())
                {
                    m_imgproj.Save(saveFileDialog.FileName);
                    m_settings.Set("LastActiveProject", saveFileDialog.FileName);
                    _MainViewModel.WindowName = saveFileDialog.FileName;
                    _MainViewModel.FileName = saveFileDialog.FileName;
                    _MainViewModel.IsDirty = false;
                }
            }
            _saveFolder = saveFileDialog.FileName;
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var fileName = _MainViewModel.FileName;
            if (fileName == null)
                SaveAsCommandBinding_Executed(sender, e);
            else
            {
                using (new WaitCursor())
                {
                    m_imgproj.Save(fileName);
                    m_settings.Set("LastActiveProject", fileName);
                    _MainViewModel.WindowName = fileName;
                    _MainViewModel.IsDirty = false;
                }
            }
        }

        private string _openFolder;
        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_MainViewModel.IsDirty)
            {
                MessageBoxResult result = MessageBox.Show("Image Project has been modified. Save first?", "Open Image Project", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Cancel) return;
                else if (result == MessageBoxResult.Yes) this.SaveCommandBinding_Executed(sender, e);
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Isomet Image Project|*.iip;*.xml|All Files|*.*";
            openFileDialog.InitialDirectory = _openFolder;
            if (openFileDialog.ShowDialog() == true)
            {
                using (new WaitCursor())
                {
                    m_imgproj.Clear();
                    m_imgproj.Load(openFileDialog.FileName);
                    _MainViewModel.Reset();
                    m_settings.Set("LastActiveProject", openFileDialog.FileName);
                    _MainViewModel.WindowName = openFileDialog.FileName;
                    _MainViewModel.FileName = openFileDialog.FileName;
                }
            }
            _openFolder = openFileDialog.FileName;
        }

        private void AlwaysEnabled_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private ImageProject m_imgproj;

        private void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_MainViewModel.IsDirty)
            {
                MessageBoxResult result = MessageBox.Show("Image Project has been modified. Save first?", "New Image Project", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Cancel) return;
                else if (result == MessageBoxResult.Yes) this.SaveCommandBinding_Executed(sender, e);
            }

            using (new WaitCursor())
            {
                m_imgproj.Clear();
                _MainViewModel.Reset();
                _MainViewModel.WindowName = "New Image Project";
                // Create a default image
                _MainViewModel.AddNewImageCommand.Execute(null);
            }
        }

        // Custom Paste Command for Data Grids
        private void DataGrid_CanPaste(object sender, CanExecuteRoutedEventArgs e)
        {
            var grid = (sender as DataGridControl);

            if (grid.SelectedCellRanges.Count > 0)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
            e.Handled = true;
        }

        private void DataGrid_Paste(object sender, ExecutedRoutedEventArgs e)
        {
            var grid = (sender as DataGridControl);

            List<string[]> rowData = ClipboardHelper.ParseClipboardData();

            if (rowData == null) return;
            else if (rowData.Count == 0) return;
            else if (grid.SelectedCellRanges.Count == 0) return;

            int clipboardGridWidth = rowData[0].Count();
            int clipboardGridHeight = rowData.Count;

            List<SelectionCellRange> newRange = new List<SelectionCellRange>();
            foreach (SelectionCellRange range in grid.SelectedCellRanges)
            // var range = grid.SelectedCellRanges[0];
            {
                if (!range.IsEmpty)
                {
                    int pasteWidthMultiple = (range.ColumnRange.Length / clipboardGridWidth);
                    int pasteHeightMultiple = (range.ItemRange.Length / clipboardGridHeight);
                    if (pasteWidthMultiple < 1) pasteWidthMultiple = 1;
                    if (pasteHeightMultiple < 1) pasteHeightMultiple = 1;

                    int startColumn = (range.ColumnRange.StartIndex);
                    int startRow = (range.ItemRange.StartIndex);

                    int currentRow = 0;
                    int currentCol = 0;

                    int endColumn = startColumn;
                    int endRow = startRow;

                    if (((startRow + (pasteHeightMultiple * clipboardGridHeight)) <= grid.Items.Count) &&
                        ((startColumn + (pasteWidthMultiple * clipboardGridWidth)) <= grid.Columns.Count))
                    {

                        for (int row = startRow; row < (startRow + (pasteHeightMultiple * clipboardGridHeight)); row++)
                        {
                            endRow = row;
                            if (row >= grid.Items.Count) break;

                            string[] individualRowData = rowData[currentRow++];
                            if (currentRow >= clipboardGridHeight) currentRow = 0;
                            for (int col = startColumn; col < (startColumn + (pasteWidthMultiple * clipboardGridWidth)); col++)
                            {
                                endColumn = col;
                                if (col >= grid.Columns.Count) break;

                                PropertyInfo prop = grid.Items[row].GetType().GetProperty(grid.Columns[col].FieldName);
                                if (prop == null)
                                {
                                    currentCol++;
                                    continue;
                                }

                                if (prop.PropertyType.GetProperty("Value") != null)
                                {
                                    // Custom class with Value property
                                    object target = prop.GetValue(grid.Items[row], null);
                                    dynamic targetAsType = Convert.ChangeType(target, prop.PropertyType);
                                    double d;
                                    if (Double.TryParse(individualRowData[currentCol++], out d))
                                    {
                                        targetAsType.Value = d;
                                        prop.SetValue(grid.Items[row], target);
                                    }
                                }
                                else if (prop.PropertyType == typeof(double))
                                {
                                    double d;
                                    if (Double.TryParse(individualRowData[currentCol++], out d))
                                    {
                                        prop.SetValue(grid.Items[row], d);
                                    }
                                }
                                else if (prop.PropertyType == typeof(float))
                                {
                                    float d;
                                    if (Single.TryParse(individualRowData[currentCol++], out d))
                                    {
                                        prop.SetValue(grid.Items[row], d);
                                    }
                                }
                                else if (prop.PropertyType == typeof(uint))
                                {
                                    uint i;
                                    if (UInt32.TryParse(individualRowData[currentCol++], out i))
                                    {
                                        prop.SetValue(grid.Items[row], i);
                                    }
                                }
                                else if (prop.PropertyType == typeof(int))
                                {
                                    int i;
                                    if (Int32.TryParse(individualRowData[currentCol++], out i))
                                    {
                                        prop.SetValue(grid.Items[row], i);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        prop.SetValue(grid.Items[row], individualRowData[currentCol++]);
                                    }
                                    catch (ArgumentException)
                                    {
                                        return;
                                    }
                                }
                                if (currentCol >= clipboardGridWidth) currentCol = 0;
                            }
                        }
                    }

                    newRange.Add(new SelectionCellRange(startRow, startColumn, endRow, endColumn));
                }
            }
            grid.SelectedCellRanges.Clear();
            foreach (var modifiedRange in newRange)
                grid.SelectedCellRanges.Add(modifiedRange);
        }

        public static string LayoutFileName
        {
            get
            {
                return "CurrentLayout.xml";
            }
        }

        public static string DefaultFileName
        {
            get
            {
                return "DefaultLayout.xml";
            }
        }

        /// <summary>
        /// Get a path to the directory where the application
        /// can persist/load user data on session exit and re-start.
        /// </summary>
        public static string DirAppData
        {
            get
            {
                string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                                 System.IO.Path.DirectorySeparatorChar + App.Company;

                try
                {
                    if (System.IO.Directory.Exists(dirPath) == false)
                        System.IO.Directory.CreateDirectory(dirPath);
                }
                catch
                {
                }

                return dirPath;
            }
        }

        public static string Company
        {
            get
            {
                return "Isomet";
            }
        }

    }

}
