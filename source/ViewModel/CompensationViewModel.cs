using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using iMS;
using System.Windows.Input;
using ImsHwServer;
using Microsoft.Win32;
using System.Windows;
using Grpc.Core;

namespace iMS_Studio.ViewModel
{
    public enum CompensationPlotType
    {
        Amplitude,
        Phase,
        SyncAnlg,
        SyncDig
    }

    public class CompensationViewModel : DockPaneViewModel
    {
        private Channel _channel;
        private ims_system thisIMS;

        // The Model
        private List<iMS.CompensationTable> _compTable = null;

        private double MinFrequency { get; set; }
        private double MaxFrequency { get; set; }
        private int LUTDepth { get; set; }

        private bool _channelComp;
        public bool ChannelComp
        {
            get { return _channelComp; }
            set
            {
                if (value != _channelComp)
                {
                    _channelComp = value;
                    OnPropertyChanged("ChannelComp");
                }
            }
        }

        private int _rfChannels;
        public int RFChannels {
            get { return _rfChannels; }
            set
            {
                if (value != _rfChannels)
                {
                    _rfChannels = value;
                    OnPropertyChanged("RFChannels");
                }
            }
        }

        public CompensationViewModel(ims_system ims, Channel channel)
        {
            _plotType = CompensationPlotType.Amplitude;
            YAxisTitle = "Amplitude (%)";
            PlotTitle = "Amplitude Compensation";
            thisIMS = ims;
            _channel = channel;
            GlobalScope = true;

            MHz lf, uf;
            if (thisIMS != null)
            {
                lf = new MHz(ims.Synth.Cap.LowerFreq);
                uf = new MHz(ims.Synth.Cap.UpperFreq);
                RFChannels = ims.Synth.Cap.Channels;
                LUTDepth = ims.Synth.Cap.LutDepth;
                ChannelComp = ims.Synth.Cap.ChannelComp;
            }
            else {
                lf = new MHz(0.0);
                uf = new MHz(250.0);
                RFChannels = 4;
                LUTDepth = 12;
                ChannelComp = true;
            }
            MinFrequency = lf.Value;
            MaxFrequency = uf.Value;
            _compTable = new List<CompensationTable>(RFChannels);
            for (int i = 1; i <= RFChannels; i++) _compTable.Add(new CompensationTable(LUTDepth, lf, uf));
 
            XYPairing = false;
            ContentId = "CompensationView";

            EnumerateData();
            PlotDataCh4 = PlotDataCh3 = PlotDataCh2 = PlotData;
        }

        private CompositeDataSource _plotData;
        public CompositeDataSource PlotData
        {
            get { return _plotData; }
            set
            {
                if (_plotData != value)
                {
                    _plotData = value;
                    OnPropertyChanged("PlotData");
                }
            }
        }

        private CompositeDataSource _plotDataCh2;
        public CompositeDataSource PlotDataCh2
        {
            get { return _plotDataCh2; }
            set
            {
                if (_plotDataCh2 != value)
                {
                    _plotDataCh2 = value;
                    OnPropertyChanged("PlotDataCh2");
                }
            }
        }

        private CompositeDataSource _plotDataCh3;
        public CompositeDataSource PlotDataCh3
        {
            get { return _plotDataCh3; }
            set
            {
                if (_plotDataCh3 != value)
                {
                    _plotDataCh3 = value;
                    OnPropertyChanged("PlotDataCh3");
                }
            }
        }

        private CompositeDataSource _plotDataCh4;
        public CompositeDataSource PlotDataCh4
        {
            get { return _plotDataCh4; }
            set
            {
                if (_plotDataCh4 != value)
                {
                    _plotDataCh4 = value;
                    OnPropertyChanged("PlotDataCh4");
                }
            }
        }

        private void EnumerateData()
        {
            var _listfreq = new List<double>();
            for (int i = 0; i < (1 << LUTDepth); i++)
            {
                _listfreq.Add(MinFrequency + ((double)i * (MaxFrequency - MinFrequency) / ((1 << LUTDepth) - 1)));
            }
            var _xval = new EnumerableDataSource<double>(_listfreq);
            _xval.SetXMapping(x => x);

            EnumerableDataSource<CompensationPoint> _yval;
            _yval = new EnumerableDataSource<CompensationPoint>(_compTable[0]);
            switch (_plotType)
            {
                case CompensationPlotType.Amplitude: _yval.SetYMapping(y => y.Amplitude.Value); break;
                case CompensationPlotType.Phase: _yval.SetYMapping(y => y.Phase.Value); break;
                case CompensationPlotType.SyncAnlg: _yval.SetYMapping(y => y.SyncAnlg); break;
                case CompensationPlotType.SyncDig: _yval.SetYMapping(y => y.SyncDig); break;
            }

            PlotData = _xval.Join(_yval);

            if (!GlobalScope)
            {
                for (int i = 1; i < RFChannels; i++)
                {
                    _yval = new EnumerableDataSource<CompensationPoint>(_compTable[i]);
                    switch (_plotType)
                    {
                        case CompensationPlotType.Amplitude: _yval.SetYMapping(y => y.Amplitude.Value); break;
                        case CompensationPlotType.Phase: _yval.SetYMapping(y => y.Phase.Value); break;
                        case CompensationPlotType.SyncAnlg: _yval.SetYMapping(y => y.SyncAnlg); break;
                        case CompensationPlotType.SyncDig: _yval.SetYMapping(y => y.SyncDig); break;
                    }

                    switch(i)
                    {
                        case 1:
                            PlotDataCh2 = _xval.Join(_yval);
                            break;
                        case 2:
                            PlotDataCh3 = _xval.Join(_yval);
                            break;
                        case 3:
                            PlotDataCh4 = _xval.Join(_yval);
                            break;
                    }
                }
            }
        }

