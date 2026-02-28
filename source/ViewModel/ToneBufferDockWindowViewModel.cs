using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMS;
using System.Collections.Specialized;
using Xceed.Wpf.DataGrid;
using System.Windows.Input;
using System.ComponentModel;

namespace iMS_Studio.ViewModel
{
    public enum ControlSourceVM
    {
        [Description("User")]
        HOST,
        [Description("External")]
        EXTERNAL,
        [Description("Extended External")]
        EXTERNAL_EXTENDED
    };

    public class ToneBufferDockWindowViewModel : DockWindowViewModel
    {
        private int _rowUpdated;
        public int RowUpdated
        {
            get { return _rowUpdated; }
            set
            {
                _rowUpdated = value;
                OnPropertyChanged("RowUpdated");
            }
        }

        private int _rowSelected;
        public int RowSelected
        {
            get { return _rowSelected; }
            set
            {
                if (_rowSelected != value)
                {
                    _rowSelected = value;
                    OnPropertyChanged("RowSelected");
                }
            }
        }

        private ToneBuffer _tbuf = null;
        public ToneBuffer TBufRef
        {
            get { return _tbuf; }
            set
            {
                _tbuf = value;
                var toneEntries = new ObservableCollectionEx<ToneBufferEntryVM>();
                foreach (var pt in _tbuf)
                    toneEntries.Add(new ToneBufferEntryVM(pt));
                toneEntries.ItemPropertyChanged += (o, e) =>
                {
                    this.IsDirty = true;
                    RowUpdated = e.CollectionIndex;
                };
                ToneData = toneEntries;

            }
        }
       
        public ToneBufferDockWindowViewModel(ToneBuffer tbuf) : base()
        {
            _tbuf = tbuf;

            _toneEntries = new ObservableCollectionEx<ToneBufferEntryVM>();
            foreach (var tbe in tbuf)
                _toneEntries.Add(new ToneBufferEntryVM(tbe));

            ContentId = "Tone Buffer " + System.Guid.NewGuid().ToString();

            _toneEntries.ItemPropertyChanged += (o, e) =>
            {
                this.IsDirty = true;
                RowUpdated = e.CollectionIndex;
            };
            _rowSelected = 0;

            this.ControlSource = ControlSourceVM.HOST;
            this.AmplCompEnabled = true;
            this.PhaseCompEnabled = true;
        }

        private ObservableCollectionEx<ToneBufferEntryVM> _toneEntries;
        public ObservableCollectionEx<ToneBufferEntryVM> ToneData
        {
            get { return _toneEntries; }
            set
            {
                ApplyPropertyChange<ToneBufferDockWindowViewModel, ObservableCollectionEx<ToneBufferEntryVM>>(ref _toneEntries, o => o.ToneData, value);
            }
        }

        /* Table Manipulation Commands */
        #region InterpolateCommand
        private RelayCommand<IList<SelectionCellRange>> _InterpolateCommand;
        public ICommand InterpolateCommand
        {
            get
            {
                if (_InterpolateCommand == null)
                {
                    _InterpolateCommand = new RelayCommand<IList<SelectionCellRange>>(param => InterpolateCommand_Execute(param),
                        param => IfSelected_CanExecute(param));
                }
                return _InterpolateCommand;
            }
        }

        void InterpolateCommand_Execute(IList<SelectionCellRange> range)
        {
            if (range != null)
            {
                using (new WaitCursor())
                {
                    ObservableCollectionEx<ToneBufferEntryVM> data = new ObservableCollectionEx<ToneBufferEntryVM>(ToneData);
                    List<List<int>> selectedCells = new List<List<int>>();
                    for (int i = 0; i < ToneBufferEntryVM.FieldCount; i++) selectedCells.Add(new List<int>());
                    foreach (var r in range)
                    {
                        for (int i = r.ColumnRange.StartIndex; i <= r.ColumnRange.EndIndex; i++)
                        {
                            if (i < ToneBufferEntryVM.FieldCount)
                            {
                                for (int j = r.ItemRange.StartIndex; j <= r.ItemRange.EndIndex; j++)
                                {
                                    if (!selectedCells[i].Exists(x => x == j)) selectedCells[i].Add(j);
                                }
                            }
                        }
                    }
                    foreach (var column in selectedCells)
                    {
                        if (column.Count < 2) continue;
                        int startRow = column.Min();
                        int endRow = column.Max();
                        double startValue = data[startRow].GetByIndex(selectedCells.IndexOf(column));
                        double endValue = data[endRow].GetByIndex(selectedCells.IndexOf(column));
                        for (int i = 0; i <= (endRow - startRow); i++)
                        {
                            data[startRow + i].SetByIndex(selectedCells.IndexOf(column), startValue + (endValue - startValue) * ((double)i / (endRow - startRow)));
                        }
                    }
                    ToneData = data;
                }
            }
        }

        #endregion

        bool IfSelected_CanExecute(IList<SelectionCellRange> range)
        {
            if (range == null) return false;
            else if (range.Count == 0) return false;
            else return true;
        }

        private iMS.SignalPath.ToneBufferControl _TBufControl;
        public iMS.SignalPath.ToneBufferControl TBufControl
        {
            get { return _TBufControl; }
            set
            {
                if (_TBufControl != value)
                {
                    _TBufControl = value;
                   // OnPropertyChanged("TBufControl");
                }
            }
        }

        #region ControlSource
        private ControlSourceVM _ctrlSource;
        public ControlSourceVM ControlSource
        {
            get { return _ctrlSource; }
            set
            {
                if (value != _ctrlSource)
                {
                    _ctrlSource = value;
                    switch (value)
                    {
                        case ControlSourceVM.EXTERNAL: TBufControl = iMS.SignalPath.ToneBufferControl.EXTERNAL; break;
                        case ControlSourceVM.EXTERNAL_EXTENDED: TBufControl = iMS.SignalPath.ToneBufferControl.EXTERNAL_EXTENDED; break;
                        case ControlSourceVM.HOST: TBufControl = iMS.SignalPath.ToneBufferControl.HOST; break;
                    }
//                    OnPropertyChanged("ControlSource");
                }
            }
        }
        #endregion 

        private bool _amplComp;
        public bool AmplCompEnabled
        {
            get { return _amplComp; }
            set
            {
                if (_amplComp != value)
                {
                    _amplComp = value;
                    OnPropertyChanged("AmplCompEnabled");
                }
            }
        }

        private bool _phaseComp;
        public bool PhaseCompEnabled
        {
            get { return _phaseComp; }
            set
            {
                if (_phaseComp != value)
                {
                    _phaseComp = value;
                    OnPropertyChanged("PhaseCompEnabled");
                }
            }
        }

    }
}
