using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;
using CrossPuzzleClient.Common;
using CrossPuzzleClient.Observables;
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
        private IDisposable _counter;
        private ISchedulerProvider _scheduler;
        private CellEmptyViewModel _currentSelectedCell;
        private ObservableCollection<CellEmptyViewModel> _currentSelectedCells;
        private bool _isBoardEnabled;
        private bool _showGameOverPopup;
        private string _gameScoreDisplay;

        public PuzzleBoardViewModel(IPuzzlesService puzzlesService, ISchedulerProvider scheduler)
        {
            _cells = new ObservableCollection<CellEmptyViewModel>();
            _words = new ObservableCollection<WordViewModel>();
            _scheduler = scheduler;
            _puzzlesService = puzzlesService;
            CreateCellsForBoard();
            RegisterForMessage();
        }


        public ISchedulerProvider SchedulerProvider
        {
            get { return _scheduler; }
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

        public ObservableCollection<CellEmptyViewModel> CurrentSelectedCells
        {
            get { return _currentSelectedCells; }
        }

        public CellEmptyViewModel CurrentSelectedCell
        {
            get { return _currentSelectedCell; }
            set
            {
                SetProperty(ref _currentSelectedCell, value);
                SetLikelyWordMatchOnBoardForSelectedCell(value);
                ShowCorrections(value);
            }
        }

        private void ShowCorrections(CellEmptyViewModel cellEmptyViewModel)
        {
            //Display the word entrie control and use it to show correct value of errorneous answer
            //First determine which is the failing word based on the offered cell.
            //Then handl
            throw new NotImplementedException();
        }

        public bool IsBoardEnabled
        {
            get { return _isBoardEnabled; }
        }

        private void SetLikelyWordMatchOnBoardForSelectedCell(CellEmptyViewModel value)
        {
            var word = Words.FirstOrDefault(x => 
                                    x.Cells.Count(y => y.Col == value.Col && y.Row == value.Row) > 0
                                );

            if(word == null) return;
            if (word.Direction == Direction.Down)
            {
                SelectedWordDown = word;
            }
            else
            {
                SelectedWordAcross = word;
            }
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
                if (_selectedWord != null)
                {
                    SetCellsForWordsTo(CellState.IsUsed);
                    _selectedWord.RejectCellValueChanges();
                }
                SetProperty(ref _selectedWord, value);
                if (_selectedWord != null)
                {
                    SetCellsForWordsTo(CellState.IsActive);
                    _selectedWord.AcceptCellValueChanges();
                }
            }
        }

        private void SetCellsForWordsTo(CellState isActive)
        {
            foreach (var cell in _selectedWord.Cells)
            {
                var cell1 = cell;
                Cells.First(x => x.Row == cell1.Row && x.Col == cell1.Col).IsVisible = isActive;
            }
        }

        private void StopGameIfFinished()
        {
            if(Words.Count(x => x.EnteredValueAddedToBoard) == Words.Count)
            {
                StopGame();
                GameIsRunning = false;
                FireGameCompleteMessage();
            }
        }

        private void FireGameCompleteMessage()
        {
            var score = Convert.ToInt32(GetGameScore());

            Messenger.Default.Send(new GameCompleteMessage(){ScorePercentage = score, UserName= "Abdul"});
        }

        private double GetGameScore()
        {
            var correctCount = Convert.ToDouble(Words.Count(x => x.IsWordAnswerCorrect));
            var wordCount = Convert.ToDouble(Words.Count());
            return (correctCount/wordCount)*100;
        }

        public bool GameIsRunning
        {
            get { return _gameIsRunning; }
            set { SetProperty(ref _gameIsRunning, value); }
        }

        public bool ShowGameOverPopup
        {
            get { return _showGameOverPopup; }
            set{SetProperty(ref _showGameOverPopup, value);}
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

        public string GameScoreDisplay
        {
            get { return _gameScoreDisplay; }
            set { SetProperty(ref _gameScoreDisplay, value); }
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
            //Pause by dispossing current Observable.
            StopTime();
            GameIsRunning = !_gameIsRunning;

            SetStartPauseDisplayCommand();
            if(_gameIsRunning) GameCountUpCommand.Execute(null);

        }

        private void StopTime()
        {
            if (_gameIsRunning && _counter != null)
            {
                using (_counter)
                {
                }
            }
        }

        private void StopGame()
        {
           StopTime();
           StartPauseButtonCaption = "Start";
        }

        public void BeginCount()
        {
            //var scheduler = new DispatcherScheduler(Application.Current. Dispatcher);
            var secondsObserver = Observable.Interval(TimeSpan.FromSeconds(1));

            _counter = secondsObserver.ObserveOn(_scheduler.Dispatcher).SubscribeOn(_scheduler.ThreadPool).Subscribe(
            //_counter = secondsObserver.ObserveOnDispatcher().Subscribe(
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
            SelectedWord.EnteredValueAddedToBoard = true;
            StopGameIfFinished();

        }

        public ObservableCollection<WordViewModel> WordsAcross
        {
            get { return new ObservableCollection<WordViewModel>(Words.Where(x => x.Direction == Direction.Across)); }
        }

        private void GameComplete(GameCompleteMessage gameCompleteMessage)
        {
            ShowGameOverPopup = true;
            HiglightFailingWords();
            GameScoreDisplay = string.Format("You scored {0}%", gameCompleteMessage.ScorePercentage);
        }

        private void HiglightFailingWords()
        {
            var erroredCell = from word in Words
                       from cells in word.Cells
                       where word.IsWordAnswerCorrect == false
                       select cells;
            foreach (var cellEmptyViewModel in erroredCell)
            {
                CellEmptyViewModel model = cellEmptyViewModel;
                this.Cells.First(x => x.Col == model.Col && x.Row == model.Row).IsVisible = CellState.IsError;
            }
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
                ResetValueForEnteredWordIfNotAddedToTheBoard();

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

        private void ResetValueForEnteredWordIfNotAddedToTheBoard()
        {
            if (_selectedWord != null && ! _selectedWord.EnteredValueAddedToBoard)
            {
                var wordTypedButNotAddedToBoard = Words.FirstOrDefault(x => x == SelectedWord);

                foreach (var lastselectedWordCell in wordTypedButNotAddedToBoard.Cells)
                {
                    lastselectedWordCell.EnteredValue = null;
                }
            }
        }

        private void SetSelectedWordCurrentCellPosition(WordViewModel value)
        {
            _currentWordPosition = 0;
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
                ResetValueForEnteredWordIfNotAddedToTheBoard();

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
            Messenger.Default.Register<GameCompleteMessage>(this, m => GameComplete(m));
        }

        private void HandleKeyEvent(KeyReceivedMessage keyReceivedMessage)
        {
            if(SelectedWord == null) return;
            switch (keyReceivedMessage.KeyCharType)
            {
                case KeyCharType.Delete:
                case KeyCharType.BackSpace:
                    if (_currentWordPosition > 0)
                    {
                        _currentWordPosition -= 1;
                        SelectedWord.Cells[_currentWordPosition].EnteredValue = string.Empty;
                    }
                    break;
                default:
                    if (_currentWordPosition < SelectedWord.Cells.Count)
                    {
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
                    }
                    break;
            }
            ShowCompleteTick = SetShowCompleteTick();
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