        private CompensationPlotType _plotType;
        public CompensationPlotType PlotType
        {
            get { return _plotType; }
            set
            {
                if (value != _plotType)
                {
                    _plotType = value;
                    EnumerateData();
                    switch (_plotType)
                    {
                        case CompensationPlotType.Amplitude: PlotTitle = "Amplitude Compensation"; break;
                        case CompensationPlotType.Phase: PlotTitle = "Phase Steering"; break;
                        case CompensationPlotType.SyncAnlg: PlotTitle = "Synchronous Analog Lookup"; break;
                        case CompensationPlotType.SyncDig: PlotTitle = "Synchronous Digital Lookup"; break;
                    }
                    switch (_plotType)
                    {
                        case CompensationPlotType.Amplitude: YAxisTitle = "Amplitude (%)"; break;
                        case CompensationPlotType.Phase: YAxisTitle = "Phase (deg)"; break;
                        case CompensationPlotType.SyncAnlg: YAxisTitle = "Normalized (0 - 1.0)"; break;
                        case CompensationPlotType.SyncDig: YAxisTitle = "Decimal bus value"; break;
                    }
                    
                    OnPropertyChanged("PlotType");
                }
            }
        }

        private string _yaxisTitle;
        public string YAxisTitle
        {
            get { return _yaxisTitle; }
            set
            {
                if (value != _yaxisTitle)
                {
                    _yaxisTitle = value;
                    OnPropertyChanged("YAxisTitle");
                }
            }
        }

