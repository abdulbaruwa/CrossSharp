using System.Collections.ObjectModel;

namespace CrossPuzzleClient.ViewModels
{
    public interface IPuzzlesService
    {
        ObservableCollection<WordViewModel> GetOrdereredWordsForPuzzle(int puzzleId);
    }
}