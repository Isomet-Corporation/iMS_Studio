using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using iMS;
using System.Collections.Specialized;
using Xceed.Wpf.DataGrid;
using System.Windows.Input;
using System.ComponentModel;

namespace iMS_Studio.ViewModel
{
    public enum InterpolationStyleVM
    {
        [Description("Cubic B-Spline")]
        BSPLINE,
        [Description("Linear")]
        LINEAR,
        [Description("Linear Extrapolated")]
        LINEXTEND,
        [Description("Spot Frequency")]
        SPOT,
        [Description("Stepped")]
        STEP,
    };

    public enum CompensationModifierVM
    {
        [Description("Replace Data")]
        REPLACE,
        [Description("Multiply By")]
        MULTIPLY,
    };

    class CompensationTableDockWindowViewModel : DockWindowViewModel
    {
        private const int maxCompSize = 10000;
        private bool _IgnoreChanges = false;

        private CompensationFunction _comp = null;
        public CompensationFunction CompRef
        {
            get { return _comp; }
            set
            {
                _comp = value;
                var compPoints = new ObservableCollectionEx<CompensationPointSpecificationViewModel>();
                foreach (var pt in _comp)
                    compPoints.Add(new CompensationPointSpecificationViewModel(pt));
                compPoints.ItemPropertyChanged += (o, e) => this.IsDirty = true;
                CompData = compPoints;

                switch(_comp.AmplitudeInterpolationStyle)
                {
                    case CompensationFunction.InterpolationStyle.BSPLINE: AmplStyle = InterpolationStyleVM.BSPLINE; break;
                    case CompensationFunction.InterpolationStyle.LINEAR: AmplStyle = InterpolationStyleVM.LINEAR; break;
                    case CompensationFunction.InterpolationStyle.LINEXTEND: AmplStyle = InterpolationStyleVM.LINEXTEND; break;
                    case CompensationFunction.InterpolationStyle.SPOT: AmplStyle = InterpolationStyleVM.SPOT; break;
                    case CompensationFunction.InterpolationStyle.STEP: AmplStyle = InterpolationStyleVM.STEP; break;
                }
                switch (_comp.PhaseInterpolationStyle)
                {
                    case CompensationFunction.InterpolationStyle.BSPLINE: PhaseStyle = InterpolationStyleVM.BSPLINE; break;
                    case CompensationFunction.InterpolationStyle.LINEAR: PhaseStyle = InterpolationStyleVM.LINEAR; break;
                    case CompensationFunction.InterpolationStyle.LINEXTEND: PhaseStyle = InterpolationStyleVM.LINEXTEND; break;
                    case CompensationFunction.InterpolationStyle.SPOT: PhaseStyle = InterpolationStyleVM.SPOT; break;
                    case CompensationFunction.InterpolationStyle.STEP: PhaseStyle = InterpolationStyleVM.STEP; break;
                }
                switch (_comp.SyncAnlgInterpolationStyle)
                {
                    case CompensationFunction.InterpolationStyle.BSPLINE: SyncAStyle = InterpolationStyleVM.BSPLINE; break;
                    case CompensationFunction.InterpolationStyle.LINEAR: SyncAStyle = InterpolationStyleVM.LINEAR; break;
                    case CompensationFunction.InterpolationStyle.LINEXTEND: SyncAStyle = InterpolationStyleVM.LINEXTEND; break;
                    case CompensationFunction.InterpolationStyle.SPOT: SyncAStyle = InterpolationStyleVM.SPOT; break;
                    case CompensationFunction.InterpolationStyle.STEP: SyncAStyle = InterpolationStyleVM.STEP; break;
                }
                switch (_comp.SyncDigInterpolationStyle)
                {
                    case CompensationFunction.InterpolationStyle.BSPLINE: SyncDStyle = InterpolationStyleVM.BSPLINE; break;
                    case CompensationFunction.InterpolationStyle.LINEAR: SyncDStyle = InterpolationStyleVM.LINEAR; break;
                    case CompensationFunction.InterpolationStyle.LINEXTEND: SyncDStyle = InterpolationStyleVM.LINEXTEND; break;
                    case CompensationFunction.InterpolationStyle.SPOT: SyncDStyle = InterpolationStyleVM.SPOT; break;
                    case CompensationFunction.InterpolationStyle.STEP: SyncDStyle = InterpolationStyleVM.STEP; break;
                }
            }
        }

