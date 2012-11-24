using System.Collections.ObjectModel;
using System.Windows.Input;
using CrossPuzzleClient.Common;
using CrossPuzzleClient.ViewModels.PuzzlesView;

namespace CrossPuzzleClient.ViewModels.DesignTime
{
    public class DesignPuzzlesVm : BindableBase
    {
        public DesignPuzzlesVm()
        {
            _puzzles = new ObservableCollection<PuzzleGroupViewModel>();
            CurrentUser = "Ademola Baruwa ";
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

        private string _currentUser = "BAruwa";
        private ObservableCollection<PuzzleGroupViewModel> _puzzles;
        private PuzzleGroupViewModel _selectedPuzzleViewModel;

        public string CurrentUser
        {
            get { return _currentUser; }
            set { SetProperty(ref _currentUser, value); }

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
}