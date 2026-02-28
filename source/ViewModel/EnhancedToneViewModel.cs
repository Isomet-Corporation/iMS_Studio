using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using iMS_Studio.Model;
using ImsHwServer;
using System.ComponentModel;

namespace iMS_Studio.ViewModel
{
    public enum EnhancedToneVM
    {
        [Description("Frequency Sweep with Dwell")]
        FREQUENCY_DWELL,
        [Description("Frequency Sweep no Dwell")]
        FREQUENCY_NO_DWELL,
        [Description("Frequency Fast Modulation")]
        FREQUENCY_FAST_MOD,
        [Description("Phase Sweep with Dwell")]
        PHASE_DWELL,
        [Description("Phase Sweep no Dwell")]
        PHASE_NO_DWELL,
        [Description("Phase Fast Modulation")]
        PHASE_FAST_MOD,
        [Description("Multi-Channel Tone (No Sweep)")]
        NO_SWEEP
    };

    public enum DACCurrentReferenceVM
    {
        [Description("Full Scale")]
        FULL_SCALE,
        [Description("Half Scale")]
        HALF_SCALE,
        [Description("Quarter Scale")]
        QUARTER_SCALE,
        [Description("Eighth Scale")]
        EIGHTH_SCALE
    }

    public class EnhancedToneViewModel : DockPaneViewModel
    {
        private EnhancedToneModel theModel;
        public ICommand UpdateTone { get; set; }

        public EnhancedToneViewModel(EnhancedToneModel model) : base()
        {
            theModel = model;
            
            this.ManualTriggerSetting = false;
            this.Ch1StartFreq = 12.5;
            this.Ch1EndFreq = 100.0;
            this.Ch1StartPhase = 0.0;
            this.Ch1EndPhase = 0.0;
            this.Ch1Ampl = 100.0;
            this.Ch1UpRamp = 10;
            this.Ch1DownRamp = 20;
            this.Ch1NSteps = 50;
            this.Ch1Mode = EnhancedToneVM.FREQUENCY_NO_DWELL;
            this.Ch1DacRef = iMS.DAC_CURRENT_REFERENCE.FULL_SCALE;
            this.Ch1ProfileSetting = false;
            this.Ch2StartFreq = 12.5;
            this.Ch2EndFreq = 100.0;
            this.Ch2StartPhase = 0.0;
            this.Ch2EndPhase = 0.0;
            this.Ch2Ampl = 100.0;
            this.Ch2UpRamp = 10;
            this.Ch2DownRamp = 20;
            this.Ch2NSteps = 50;
            this.Ch2Mode = EnhancedToneVM.FREQUENCY_NO_DWELL;
            this.Ch2DacRef = iMS.DAC_CURRENT_REFERENCE.FULL_SCALE;
            this.Ch2ProfileSetting = false;
            this.Ch3StartFreq = 12.5;
            this.Ch3EndFreq = 100.0;
            this.Ch3StartPhase = 0.0;
            this.Ch3EndPhase = 0.0;
            this.Ch3Ampl = 100.0;
            this.Ch3UpRamp = 10;
            this.Ch3DownRamp = 20;
            this.Ch3NSteps = 50;
            this.Ch3Mode = EnhancedToneVM.FREQUENCY_NO_DWELL;
            this.Ch3DacRef = iMS.DAC_CURRENT_REFERENCE.FULL_SCALE;
            this.Ch3ProfileSetting = false;
            this.Ch4StartFreq = 12.5;
            this.Ch4EndFreq = 100.0;
            this.Ch4StartPhase = 0.0;
            this.Ch4EndPhase = 0.0;
            this.Ch4Ampl = 100.0;
            this.Ch4UpRamp = 10;
            this.Ch4DownRamp = 20;
            this.Ch4NSteps = 50;
            this.Ch4Mode = EnhancedToneVM.FREQUENCY_NO_DWELL;
            this.Ch4DacRef = iMS.DAC_CURRENT_REFERENCE.FULL_SCALE;
            this.Ch4ProfileSetting = false;
            ETMEnable = false;
            ETMUnlocked = true;

            ContentId = "EnhancedToneView";
        }

        private double _Ch1StartFreq;
        public double Ch1StartFreq
        {
            get { return _Ch1StartFreq; }
            set
            {
                if (value != _Ch1StartFreq)
                {
                    _Ch1StartFreq = value;
                    theModel.Ch1StartFreq = value;
                    OnPropertyChanged("Ch1StartFreq");
                }
            }
        }

        private double _Ch1EndFreq;
        public double Ch1EndFreq
        {
            get { return _Ch1EndFreq; }
            set
            {
                if (value != _Ch1EndFreq)
                {
                    _Ch1EndFreq = value;
                    theModel.Ch1EndFreq = value;
                    OnPropertyChanged("Ch1EndFreq");
                }
            }
        }

        private bool _Ch1EFreqEnabled;
        public bool Ch1EFreqEnabled
        {
            get { return _Ch1EFreqEnabled; }
            set
            {
                if (value != _Ch1EFreqEnabled)
                {
                    _Ch1EFreqEnabled = value;
                    OnPropertyChanged("Ch1EFreqEnabled");
                }
            }
        }

