using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMS_Studio;
using System.Windows.Input;

namespace iMS_Studio.ViewModel
{
    public class DockManagerViewModel
    {
        /// <summary>Gets a collection of all visible documents</summary>
        public ObservableCollection<DockWindowViewModel> Documents { get; private set; }

        public ObservableCollection<DockPaneViewModel> Anchorables { get; private set; }

        public DockManagerViewModel(IEnumerable<DockWindowViewModel> dockWindowViewModels,
            IEnumerable<DockPaneViewModel> dockPaneViewModels)
        {
            this.Documents = new ObservableCollection<DockWindowViewModel>();
            this.Anchorables = new ObservableCollection<DockPaneViewModel>();

            foreach (var document in dockWindowViewModels)
            {
                document.PropertyChanged += DockWindowViewModel_PropertyChanged;
                if (!document.IsClosed)
                    this.Documents.Add(document);
            }

            foreach (var anchorable in dockPaneViewModels)
            {
                anchorable.PropertyChanged += Anchorable_PropertyChanged;
                this.Anchorables.Add(anchorable);
            }
        }

        public void AddNewDocument(DockWindowViewModel doc)
        {
            doc.PropertyChanged += DockWindowViewModel_PropertyChanged;
            if (!doc.IsClosed)
                this.Documents.Add(doc);
        }

        private void Anchorable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //           throw new NotImplementedException();
        }

        private void DockWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DockWindowViewModel document = sender as DockWindowViewModel;

            if (e.PropertyName == nameof(DockWindowViewModel.IsClosed))
            {
                if (!document.IsClosed)
                    this.Documents.Add(document);
                else
                {
                    try
                    {
                        this.Documents.Remove(document);
                    }
                    catch (NullReferenceException)
                    {
                        // urgh. Buried bug when closing all documents (e.g. New command), referencing LayoutItem.View property get accessor
                    }
                }
            }
        }

        /// <summary>
        /// Expose command to load/save AvalonDock layout on application startup and shut-down.
        /// </summary>
        /// 
        private Behaviour.AvalonDockLayoutViewModel mAVLayout = null;
        public Behaviour.AvalonDockLayoutViewModel ADLayout
        {
            get
            {
                if (this.mAVLayout == null)
                    this.mAVLayout = new Behaviour.AvalonDockLayoutViewModel(Anchorables.ToList());

                return this.mAVLayout;
            }
        }


    }


}