        private string _plotTitle;
        public string PlotTitle
        {
            get { return _plotTitle; }
            set
            {
                if (value != _plotTitle)
                {
                    _plotTitle = value;
                    OnPropertyChanged("PlotTitle");
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

        private bool _globalScope;
        public bool GlobalScope
        {
            get { return _globalScope; }
            set
            {
                if (value != _globalScope)
                {
                    _globalScope = value;
                    OnPropertyChanged("GlobalScope");
                }
            }
        }

        public void ApplyGlobalFunc(CompensationFunction func, CompensationFeature feat, CompensationModifier modifier)
        {
            _compTable[0].ApplyFunction(func, feat, modifier);
            switch (feat)
            {
                case CompensationFeature.AMPLITUDE: PlotType = CompensationPlotType.Amplitude; break;
                case CompensationFeature.PHASE: PlotType = CompensationPlotType.Phase; break;
                case CompensationFeature.SYNC_ANLG: PlotType = CompensationPlotType.SyncAnlg; break;
                case CompensationFeature.SYNC_DIG: PlotType = CompensationPlotType.SyncDig; break;
            }
            EnumerateData();
        }

        public void ApplyChannelFunc(RFChannel chan, CompensationFunction func, CompensationFeature feat, CompensationModifier modifier)
        {
            if (chan.IsAll())
            {
                _compTable[0].ApplyFunction(func, feat, modifier);
            }
            else
            {
                _compTable[chan.__val__() - 1].ApplyFunction(func, feat, modifier);
            }
            switch (feat)
            {
                case CompensationFeature.AMPLITUDE: PlotType = CompensationPlotType.Amplitude; break;
                case CompensationFeature.PHASE: PlotType = CompensationPlotType.Phase; break;
                case CompensationFeature.SYNC_ANLG: PlotType = CompensationPlotType.SyncAnlg; break;
                case CompensationFeature.SYNC_DIG: PlotType = CompensationPlotType.SyncDig; break;
            }
            EnumerateData();
        }

        private ICommand _importComp;
        public ICommand ImportComp
        {
            get
            {
                if (_importComp == null)
                {
                    _importComp = new RelayCommand(param => ImportComp_Execute());
                }
                return _importComp;
            }
        }

        private ICommand _exportComp;
        public ICommand ExportComp
        {
            get
            {
                if (_exportComp == null)
                {
                    _exportComp = new RelayCommand(param => ExportComp_Execute());
                }
                return _exportComp;
            }
        }

        private ICommand _downloadComp;
        public ICommand DownloadComp
        {
            get
            {
                if (_downloadComp == null)
                {
                    _downloadComp = new RelayCommand(param => DownloadComp_Execute(), valid => DownloadComp_CanExecute());
                }
                return _downloadComp;
            }
        }

        private string _openFolder;
        public void ImportComp_Execute()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Isomet Compensation File|*.lut|All Files|*.*";
            if (_openFolder != null)
            {
                openFileDialog.InitialDirectory = _openFolder;
            }
            if (openFileDialog.ShowDialog() == true)
            {
                using (new WaitCursor())
                {
                    var cti = new CompensationTableImporter(openFileDialog.FileName);
                    if (cti.IsValid() && cti.IsGlobal())
                    {
                        var ct = cti.RetrieveGlobalLUT();
                        // Resize table to match view model CT parameters
                        _compTable[0] = new CompensationTable(LUTDepth, new MHz(MinFrequency), new MHz(MaxFrequency), ct);
                    }
                    else
                    {
                        for (int i = 1; i <= RFChannels; i++)
                        {
                            var ct = cti.RetrieveChannelLUT(new RFChannel(i));
                            _compTable[i - 1] = new CompensationTable(LUTDepth, new MHz(MinFrequency), new MHz(MaxFrequency), ct);
                        }
                    }
                    EnumerateData();
                }
            }
            _openFolder = openFileDialog.FileName;
        }

        private string _saveFolder;
        public void ExportComp_Execute()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Isomet Compensation File|*.lut|All Files|*.*";
            if (_saveFolder != null)
            {
                saveFileDialog.InitialDirectory = _saveFolder;
            }
            if (saveFileDialog.ShowDialog() == true)
            {
                using (new WaitCursor())
                {
                    var cte = new CompensationTableExporter(RFChannels);
                    if (GlobalScope)
                    {
                        cte.ProvideGlobalTable(_compTable[0]);
                        cte.ExportGlobalLUT(saveFileDialog.FileName);
                    } else
                    {
                        for (int i = 1; i <= RFChannels; i++)
                        {
                            cte.ProvideChannelTable(new RFChannel(i), _compTable[i-1]);
                        }
                        cte.ExportChannelLUT(saveFileDialog.FileName);
                    }
                }
            }
            _saveFolder = saveFileDialog.FileName;
        }

        async void DownloadComp_Execute()
        {
            using (new WaitCursor())    
            {
                int chan = (GlobalScope) ? RFChannel.all : RFChannel.min;
                do
                {
                    // Create Download Buffer on server
                    var CompDL = new compensation_downloader.compensation_downloaderClient(_channel);
                    compensation_header hdr = new compensation_header();
                    hdr.NPts = (uint)(1 << LUTDepth);
                    hdr.IsXY = XYPairing;
                    DownloadHandle handle = CompDL.create(hdr);

                    // Add compensation points
                    using (var call = CompDL.add())
                    {
                        iMS.CompensationTable tbl;
                        if (GlobalScope)
                        {
                            tbl = _compTable[0];
                        } else
                        {
                            tbl = _compTable[chan - RFChannel.min];
                        }
                        foreach (var pt in tbl)
                        {
                            compensation_point point = new compensation_point();
                            point.Context = handle;
                            point.Amplitude = pt.Amplitude.Value;
                            point.Phase = pt.Phase.Value;
                            point.SyncAnlg = pt.SyncAnlg;
                            point.SyncDig = pt.SyncDig;
                            await call.RequestStream.WriteAsync(point);
                        }
                        await call.RequestStream.CompleteAsync();

                        Google.Protobuf.WellKnownTypes.Empty empty = await call.ResponseAsync;
                    }

                    // Download to HW
                    compensation_download comp_dl = new compensation_download();
                    comp_dl.Context = handle;
                    comp_dl.Channel = (uint)chan;
                    DownloadStatus dlstat = CompDL.download(comp_dl);

                    // Wait until finished
                    while (dlstat.Status == DownloadStatus.Types.DLStatus.Downloading)
                    {
                        System.Threading.Thread.Sleep(50);
                        dlstat = CompDL.dlstatus(handle);
                    }

                    if (dlstat.Status != DownloadStatus.Types.DLStatus.DlFinished)
                    {
                        MessageBoxResult result = MessageBox.Show("Unable to Download Compensation Table", "Download Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                } while (chan++ < RFChannel.max);
            }
        }

        private bool DownloadComp_CanExecute()
        {
            if (thisIMS != null)
                return true;
            return false;
        }

        public override void Reset()
        {
           // throw new NotImplementedException();
        }
    }
}
