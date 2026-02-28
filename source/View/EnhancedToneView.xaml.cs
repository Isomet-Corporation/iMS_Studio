using ImsHwServer;
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
    /// Interaction logic for EnhancedToneView.xaml
    /// </summary>
    public partial class EnhancedToneView : UserControl
    {
        public EnhancedToneView()
        {
            InitializeComponent();

            Ch1ModeBox.ItemsSource = Enum.GetValues(typeof(iMS_Studio.ViewModel.EnhancedToneVM)).Cast<iMS_Studio.ViewModel.EnhancedToneVM>();
            Ch2ModeBox.ItemsSource = Enum.GetValues(typeof(iMS_Studio.ViewModel.EnhancedToneVM)).Cast<iMS_Studio.ViewModel.EnhancedToneVM>();
            Ch3ModeBox.ItemsSource = Enum.GetValues(typeof(iMS_Studio.ViewModel.EnhancedToneVM)).Cast<iMS_Studio.ViewModel.EnhancedToneVM>();
            Ch4ModeBox.ItemsSource = Enum.GetValues(typeof(iMS_Studio.ViewModel.EnhancedToneVM)).Cast<iMS_Studio.ViewModel.EnhancedToneVM>();

        }
    }
}
