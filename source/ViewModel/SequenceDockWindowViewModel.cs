using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using iMS;

namespace iMS_Studio.ViewModel
{
    class SequenceDockWindowViewModel : DockWindowViewModel
    {
        private const int maxSeqSize = 10000;

        private ImageSequence _seq;
        public ImageSequence Sequence
        {
            get { return _seq; }
            set
            {
                if (_seq != value)
                {
                    _seq = value;
                    OnPropertyChanged("Sequence");
                }
            }
        }

        public SequenceDockWindowViewModel(ImageGroup file) : base()
        {
            ObservableCollectionEx<ImageSequenceEntryViewModel> _SequenceData = new ObservableCollectionEx<ImageSequenceEntryViewModel>();
            foreach(ImageSequenceEntry ise in file.Sequence)
            {
                var seq_vm = new ImageSequenceEntryViewModel(ise);
                seq_vm.UUID = ise.UUID;
                seq_vm.ImageName = "[Not Found]";
                foreach (var img in file)
                {
                    if (ise.UUID == img.GetUUID)
                    {
                        seq_vm.ImageName = img.Name;
                        break;
                    }
                }
                seq_vm.IntOsc = ise.IntOsc.Value / 1000.0;
                seq_vm.ExtDiv = ise.ExtDiv;
                seq_vm.RptType = ise.RptType;
                seq_vm.NumRpts = ise.NumRpts;
                seq_vm.PostImgDelay = ise.PostImgDelay.TotalMilliseconds;
                seq_vm.SyncOutDelay = ise.SyncOutDelay.TotalMilliseconds * 1000.0;

                TermAction = file.Sequence.TermAction;
                TermValue = file.Sequence.TermValue;

                _SequenceData.Add(seq_vm);

            }
            SequenceData = new ObservableCollectionEx<ImageSequenceEntryViewModel>(_SequenceData);
            SequenceData.ItemPropertyChanged += (o, e) => this.IsDirty = true;

            /* Create a list of all the available Image Group names to use in drop down box */
            ImageNames = new ImageList(file);

            ContentId = file.GetUUID.ToString();

        }

        private ObservableCollectionEx<ImageSequenceEntryViewModel> _seqEntries;
        public ObservableCollectionEx<ImageSequenceEntryViewModel> SequenceData
        {
            get { return _seqEntries; }
            set
            {
                ApplyPropertyChange<SequenceDockWindowViewModel, ObservableCollectionEx<ImageSequenceEntryViewModel>>(ref _seqEntries, o => o.SequenceData, value);
//                _seqEntries = value;
                //OnPropertyChanged("SequenceData");
            }
        }

        private ImageList _imgNames;
        public ImageList ImageNames
        {
            get { return _imgNames; }
            set
            {
                ApplyPropertyChange<SequenceDockWindowViewModel, ImageList>(ref _imgNames, o => o.ImageNames, value);
//                _imgNames = value;
//                OnPropertyChanged("ImageNames");
            }
        }

        private SequenceTermAction _termAction;
        public SequenceTermAction TermAction
        {
            get { return _termAction; }
            set
            {
                ApplyPropertyChange<SequenceDockWindowViewModel, SequenceTermAction>(ref _termAction, o => o.TermAction, value);
//                _termAction = value;
//                OnPropertyChanged("TermAction");
            }
        }

        private int _termValue;
        public int TermValue
        {
            get { return _termValue; }
            set
            {
                ApplyPropertyChange<SequenceDockWindowViewModel, int>(ref _termValue, o => o.TermValue, value);
//                _termValue = value;
                //OnPropertyChanged("TermValue");
            }
        }

        public int SeqCount
        {
            get { return _seqEntries.Count; }
            set
            {
                int changeSize;
                if (value > maxSeqSize)
                {
                    changeSize = maxSeqSize - _seqEntries.Count;
                }
                else if (value < 0)
                {
                    changeSize = -_seqEntries.Count;
                }
                else
                {
                    changeSize = value - _seqEntries.Count;
                }

                if (changeSize == 0) return;
                else if (changeSize < 0)
                {
                    while (changeSize++ < 0)
                        _seqEntries.RemoveAt(_seqEntries.Count - 1);
                }
                else
                {
                    while (changeSize-- > 0)
                        _seqEntries.Add(new ImageSequenceEntryViewModel());
                }
                this.IsDirty = true;
                OnPropertyChanged("ImageSize");
            }
        }

    }

    public class ImageList : ObservableCollectionEx<SequenceFileName>
    {
        public ImageList(ImageGroup file) : base()
        {
            foreach (var f in file)
            {
                this.Add(new SequenceFileName(f.Name));
            }
        }
    }

    public class SequenceFileName : BaseViewModel
    {
        public SequenceFileName(string s) { FileName = s; }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged("FileName");
            }
        }
    }

}
