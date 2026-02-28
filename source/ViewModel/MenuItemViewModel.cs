using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace iMS_Studio.ViewModel
{
  public class MenuItemViewModel : BaseViewModel
  {
    #region Properties

    public string Header { get; set; }
    public bool IsCheckable { get; set; }
    public ObservableCollection<MenuItemViewModel> Items { get; set; }
    public ICommand Command { get; set; }

    #region IsChecked
    private bool _IsChecked;
    public bool IsChecked
    {
      get { return _IsChecked; }
      set
      {
        if (_IsChecked != value)
        {
          _IsChecked = value;
          OnPropertyChanged(nameof(IsChecked));
        }
      }
    }
        #endregion

        #region IsEnabled
        private bool _IsEnabled;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set
            {
                if (_IsEnabled != value)
                {
                    _IsEnabled = value;
                    OnPropertyChanged(nameof(IsEnabled));
                }
            }
        }
        #endregion

        #endregion

        public MenuItemViewModel()
    {
      this.Items = new ObservableCollection<MenuItemViewModel>();
            this.IsEnabled = true;
    }
  }

    public class DocumentMenuItemViewModel : MenuItemViewModel
    {
        private DockWindowViewModel _dockWindow;
        public DockWindowViewModel DocReference
        {
            get { return _dockWindow; }
            private set
            {
                _dockWindow = value;
            }
        }

        public DocumentMenuItemViewModel(DockWindowViewModel window) : base()
        {
            DocReference = window;
        }
    }

    public class WorkspacePanelMenuItemViewModel : MenuItemViewModel
    {
        private DockPaneViewModel _dockPanel;
        public DockPaneViewModel DocReference
        {
            get { return _dockPanel; }
            private set
            {
                _dockPanel = value;
            }
        }

        public WorkspacePanelMenuItemViewModel(DockPaneViewModel panel) : base()
        {
            DocReference = panel;
        }
    }

}
