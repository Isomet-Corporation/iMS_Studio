using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// nicked from:
// https://stackoverflow.com/questions/3801681/good-or-bad-practice-for-dialogs-in-wpf-with-mvvm

namespace iMS_Studio
{
    public interface IUIWindowDialogService
    {
        bool? ShowDialog(string title, object datacontext);
    }

    public class WpfUIWindowDialogService : IUIWindowDialogService
    {
        public bool? ShowDialog(string title, object datacontext)
        {
            var win = new WindowDialog();
            win.Title = title;
            win.DataContext = datacontext;

            return win.ShowDialog();
        }
    }
}
