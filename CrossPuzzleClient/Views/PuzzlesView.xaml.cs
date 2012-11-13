﻿using System;
using System.Collections.Generic;
using CrossPuzzleClient.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace CrossPuzzleClient.Views
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class PuzzlesView : CrossPuzzleClient.Common.LayoutAwarePage
    {
        public PuzzlesView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Assign a collection of bindable groups to this.DefaultViewModel["Groups"]
            //var item = (DesignPuzzlesVM) navigationParameter;
            //DefaultViewModel["PuzzleVm"] = item.PuzzleGroups;
        }

        private void itemGridView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            var sourceDataContext = (sender as FrameworkElement).DataContext;
            if(sourceDataContext != null)
            {
               if(e.ClickedItem == null) return;
               ((PuzzlesViewModel) sourceDataContext).SelectedPuzzleGroupViewModel = (PuzzleViewModel) e.ClickedItem;
              ((PuzzlesViewModel )sourceDataContext).StartPuzzleCommand.Execute(null);
            }
        }
    }
}
