using System.Linq;
using System.Windows.Input;
using CrossPuzzleClient.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CrossPuzzleClient.ViewModels
{
    public sealed class PuzzlesViewModel : BindableBase
    {
        private ObservableCollection<PuzzleGroupViewModel> _puzzles;
        public string CurrentUser { get; set; }

        public ObservableCollection<PuzzleGroupViewModel> PuzzleGroups
        {
            get { return _puzzles; }
            set { SetProperty(ref _puzzles, value); }
        }

        public ICommand StartPuzzleCommand
        {
            get { return new DelegateCommand(() => StartPuzzle()); }
        }

        private void StartPuzzle()
        {

        }
    }

    public class PuzzleGroupViewModel
    {
        public string Category { get; set; }
        public ObservableCollection<PuzzleViewModel> Puzzles { get; set; }
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

        public string CurrentUser
        {
            get { return _currentUser; }

        }

        public ObservableCollection<PuzzleGroupViewModel> PuzzleGroups
        {
            get { return _puzzles; }
            set { SetProperty(ref _puzzles, value); }
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