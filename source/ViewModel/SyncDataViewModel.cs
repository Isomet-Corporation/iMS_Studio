using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMS;
using System.Windows.Input;
using iMS_Studio.Model;
using ImsHwServer;

namespace iMS_Studio.ViewModel
{
    public class SyncDataViewModel : DockPaneViewModel
    {
        private SignalPathModel theModel;

        public SyncDataViewModel(SignalPathModel model) : base()
        {
            theModel = model;
            _pulseBitEnables = new bool[12];

            this.Reset();

            SyncInvert = true;
            PulseLengthEnable = PulseLengthEnable;  // refresh

            ContentId = "SyncData";
        }

        #region SyncDelay
        private double _syncDelay;
        public double SyncDelay
        {
            get { return _syncDelay; }
            set
            {
                if (value != _syncDelay)
                {
                    _syncDelay = value;
                    theModel.SyncDelay = value;
                    OnPropertyChanged("SyncDelay");
                }
            }
        }
        #endregion

        #region PulseLength
        private double _pulseLength;
        public double PulseLength
        {
            get { return _pulseLength; }
            set
            {
                if (value != _pulseLength)
                {
                    _pulseLength = value;
                    theModel.PulseLength = value;
                    OnPropertyChanged("PulseLength");
                }
            }
        }
        #endregion

        #region PulseEnabled
        private bool _pulseEnable;
        public bool PulseEnable
        {
            get { return _pulseEnable; }
            set
            {
                if (value != _pulseEnable)
                {
                    _pulseEnable = value;
                    theModel.PulseEnAllBits = value;
                    OnPropertyChanged("PulseEnable");
                    PulseLengthEnable = PulseLengthEnable;
                }
            }
        }

        private bool[] _pulseBitEnables;
        public bool Pulse0Enable
        {
            get { return _pulseBitEnables[0]; }
            set
            {
                if (value != _pulseBitEnables[0])
                {
                    _pulseBitEnables[0] = value;
                    theModel.PulseEnBit(0, value);
                    OnPropertyChanged("Pulse0Enable");
                    PulseLengthEnable = PulseLengthEnable;
                }
            }
        }

        public bool Pulse1Enable
        {
            get { return _pulseBitEnables[1]; }
            set
            {
                if (value != _pulseBitEnables[1])
                {
                    _pulseBitEnables[1] = value;
                    theModel.PulseEnBit(1, value);
                    OnPropertyChanged("Pulse1Enable");
                    PulseLengthEnable = PulseLengthEnable;
                }
            }
        }

        public bool Pulse2Enable
        {
            get { return _pulseBitEnables[2]; }
            set
            {
                if (value != _pulseBitEnables[2])
                {
                    _pulseBitEnables[2] = value;
                    theModel.PulseEnBit(2, value);
                    OnPropertyChanged("Pulse2Enable");
                    PulseLengthEnable = PulseLengthEnable;
                }
            }
        }

        public bool Pulse3Enable
        {
            get { return _pulseBitEnables[3]; }
            set
            {
                if (value != _pulseBitEnables[3])
                {
                    _pulseBitEnables[3] = value;
                    theModel.PulseEnBit(3, value);
                    OnPropertyChanged("Pulse3Enable");
                    PulseLengthEnable = PulseLengthEnable;
                }
            }
        }

        public bool Pulse4Enable
        {
            get { return _pulseBitEnables[4]; }
            set
            {
                if (value != _pulseBitEnables[4])
                {
                    _pulseBitEnables[4] = value;
                    theModel.PulseEnBit(4, value);
                    OnPropertyChanged("Pulse4Enable");
                    PulseLengthEnable = PulseLengthEnable;
                }
            }
        }

        public bool Pulse5Enable
        {
            get { return _pulseBitEnables[5]; }
            set
            {
                if (value != _pulseBitEnables[5])
                {
                    _pulseBitEnables[5] = value;
                    theModel.PulseEnBit(5, value);
                    OnPropertyChanged("Pulse5Enable");
                    PulseLengthEnable = PulseLengthEnable;
                }
            }
        }

        public bool Pulse6Enable
        {
            get { return _pulseBitEnables[6]; }
            set
            {
                if (value != _pulseBitEnables[6])
                {
                    _pulseBitEnables[6] = value;
                    theModel.PulseEnBit(6, value);
                    OnPropertyChanged("Pulse6Enable");
                    PulseLengthEnable = PulseLengthEnable;
                }
            }
        }

