using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMS;
using System.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;

namespace iMS_Studio.ViewModel
{
    public enum ImageRepeatsVM
    {
        [Description("Repeat Forever")]
        FOREVER,
        [Description("No Repeats")]
        NONE,
        [Description("Programmed")]
        PROGRAM
    };
    public enum FreqResolutionVM
    {
        [Description("16 bits")]
        BITS16 = 2,
        [Description("24 bits")]
        BITS24 = 3,
        [Description("32 bits")]
        BITS32 = 4
    };

    public class PlayConfigurationViewModel : DockPaneViewModel
    {
        private iMS.ImagePlayer.PlayConfiguration _cfg;
        public PlayConfigurationViewModel(ImagePlayer.PlayConfiguration cfg)
        {
            this._cfg = cfg;
            NumRepeats = 1;
            RepeatsType = ImageRepeatsVM.NONE;
            SupportedResolutions = new ObservableCollection<FreqResolutionVM> { FreqResolutionVM.BITS16 };
            FreqResolution = FreqResolutionVM.BITS16;
            _postDelay = 0.0;
            _phaseComp = true;
            _amplComp = true;

            ContentId = "PlayConfiguration";
        }

        #region ClockPolarity
        public Polarity ClockPolarity
        {
            get { return _cfg.clk_pol; }
            set
            {
                if (value != _cfg.clk_pol)
                {
                    _cfg.clk_pol = value;
                    OnPropertyChanged("ClockPolarity");
                }
            }
        }
        #endregion 

        #region TriggerPolarity
        public Polarity TriggerPolarity
        {
            get { return _cfg.trig_pol; }
            set
            {
                if (value != _cfg.trig_pol)
                {
                    _cfg.trig_pol = value;
                    OnPropertyChanged("TriggerPolarity");
                }
            }
        }
        #endregion

        #region ClockSource
        public ImagePlayer.PointClock ClockSource
        {
            get { return _cfg.int_ext; }
            set
            {
                if (value != _cfg.int_ext)
                {
                    _cfg.int_ext = value;
                    OnPropertyChanged("ClockSource");
                }
            }
        }
        #endregion 

        #region TriggerType
        public ImagePlayer.ImageTrigger TriggerType
        {
            get { return _cfg.trig; }
            set
            {
                if (value != _cfg.trig)
                {
                    _cfg.trig = value;
                    OnPropertyChanged("TriggerType");
                }
            }
        }
        #endregion 

        #region RepeatsType
        private ImageRepeatsVM _imgRepeats;
        public ImageRepeatsVM RepeatsType
        {
            get { return _imgRepeats; }
            set
            {
                if (value != _imgRepeats)
                {
                    _imgRepeats = value;
                    switch (value)
                    {
                        case ImageRepeatsVM.FOREVER: _cfg.rpts = ImageRepeats.FOREVER; break;
                        case ImageRepeatsVM.NONE: _cfg.rpts = ImageRepeats.NONE; break;
                        case ImageRepeatsVM.PROGRAM: _cfg.rpts = ImageRepeats.PROGRAM; break;
                    }
                    OnPropertyChanged("RepeatsType");
                }
            }
        }
        #endregion 

        #region NumRepeats
        private int _numRepeats;
        public int NumRepeats
        {
            get { return _numRepeats; }
            set
            {
                if (value != _numRepeats)
                {
                    _numRepeats = value;
                    _cfg.n_rpts = value-1;
                    OnPropertyChanged("NumRepeats");
                }
            }
        }
        #endregion 

        #region PostDelay
        private double _postDelay;
        public double PostDelay
        {
            get { return _postDelay; }
            set
            {
                if (value != _postDelay)
                {
                    _postDelay = value;
                    //                    _cfg.del = new TimeSpan((long)(1000.0 * value));
                    Int64 ticksPerMS = (TimeSpan.TicksPerSecond / 1000);
                    _cfg.del = TimeSpan.FromTicks((Int64)(value * ticksPerMS));
                    OnPropertyChanged("PostDelay");
                }
            }
        }
        #endregion

        #region AmplCompEnabled
        private bool _amplComp;
        public bool AmplCompEnabled
        {
            get { return _amplComp; }
            set
            {
                if (_amplComp != value)
                {
                    _amplComp = value;
                    OnPropertyChanged("AmplCompEnabled");
                }
            }
        }
        #endregion

        #region PhaseCompEnabled
        private bool _phaseComp;
        public bool PhaseCompEnabled
        {
            get { return _phaseComp; }
            set
            {
                if (_phaseComp != value)
                {
                    _phaseComp = value;
                    OnPropertyChanged("PhaseCompEnabled");
                }
            }
        }
        #endregion  

        private ObservableCollection<FreqResolutionVM> _supportedResolutions;
        public ObservableCollection<FreqResolutionVM> SupportedResolutions
        {
            get { return _supportedResolutions; }
            set
            {
                _supportedResolutions = value;
//                OnPropertyChanged("SupportedResolutions");
            }
        }

        private FreqResolutionVM _freqResolution;
        public FreqResolutionVM FreqResolution
        {
            get { return _freqResolution; }
            set
            {
                if (_freqResolution != value)
                {
                    _freqResolution = value;
                    OnPropertyChanged("FreqResolution");
                }
            }
        }


        public override void Reset()
        {
            //throw new NotImplementedException();
        }
    }
}
