using Grpc.Core;
using ImsHwServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iMS_Studio.Model
{
    public class SignalPathModel
    {
        private Channel _channel;
        private ims_system thisIMS;
        private bool hasIndependentChannels;

        private PowerSettings _settings;
        private ChannelPowerSettings[] _chanSettings;
        private SyncDelay _syncdelay;
        private ChanDelay _chandelay;
        private MasterSwitch _mastersw;
        private SyncMapping[] _syncmap;
        private bool _autoPhaseResync;

        private bool _isSyncDelayDirty, _isPowerSettingsDirty, _isChanDelayDirty;
        private bool[] _isChannelSettingsDirty;
        private Timer _timer;

        public SignalPathModel(Channel channel, ims_system ims)
        {
            _channel = channel;
            thisIMS = ims;
            _isSyncDelayDirty = false;
            _isPowerSettingsDirty = false;
            _isChanDelayDirty = false;
            _isChannelSettingsDirty = new bool[] { true, true, true, true };
            _autoPhaseResync = false;

            hasIndependentChannels = thisIMS != null ? thisIMS.Synth.Cap.ChannelComp : true;

            _settings = new PowerSettings
            {
                DDSPower = 25,
                Wiper1Power = 50,
                Wiper2Power = 50,
                Src = PowerSettings.Types.AmplitudeControl.Wiper1
            };

            _chanSettings = new ChannelPowerSettings[]
            {
                new ChannelPowerSettings { Channel=1, PowerLevel=50.0, Src=ChannelPowerSettings.Types.PowerControl.Internal },
                new ChannelPowerSettings { Channel=2, PowerLevel=50.0, Src=ChannelPowerSettings.Types.PowerControl.Internal },
                new ChannelPowerSettings { Channel=3, PowerLevel=50.0, Src=ChannelPowerSettings.Types.PowerControl.Internal },
                new ChannelPowerSettings { Channel=4, PowerLevel=50.0, Src=ChannelPowerSettings.Types.PowerControl.Internal }
            };

            _syncdelay = new SyncDelay
            {
                Delay = 0,
                PulseLength = 0,
                PulseEnAll = false,
                Invert = true
            };
            for (int i=0; i<12; i++)
            {
                _syncdelay.PulseEnBits.Add(false);
            }

            _chandelay = new ChanDelay
            {
                Delay12 = 0,
                Delay34 = 0
            };

            _mastersw = new MasterSwitch
            {
                AmplifierEn = false,
                RFChannel12En = false,
                RFChannel34En = false,
                ExternalEn = false
            };

            _syncmap = new SyncMapping[]
            {
                new SyncMapping() {
                    Sink = SyncMapping.Types.SyncSink.AnalogA,
                    Src = SyncMapping.Types.SyncSource.ImageAnalogA
                },
                new SyncMapping() {
                    Sink = SyncMapping.Types.SyncSink.AnalogB,
                    Src = SyncMapping.Types.SyncSource.ImageAnalogB
                },
                new SyncMapping() {
                    Sink = SyncMapping.Types.SyncSink.Digital,
                    Src = SyncMapping.Types.SyncSource.ImageDigital
                }
            };

            if (thisIMS != null)
            {
                var client = new ImsHwServer.signal_path.signal_pathClient(_channel);

                if (hasIndependentChannels)
                {
                    for (int i = 1; i <= 4; i++) client.channel_power(_chanSettings[i - 1]);
                }
                else
                {
                    client.dds_power(_settings);
                }
                client.set_sync_delay(_syncdelay);
                client.rf_enable(_mastersw);
                foreach (var syncmap in _syncmap)
                    client.set_sync_map(syncmap);
            }

            _timer = new Timer(OnTimer, null, TimeSpan.Zero, new TimeSpan(0, 0, 0, 0, 100));
        }

/*        private void StartTimer()
        {
            _timer.Change(TimeSpan.Zero, new TimeSpan(0, 0, 0, 0, 999));
        }

        private void StopTimer()
        {
            _timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }*/

        private void OnTimer(object state)
        {
            if (_isSyncDelayDirty || _isPowerSettingsDirty || _isChanDelayDirty)
            {
                updateHW(_isPowerSettingsDirty, _isSyncDelayDirty, _isChanDelayDirty);
                _isSyncDelayDirty = false;
                _isPowerSettingsDirty = false;
                _isChanDelayDirty = false;
            }
            for (int i=1; i<=4; i++)
            {
                if (_isChannelSettingsDirty[i-1])
                {
                    updateChan(i);
                    _isChannelSettingsDirty[i - 1] = false;
                }
            }
        }

        private void updateHW(bool updatePower, bool updateDelay, bool updateChanDelay)
        {
            if (thisIMS != null)
            {
                var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                if (updatePower) client.dds_power(_settings);
                if (updateDelay) client.set_sync_delay(_syncdelay);
                if (updateChanDelay) client.set_chan_delay(_chandelay);
            }
        }

        private void updateChan(int chan)
        {
            if (!hasIndependentChannels) return;
            if (thisIMS != null)
            {
                var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                client.channel_power(_chanSettings[chan-1]);
            }
        }

        public bool HasIndependentChannels {  get { return  hasIndependentChannels; } }
        public int Channels { get { if (thisIMS != null) { return thisIMS.Synth.Cap.Channels; } else { return 1; } } }

        public double DDSPower
        {
            get { return _settings.DDSPower; }
            set
            {
                _settings.DDSPower = value;
                _isPowerSettingsDirty = true;
            }
        }

        public double Wiper1Power
        {
            get { return _settings.Wiper1Power; }
            set
            {
                _settings.Wiper1Power = value;
                _chanSettings[0].PowerLevel = value;
                if (!hasIndependentChannels) _isPowerSettingsDirty = true;
                _isChannelSettingsDirty[0] = true;
            }
        }

        public double Wiper2Power
        {
            get { return _settings.Wiper2Power; }
            set
            {
                _settings.Wiper2Power = value;
                _chanSettings[1].PowerLevel = value;
                if (!hasIndependentChannels) _isPowerSettingsDirty = true;
                _isChannelSettingsDirty[1] = true;
            }
        }

        public double Chan3Power
        {
            get { return _chanSettings[2].PowerLevel; }
            set
            {
                _chanSettings[2].PowerLevel = value;
                _isChannelSettingsDirty[2] = true;
            }
        }

        public double Chan4Power
        {
            get { return _chanSettings[3].PowerLevel; }
            set
            {
                _chanSettings[3].PowerLevel = value;
                _isChannelSettingsDirty[3] = true;
             }
        }

        public double SyncDelay
        {
            get { return _syncdelay.Delay; }
            set
            {
                _syncdelay.Delay = (uint)(value * 100 + 0.5) * 10;
                _isSyncDelayDirty = true;
            }
        }

        public double PulseLength
        {
            get { return _syncdelay.PulseLength; }
            set
            {
                _syncdelay.PulseLength = (uint)(value * 100 + 0.5) * 10;
                _isSyncDelayDirty = true;
            }
        }

        public bool PulseEnAllBits
        {
            get { return _syncdelay.PulseEnAll;  }
            set
            {
                if (_syncdelay.PulseEnAll != value)
                {
                    _syncdelay.PulseEnAll = value;
                    _isSyncDelayDirty = true;
                }
            }
        }

        public void PulseEnBit(int bit, bool value)
        {
            
            if (_syncdelay.PulseEnBits[bit] != value)
            {
                _syncdelay.PulseEnBits[bit] = value;
                _isSyncDelayDirty = true;
            }
        }

        public bool SyncInvert
        {
            get { return _syncdelay.Invert; }
            set
            {
                if (_syncdelay.Invert != value)
                {
                    _syncdelay.Invert = value;
                    _isSyncDelayDirty = true;
                }
            }
        }

        public double Chan12Delay
        {
            get { return _chandelay.Delay12; }
            set {
                _chandelay.Delay12 = (uint)(value * 100 + 0.5) * 10;
                _isChanDelayDirty = true;
            }
        }

        public double Chan34Delay
        {
            get { return _chandelay.Delay34; }
            set
            {
                _chandelay.Delay34 = (uint)(value * 100 + 0.5) * 10;
                _isChanDelayDirty = true;
            }
        }

        public PowerSettings.Types.AmplitudeControl AmplControl
        {
            get { return _settings.Src; }
            set
            {
                if (hasIndependentChannels)
                    // Only relevant to Synths with muxed controls
                    return;

                if (_settings.Src != value)
                {
                    _settings.Src = value;
                    if (thisIMS != null)
                    {
                        var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                        client.dds_power(_settings);
                    }
                }
            }
        }

        public bool Chan1Source
        {
            get { return _chanSettings[0].Src == ChannelPowerSettings.Types.PowerControl.Internal; }
            set
            {
                _chanSettings[0].Src = value ? ChannelPowerSettings.Types.PowerControl.Internal : ChannelPowerSettings.Types.PowerControl.External;
                _isChannelSettingsDirty[0] = true;
            }
        }

        public bool Chan2Source
        {
            get { return _chanSettings[1].Src == ChannelPowerSettings.Types.PowerControl.Internal; }
            set
            {
                _chanSettings[1].Src = value ? ChannelPowerSettings.Types.PowerControl.Internal : ChannelPowerSettings.Types.PowerControl.External;
                _isChannelSettingsDirty[1] = true;
            }
        }

        public bool Chan3Source
        {
            get { return _chanSettings[2].Src == ChannelPowerSettings.Types.PowerControl.Internal; }
            set
            {
                _chanSettings[2].Src = value ? ChannelPowerSettings.Types.PowerControl.Internal : ChannelPowerSettings.Types.PowerControl.External;
                _isChannelSettingsDirty[2] = true;
            }
        }

        public bool Chan4Source
        {
            get { return _chanSettings[3].Src == ChannelPowerSettings.Types.PowerControl.Internal; }
            set
            {
                _chanSettings[3].Src = value ? ChannelPowerSettings.Types.PowerControl.Internal : ChannelPowerSettings.Types.PowerControl.External;
                _isChannelSettingsDirty[3] = true;
            }
        }

        public bool AmplifierEnable
        {
            get { return _mastersw.AmplifierEn; }
            set
            {
                if (_mastersw.AmplifierEn != value)
                {
                    _mastersw.AmplifierEn = value;
                    if (thisIMS != null)
                    {
                        var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                        client.rf_enable(_mastersw);
                    }
                }
            }
        }

        public bool RF12Enable
        {
            get { return _mastersw.RFChannel12En; }
            set
            {
                if (_mastersw.RFChannel12En != value)
                {
                    _mastersw.RFChannel12En = value;
                    if (thisIMS != null)
                    {
                        var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                        client.rf_enable(_mastersw);
                    }
                }
            }
        }

        public bool RF34Enable
        {
            get { return _mastersw.RFChannel34En; }
            set
            {
                if (_mastersw.RFChannel34En != value)
                {
                    _mastersw.RFChannel34En = value;
                    if (thisIMS != null)
                    {
                        var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                        client.rf_enable(_mastersw);
                    }
                }
            }
        }

        public SyncMapping.Types.SyncSource SyncAnlgA
        {
            get { return _syncmap[0].Src; }
            set
            {
                if (_syncmap[0].Src != value)
                {
                    _syncmap[0].Src = value;
                    if (thisIMS != null)
                    {
                        var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                        client.set_sync_map(_syncmap[0]);
                    }
                }
            }
        }

        public SyncMapping.Types.SyncSource SyncAnlgB
        {
            get { return _syncmap[1].Src; }
            set
            {
                if (_syncmap[1].Src != value)
                {
                    _syncmap[1].Src = value;
                    if (thisIMS != null)
                    {
                        var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                        client.set_sync_map(_syncmap[1]);
                    }
                }
            }
        }

        public SyncMapping.Types.SyncSource SyncDig
        {
            get { return _syncmap[2].Src; }
            set
            {
                if (_syncmap[2].Src != value)
                {
                    _syncmap[2].Src = value;
                    if (thisIMS != null)
                    {
                        var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                        client.set_sync_map(_syncmap[2]);
                    }
                }
            }
        }

        public bool AutoPhaseClear
        {
            get { return _autoPhaseResync; }
            set
            {
                if (_autoPhaseResync != value)
                {
                    _autoPhaseResync = value;
                    if (thisIMS != null)
                    {
                        var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                        var AutoClear = new Google.Protobuf.WellKnownTypes.BoolValue();
                        AutoClear.Value = _autoPhaseResync;
                        client.auto_clear(AutoClear);
                    }
                }
            }
        }

        public void PhaseResync()
        {
            if (thisIMS != null)
            {
                var client = new ImsHwServer.signal_path.signal_pathClient(_channel);
                client.clear_phase(new Google.Protobuf.WellKnownTypes.Empty());
            }

        }
    }
}