        private double _Ch1StartPhase;
        public double Ch1StartPhase
        {
            get { return _Ch1StartPhase; }
            set
            {
                if (value != _Ch1StartPhase)
                {
                    _Ch1StartPhase = value;
                    theModel.Ch1StartPhase = value;
                    OnPropertyChanged("Ch1StartPhase");
                }
            }
        }

        private double _Ch1EndPhase;
        public double Ch1EndPhase
        {
            get { return _Ch1EndPhase; }
            set
            {
                if (value != _Ch1EndPhase)
                {
                    _Ch1EndPhase = value;
                    theModel.Ch1EndPhase = value;
                    OnPropertyChanged("Ch1EndPhase");
                }
            }
        }

        private bool _Ch1EPhsEnabled;
        public bool Ch1EPhsEnabled
        {
            get { return _Ch1EPhsEnabled; }
            set
            {
                if (value != _Ch1EPhsEnabled)
                {
                    _Ch1EPhsEnabled = value;
                    OnPropertyChanged("Ch1EPhsEnabled");
                }
            }
        }

        private double _Ch1Ampl;
        public double Ch1Ampl
        {
            get { return _Ch1Ampl; }
            set
            {
                if (value != _Ch1Ampl)
                {
                    _Ch1Ampl = value;
                    theModel.Ch1Ampl = value;
                    OnPropertyChanged("Ch1Ampl");
                }
            }
        }

        private bool _Ch1AmplEnabled;
        public bool Ch1AmplEnabled
        {
            get { return _Ch1AmplEnabled; }
            set
            {
                if (value != _Ch1AmplEnabled)
                {
                    _Ch1AmplEnabled = value;
                    OnPropertyChanged("Ch1AmplEnabled");
                }
            }
        }

        private uint _Ch1UpRamp;
        public uint Ch1UpRamp
        {
            get { return _Ch1UpRamp; }
            set
            {
                if (value != _Ch1UpRamp)
                {
                    _Ch1UpRamp = value;
                    theModel.Ch1UpRamp = value;
                    OnPropertyChanged("Ch1UpRamp");
                }
            }
        }

        private uint _Ch1DownRamp;
        public uint Ch1DownRamp
        {
            get { return _Ch1DownRamp; }
            set
            {
                if (value != _Ch1DownRamp)
                {
                    _Ch1DownRamp = value;
                    theModel.Ch1DownRamp = value;
                    OnPropertyChanged("Ch1DownRamp");
                }
            }
        }

        private uint _Ch1NSteps;
        public uint Ch1NSteps
        {
            get { return _Ch1NSteps; }
            set
            {
                if (value != _Ch1NSteps)
                {
                    _Ch1NSteps = value;
                    theModel.Ch1NSteps = value;
                    OnPropertyChanged("Ch1NSteps");
                }
            }
        }

        private bool _Ch1LSwpEnabled;
        public bool Ch1LSwpEnabled
        {
            get { return _Ch1LSwpEnabled; }
            set
            {
                if (value != _Ch1LSwpEnabled)
                {
                    _Ch1LSwpEnabled = value;
                    OnPropertyChanged("Ch1LSwpEnabled");
                }
            }
        }

        private EnhancedToneVM _Ch1Mode;
        public EnhancedToneVM Ch1Mode
        {
            get { return _Ch1Mode; }
            set
            {
                if (value != _Ch1Mode)
                {
                    _Ch1Mode = value;
                    switch (_Ch1Mode)
                    {
                        case EnhancedToneVM.FREQUENCY_DWELL: theModel.Ch1Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.FrequencyDwell; break;
                        case EnhancedToneVM.FREQUENCY_NO_DWELL: theModel.Ch1Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.FrequencyNoDwell; break;
                        case EnhancedToneVM.FREQUENCY_FAST_MOD: theModel.Ch1Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.FrequencyFastMod; break;
                        case EnhancedToneVM.PHASE_DWELL: theModel.Ch1Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.PhaseDwell; break;
                        case EnhancedToneVM.PHASE_NO_DWELL: theModel.Ch1Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.PhaseNoDwell; break;
                        case EnhancedToneVM.PHASE_FAST_MOD: theModel.Ch1Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.PhaseFastMod; break;
                        case EnhancedToneVM.NO_SWEEP: theModel.Ch1Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.NoSweep; break;
                    }
                    Ch1AmplEnabled = (_Ch1Mode == EnhancedToneVM.NO_SWEEP);
                    Ch1EFreqEnabled = ((_Ch1Mode == EnhancedToneVM.FREQUENCY_NO_DWELL) || (_Ch1Mode == EnhancedToneVM.FREQUENCY_DWELL) || (_Ch1Mode == EnhancedToneVM.FREQUENCY_FAST_MOD));
                    Ch1EPhsEnabled = ((_Ch1Mode == EnhancedToneVM.PHASE_NO_DWELL) || (_Ch1Mode == EnhancedToneVM.PHASE_DWELL) || (_Ch1Mode == EnhancedToneVM.PHASE_FAST_MOD));
                    Ch1LSwpEnabled = ((_Ch1Mode != EnhancedToneVM.NO_SWEEP) && (_Ch1Mode != EnhancedToneVM.FREQUENCY_FAST_MOD) && (_Ch1Mode != EnhancedToneVM.PHASE_FAST_MOD));
                    OnPropertyChanged("Ch1Mode");
                }
            }
        }

