using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// nicked from:
// https://stackoverflow.com/questions/3801681/good-or-bad-practice-for-dialogs-in-wpf-with-mvvm

namespace iMS_Studio.ViewModel.Behaviour
{
    public class NewCompensationFunctionEventArgs : EventArgs
    {
        public iMS.CompensationFunction CompFunc { get; set; }
        public NewCompensationFunctionEventArgs(iMS.CompensationFunction cfunc)
        {
            this.CompFunc = new iMS.CompensationFunction(cfunc);
        }
    }

    public class RequestCloseDialogEventArgs : EventArgs
    {
        public bool DialogResult { get; set; }
        public RequestCloseDialogEventArgs(bool dialogresult)
        {
            this.DialogResult = dialogresult;
        }
    }

    public interface IDialogResultVMHelper
    {
        event EventHandler<RequestCloseDialogEventArgs> RequestCloseDialog;
    }
}
