using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using iMS;
using System.Collections.Specialized;
using System.Windows.Input;
using Xceed.Wpf.DataGrid;

namespace iMS_Studio.ViewModel
{
    public class ImageDockWindowViewModel : DockWindowViewModel
    {
        private const int maxImageSize = 1000000;
        private bool _IgnoreChanges = false;

        private iMSImage _img = null;
        public iMSImage ImageRef
        {
            get { return _img; }
            set
            {
                _img = value;
                var imgPoints = new ObservableCollectionEx<ImagePointVM>();
                foreach (var pt in _img)
                    imgPoints.Add(new ImagePointVM(pt));
                imgPoints.ItemPropertyChanged += (o, e) => this.IsDirty = true;
                ImageData = imgPoints;
            }
        }

        public ImageDockWindowViewModel(iMSImage img) : base()
        {
            _img = img;

            _imgPoints = new ObservableCollectionEx<ImagePointVM>();
            foreach (var pt in img)
                _imgPoints.Add(new ImagePointVM(pt));

            _imgPoints.CollectionChanged +=
                new NotifyCollectionChangedEventHandler(ImageData_CollectionChanged);

            ContentId = _img.GetUUID.ToString();

            _imgPoints.ItemPropertyChanged += (o, e) => this.IsDirty = true;
        }

        // Respond to changes in the collection, adding and removing items from the model list in the image
        private void ImageData_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_IgnoreChanges)
                return;

            _IgnoreChanges = true;

            // If a reset, then e.OldItems is empty. Just clear and reload.
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                iMSImage new_img = new iMSImage();
                foreach (var pt in _imgPoints)
                    new_img.Add(pt.GetUnderlyingObject());
                _img.Clear();
                foreach (var pt in new_img)
                    _img.Add(pt);

