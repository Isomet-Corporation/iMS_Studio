using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMS;

namespace iMS_Studio.ViewModel
{
    class CompensationPointSpecificationViewModel : BaseViewModel
    {
        #region Properties

        #region PointFreq
        public double PointFreq
        {
            get { return _comp_pt.Freq.Value; }
            set
            {
                if (_comp_pt.Freq.Value != value)
                {
                    _comp_pt.Freq.Value = value;
                    OnPropertyChanged(nameof(PointFreq));
                }
            }
        }
        #endregion

        #region PointAmpl
        public double PointAmpl
        {
            get { return _comp_pt.Spec.Amplitude.Value; }
            set
            {
                if (_comp_pt.Spec.Amplitude.Value != value)
                {
                    _comp_pt.Spec.Amplitude.Value = value;
                    OnPropertyChanged(nameof(PointAmpl));
                }
            }
        }
        #endregion

        #region PointPhase
        public double PointPhase
        {
            get { return _comp_pt.Spec.Phase.Value; }
            set
            {
                if (_comp_pt.Spec.Phase.Value != value)
                {
                    _comp_pt.Spec.Phase.Value = value;
                    OnPropertyChanged(nameof(PointPhase));
                }
            }
        }
        #endregion

        #region SyncDig
        public uint SyncDig
        {
            get { return _comp_pt.Spec.SyncDig; }
            set
            {
                if (_comp_pt.Spec.SyncDig != value)
                {
                    _comp_pt.Spec.SyncDig = value;
                    OnPropertyChanged(nameof(SyncDig));
                }
            }
        }
        #endregion

        #region SyncAnlg
        public double SyncAnlg
        {
            get { return _comp_pt.Spec.SyncAnlg; }
            set
            {
                if (_comp_pt.Spec.SyncAnlg != value)
                {
                    _comp_pt.Spec.SyncAnlg = value;
                    OnPropertyChanged(nameof(SyncAnlg));
                }
            }
        }
        #endregion

        #endregion

        public double GetByIndex(int index)
        {
            switch (index)
            {
                case 0: return PointFreq;
                case 1: return PointAmpl;
                case 2: return PointPhase;
                case 3: return SyncDig;
                case 4: return SyncAnlg;
                default: return 0.0;
            }
        }

        public void SetByIndex(int index, double value)
        {
            switch (index)
            {
                case 0: PointFreq = value; break;
                case 1: PointAmpl = value; break;
                case 2: PointPhase = value; break;
                case 3: SyncDig = (uint)value; break;
                case 4: SyncAnlg = (float)value; break;
            }
        }

        public static readonly int FieldCount = 5;

        private CompensationPointSpecification _comp_pt = null;

        public bool IsViewFor(CompensationPointSpecification other)
        {
            return other == _comp_pt;
        }

        public CompensationPointSpecification GetUnderlyingObject()
        {
            return _comp_pt;
        }

        public CompensationPointSpecificationViewModel(CompensationPointSpecification comp_pt)
        {
            _comp_pt = comp_pt;
        }

        public CompensationPointSpecificationViewModel()
        {
            _comp_pt = new CompensationPointSpecification();
        }
    }
}