        private iMS.DAC_CURRENT_REFERENCE _Ch1DacRef;
        public iMS.DAC_CURRENT_REFERENCE Ch1DacRef
        {
            get { return _Ch1DacRef; }
            set
            {
                if (value != _Ch1DacRef)
                {
                    _Ch1DacRef = value;
                    switch (_Ch1DacRef)
                    {
                        case iMS.DAC_CURRENT_REFERENCE.FULL_SCALE: theModel.Ch1DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.FullScale; break;
                        case iMS.DAC_CURRENT_REFERENCE.HALF_SCALE: theModel.Ch1DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.HalfScale; break;
                        case iMS.DAC_CURRENT_REFERENCE.QUARTER_SCALE: theModel.Ch1DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.QuarterScale; break;
                        case iMS.DAC_CURRENT_REFERENCE.EIGHTH_SCALE: theModel.Ch1DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.EighthScale; break;
                    }
                    OnPropertyChanged("Ch1DacRef");
                }
            }
        }

        private bool _Ch1ProfileSetting;
        public bool Ch1ProfileSetting
        {
            get { return _Ch1ProfileSetting; }
            set
            {
                if (value != _Ch1ProfileSetting)
                {
                    _Ch1ProfileSetting = value;
                    if (_ManualTriggerSetting)
                    {
                        theModel.ManualProfileTrigger(_Ch1ProfileSetting, Ch2ProfileSetting, Ch3ProfileSetting, Ch4ProfileSetting);
                    }
                    OnPropertyChanged("Ch1ProfileSetting");
                }
            }
        }

        private ICommand _Ch1ProfileCmd;
        public ICommand Ch1ProfileCmd
        {
            get
            {
                if (_Ch1ProfileCmd == null)
                {
                    _Ch1ProfileCmd = new RelayCommand(param => Ch1ProfileSetting = !Ch1ProfileSetting, param => ManualTriggerSetting);
                }
                return _Ch1ProfileCmd;
            }
        }


        private double _Ch2StartFreq;
        public double Ch2StartFreq
        {
            get { return _Ch2StartFreq; }
            set
            {
                if (value != _Ch2StartFreq)
                {
                    _Ch2StartFreq = value;
                    theModel.Ch2StartFreq = value;
                    OnPropertyChanged("Ch2StartFreq");
                }
            }
        }

        private double _Ch2EndFreq;
        public double Ch2EndFreq
        {
            get { return _Ch2EndFreq; }
            set
            {
                if (value != _Ch2EndFreq)
                {
                    _Ch2EndFreq = value;
                    theModel.Ch2EndFreq = value;
                    OnPropertyChanged("Ch2EndFreq");
                }
            }
        }

        private bool _Ch2EFreqEnabled;
        public bool Ch2EFreqEnabled
        {
            get { return _Ch2EFreqEnabled; }
            set
            {
                if (value != _Ch2EFreqEnabled)
                {
                    _Ch2EFreqEnabled = value;
                    OnPropertyChanged("Ch2EFreqEnabled");
                }
            }
        }

        private double _Ch2StartPhase;
        public double Ch2StartPhase
        {
            get { return _Ch2StartPhase; }
            set
            {
                if (value != _Ch2StartPhase)
                {
                    _Ch2StartPhase = value;
                    theModel.Ch2StartPhase = value;
                    OnPropertyChanged("Ch2StartPhase");
                }
            }
        }

        private double _Ch2EndPhase;
        public double Ch2EndPhase
        {
            get { return _Ch2EndPhase; }
            set
            {
                if (value != _Ch2EndPhase)
                {
                    _Ch2EndPhase = value;
                    theModel.Ch2EndPhase = value;
                    OnPropertyChanged("Ch2EndPhase");
                }
            }
        }

        private bool _Ch2EPhsEnabled;
        public bool Ch2EPhsEnabled
        {
            get { return _Ch2EPhsEnabled; }
            set
            {
                if (value != _Ch2EPhsEnabled)
                {
                    _Ch2EPhsEnabled = value;
                    OnPropertyChanged("Ch2EPhsEnabled");
                }
            }
        }

        private double _Ch2Ampl;
        public double Ch2Ampl
        {
            get { return _Ch2Ampl; }
            set
            {
                if (value != _Ch2Ampl)
                {
                    _Ch2Ampl = value;
                    theModel.Ch2Ampl = value;
                    OnPropertyChanged("Ch2Ampl");
                }
            }
        }

        private bool _Ch2AmplEnabled;
        public bool Ch2AmplEnabled
        {
            get { return _Ch2AmplEnabled; }
            set
            {
                if (value != _Ch2AmplEnabled)
                {
                    _Ch2AmplEnabled = value;
                    OnPropertyChanged("Ch2AmplEnabled");
                }
            }
        }

        private uint _Ch2UpRamp;
        public uint Ch2UpRamp
        {
            get { return _Ch2UpRamp; }
            set
            {
                if (value != _Ch2UpRamp)
                {
                    _Ch2UpRamp = value;
                    theModel.Ch2UpRamp = value;
                    OnPropertyChanged("Ch2UpRamp");
                }
            }
        }

        private uint _Ch2DownRamp;
        public uint Ch2DownRamp
        {
            get { return _Ch2DownRamp; }
            set
            {
                if (value != _Ch2DownRamp)
                {
                    _Ch2DownRamp = value;
                    theModel.Ch2DownRamp = value;
                    OnPropertyChanged("Ch2DownRamp");
                }
            }
        }

