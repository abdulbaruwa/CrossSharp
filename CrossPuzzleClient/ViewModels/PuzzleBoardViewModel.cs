using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CrossPuzzleClient.Common;
using GalaSoft.MvvmLight.Messaging;

namespace CrossPuzzleClient.ViewModels
{
    public class PuzzleBoardViewModel : BindableBase
    {
        private readonly ObservableCollection<CellEmptyViewModel> _cells;
        private int _cols;
        private int _currentWordPosition;
        private string _puzzleId;
        protected IPuzzlesService _puzzlesService;
        private int _rows;
        private WordViewModel _selectedWord;
        private WordViewModel _selectedWordAcross;
        private WordViewModel _selectedWordDown;
        private ObservableCollection<WordViewModel> _words;
        private bool _showCompleteTick;

        public PuzzleBoardViewModel(IPuzzlesService puzzlesService)
        {
            _cells = new ObservableCollection<CellEmptyViewModel>();
            _words = new ObservableCollection<WordViewModel>();
            _puzzlesService = puzzlesService;
            CreateCellsForBoard();
            RegisterForMessage();
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

        public WordViewModel SelectedWord
        {
            get { return _selectedWord; }
            set { SetProperty(ref _selectedWord, value); }
        }

        public bool ShowCompleteTick
        {
            get { return _showCompleteTick; }
            set { SetProperty(ref _showCompleteTick, value); }
        }

        public ObservableCollection<WordViewModel> WordsAcross
        {
            get { return new ObservableCollection<WordViewModel>(Words.Where(x => x.Direction == Direction.Across)); }
        }

        public ObservableCollection<WordViewModel> WordsDown
        {
            get { return new ObservableCollection<WordViewModel>(Words.Where(x => x.Direction == Direction.Down)); }
        }

        public WordViewModel SelectedWordDown
        {
            get { return _selectedWordDown; }
            set
            {
                SetProperty(ref _selectedWordDown, value);
                if (value != null)
                {
                    SelectedWord = value;
                    _currentWordPosition = 0;
                }
                if (_selectedWordAcross != null && value != null)
                    SelectedWordAcross = null;
            }
        }

        private bool SetShowCompleteTick()
        {
            if (SelectedWord == null) return false;
            return _currentWordPosition == SelectedWord.Cells.Count;
        }

        public WordViewModel SelectedWordAcross
        {
            get { return _selectedWordAcross; }
            set
            {
                SetProperty(ref _selectedWordAcross, value);
                if (value != null)
                {
                    SelectedWord = value;
                    _currentWordPosition = 0;

                }
                if (_selectedWordDown != null && value != null)
                    SelectedWordDown = null;
            }
        }

        private void RegisterForMessage()
        {
            Messenger.Default.Register<StartPuzzleMessage>(this, m => LoadPuzzleBoardForSelectedPuzzleId(m.PuzzleId));
            Messenger.Default.Register<KeyReceivedMessage>(this, m => HandleKeyEvent(m));
        }

        private void HandleKeyEvent(KeyReceivedMessage keyReceivedMessage)
        {
            if (_currentWordPosition < SelectedWord.Cells.Count)
            {
                SelectedWord.Cells[_currentWordPosition].EnteredValue = keyReceivedMessage.KeyChar;
                _currentWordPosition += 1;                   
                ShowCompleteTick = SetShowCompleteTick();
            }
        }

        private void LoadPuzzleBoardForSelectedPuzzleId(int puzzleId)
        {
            Words = _puzzlesService.GetOrdereredWordsForPuzzle(puzzleId);
            AddWordsToBoard();
        }

        private void CreateCellsForBoard()
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
            foreach (WordViewModel wordViewModel in Words)
            {
                bool firstCellVisited = false;
                foreach (CellEmptyViewModel cell in wordViewModel.Cells)
                {
                    string startPositionForWordOnBoard = string.Empty;

                    int cellPositionOnBoard = (cell.Row*12) + cell.Col;
                    if (!firstCellVisited) startPositionForWordOnBoard = wordViewModel.Index.ToString();
                    firstCellVisited = true;

                    Cells[cellPositionOnBoard] = new CellViewModel(cell.Col, cell.Row, cell.Value, wordViewModel,
                                                                   startPositionForWordOnBoard);
                }
            }
        }

        internal static string GetRandomChar()
        {
            string x = Convert.ToChar(new Random().Next(65, 90)).ToString();
            return x;
        }
    }
}