using System.Linq;
using System.Windows.Input;
using CrossPuzzleClient.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CrossPuzzleClient.DataModel;
using CrossPuzzleClient.Infrastructure;
using CrossPuzzleClient.Views;
using GalaSoft.MvvmLight.Messaging;

namespace CrossPuzzleClient.ViewModels
{
    public sealed class PuzzlesViewModel : BindableBase
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
        public ObservableCollection<PuzzleGroupViewModel> PuzzleGroups
        {
            get { return _puzzles; }
            set { SetProperty(ref _puzzles, value);
            }
        }

        public ICommand StartPuzzleCommand
        {
            get { return new DelegateCommand (() => StartPuzzle()); }
        }

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


        private void StartPuzzle()
        {
            object param = "parameter";

            navigation.Navigate<PuzzleBoard>(param);
            
            Messenger.Default.Send<StartPuzzleMessage>(new StartPuzzleMessage() 
            {PuzzleId = SelectedPuzzleGroupViewModel.PuzzleId});
        }
    }

    public class DesignPuzzlesVM : BindableBase
    {
        public DesignPuzzlesVM()
        {
            _puzzles = new ObservableCollection<PuzzleGroupViewModel>();
            _currentUser = "Ademola Baruwa ";
            var sciencegroup = new PuzzleGroupViewModel(){Category = "Science", Puzzles = new ObservableCollection<PuzzleViewModel>()};

            sciencegroup.Puzzles.Add(ViewModelHelper.FakePuzzleBuilder("Human Skeleton Puzzles"));
            sciencegroup.Puzzles.Add(ViewModelHelper.FakePuzzleBuilder("Resperatory System"));
            sciencegroup.Puzzles.Add(ViewModelHelper.FakePuzzleBuilder("Muscle System"));

            PuzzleGroups.Add(sciencegroup);


            var  englishgroup = new PuzzleGroupViewModel() {Category = "English",Puzzles = new ObservableCollection<PuzzleViewModel>()};
            englishgroup.Puzzles.Add(ViewModelHelper.FakePuzzleBuilder("English Vocabs Puzzles"));
            englishgroup.Puzzles.Add(ViewModelHelper.FakePuzzleBuilder("Grammer"));
            PuzzleGroups.Add(englishgroup);
            var geographygroup = new PuzzleGroupViewModel() {Category = "Geography", Puzzles = new ObservableCollection<PuzzleViewModel>()};
            geographygroup.Puzzles.Add(ViewModelHelper.FakePuzzleBuilder("Rivers Puzzles"));
            geographygroup.Puzzles.Add(ViewModelHelper.FakePuzzleBuilder("Tectonic Plates Puzzles"));
            geographygroup.Puzzles.Add(ViewModelHelper.FakePuzzleBuilder("Polution Puzzles"));
            geographygroup.Puzzles.Add(ViewModelHelper.FakePuzzleBuilder("Volcanoes Puzzles"));
            PuzzleGroups.Add(geographygroup);
            
        }

        private string _currentUser;
        private ObservableCollection<PuzzleGroupViewModel> _puzzles;
        private PuzzleGroupViewModel _selectedPuzzleViewModel;

        public string CurrentUser
        {
            get { return _currentUser; }

        }

        public ObservableCollection<PuzzleGroupViewModel> PuzzleGroups
        {
            get { return _puzzles; }
            set { SetProperty(ref _puzzles, value); }
        }

        public PuzzleGroupViewModel SelectedPuzzleGroupViewModel
        {
            get { return _selectedPuzzleViewModel; }
            set { SetProperty(ref _selectedPuzzleViewModel, value); }
        }

        public ObservableCollection<PuzzleViewModel> PuzzleGamesForGroup
        {
            get { return SelectedPuzzleGroupViewModel.Puzzles; }
        }

        public ICommand StartPuzzleCommand
        {
            get { return new DelegateCommand(() => StartPuzzle()); }
        }

        private void StartPuzzle()
        {
            //load the board view, Will pass 

        }
    }

    public static class ViewModelHelper
    {
        public static PuzzleViewModel FakePuzzleBuilder(string title) 
        {
            var puzzleVm = new PuzzleViewModel { Title = title};
            puzzleVm.Words = new List<string>();
            puzzleVm.Words.Add("First");
            puzzleVm.Words.Add("Second");
            puzzleVm.Words.Add("Third");
            puzzleVm.Words.Add("Forth");
            puzzleVm.Words.Add("Fifth");
            puzzleVm.Words.Add("Sixth");
            return puzzleVm;
        }

    }
}