        public bool Pulse7Enable
        {
            get { return _pulseBitEnables[7]; }
            set
            {
                if (value != _pulseBitEnables[7])
                {
                    _pulseBitEnables[7] = value;
                    theModel.PulseEnBit(7, value);
                    OnPropertyChanged("Pulse7Enable");
                    PulseLengthEnable = PulseLengthEnable;
                }
            }
        }

        public bool Pulse8Enable
        {
            get { return _pulseBitEnables[8]; }
            set
            {
                if (value != _pulseBitEnables[8])
                {
                    _pulseBitEnables[8] = value;
                    theModel.PulseEnBit(8, value);
                    OnPropertyChanged("Pulse8Enable");
                    PulseLengthEnable = PulseLengthEnable;
                }
            }
        }

        public bool Pulse9Enable
        {
            get { return _pulseBitEnables[9]; }
            set
            {
                if (value != _pulseBitEnables[9])
                {
                    _pulseBitEnables[9] = value;
                    theModel.PulseEnBit(9, value);
                    OnPropertyChanged("Pulse9Enable");
                    PulseLengthEnable = PulseLengthEnable;
                }
            }
        }

        public bool Pulse10Enable
        {
            get { return _pulseBitEnables[10]; }
            set
            {
                if (value != _pulseBitEnables[10])
                {
                    _pulseBitEnables[10] = value;
                    theModel.PulseEnBit(10, value);
                    OnPropertyChanged("Pulse10Enable");
                    PulseLengthEnable = PulseLengthEnable;
                }
            }
        }

        public bool Pulse11Enable
        {
            get { return _pulseBitEnables[11]; }
            set
            {
                if (value != _pulseBitEnables[11])
                {
                    _pulseBitEnables[11] = value;
                    theModel.PulseEnBit(11, value);
                    OnPropertyChanged("Pulse11Enable");
                    PulseLengthEnable = PulseLengthEnable;
                }
            }
        }

        #endregion

        private bool _syncInvert;
        public bool SyncInvert
        {
            get { return _syncInvert; }
            set
            {
                if (value != _syncInvert)
                {
                    _syncInvert = value;
                    theModel.SyncInvert = value;
                    OnPropertyChanged("SyncInvert");
                }
            }
        }


        private SyncMapping.Types.SyncSource _AnlgASyncSrc;
        public SyncMapping.Types.SyncSource AnlgASyncSrc
        {
            get { return _AnlgASyncSrc; }
            set
            {
                if (value != _AnlgASyncSrc)
                {
                    _AnlgASyncSrc = value;
                    theModel.SyncAnlgA = value;
                    OnPropertyChanged("AnlgASyncSrc");
                }
            }
        }

        private SyncMapping.Types.SyncSource _AnlgBSyncSrc;
        public SyncMapping.Types.SyncSource AnlgBSyncSrc
        {
            get { return _AnlgBSyncSrc; }
            set
            {
                if (value != _AnlgBSyncSrc)
                {
                    _AnlgBSyncSrc = value;
                    theModel.SyncAnlgB = value;
                    OnPropertyChanged("AnlgBSyncSrc");
                }
            }
        }

        private SyncMapping.Types.SyncSource _DigSyncSrc;
        public SyncMapping.Types.SyncSource DigSyncSrc
        {
            get { return _DigSyncSrc; }
            set
            {
                if (value != _DigSyncSrc)
                {
                    _DigSyncSrc = value;
                    theModel.SyncDig = value;
                    OnPropertyChanged("DigSyncSrc");
                }
            }
        }

        public bool PulseLengthEnable
        {
            get { 
                bool en = false;
                foreach (var b in _pulseBitEnables)
                {
                    if (b == true) en = true;
                }
                if (_pulseEnable == true) en = true;
                return en;
            }
            set
            {
                if (value)
                {
                    theModel.PulseLength = PulseLength;
                }
                else
                {
                    theModel.PulseLength = 0;
                }

                OnPropertyChanged("PulseLengthEnable");
            }
        }

        public override void Reset()
        {
            SyncDelay = 0.0;
            PulseLength = 0.01;
            PulseEnable = false;

            AnlgASyncSrc = SyncMapping.Types.SyncSource.ImageAnalogA;
            AnlgBSyncSrc = SyncMapping.Types.SyncSource.ImageAnalogB;
            DigSyncSrc = SyncMapping.Types.SyncSource.ImageDigital;
        }
    }
}
