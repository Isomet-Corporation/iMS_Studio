using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using iMS_Studio.ViewModel;
using System;
using System.Windows.Data;

namespace iMS_Studio.View
{
    /// <summary>
    /// Interaction logic for ImageProjectTreeView.xaml
    /// </summary>
    public partial class ImageProjectTreeView : UserControl
    {
        public ImageProjectTreeView()
        {
            InitializeComponent();
        }

        // double click opens window if closed and navigates to it
        private void OnItemMouseDoubleClick(object sender, MouseButtonEventArgs args)
        {
            ImageProjectTreeSubItems item;

            if (sender is TreeViewItem)
            {
                if ((sender as TreeViewItem).DataContext is ImageProjectTreeSubItems)
                {
                    //if (!(sender as TreeViewItem).IsSelected)
                    //    return;
                    item = (sender as TreeViewItem).DataContext as ImageProjectTreeSubItems;
                    item.IsClosed = false;
                    item.IsSelected = true;
                }
            }
            else if (sender is ListViewItem)
            {
                if ((sender as ListViewItem).Content is ImageProjectTreeSubItems)
                {
                    //if (!(sender as ListViewItem).IsSelected)
                    //    return;
                    item = (sender as ListViewItem).Content as ImageProjectTreeSubItems;
                    item.IsClosed = false;
                    item.IsSelected = true;
                }
            }

        }

        private string cancelString;
        private Point _startPoint;
        private bool _IsDragging = false;

        // First click selects, second click edits
        private void OnItemMouseClick(object sender, MouseButtonEventArgs args)
        {
            if (sender is TreeViewItem)
            {
                // Save Mouse position for drag & drop
                _startPoint = args.GetPosition(null);

                // Deselect ListViews
                foreach (ImageProjectTreeSubItems other in this.lvCompTable.Items)
                    other.IsSelected = false;
                foreach (ImageProjectTreeSubItems other in this.lvTbufTable.Items)
                    other.IsSelected = false;

                var item = (sender as TreeViewItem);
                if (item.IsSelected)
                {
                    if (!(item.DataContext as IImageGroup).IsEditing)
                    {
                        (item.DataContext as IImageGroup).IsEditing = true;
                        args.Handled = true;
                    }
                }

            }
            else if (sender is ListViewItem)
            {
                // Deselect others
                if ((sender as ListViewItem).Content is ToneBufferTreeSubItems)
                {
                    foreach (ImageProjectTreeSubItems other in this.lvCompTable.Items)
                        other.IsSelected = false;
                }
                else if ((sender as ListViewItem).Content is CompensationFunctionTreeSubItems)
                {
                    foreach (ImageProjectTreeSubItems other in this.lvTbufTable.Items)
                        other.IsSelected = false;
                }
                foreach (IImageGroup other in this.trvImgProj.Items)
                {
                    other.IsSelected = false;
                    if (other.GetType() == typeof(ImageProjectItems))
                    {
                        foreach (ImageProjectTreeSubItems others in other.Items)
                            others.IsSelected = false;
                    }
                }
                var item = (sender as ListViewItem);
                if (item.IsSelected)
                {
                    if (item.Content is ImageProjectTreeSubItems)
                    {
                        if (!(item.Content as ImageProjectTreeSubItems).IsEditing)
                        {
                            (item.Content as ImageProjectTreeSubItems).IsEditing = true;
                            args.Handled = true;
                        }
                    }
                }
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs args)
        {
            if (args.LeftButton == MouseButtonState.Pressed && !_IsDragging)
            {
                Point position = args.GetPosition(null);
                if (Math.Abs(position.X - _startPoint.X) >
                    SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - _startPoint.Y) >
                    SystemParameters.MinimumVerticalDragDistance)
                {
                    StartDrag(args);
                }
            }
        }

        private object _draggedItem;
        private object _target;

        private void StartDrag(MouseEventArgs e)
        {
            _IsDragging = true;
            _draggedItem = this.trvImgProj.SelectedItem;
            DataObject data = null;

            data = new DataObject("inadt", _draggedItem);

            if (data != null)
            {
                DragDropEffects dde = DragDropEffects.All;
                DragDropEffects de = DragDrop.DoDragDrop(this.trvImgProj, data, dde);
                if ((de == DragDropEffects.Move) && (_target != null))
                {
                    // A move drop was accepted
                    ICommand move = (this.DataContext as ImageProjectTreeViewModel)._move;

                    if (_target.GetType() == typeof(GroupBox))
                    {
                        if (move != null && move.CanExecute(null))
                            move.Execute(null);
                    }
                    else
                    {
                        if (move != null && move.CanExecute((_target as TreeViewItem).Header))
                            move.Execute((_target as TreeViewItem).Header);
                    }
                    _target = null;
                    _draggedItem = null;
                }
                else if ((de == DragDropEffects.Copy) && (_target != null))
                {
                    // A copy drop was accepted
                    ICommand copy = (this.DataContext as ImageProjectTreeViewModel)._copy;

                    if (_target.GetType() == typeof(GroupBox))
                    {
                        if (copy != null && copy.CanExecute(null))
                            copy.Execute(null);
                    }
                    else
                    {
                        if (copy != null && copy.CanExecute((_target as TreeViewItem).Header))
                            copy.Execute((_target as TreeViewItem).Header);
                    }
                    _target = null;
                    _draggedItem = null;
                }
            }
            _IsDragging = false;
        }

