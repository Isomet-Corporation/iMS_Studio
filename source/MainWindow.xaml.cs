using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf;
using AvalonDock.Layout.Serialization;
using AvalonDock;
using iMS_Studio.ViewModel;

namespace iMS_Studio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddPresetButton_Click(object sender, RoutedEventArgs e)
        {
            var addButton = sender as FrameworkElement;
            if (addButton != null)
            {
                addButton.ContextMenu.IsOpen = true;
            }
        }

        private void DockingManager_Loaded(object sender, RoutedEventArgs e)
        {
            //int i=0;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var VM = (this.DataContext as MainViewModel);
            if (VM != null)
            {
                if (VM.IsDirty)
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("Image Project has been modified. Save before exit?", "New Image Project", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Cancel) e.Cancel = true;
                    else if (result == MessageBoxResult.Yes)
                    {
                        if (ApplicationCommands.Save.CanExecute(null, null))
                            ApplicationCommands.Save.Execute(null, null);
                    }
                }
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            // Detect Mouse Clicks on any part of the window that isn't over the Project Explorer and cancel any edits in progress
            if (e.Source.GetType() == typeof(AvalonDock.Controls.LayoutAnchorablePaneControl))
            {
                var item = e.Source as AvalonDock.Controls.LayoutAnchorablePaneControl;
                if (item.SelectedItem.GetType() == typeof(AvalonDock.Layout.LayoutAnchorable))
                {
                    var pane = item.SelectedItem as AvalonDock.Layout.LayoutAnchorable;
                    if (pane.ContentId == "ProjectExplorer")
                        return;
                }
            }

            var VM = (this.DataContext as MainViewModel);
            if (VM != null)
            {
                VM.ClearAllIsEditingExceptThis(sender);
            }
        }
    }

}
