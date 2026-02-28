using iMS_Studio.ViewModel;
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
    /// Interaction logic for PlayConfigurationView.xaml
    /// </summary>
    public partial class PlayConfigurationView : UserControl
    {
        public PlayConfigurationView()
        {
            InitializeComponent();
            ImgRpts.ItemsSource = Enum.GetValues(typeof(iMS_Studio.ViewModel.ImageRepeatsVM)).Cast<iMS_Studio.ViewModel.ImageRepeatsVM>();
            //FreqRes.ItemsSource = Enum.GetValues(typeof(iMS_Studio.ViewModel.FreqResolutionVM)).Cast<iMS_Studio.ViewModel.FreqResolutionVM>();
        }
    }
}
