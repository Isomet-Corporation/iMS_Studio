using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMS;

namespace iMS_Studio.ViewModel
{
    public class ToneBufferEntryVM : BaseViewModel
    {
        #region Properties

        public double FreqCh1
        {
            get { return _tbe.FreqCh1.Value; }
            set
            {
                if (_tbe.FreqCh1.Value != value)
                {
                    _tbe.FreqCh1.Value = value;
                    OnPropertyChanged("FreqCh1");
                }
            }
        }

        public double AmplCh1
        {
            get { return _tbe.AmplCh1.Value; }
            set
            {
                if (_tbe.AmplCh1.Value != value)
                {
                    _tbe.AmplCh1.Value = value;
                    OnPropertyChanged("AmplCh1");
                }
            }
        }

        public double PhaseCh1
        {
            get { return _tbe.PhaseCh1.Value; }
            set
            {
                if (_tbe.PhaseCh1.Value != value)
                {
                    _tbe.PhaseCh1.Value = value;
                    OnPropertyChanged("PhaseCh1");
                }
            }
        }

        public double FreqCh2
        {
            get { return _tbe.FreqCh2.Value; }
            set
            {
                if (_tbe.FreqCh2.Value != value)
                {
                    _tbe.FreqCh2.Value = value;
                    OnPropertyChanged("FreqCh2");
                }
            }
        }

        public double AmplCh2
        {
            get { return _tbe.AmplCh2.Value; }
            set
            {
                if (_tbe.AmplCh2.Value != value)
                {
                    _tbe.AmplCh2.Value = value;
                    OnPropertyChanged("AmplCh2");
                }
            }
        }

        public double PhaseCh2
        {
            get { return _tbe.PhaseCh2.Value; }
            set
            {
                if (_tbe.PhaseCh2.Value != value)
                {
                    _tbe.PhaseCh2.Value = value;
                    OnPropertyChanged("PhaseCh2");
                }
            }
        }

        public double FreqCh3
        {
            get { return _tbe.FreqCh3.Value; }
            set
            {
                if (_tbe.FreqCh3.Value != value)
                {
                    _tbe.FreqCh3.Value = value;
                    OnPropertyChanged("FreqCh3");
                }
            }
        }

        public double AmplCh3
        {
            get { return _tbe.AmplCh3.Value; }
            set
            {
                if (_tbe.AmplCh3.Value != value)
                {
                    _tbe.AmplCh3.Value = value;
                    OnPropertyChanged("AmplCh3");
                }
            }
        }

        public double PhaseCh3
        {
            get { return _tbe.PhaseCh3.Value; }
            set
            {
                if (_tbe.PhaseCh3.Value != value)
                {
                    _tbe.PhaseCh3.Value = value;
                    OnPropertyChanged("PhaseCh3");
                }
            }
        }

        public double FreqCh4
        {
            get { return _tbe.FreqCh4.Value; }
            set
            {
                if (_tbe.FreqCh4.Value != value)
                {
                    _tbe.FreqCh4.Value = value;
                    OnPropertyChanged("FreqCh4");
                }
            }
        }

        public double AmplCh4
        {
            get { return _tbe.AmplCh4.Value; }
            set
            {
                if (_tbe.AmplCh4.Value != value)
                {
                    _tbe.AmplCh4.Value = value;
                    OnPropertyChanged("AmplCh4");
                }
            }
        }

        public double PhaseCh4
        {
            get { return _tbe.PhaseCh4.Value; }
            set
            {
                if (_tbe.PhaseCh4.Value != value)
                {
                    _tbe.PhaseCh4.Value = value;
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
            }
        }

        public static readonly int FieldCount = 12;

        private TBEntry _tbe = null;

        public bool IsViewFor(TBEntry other)
        {
            return other == _tbe;
        }

        public TBEntry GetUnderlyingObject()
        {
            return _tbe;
        }

        public ToneBufferEntryVM(TBEntry tbe)
        {
            _tbe = tbe;
        }

        public ToneBufferEntryVM()
        {
            _tbe = new TBEntry();
        }


    }
}
