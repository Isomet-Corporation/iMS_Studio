using Grpc.Core;
using iMS_Studio.ViewModel;
using ImsHwServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace iMS_Studio.Model
{
    public class ToneBufferModel
    {
        private Channel _channel;
        private ims_system thisIMS;
        private ICommand stopImage;

        private bool _IsEnabled;
        public bool IsEnabled {
            get { return _IsEnabled; }
            set {
                if (!value)
                {
                    this.StopTonePlayback();
                }
                else
                {
                    this.StartTonePlayback();
                }
                _IsEnabled = value;
            }
        }

        public ToneBufferModel(Channel channel, ims_system ims, ICommand StopFunc)
        {
            _channel = channel;
            thisIMS = ims;
            stopImage = StopFunc;
            _IsEnabled = false;

            _amplComp = iMS.SignalPath.Compensation.ACTIVE;
            _phaseComp = iMS.SignalPath.Compensation.ACTIVE;
            _control = iMS.SignalPath.ToneBufferControl.HOST;
        }

        public async void DownloadFullBuffer(ToneBufferDockWindowViewModel ToneBufferVM)
        {
            if (thisIMS != null && !IsEnabled)
            {
                using (new WaitCursor())
                {
                    var client = new ImsHwServer.tonebuffer_downloader.tonebuffer_downloaderClient(_channel);
                    tonebuffer_header hdr = new tonebuffer_header();
                    hdr.First = 0;
                    hdr.Last = 255;
                    DownloadHandle handle = client.create(hdr);

                    // Add tone buffer entries
                    using (var call = client.add())
                    {
                        foreach (var tbe in ToneBufferVM.ToneData)
                        {
                            tonebuffer_entry entry = new tonebuffer_entry();
                            entry.Context = handle;
                            entry.FreqCh1 = tbe.FreqCh1; entry.FreqCh2 = tbe.FreqCh2; entry.FreqCh3 = tbe.FreqCh3; entry.FreqCh4 = tbe.FreqCh4;
                            entry.AmplCh1 = tbe.AmplCh1; entry.AmplCh2 = tbe.AmplCh2; entry.AmplCh3 = tbe.AmplCh3; entry.AmplCh4 = tbe.AmplCh4;
                            entry.PhsCh1 = tbe.PhaseCh1; entry.PhsCh2 = tbe.PhaseCh2; entry.PhsCh3 = tbe.PhaseCh3; entry.PhsCh4 = tbe.PhaseCh4;
                            await call.RequestStream.WriteAsync(entry);
                        }
                        await call.RequestStream.CompleteAsync();

                        Google.Protobuf.WellKnownTypes.Empty empty = await call.ResponseAsync;
                    }

                    // Download to HW
                    DownloadStatus dlstat = client.download(handle);

                    // Wait until finished 
                    System.Threading.SpinWait.SpinUntil(() => client.dlstatus(handle).Status != DownloadStatus.Types.DLStatus.Downloading, TimeSpan.FromSeconds(15));
                    /*int timeout = 0;
                    while (dlstat.Status == DownloadStatus.Types.DLStatus.Downloading)
                    {
                        dlstat = client.dlstatus(handle);
                        System.Threading.Thread.Sleep(new TimeSpan(0, 0, 1));
                        if (timeout++ > 15) break;
                    }*/

                    dlstat = client.dlstatus(handle);
                    if (dlstat.Status == DownloadStatus.Types.DLStatus.DlError)
                    {
                        MessageBox.Show("Error Downloading Tone Buffer!", "Download Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        stopImage.Execute(null);
                    }
                    else if (dlstat.Status == DownloadStatus.Types.DLStatus.Downloading)
                    {
                        MessageBox.Show("Timed Out Downloading Tone Buffer!", "Download Timeout", MessageBoxButton.OK, MessageBoxImage.Error);
                        stopImage.Execute(null);
                    }
                    else
                    {
                        this.IsEnabled = true;
                    }
                }
            }
        }

        public async void DownloadSingleEntry(ToneBufferDockWindowViewModel ToneBufferVM, int index)
        {
            if (thisIMS != null)
            {
                using (new WaitCursor())
                {
                    var client = new ImsHwServer.tonebuffer_downloader.tonebuffer_downloaderClient(_channel);
                    tonebuffer_header hdr = new tonebuffer_header();
                    hdr.First = (uint)index;
                    hdr.Last = (uint)index;
                    DownloadHandle handle = client.create(hdr);

                    // Add tone buffer entries
                    using (var call = client.add())
                    {
                        var tbe = ToneBufferVM.ToneData[index];
                        tonebuffer_entry entry = new tonebuffer_entry();
                        entry.Context = handle;
                        entry.FreqCh1 = tbe.FreqCh1; entry.FreqCh2 = tbe.FreqCh2; entry.FreqCh3 = tbe.FreqCh3; entry.FreqCh4 = tbe.FreqCh4;
                        entry.AmplCh1 = tbe.AmplCh1; entry.AmplCh2 = tbe.AmplCh2; entry.AmplCh3 = tbe.AmplCh3; entry.AmplCh4 = tbe.AmplCh4;
                        entry.PhsCh1 = tbe.PhaseCh1; entry.PhsCh2 = tbe.PhaseCh2; entry.PhsCh3 = tbe.PhaseCh3; entry.PhsCh4 = tbe.PhaseCh4;
                        await call.RequestStream.WriteAsync(entry);

                        await call.RequestStream.CompleteAsync();

                        Google.Protobuf.WellKnownTypes.Empty empty = await call.ResponseAsync;
                    }

                    // Download to HW
                    DownloadStatus dlstat = client.download(handle);

                    // Wait until finished 
                    while (dlstat.Status == DownloadStatus.Types.DLStatus.Downloading)
                    {
                        dlstat = client.dlstatus(handle);
                    }

                    if (dlstat.Status == DownloadStatus.Types.DLStatus.DlError)
                    {
                        //MessageBox.Show("Error Downloading Tone Entry!", "Download Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private iMS.SignalPath.Compensation _amplComp;
        public iMS.SignalPath.Compensation AmplComp
        {
            get { return _amplComp; }
            set { _amplComp = value; }
        }

        private iMS.SignalPath.Compensation _phaseComp;
        public iMS.SignalPath.Compensation PhaseComp
        {
            get { return _phaseComp; }
            set { _phaseComp = value; }
        }

        private iMS.SignalPath.ToneBufferControl _control;
        public iMS.SignalPath.ToneBufferControl TBufControl
        {
            get { return _control; }
            set { _control = value; }
        }

        public void StartTonePlayback()
        {
            if (!_IsEnabled)
            {
                var client = new ImsHwServer.tonebuffer_manager.tonebuffer_managerClient(_channel);
                tonebuffer_params tparam = new tonebuffer_params();
                tparam.AmplitudeCompensationEnabled = (AmplComp == iMS.SignalPath.Compensation.ACTIVE);
                tparam.PhaseCompensationEnabled = (PhaseComp == iMS.SignalPath.Compensation.ACTIVE);
                switch (TBufControl)
                {
                    case iMS.SignalPath.ToneBufferControl.HOST: tparam.Control = tonebuffer_params.Types.tonebuffer_control_type.Host; break;
                    case iMS.SignalPath.ToneBufferControl.EXTERNAL: tparam.Control = tonebuffer_params.Types.tonebuffer_control_type.External; break;
                    case iMS.SignalPath.ToneBufferControl.EXTERNAL_EXTENDED: tparam.Control = tonebuffer_params.Types.tonebuffer_control_type.ExternalExtended; break;
                }
                Google.Protobuf.WellKnownTypes.Empty empty = client.start(tparam);
            }
        }

        public void SelectToneIndex(int index)
        {
            if (_IsEnabled)
            {
                var client = new ImsHwServer.tonebuffer_manager.tonebuffer_managerClient(_channel);
                tonebuffer_index tbuf_idx = new tonebuffer_index();
                tbuf_idx.Index = (uint)index;
                Google.Protobuf.WellKnownTypes.Empty empty = client.select(tbuf_idx);
            }
        }

        public void StopTonePlayback()
        {
            if (_IsEnabled)
            {
                var client = new ImsHwServer.tonebuffer_manager.tonebuffer_managerClient(_channel);
                Google.Protobuf.WellKnownTypes.Empty empty = client.stop(new Google.Protobuf.WellKnownTypes.Empty());
            }
        }
    }
}

