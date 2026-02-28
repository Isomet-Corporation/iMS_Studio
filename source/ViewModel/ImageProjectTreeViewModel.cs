using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using iMS;

namespace iMS_Studio.ViewModel
{
    public class ImageProjectTreeViewModel : DockPaneViewModel
    {
        public ICommand _move { get; private set; }
        public ICommand _copy { get; private set; }

        public ImageProjectTreeViewModel(ICommand moveCommand, ICommand copyCommand)
        {
            ImageProjectTable = new ObservableCollection<IImageGroup>();
            CompensationFunctionTable = new ObservableCollection<CompensationFunctionTreeSubItems>();
            ToneBufferTable = new ObservableCollection<ToneBufferTreeSubItems>();
            _move = moveCommand;
            _copy = copyCommand;
            PasteBufferOccupied = false;

            ContentId = "ProjectExplorer";
        }

        public ImageProjectItems GetSelectedImageProjectTableEntry()
        {
            foreach (IImageGroup item in imgTable)
            {
                if (item.GetType() == typeof(ImageProjectItems))
                {
                    var imgProjItem = item as ImageProjectItems;
                    if (item.IsSelected) return imgProjItem;
                    // Also return parent item if a child is selected
                    foreach (ImageProjectTreeSubItems subitem in imgProjItem.Items)
                    {
                        if (subitem.IsSelected) return imgProjItem;
                    }
                }
            }
            return null;
        }

        public ImageProjectTreeSubItems GetSelectedImageEntry()
        {
            foreach (var item in imgTable)
            {
                if (item.GetType() == typeof(ImageProjectItems))
                {
                    var imgProjItem = item as ImageProjectItems;
                    foreach (ImageProjectTreeSubItems subitem in imgProjItem.Items)
                    {
                        if (subitem.IsSelected) return subitem;
                    }
                }
                else
                {
                    var imgItem = item as ImageProjectTreeSubItems;
                    if (imgItem.IsSelected) return imgItem;
                }
            }
            return null;
        }

        public CompensationFunctionTreeSubItems GetSelectedCompensationFunctionEntry()
        {
            foreach (var item in compFunction)
            {
                if (item.IsSelected) return item;
            }
            return null;
        }

        public ToneBufferTreeSubItems GetSelectedToneBufferTableEntry()
        {
            foreach (var item in tbufTable)
            {
                if (item.IsSelected) return item;
            }
            return null;
        }

        private ObservableCollection<IImageGroup> imgTable;
        public ObservableCollection<IImageGroup> ImageProjectTable
        {
            get { return imgTable; }
            private set
            {
                imgTable = value;
            }
        }

        private ObservableCollection<CompensationFunctionTreeSubItems> compFunction;
        public ObservableCollection<CompensationFunctionTreeSubItems> CompensationFunctionTable
        {
            get { return compFunction; }
            private set
            {
                compFunction = value;
            }
        }

        private ObservableCollection<ToneBufferTreeSubItems> tbufTable;
        public ObservableCollection<ToneBufferTreeSubItems> ToneBufferTable
        {
            get { return tbufTable; }
            private set
            {
                tbufTable = value;
            }
        }

        public override void Reset()
        {
            imgTable.Clear();
            compFunction.Clear();
            tbufTable.Clear();
            PasteBufferOccupied = false;
        }

        #region IsSelected
        private bool _PasteBufferOccupied;
        public bool PasteBufferOccupied
        {
            get { return _PasteBufferOccupied; }
            set
            {
                if (_PasteBufferOccupied != value)
                {
                    _PasteBufferOccupied = value;
                    OnPropertyChanged(nameof(PasteBufferOccupied));
                }
            }
        }
        #endregion

    }

    public abstract class IImageGroup : BaseViewModel
    {
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

        #region IsEditing
        private bool _IsEditing;
        public virtual bool IsEditing
        {
            get { return _IsEditing; }
            set
            {
                if (_IsEditing != value)
                {
                    _IsEditing = value;
                    OnPropertyChanged(nameof(IsEditing));
                }
            }
        }
        #endregion

        #region IsMarkedForCut
        private bool _IsMarkedForCut;
        public virtual bool IsMarkedForCut
        {
            get { return _IsMarkedForCut; }
            set
            {
                if (_IsMarkedForCut != value)
                {
                    _IsMarkedForCut = value;
                    OnPropertyChanged(nameof(IsMarkedForCut));
                }
            }
        }
        #endregion

        public ObservableCollection<ImageProjectTreeSubItems> Items { get; protected set; }
    }

    public class ImageProjectItems : IImageGroup
    {
        public ImageProjectItems(ImageGroup imgGroup)
        {
            this.ImageGroupRef = imgGroup;
            this.Items = new ObservableCollection<ImageProjectTreeSubItems>();
            this.IsSelected = false;
            this.IsEditing = true;
            this.IsMarkedForCut = false;
        }

