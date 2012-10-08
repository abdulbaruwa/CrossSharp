using System.Linq;
namespace CrossPuzzleClient.ViewModels
{
    public class DesignPuzzleBoardViewModel : PuzzleBoardViewModel
    {

        public DesignPuzzleBoardViewModel()
            : base(new FakePuzzlesService())
        {
            Words = _puzzlesService.GetOrdereredWordsForPuzzle(0);

            SelectedWord = (from word in Words
                               where word.Cells.Count == Words.Max(x => x.Cells.Count)
                               select word).FirstOrDefault();

            AddWordsToBoard();

            UserName = "Abdul";
        }
    }
}