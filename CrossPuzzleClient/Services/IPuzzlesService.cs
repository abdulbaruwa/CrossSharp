using System.Collections.Generic;
using System.Collections.ObjectModel;
using CrossPuzzleClient.ViewModels.PuzzlesView;

namespace CrossPuzzleClient.Services
{
    public interface IPuzzlesService
    {
        ObservableCollection<WordViewModel> GetOrdereredWordsForPuzzle(int puzzleId, string user);
        IList<WordViewModel> GetWordsInsertableIntoPuzzle(Dictionary<string, string> words);
        string[,] GetEmptyBoard();
    }
}