        private uint _Ch2NSteps;
        public uint Ch2NSteps
        {
            get { return _Ch2NSteps; }
            set
            {
                if (value != _Ch2NSteps)
                {
                    _Ch2NSteps = value;
                    theModel.Ch2NSteps = value;
                    OnPropertyChanged("Ch2NSteps");
                }
            }
        }

        private bool _Ch2LSwpEnabled;
        public bool Ch2LSwpEnabled
        {
            get { return _Ch2LSwpEnabled; }
            set
            {
                if (value != _Ch2LSwpEnabled)
                {
                    _Ch2LSwpEnabled = value;
                    OnPropertyChanged("Ch2LSwpEnabled");
                }
            }
        }

        private EnhancedToneVM _Ch2Mode;
        public EnhancedToneVM Ch2Mode
        {
            get { return _Ch2Mode; }
            set
            {
                if (value != _Ch2Mode)
                {
                    _Ch2Mode = value;
                    switch (_Ch2Mode)
                    {
                        case EnhancedToneVM.FREQUENCY_DWELL: theModel.Ch2Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.FrequencyDwell; break;
                        case EnhancedToneVM.FREQUENCY_NO_DWELL: theModel.Ch2Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.FrequencyNoDwell; break;
                        case EnhancedToneVM.FREQUENCY_FAST_MOD: theModel.Ch2Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.FrequencyFastMod; break;
                        case EnhancedToneVM.PHASE_DWELL: theModel.Ch2Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.PhaseDwell; break;
                        case EnhancedToneVM.PHASE_NO_DWELL: theModel.Ch2Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.PhaseNoDwell; break;
                        case EnhancedToneVM.PHASE_FAST_MOD: theModel.Ch2Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.PhaseFastMod; break;
                        case EnhancedToneVM.NO_SWEEP: theModel.Ch2Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.NoSweep; break;
                    }
                    Ch2AmplEnabled = (_Ch2Mode == EnhancedToneVM.NO_SWEEP);
                    Ch2EFreqEnabled = ((_Ch2Mode == EnhancedToneVM.FREQUENCY_NO_DWELL) || (_Ch2Mode == EnhancedToneVM.FREQUENCY_DWELL) || (_Ch2Mode == EnhancedToneVM.FREQUENCY_FAST_MOD));
                    Ch2EPhsEnabled = ((_Ch2Mode == EnhancedToneVM.PHASE_NO_DWELL) || (_Ch2Mode == EnhancedToneVM.PHASE_DWELL) || (_Ch2Mode == EnhancedToneVM.PHASE_FAST_MOD));
                    Ch2LSwpEnabled = ((_Ch2Mode != EnhancedToneVM.NO_SWEEP) && (_Ch2Mode != EnhancedToneVM.FREQUENCY_FAST_MOD) && (_Ch2Mode != EnhancedToneVM.PHASE_FAST_MOD));
                    OnPropertyChanged("Ch2Mode");
                }
            }
        }

        private iMS.DAC_CURRENT_REFERENCE _Ch2DacRef;
        public iMS.DAC_CURRENT_REFERENCE Ch2DacRef
        {
            get { return _Ch2DacRef; }
            set
            {
                if (value != _Ch2DacRef)
                {
                    _Ch2DacRef = value;
                    switch (_Ch2DacRef)
                    {
                        case iMS.DAC_CURRENT_REFERENCE.FULL_SCALE: theModel.Ch2DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.FullScale; break;
                        case iMS.DAC_CURRENT_REFERENCE.HALF_SCALE: theModel.Ch2DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.HalfScale; break;
                        case iMS.DAC_CURRENT_REFERENCE.QUARTER_SCALE: theModel.Ch2DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.QuarterScale; break;
                        case iMS.DAC_CURRENT_REFERENCE.EIGHTH_SCALE: theModel.Ch2DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.EighthScale; break;
                    }
                    OnPropertyChanged("Ch2DacRef");
                }
            }
        }

        private bool _Ch2ProfileSetting;
        public bool Ch2ProfileSetting
        {
            get { return _Ch2ProfileSetting; }
            set
            {
                if (value != _Ch2ProfileSetting)
                {
                    _Ch2ProfileSetting = value;
                    if (_ManualTriggerSetting)
                    {
                        theModel.ManualProfileTrigger(Ch1ProfileSetting, _Ch2ProfileSetting, Ch3ProfileSetting, Ch4ProfileSetting);
                    }
                    OnPropertyChanged("Ch2ProfileSetting");
                }
            }
        }

        private ICommand _Ch2ProfileCmd;
        public ICommand Ch2ProfileCmd
        {
            get
            {
                if (_Ch2ProfileCmd == null)
                {
                    _Ch2ProfileCmd = new RelayCommand(param => Ch2ProfileSetting = !Ch2ProfileSetting, param => ManualTriggerSetting);
                }
                return _Ch2ProfileCmd;
            }
        }


        private double _Ch3StartFreq;
        public double Ch3StartFreq
        {
            get { return _Ch3StartFreq; }
            set
            {
                if (value != _Ch3StartFreq)
                {
                    _Ch3StartFreq = value;
                    theModel.Ch3StartFreq = value;
                    OnPropertyChanged("Ch3StartFreq");
                }
            }
        }

