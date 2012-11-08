using System.Collections.ObjectModel;
using CrossPuzzleClient.ViewModels;

namespace CrossPuzzleClient.Infrastructure
{
    public class PuzzlesViewModelFactory : IPuzzlesViewModelFactory
    {
        private readonly IPuzzleRepository _puzzleRepository;
        private readonly INavigationService _navigationService;

        public PuzzlesViewModelFactory(IPuzzleRepository puzzleRepository, INavigationService navigationService)
        {
            _puzzleRepository = puzzleRepository;
            _navigationService = navigationService;
        }

        public PuzzlesViewModel CreateInstance()
        {
            var puzzleGroups = _puzzleRepository.GetPuzzles();
            var puzzlesViewModel = new PuzzlesViewModel(_navigationService, _puzzleRepository);
            foreach (var puzzleGroup in puzzleGroups)
            {
                var group = new PuzzleGroupViewModel() { Category = puzzleGroup.Name, Puzzles = new ObservableCollection<PuzzleViewModel>() };
                foreach (var puzzle in puzzleGroup.Puzzles)
                {
                    group.Puzzles.Add(new PuzzleViewModel() { Title = puzzle.Title, PuzzleId = puzzle.PuzzleSubGroupId });
                }
                puzzlesViewModel.PuzzleGroups.Add(group);
            }
            return puzzlesViewModel;
        }
    }
}