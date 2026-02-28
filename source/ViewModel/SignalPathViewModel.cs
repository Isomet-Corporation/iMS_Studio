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
    public class SignalPathViewModel : DockPaneViewModel
    {
        private SignalPathModel theModel;

        public SignalPathViewModel(SignalPathModel model) : base()
        {
            theModel = model;

            this.Reset();

            XYPairing = false;
            DelayEnable = true;
            ContentId = "SignalPath";
        }

        public bool HasIndependentChannelControl
        {
            get { return theModel.HasIndependentChannels; }
        }

        public bool HasChannel2
        {
            get { return (/*theModel.HasIndependentChannels && */(theModel.Channels > 1)); }
        }

        public bool HasChannel3
        {
            get { return (theModel.HasIndependentChannels && (theModel.Channels > 2)); }
        }

        public bool HasChannel4
        {
            get { return (theModel.HasIndependentChannels && (theModel.Channels > 3)); }
        }

        private double _DDSPower;
        public double DDSPower
        {
            get { return _DDSPower; }
            set
            {
                if (value != _DDSPower)
                {
                    _DDSPower = value;
                    theModel.DDSPower = value;
                    OnPropertyChanged("DDSPower");
                }
            }
        }

        private double _Wiper1Power;
        public double Wiper1Power
        {
            get { return _Wiper1Power; }
            set
            {
                if (value != _Wiper1Power)
                {
                    _Wiper1Power = value;
                    theModel.Wiper1Power = value;

                    if (HasIndependentChannelControl && XYPairing)
                    {
                        _Wiper2Power = Wiper1Power;
                        theModel.Wiper2Power = Wiper1Power;
                        OnPropertyChanged("Wiper2Power");
                    }

                    OnPropertyChanged("Wiper1Power");
                }
            }
        }

        private double _Wiper2Power;
        public double Wiper2Power
        {
            get { return _Wiper2Power; }
            set
            {
                if (value != _Wiper2Power)
                {
                    _Wiper2Power = value;
                    theModel.Wiper2Power = value;

                    if (HasIndependentChannelControl && XYPairing)
                    {
                        _Wiper1Power = Wiper2Power;
                        theModel.Wiper1Power = Wiper2Power;
                        OnPropertyChanged("Wiper1Power");
                    }

                    OnPropertyChanged("Wiper2Power");
                }
            }
        }

        private double _Chan3Power;
        public double Chan3Power
        {
            get { return _Chan3Power; }
            set
            {
                if (value != _Chan3Power)
                {
                    _Chan3Power = value;
                    theModel.Chan3Power = value;

                    if (XYPairing)
                    {
                        _Chan4Power = Chan3Power;
                        theModel.Chan4Power = Chan3Power;
                        OnPropertyChanged("Chan4Power");
                    }

                    OnPropertyChanged("Chan3Power");
                }
            }
        }

        private double _Chan4Power;
        public double Chan4Power
        {
            get { return _Chan4Power; }
            set
            {
                if (value != _Chan4Power)
                {
                    _Chan4Power = value;
                    theModel.Chan4Power = value;

                    if (XYPairing)
                    {
                        _Chan3Power = Chan4Power;
                        theModel.Chan3Power = Chan4Power;
                        OnPropertyChanged("Chan3Power");
                    }

                    OnPropertyChanged("Chan4Power");
                }
            }
        }

        private bool _Chan1Src;
        public bool Chan1Src
        {
            get { return _Chan1Src || !HasIndependentChannelControl; }
            set
            {
                if (value != _Chan1Src)
                {
                    _Chan1Src = value;
                    theModel.Chan1Source = value;
                    OnPropertyChanged("Chan1Src");
                }
            }
        }

        private bool _Chan2Src;
        public bool Chan2Src
        {
            get { return _Chan2Src || !HasIndependentChannelControl; }
            set
            {
                if (value != _Chan2Src)
                {
                    _Chan2Src = value;
                    theModel.Chan2Source = value;
                    OnPropertyChanged("Chan2Src");
                }
            }
        }

        private bool _Chan3Src;
        public bool Chan3Src
        {
            get { return _Chan3Src; }
            set
            {
                if (value != _Chan3Src)
                {
                    _Chan3Src = value;
                    theModel.Chan3Source = value;
                    OnPropertyChanged("Chan3Src");
                }
            }
        }

        private bool _Chan4Src;
        public bool Chan4Src
        {
            get { return _Chan4Src; }
            set
            {
                if (value != _Chan4Src)
                {
                    _Chan4Src = value;
                    theModel.Chan4Source = value;
                    OnPropertyChanged("Chan4Src");
                }
            }
        }

        private bool _XYPairing;
        public bool XYPairing
        {
            get { return _XYPairing; }
            set
            {
                if (value != _XYPairing)
                {
                    _XYPairing = value;
                    OnPropertyChanged("XYPairing");
                }
            }
        }

        #region ChanDelay
        private double _chan12Delay;
        public double Chan12Delay
        {
            get { return _chan12Delay; }
            set
            {
                if (value != _chan12Delay)
                {
                    _chan12Delay = value;
                    theModel.Chan12Delay = value;
                    OnPropertyChanged("Chan12Delay");
                }
            }
        }

        private double _chan34Delay;
        public double Chan34Delay
        {
            get { return _chan34Delay; }
            set
            {
                if (value != _chan34Delay)
                {
                    _chan34Delay = value;
                    theModel.Chan34Delay = value;
                    OnPropertyChanged("Chan34Delay");
                }
            }
        }
        #endregion


        private PowerSettings.Types.AmplitudeControl _AmplControl;
        public PowerSettings.Types.AmplitudeControl AmplControl
        {
            get { return _AmplControl; }
            set
            {
                if (value != _AmplControl)
                {
                    _AmplControl = value;
                    theModel.AmplControl = value;
                    OnPropertyChanged("AmplControl");
                }
            }
        }

        private bool _AmpEnable;
        public bool AmpEnable
        {
            get { return _AmpEnable; }
            set
            {
                _AmpEnable = value;
                theModel.AmplifierEnable = value;
                OnPropertyChanged("AmpEnable");
            }
        }

        private ICommand _ampEnableCmd;
        public ICommand AmpEnableCmd
        {
            get {
                if (_ampEnableCmd == null)
                {
                    _ampEnableCmd = new RelayCommand(param => AmpEnable = !AmpEnable);
                }
                return _ampEnableCmd; }
        }

        private bool _RF12Enable;
        public bool RF12Enable
        {
            get { return _RF12Enable; }
            set
            {
                _RF12Enable = value;
                theModel.RF12Enable = value;
                OnPropertyChanged("RF12Enable");
            }
        }

        private ICommand _RF12EnableCmd;
        public ICommand RF12EnableCmd
        {
            get
            {
                if (_RF12EnableCmd == null)
                {
                    _RF12EnableCmd = new RelayCommand(param => RF12Enable = !RF12Enable);
                }
                return _RF12EnableCmd;
            }
        }

        private bool _RF34Enable;
        public bool RF34Enable
        {
            get { return _RF34Enable; }
            set
            {
                _RF34Enable = value;
                theModel.RF34Enable = value;
                OnPropertyChanged("RF34Enable");
            }
        }

        private ICommand _RF34EnableCmd;
        public ICommand RF34EnableCmd
        {
            get
            {
                if (_RF34EnableCmd == null)
                {
                    _RF34EnableCmd = new RelayCommand(param => RF34Enable = !RF34Enable);
                }
                return _RF34EnableCmd;
            }
        }

        private ICommand _chan1SrcCmd;
        public ICommand Chan1SrcCmd
        {
            get
            {
                if (_chan1SrcCmd == null)
                {
                    _chan1SrcCmd = new RelayCommand(param => Chan1Src = !Chan1Src);
                }
                return _chan1SrcCmd;
            }
        }

        private ICommand _chan2SrcCmd;
        public ICommand Chan2SrcCmd
        {
            get
            {
                if (_chan2SrcCmd == null)
                {
                    _chan2SrcCmd = new RelayCommand(param => Chan2Src = !Chan2Src);
                }
                return _chan2SrcCmd;
            }
        }

        private ICommand _chan3SrcCmd;
        public ICommand Chan3SrcCmd
        {
            get
            {
                if (_chan3SrcCmd == null)
                {
                    _chan3SrcCmd = new RelayCommand(param => Chan3Src = !Chan3Src);
                }
                return _chan3SrcCmd;
            }
        }

        private ICommand _chan4SrcCmd;
        public ICommand Chan4SrcCmd
        {
            get
            {
                if (_chan4SrcCmd == null)
                {
                    _chan4SrcCmd = new RelayCommand(param => Chan4Src = !Chan4Src);
                }
                return _chan4SrcCmd;
            }
        }

        private bool _AutoPhaseClear;
        public bool AutoPhaseClear
        {
            get { return _AutoPhaseClear; }
            set
            {
                _AutoPhaseClear = value;
                theModel.AutoPhaseClear = value;
                OnPropertyChanged("AutoPhaseClear");
            }
        }

        private ICommand _autoPhaseClearCmd;
        public ICommand AutoPhaseClearCmd
        {
            get
            {
                if (_autoPhaseClearCmd == null)
                {
                    _autoPhaseClearCmd = new RelayCommand(param => AutoPhaseClear = !AutoPhaseClear);
                }
                return _autoPhaseClearCmd;
            }
        }

        private ICommand _phaseResyncCmd;
        public ICommand PhaseResync
        {
            get
            {
                if (_phaseResyncCmd == null)
                {
                    _phaseResyncCmd = new RelayCommand(param => PhaseResync_Execute(), valid => true);
                }
                return _phaseResyncCmd;
            }
        }

        void PhaseResync_Execute()
        {
            theModel.PhaseResync();
        }

        private bool _delayEnable;
        public bool DelayEnable
        {
            get { return _delayEnable; }
            set
            {
                _delayEnable = value;
                OnPropertyChanged("DelayEnable");
            }
        }

        public override void Reset()
        {
            AmpEnable = false;
            RF12Enable = false;
            RF34Enable = false;
            DDSPower = 25.0;
            AmplControl = PowerSettings.Types.AmplitudeControl.Wiper1;
            Chan1Src = Chan2Src = Chan3Src = Chan4Src = true;
            Wiper1Power = Wiper2Power = Chan3Power = Chan4Power = 50.0;
        }
    }
}