        private double _Ch3EndFreq;
        public double Ch3EndFreq
        {
            get { return _Ch3EndFreq; }
            set
            {
                if (value != _Ch3EndFreq)
                {
                    _Ch3EndFreq = value;
                    theModel.Ch3EndFreq = value;
                    OnPropertyChanged("Ch3EndFreq");
                }
            }
        }

        private bool _Ch3EFreqEnabled;
        public bool Ch3EFreqEnabled
        {
            get { return _Ch3EFreqEnabled; }
            set
            {
                if (value != _Ch3EFreqEnabled)
                {
                    _Ch3EFreqEnabled = value;
                    OnPropertyChanged("Ch3EFreqEnabled");
                }
            }
        }

        private double _Ch3StartPhase;
        public double Ch3StartPhase
        {
            get { return _Ch3StartPhase; }
            set
            {
                if (value != _Ch3StartPhase)
                {
                    _Ch3StartPhase = value;
                    theModel.Ch3StartPhase = value;
                    OnPropertyChanged("Ch3StartPhase");
                }
            }
        }

        private double _Ch3EndPhase;
        public double Ch3EndPhase
        {
            get { return _Ch3EndPhase; }
            set
            {
                if (value != _Ch3EndPhase)
                {
                    _Ch3EndPhase = value;
                    theModel.Ch3EndPhase = value;
                    OnPropertyChanged("Ch3EndPhase");
                }
            }
        }

        private bool _Ch3EPhsEnabled;
        public bool Ch3EPhsEnabled
        {
            get { return _Ch3EPhsEnabled; }
            set
            {
                if (value != _Ch3EPhsEnabled)
                {
                    _Ch3EPhsEnabled = value;
                    OnPropertyChanged("Ch3EPhsEnabled");
                }
            }
        }

        private double _Ch3Ampl;
        public double Ch3Ampl
        {
            get { return _Ch3Ampl; }
            set
            {
                if (value != _Ch3Ampl)
                {
                    _Ch3Ampl = value;
                    theModel.Ch3Ampl = value;
                    OnPropertyChanged("Ch3Ampl");
                }
            }
        }

        private bool _Ch3AmplEnabled;
        public bool Ch3AmplEnabled
        {
            get { return _Ch3AmplEnabled; }
            set
            {
                if (value != _Ch3AmplEnabled)
                {
                    _Ch3AmplEnabled = value;
                    OnPropertyChanged("Ch3AmplEnabled");
                }
            }
        }

        private uint _Ch3UpRamp;
        public uint Ch3UpRamp
        {
            get { return _Ch3UpRamp; }
            set
            {
                if (value != _Ch3UpRamp)
                {
                    _Ch3UpRamp = value;
                    theModel.Ch3UpRamp = value;
                    OnPropertyChanged("Ch3UpRamp");
                }
            }
        }

        private uint _Ch3DownRamp;
        public uint Ch3DownRamp
        {
            get { return _Ch3DownRamp; }
            set
            {
                if (value != _Ch3DownRamp)
                {
                    _Ch3DownRamp = value;
                    theModel.Ch3DownRamp = value;
                    OnPropertyChanged("Ch3DownRamp");
                }
            }
        }

        private uint _Ch3NSteps;
        public uint Ch3NSteps
        {
            get { return _Ch3NSteps; }
            set
            {
                if (value != _Ch3NSteps)
                {
                    _Ch3NSteps = value;
                    theModel.Ch3NSteps = value;
                    OnPropertyChanged("Ch3NSteps");
                }
            }
        }

        private bool _Ch3LSwpEnabled;
        public bool Ch3LSwpEnabled
        {
            get { return _Ch3LSwpEnabled; }
            set
            {
                if (value != _Ch3LSwpEnabled)
                {
                    _Ch3LSwpEnabled = value;
                    OnPropertyChanged("Ch3LSwpEnabled");
                }
            }
        }

        private EnhancedToneVM _Ch3Mode;
        public EnhancedToneVM Ch3Mode
        {
            get { return _Ch3Mode; }
            set
            {
                if (value != _Ch3Mode)
                {
                    _Ch3Mode = value;
                    switch (_Ch3Mode)
                    {
                        case EnhancedToneVM.FREQUENCY_DWELL: theModel.Ch3Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.FrequencyDwell; break;
                        case EnhancedToneVM.FREQUENCY_NO_DWELL: theModel.Ch3Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.FrequencyNoDwell; break;
                        case EnhancedToneVM.FREQUENCY_FAST_MOD: theModel.Ch3Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.FrequencyFastMod; break;
                        case EnhancedToneVM.PHASE_DWELL: theModel.Ch3Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.PhaseDwell; break;
                        case EnhancedToneVM.PHASE_NO_DWELL: theModel.Ch3Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.PhaseNoDwell; break;
                        case EnhancedToneVM.PHASE_FAST_MOD: theModel.Ch3Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.PhaseFastMod; break;
                        case EnhancedToneVM.NO_SWEEP: theModel.Ch3Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.NoSweep; break;
                    }
                    Ch3AmplEnabled = (_Ch3Mode == EnhancedToneVM.NO_SWEEP);
                    Ch3EFreqEnabled = ((_Ch3Mode == EnhancedToneVM.FREQUENCY_NO_DWELL) || (_Ch3Mode == EnhancedToneVM.FREQUENCY_DWELL) || (_Ch3Mode == EnhancedToneVM.FREQUENCY_FAST_MOD));
                    Ch3EPhsEnabled = ((_Ch3Mode == EnhancedToneVM.PHASE_NO_DWELL) || (_Ch3Mode == EnhancedToneVM.PHASE_DWELL) || (_Ch3Mode == EnhancedToneVM.PHASE_FAST_MOD));
                    Ch3LSwpEnabled = ((_Ch3Mode != EnhancedToneVM.NO_SWEEP) && (_Ch3Mode != EnhancedToneVM.FREQUENCY_FAST_MOD) && (_Ch3Mode != EnhancedToneVM.PHASE_FAST_MOD));
                    OnPropertyChanged("Ch3Mode");
                }
            }
        }