        private void OnDrop(object sender, DragEventArgs args)
        {
            args.Effects = DragDropEffects.None;
            args.Handled = true;

            // Verify that this is a valid drop and then store the drop target
            TreeViewItem TargetItem = Utils.GetNearestContainer<TreeViewItem>
                (args.OriginalSource as UIElement);
            if (TargetItem != null && _draggedItem != null)
            {
                _target = TargetItem;
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    args.Effects = DragDropEffects.Copy;
                else
                    args.Effects = DragDropEffects.Move;
            }
        }

        private void GroupBox_Drop(object sender, DragEventArgs args)
        {
            _target = sender;
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                args.Effects = DragDropEffects.Copy;
            else
                args.Effects = DragDropEffects.Move;
        }

        // Allow Enter and Escape keys to have their usual meanings
        private void OnKeyDown(object sender, KeyEventArgs args)
        {
            if (args.Key == Key.Enter)
            {
                if (sender is TreeViewItem)
                {
                    var item = (sender as TreeViewItem);
                    if ((item.DataContext as IImageGroup).IsEditing)
                    {
                        (item.DataContext as IImageGroup).IsEditing = false;
                        args.Handled = true;
                    }
                }
                else if (sender is ListViewItem)
                {
                    var item = (sender as ListViewItem);
                    if (item.Content is ImageProjectTreeSubItems)
                    {
                        if ((item.Content as ImageProjectTreeSubItems).IsEditing)
                        {
                            (item.Content as ImageProjectTreeSubItems).IsEditing = false;
                            args.Handled = true;
                        }
                    }
                }
            }
            else if (args.Key == Key.Escape)
            {
                if (sender is TreeViewItem)
                {
                    var item = (sender as TreeViewItem);
                    if ((item.DataContext as IImageGroup).IsEditing)
                    {
                        TextBox box = Utils.FindVisualChild<TextBox>(item);
                        if (box != null)
                        {
                            if (box.Name == "EditBox")
                                box.Text = cancelString;
                        }

                        (item.DataContext as IImageGroup).IsEditing = false;
                        args.Handled = true;
                    }
                }

                else if (sender is ListViewItem)
                {
                    var item = (sender as ListViewItem);
                    if (item.Content is ImageProjectTreeSubItems)
                    {
                        if ((item.Content as ImageProjectTreeSubItems).IsEditing)
                        {
                            ContentPresenter contentPresenter = Utils.FindVisualChild<ContentPresenter>(item);
                            DataTemplate dt = contentPresenter.ContentTemplate;
                            TextBox box = dt.FindName("EditBox", contentPresenter) as TextBox;
                            if (box != null)
                            {
                                box.Text = cancelString;
                            }

                            (item.Content as ImageProjectTreeSubItems).IsEditing = false;
                            args.Handled = true;
                        }
                    }
                }
            }
        }

        // When cell editor appears, focus the keyboard to it and select all text
        private void EditBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var box = sender as TextBox;
            if ((bool)e.NewValue && box != null)
            {
                box.Focus();
                box.SelectAll();
                cancelString = string.Copy(box.Text);
            }
        }

        // When adding a new entry, ensure it starts in focus and with all text selected
        private void EditBox_Initialized(object sender, System.EventArgs e)
        {
            var box = sender as TextBox;
            if (box != null)
            {
                box.Focus();
                box.SelectAll();
                cancelString = string.Copy(box.Text);
            }
        }

        private void GroupBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            FrameworkElement visibleElement = e.OriginalSource as FrameworkElement;

