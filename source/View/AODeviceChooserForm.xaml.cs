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

namespace iMS_Studio.View
{
    /// <summary>
    /// Interaction logic for AODeviceChooserForm.xaml
    /// </summary>
    public partial class AODeviceChooserForm : UserControl
    {
        public AODeviceChooserForm()
        {
            InitializeComponent();

            xtalComboBox.ItemsSource = Enum.GetValues(typeof(iMS_Studio.ViewModel.CrystalTypesVM)).Cast<iMS_Studio.ViewModel.CrystalTypesVM>();

            int i = 0;
            foreach (var item in AODeviceList.Items)
            {
                (item as ListViewItem).Background = ((i % 2) == 1) ? Brushes.DarkGray : Brushes.LightGray;
                i++;
            }

        }

        private void customParameters_Click(object sender, RoutedEventArgs e)
        {
            this.AODeviceList.SelectedItem = null;
        }
    }
}