        #region ProjectReference
        private ImageGroup _imgGroup;
        public ImageGroup ImageGroupRef
        {
            get { return _imgGroup; }
            set {
                if (_imgGroup != value)
                {
                    _imgGroup = value;
                    OnPropertyChanged("ImageGroupRef");
                }
            }
        }
        #endregion

        #region  Name
        public string Name
        {
            get { return _imgGroup.Name; }
            set
            {
                if (_imgGroup.Name != value)
                {
                    _imgGroup.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        #endregion
    }

    public class ImageProjectTreeSubItems : IImageGroup
    {
        #region IsClosed
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set
            {
                if (_IsClosed != value)
                {
                    _IsClosed = value;
                    OnPropertyChanged(nameof(IsClosed));
                }
            }
        }
        #endregion

        #region DocumentReference
        private DockWindowViewModel _docRef;
        public DockWindowViewModel DocReference
        {
            get { return _docRef; }
            private set { _docRef = value; }
        }
        #endregion

        public ImageProjectTreeSubItems(DockWindowViewModel docRef)
        {
            DocReference = docRef;
            this.IsClosed = false;
            this.IsSelected = false;
            this.IsEditing = true;
            this.IsMarkedForCut = false;
        }

    }

    public class ImageTreeSubItems : ImageProjectTreeSubItems
    {
        public ImageTreeSubItems(iMSImage imgRef, DockWindowViewModel docRef) : base(docRef)
        {
            ImageRef = imgRef;
            _entries = ImageRef.Count;
        }

        #region ProjectReference
        private iMSImage _imgRef;
        public iMSImage ImageRef
        {
            get { return _imgRef; }
            set {
                if (_imgRef != value)
                {
                    _imgRef = value;
                    OnPropertyChanged("ImageRef");
                }
            }
        }
        #endregion

        #region  Name
        public string Name
        {
            get { return ImageRef.Name; }
            set
            {
                if (ImageRef.Name != value)
                {
                    ImageRef.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        #endregion

        #region  Entries
        private int _entries;
        public int Entries
        {
            get { return _entries; }
            set
            {
                if (_entries != value)
                {
                    _entries = value;
                    OnPropertyChanged("Entries");
                }
            }
        }
        #endregion
    }

    public class ImageSeqTreeSubItems : ImageProjectTreeSubItems
    {
        public ImageSeqTreeSubItems(ImageSequence seqRef, DockWindowViewModel docRef) : base(docRef)
        {
            ImageSequenceRef = seqRef;
        }

        #region ProjectReference
        private ImageSequence _seqRef;
        public ImageSequence ImageSequenceRef
        {
            get { return _seqRef; }
            set {
                if (_seqRef != value)
                {
                    _seqRef = value;
                    OnPropertyChanged("ImageSequenceRef");
                }
            }
        }
        #endregion

        #region  Name
        private string _nullName;
        public string Name
        {
            get { return "[--sequence--]"; }
            set { _nullName = value; }
        }
        #endregion

        public override bool IsEditing
        {
            get { return false; }
        }

        #region  Entries
        public int Entries
        {
            get { return ImageSequenceRef.Count; }
        }
        #endregion
    }

    public class CompensationFunctionTreeSubItems : ImageProjectTreeSubItems
    {
        public CompensationFunctionTreeSubItems(CompensationFunction compRef, DockWindowViewModel docRef) : base(docRef)
        {
            CompensationFunctionRef = compRef;
            _entries = CompensationFunctionRef.Count;
        }

        #region ProjectReference
        private CompensationFunction _compRef;
        public CompensationFunction CompensationFunctionRef
        {
            get { return _compRef; }
            set {
                if (_compRef != value)
                {
                    _compRef = value;
                    OnPropertyChanged("CompensationFunctionRef");
                }
            }
        }
        #endregion

        #region  Name
        public string Name
        {
            get { return CompensationFunctionRef.Name; }
            set
            {
                if (CompensationFunctionRef.Name != value)
                {
                    CompensationFunctionRef.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        #endregion

        #region  Entries
        private int _entries;
        public int Entries
        {
            get { return _entries; }
            set
            {
                if (_entries != value)
                {
                    _entries = value;
                    OnPropertyChanged("Entries");
                }
            }
        }
        #endregion
    }

    public class ToneBufferTreeSubItems : ImageProjectTreeSubItems
    {
        public ToneBufferTreeSubItems(ToneBuffer tbufRef, DockWindowViewModel docRef) : base(docRef)
        {
            ToneBufferRef = tbufRef;
        }

        #region ProjectReference
        private ToneBuffer _tbufRef;
        public ToneBuffer ToneBufferRef
        {
            get { return _tbufRef; }
            set {
                if (_tbufRef != value)
                {
                    _tbufRef = value;
                    OnPropertyChanged("ToneBufferRef");
                }
            }
        }
        #endregion

        #region  Name
        public string Name
        {
            get { return ToneBufferRef.Name; }
            set
            {
                if (ToneBufferRef.Name != value)
                {
                    ToneBufferRef.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        #endregion

        #region  Entries
        public int Entries
        {
            get { return ToneBufferRef.Count; }
        }
        #endregion
    }

}