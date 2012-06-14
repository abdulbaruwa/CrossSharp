using System.Collections.Generic;

namespace CrossPuzzleClient.ViewModels
{
    public interface IPuzzleRepository
    {
        List<string> GetPuzzleWithId(int puzzleId);
    }
}