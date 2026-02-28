using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMS_Studio.ViewModel
{
  public abstract class BaseViewModel: INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler eventHandler = this.PropertyChanged;
        if (eventHandler != null)
        {
            eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

        #region ContentId
        private string _contentId = null;
        public string ContentId
        {
            get { return _contentId; }
            set
            {
                if (_contentId != value)
                {
                    _contentId = value;
                    OnPropertyChanged("ContentId");
                }
            }
        }

        #endregion

    }

}
