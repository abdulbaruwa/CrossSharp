using CrossPuzzleClient.ViewModels;

namespace CrossPuzzleClient.Infrastructure
{
    public interface IPuzzlesViewModelFactory
    {
        PuzzlesViewModel CreateInstance();
    }
}