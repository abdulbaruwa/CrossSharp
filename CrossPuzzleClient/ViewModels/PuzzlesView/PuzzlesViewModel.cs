using System.Linq;
using System.Threading.Tasks;
using CrossPuzzleClient.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CrossPuzzleClient.DataModel;
using CrossPuzzleClient.Infrastructure;
using CrossPuzzleClient.Services;
using CrossPuzzleClient.ViewModels.PuzzleBoardView;
using CrossPuzzleClient.Views;
using GalaSoft.MvvmLight.Messaging;
using Windows.UI.Xaml.Media.Imaging;

namespace CrossPuzzleClient.ViewModels.PuzzlesView
{
    public sealed class PuzzlesViewModel : ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly IPuzzleRepository _puzzleRepository;
        private readonly IUserService _userService;
        private ObservableCollection<PuzzleGroupViewModel> _puzzles = new ObservableCollection<PuzzleGroupViewModel>();
        private PuzzleViewModel _selectedPuzzleViewModel;
        private object _selectedValueBinding;
        private List<PuzzleGroup> _puzzleGroupData = new List<PuzzleGroup>();
        private string _currentUser;
        private BitmapImage _smallImage;

        public PuzzlesViewModel(INavigationService navigationService, IPuzzleRepository puzzleRepository, IUserService userService)
        {
            navigation = navigationService;
            _puzzleRepository = puzzleRepository;
            _userService = userService;

            RegisterForMessage();
            StartPuzzleCommand = new RelayCommand<PuzzleViewModel>(StartPuzzle);
        }

        public override async void LoadState(object navParameter, Dictionary<string, object> viewModelState)
        {
            CurrentUser = await _userService.GetCurrentUserAsync();
            SmallImage = await _userService.LoadUserImageAsync();
            var puzzleGroups = await Task.Run(() => _puzzleRepository.GetPuzzles(CurrentUser));
            PuzzleGroupData = puzzleGroups;
            PuzzleGroupViewModels = await GetPuzzleGroup(puzzleGroups);
        }

        public BitmapImage SmallImage
        {
            get { return _smallImage; }
            set { SetProperty(ref _smallImage, value); }
        }


        public async Task<ObservableCollection<PuzzleGroupViewModel>> GetPuzzleGroup(List<PuzzleGroup> puzzleGroups)
        {
            var puzzleGroupViewModels = new ObservableCollection<PuzzleGroupViewModel>();
            foreach (var puzzleGroup in puzzleGroups)
            {
                var group = new PuzzleGroupViewModel() { Category = puzzleGroup.Name, Puzzles = new ObservableCollection<PuzzleViewModel>() };
                foreach (var puzzle in puzzleGroup.Puzzles)
                {
                    group.Puzzles.Add(new PuzzleViewModel() { Title = puzzle.Title, PuzzleId = puzzle.PuzzleSubGroupId, GameScore = puzzle.GameScore });
                }
                puzzleGroupViewModels.Add(group);
            }

            return puzzleGroupViewModels;
        }

        private void RegisterForMessage()
        {
            Messenger.Default.Register<GameCompleteMessage>(this, m => SaveGameDataFromGameCompleteMessage(m));
        }

        private void SaveGameDataFromGameCompleteMessage(GameCompleteMessage gameCompleteMessage)
        {
            var puzzleGame = (from p in PuzzleGroupData
                             from ps in p.Puzzles
                             where ps.PuzzleSubGroupId == gameCompleteMessage.GameId
                             select ps).FirstOrDefault();
            if (puzzleGame != null)
            {
                puzzleGame.GameScore = gameCompleteMessage.ScorePercentage;
            }

            _puzzleRepository.UpdateGameData(PuzzleGroupData,CurrentUser);
        }

        public string CurrentUser
        {
            get { return _currentUser; }
            set { SetProperty(ref _currentUser, value); }
        }

        public ObservableCollection<PuzzleGroupViewModel> PuzzleGroupViewModels
        {
            get { return _puzzles; }
            set { SetProperty(ref _puzzles, value);
            }
        }

        public RelayCommand<PuzzleViewModel> StartPuzzleCommand { get; private set; }

        public PuzzleViewModel SelectedPuzzleGroupViewModel
        {
            get { return _selectedPuzzleViewModel; }
            set
            {
                SetProperty(ref _selectedPuzzleViewModel, value);
            }
        }
        
        public object SelectedValueBinding
        {
            get { return _selectedValueBinding; }
        }

        public List<PuzzleGroup> PuzzleGroupData
        {
            get { return _puzzleGroupData; }
            set { SetProperty(ref _puzzleGroupData, value); }
        }

        private void StartPuzzle(PuzzleViewModel puzzleViewModel)
        {
            navigation.Navigate<PuzzleBoard>(puzzleViewModel);
        }
    }
}