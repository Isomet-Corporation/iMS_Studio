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
using Xceed.Wpf.DataGrid;

namespace iMS_Studio.View
{
    /// <summary>
    /// Interaction logic for SequenceDockWindowView.xaml
    /// </summary>
    public partial class SequenceDockWindowView : UserControl
    {
        //private bool ControlKeyPressed = false;
        //private bool ShiftKeyPressed = false;

        public SequenceDockWindowView()
        {
            InitializeComponent();
            SequenceEntries.ClipboardExporters["CSV"].IncludeColumnHeaders = false;
            SequenceEntries.ClipboardExporters["Text"].IncludeColumnHeaders = true;
        }

        private void TermActionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((iMS.SequenceTermAction)((sender as ComboBox).SelectedValue) == iMS.SequenceTermAction.REPEAT_FROM)
            {
                TermValueSelector.IsEnabled = true;
            } else
            {
                TermValueSelector.IsEnabled = false;
            }

        }

        private void SequenceEntries_KeyDown(object sender, KeyEventArgs e)
        {
            bool isCharOrDigit =
         (e.Key >= Key.A && e.Key <= Key.Z ||
         e.Key >= Key.D0 && e.Key <= Key.D9 ||
         e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 ||
         e.Key == Key.OemPeriod);

            var grid = (sender as DataGridControl);
            if (isCharOrDigit && !grid.IsBeingEdited)
            {
                grid.BeginEdit();
            }

        }

        private void ColumnHeader_PreviewMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void SequenceEntries_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}
