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
using System.Diagnostics;
using iMS_Studio.ViewModel;
using Xceed.Wpf.DataGrid.Export;
using System.Reflection;

namespace iMS_Studio.View
{
    /// <summary>
    /// Interaction logic for SampleDockWindowView.xaml
    /// </summary>
    public partial class ImageDockWindowView : UserControl
    {
        private bool ControlKeyPressed = false;
        private bool ShiftKeyPressed = false;

        public ImageDockWindowView()
        {
            InitializeComponent();
            ImagePoints.ClipboardExporters["CSV"].IncludeColumnHeaders = false;
            ImagePoints.ClipboardExporters["Text"].IncludeColumnHeaders = true;
            //ImagePoints.ClipboardExporters["UnicodeText"].IncludeColumnHeaders = false;
            //ImagePoints.ClipboardExporters["HTML Format"].IncludeColumnHeaders = false;
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

                ImagePoints.EndEdit();

                // Paste Value to all Selected Cells
                var text = (sender as TextBox).Text;
                foreach (SelectionCellRange range in ImagePoints.SelectedCellRanges)
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
                                PropertyInfo prop = ImagePoints.Items[row].GetType().GetProperty(ImagePoints.Columns[col].FieldName);
                                if (prop == null)
                                {
                                    continue;
                                }

                                if (prop.PropertyType == typeof(double))
                                {
                                    double d;
                                    if (Double.TryParse(text, out d))
                                    {
                                        prop.SetValue(ImagePoints.Items[row], d);
                                    }
                                }
                                else if (prop.PropertyType == typeof(float))
                                {
                                    float d;
                                    if (Single.TryParse(text, out d))
                                    {
                                        prop.SetValue(ImagePoints.Items[row], d);
                                    }
                                }
                                else if (prop.PropertyType == typeof(uint))
                                {
                                    uint i;
                                    if (UInt32.TryParse(text, out i))
                                    {
                                        prop.SetValue(ImagePoints.Items[row], i);
                                    }
                                }
                                else if (prop.PropertyType == typeof(int))
                                {
                                    int i;
                                    if (Int32.TryParse(text, out i))
                                    {
                                        prop.SetValue(ImagePoints.Items[row], i);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        prop.SetValue(ImagePoints.Items[row], text);
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
                int nextColumn = ImagePoints.SelectedCellRanges[0].ColumnRange.StartIndex;
                int nextRow = ImagePoints.SelectedCellRanges[0].ItemRange.StartIndex;

                ImagePoints.SelectedCellRanges.Clear();
                if ((nextRow + 1) < ImagePoints.Items.Count)
                    ImagePoints.SelectedCellRanges.Add(new SelectionCellRange(nextRow + 1, nextColumn));

                ImagePoints.BeginEdit();
            }
        }

        private void ImagePoints_KeyDown(object sender, KeyEventArgs e)
        {
            // Detect ascii keypress and start editing cell
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
            if (ImagePoints.SelectedCellRanges.Count > 0)
                startColumnIndex = ImagePoints.SelectedCellRanges[0].ColumnRange.StartIndex;

            var column = ImagePoints.Columns.Where(x => header.ParentColumn == x).FirstOrDefault();
            int columnIndex = ImagePoints.Columns.IndexOf(column);

            if (!ShiftKeyPressed && !ControlKeyPressed)
            {
                ImagePoints.SelectedCellRanges.Clear();
                if (columnIndex < ImagePoints.Columns.Count)
                    ImagePoints.SelectedCellRanges.Add(new SelectionCellRange(0, columnIndex, ImagePoints.Items.Count - 1, columnIndex));
            }
            else if (ShiftKeyPressed && !ControlKeyPressed)
            {
                ImagePoints.SelectedCellRanges.Clear();
                if (startColumnIndex > columnIndex)
                {
                    int temp = columnIndex;
                    columnIndex = startColumnIndex;
                    startColumnIndex = temp;
                }

                if (columnIndex < ImagePoints.Columns.Count)
                    ImagePoints.SelectedCellRanges.Add(new SelectionCellRange(0, startColumnIndex, ImagePoints.Items.Count - 1, columnIndex));
            }
            else if (!ShiftKeyPressed && ControlKeyPressed)
            {
                var newSelection = new List<SelectionCellRange>();
                bool removedColumn = false;
                foreach (var range in ImagePoints.SelectedCellRanges)
                {
                    if ((range.ItemRange.StartIndex == 0 && range.ItemRange.EndIndex == (ImagePoints.Items.Count - 1)) &&
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
                                newSelection.Add(new SelectionCellRange(0, columnIndex + 1, ImagePoints.Items.Count - 1, range.ColumnRange.EndIndex));
                            }
                            else if (columnIndex == range.ColumnRange.EndIndex)
                            {
                                newSelection.Add(new SelectionCellRange(0, range.ColumnRange.StartIndex, ImagePoints.Items.Count - 1, columnIndex - 1));
                            }
                            else
                            {
                                // Create two new ranges
                                newSelection.Add(new SelectionCellRange(0, range.ColumnRange.StartIndex, ImagePoints.Items.Count - 1, columnIndex - 1));
                                newSelection.Add(new SelectionCellRange(0, columnIndex + 1, ImagePoints.Items.Count - 1, range.ColumnRange.EndIndex));
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
                    newSelection.Add(new SelectionCellRange(0, columnIndex, ImagePoints.Items.Count - 1, columnIndex));
                }
                ImagePoints.SelectedCellRanges.Clear();
                foreach (var range in newSelection)
                    ImagePoints.SelectedCellRanges.Add(range);
            }
        }

        private void ImagePoints_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                ControlKeyPressed = false;
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                ShiftKeyPressed = false;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Trigger Property update (sets dirty flag)
        }
    }

}
