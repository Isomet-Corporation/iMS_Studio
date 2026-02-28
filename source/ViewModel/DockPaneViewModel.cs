using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMS;
using System.Windows.Input;

namespace iMS_Studio.ViewModel
{
    public abstract class DockPaneViewModel : BaseViewModel
    {
        #region CloseCommand
        private ICommand _CloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                return null;
            }
        }
        #endregion

        #region IsClosed
        public bool IsClosed
        {
            get { return false; }
        }
        #endregion

        #region CanClose
        public bool CanClose
        {
            get { return false; }
        }
        #endregion

        #region Title
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (_Title != value)
                {
                    _Title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }
        #endregion

        #region IsSelected
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        #endregion

        #region IsVisible
        private bool _IsVisible;
        public bool IsVisible
        {
            get { return _IsVisible; }
            set
            {
                if (value != _IsVisible)
                {
                    _IsVisible = value;
                    OnPropertyChanged("IsVisible");
                }
            }
        }
        #endregion

        public abstract void Reset();

        public DockPaneViewModel()
        {
            IsVisible = true;
        }
    }
}
