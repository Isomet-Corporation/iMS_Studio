using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMS;

namespace iMS_Studio.ViewModel
{
    class ImageSequenceEntryViewModel : BaseViewModel
    {

        #region Properties

        #region UUID
        private System.Guid _UUID;
        public System.Guid UUID
        {
            get { return _UUID; }
            set
            {
                if (_UUID != value)
                {
                    _UUID = value;
                    OnPropertyChanged(nameof(UUID));
                }
            }
        }
        #endregion

        #region ImageName
        private string _Name;
        public string ImageName
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged(nameof(ImageName));
                }
            }
        }
        #endregion

        #region ExtDiv
        private int _ExtDiv;
        public int ExtDiv
        {
            get { return _ExtDiv; }
            set
            {
                if (_ExtDiv != value)
                {
                    _ExtDiv = value;
                    OnPropertyChanged(nameof(ExtDiv));
                }
            }
        }
        #endregion

        #region IntOsc
        private double _IntOsc;
        public double IntOsc
        {
            get { return _IntOsc; }
            set
            {
                if (_IntOsc != value)
                {
                    _IntOsc = value;
                    OnPropertyChanged(nameof(IntOsc));
                }
            }
        }
        #endregion

        #region RptType
        private ImageRepeats _RptType;
        public ImageRepeats RptType
        {
            get { return _RptType; }
            set
            {
                if (_RptType != value)
                {
                    _RptType = value;
                    OnPropertyChanged(nameof(RptType));
                }
            }
        }
        #endregion

        #region NumRpts
        private int _NumRpts;
        public int NumRpts
        {
            get { return _NumRpts; }
            set
            {
                if (_NumRpts != value)
                {
                    _NumRpts = value;
                    OnPropertyChanged(nameof(NumRpts));
                }
            }
        }
        #endregion

        #region PostImgDelay
        private double _PostImgDelay;
        public double PostImgDelay
        {
            get { return _PostImgDelay; }
            set
            {
                if (_PostImgDelay != value)
                {
                    _PostImgDelay = value;
                    OnPropertyChanged(nameof(PostImgDelay));
                }
            }
        }
        #endregion

        #region SyncOutDelay
        private double _SyncOutDelay;
        public double SyncOutDelay
        {
            get { return _SyncOutDelay; }
            set
            {
                if (_SyncOutDelay != value)
                {
                    _SyncOutDelay = value;
                    OnPropertyChanged(nameof(SyncOutDelay));
                }
            }
        }
        #endregion

        #endregion

        private ImageSequenceEntry _seq_entry = null;

        public bool IsViewFor(ImageSequenceEntry other)
        {
            return other == _seq_entry;
        }

        public ImageSequenceEntry GetUnderlyingObject()
        {
            return _seq_entry;
        }

        public ImageSequenceEntryViewModel(ImageSequenceEntry seq_entry)
        {
            _seq_entry = seq_entry;
            _UUID = seq_entry.UUID;
        }

        public ImageSequenceEntryViewModel()
        {
            _seq_entry = new ImageSequenceEntry();
            _UUID = _seq_entry.UUID;
        }

    }
}
