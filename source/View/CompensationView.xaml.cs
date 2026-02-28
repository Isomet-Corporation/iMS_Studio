using Microsoft.Research.DynamicDataDisplay.Charts;
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
    /// Interaction logic for CompensationView.xaml
    /// </summary>
    public partial class CompensationView : UserControl
    {
        public CompensationView()
        {
            InitializeComponent();
         }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            compPlotter.Viewport.FitToView();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                compPlotter.Legend.Visibility = Visibility.Hidden;
            } else
            {
                compPlotter.Legend.Visibility = Visibility.Visible;
            }
        }

        private void CompPlotter_Loaded(object sender, RoutedEventArgs e)
        {
            // Initial state
            compPlotter.Legend.Visibility = Visibility.Hidden;
        }
    }
}
