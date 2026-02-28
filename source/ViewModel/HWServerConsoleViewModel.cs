using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMS_Studio.ViewModel
{
    public class HWServerConsoleViewModel : DockPaneViewModel
    {
        public HWServerConsoleViewModel()
        {
            ContentId = "HWServerConsole";
        }

        private string _consoleText;
        public string ConsoleText
        {  get { return _consoleText; }
            set
            {
                _consoleText = value;
                OnPropertyChanged("ConsoleText");
            }
        }

        public override void Reset()
        {

        }
    }
}
