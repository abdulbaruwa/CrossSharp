using System.Collections.Generic;

namespace CrossPuzzleClient.ViewModels
{
    public interface IPuzzleRepository
    {
        Dictionary<string,string> GetPuzzleWithId(int puzzleId);
    }
}