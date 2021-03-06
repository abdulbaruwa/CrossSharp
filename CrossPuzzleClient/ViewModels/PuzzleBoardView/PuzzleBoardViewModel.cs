﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CrossPuzzleClient.Common;
using CrossPuzzleClient.GameStates;
using CrossPuzzleClient.Infrastructure;
using CrossPuzzleClient.Observables;
using CrossPuzzleClient.Services;
using CrossPuzzleClient.ViewModels.PuzzlesView;
using GalaSoft.MvvmLight.Messaging;
using Windows.UI.Xaml.Media.Imaging;
namespace CrossPuzzleClient.ViewModels.PuzzleBoardView
{
    public class PuzzleBoardViewModel : ViewModelBase
    {
        private const string HoursStateName = "Hours";
        private const string MinutesStateName = "Minutes";
        private const string SecondsStateName = "Seconds";
        private const string CellStateName = "Cells";
        private const string WordsStateName = "Words";
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
        private readonly IUserService _userService;
        private CellEmptyViewModel _currentSelectedCell;
        private ObservableCollection<CellEmptyViewModel> _currentSelectedCells;
        private bool _isBoardEnabled;
        private bool _showGameOverPopup;
        private string _gameScoreDisplay;
        private bool _acrossAndDownVisible;
        private bool _wordSelectedVisibility;
        private string _currentUser;
        private BitmapImage _smallImage;

        public PuzzleBoardViewModel(IPuzzlesService puzzlesService, ISchedulerProvider scheduler, IUserService userService)
        {
            _cells = new ObservableCollection<CellEmptyViewModel>();
            _words = new ObservableCollection<WordViewModel>();
            _scheduler = scheduler;
            _userService = userService;
            _puzzlesService = puzzlesService;
            CreateCellsForBoard();
        }

        public override async void LoadState(object navParameter, Dictionary<string, object> viewModelState)
        {
            var puzzleViewModelSerialized = JsonUtility.FromJson<PuzzleViewModel>(navParameter.ToString());
            var loadUserImageAsyncTask = _userService.LoadUserImageAsync();
            RegisterForMessage();
            CurrentUser = await _userService.GetCurrentUserAsync();
            CurrentGameState = new GameNotStartedState(this);
            var puzzleViewModel = puzzleViewModelSerialized as PuzzleViewModel;
            PuzzleViewModel = puzzleViewModel;
            if (puzzleViewModel != null) LoadPuzzleBoardForSelectedPuzzleId();
            if (viewModelState != null && viewModelState.ContainsKey(CellStateName) && viewModelState.ContainsKey(WordsStateName))
            {
                DeserializeAndUpdateWordsAndCellsData(viewModelState);
            }
            SmallImage = await loadUserImageAsyncTask;
        }

        public PuzzleViewModel PuzzleViewModel { get; set; }

        private async void DeserializeAndUpdateWordsAndCellsData(Dictionary<string, object> viewModelState)
        {
            var cells = await Task.Run(() =>  JsonUtility.FromJson<List<CellEmptyViewModel>>(viewModelState[CellStateName].ToString()));
            var words = await Task.Run(() => JsonUtility.FromJson<List<KeyValuePair<string,bool>>>(viewModelState[WordsStateName].ToString()));
            _hours = viewModelState.ContainsKey(HoursStateName) ? Convert.ToInt32(viewModelState[HoursStateName]) : 0;
            _minutes = viewModelState.ContainsKey(MinutesStateName) ? Convert.ToInt32(viewModelState[MinutesStateName]) : 0;
            _seconds = viewModelState.ContainsKey(SecondsStateName) ? Convert.ToInt32(viewModelState[SecondsStateName]) : 0;
            SetGameCountDown();
            UpdateCellsAndWordCellsEnteredValuesWithValuesFrom(cells);
            await Task.Run(() =>
                               {
                                   (from w in words
                                           from y in Words
                                           where w.Key == y.Word
                                           select y.EnteredValueAddedToBoard = w.Value).ToArray();
                               }
                );
        }

