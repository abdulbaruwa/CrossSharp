using System;
using System.Linq;
using CrossPuzzleClient.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CrossPuzzleClient.ViewModels
{
    public class PuzzleBoardViewModel : BindableBase
    {
        private readonly ObservableCollection<CellEmptyViewModel> _cells;
        private int _cols;
        private int _rows;
        private ObservableCollection<WordViewModel> _words;
        private string _puzzleId;
        protected IPuzzlesService _puzzlesService;

        public PuzzleBoardViewModel(IPuzzlesService puzzlesService)
        {
            _cells = new ObservableCollection<CellEmptyViewModel>();
            _words = new ObservableCollection<WordViewModel>();
            _puzzlesService = puzzlesService;
            CreateBubblesAsync();
            RegisterForMessage();
        }

        private void RegisterForMessage()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<StartPuzzleMessage>(this, m => LoadPuzzleBoardForSelectedPuzzleId(m.PuzzleId));
        }

        private void LoadPuzzleBoardForSelectedPuzzleId(int puzzleId)
        {
            Words = _puzzlesService.GetOrdereredWordsForPuzzle(puzzleId);
            AddWordsToBoard();
        }

        public string PuzzleId
        {
            get { return _puzzleId; }
            set { SetProperty(ref _puzzleId, value); }
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
                from row in Enumerable.Range(0, 12)
                from col in Enumerable.Range(0, 12)
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

                    var cellPositionOnBoard = (cell.Row*12) + cell.Col;
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