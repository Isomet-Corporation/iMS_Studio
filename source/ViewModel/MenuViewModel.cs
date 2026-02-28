using Gat.Controls;
using iMS_Studio.ViewModel.Behaviour;
using Microsoft.Win32;
using Microsoft.Windows.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace iMS_Studio.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        public IEnumerable<MenuItemViewModel> Items { get; private set; }

        private readonly MenuItemViewModel FileMenuItemViewModel;
        private readonly MenuItemViewModel EditMenuItemViewModel;
        private readonly MenuItemViewModel WindowMenuItemViewModel;
        private readonly MenuItemViewModel ToolsMenuItemViewModel;
        private readonly MenuItemViewModel HelpMenuItemViewModel;
        private readonly MenuItemViewModel WindowDocumentsMenuItemViewModel;
        private readonly MenuItemViewModel WindowPanelsMenuItemViewModel;
        private bool IgnoreEvents = false;

        private WpfUIWindowDialogService uiDialogService;
        private AODeviceChooserVM aodevVM;

        public MenuViewModel(IEnumerable<DockWindowViewModel> dockWindows, IEnumerable<DockPaneViewModel> dockPanels)
        {
            this.uiDialogService = new WpfUIWindowDialogService();
            this.aodevVM = new AODeviceChooserVM();
            this.aodevVM.RequestCloseDialog += AodevVM_RequestCloseDialog;

            var items = new List<MenuItemViewModel>();

            // File Menu
            {
                var file = this.FileMenuItemViewModel = new MenuItemViewModel() { Header = "_File" };

                file.Items.Add(new MenuItemViewModel() { Header = "_New", IsCheckable = false, Command = ApplicationCommands.New });
                file.Items.Add(new MenuItemViewModel() { Header = "_Open", IsCheckable = false, Command = ApplicationCommands.Open });
                file.Items.Add(new MenuItemViewModel() { Header = "_Save", IsCheckable = false, Command = ApplicationCommands.Save });
                file.Items.Add(new MenuItemViewModel() { Header = "Save _As", IsCheckable = false, Command = ApplicationCommands.SaveAs });
                file.Items.Add(new MenuItemViewModel() { Header = "E_xit", IsCheckable = false, Command = new RelayCommand(call => Application.Current.MainWindow.Close()) });
                //file.Items.Add(new MenuItemViewModel() { Header = "_Exit", IsCheckable = false, Command = ApplicationCommands.Close });

                items.Add(file);
            }

            // Edit Menu
            {
                var edit = this.EditMenuItemViewModel = new MenuItemViewModel() { Header = "_Edit" };

                edit.Items.Add(new MenuItemViewModel() { Header = "_Undo", IsCheckable = false, Command = ApplicationCommands.Undo });
                edit.Items.Add(new MenuItemViewModel() { Header = "_Redo", IsCheckable = false, Command = ApplicationCommands.Redo });
                edit.Items.Add(new MenuItemViewModel() { Header = "_Cut", IsCheckable = false, Command = ApplicationCommands.Cut });
                edit.Items.Add(new MenuItemViewModel() { Header = "Cop_y", IsCheckable = false, Command = ApplicationCommands.Copy });
                edit.Items.Add(new MenuItemViewModel() { Header = "_Paste", IsCheckable = false, Command = ApplicationCommands.Paste });
                edit.Items.Add(new MenuItemViewModel() { Header = "Select _All", IsCheckable = false, Command = ApplicationCommands.SelectAll });

                items.Add(edit);
            }

            // Window Menu
            {
                var window = this.WindowMenuItemViewModel = new MenuItemViewModel() { Header = "_Window" };

                var window_docs = this.WindowDocumentsMenuItemViewModel = new MenuItemViewModel()
                {
                    Header = "_Documents",
                    IsCheckable = false,
                    IsEnabled = false
                };
                window.Items.Add(window_docs);
                foreach (var dockWindow in dockWindows)
                {
                    window_docs.Items.Add(GetMenuItemViewModel(dockWindow));
                    window_docs.IsEnabled = true;
                }

                var window_panels = this.WindowPanelsMenuItemViewModel = new MenuItemViewModel()
                {
                    Header = "_Workspace Panels",
                    IsCheckable = false
                };
                window.Items.Add(window_panels);
                foreach (var dockPanel in dockPanels)
                    window_panels.Items.Add(GetPanelItemViewModel(dockPanel));

                items.Add(window);
            }

            // Tools Menu
            {
                var tools = this.ToolsMenuItemViewModel = new MenuItemViewModel() { Header = "_Tools" };

                tools.Items.Add(new MenuItemViewModel() { Header = "_AO Device Chooser", IsCheckable = false, Command = this.AODeviceChooserCommand });
                items.Add(tools);
            }

            // Help Menu
            {
                var help = this.HelpMenuItemViewModel = new MenuItemViewModel() { Header = "_Help" };

                help.Items.Add(new MenuItemViewModel() { Header = "iMS _Documentation", IsCheckable = false, Command = this.LaunchDocCommand });
                help.Items.Add(new MenuItemViewModel() { Header = "_About", IsCheckable = false, Command = this.AboutBoxCommand });
                items.Add(help);
            }

            this.Items = items;
        }

        public event EventHandler<NewCompensationFunctionEventArgs> CreateNewCompensationFunction;
        private void AodevVM_RequestCloseDialog(object sender, RequestCloseDialogEventArgs e)
        {
            if (e.DialogResult == true)
            {
                var aod = aodevVM.AODModel;
                iMS.CompensationFunction cfunc = aod.GetCompensationFunction(aodevVM.TargetWavelength);
                if (aod.Model == "Custom")
                {
                    cfunc.Name = aod.Model + " " + aod.Material.Type.ToString() + " Fc=" + aod.CentreFrequency + " BW=" + aod.SweepBW + " Geo="
                        + aod.GeomConstant + " WL=" + aodevVM.TargetWavelength + "um";
                }
                else
                {
                    cfunc.Name = aod.Model + " WL=" + aodevVM.TargetWavelength + "um";
                }
                cfunc.PhaseInterpolationStyle = iMS.CompensationFunction.InterpolationStyle.LINEAR;
                CreateNewCompensationFunction?.Invoke(this, new NewCompensationFunctionEventArgs(cfunc));
            }
        }

        private ICommand _AboutBoxCommand;
        public ICommand AboutBoxCommand
        {
            get
            {
                if (_AboutBoxCommand == null)
                    _AboutBoxCommand = new RelayCommand(call => AboutBox_Show());
                return _AboutBoxCommand;
            }
        }

        private ICommand _LaunchDocCommand;
        public ICommand LaunchDocCommand
        {
            get
            {
                if (_LaunchDocCommand == null)
                    _LaunchDocCommand = new RelayCommand(call => LaunchHelp());
                return _LaunchDocCommand;
            }
        }

        private ICommand _AODeviceChooserCommand;
        public ICommand AODeviceChooserCommand
        {
            get
            {
                if (_AODeviceChooserCommand == null)
                    _AODeviceChooserCommand = new RelayCommand(call => AODeviceChooser());
                return _AODeviceChooserCommand;
            }
        }

        public class VersionInfo
        {
            public string Name { get; set; }
            public string Version { get; set; }
        }

        private List<VersionInfo> _ComponentVersionList;
        public List<VersionInfo> ComponentVersionList
        {
            get
            {
                if (_ComponentVersionList == null)
                    _ComponentVersionList = new List<VersionInfo>();
                return _ComponentVersionList;
            }
        }

        private void AboutBox_Show()
        {
            System.Reflection.Assembly assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();

            var aboutBox = new AboutControlView();
            AboutControlViewModel vm = (AboutControlViewModel)aboutBox.FindResource("ViewModel");
            Run linkText = new Run("http://www.isomet.com/");
            var link = new Hyperlink(linkText);
            link.ToolTip = "For latest updates please visit Isomet website";
            link.NavigateUri = new System.Uri("http://www.isomet.com/");
            link.RequestNavigate += (sender, e) =>
            {
                System.Diagnostics.Process.Start(e.Uri.ToString());
            };
            var img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri("pack://application:,,,/Images\\Isomet icon 128 128.ico");
            img.EndInit();
            vm.ApplicationLogo = img;
            vm.PublisherLogo = img;
            //vm.AdditionalNotes = link.OriginalString;
            vm.AdditionalNotes = "\nNotes\t\t\tv1.0\nMore Notes\t\tv2.9";
            (((aboutBox.Content as Grid)
                .Children[3] as Grid)
                    .Children[2] as Label).Content = link;

            var versionList = new ListView();
            versionList.ItemsSource = ComponentVersionList;
            var gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn { Header = "Component", Width = 130, DisplayMemberBinding = new Binding("Name") });
            gridView.Columns.Add(new GridViewColumn { Header = "Version", Width = 180, DisplayMemberBinding = new Binding("Version") });
            versionList.View = gridView;
            versionList.KeyDown += (o, e) =>
            {
                if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    var builder = new StringBuilder();
                    foreach (VersionInfo item in versionList.SelectedItems)
                    {
                        builder.AppendLine(item.Name + " = " + item.Version);
                    }

                    Clipboard.SetText(builder.ToString());
                }
            };
            (aboutBox.Content as Grid).Children.RemoveAt(4);
            Grid.SetRow(versionList, 4);
            (aboutBox.Content as Grid).Children.Add(versionList);

            vm.Window.Content = aboutBox;
            vm.Window.Show();
            
        }

        private static string DefaultWebBrowser
        {
            get
            {

                string path = @"\http\shell\open\command";

                using (RegistryKey reg = Registry.ClassesRoot.OpenSubKey(path))
                {
                    if (reg != null)
                    {
                        string webBrowserPath = reg.GetValue(String.Empty) as string;

                        if (!String.IsNullOrEmpty(webBrowserPath))
                        {
                            if (webBrowserPath.First() == '"')
                            {
                                return webBrowserPath.Split('"')[1];
                            }

                            return webBrowserPath.Split(' ')[0];
                        }
                    }

                    return null;
                }
            }
        }

        private void LaunchHelp()
        {
            RegistryKey localKey;
            if (Environment.Is64BitOperatingSystem)
                localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            else
                localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            if (localKey != null)
            {
                using (RegistryKey myKey = localKey.OpenSubKey(@"Software\Isomet\iMS_SDK"))
                {
                    if (myKey != null)
                    {
                        object InstallPath = myKey.GetValue("InstallPath");
                        if (!string.IsNullOrEmpty(InstallPath as string))
                        {
                            try
                            {
                                Process proc = new Process();
                                proc.StartInfo.FileName = DefaultWebBrowser;
                                proc.StartInfo.Arguments = "\"" + InstallPath + "\\doc\\html\\index.html" + "\"";
                                proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

                                proc.Start();
                            }
                            catch (Exception) { }
                        }

                    }
                }
            }
        }

        private void AODeviceChooser()
        {
            var result = this.uiDialogService.ShowDialog("Acousto-Optice Device Compensation", aodevVM);
        }

        private DocumentMenuItemViewModel GetMenuItemViewModel(DockWindowViewModel dockWindowViewModel)
        {
            var menuItemViewModel = new DocumentMenuItemViewModel(dockWindowViewModel);
            menuItemViewModel.IsCheckable = true;

            menuItemViewModel.Header = dockWindowViewModel.Title;
            menuItemViewModel.IsChecked = dockWindowViewModel.IsSelected;

            dockWindowViewModel.PropertyChanged += (o, e) =>
            {
                if (!IgnoreEvents)
                {
                    if (e.PropertyName == nameof(DockWindowViewModel.IsSelected))
                        menuItemViewModel.IsChecked = dockWindowViewModel.IsSelected;
                    if (e.PropertyName == nameof(DockWindowViewModel.Title))
                        menuItemViewModel.Header = dockWindowViewModel.Title;
                }
            };

            menuItemViewModel.PropertyChanged += (o, e) =>
            {
                if (!IgnoreEvents)
                {
                    if (e.PropertyName == nameof(MenuItemViewModel.IsChecked))
                    {
                        IgnoreEvents = true;
                        if (menuItemViewModel.IsChecked == true)
                        {
                            dockWindowViewModel.IsSelected = true;
                            foreach (DocumentMenuItemViewModel window in this.WindowDocumentsMenuItemViewModel.Items)
                            {
                                if (window != menuItemViewModel)
                                    window.IsChecked = false;
                            }
                        }
                        else menuItemViewModel.IsChecked = true;
                        IgnoreEvents = false;
                    }
                }
            };

            return menuItemViewModel;
        }

        private WorkspacePanelMenuItemViewModel GetPanelItemViewModel(DockPaneViewModel dockPaneViewModel)
        {
            var menuItemViewModel = new WorkspacePanelMenuItemViewModel(dockPaneViewModel);
            menuItemViewModel.IsCheckable = true;

            menuItemViewModel.Header = dockPaneViewModel.Title;
            menuItemViewModel.IsChecked = dockPaneViewModel.IsVisible;

            dockPaneViewModel.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(DockPaneViewModel.IsVisible))
                    menuItemViewModel.IsChecked = dockPaneViewModel.IsVisible;
            };

            menuItemViewModel.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(MenuItemViewModel.IsChecked))
                {
                    if (dockPaneViewModel.IsVisible != menuItemViewModel.IsChecked)
                    {
                        dockPaneViewModel.IsVisible = menuItemViewModel.IsChecked;
                        if (menuItemViewModel.IsChecked == true)
                        {
                            dockPaneViewModel.IsSelected = true;
                        }
                    }
                }
            };

            return menuItemViewModel;
        }

        public void AddNewImageToWindowMenu(DockWindowViewModel dockWindow)
        {
            this.WindowDocumentsMenuItemViewModel.Items.Add(GetMenuItemViewModel(dockWindow));
            this.WindowDocumentsMenuItemViewModel.IsEnabled = true;
        }

        public void RemoveImageFromWindowMenu(DockWindowViewModel dockWindow)
        {
            foreach (DocumentMenuItemViewModel window in this.WindowDocumentsMenuItemViewModel.Items)
            {
                if (window.DocReference == dockWindow)
                {
                    WindowDocumentsMenuItemViewModel.Items.Remove(window);
                    break;
                }
            }
            if (this.WindowDocumentsMenuItemViewModel.Items.Count == 0)
                this.WindowDocumentsMenuItemViewModel.IsEnabled = false;
        }

        public void RemoveAll()
        {
            WindowDocumentsMenuItemViewModel.Items.Clear();
            this.WindowDocumentsMenuItemViewModel.IsEnabled = false;
        }


    }
}
