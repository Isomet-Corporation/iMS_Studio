using iMS_Studio.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AvalonDock.Layout;

namespace iMS_Studio.View.Pane
{

  class LayoutInitializer : ILayoutUpdateStrategy
  {
    public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
    {
            var viewModel = (DockPaneViewModel)anchorableToShow.Content;
            //            var layoutContent = layout.Descendents().OfType<LayoutAnchorable>().FirstOrDefault(x => x.ContentId == viewModel.ContentId);
            //var layoutContent = layout.Descendents().OfType<LayoutAnchorable>();
            /*if (layoutContent == null)
                return false;
            layoutContent.Content = anchorableToShow.Content;*/
            // LayoutAnchorablePane destPane = destinationContainer as LayoutAnchorablePane;
            var layoutPaneCollection = layout.Descendents().OfType<LayoutAnchorGroup>();

            switch (viewModel.ContentId)
            {
                case "ProjectExplorer":
                    {
                        var layoutPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(x => x.Name == "AnchorablesPaneLeft");
                        if (layoutPane == null)
                            return false;
                        layoutPane.Children.Add(anchorableToShow);
                    };
                    break;
                case "CalibrationView":
                    {
                        //                        var layoutPane = layout.Descendents().OfType<LayoutAnchorGroup>().Where(x => x.Parent.GetSide() == AnchorSide.Right).FirstOrDefault();
                        //                        var layoutPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(x => x.Name == "AnchorablesPaneRight");
                        var layoutPane = layoutPaneCollection.ToList()[0];
                                                if (layoutPane == null)
                                                    return false;
                        //                        anchorableToShow.ToggleAutoHide();
                        anchorableToShow.AutoHideWidth = 250;
                        anchorableToShow.AutoHideMinWidth = 150;
//                        anchorableToShow.AddToLayout(layout.Manager, AnchorableShowStrategy.Right);

                        layoutPane.Children.Add(anchorableToShow);
                    };
                    break;
                case "EnhancedToneView":
                    {
                        var layoutPane = layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                        if (layoutPane == null)
                            return false;
                        //anchorableToShow.ToggleAutoHide();
                        //anchorableToShow.AutoHideWidth = 145;
                        layoutPane.Children.Add(anchorableToShow);
                    };
                    break;
                case "SignalPath":
                    {
                        //                        var layoutPane = layout.Descendents().OfType<LayoutAnchorGroup>().Where(x => x.Parent.GetSide() == AnchorSide.Right).FirstOrDefault();
                        //                        var layoutPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(x => x.Name == "AnchorablesPaneRight");
                        var layoutPane = layoutPaneCollection.ToList()[1];
                        if (layoutPane == null)
                            return false;
//                        anchorableToShow.ToggleAutoHide();
                        anchorableToShow.AutoHideWidth = 250;
                        anchorableToShow.AutoHideMinWidth = 210;
//                        anchorableToShow.AddToLayout(layout.Manager, AnchorableShowStrategy.Right);
                        layoutPane.Children.Add(anchorableToShow);
                    };
                    break;
                case "SysFunc":
                    {
                        //                        var layoutPane = layout.Descendents().OfType<LayoutAnchorGroup>().Where(x => x.Parent.GetSide() == AnchorSide.Right).FirstOrDefault();
                        //                        var layoutPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(x => x.Name == "AnchorablesPaneRight");
                        var layoutPane = layoutPaneCollection.ToList()[2];
                        if (layoutPane == null)
                            return false;
                        //                        anchorableToShow.ToggleAutoHide();
                        anchorableToShow.AutoHideWidth = 250;
                        anchorableToShow.AutoHideMinWidth = 210;
                        //                        anchorableToShow.AddToLayout(layout.Manager, AnchorableShowStrategy.Right);
                        layoutPane.Children.Add(anchorableToShow);
                    };
                    break;
                case "PlayConfiguration":
                    {
                        //                        var layoutPane = layout.Descendents().OfType<LayoutAnchorGroup>().Where(x => x.Parent.GetSide() == AnchorSide.Right).FirstOrDefault();
                        //                        var layoutPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(x => x.Name == "AnchorablesPaneRight");
                        var layoutPane = layoutPaneCollection.ToList()[3];
                        if (layoutPane == null)
                            return false;
//                        anchorableToShow.ToggleAutoHide();
                        anchorableToShow.AutoHideWidth = 205;
                        anchorableToShow.AutoHideMinWidth = 150;
//                        anchorableToShow.AddToLayout(layout.Manager, AnchorableShowStrategy.Right);
                        layoutPane.Children.Add(anchorableToShow);
                    };
                    break;
                case "SyncData":
                    {
                        //                        var layoutPane = layout.Descendents().OfType<LayoutAnchorGroup>().Where(x => x.Parent.GetSide() == AnchorSide.Right).FirstOrDefault();
                        //                        var layoutPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(x => x.Name == "AnchorablesPaneRight");
                        var layoutPane = layoutPaneCollection.ToList()[4];
                        if (layoutPane == null)
                            return false;
//                        anchorableToShow.ToggleAutoHide();
                        anchorableToShow.AutoHideWidth = 250;
                        anchorableToShow.AutoHideMinWidth = 150;
//                        anchorableToShow.AddToLayout(layout.Manager, AnchorableShowStrategy.Right);
                        layoutPane.Children.Add(anchorableToShow);
                    };
                    break;
                case "CompensationView":
                    {
                        //                        var layoutPane = layout.Descendents().OfType<LayoutAnchorGroup>().Where(x => x.Parent.GetSide() == AnchorSide.Right).FirstOrDefault();
                        //                        var layoutPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(x => x.Name == "AnchorablesPaneRight");
                        var layoutPane = layoutPaneCollection.ToList()[5];
                        if (layoutPane == null)
                            return false;
//                        anchorableToShow.ToggleAutoHide();
                        anchorableToShow.AutoHideWidth = 400;
                        anchorableToShow.AutoHideMinWidth = 250;
//                        anchorableToShow.AddToLayout(layout.Manager, AnchorableShowStrategy.Right);
                        layoutPane.Children.Add(anchorableToShow);
                    };
                    break;
                case "HWServerConsole":
                    {
                        //var layoutPane = layout.Descendents().OfType<LayoutAnchorGroup>().Where(x => x.Parent.GetSide() == AnchorSide.Bottom).FirstOrDefault();
                        // var layoutPane = layoutPaneCollection.Where(x => x.Parent.GetSide() == AnchorSide.Bottom).FirstOrDefault();
                        var layoutPane = layoutPaneCollection.ToList()[6];
                        if (layoutPane == null)
                            return false;
                       // anchorableToShow.ToggleAutoHide();
                        anchorableToShow.AutoHideHeight = 120;
                        layoutPane.Children.Add(anchorableToShow);
                    };
                    break;

            }
            


            // Add layoutContent to it's previous container
            //var layoutContainer = layoutContent.GetType().GetProperty("PreviousContainer", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(layoutContent, null) as ILayoutContainer;
            //if (layoutContainer is LayoutAnchorablePane)
            //    (layoutContainer as LayoutAnchorablePane).Children.Add(layoutContent as LayoutAnchorable);
            //else if (layoutContainer is LayoutDocumentPane)
            //    (layoutContainer as LayoutDocumentPane).Children.Add(layoutContent);
            //else
            //    throw new NotSupportedException();
            return true;

            //AD wants to add the anchorable into destinationContainer
            //just for test provide a new anchorablepane 
            //if the pane is floating let the manager go ahead
//            LayoutAnchorablePane destPane = destinationContainer as LayoutAnchorablePane;
      //if (destinationContainer != null &&
          //destinationContainer.FindParent<LayoutFloatingWindow>() != null)
        //return false;

      /*var toolsPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(d => d.Name == "ToolsPane");
      if (toolsPane != null)
      {
        toolsPane.Children.Add(anchorableToShow);
        return true;
      }*/

      //return false;

    }


    public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
    {
    }


    public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
    {
      return false;
    }

    public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
    {

    }
  }
}
