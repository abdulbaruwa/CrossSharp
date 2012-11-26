using System;
using CrossPuzzleClient.ViewModels.PuzzleBoardView;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CrossPuzzleClient.Views
{
    public class BoardCanvas : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            // Get the collection of children
            UIElementCollection mychildren = Children;

            // Get total number of children
            int count = mychildren.Count;

            // Measure first 9 children giving them space up to 100x100, remaining children get 0x0 

            foreach (FrameworkElement child in Children)
            {
                child.Measure(new Size(50, 50));
            }

            // return the size available to the whole panel, which is 300x300
            return new Size(600, 600);
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            // Get the collection of children
            UIElementCollection mychildren = Children;

            // Get total number of children
            int count = mychildren.Count;

            // Arrange children
            // We're only allowing 9 children in this panel.  More children will get a 0x0 layout slot.
            int i;
            for (i = 0; i < count; i++)
            {

                // Get (left, top) origin point for the element in the 3x3 block
                Point cellOrigin = GetOrigin(i, 12, new Size(50, 50));

                // Arrange child
                // Get desired height and width. This will not be larger than 100x100 as set in MeasureOverride.
              //  var cellViewModel = contentPresenter.DataContext as CellViewModel;
                double dw = mychildren[i].DesiredSize.Width;
                double dh = mychildren[i].DesiredSize.Height;

                mychildren[i].Arrange(new Rect(cellOrigin.X, cellOrigin.Y, dw, dh));
            }

            // Return final size of the panel
            return finalSize;
        }
        protected Point GetOrigin(int blockNum, int blocksPerRow, Size itemSize)
        {
            // Get row number (zero-based)
            int row = (int)Math.Floor((decimal)(blockNum / blocksPerRow));

            // Get column number (zero-based)
            int column = blockNum - blocksPerRow * row;

            // Calculate origin
            Point origin = new Point(itemSize.Width * column, itemSize.Height * row);
            return origin;

        }
        internal static int BubbleSize
        {
            get { return 42; }
        }

        internal int ColumnCount
        {
            get { return (int)Math.Floor(base.ActualWidth / BubbleSize); }
        }

        internal int RowCount
        {
            get { return (int)Math.Floor(base.ActualHeight / BubbleSize); }
        }

        internal double CalculateLeft(FrameworkElement cellContainer)
        {
            if (cellContainer == null)
                throw new ArgumentNullException("cellContainer");

            var bubble = cellContainer.DataContext as CellViewModel;
            if (bubble == null)
                throw new ArgumentException("Element does not have a CellEmptyViewModel as its DataContext.", "cellContainer");

            return this.CalculateLeft(bubble.Col);
        }

        internal double CalculateTop(FrameworkElement cellContainer)
        {
            if (cellContainer == null)
                throw new ArgumentNullException("cellContainer");

            var bubble = cellContainer.DataContext as CellViewModel;
            if (bubble == null)
                throw new ArgumentException("Element does not have a CellEmptyViewModel as its DataContext.", "cellContainer");

            return this.CalculateTop(bubble.Row);
        }

        double CalculateLeft(int column)
        {
            double bubblesWidth = BubbleSize * this.ColumnCount;
            double horizOffset = (base.ActualWidth - bubblesWidth) / 2;
            return column * BubbleSize + horizOffset;
        }

        double CalculateTop(int row)
        {
            double bubblesHeight = BubbleSize * this.RowCount;
            double vertOffset = (base.ActualHeight - bubblesHeight) / 2;
            return row * BubbleSize + vertOffset;
        }

        //protected  void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        //{
        //    var contentPresenter = visualAdded as ContentPresenter;
        //    if (contentPresenter != null)
        //    {
        //        var bubble = contentPresenter.DataContext as CellEmptyViewModel;
        //        if (bubble != null)
        //        {
        //            Canvas.SetLeft(contentPresenter, CalculateLeft(bubble.Column));
        //            Canvas.SetTop(contentPresenter, CalculateTop(bubble.Row));

        //            contentPresenter.Width = BubbleSize;
        //            contentPresenter.Height = BubbleSize;
        //        }
        //    }

        //    base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        //}

    }


}