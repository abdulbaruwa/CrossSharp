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
            AddWordsToBoard();
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
            var wordviewmodels = new List<WordViewModel>();
            foreach (var word in wordsInserted)
            {
                var position = (word.row * 15) + word.col;

               var wordViewModel = new WordViewModel()
                {
                    Cells = new ObservableCollection<CellEmptyViewModel>(),
                    Direction = GetDirection(word.orientation),
                    Word = word.word,
                    Index = position
                };

                var row = word.row;
                var col = word.col;
                foreach (var character in word.word)
                {
                    var cell = new CellViewModel(col, row, character.ToString(), wordViewModel,string.Empty);
                    if (word.orientation == CoreHorizontal.Orientation.horizontal)
                        col += 1;
                    else
                        row += 1;

                   wordViewModel.Cells.Add(cell); 
                }
                wordviewmodels.Add((wordViewModel));
            }

            var orderedWords = wordviewmodels.OrderBy(x => x.Index).ToList();
            int lastindex = orderedWords[0].Index;
            for (int i = 0; i < orderedWords.Count; i++)
            {
                if(i > 0 )
                {
                    if (orderedWords[i].Index == lastindex)
                    {
                        lastindex = Words[i].Index;
                        orderedWords[i].Index = orderedWords[i - 1].Index;
                    }
                    else
                    {
                        lastindex = orderedWords[i].Index;
                        orderedWords[i].Index = orderedWords[i - 1].Index + 1;
                    }
                }
                else
                {
                    orderedWords[i].Index = 1;
                }
                Words.Add(orderedWords[i]);
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

        public void AddWordsToBoard()
        {
            foreach (var wordViewModel in Words)
            {
                var firstCellVisited = false;
                foreach (var cell in wordViewModel.Cells)
                {
                    var startPositionForWordOnBoard = string.Empty;

                    var cellPositionOnBoard = (cell.Row*15) + cell.Col;
                    if (!firstCellVisited) startPositionForWordOnBoard = wordViewModel.Index.ToString();
                    firstCellVisited = true;

                    Cells[cellPositionOnBoard] = new CellViewModel(cell.Col, cell.Row, cell.Value, wordViewModel,startPositionForWordOnBoard);
                }
            }
        }

        internal static string GetRandomChar()
        {
            var x = Convert.ToChar(new Random().Next(65, 90)).ToString();
            return x;
        }
    }

}