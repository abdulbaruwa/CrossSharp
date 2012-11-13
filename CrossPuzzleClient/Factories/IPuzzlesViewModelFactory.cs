using CrossPuzzleClient.ViewModels;

namespace CrossPuzzleClient.Factories
{
    public interface IPuzzlesViewModelFactory
    {
        PuzzlesViewModel CreateInstance(string userName);
    }
}