        private iMS.DAC_CURRENT_REFERENCE _Ch3DacRef;
        public iMS.DAC_CURRENT_REFERENCE Ch3DacRef
        {
            get { return _Ch3DacRef; }
            set
            {
                if (value != _Ch3DacRef)
                {
                    _Ch3DacRef = value;
                    switch (_Ch3DacRef)
                    {
                        case iMS.DAC_CURRENT_REFERENCE.FULL_SCALE: theModel.Ch3DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.FullScale; break;
                        case iMS.DAC_CURRENT_REFERENCE.HALF_SCALE: theModel.Ch3DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.HalfScale; break;
                        case iMS.DAC_CURRENT_REFERENCE.QUARTER_SCALE: theModel.Ch3DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.QuarterScale; break;
                        case iMS.DAC_CURRENT_REFERENCE.EIGHTH_SCALE: theModel.Ch3DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.EighthScale; break;
                    }
                    OnPropertyChanged("Ch3DacRef");
                }
            }
        }

        private bool _Ch3ProfileSetting;
        public bool Ch3ProfileSetting
        {
            get { return _Ch3ProfileSetting; }
            set
            {
                if (value != _Ch3ProfileSetting)
                {
                    _Ch3ProfileSetting = value;
                    if (_ManualTriggerSetting)
                    {
                        theModel.ManualProfileTrigger(Ch1ProfileSetting, Ch2ProfileSetting, _Ch3ProfileSetting, Ch4ProfileSetting);
                    }
                    OnPropertyChanged("Ch3ProfileSetting");
                }
            }
        }

        private ICommand _Ch3ProfileCmd;
        public ICommand Ch3ProfileCmd
        {
            get
            {
                if (_Ch3ProfileCmd == null)
                {
                    _Ch3ProfileCmd = new RelayCommand(param => Ch3ProfileSetting = !Ch3ProfileSetting, param => ManualTriggerSetting);
                }
                return _Ch3ProfileCmd;
            }
        }


        private double _Ch4StartFreq;
        public double Ch4StartFreq
        {
            get { return _Ch4StartFreq; }
            set
            {
                if (value != _Ch4StartFreq)
                {
                    _Ch4StartFreq = value;
                    theModel.Ch4StartFreq = value;
                    OnPropertyChanged("Ch4StartFreq");
                }
            }
        }

        private double _Ch4EndFreq;
        public double Ch4EndFreq
        {
            get { return _Ch4EndFreq; }
            set
            {
                if (value != _Ch4EndFreq)
                {
                    _Ch4EndFreq = value;
                    theModel.Ch4EndFreq = value;
                    OnPropertyChanged("Ch4EndFreq");
                }
            }
        }

        private bool _Ch4EFreqEnabled;
        public bool Ch4EFreqEnabled
        {
            get { return _Ch4EFreqEnabled; }
            set
            {
                if (value != _Ch4EFreqEnabled)
                {
                    _Ch4EFreqEnabled = value;
                    OnPropertyChanged("Ch4EFreqEnabled");
                }
            }
        }

        private double _Ch4StartPhase;
        public double Ch4StartPhase
        {
            get { return _Ch4StartPhase; }
            set
            {
                if (value != _Ch4StartPhase)
                {
                    _Ch4StartPhase = value;
                    theModel.Ch4StartPhase = value;
                    OnPropertyChanged("Ch4StartPhase");
                }
            }
        }

        private double _Ch4EndPhase;
        public double Ch4EndPhase
        {
            get { return _Ch4EndPhase; }
            set
            {
                if (value != _Ch4EndPhase)
                {
                    _Ch4EndPhase = value;
                    theModel.Ch4EndPhase = value;
                    OnPropertyChanged("Ch4EndPhase");
                }
            }
        }

        private bool _Ch4EPhsEnabled;
        public bool Ch4EPhsEnabled
        {
            get { return _Ch4EPhsEnabled; }
            set
            {
                if (value != _Ch4EPhsEnabled)
                {
                    _Ch4EPhsEnabled = value;
                    OnPropertyChanged("Ch4EPhsEnabled");
                }
            }
        }

        private double _Ch4Ampl;
        public double Ch4Ampl
        {
            get { return _Ch4Ampl; }
            set
            {
                if (value != _Ch4Ampl)
                {
                    _Ch4Ampl = value;
                    theModel.Ch4Ampl = value;
                    OnPropertyChanged("Ch4Ampl");
                }
            }
        }

