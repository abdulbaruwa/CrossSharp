using System;
using System.Linq;
using CrossPuzzleClient.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CrossSharp.Core;

namespace CrossPuzzleClient.ViewModels
{

    public class DesignPuzzleBoardViewModel : PuzzleBoardViewModel
    {
        public DesignPuzzleBoardViewModel()
        {
            Words = new ObservableCollection<WordViewModel>();
            CreateDummyData(); 
        }

        private void CreateDummyData()
        {
            var words = new List<string>();
            words.Add("Bamidele");
            words.Add("station");
            words.Add("india");
            words.Add("Adams");
            words.Add("fards");
            words.Add("novemb");
            words.Add("belt");
            words.Add("train");
            words.Add("adeola");
            words.Add("amoeba");
            words.Add("moscow");
            var board = CoreHorizontal.GetBoard(15,15);
            var result = (CoreVertical.AddWordsAttempts(words.ToArray(), board));

            var wordsInserted = result.Item1.Where(x => x.inserted);
            foreach (var word in wordsInserted)
            {
               var wordViewModel = new WordViewModel()
                {
                    Cells = new ObservableCollection<CellEmptyViewModel>(),
                    Direction = GetDirection(word.orientation),
                    Word = word.word
                };

                var row = word.row;
                var col = word.col;
                foreach (var character in word.word)
                {
                    var cell = new CellViewModel(col, row, character.ToString(), wordViewModel);
                    if (word.orientation == CoreHorizontal.Orientation.horizontal)
                        col += col;
                    else
                        row += row;

                   wordViewModel.Cells.Add(cell); 
                }
                this.Words.Add((wordViewModel));

            }

        }

        private Direction GetDirection(CoreHorizontal.Orientation orientation)
        {
            return orientation == CoreHorizontal.Orientation.horizontal ? Direction.Across : Direction.Down;
        }
    }

    public class PuzzleBoardViewModel : BindableBase
    {
        private readonly ObservableCollection<CellEmptyViewModel> _cells;
        private int _cols;
        private int _rows;
        private ObservableCollection<WordViewModel> _words;
        public PuzzleBoardViewModel()
        {
            _cells = new ObservableCollection<CellEmptyViewModel>();
            _words = new ObservableCollection<WordViewModel>();
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

        public ObservableCollection<CellEmptyViewModel> Cells
        {
            get { return _cells; }
        }

        public ObservableCollection<WordViewModel> Words
        {
            get { return _words; }
            set { SetProperty(ref _words, value); }
        }
        
        public ObservableCollection<WordViewModel> WordsAcross
        {
            get { return new ObservableCollection<WordViewModel>(Words.Where(x => x.Direction == Direction.Across)); }
        }

        public ObservableCollection<WordViewModel> WordsDown
        {
            get { return new ObservableCollection<WordViewModel>(Words.Where(x => x.Direction == Direction.Down)); }
        }


        private void CreateBubblesAsync()
        {
            var cells = new List<CellEmptyViewModel>();
            cells.AddRange(
                from row in Enumerable.Range(0, 15)
                from col in Enumerable.Range(0, 15)
                select new CellEmptyViewModel(col, row, string.Empty));

            foreach (CellEmptyViewModel cellViewModel in cells)
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