using System.Collections.ObjectModel;

namespace CrossPuzzleClient.ViewModels
{
    public class PuzzleGroupViewModel
    {
        public string Category { get; set; }
        public ObservableCollection<PuzzleViewModel> Puzzles { get; set; }
    }
}