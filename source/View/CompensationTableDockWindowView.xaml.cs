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
    /// Interaction logic for CompensationTableDockWindowView.xaml
    /// </summary>
    public partial class CompensationTableDockWindowView : UserControl
    {
        private bool ControlKeyPressed = false;
        private bool ShiftKeyPressed = false;

        public CompensationTableDockWindowView()
        {
            InitializeComponent();
            CompEntries.ClipboardExporters["CSV"].IncludeColumnHeaders = false;
            CompEntries.ClipboardExporters["Text"].IncludeColumnHeaders = true;

            AmplStyleBox.ItemsSource = Enum.GetValues(typeof(iMS_Studio.ViewModel.InterpolationStyleVM)).Cast<iMS_Studio.ViewModel.InterpolationStyleVM>();
            PhaseStyleBox.ItemsSource = Enum.GetValues(typeof(iMS_Studio.ViewModel.InterpolationStyleVM)).Cast<iMS_Studio.ViewModel.InterpolationStyleVM>();
            SyncAStyleBox.ItemsSource = Enum.GetValues(typeof(iMS_Studio.ViewModel.InterpolationStyleVM)).Cast<iMS_Studio.ViewModel.InterpolationStyleVM>();
            SyncDStyleBox.ItemsSource = Enum.GetValues(typeof(iMS_Studio.ViewModel.InterpolationStyleVM)).Cast<iMS_Studio.ViewModel.InterpolationStyleVM>();

            ModifierBox.ItemsSource = Enum.GetValues(typeof(iMS_Studio.ViewModel.CompensationModifierVM)).Cast<iMS_Studio.ViewModel.CompensationModifierVM>();
        }

        private void IndexValue_QueryValue(object sender, Xceed.Wpf.DataGrid.DataGridItemPropertyQueryValueEventArgs e)
        {
            CompensationPointSpecificationViewModel row = e.Item as CompensationPointSpecificationViewModel;
            //var collectionViewSource = this.grid.FindResource("comp_data") as DataGridCollectionViewSource;

            if (row != null)
            {
                try
                {
                    int currentRowIndex = CompEntries.Items.IndexOf(row) + 1;
                    // if (row != null)
                    {
                        e.Value = currentRowIndex;
                    }
                }
                catch (InvalidOperationException)
                {
                    // Disable this exception which occurs when editing a cell
                }
            }
            else e.Value = -1;
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
                // Don't proceed if data is invalid
                double test;
                if (!Double.TryParse((sender as TextBox).Text, out test))
                {
                    e.Handled = true;
                    return;
                }

                CompEntries.EndEdit();

                // Paste Value to all Selected Cells
                var text = (sender as TextBox).Text;
                foreach (SelectionCellRange range in CompEntries.SelectedCellRanges)
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
                                PropertyInfo prop = CompEntries.Items[row].GetType().GetProperty(CompEntries.Columns[col].FieldName);
                                if (prop == null)
                                {
                                    continue;
                                }

                                if (prop.PropertyType == typeof(double))
                                {
                                    double d;
                                    if (Double.TryParse(text, out d))
                                    {
                                        prop.SetValue(CompEntries.Items[row], d);
                                    }
                                }
                                else if (prop.PropertyType == typeof(float))
                                {
                                    float d;
                                    if (Single.TryParse(text, out d))
                                    {
                                        prop.SetValue(CompEntries.Items[row], d);
                                    }
                                }
                                else if (prop.PropertyType == typeof(uint))
                                {
                                    uint i;
                                    if (UInt32.TryParse(text, out i))
                                    {
                                        prop.SetValue(CompEntries.Items[row], i);
                                    }
                                }
                                else if (prop.PropertyType == typeof(int))
                                {
                                    int i;
                                    if (Int32.TryParse(text, out i))
                                    {
                                        prop.SetValue(CompEntries.Items[row], i);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        prop.SetValue(CompEntries.Items[row], text);
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

                // Then move to next cell down
                int nextColumn = CompEntries.SelectedCellRanges[0].ColumnRange.StartIndex;
                int nextRow = CompEntries.SelectedCellRanges[0].ItemRange.StartIndex;

                CompEntries.SelectedCellRanges.Clear();
                if ((nextRow + 1) < CompEntries.Items.Count)
                    CompEntries.SelectedCellRanges.Add(new SelectionCellRange(nextRow + 1, nextColumn));

                CompEntries.BeginEdit();
            }
        }

        private void CompEntries_KeyDown(object sender, KeyEventArgs e)
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
            if (CompEntries.SelectedCellRanges.Count > 0)
                startColumnIndex = CompEntries.SelectedCellRanges[0].ColumnRange.StartIndex;

            var column = CompEntries.Columns.Where(x => header.ParentColumn == x).FirstOrDefault();
            int columnIndex = CompEntries.Columns.IndexOf(column);

            if (!ShiftKeyPressed && !ControlKeyPressed)
            {
                CompEntries.SelectedCellRanges.Clear();
                if (columnIndex < CompEntries.Columns.Count)
                    CompEntries.SelectedCellRanges.Add(new SelectionCellRange(0, columnIndex, CompEntries.Items.Count - 1, columnIndex));
            }
            else if (ShiftKeyPressed && !ControlKeyPressed)
            {
                CompEntries.SelectedCellRanges.Clear();
                if (startColumnIndex > columnIndex)
                {
                    int temp = columnIndex;
                    columnIndex = startColumnIndex;
                    startColumnIndex = temp;
                }

                if (columnIndex < CompEntries.Columns.Count)
                    CompEntries.SelectedCellRanges.Add(new SelectionCellRange(0, startColumnIndex, CompEntries.Items.Count - 1, columnIndex));
            }
            else if (!ShiftKeyPressed && ControlKeyPressed)
            {
                var newSelection = new List<SelectionCellRange>();
                bool removedColumn = false;
                foreach (var range in CompEntries.SelectedCellRanges)
                {
                    if ((range.ItemRange.StartIndex == 0 && range.ItemRange.EndIndex == (CompEntries.Items.Count - 1)) &&
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
                                newSelection.Add(new SelectionCellRange(0, columnIndex + 1, CompEntries.Items.Count - 1, range.ColumnRange.EndIndex));
                            }
                            else if (columnIndex == range.ColumnRange.EndIndex)
                            {
                                newSelection.Add(new SelectionCellRange(0, range.ColumnRange.StartIndex, CompEntries.Items.Count - 1, columnIndex - 1));
                            }
                            else
                            {
                                // Create two new ranges
                                newSelection.Add(new SelectionCellRange(0, range.ColumnRange.StartIndex, CompEntries.Items.Count - 1, columnIndex - 1));
                                newSelection.Add(new SelectionCellRange(0, columnIndex + 1, CompEntries.Items.Count - 1, range.ColumnRange.EndIndex));
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
                    newSelection.Add(new SelectionCellRange(0, columnIndex, CompEntries.Items.Count - 1, columnIndex));
                }
                CompEntries.SelectedCellRanges.Clear();
                foreach (var range in newSelection)
                    CompEntries.SelectedCellRanges.Add(range);
            }
        }

        private void CompEntries_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                ControlKeyPressed = false;
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                ShiftKeyPressed = false;
        }

        private void CompEntries_SelectionChanged(object sender, DataGridSelectionChangedEventArgs e)
        {
            e.Handled = true;

            var dg = sender as DataGridControl;
            if (dg.SelectedCellRanges.Count > 0)
            {
                (dg.DataContext as CompensationTableDockWindowViewModel).RowSelected = dg.SelectedCellRanges[0].ItemRange.StartIndex;
            }
        }
    }

}