            if ( (visibleElement.GetType() != typeof(TextBlock)) &&
                    (visibleElement.GetType() != typeof(TextBox)) &&
                    (visibleElement.GetType() != typeof(TreeViewItem)) )
            {
                foreach (IImageGroup item in this.trvImgProj.Items)
                {
                    item.IsSelected = false;
                    if (item.GetType() == typeof(ImageProjectItems))
                    {
                        foreach (IImageGroup subitem in item.Items)
                            subitem.IsSelected = false;
                    }
                }
            }
        }

        private void ClearAllSelected()
        {
            foreach (ImageProjectTreeSubItems item in this.lvCompTable.Items)
                item.IsSelected = false;
            foreach (ImageProjectTreeSubItems item in this.lvTbufTable.Items)
                item.IsSelected = false;
            foreach (IImageGroup item in this.trvImgProj.Items)
            {
                item.IsSelected = false;
                if (item.GetType() == typeof(ImageProjectItems))
                {
                    foreach (ImageProjectTreeSubItems subitem in item.Items)
                        subitem.IsSelected = false;
                }
            }
        }

        private void EditBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ClearAllSelected();   
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs args)
        {
            args.Handled = true;
            var menu = sender as ContextMenu;
            if (menu != null)
            {
                if (menu.PlacementTarget.GetType() == typeof(TreeViewItem))
                {
                    ClearAllSelected();
                    var tvi = menu.PlacementTarget as TreeViewItem;
                    (tvi.Header as IImageGroup).IsSelected = true;
                }
                else if (menu.PlacementTarget is ListView)
                {
                    var lvi = menu.PlacementTarget as ListView;
                    int index = lvi.SelectedIndex;
                    ClearAllSelected();
                    if (index >= 0) (lvi.Items[index] as IImageGroup).IsSelected = true;
                }
                else
                {
                    ClearAllSelected();
                }
            }
        }

        IImageGroup CopySource = null;

        private void MenuItemCut_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            CopySource = null;
            foreach (IImageGroup item in this.trvImgProj.Items)
            {
                if (item.GetType() == typeof(ImageProjectItems))
                {
                    foreach (ImageProjectTreeSubItems subitem in item.Items)
                    {
                        if (subitem.GetType() == typeof(ImageTreeSubItems))
                        {
                            if (subitem.IsSelected) CopySource = subitem;
                            subitem.IsMarkedForCut = subitem.IsSelected;
                        }
                    }
                }
                else
                {
                    if (item.IsSelected) CopySource = item;
                    item.IsMarkedForCut = item.IsSelected;
                }
            }
            if (CopySource != null)
            {
                var model = this.DataContext as ImageProjectTreeViewModel;
                if (model != null) model.PasteBufferOccupied = true;
            }
        }

        private void MenuItemCopy_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            CopySource = null;
            foreach (IImageGroup item in this.trvImgProj.Items)
            {
                if (item.IsSelected) CopySource = item;
                item.IsMarkedForCut = false;
                if (item.GetType() == typeof(ImageProjectItems))
                {
                    foreach (ImageProjectTreeSubItems subitem in item.Items)
                    {
                        if (subitem.GetType() == typeof(ImageTreeSubItems))
                        {
                            if (subitem.IsSelected) CopySource = subitem;
                            subitem.IsMarkedForCut = false;
                        }
                    }
                }
            }
            if (CopySource != null)
            {
                var model = this.DataContext as ImageProjectTreeViewModel;
                if (model != null) model.PasteBufferOccupied = true;
            }
        }

        private void MenuItemPaste_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            bool DoCut = false;

            if (CopySource == null) return;

            IImageGroup CopyDest = null;
            foreach (IImageGroup item in this.trvImgProj.Items)
            {
                if (item.IsMarkedForCut) DoCut = true;
                item.IsMarkedForCut = false;
                if (item.IsSelected) CopyDest = item;
                if (item.GetType() == typeof(ImageProjectItems))
                {
                    foreach (ImageProjectTreeSubItems subitem in item.Items)
                    {
                        if (subitem.IsMarkedForCut) DoCut = true;
                        subitem.IsMarkedForCut = false;
                        if (subitem.IsSelected) CopyDest = subitem;
                    }
                }
            }

            var model = this.DataContext as ImageProjectTreeViewModel;
            if (model == null) return;
            ICommand operation = null;
            if (DoCut)
            {
                operation = model._move;
            }
            else
            {
                operation = model._copy;
            }
            if (CopyDest != null)
                CopyDest.IsSelected = false;
            CopySource.IsSelected = true;
            if (operation != null && operation.CanExecute(CopyDest))
                operation.Execute(CopyDest);
            // For Cut operations, only allow one paste
            if (DoCut)
            {
                CopySource = null;
                model.PasteBufferOccupied = false;
            }
        }

        private void MenuItemClose_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            foreach (IImageGroup item in this.trvImgProj.Items)
            {
                if (item.GetType() == typeof(ImageProjectItems))
                {
                    if (item.IsSelected)
                    {
                        foreach (ImageProjectTreeSubItems subitem in item.Items)
                            subitem.IsClosed = true;
                    }
                    else
                    {
                        foreach (ImageProjectTreeSubItems subitem in item.Items)
                        {
                            if (subitem.IsSelected) subitem.IsClosed = true;
                        }
                    }
                }
                else
                {
                    if (item.IsSelected) (item as ImageProjectTreeSubItems).IsClosed = true;
                }
            }
            foreach (CompensationFunctionTreeSubItems item in this.lvCompTable.Items)
            {
                if (item.IsSelected) item.IsClosed = true;
            }
            foreach (ToneBufferTreeSubItems item in this.lvTbufTable.Items)
            {
                if (item.IsSelected) item.IsClosed = true;
            }
        }

        private void MenuItemCut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (sender.GetType() == typeof(TreeView)) || (sender.GetType() == typeof(TreeViewItem));
        }

        private void MenuItemCopy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (sender.GetType() == typeof(TreeView)) || (sender.GetType() == typeof(TreeViewItem));
        }

        private void MenuItemPaste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (CopySource != null);
        }
    }

}
