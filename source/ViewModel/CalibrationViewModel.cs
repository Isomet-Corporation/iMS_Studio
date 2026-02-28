using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using iMS_Studio.Model;

namespace iMS_Studio.ViewModel
{
    public class CalibrationViewModel : DockPaneViewModel
    {
        private CalibrationToneModel theModel;
        public ICommand UpdateTone { get; set; }

        public CalibrationViewModel(CalibrationToneModel model) : base()
        {
            theModel = model;
            
            STMFreq = 100.0;
            STMAmpl = 0.0;
            STMPhase = 0.0;
            STMUnlocked = true;
            STMEnable = false;

            ContentId = "CalibrationView";
        }

        private double _STMFreq;
        public double STMFreq
        {
            get { return _STMFreq; }
            set
            {
                if (value != _STMFreq)
                {
                    _STMFreq = value;
                    theModel.CalibrationFrequency = value;
                    OnPropertyChanged("STMFreq");
                }
            }
        }

        private double _STMAmpl;
        public double STMAmpl
        {
            get { return _STMAmpl; }
            set
            {
                if (value != _STMAmpl)
                {
                    _STMAmpl = value;
                    theModel.CalibrationAmplitude = value;
                    OnPropertyChanged("STMAmpl");
                }
            }
        }

        private double _STMPhase;
        public double STMPhase
        {
            get { return _STMPhase; }
            set
            {
                if (value != _STMPhase)
                {
                    _STMPhase = value;
                    theModel.CalibrationPhase = value;
                    OnPropertyChanged("STMPhase");
                }
            }
        }

        private bool _STMUnlocked;
        public bool STMUnlocked
        {
            get { return _STMUnlocked; }
            set
            {
                _STMUnlocked = value;
                OnPropertyChanged("STMUnlocked");
            }
        }

        private ICommand _STMHoldCh1Cmd;
        public bool STMHoldCh1
        {
            get { return theModel.LockChan1; }
            set
            {
                theModel.LockChan1 = value;
                OnPropertyChanged("STMHoldCh1");
                OnPropertyChanged("STMHoldCh2");  // Update both because they may be channel paired
            }
        }
        public ICommand STMHoldCh1Cmd
        {
            get
            {
                if (_STMHoldCh1Cmd == null)
                {
                    _STMHoldCh1Cmd = new RelayCommand(param => STMHoldCh1 = !STMHoldCh1, en => STMUnlocked);
                }
                return _STMHoldCh1Cmd;
            }
        }

        private ICommand _STMHoldCh2Cmd;
        public bool STMHoldCh2
        {
            get { return theModel.LockChan2; }
            set
            {
                theModel.LockChan2 = value;
                OnPropertyChanged("STMHoldCh2");
                OnPropertyChanged("STMHoldCh1");  // Update both because they may be channel paired
            }
        }
        public ICommand STMHoldCh2Cmd
        {
            get
            {
                if (_STMHoldCh2Cmd == null)
                {
                    _STMHoldCh2Cmd = new RelayCommand(param => STMHoldCh2 = !STMHoldCh2, en => STMUnlocked);
                }
                return _STMHoldCh2Cmd;
            }
        }

        private ICommand _STMHoldCh3Cmd;
        public bool STMHoldCh3
        {
            get { return theModel.LockChan3; }
            set
            {
                theModel.LockChan3 = value;
                OnPropertyChanged("STMHoldCh3");
                OnPropertyChanged("STMHoldCh4");  // Update both because they may be channel paired
            }
        }
        public ICommand STMHoldCh3Cmd
        {
            get
            {
                if (_STMHoldCh3Cmd == null)
                {
                    _STMHoldCh3Cmd = new RelayCommand(param => STMHoldCh3 = !STMHoldCh3, en => STMUnlocked);
                }
                return _STMHoldCh3Cmd;
            }
        }

        private ICommand _STMHoldCh4Cmd;
        public bool STMHoldCh4
        {
            get { return theModel.LockChan4; }
            set
            {
                theModel.LockChan4 = value;
                OnPropertyChanged("STMHoldCh4");
                OnPropertyChanged("STMHoldCh3");  // Update both because they may be channel paired
            }
        }
        public ICommand STMHoldCh4Cmd
        {
            get
            {
                if (_STMHoldCh4Cmd == null)
                {
                    _STMHoldCh4Cmd = new RelayCommand(param => STMHoldCh4 = !STMHoldCh4, en => STMUnlocked);
                }
                return _STMHoldCh4Cmd;
            }
        }

        private bool _STMEnable;
        public bool STMEnable
        {
            get { return _STMEnable; }
            set
            {
                _STMEnable = value;
                theModel.CalibrationEnable = value;
                if (UpdateTone != null) UpdateTone.Execute(value);
                OnPropertyChanged("STMEnable");
            }
        }

        private ICommand _STMEnableCmd;
        public ICommand STMEnableCmd
        {
            get
            {
                if (_STMEnableCmd == null)
                {
                    _STMEnableCmd = new RelayCommand(param => STMEnable = !STMEnable, en => STMUnlocked);
                }
                return _STMEnableCmd;
            }
        }

        public void SetFAP(iMS.FAP fap)
        {
            STMFreq = fap.freq.Value;
            STMAmpl = fap.ampl.Value;
            STMPhase = fap.phase.Value;
        }

        public double MinFrequency
        {
            get { return theModel.MinFrequency; }
        }

        public double MaxFrequency
        {
            get { return theModel.MaxFrequency; }
        }

        public override void Reset()
        {
            // Use Calibration Tone to set DDS output to zero
            this.STMEnable = true;
            this.STMEnable = false;
            this.STMAmpl = 0.0;
        }
    }
}