        public override void SaveState(Dictionary<string, object> viewModelState)
        {
            if (viewModelState != null)
            {
                var cells = Cells.Select(x =>new CellEmptyViewModel(x.Col, x.Row, x.Value) {EnteredValue = x.EnteredValue}
                    ).ToArray();
                viewModelState[CellStateName] = JsonUtility.ToJson(cells);

                var wordsEnteredOnBoard = Words.Select(x => new KeyValuePair<string, bool>(x.Word,x.EnteredValueAddedToBoard)).ToArray();
                viewModelState[WordsStateName] = JsonUtility.ToJson(wordsEnteredOnBoard);

                viewModelState[HoursStateName] = _hours;
                viewModelState[MinutesStateName] = _minutes;
                viewModelState[SecondsStateName] = _seconds;
                
            }
        }

        private void UpdateCellsAndWordCellsEnteredValuesWithValuesFrom(List<CellEmptyViewModel> cells)
        {
            foreach (var cell in cells)
            {
                var cellref = Cells.FirstOrDefault(x => x.Row == cell.Row && x.Col == cell.Col);
                if (cellref != null)
                {
                    cellref.EnteredValue = cell.EnteredValue;
                    //Words also have Cells. Now these cells may have values in their 'EnteredValue' field
                    //so we update also to match the session values
                    //Get from Words in viewmodel the matching cell and update it's EnteredValue

                    
                    foreach(var wordWithCellRef in Words.Where(x => x.Cells.Any(y => y.Col == cellref.Col && y.Row == cellref.Row)))
                    {
                        var wordCellRef = wordWithCellRef.Cells.FirstOrDefault(z => z.Col == cellref.Col && z.Row == cellref.Row);
                        if (wordCellRef != null)
                        {
                            wordCellRef.EnteredValue = cell.EnteredValue;
                        }
                    }
                }
            }
        }

        public string DisplayTitle
        {
            get { return PuzzleViewModel != null ? PuzzleViewModel.Group + "-" + PuzzleViewModel.Title : string.Empty; }
            //get { return "Puzzle"; }
        }

        public BitmapImage SmallImage
        {
            get { return _smallImage; }
            set { SetProperty(ref  _smallImage, value); }
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
            }
        }

        public bool AcrossAndDownVisible
        {
            get { return _acrossAndDownVisible; }
            set { SetProperty(ref _acrossAndDownVisible, value); }
        }

        public bool WordSelectedVisibility
        {
            get { return _wordSelectedVisibility; }
            set { SetProperty(ref _wordSelectedVisibility, value); }
        }

        public bool IsBoardEnabled
        {
            get { return _isBoardEnabled; }
        }