        private bool _Ch4AmplEnabled;
        public bool Ch4AmplEnabled
        {
            get { return _Ch4AmplEnabled; }
            set
            {
                if (value != _Ch4AmplEnabled)
                {
                    _Ch4AmplEnabled = value;
                    OnPropertyChanged("Ch4AmplEnabled");
                }
            }
        }

        private uint _Ch4UpRamp;
        public uint Ch4UpRamp
        {
            get { return _Ch4UpRamp; }
            set
            {
                if (value != _Ch4UpRamp)
                {
                    _Ch4UpRamp = value;
                    theModel.Ch4UpRamp = value;
                    OnPropertyChanged("Ch4UpRamp");
                }
            }
        }

        private uint _Ch4DownRamp;
        public uint Ch4DownRamp
        {
            get { return _Ch4DownRamp; }
            set
            {
                if (value != _Ch4DownRamp)
                {
                    _Ch4DownRamp = value;
                    theModel.Ch4DownRamp = value;
                    OnPropertyChanged("Ch4DownRamp");
                }
            }
        }

        private uint _Ch4NSteps;
        public uint Ch4NSteps
        {
            get { return _Ch4NSteps; }
            set
            {
                if (value != _Ch4NSteps)
                {
                    _Ch4NSteps = value;
                    theModel.Ch4NSteps = value;
                    OnPropertyChanged("Ch4NSteps");
                }
            }
        }

        private bool _Ch4LSwpEnabled;
        public bool Ch4LSwpEnabled
        {
            get { return _Ch4LSwpEnabled; }
            set
            {
                if (value != _Ch4LSwpEnabled)
                {
                    _Ch4LSwpEnabled = value;
                    OnPropertyChanged("Ch4LSwpEnabled");
                }
            }
        }

        private EnhancedToneVM _Ch4Mode;
        public EnhancedToneVM Ch4Mode
        {
            get { return _Ch4Mode; }
            set
            {
                if (value != _Ch4Mode)
                {
                    _Ch4Mode = value;
                    switch (_Ch4Mode)
                    {
                        case EnhancedToneVM.FREQUENCY_DWELL: theModel.Ch4Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.FrequencyDwell; break;
                        case EnhancedToneVM.FREQUENCY_NO_DWELL: theModel.Ch4Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.FrequencyNoDwell; break;
                        case EnhancedToneVM.FREQUENCY_FAST_MOD: theModel.Ch4Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.FrequencyFastMod; break;
                        case EnhancedToneVM.PHASE_DWELL: theModel.Ch4Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.PhaseDwell; break;
                        case EnhancedToneVM.PHASE_NO_DWELL: theModel.Ch4Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.PhaseNoDwell; break;
                        case EnhancedToneVM.PHASE_FAST_MOD: theModel.Ch4Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.PhaseFastMod; break;
                        case EnhancedToneVM.NO_SWEEP: theModel.Ch4Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.NoSweep; break;
                    }
                    Ch4AmplEnabled = (_Ch4Mode == EnhancedToneVM.NO_SWEEP);
                    Ch4EFreqEnabled = ((_Ch4Mode == EnhancedToneVM.FREQUENCY_NO_DWELL) || (_Ch4Mode == EnhancedToneVM.FREQUENCY_DWELL) || (_Ch4Mode == EnhancedToneVM.FREQUENCY_FAST_MOD));
                    Ch4EPhsEnabled = ((_Ch4Mode == EnhancedToneVM.PHASE_NO_DWELL) || (_Ch4Mode == EnhancedToneVM.PHASE_DWELL) || (_Ch4Mode == EnhancedToneVM.PHASE_FAST_MOD));
                    Ch4LSwpEnabled = ((_Ch4Mode != EnhancedToneVM.NO_SWEEP) && (_Ch4Mode != EnhancedToneVM.FREQUENCY_FAST_MOD) && (_Ch4Mode != EnhancedToneVM.PHASE_FAST_MOD));
                    OnPropertyChanged("Ch4Mode");
                }
            }
        }

        private iMS.DAC_CURRENT_REFERENCE _Ch4DacRef;
        public iMS.DAC_CURRENT_REFERENCE Ch4DacRef
        {
            get { return _Ch4DacRef; }
            set
            {
                if (value != _Ch4DacRef)
                {
                    _Ch4DacRef = value;
                    switch (_Ch4DacRef)
                    {
                        case iMS.DAC_CURRENT_REFERENCE.FULL_SCALE: theModel.Ch4DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.FullScale; break;
                        case iMS.DAC_CURRENT_REFERENCE.HALF_SCALE: theModel.Ch4DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.HalfScale; break;
                        case iMS.DAC_CURRENT_REFERENCE.QUARTER_SCALE: theModel.Ch4DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.QuarterScale; break;
                        case iMS.DAC_CURRENT_REFERENCE.EIGHTH_SCALE: theModel.Ch4DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.EighthScale; break;
                    }
                    OnPropertyChanged("Ch4DacRef");
                }
            }
        }

        private bool _Ch4ProfileSetting;
        public bool Ch4ProfileSetting
        {
            get { return _Ch4ProfileSetting; }
            set
            {
                if (value != _Ch4ProfileSetting)
                {
                    _Ch4ProfileSetting = value;
                    if (_ManualTriggerSetting)
                    {
                        theModel.ManualProfileTrigger(Ch1ProfileSetting, Ch2ProfileSetting, Ch3ProfileSetting, _Ch4ProfileSetting);
                    }

                    OnPropertyChanged("Ch4ProfileSetting");
                }
            }
        }

