using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using ImsHwServer;
using iMS;

namespace iMS_Studio.Model
{
    public class CalibrationToneModel
    {
        private Channel _channel;
        private ims_system thisIMS;

        private CalibrationTone _tone;
        private bool _isEnabled;
        private bool[] _chanLock;
        private bool _toneDirty;
        private bool[] _lockDirty;

        public CalibrationToneModel(Channel channel, ims_system ims)
        {
            _channel = channel;
            thisIMS = ims;

            var client = new ImsHwServer.signal_path.signal_pathClient(_channel);

            _tone = new CalibrationTone()
            {
                Freq = 100.0,
                Ampl = 0.0,
                Phs = 0.0
            };
            _isEnabled = false;
            _chanLock = new bool[4] { false, false, false, false };
            _toneDirty = false;
            _lockDirty = new bool[4] { false, false, false, false };
        }

        public double CalibrationFrequency
        {
            get { return _tone.Freq; }
            set
            {
                if (_tone.Freq != value)
                {
                    _tone.Freq = value;
                    _toneDirty = true;
                    if (_isEnabled)
                        updateHW();
                }
            }
        }

        public double CalibrationAmplitude
        {
            get { return _tone.Ampl; }
            set
            {
                if (_tone.Ampl != value)
                {
                    _tone.Ampl = value;
                    _toneDirty = true;
                    if (_isEnabled)
                        updateHW();
                }
            }
        }

        public double CalibrationPhase
        {
            get { return _tone.Phs; }
            set
            {
                if (_tone.Phs != value)
                {
                    _tone.Phs = value;
                    _toneDirty = true;
                    if (_isEnabled)
                        updateHW();
                }
            }
        }

        public bool LockChan1
        {
            get {
                var LockState = new signal_path.signal_pathClient(_channel);
                ImsHwServer.ChannelFlags chanFlags = LockState.get_hold_state(new Google.Protobuf.WellKnownTypes.Empty());
                _chanLock[0] = chanFlags.Ch1;
                return _chanLock[0];
            }
            set
            {
                if (_chanLock[0] != value)
                {
                    _chanLock[0] = value;
                    _lockDirty[0] = true;
                    if (!_chanLock[0])
                    {
                        // If re-enabling channel, re-send tone
                        _toneDirty = true;
                    }
                    if (_isEnabled)
                    {
                        updateHW();
                    }
                }
            }
        }

        public bool LockChan2
        {
            get {
                var LockState = new signal_path.signal_pathClient(_channel);
                ImsHwServer.ChannelFlags chanFlags = LockState.get_hold_state(new Google.Protobuf.WellKnownTypes.Empty());
                _chanLock[1] = chanFlags.Ch2;
                return _chanLock[1];
            }
            set
            {
                if (_chanLock[1] != value)
                {
                    _chanLock[1] = value;
                    _lockDirty[1] = true;
                    if (!_chanLock[1])
                    {
                        // If re-enabling channel, re-send tone
                        _toneDirty = true;
                    }
                    if (_isEnabled)
                    {
                        updateHW();
                    }
                }
            }
        }

        public bool LockChan3
        {
            get {
                var LockState = new signal_path.signal_pathClient(_channel);
                ImsHwServer.ChannelFlags chanFlags = LockState.get_hold_state(new Google.Protobuf.WellKnownTypes.Empty());
                _chanLock[2] = chanFlags.Ch3;
                return _chanLock[2];
            }
            set
            {
                if (_chanLock[2] != value)
                {
                    _chanLock[2] = value;
                    _lockDirty[2] = true;
                    if (!_chanLock[2])
                    {
                        // If re-enabling channel, re-send tone
                        _toneDirty = true;
                    }
                    if (_isEnabled)
                    {
                        updateHW();
                    }
                }
            }
        }

        public bool LockChan4
        {
            get {
                var LockState = new signal_path.signal_pathClient(_channel);
                ImsHwServer.ChannelFlags chanFlags = LockState.get_hold_state(new Google.Protobuf.WellKnownTypes.Empty());
                _chanLock[3] = chanFlags.Ch4;
                return _chanLock[3];
            }
            set
            {
                if (_chanLock[3] != value)
                {
                    _chanLock[3] = value;
                    _lockDirty[3] = true;
                    if (!_chanLock[3]) {
                        // If re-enabling channel, re-send tone
                        _toneDirty = true;
                    }
                    if (_isEnabled)
                    {
                        updateHW();
                    }
                }
            }
        }

        private void updateHW()
        {
            if (thisIMS != null)
            {
                var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                var lockMsg = new ImsHwServer.ChannelFlags();
                bool HoldChange = false;
                lockMsg.Ch1 = false;
                lockMsg.Ch2 = false;
                lockMsg.Ch3 = false;
                lockMsg.Ch4 = false;
                if (_lockDirty[0] && _chanLock[0])
                {
                    HoldChange = true;
                    lockMsg.Ch1 = true;
                    _lockDirty[0] = false;
                }
                if (_lockDirty[1] && _chanLock[1])
                {
                    HoldChange = true;
                    lockMsg.Ch2 = true;
                    _lockDirty[1] = false;
                }
                if (_lockDirty[2] && _chanLock[2])
                {
                    HoldChange = true;
                    lockMsg.Ch3 = true;
                    _lockDirty[2] = false;
                }
                if (_lockDirty[3] && _chanLock[3])
                {
                    HoldChange = true;
                    lockMsg.Ch4 = true;
                    _lockDirty[3] = false;
                }
                if (HoldChange)
                {
                    client.hold_tone(lockMsg);
                }

                bool FreedChange = false;
                lockMsg.Ch1 = false;
                lockMsg.Ch2 = false;
                lockMsg.Ch3 = false;
                lockMsg.Ch4 = false;
                if (_lockDirty[0] && !_chanLock[0])
                {
                    FreedChange = true;
                    lockMsg.Ch1 = true;
                    _lockDirty[0] = false;
                }
                if (_lockDirty[1] && !_chanLock[1])
                {
                    FreedChange = true;
                    lockMsg.Ch2 = true;
                    _lockDirty[1] = false;
                }
                if (_lockDirty[2] && !_chanLock[2])
                {
                    FreedChange = true;
                    lockMsg.Ch3 = true;
                    _lockDirty[2] = false;
                }
                if (_lockDirty[3] && !_chanLock[3])
                {
                    FreedChange = true;
                    lockMsg.Ch4 = true;
                    _lockDirty[3] = false;
                }
                if (FreedChange)
                {
                    client.free_tone(lockMsg);
                }
                if (_toneDirty)
                {
                    client.set_tone(_tone);
                    _toneDirty = false;
                }
            }
        }

        public bool CalibrationEnable
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    if (!_isEnabled)
                    {
                        if (thisIMS != null)
                        {
                            var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                            // First set tone to zero amplitude
                            client.set_tone(new CalibrationTone() { Ampl = 0.0 });
                            // Then Switch out of STM mode
                            client.clear_tone(new Google.Protobuf.WellKnownTypes.Empty());
                        }
                    }
                    else
                    {
                        _toneDirty = true;
                        _lockDirty[0] = true;
                        _lockDirty[1] = true;
                        _lockDirty[2] = true;
                        _lockDirty[3] = true;
                        updateHW();
                    }
                }
            }
        }

        public bool CalibrationCanEnable
        {
            get
            {
                var SigPath = new signal_path.signal_pathClient(_channel);
                ImsHwServer.SignalPathStatus sigPathSts = SigPath.get_status(new Google.Protobuf.WellKnownTypes.Empty());
                return (!sigPathSts.EnhancedTonePlaying);
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
