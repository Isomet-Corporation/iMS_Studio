using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMS;

namespace iMS_Studio.ViewModel
{
    public class ImagePointVM : BaseViewModel
    {
        #region Properties

        public uint SyncD
        {
            get { return _img_pt.SyncD; }
            set
            {
                if (_img_pt.SyncD != value)
                {
                    _img_pt.SyncD = value;
                    OnPropertyChanged("SyncD");
                }
            }
        }

        public float SyncA1
        {
            get { return _img_pt.SyncA1; }
            set
            {
                if (_img_pt.SyncA1 != value)
                {
                    _img_pt.SyncA1 = value;
                    OnPropertyChanged("SyncA1");
                }
            }
        }

        public float SyncA2
        {
            get { return _img_pt.SyncA2; }
            set
            {
                if (_img_pt.SyncA2 != value)
                {
                    _img_pt.SyncA2 = value;
                    OnPropertyChanged("SyncA2");
                }
            }
        }

        public double FreqCh1
        {
            get { return _img_pt.FreqCh1.Value; }
            set
            {
                if (_img_pt.FreqCh1.Value != value)
                {
                    _img_pt.FreqCh1.Value = value;
                    OnPropertyChanged("FreqCh1");
                }
            }
        }

        public double AmplCh1
        {
            get { return _img_pt.AmplCh1.Value; }
            set
            {
                if (_img_pt.AmplCh1.Value != value)
                {
                    _img_pt.AmplCh1.Value = value;
                    OnPropertyChanged("AmplCh1");
                }
            }
        }

        public double PhaseCh1
        {
            get { return _img_pt.PhaseCh1.Value; }
            set
            {
                if (_img_pt.PhaseCh1.Value != value)
                {
                    _img_pt.PhaseCh1.Value = value;
                    OnPropertyChanged("PhaseCh1");
                }
            }
        }

        public double FreqCh2
        {
            get { return _img_pt.FreqCh2.Value; }
            set
            {
                if (_img_pt.FreqCh2.Value != value)
                {
                    _img_pt.FreqCh2.Value = value;
                    OnPropertyChanged("FreqCh2");
                }
            }
        }

        public double AmplCh2
        {
            get { return _img_pt.AmplCh2.Value; }
            set
            {
                if (_img_pt.AmplCh2.Value != value)
                {
                    _img_pt.AmplCh2.Value = value;
                    OnPropertyChanged("AmplCh2");
                }
            }
        }

        public double PhaseCh2
        {
            get { return _img_pt.PhaseCh2.Value; }
            set
            {
                if (_img_pt.PhaseCh2.Value != value)
                {
                    _img_pt.PhaseCh2.Value = value;
                    OnPropertyChanged("PhaseCh2");
                }
            }
        }

        public double FreqCh3
        {
            get { return _img_pt.FreqCh3.Value; }
            set
            {
                if (_img_pt.FreqCh3.Value != value)
                {
                    _img_pt.FreqCh3.Value = value;
                    OnPropertyChanged("FreqCh3");
                }
            }
        }

        public double AmplCh3
        {
            get { return _img_pt.AmplCh3.Value; }
            set
            {
                if (_img_pt.AmplCh3.Value != value)
                {
                    _img_pt.AmplCh3.Value = value;
                    OnPropertyChanged("AmplCh3");
                }
            }
        }

        public double PhaseCh3
        {
            get { return _img_pt.PhaseCh3.Value; }
            set
            {
                if (_img_pt.PhaseCh3.Value != value)
                {
                    _img_pt.PhaseCh3.Value = value;
                    OnPropertyChanged("PhaseCh3");
                }
            }
        }

        public double FreqCh4
        {
            get { return _img_pt.FreqCh4.Value; }
            set
            {
                if (_img_pt.FreqCh4.Value != value)
                {
                    _img_pt.FreqCh4.Value = value;
                    OnPropertyChanged("FreqCh4");
                }
            }
        }

        public double AmplCh4
        {
            get { return _img_pt.AmplCh4.Value; }
            set
            {
                if (_img_pt.AmplCh4.Value != value)
                {
                    _img_pt.AmplCh4.Value = value;
                    OnPropertyChanged("AmplCh4");
                }
            }
        }

        public double PhaseCh4
        {
            get { return _img_pt.PhaseCh4.Value; }
            set
            {
                if (_img_pt.PhaseCh4.Value != value)
                {
                    _img_pt.PhaseCh4.Value = value;
                    OnPropertyChanged("PhaseCh4");
                }
            }
        }

        #endregion

        public double GetByIndex(int index)
        {
            switch (index)
            {
                case 0: return FreqCh1; 
                case 1: return AmplCh1;
                case 2: return PhaseCh1;
                case 3: return FreqCh2;
                case 4: return AmplCh2;
                case 5: return PhaseCh2;
                case 6: return FreqCh3;
                case 7: return AmplCh3;
                case 8: return PhaseCh3;
                case 9: return FreqCh4;
                case 10: return AmplCh4;
                case 11: return PhaseCh4;
                case 12: return SyncD;
                case 13: return SyncA1;
                case 14: return SyncA2;
                default: return 0.0;
            }
        }

        public void SetByIndex(int index, double value)
        {
            switch (index)
            {
                case 0: FreqCh1 = value; break;
                case 1: AmplCh1 = value; break;
                case 2: PhaseCh1 = value; break;
                case 3: FreqCh2 = value; break;
                case 4: AmplCh2 = value; break;
                case 5: PhaseCh2 = value; break;
                case 6: FreqCh3 = value; break;
                case 7: AmplCh3 = value; break;
                case 8: PhaseCh3 = value; break;
                case 9: FreqCh4 = value; break;
                case 10: AmplCh4 = value; break;
                case 11: PhaseCh4 = value; break;
                case 12: SyncD = (uint)value; break;
                case 13: SyncA1 = (float)value; break;
                case 14: SyncA2 = (float)value; break;
            }
        }

        public static readonly int FieldCount = 15;

        private ImagePoint _img_pt = null;

        public bool IsViewFor(ImagePoint other)
        {
            return other == _img_pt;
        }

        public ImagePoint GetUnderlyingObject()
        {
            return _img_pt;
        }

        public ImagePointVM(ImagePoint img_pt)
        {
            _img_pt = img_pt;
        }

        public ImagePointVM()
        {
            _img_pt = new ImagePoint();
        }

    }
}