        private ICommand _Ch4ProfileCmd;
        public ICommand Ch4ProfileCmd
        {
            get
            {
                if (_Ch4ProfileCmd == null)
                {
                    _Ch4ProfileCmd = new RelayCommand(param => Ch4ProfileSetting = !Ch4ProfileSetting, param => ManualTriggerSetting);
                }
                return _Ch4ProfileCmd;
            }
        }

        private bool _ETMEnable;
        public bool ETMEnable
        {
            get { return _ETMEnable; }
            set
            {
                _ETMEnable = value;
                theModel.EnhancedToneEnable = value;
                // Make sure Profile buttons are synchronised with hardware.
                if (_ETMEnable)
                {
                    if (!_ManualTriggerSetting)
                    {
                        theModel.ExternalProfileTrigger();
                    }
                    else
                    {
                        theModel.ManualProfileTrigger(Ch1ProfileSetting, Ch2ProfileSetting, Ch3ProfileSetting, Ch4ProfileSetting);
                    }
                }
                if (UpdateTone != null) UpdateTone.Execute(value);
                OnPropertyChanged("ETMEnable");
            }
        }

        private bool _ETMUnlocked;
        public bool ETMUnlocked
        {
            get { return _ETMUnlocked; }
            set
            {
                _ETMUnlocked = value;
                OnPropertyChanged("ETMUnlocked");
            }
        }

        private ICommand _ETMEnableCmd;
        public ICommand ETMEnableCmd
        {
            get
            {
                if (_ETMEnableCmd == null)
                {
                    _ETMEnableCmd = new RelayCommand(param => ETMEnable = !ETMEnable, en => ETMUnlocked);
                }
                return _ETMEnableCmd;
            }
        }

        private bool _ManualTriggerSetting;
        public bool ManualTriggerSetting
        {
            get { return _ManualTriggerSetting; }
            set
            {
                if (value != _ManualTriggerSetting)
                {
                    _ManualTriggerSetting = value;
                    if (!_ManualTriggerSetting)
                    {
                        theModel.ExternalProfileTrigger();
                    } else
                    {
                        theModel.ManualProfileTrigger(Ch1ProfileSetting, Ch2ProfileSetting, Ch3ProfileSetting, Ch4ProfileSetting);
                    }
                    OnPropertyChanged("ManualTriggerSetting");
                }
            }
        }

        private ICommand _ManualTriggerCmd;
        public ICommand ManualTriggerCmd
        {
            get
            {
                if (_ManualTriggerCmd == null)
                {
                    _ManualTriggerCmd = new RelayCommand(param => ManualTriggerSetting = !ManualTriggerSetting, param => ETMEnable);
                }
                return _ManualTriggerCmd;
            }
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
           // this.ETMEnable = true;
            this.Ch1StartFreq = 12.5;
            this.Ch1EndFreq = 100.0;
            this.Ch1StartPhase = 0.0;
            this.Ch1EndPhase = 0.0;
            this.Ch1Ampl = 100.0;
            this.Ch1UpRamp = 10;
            this.Ch1DownRamp = 20;
            this.Ch1NSteps = 50;
            this.Ch1Mode = EnhancedToneVM.FREQUENCY_NO_DWELL;
            this.Ch1DacRef = iMS.DAC_CURRENT_REFERENCE.FULL_SCALE;
            this.Ch2StartFreq = 12.5;
            this.Ch2EndFreq = 100.0;
            this.Ch2StartPhase = 0.0;
            this.Ch2EndPhase = 0.0;
            this.Ch2Ampl = 100.0;
            this.Ch2UpRamp = 10;
            this.Ch2DownRamp = 20;
            this.Ch2NSteps = 50;
            this.Ch2Mode = EnhancedToneVM.FREQUENCY_NO_DWELL;
            this.Ch2DacRef = iMS.DAC_CURRENT_REFERENCE.FULL_SCALE;
            this.Ch3StartFreq = 12.5;
            this.Ch3EndFreq = 100.0;
            this.Ch3StartPhase = 0.0;
            this.Ch3EndPhase = 0.0;
            this.Ch3Ampl = 100.0;
            this.Ch3UpRamp = 10;
            this.Ch3DownRamp = 20;
            this.Ch3NSteps = 50;
            this.Ch3Mode = EnhancedToneVM.FREQUENCY_NO_DWELL;
            this.Ch3DacRef = iMS.DAC_CURRENT_REFERENCE.FULL_SCALE;
            this.Ch4StartFreq = 12.5;
            this.Ch4EndFreq = 100.0;
            this.Ch4StartPhase = 0.0;
            this.Ch4EndPhase = 0.0;
            this.Ch4Ampl = 100.0;
            this.Ch4UpRamp = 10;
            this.Ch4DownRamp = 20;
            this.Ch4NSteps = 50;
            this.Ch4Mode = EnhancedToneVM.FREQUENCY_NO_DWELL;
            this.Ch4DacRef = iMS.DAC_CURRENT_REFERENCE.FULL_SCALE;
            this.ETMEnable = false;
        }

    }
}
