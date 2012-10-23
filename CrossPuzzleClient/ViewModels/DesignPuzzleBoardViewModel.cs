using System.Linq;
using System.Diagnostics;
using CrossPuzzleClient.Observables;

namespace CrossPuzzleClient.ViewModels
{
    public class DesignPuzzleBoardViewModel : PuzzleBoardViewModel
    {

        public DesignPuzzleBoardViewModel()
            : base(new FakePuzzlesService(), new TestSchedulers())
        {
            Debug.WriteLine("Username {0}", UserName);
            Debug.WriteLine("StartPauseButtonCaption {0}", StartPauseButtonCaption);

            Words = _puzzlesService.GetOrdereredWordsForPuzzle(0);

            SelectedWord = (from word in Words
                               where word.Cells.Count == Words.Max(x => x.Cells.Count)
                               select word).FirstOrDefault();

            GameIsRunning = true;

            UserName = "Abdul";

            StartPauseButtonCaption = "Pause";

            GameCountDown = "00:00:00";
            
            AddWordsToBoard();

        }

    }
}