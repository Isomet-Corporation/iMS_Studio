using iMS_Studio.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for ToneBufferDockWindowView.xaml
    /// </summary>
    public partial class ToneBufferDockWindowView : UserControl
    {
        private bool ControlKeyPressed = false;
        private bool ShiftKeyPressed = false;

        public ToneBufferDockWindowView()
        {
            InitializeComponent();
            ToneEntries.ClipboardExporters["CSV"].IncludeColumnHeaders = false;
            ToneEntries.ClipboardExporters["Text"].IncludeColumnHeaders = true;
        }
        private void IndexValue_QueryValue(object sender, Xceed.Wpf.DataGrid.DataGridItemPropertyQueryValueEventArgs e)
        {
            ToneBufferEntryVM row = e.Item as ToneBufferEntryVM;
            var collectionViewSource = this.grid.FindResource("tone_data") as DataGridCollectionViewSource;

            {
                try
                {
                    int currentRowIndex = ToneEntries.Items.IndexOf(row) + 1;
                    if (row != null)
                    {
                        e.Value = currentRowIndex - 1;
                    }
                }
                catch (InvalidOperationException)
                {
                    // Disable this exception which occurs when editing a cell
                }
            }
        }

        private void TextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var box = sender as TextBox;
            if ((bool)e.NewValue && box != null)
            {
                box.Focus();
                box.SelectAll();
            }

        }

        private void TextBox_Initialized(object sender, EventArgs e)
        {
            var box = sender as TextBox;
            if (box != null)
            {
                box.Focus();
                box.SelectAll();
            }
        }

        // Enter key commits selection and selects the next box down
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ToneEntries.EndEdit();


                // Paste Value to all Selected Cells
                var text = (sender as TextBox).Text;
                foreach (SelectionCellRange range in ToneEntries.SelectedCellRanges)
                {
                    if (!range.IsEmpty)
                    {
                        int startColumn = (range.ColumnRange.StartIndex);
                        int startRow = (range.ItemRange.StartIndex);
                        int endColumn = (range.ColumnRange.EndIndex);
                        int endRow = (range.ItemRange.EndIndex);
                        for (int row = startRow; row <= endRow; row++)
                        {
                            for (int col = startColumn; col <= endColumn; col++)
                            {
                                PropertyInfo prop = ToneEntries.Items[row].GetType().GetProperty(ToneEntries.Columns[col].FieldName);
                                if (prop == null)
                                {
                                    continue;
                                }

                                if (prop.PropertyType == typeof(double))
                                {
                                    double d;
                                    if (Double.TryParse(text, out d))
                                    {
                                        prop.SetValue(ToneEntries.Items[row], d);
                                    }
                                }
                                else if (prop.PropertyType == typeof(float))
                                {
                                    float d;
                                    if (Single.TryParse(text, out d))
                                    {
                                        prop.SetValue(ToneEntries.Items[row], d);
                                    }
                                }
                                else if (prop.PropertyType == typeof(uint))
                                {
                                    uint i;
                                    if (UInt32.TryParse(text, out i))
                                    {
                                        prop.SetValue(ToneEntries.Items[row], i);
                                    }
                                }
                                else if (prop.PropertyType == typeof(int))
                                {
                                    int i;
                                    if (Int32.TryParse(text, out i))
                                    {
                                        prop.SetValue(ToneEntries.Items[row], i);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        prop.SetValue(ToneEntries.Items[row], text);
                                    }
                                    catch (ArgumentException)
                                    {
                                        return;
                                    }
                                }
                            }
                        }

                    }
                }

                int nextColumn = ToneEntries.SelectedCellRanges[0].ColumnRange.StartIndex;
                int nextRow = ToneEntries.SelectedCellRanges[0].ItemRange.StartIndex;

                ToneEntries.SelectedCellRanges.Clear();
                if ((nextRow + 1) < ToneEntries.Items.Count)
                    ToneEntries.SelectedCellRanges.Add(new SelectionCellRange(nextRow + 1, nextColumn));

                ToneEntries.BeginEdit();
            }
        }

        private void ToneEntries_KeyDown(object sender, KeyEventArgs e)
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


            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                ControlKeyPressed = true;
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                ShiftKeyPressed = true;

        }

        private void ColumnHeader_PreviewMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            ColumnManagerCell header = sender as ColumnManagerCell;

            int startColumnIndex = 0;
            if (ToneEntries.SelectedCellRanges.Count > 0)
                startColumnIndex = ToneEntries.SelectedCellRanges[0].ColumnRange.StartIndex;

            var column = ToneEntries.Columns.Where(x => header.ParentColumn == x).FirstOrDefault();
            int columnIndex = ToneEntries.Columns.IndexOf(column);

            if (!ShiftKeyPressed && !ControlKeyPressed)
            {
                ToneEntries.SelectedCellRanges.Clear();
                if (columnIndex < ToneEntries.Columns.Count)
                    ToneEntries.SelectedCellRanges.Add(new SelectionCellRange(0, columnIndex, ToneEntries.Items.Count - 1, columnIndex));
            }
            else if (ShiftKeyPressed && !ControlKeyPressed)
            {
                ToneEntries.SelectedCellRanges.Clear();
                if (startColumnIndex > columnIndex)
                {
                    int temp = columnIndex;
                    columnIndex = startColumnIndex;
                    startColumnIndex = temp;
                }

                if (columnIndex < ToneEntries.Columns.Count)
                    ToneEntries.SelectedCellRanges.Add(new SelectionCellRange(0, startColumnIndex, ToneEntries.Items.Count - 1, columnIndex));
            }
            else if (!ShiftKeyPressed && ControlKeyPressed)
            {
                var newSelection = new List<SelectionCellRange>();
                bool removedColumn = false;
                foreach (var range in ToneEntries.SelectedCellRanges)
                {
                    if ((range.ItemRange.StartIndex == 0 && range.ItemRange.EndIndex == (ToneEntries.Items.Count - 1)) &&
                        (range.ColumnRange.StartIndex <= columnIndex && range.ColumnRange.EndIndex >= columnIndex))
                    {
                        if (range.ColumnRange.StartIndex == range.ColumnRange.EndIndex)
                        {
                            // Do Nothing. Just remove this from the Selection Rnage
                        }
                        else
                        {
                            if (columnIndex == range.ColumnRange.StartIndex)
                            {
                                newSelection.Add(new SelectionCellRange(0, columnIndex + 1, ToneEntries.Items.Count - 1, range.ColumnRange.EndIndex));
                            }
                            else if (columnIndex == range.ColumnRange.EndIndex)
                            {
                                newSelection.Add(new SelectionCellRange(0, range.ColumnRange.StartIndex, ToneEntries.Items.Count - 1, columnIndex - 1));
                            }
                            else
                            {
                                // Create two new ranges
                                newSelection.Add(new SelectionCellRange(0, range.ColumnRange.StartIndex, ToneEntries.Items.Count - 1, columnIndex - 1));
                                newSelection.Add(new SelectionCellRange(0, columnIndex + 1, ToneEntries.Items.Count - 1, range.ColumnRange.EndIndex));
                            }
                        }
                        removedColumn = true;
                    }
                    else
                    {
                        newSelection.Add(range);
                    }
                }
                if (!removedColumn)
                {
                    newSelection.Add(new SelectionCellRange(0, columnIndex, ToneEntries.Items.Count - 1, columnIndex));
                }
                ToneEntries.SelectedCellRanges.Clear();
                foreach (var range in newSelection)
                    ToneEntries.SelectedCellRanges.Add(range);
            }
        }

        private void ToneEntries_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                ControlKeyPressed = false;
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                ShiftKeyPressed = false;
        }

        private void ToneEntries_SelectionChanged(object sender, DataGridSelectionChangedEventArgs e)
        {
            e.Handled = true;

            var dg = sender as DataGridControl;
            if (dg.SelectedCellRanges.Count > 0)
            {
                (dg.DataContext as ToneBufferDockWindowViewModel).RowSelected = dg.SelectedCellRanges[0].ItemRange.StartIndex;
            }
        }

    }
}
