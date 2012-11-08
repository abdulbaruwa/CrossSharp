using System.Collections.Generic;
using CrossPuzzleClient.DataModel;

namespace CrossPuzzleClient.ViewModels
{
    public interface IPuzzleRepository
    {
        Dictionary<string,string> GetPuzzleWithId(int puzzleId);
        List<PuzzleGroup> GetPuzzles();
    }
}