                var _new_img_points = new ObservableCollectionEx<ImagePointVM>();
                foreach (var pt in _img)
                    _new_img_points.Add(new ImagePointVM(pt));
                _imgPoints = _new_img_points;
            }
            else
            {
                // Remove items from collection.
                /*var toRemove = new List<ImagePoint>();

                if (null != e.OldItems && e.OldItems.Count > 0)
                    foreach (var item in e.OldItems)
                        foreach (var existingItem in _img)
                            if (((ImagePointVM)item).IsViewFor(existingItem))
                                toRemove.Add(existingItem);

                foreach (var item in toRemove)
                    _img.Remove(item);

                // Add new items to the collection.
                if (null != e.NewItems && e.NewItems.Count > 0)
                    foreach (var item in e.NewItems)
                        _img.Add(((ImagePointVM)item).GetUnderlyingObject());*/
            }
            
            _IgnoreChanges = false;
        }

        private ObservableCollectionEx<ImagePointVM> _imgPoints;
        public ObservableCollectionEx<ImagePointVM> ImageData
        {
            get { return _imgPoints; }
            private set
            {
                if (value == _imgPoints)
                    return;

                _imgPoints.CollectionChanged -= ImageData_CollectionChanged;
                _imgPoints = value;
                _imgPoints.CollectionChanged += ImageData_CollectionChanged;

                // Underlying collection was rebuilt.
                this.ImageData_CollectionChanged(
                    this,
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Reset
                    )
                );

                this.IsDirty = true;
                OnPropertyChanged("ImageData");
            }
        }

        public int ImageSize
        {
            get { return _imgPoints.Count; }
            set
            {
                int changeSize;

                if (value > maxImageSize)
                {
                    changeSize = maxImageSize - _imgPoints.Count;
                }
                else if (value < 0)
                {
                    changeSize = -_imgPoints.Count;
                }
                else
                {
                    changeSize = value - _imgPoints.Count;
                }

                if (changeSize == 0) return;
                else if (changeSize < 0)
                {
                    while (changeSize++ < 0)
                    {
                        _img.RemoveAt(_img.Count - 1);
                        _imgPoints.RemoveAt(_imgPoints.Count - 1);
                    }
                }
                else
                {
                    while (changeSize-- > 0)
                    {
                        var pt = new ImagePoint();
                        _img.Add(pt);
                        pt = _img.Last();
                        _imgPoints.Add(new ImagePointVM(pt));
                    }
                }
                this.IsDirty = true;
                OnPropertyChanged("ImageSize");
            }
        }

        public double ImageClockRate
        {
            get {
                if (_img != null)
                    return (_img.ClockRate).Value / 1000.0;
                else
                    return 0.0;
            }
            set {
                if (_img != null)
                {
                    if ((_img.ClockRate).Value != (value * 1000.0))
                    {
                        (_img.ClockRate).Value = (value * 1000.0);
                        this.IsDirty = true;
                        OnPropertyChanged("ImageClockRate");
                    }
                }
            }
        }

        public int ExtClockDivide
        {
            get {
                if (_img != null)
                    return (_img.ExtClockDivide);
                else
                    return 1;
            }

            set
            {
                if (_img != null)
                {
                    if (_img.ExtClockDivide != value)
                    {
                        _img.ExtClockDivide = value;
                        this.IsDirty = true;
                        OnPropertyChanged("ExtClockDivide");
                    }
                }
            }
        }

        /* Table Manipulation Commands */
        #region InsertNewImageRowsCommand
        private RelayCommand< IList < SelectionCellRange > > _InsertNewImageRowsCommand;
        public ICommand InsertNewImageRowsCommand
        {
            get
            {
                if (_InsertNewImageRowsCommand == null)
                {
                    _InsertNewImageRowsCommand = new RelayCommand<IList<SelectionCellRange>>(param => InsertNewImageRowsCommand_Execute(param),
                        param => IfSelected_CanExecute(param));
                }
                return _InsertNewImageRowsCommand;
            }
        }

        void InsertNewImageRowsCommand_Execute(IList<SelectionCellRange> range)
        {
            if (range != null)
            {
                List<int> rowsSelected = new List<int>();
                foreach (var r in range)
                {
                    for (int i=r.ItemRange.StartIndex; i<=r.ItemRange.EndIndex; i++)
                    {
                        if (!rowsSelected.Exists(x => x == i)) rowsSelected.Add(i);
                    }
                }
                for (int i = 0; i < rowsSelected.Count; i++)
                {
                    var pt = new ImagePoint();
                    _img.Insert(rowsSelected.Min(), pt);
                    pt = _img[rowsSelected.Min()];
                    this.ImageData.Insert(rowsSelected.Min(), new ImagePointVM(pt));
                }
                this.IsDirty = true;
                //range.Clear();
                OnPropertyChanged("ImageSize");
            }
        }

        #endregion

        #region DeleteImageRowsCommand
        private RelayCommand<IList<SelectionCellRange>> _DeleteImageRowsCommand;
        public ICommand DeleteImageRowsCommand
        {
            get
            {
                if (_DeleteImageRowsCommand == null)
                {
                    _DeleteImageRowsCommand = new RelayCommand<IList<SelectionCellRange>>(param => DeleteImageRowsCommand_Execute(param),
                        param => IfSelected_CanExecute(param));
                }
                return _DeleteImageRowsCommand;
            }
        }

        void DeleteImageRowsCommand_Execute(IList<SelectionCellRange> range)
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
                    this._img.RemoveAt(rowsSelected[i]);
                    this._imgPoints.RemoveAt(rowsSelected[i]);
                }
                //range.Clear();
                this.IsDirty = true;
                OnPropertyChanged("ImageSize");
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
                using (new WaitCursor())
                {
                    ObservableCollectionEx<ImagePointVM> data = new ObservableCollectionEx<ImagePointVM>(ImageData);
                    List<List<int>> selectedCells = new List<List<int>>();
                    for (int i = 0; i < ImagePointVM.FieldCount; i++) selectedCells.Add(new List<int>());
                    foreach (var r in range)
                    {
                        for (int i = r.ColumnRange.StartIndex; i <= r.ColumnRange.EndIndex; i++)
                        {
                            if (i < ImagePointVM.FieldCount)
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
                    ImageData = data;
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
    }
}