        private InterpolationStyleVM _amplStyle;
        public InterpolationStyleVM AmplStyle
        {
            get { return _amplStyle; }
            set
            {
                if (value != _amplStyle)
                {
                    _amplStyle = value;
                    switch(_amplStyle)
                    {
                        case InterpolationStyleVM.BSPLINE: _comp.AmplitudeInterpolationStyle = CompensationFunction.InterpolationStyle.BSPLINE; break;
                        case InterpolationStyleVM.LINEAR: _comp.AmplitudeInterpolationStyle = CompensationFunction.InterpolationStyle.LINEAR; break;
                        case InterpolationStyleVM.LINEXTEND: _comp.AmplitudeInterpolationStyle = CompensationFunction.InterpolationStyle.LINEXTEND; break;
                        case InterpolationStyleVM.SPOT: _comp.AmplitudeInterpolationStyle = CompensationFunction.InterpolationStyle.SPOT; break;
                        case InterpolationStyleVM.STEP: _comp.AmplitudeInterpolationStyle = CompensationFunction.InterpolationStyle.STEP; break;
                    }

                    OnPropertyChanged("AmplStyle");
                }
            }
        }

        private InterpolationStyleVM _phsStyle;
        public InterpolationStyleVM PhaseStyle
        {
            get { return _phsStyle; }
            set
            {
                if (value != _phsStyle)
                {
                    _phsStyle = value;
                    switch (_phsStyle)
                    {
                        case InterpolationStyleVM.BSPLINE: _comp.PhaseInterpolationStyle = CompensationFunction.InterpolationStyle.BSPLINE; break;
                        case InterpolationStyleVM.LINEAR: _comp.PhaseInterpolationStyle = CompensationFunction.InterpolationStyle.LINEAR; break;
                        case InterpolationStyleVM.LINEXTEND: _comp.PhaseInterpolationStyle = CompensationFunction.InterpolationStyle.LINEXTEND; break;
                        case InterpolationStyleVM.SPOT: _comp.PhaseInterpolationStyle = CompensationFunction.InterpolationStyle.SPOT; break;
                        case InterpolationStyleVM.STEP: _comp.PhaseInterpolationStyle = CompensationFunction.InterpolationStyle.STEP; break;
                    }

                    OnPropertyChanged("PhaseStyle");
                }
            }
        }

        private InterpolationStyleVM _syncaStyle;
        public InterpolationStyleVM SyncAStyle
        {
            get { return _syncaStyle; }
            set
            {
                if (value != _syncaStyle)
                {
                    _syncaStyle = value;
                    switch (_syncaStyle)
                    {
                        case InterpolationStyleVM.BSPLINE: _comp.SyncAnlgInterpolationStyle = CompensationFunction.InterpolationStyle.BSPLINE; break;
                        case InterpolationStyleVM.LINEAR: _comp.SyncAnlgInterpolationStyle = CompensationFunction.InterpolationStyle.LINEAR; break;
                        case InterpolationStyleVM.LINEXTEND: _comp.SyncAnlgInterpolationStyle = CompensationFunction.InterpolationStyle.LINEXTEND; break;
                        case InterpolationStyleVM.SPOT: _comp.SyncAnlgInterpolationStyle = CompensationFunction.InterpolationStyle.SPOT; break;
                        case InterpolationStyleVM.STEP: _comp.SyncAnlgInterpolationStyle = CompensationFunction.InterpolationStyle.STEP; break;
                    }

                    OnPropertyChanged("SyncAStyle");
                }
            }
        }

