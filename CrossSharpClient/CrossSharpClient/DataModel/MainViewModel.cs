using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CrossSharpClient.Common;
using Windows.Foundation.Metadata;

namespace CrossSharpClient.DataModel
{
    [WebHostHidden]
    public sealed class MainViewModel : BindableBase
    {
        public MainViewModel()
        {
            Board = new BoardViewModel();
        }

        public BoardViewModel Board { get; private set; }
    }

    public sealed class BoardViewModel : BindableBase
    {
        private readonly ObservableCollection<CellViewModel> _cells;
        private int _cols;

        private int _rows;

        public BoardViewModel()
        {
            _cells = new ObservableCollection<CellViewModel>();
            CreateBubblesAsync();
        }

        public int Cols
        {
            get { return _cols; }
        }

        public int Rows
        {
            get { return _rows; }
        }

        public ObservableCollection<CellViewModel> Cells
        {
            get { return _cells; }
        }

        private void CreateBubblesAsync()
        {
            var cells = new List<CellViewModel>();
            cells.AddRange(
                from row in Enumerable.Range(0, 15)
                from col in Enumerable.Range(0, 15)
                select new CellViewModel(row, col, GetRandomChar(),true,Direction.Down));

            foreach (CellViewModel cellViewModel in cells)
            {
                _cells.Add(cellViewModel);
            }
        }
        internal static string GetRandomChar()
        {
            var x = Convert.ToChar(new Random().Next(65, 90)).ToString();
            return x;
        }
    }
}