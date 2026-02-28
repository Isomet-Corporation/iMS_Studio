using Grpc.Core;
using iMS;
using ImsHwServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iMS_Studio.Model
{
    public class SystemFuncModel
    {
        private Channel _channel;
        private ims_system thisIMS;

        private ClockGenerator _clockGenerator;
        private bool _clockGenEnabled;

        private bool _isClockGenDirty;
        private Timer _timer;

        public SystemFuncModel(Channel channel, ims_system ims)
        {
            _channel = channel;
            thisIMS = ims;

            _isClockGenDirty = false;

            _clockGenerator = new ClockGenerator()
            {
                AlwaysOn = true,
                ClockFreq = 100.0,
                ClockPolarity = false,
                TrigPolarity = false,
                DutyCycle = 50.0,
                OscPhase = 0.0,
                GenerateTrigger = false
            };
            _clockGenEnabled = false;

            if (thisIMS != null)
            {
                var client = new ImsHwServer.sys_func.sys_funcClient(_channel);

                client.dis_clock_gen(new Google.Protobuf.WellKnownTypes.Empty());
            }

            _timer = new Timer(OnTimer, null, TimeSpan.Zero, new TimeSpan(0, 0, 0, 0, 100));
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
            if (_isClockGenDirty && _clockGenEnabled)
            {
                StopTimer();
                updateHW(_isClockGenDirty);
                _isClockGenDirty = false;
                StartTimer();
            }
        }

        private void updateHW(bool clockGen)
        {
            if (thisIMS != null)
            {
                var client = new ImsHwServer.sys_func.sys_funcClient(_channel);
                if (clockGen)
                {
                    if (_clockGenEnabled)
                    {
                        client.en_clock_gen(_clockGenerator);
                    }
                    else
                    {
                        client.dis_clock_gen(new Google.Protobuf.WellKnownTypes.Empty());
                    }
                }
            }
        }

        public bool ClockGenEnabled
        {
            get { return _clockGenEnabled; }
            set
            {
                if (value != _clockGenEnabled)
                {
                    _clockGenEnabled = value;
                    if (_clockGenEnabled)
                    {
                        StartTimer();
                    }
                    else
                    {
                        StopTimer();
                    }
                    updateHW(true);
                    _isClockGenDirty = false;
                }
            }
        }

        public double ClockFreq
        {
            get { return _clockGenerator.ClockFreq; }
            set
            {
                if (_clockGenerator.ClockFreq != value)
                {
                    _clockGenerator.ClockFreq = value;
                    _isClockGenDirty = true;
                }
            }
        }

        public double OscPhase
        {
            get { return _clockGenerator.OscPhase; }
            set
            {
                if (_clockGenerator.OscPhase != value)
                {
                    _clockGenerator.OscPhase = value;
                    _isClockGenDirty = true;
                }
            }
        }

        public double DutyCycle
        {
            get {  return (double)_clockGenerator.DutyCycle;}
            set
            {
                if (_clockGenerator.DutyCycle != value)
                {
                    _clockGenerator.DutyCycle = value;
                    _isClockGenDirty = true;
                }
            }
        }

        public bool AlwaysOn
        {
            get { return _clockGenerator.AlwaysOn; }
            set
            {
                if (_clockGenerator.AlwaysOn != value)
                {
                    _clockGenerator.AlwaysOn = value;
                    _isClockGenDirty = true;
                }
            }
        }

        public bool GenerateTrigger
        {
            get { return _clockGenerator.GenerateTrigger; }
            set
            {
                if (_clockGenerator.GenerateTrigger != value)
                {
                    _clockGenerator.GenerateTrigger = value;
                    _isClockGenDirty = true;
                }
            }
        }

        public bool ClockPolarity
        {
            get { return _clockGenerator.ClockPolarity; }
            set
            {
                if (_clockGenerator.ClockPolarity != value)
                {
                    _clockGenerator.ClockPolarity = value;
                    _isClockGenDirty = true;
                }
            }
        }

        public bool TrigPolarity
        {
            get { return (_clockGenerator.TrigPolarity); }
            set
            {
                if (_clockGenerator.TrigPolarity != value)
                {
                    _clockGenerator.TrigPolarity = value;
                    _isClockGenDirty = true;
                }
            }
        }

        public double MinImageRate
        {
            get
            {
                return 0.02;
            }
        }

        public double MaxImageRate
        {
            get
            {
                if (thisIMS != null)
                {
                    return thisIMS.Ctlr.Cap.MaxImgRate / 1000.0;
                }
                else
                {
                    return 1600.0;
                }
            }
        }

    }
}
