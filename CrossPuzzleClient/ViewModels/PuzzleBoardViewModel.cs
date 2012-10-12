using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;
using CrossPuzzleClient.Common;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using Windows.UI.Xaml;

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
        private string _userName = "Abdul";
        private string _startPauseButtonCaption = "Start";
        private string _gameCountDown = "00:00:00";
        private bool _gameIsRunning;
        private int _seconds;
        private int _minutes;
        private int _hours;
        private int _days;
        private ITimeCounter _timeCounter;
        private ICommand _gameCountUp;

        public PuzzleBoardViewModel(IPuzzlesService puzzlesService)
        {
            _cells = new ObservableCollection<CellEmptyViewModel>();
            _words = new ObservableCollection<WordViewModel>();
            _puzzlesService = puzzlesService;
            CreateCellsForBoard();
            RegisterForMessage();
        }

        public void SetTimeCounter(ITimeCounter timeCounter)
        {
            _timeCounter = timeCounter;
        }

        public void SetGameCountUpCommand(ICommand gameCountUpCommand)
        {
            _gameCountUp = gameCountUpCommand;
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
            set
            {
                if(_selectedWord != null) _selectedWord.RejectCellValueChanges();
                SetProperty(ref _selectedWord, value);
                if (_selectedWord != null) _selectedWord.AcceptCellValueChanges();
            }
        }

        public string GameCountDown
        {
            get { return _gameCountDown; }
            set { SetProperty(ref _gameCountDown, value); }
        }
        public string StartPauseButtonCaption
        {
            get { return _startPauseButtonCaption; }
            set { SetProperty(ref _startPauseButtonCaption,value); }
        }

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName,value);}
        }

        public bool ShowCompleteTick
        {
            get { return _showCompleteTick; }
            set { SetProperty(ref _showCompleteTick, value); }
        }

        public ICommand AddWordToBoardCommand
        {
           get{return new DelegateCommand(LoadWordToBoard);} 
        }

        public ICommand GameCountUpCommand
        {
            get
            {
                if(_gameCountUp == null)return new DelegateCommand(BeginCount);
                return _gameCountUp;
            }
        }
        public ICommand StartPauseCommand
        {
            get { return new DelegateCommand(StartPauseGame); }
        }

        public void SetStartPauseDisplayCommand()
        {
            StartPauseButtonCaption = _gameIsRunning ? "Pause" : "Start";
        }

        
        private void StartPauseGame()
        {
            _gameIsRunning = !_gameIsRunning;
            SetStartPauseDisplayCommand();
            GameCountUpCommand.Execute(null);

            //var secondsObserver = Observable.Interval(new TimeSpan(1000));
            //secondsObserver.ObserveOnDispatcher().Subscribe(
            //    x =>
            //    {
            //        if (_seconds == 59 && _minutes == 59 && _hours == 59)
            //        {
            //            _seconds = 0;
            //            _minutes = 0;
            //            _hours = 0;
            //            _days = _days + 1;
            //        }
            //        else if (_seconds == 59 && _minutes == 59)
            //        {
            //            _seconds = 0;
            //            _minutes = 0;
            //            _hours = _hours + 1;
            //        }
            //        else if (_seconds == 59)
            //        {
            //            _seconds = 0;
            //            _minutes = _minutes + 1;
            //        }
            //        else
            //        {
            //            _seconds = _seconds + 1;
            //        }

            //        GameCountDown = ConvertIntTwoUnitStringNumber(_hours) + ":" +
            //                        ConvertIntTwoUnitStringNumber(_minutes) + ":" +
            //                        ConvertIntTwoUnitStringNumber(_seconds);

            //    }
            //   );
        }

        public void BeginCount()
        {
            //var scheduler = new DispatcherScheduler(Application.Current. Dispatcher);
            var secondsObserver = Observable.Interval(TimeSpan.FromSeconds(1));
            secondsObserver.ObserveOnDispatcher().Subscribe(
                x =>
                {
                    if (_seconds == 59 && _minutes == 59 && _hours == 59)
                    {
                        _seconds = 0;
                        _minutes = 0;
                        _hours = 0;
                        _days = _days + 1;
                    }
                    else if (_seconds == 59 && _minutes == 59)
                    {
                        _seconds = 0;
                        _minutes = 0;
                        _hours = _hours + 1;
                    }
                    else if (_seconds == 59)
                    {
                        _seconds = 0;
                        _minutes = _minutes + 1;
                    }
                    else
                    {
                        _seconds = _seconds + 1;
                    }

                    GameCountDown = ConvertIntTwoUnitStringNumber(_hours) + ":" +
                                    ConvertIntTwoUnitStringNumber(_minutes) + ":" +
                                    ConvertIntTwoUnitStringNumber(_seconds);

                }
               );
        }
        private string ConvertIntTwoUnitStringNumber(int number)
        {
            if (number < 10)
                return "0" + number.ToString();
            return number.ToString();
        }

        private void LoadWordToBoard()
        {
            foreach (var cell in SelectedWord.Cells)
            {
                Cells.First(x => x.Row == cell.Row && x.Col == cell.Col).EnteredValue = cell.EnteredValue;
            }
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
                    SetSelectedWordCurrentCellPosition(value);
                }
                if (_selectedWordAcross != null && value != null)
                    SelectedWordAcross = null;
            }
        }

        private void SetSelectedWordCurrentCellPosition(WordViewModel value)
        {
            _currentWordPosition = 0;

            //if (value.Cells.Any(x => string.IsNullOrEmpty(x.EnteredValue) == false))
            //{
            //    for (int index = 0; index < value.Cells.Count; index++)
            //    {
            //        var cell = value.Cells[index];
            //        if (string.IsNullOrEmpty(cell.EnteredValue))
            //        {
            //            _currentWordPosition = index;
            //        }
            //    }
            //}
            //else
            //{
            //    _currentWordPosition = 0;
            //}
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
                    SetSelectedWordCurrentCellPosition(value);
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
                switch (keyReceivedMessage.KeyCharType)
                {
                    case KeyCharType.Delete:
                    case KeyCharType.BackSpace:
                        _currentWordPosition -= 1;
                        SelectedWord.Cells[_currentWordPosition].EnteredValue = string.Empty;
                        break;
                    default:
                        SelectedWord.Cells[_currentWordPosition].EnteredValue = keyReceivedMessage.KeyChar;
                        var cellValueChangedMesage =
                            new CellValueChangedMessage()
                                {
                                    Character = keyReceivedMessage.KeyChar,
                                    Col = SelectedWord.Cells[_currentWordPosition].Col,
                                    Row = SelectedWord.Cells[_currentWordPosition].Row
                                };
                        Messenger.Default.Send(cellValueChangedMesage);
                        _currentWordPosition += 1;                   
                        break;
                }
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