        private InterpolationStyleVM _syncdStyle;
        public InterpolationStyleVM SyncDStyle
        {
            get { return _syncdStyle; }
            set
            {
                if (value != _syncdStyle)
                {
                    _syncdStyle = value;
                    switch (_syncdStyle)
                    {
                        case InterpolationStyleVM.BSPLINE: _comp.SyncDigInterpolationStyle = CompensationFunction.InterpolationStyle.BSPLINE; break;
                        case InterpolationStyleVM.LINEAR: _comp.SyncDigInterpolationStyle = CompensationFunction.InterpolationStyle.LINEAR; break;
                        case InterpolationStyleVM.LINEXTEND: _comp.SyncDigInterpolationStyle = CompensationFunction.InterpolationStyle.LINEXTEND; break;
                        case InterpolationStyleVM.SPOT: _comp.SyncDigInterpolationStyle = CompensationFunction.InterpolationStyle.SPOT; break;
                        case InterpolationStyleVM.STEP: _comp.SyncDigInterpolationStyle = CompensationFunction.InterpolationStyle.STEP; break;
                    }

                    OnPropertyChanged("SyncDStyle");
                }
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

        public ICommand _genAmpl { get; private set; }
        public ICommand _genPhs { get; private set; }
        public ICommand _genSyncD { get; private set; }
        public ICommand _genSyncA { get; private set; }

        public CompensationTableDockWindowViewModel(CompensationFunction comp, int channels, bool scope,
            ICommand GenAmpl, ICommand GenPhs, ICommand GenSyncD, ICommand GenSyncA) : base()
        {
            _comp = comp;
            GlobalScope = scope;
            RFChannels = channels;
            CurrentChannel = 1;

            switch (_comp.AmplitudeInterpolationStyle)
            {
                case CompensationFunction.InterpolationStyle.BSPLINE: AmplStyle = InterpolationStyleVM.BSPLINE; break;
                case CompensationFunction.InterpolationStyle.LINEAR: AmplStyle = InterpolationStyleVM.LINEAR; break;
                case CompensationFunction.InterpolationStyle.LINEXTEND: AmplStyle = InterpolationStyleVM.LINEXTEND; break;
                case CompensationFunction.InterpolationStyle.SPOT: AmplStyle = InterpolationStyleVM.SPOT; break;
                case CompensationFunction.InterpolationStyle.STEP: AmplStyle = InterpolationStyleVM.STEP; break;
            }
            switch (_comp.PhaseInterpolationStyle)
            {
                case CompensationFunction.InterpolationStyle.BSPLINE: PhaseStyle = InterpolationStyleVM.BSPLINE; break;
                case CompensationFunction.InterpolationStyle.LINEAR: PhaseStyle = InterpolationStyleVM.LINEAR; break;
                case CompensationFunction.InterpolationStyle.LINEXTEND: PhaseStyle = InterpolationStyleVM.LINEXTEND; break;
                case CompensationFunction.InterpolationStyle.SPOT: PhaseStyle = InterpolationStyleVM.SPOT; break;
                case CompensationFunction.InterpolationStyle.STEP: PhaseStyle = InterpolationStyleVM.STEP; break;
            }
            switch (_comp.SyncAnlgInterpolationStyle)
            {
                case CompensationFunction.InterpolationStyle.BSPLINE: SyncAStyle = InterpolationStyleVM.BSPLINE; break;
                case CompensationFunction.InterpolationStyle.LINEAR: SyncAStyle = InterpolationStyleVM.LINEAR; break;
                case CompensationFunction.InterpolationStyle.LINEXTEND: SyncAStyle = InterpolationStyleVM.LINEXTEND; break;
                case CompensationFunction.InterpolationStyle.SPOT: SyncAStyle = InterpolationStyleVM.SPOT; break;
                case CompensationFunction.InterpolationStyle.STEP: SyncAStyle = InterpolationStyleVM.STEP; break;
            }
            switch (_comp.SyncDigInterpolationStyle)
            {
                case CompensationFunction.InterpolationStyle.BSPLINE: SyncDStyle = InterpolationStyleVM.BSPLINE; break;
                case CompensationFunction.InterpolationStyle.LINEAR: SyncDStyle = InterpolationStyleVM.LINEAR; break;
                case CompensationFunction.InterpolationStyle.LINEXTEND: SyncDStyle = InterpolationStyleVM.LINEXTEND; break;
                case CompensationFunction.InterpolationStyle.SPOT: SyncDStyle = InterpolationStyleVM.SPOT; break;
                case CompensationFunction.InterpolationStyle.STEP: SyncDStyle = InterpolationStyleVM.STEP; break;
            }
            _rowSelected = 0;

            _compPoints = new ObservableCollectionEx<CompensationPointSpecificationViewModel>();
            foreach (var pt in comp)
                _compPoints.Add(new CompensationPointSpecificationViewModel(pt));

            _compPoints.CollectionChanged +=
                new NotifyCollectionChangedEventHandler(CompData_CollectionChanged);

            ContentId = _comp.GetUUID.ToString();

            _compPoints.ItemPropertyChanged += (o, e) => this.IsDirty = true;

            _genAmpl = GenAmpl;
            _genPhs = GenPhs;
            _genSyncD = GenSyncD;
            _genSyncA = GenSyncA;
        }

        private bool _globalScope;
        public bool GlobalScope
        {
            get { return _globalScope; }
            set
            {
                if (value != _globalScope)
                {
                    _globalScope = value;
                    OnPropertyChanged("GlobalScope");
                }
            }
        }

        private int _rfChannels;
        public int RFChannels
        {
            get { return _rfChannels; }
            set
            {
                if (value != _rfChannels)
                {
                    _rfChannels = value;
                    OnPropertyChanged("RFChannels");
                }
            }
        }

        private int _currentChan;
        public int CurrentChannel
        {
            get { return _currentChan; }
            set
            {
                _currentChan = value;
                OnPropertyChanged("CurrentChannel");
            }
        }

        private CompensationModifierVM _modifier;
        public CompensationModifierVM Modifier
        {
            get { return _modifier; }
            set
            {
                _modifier = value;
                OnPropertyChanged("Modifier");
            }
        }

        private void CompData_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_IgnoreChanges)
                return;

            _IgnoreChanges = true;

            // If a reset, then e.OldItems is empty. Just clear and reload.
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                CompensationFunction new_comp = new CompensationFunction();
                foreach (var pt in _compPoints)
                    new_comp.Add(pt.GetUnderlyingObject());
                _comp.Clear();
                foreach (var pt in new_comp)
                    _comp.Add(pt);

                var _new_comp_points = new ObservableCollectionEx<CompensationPointSpecificationViewModel>();
                foreach (var pt in _comp)
                    _new_comp_points.Add(new CompensationPointSpecificationViewModel(pt));
                _compPoints = _new_comp_points;
            }
            else
            {
                // Remove items from collection.
  /*              var toRemove = new List<CompensationPointSpecification>();

                if (null != e.OldItems && e.OldItems.Count > 0)
                    foreach (var item in e.OldItems)
                        foreach (var existingItem in _comp)
                            if (((CompensationPointSpecificationViewModel)item).IsViewFor(existingItem))
                                toRemove.Add(existingItem);

                foreach (var item in toRemove)
                    _comp.Remove(item);

                // Add new items to the collection.
                if (null != e.NewItems && e.NewItems.Count > 0)
                    foreach (var item in e.NewItems)
                        _comp.Add(((CompensationPointSpecificationViewModel)item).GetUnderlyingObject());*/
            }

