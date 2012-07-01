namespace CrossPuzzleClient.ViewModels
{
    public class DesignPuzzleBoardViewModel : PuzzleBoardViewModel
    {

        public DesignPuzzleBoardViewModel()
            : base(new FakePuzzlesService())
        {
            Words = _puzzlesService.GetOrdereredWordsForPuzzle(0);
            AddWordsToBoard();
        }
    }
}