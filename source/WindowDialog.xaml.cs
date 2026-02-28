using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using iMS_Studio.ViewModel.Behaviour;

namespace iMS_Studio
{
    /// <summary>
    /// Interaction logic for WindowDialog.xaml
    /// </summary>
    public partial class WindowDialog : Window
    {
        // Note: If the window is closed, it has no DialogResult
        private bool _isClosed = false;

        public WindowDialog()
        {
            InitializeComponent();
            this.DialogPresenter.DataContextChanged += DialogPresenterDataContextChanged;
            this.Closed += DialogWindowClosed;
        }

        void DialogWindowClosed(object sender, EventArgs e)
        {
            this._isClosed = true;
        }

        private void DialogPresenterDataContextChanged(object sender,
                                   DependencyPropertyChangedEventArgs e)
        {
            var d = e.NewValue as IDialogResultVMHelper;

            if (d == null)
                return;

            d.RequestCloseDialog += new EventHandler<RequestCloseDialogEventArgs>
                                        (DialogResultTrueEvent).MakeWeak(
                                            eh => d.RequestCloseDialog -= eh);
        }

        private void DialogResultTrueEvent(object sender,
                                  RequestCloseDialogEventArgs eventargs)
        {
            // Important: Do not set DialogResult for a closed window
            // GC clears windows anyways and with MakeWeak it
            // closes out with IDialogResultVMHelper
            if (_isClosed) return;

            this.DialogResult = eventargs.DialogResult;
        }
    }
}