        private void SetLikelyWordMatchOnBoardForSelectedCell(CellEmptyViewModel value)
        {
           
            var words = Words.Where(x => x.Cells.Count(y => y.Col == value.Col && y.Row == value.Row) > 0
                    );
            var word = CurrentGameState is GameFinishedWithErrorsState
                                     ? words.FirstOrDefault(x => x.IsWordAnswerCorrect == false)
                                     : words.FirstOrDefault();
            if (word == null)
            {
                SelectedWord = null;
                return;
            }
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
                //Before property change
                if (_selectedWord != null )
                {
                    SetBackgroundColourFlagForWordCellsTo(CellState.IsUsed);
                }
                SetProperty(ref _selectedWord, value);

                //After property change
                if (_selectedWord != null)
                {
                    SetBackgroundColourFlagForWordCellsTo(CellState.IsActive);
                    _selectedWord.AcceptCellValueChanges();
                    ShowSelectedWordIfErrored();
                }
            }
        }

        private void ShowSelectedWordIfErrored()
        {
            if (!(CurrentGameState is GameFinishedWithErrorsState)) return;
            WordSelectedVisibility = !SelectedWord.IsWordAnswerCorrect;
        }

        private void SetBackgroundColourFlagForWordCellsTo(CellState cellState)
        {
            if (!(CurrentGameState is GameInProgressState)) return;
            foreach (var cell in _selectedWord.Cells)
            {
                var cell1 = cell;
                Cells.First(x => x.Row == cell1.Row && x.Col == cell1.Col).IsVisible = cellState;
            }
        }

        private void StopGameIfFinished()
        {
            if(Words.Count(x => x.EnteredValueAddedToBoard) == Words.Count)
            {
                var gameScore = Convert.ToInt32(GetGameScore());

                var finishedGameState = gameScore == 100
                                                   ? (IGameState) new GameFinishedState(this)
                                                   : new GameFinishedWithErrorsState(this);

                if(CurrentGameState.CanBecome(finishedGameState)) GameStateBecome(finishedGameState);

                GameIsRunning = false;
            }
        }

        public void FireGameCompleteMessage()
        {
            var score = Convert.ToInt32(GetGameScore());
            Messenger.Default.Send(new GameCompleteMessage(){ScorePercentage = score, UserName= CurrentUser, GameId = PuzzleViewModel.PuzzleId});
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

        public string CurrentUser
        {
            get { return _currentUser; }
            set { SetProperty(ref _currentUser,value);}
        }

        public bool ShowCompleteTick
        {
            get { return _showCompleteTick; }
            set { SetProperty(ref _showCompleteTick, value); }
        }

        public ICommand AddEnteredWordOnToBoardCommand
        {
           get{return new DelegateCommand(AddEnteredWordOnToBoard);} 
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
            //Refactoring to use State Machine
            IGameState gameStateToBecome = null;
            if (CurrentGameState is GameNotStartedState || CurrentGameState is GamePauseState)
            {
                gameStateToBecome = new GameInProgressState(this);
            }
            else if(CurrentGameState is GameInProgressState)
            {
                gameStateToBecome = new GamePauseState(this);
            }

            if (gameStateToBecome == null) return;
            GameStateBecome(gameStateToBecome);

        }

        public void GameStateBecome(IGameState newGameState)
        {
            if (CurrentGameState.CanBecome(newGameState))
            {
                SetGameStatusForNewState(newGameState);
                newGameState.Become(CurrentGameState);

                SetDisplayForAcrossAndDownControls();
            }
        }

        private void SetDisplayForAcrossAndDownControls()
        {
            AcrossAndDownVisible = CurrentGameState is GameFinishedWithErrorsState ||
                                   CurrentGameState is GameInProgressState || CurrentGameState is GameNotStartedState;
        }

        
        private void SetGameStatusForNewState(IGameState newGameState)
        {
            GameIsRunning = newGameState is GameInProgressState;
        }

        public void PauseTimer()
        {
            if (_counter != null)
            {
                using (_counter)
                {
                }
            }
        }

        public void StopTime()
        {
            if (_gameIsRunning && _counter != null)
            {
                using (_counter)
                {
                }
            }
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
                    SetGameCountDown();
                    //GameCountDown = ConvertIntTwoUnitStringNumber(_hours) + ":" +
                    //                ConvertIntTwoUnitStringNumber(_minutes) + ":" +
                    //                ConvertIntTwoUnitStringNumber(_seconds);
                }
               );
        }

        private void SetGameCountDown()
        {
            GameCountDown = ConvertIntTwoUnitStringNumber(_hours) + ":" +
                   ConvertIntTwoUnitStringNumber(_minutes) + ":" +
                   ConvertIntTwoUnitStringNumber(_seconds);
        }

        private string ConvertIntTwoUnitStringNumber(int number)
        {
            if (number < 10)
                return "0" + number.ToString();
            return number.ToString();
        }

        private void AddEnteredWordOnToBoard()
        {
            UpdateCellsEnteredValuesWithValuesFrom(SelectedWord.Cells);
            SelectedWord.EnteredValueAddedToBoard = true;
            StopGameIfFinished();
        }

        private void UpdateCellsEnteredValuesWithValuesFrom(IEnumerable<CellEmptyViewModel> cells)
        {
            foreach (var cell in cells )
            {
                var cellref = Cells.FirstOrDefault(x => x.Row == cell.Row && x.Col == cell.Col);
                if (cellref != null)
                {
                    cellref.EnteredValue = cell.EnteredValue;
                }
            }
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

        public GameState CurrentGameState { get; set; }

        private void RegisterForMessage()
        {
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

        private void LoadPuzzleBoardForSelectedPuzzleId()
        {

            //GameId = this.PuzzleViewModel.PuzzleId;
            Words = _puzzlesService.GetOrdereredWordsForPuzzle(PuzzleViewModel.PuzzleId,CurrentUser);
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
         
                    Cells[cellPositionOnBoard] = new CellViewModel(cell.Col, cell.Row, cell.Value, wordViewModel, startPositionForWordOnBoard);
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