using System.Windows.Input;
using CrossPuzzleClient.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CrossPuzzleClient.DataModel;
using CrossPuzzleClient.Infrastructure;
using CrossPuzzleClient.Views;
using Windows.UI.Xaml.Data;

namespace CrossPuzzleClient.ViewModels
{
    public sealed class PuzzlesViewModel : BindableBase
    {

        private readonly INavigationService navigation;
        private readonly IPuzzleRepository _puzzleRepository;
        private ObservableCollection<PuzzleGroupViewModel> _puzzles = new ObservableCollection<PuzzleGroupViewModel>();
        private object _selectedPuzzleViewModel;
        private object _selectedValueBinding;

        public PuzzlesViewModel(INavigationService navigationService, IPuzzleRepository puzzleRepository)
        {
            navigation = navigationService;
            _puzzleRepository = puzzleRepository;
            
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
            get { return new DelegateCommand(() => StartPuzzle()); }
        }

        public object SelectedPuzzleGroupViewModel
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


        private void StartPuzzle()
        {
            object param = "parameter";
            navigation.Navigate<PuzzleBoard>(param);

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<StartPuzzleMessage>(new StartPuzzleMessage() 
            {PuzzleId = 0});
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