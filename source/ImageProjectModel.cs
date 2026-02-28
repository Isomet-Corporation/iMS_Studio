using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using iMS;

namespace iMS_Studio_first_attempt
{
    public class ImageProjectModel : INotifyPropertyChanged
    {
        private ImageProject _imgProj;

        public ImageFileList ImageFileList {
            get { return _imgProj.imgFileList; }
            set
            {
                _imgProj.imgFileList = value;
                this.NotifyPropertyChanged("ImageFileList");
            }
        }

        public CompensationTableList CompensationTableList
        {
            get { return _imgProj.cTblList; }
            set
            {
                _imgProj.cTblList = value;
                this.NotifyPropertyChanged("CompensationTableList");
            }
        }

        public ToneBufferList ToneBufferList
        {
            get { return _imgProj.toneBufList; }
            set
            {
                _imgProj.toneBufList = value;
                this.NotifyPropertyChanged("ToneBufferList");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }
}
