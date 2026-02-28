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
    public class SysFuncViewModel : DockPaneViewModel
    {
        private SystemFuncModel theModel;

        public SysFuncViewModel(SystemFuncModel model) : base()
        {
            theModel = model;

            this.Reset();

            ContentId = "SysFunc";
        }

        private double _ClockFreq;
        public double ClockFreq
        {
            get { return _ClockFreq; }
            set {
                if (value != _ClockFreq)
                {
                    _ClockFreq = value;
                    theModel.ClockFreq = value;
                    OnPropertyChanged("ClockFreq");
                }
            }
        }

        private double _DutyCycle;
        public double DutyCycle
        {
            get { return _DutyCycle; }
            set
            {
                if (value != _DutyCycle)
                {
                    _DutyCycle = value;
                    theModel.DutyCycle = value;
                    OnPropertyChanged("DutyCycle");
                }
            }
        }

        private double _OscPhase;
        public double OscPhase
        {
            get { return _OscPhase; }
            set
            {
                if (value != _OscPhase)
                {
                    _OscPhase = value;
                    theModel.OscPhase = value;
                    OnPropertyChanged("OscPhase");
                }
            }
        }

        private bool _AlwaysOn;
        public bool AlwaysOn
        {
            get { return _AlwaysOn; }
            set
            {
                if (value != _AlwaysOn)
                {
                    _AlwaysOn = value; theModel.AlwaysOn = value;
                    OnPropertyChanged("AlwaysOn");
                }
            }
        }

        private bool _GenTrigger;
        public bool GenerateTrigger
        {
            get { return _GenTrigger; }
            set
            {
                if (value != _GenTrigger)
                {
                    _GenTrigger = value; theModel.GenerateTrigger = value;
                    OnPropertyChanged("GenerateTrigger");
                }
            }
        }

        private bool _ClockPolarity;
        public bool ClockPolarity
        {
            get { return _ClockPolarity; }
            set
            {
                if (value != _ClockPolarity)
                {
                    _ClockPolarity = value; theModel.ClockPolarity = value;
                    OnPropertyChanged("ClockPolarity");
                }
            }
        }

        private bool _TrigPolarity;
        public bool TrigPolarity
        {
            get { return _TrigPolarity; }
            set
            {
                if (value != _TrigPolarity)
                {
                    _TrigPolarity = value; theModel.TrigPolarity = value;
                    OnPropertyChanged("TrigPolarity");
                }
            }
        }

        private bool _ClockGenEnable;
        public bool ClockGenEnable
        {
            get { return _ClockGenEnable; }
            set
            {
                _ClockGenEnable = value;
                theModel.ClockGenEnabled = value;
                OnPropertyChanged("ClockGenEnable");
            }
        }

        private ICommand _clockGenEnableCmd;
        public ICommand ClockGenEnableCmd
        {
            get {
                if (_clockGenEnableCmd == null)
                {
                    _clockGenEnableCmd = new RelayCommand(param => ClockGenEnable = !ClockGenEnable);
                }
                return _clockGenEnableCmd; }
        }
        public double MinImageRate
        {
            get { return theModel.MinImageRate; }
        }

        public double MaxImageRate
        {
            get { return theModel.MaxImageRate; }
        }

        public override void Reset()
        {
            ClockGenEnable = false;
            AlwaysOn = true;
            GenerateTrigger = false;
            ClockPolarity = false;
            TrigPolarity = false;
            ClockFreq = 100.0;
            OscPhase = 0.0;
            DutyCycle = 50.0;
        }
    }
}
