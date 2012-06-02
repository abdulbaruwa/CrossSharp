using System.Collections.Generic;
using System.Collections.ObjectModel;
using CrossSharpClient.Common;

namespace CrossSharpClient.DataModel
{
    public sealed class PuzzlesViewModel : BindableBase
    {
        private ObservableCollection<PuzzleViewModel> _puzzles;
        public string CurrentUser { get; set; }

        public ObservableCollection<PuzzleViewModel> Puzzles
        {
            get { return _puzzles; }
            set { SetProperty(ref _puzzles, value); }
        }

        internal List<G> 
    }

    public class PuzzleViewModel
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public List<string> Words { get; set; }
    }

    public class DesignPuzzlesVM : BindableBase
    {
        public DesignPuzzlesVM()
        {
            _puzzles = new ObservableCollection<PuzzleViewModel>();

            _currentUser = "Ademola Abdulrasheed Dabira Adedayo Baruwa ";
            _puzzles.Add(ViewModelHelper.FakePuzzleBuilder("Science Puzzles"));
            _puzzles.Add(ViewModelHelper.FakePuzzleBuilder("Englis Vocabs Puzzles"));
            _puzzles.Add(ViewModelHelper.FakePuzzleBuilder("Geography Rivers Puzzles"));
        }

        private string _currentUser;
        private ObservableCollection<PuzzleViewModel> _puzzles  ;

        public string CurrentUser
        {
            get { return _currentUser; }

        }

        public ObservableCollection<PuzzleViewModel> Puzzles
        {
            get { return _puzzles; }
            set { SetProperty(ref _puzzles, value); }
        }


    }


    public static class ViewModelHelper
    {
        public static PuzzleViewModel FakePuzzleBuilder(string title)
        {
            var puzzleVm = new PuzzleViewModel { Title = title };            puzzleVm.Words = new List<string>();
            puzzleVm.Words.Add("First");
            puzzleVm.Words.Add("Second");
            puzzleVm.Words.Add("Third");
            puzzleVm.Words.Add("Forth");
            puzzleVm.Words.Add("Fifth");
            return puzzleVm;
        }
    }
}