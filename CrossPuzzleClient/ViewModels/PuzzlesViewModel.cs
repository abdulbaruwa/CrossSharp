using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CrossPuzzleClient.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CrossPuzzleClient.DataModel;
using CrossPuzzleClient.Infrastructure;
using CrossPuzzleClient.Views;
using GalaSoft.MvvmLight.Messaging;
using Windows.System.UserProfile;

namespace CrossPuzzleClient.ViewModels
{
    public sealed class PuzzlesViewModel : ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly IPuzzleRepository _puzzleRepository;
        private ObservableCollection<PuzzleGroupViewModel> _puzzles = new ObservableCollection<PuzzleGroupViewModel>();
        private PuzzleViewModel _selectedPuzzleViewModel;
        private object _selectedValueBinding;
        private List<PuzzleGroup> _puzzleGroupData = new List<PuzzleGroup>();

        public PuzzlesViewModel(INavigationService navigationService, IPuzzleRepository puzzleRepository)
        {
            navigation = navigationService;
            _puzzleRepository = puzzleRepository;
            RegisterForMessage();
            StartPuzzleCommand = new RelayCommand<PuzzleViewModel>(StartPuzzle);
        }
        public override async void LoadState(object navParameter, Dictionary<string, object> viewModelState)
        {
            PuzzleGroupViewModels = await GetPuzzleGroup();
        }

        public async void LoadState()
        {
            PuzzleGroupViewModels =  await GetPuzzleGroup();
        }

        public async Task<ObservableCollection<PuzzleGroupViewModel>> GetPuzzleGroup()
        {
            var user = await UserInformation.GetFirstNameAsync();
            var puzzleGroups = _puzzleRepository.GetPuzzles(user);
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

            _puzzleRepository.UpdateGameData(PuzzleGroupData, new UserService().GetCurrentUser());
        }

        public string CurrentUser { get; set; }

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
            
            //Messenger.Default.Send<StartPuzzleMessage>(new StartPuzzleMessage() 
            //{PuzzleId = SelectedPuzzleGroupViewModel.PuzzleId});
        }
    }


}