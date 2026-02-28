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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iMS_Studio.View
{
    /// <summary>
    /// Interaction logic for HWServerConsole.xaml
    /// </summary>
    public partial class HWServerConsole : UserControl
    {
        public HWServerConsole()
        {
            InitializeComponent();
        }

        private void console_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox).ScrollToEnd();
        }

        private void console_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Home)
                (sender as TextBox).ScrollToHome();
            else if (e.Key == Key.End)
                (sender as TextBox).ScrollToEnd();
            e.Handled = true;
        }
    }
}
