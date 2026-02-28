using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using ImsHwServer;
using System.Threading;
using System.Windows;

namespace iMS_Studio.Model
{
    public class EnhancedToneModel
    {
        private Channel _channel;
        private ims_system thisIMS;

        private EnhancedTone _tone;
        private bool _isEnabled;
        private bool _isDirty;

        private Timer _timer;

        public double Ch1StartFreq
        {
            get { return _tone.Chan[0].StartFreq; }
            set
            {
                if (_tone.Chan[0].StartFreq != value)
                {
                    _tone.Chan[0].StartFreq = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch1EndFreq
        {
            get { return _tone.Chan[0].EndFreq; }
            set
            {
                if (_tone.Chan[0].EndFreq != value)
                {
                    _tone.Chan[0].EndFreq = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch1StartPhase
        {
            get { return _tone.Chan[0].StartPhs; }
            set
            {
                if (_tone.Chan[0].StartPhs != value)
                {
                    _tone.Chan[0].StartPhs = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch1EndPhase
        {
            get { return _tone.Chan[0].EndPhs; }
            set
            {
                if (_tone.Chan[0].EndPhs != value)
                {
                    _tone.Chan[0].EndPhs = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch1Ampl
        {
            get { return _tone.Chan[0].Ampl; }
            set
            {
                if (_tone.Chan[0].Ampl != value)
                {
                    _tone.Chan[0].Ampl = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public uint Ch1UpRamp
        {
            get { return _tone.Chan[0].UpRamp; }
            set
            {
                if (_tone.Chan[0].UpRamp != value)
                {
                    _tone.Chan[0].UpRamp = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public uint Ch1DownRamp
        {
            get { return _tone.Chan[0].DownRamp; }
            set
            {
                if (_tone.Chan[0].DownRamp != value)
                {
                    _tone.Chan[0].DownRamp = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public uint Ch1NSteps
        {
            get { return _tone.Chan[0].NSteps; }
            set
            {
                if (_tone.Chan[0].NSteps != value)
                {
                    _tone.Chan[0].NSteps = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public EnhancedTone.Types.ChannelSweep.Types.Mode Ch1Mode
        {
            get { return _tone.Chan[0].Mode; }
            set
            {
                if (_tone.Chan[0].Mode != value)
                {
                    _tone.Chan[0].Mode = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference Ch1DacRef
        {
            get { return _tone.Chan[0].DacRef; }
            set
            {
                if (_tone.Chan[0].DacRef != value)
                {
                    _tone.Chan[0].DacRef = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch2StartFreq
        {
            get { return _tone.Chan[1].StartFreq; }
            set
            {
                if (_tone.Chan[1].StartFreq != value)
                {
                    _tone.Chan[1].StartFreq = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch2EndFreq
        {
            get { return _tone.Chan[1].EndFreq; }
            set
            {
                if (_tone.Chan[1].EndFreq != value)
                {
                    _tone.Chan[1].EndFreq = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch2StartPhase
        {
            get { return _tone.Chan[1].StartPhs; }
            set
            {
                if (_tone.Chan[1].StartPhs != value)
                {
                    _tone.Chan[1].StartPhs = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch2EndPhase
        {
            get { return _tone.Chan[1].EndPhs; }
            set
            {
                if (_tone.Chan[1].EndPhs != value)
                {
                    _tone.Chan[1].EndPhs = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch2Ampl
        {
            get { return _tone.Chan[1].Ampl; }
            set
            {
                if (_tone.Chan[1].Ampl != value)
                {
                    _tone.Chan[1].Ampl = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public uint Ch2UpRamp
        {
            get { return _tone.Chan[1].UpRamp; }
            set
            {
                if (_tone.Chan[1].UpRamp != value)
                {
                    _tone.Chan[1].UpRamp = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public uint Ch2DownRamp
        {
            get { return _tone.Chan[1].DownRamp; }
            set
            {
                if (_tone.Chan[1].DownRamp != value)
                {
                    _tone.Chan[1].DownRamp = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public uint Ch2NSteps
        {
            get { return _tone.Chan[1].NSteps; }
            set
            {
                if (_tone.Chan[1].NSteps != value)
                {
                    _tone.Chan[1].NSteps = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public EnhancedTone.Types.ChannelSweep.Types.Mode Ch2Mode
        {
            get { return _tone.Chan[1].Mode; }
            set
            {
                if (_tone.Chan[1].Mode != value)
                {
                    _tone.Chan[1].Mode = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference Ch2DacRef
        {
            get { return _tone.Chan[1].DacRef; }
            set
            {
                if (_tone.Chan[1].DacRef != value)
                {
                    _tone.Chan[1].DacRef = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch3StartFreq
        {
            get { return _tone.Chan[2].StartFreq; }
            set
            {
                if (_tone.Chan[2].StartFreq != value)
                {
                    _tone.Chan[2].StartFreq = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch3EndFreq
        {
            get { return _tone.Chan[2].EndFreq; }
            set
            {
                if (_tone.Chan[2].EndFreq != value)
                {
                    _tone.Chan[2].EndFreq = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch3StartPhase
        {
            get { return _tone.Chan[2].StartPhs; }
            set
            {
                if (_tone.Chan[2].StartPhs != value)
                {
                    _tone.Chan[2].StartPhs = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch3EndPhase
        {
            get { return _tone.Chan[2].EndPhs; }
            set
            {
                if (_tone.Chan[2].EndPhs != value)
                {
                    _tone.Chan[2].EndPhs = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch3Ampl
        {
            get { return _tone.Chan[2].Ampl; }
            set
            {
                if (_tone.Chan[2].Ampl != value)
                {
                    _tone.Chan[2].Ampl = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public uint Ch3UpRamp
        {
            get { return _tone.Chan[2].UpRamp; }
            set
            {
                if (_tone.Chan[2].UpRamp != value)
                {
                    _tone.Chan[2].UpRamp = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public uint Ch3DownRamp
        {
            get { return _tone.Chan[2].DownRamp; }
            set
            {
                if (_tone.Chan[2].DownRamp != value)
                {
                    _tone.Chan[2].DownRamp = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public uint Ch3NSteps
        {
            get { return _tone.Chan[2].NSteps; }
            set
            {
                if (_tone.Chan[2].NSteps != value)
                {
                    _tone.Chan[2].NSteps = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public EnhancedTone.Types.ChannelSweep.Types.Mode Ch3Mode
        {
            get { return _tone.Chan[2].Mode; }
            set
            {
                if (_tone.Chan[2].Mode != value)
                {
                    _tone.Chan[2].Mode = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference Ch3DacRef
        {
            get { return _tone.Chan[2].DacRef; }
            set
            {
                if (_tone.Chan[2].DacRef != value)
                {
                    _tone.Chan[2].DacRef = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch4StartFreq
        {
            get { return _tone.Chan[3].StartFreq; }
            set
            {
                if (_tone.Chan[3].StartFreq != value)
                {
                    _tone.Chan[3].StartFreq = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch4EndFreq
        {
            get { return _tone.Chan[3].EndFreq; }
            set
            {
                if (_tone.Chan[3].EndFreq != value)
                {
                    _tone.Chan[3].EndFreq = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch4StartPhase
        {
            get { return _tone.Chan[3].StartPhs; }
            set
            {
                if (_tone.Chan[3].StartPhs != value)
                {
                    _tone.Chan[3].StartPhs = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch4EndPhase
        {
            get { return _tone.Chan[3].EndPhs; }
            set
            {
                if (_tone.Chan[3].EndPhs != value)
                {
                    _tone.Chan[3].EndPhs = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public double Ch4Ampl
        {
            get { return _tone.Chan[3].Ampl; }
            set
            {
                if (_tone.Chan[3].Ampl != value)
                {
                    _tone.Chan[3].Ampl = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public uint Ch4UpRamp
        {
            get { return _tone.Chan[3].UpRamp; }
            set
            {
                if (_tone.Chan[3].UpRamp != value)
                {
                    _tone.Chan[3].UpRamp = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public uint Ch4DownRamp
        {
            get { return _tone.Chan[3].DownRamp; }
            set
            {
                if (_tone.Chan[3].DownRamp != value)
                {
                    _tone.Chan[3].DownRamp = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public uint Ch4NSteps
        {
            get { return _tone.Chan[3].NSteps; }
            set
            {
                if (_tone.Chan[3].NSteps != value)
                {
                    _tone.Chan[3].NSteps = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public EnhancedTone.Types.ChannelSweep.Types.Mode Ch4Mode
        {
            get { return _tone.Chan[3].Mode; }
            set
            {
                if (_tone.Chan[3].Mode != value)
                {
                    _tone.Chan[3].Mode = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference Ch4DacRef
        {
            get { return _tone.Chan[3].DacRef; }
            set
            {
                if (_tone.Chan[3].DacRef != value)
                {
                    _tone.Chan[3].DacRef = value;
                    _isDirty = true;
                    //if (_isEnabled) updateHW();
                }
            }
        }

        public EnhancedToneModel(Channel channel, ims_system ims)
        {
            _channel = channel;
            thisIMS = ims;
            _isDirty = false;

            var client = new ImsHwServer.signal_path.signal_pathClient(_channel);

            _tone = new EnhancedTone()
            {
                Chan = {
                    // Channel 1
                    new EnhancedTone.Types.ChannelSweep
                    {
                    StartFreq = 12.5,
                    EndFreq = 100.0,
                    StartPhs = 0.0,
                    EndPhs = 0.0,
                    Ampl = 100.0,
                    UpRamp = 10,
                    DownRamp = 20,
                    NSteps = 50,
                    Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.FrequencyNoDwell,
                    DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.FullScale
                    },
                    // Channel 2
                    new EnhancedTone.Types.ChannelSweep
                    {
                    StartFreq = 12.5,
                    EndFreq = 100.0,
                    StartPhs = 0.0,
                    EndPhs = 0.0,
                    Ampl = 100.0,
                    UpRamp = 10,
                    DownRamp = 20,
                    NSteps = 50,
                    Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.FrequencyNoDwell,
                    DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.FullScale
                    },
                    // Channel 3
                    new EnhancedTone.Types.ChannelSweep
                    {
                    StartFreq = 12.5,
                    EndFreq = 100.0,
                    StartPhs = 0.0,
                    EndPhs = 0.0,
                    Ampl = 100.0,
                    UpRamp = 10,
                    DownRamp = 20,
                    NSteps = 50,
                    Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.FrequencyNoDwell,
                    DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.FullScale
                    },
                    // Channel 4
                    new EnhancedTone.Types.ChannelSweep
                    {
                    StartFreq = 12.5,
                    EndFreq = 100.0,
                    StartPhs = 0.0,
                    EndPhs = 0.0,
                    Ampl = 100.0,
                    UpRamp = 10,
                    DownRamp = 20,
                    NSteps = 50,
                    Mode = EnhancedTone.Types.ChannelSweep.Types.Mode.FrequencyNoDwell,
                    DacRef = EnhancedTone.Types.ChannelSweep.Types.DacCurrentReference.FullScale
                    }
                }
            };
            _isEnabled = false;

            _timer = new Timer(OnTimer, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }

        private void StartTimer()
        {
            _timer.Change(TimeSpan.Zero, new TimeSpan(0, 0, 0, 0, 100));
        }

        private void StopTimer()
        {
            _timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }

        private void OnTimer(object state)
        {
            if (_isDirty)
            {
                _isDirty = false;
                StopTimer();
                updateHW();
                StartTimer();
            }
        }

        private void updateHW()
        {
            if (thisIMS != null)
            {
                var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                client.set_enh_tone(_tone);
            }
        }

        public bool EnhancedToneCanEnable
        {
            get {
                var SigPath = new signal_path.signal_pathClient(_channel);
                ImsHwServer.SignalPathStatus sigPathSts = SigPath.get_status(new Google.Protobuf.WellKnownTypes.Empty());
                return (!sigPathSts.CalibrationTonePlaying);
            }
        }

        public bool EnhancedToneEnable
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    if (!_isEnabled)
                    {
                        StopTimer();
                        if (thisIMS != null)
                        {
                            var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                            // First Clear EnhancedTone
                            client.clear_enh_tone(new Google.Protobuf.WellKnownTypes.Empty());
                            // Then set tone to zero amplitude
                            client.set_tone(new CalibrationTone() { Ampl = 0.0 });
                            // Then Switch out of STM mode
                            client.clear_tone(new Google.Protobuf.WellKnownTypes.Empty());
                        }
                    }
                    else
                    {
                        _isDirty = true;
                        StartTimer();
                    }
                }
            }
        }

        public void ManualProfileTrigger(bool ch1, bool ch2, bool ch3, bool ch4)
        {
            if (_isEnabled && (thisIMS != null))
            {
                var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                SweepControl swc = new SweepControl();
                swc.ManualTriggerEnable = true;
                swc.TrigCh1 = ch1;
                swc.TrigCh2 = ch2;
                swc.TrigCh3 = ch3;
                swc.TrigCh4 = ch4;
                client.control_enh_sweep(swc);
            }
        }

        public void ExternalProfileTrigger()
        {
            if (_isEnabled && (thisIMS != null))
            {
                var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                SweepControl swc = new SweepControl();
                swc.ManualTriggerEnable = false;
                client.control_enh_sweep(swc);
            }
        }

        public double MinFrequency
        {
            get
            {
                if (thisIMS != null)
                {
                    return thisIMS.Synth.Cap.LowerFreq;
                }
                else
                {
                    return 0.0;
                }
            }
        }

        public double MaxFrequency
        {
            get
            {
                if (thisIMS != null)
                {
                    return thisIMS.Synth.Cap.UpperFreq;
                }
                else
                {
                    return 250.0;
                }
            }
        }
    }
}