            _IgnoreChanges = false;
        }

        private ObservableCollectionEx<CompensationPointSpecificationViewModel> _compPoints;
        public ObservableCollectionEx<CompensationPointSpecificationViewModel> CompData
        {
            get { return _compPoints; }
            set
            {
                if (value == _compPoints)
                    return;

                _compPoints.CollectionChanged -= CompData_CollectionChanged;
                _compPoints = value;
                _compPoints.CollectionChanged += CompData_CollectionChanged;

                // Underlying collection was rebuilt.
                this.CompData_CollectionChanged(
                    this,
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Reset
                    )
                );
                this.IsDirty = true;
                OnPropertyChanged("CompData");
            }
        }

        public int CompSize
        {
            get { return _compPoints.Count; }
            set
            {
                int changeSize;
                if (value > maxCompSize)
                {
                    changeSize = maxCompSize - _compPoints.Count;
                }
                else if (value < 0)
                {
                    changeSize = -_compPoints.Count;
                }
                else
                {
                    changeSize = value - _compPoints.Count;
                }

                if (changeSize == 0) return;
                else if (changeSize < 0)
                {
                    while (changeSize++ < 0)
                    {
                        _comp.RemoveAt(_comp.Count - 1);
                        _compPoints.RemoveAt(_compPoints.Count - 1);
                    }
                }
                else
                {
                    while (changeSize-- > 0)
                    {
                        var pt = new CompensationPointSpecification();
                        _comp.Add(pt);
                        pt = _comp.Last();
                        _compPoints.Add(new CompensationPointSpecificationViewModel(pt));
                    }
                }
                this.IsDirty = true;
                OnPropertyChanged("CompSize");
            }
        }

        /* Table Manipulation Commands */
        #region InsertNewRowsCommand
        private RelayCommand<IList<SelectionCellRange>> _InsertNewRowsCommand;
        public ICommand InsertNewRowsCommand
        {
            get
            {
                if (_InsertNewRowsCommand == null)
                {
                    _InsertNewRowsCommand = new RelayCommand<IList<SelectionCellRange>>(param => InsertNewRowsCommand_Execute(param),
                        param => IfSelected_CanExecute(param));
                }
                return _InsertNewRowsCommand;
            }
        }

        void InsertNewRowsCommand_Execute(IList<SelectionCellRange> range)
        {
            if (range != null)
            {
                List<int> rowsSelected = new List<int>();
                foreach (var r in range)
                {
                    for (int i = r.ItemRange.StartIndex; i <= r.ItemRange.EndIndex; i++)
                    {
                        if (!rowsSelected.Exists(x => x == i)) rowsSelected.Add(i);
                    }
                }
                for (int i = 0; i < rowsSelected.Count; i++)
                {
                    var pt = new CompensationPointSpecification();
                    _comp.Insert(rowsSelected.Min(), pt);
                    pt = _comp[rowsSelected.Min()];
                    this.CompData.Insert(rowsSelected.Min(), new CompensationPointSpecificationViewModel(pt));
                }
                //range.Clear();
                this.IsDirty = true;
                OnPropertyChanged("CompSize");
            }
        }

        #endregion

        #region DeleteRowsCommand
        private RelayCommand<IList<SelectionCellRange>> _DeleteRowsCommand;
        public ICommand DeleteRowsCommand
        {
            get
            {
                if (_DeleteRowsCommand == null)
                {
                    _DeleteRowsCommand = new RelayCommand<IList<SelectionCellRange>>(param => DeleteRowsCommand_Execute(param),
                        param => IfSelected_CanExecute(param));
                }
                return _DeleteRowsCommand;
            }
        }

        void DeleteRowsCommand_Execute(IList<SelectionCellRange> range)
        {
            if (range != null)
            {
                List<int> rowsSelected = new List<int>();
                foreach (var r in range)
                {
                    for (int i = r.ItemRange.StartIndex; i <= r.ItemRange.EndIndex; i++)
                    {
                        if (!rowsSelected.Exists(x => x == i)) rowsSelected.Add(i);
                    }
                }
                rowsSelected.Sort();
                for (int i = rowsSelected.Count - 1; i >= 0; i--)
                {
                    this._comp.RemoveAt(rowsSelected[i]);
                    this._compPoints.RemoveAt(rowsSelected[i]);
                }
                //range.Clear();
                this.IsDirty = true;
                OnPropertyChanged("CompSize");
            }
        }

        #endregion

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
                ObservableCollectionEx<CompensationPointSpecificationViewModel> data = new ObservableCollectionEx<CompensationPointSpecificationViewModel>(CompData);
                List<List<int>> selectedCells = new List<List<int>>();
                for (int i = 0; i < CompensationPointSpecificationViewModel.FieldCount; i++) selectedCells.Add(new List<int>());
                foreach (var r in range)
                {
                    for (int i = r.ColumnRange.StartIndex; i <= r.ColumnRange.EndIndex; i++)
                    {
                        if (i < CompensationPointSpecificationViewModel.FieldCount)
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
                CompData = data;
            }
        }

        #endregion

        private RelayCommand _GenerateAmplCommand;
        public ICommand GenerateAmpl
        {
            get
            {
                if (_GenerateAmplCommand == null)
                {
                    _GenerateAmplCommand = new RelayCommand(o => _genAmpl.Execute(this));
                }
                return _GenerateAmplCommand;
            }
        }

        private RelayCommand _GeneratePhsCommand;
        public ICommand GeneratePhase
        {
            get
            {
                if (_GeneratePhsCommand == null)
                {
                    _GeneratePhsCommand = new RelayCommand(o => _genPhs.Execute(this));
                }
                return _GeneratePhsCommand;
            }
        }

        private RelayCommand _GenerateSyncDigCommand;
        public ICommand GenerateSyncDig
        {
            get
            {
                if (_GenerateSyncDigCommand == null)
                {
                    _GenerateSyncDigCommand = new RelayCommand(o => _genSyncD.Execute(this));
                }
                return _GenerateSyncDigCommand;
            }
        }

        private RelayCommand _GenerateSyncACommand;
        public ICommand GenerateSyncAnlg
        {
            get
            {
                if (_GenerateSyncACommand == null)
                {
                    _GenerateSyncACommand = new RelayCommand(o => _genSyncA.Execute(this));
                }
                return _GenerateSyncACommand;
            }
        }

        private RelayCommand _GenerateAllCommand;
        public ICommand GenerateAll
        {
            get
            {
                if (_GenerateAllCommand == null)
                {
                    _GenerateAllCommand = new RelayCommand(o => GenerateAll_Execute());
                }
                return _GenerateAllCommand;
            }
        }

        void GenerateAll_Execute()
        {
            this._genSyncD.Execute(this);
            this._genSyncA.Execute(this);
            this._genPhs.Execute(this);
            this._genAmpl.Execute(this);
        }


        bool IfSelected_CanExecute(IList<SelectionCellRange> range)
        {
            if (range == null) return false;
            else if (range.Count == 0) return false;
            else return true;
        }

    }
}
