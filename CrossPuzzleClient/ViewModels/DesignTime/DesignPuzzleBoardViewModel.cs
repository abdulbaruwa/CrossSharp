using System;
using System.Linq;
using System.Diagnostics;
using CrossPuzzleClient.Infrastructure;
using CrossPuzzleClient.Observables;
using CrossPuzzleClient.ViewModels.PuzzleBoardView;

namespace CrossPuzzleClient.ViewModels.DesignTime
{
    public class DesignPuzzleBoardViewModel : PuzzleBoardViewModel
    {
        public DesignPuzzleBoardViewModel()
            : base(new FakePuzzlesService(), new TestSchedulers(), new FakeUserSevice())
        {
            Debug.WriteLine("Username {0}", CurrentUser);
            Debug.WriteLine("StartPauseButtonCaption {0}", StartPauseButtonCaption);
            CurrentUser = "Abdulrahaman";

            Words = _puzzlesService.GetOrdereredWordsForPuzzle(0,CurrentUser);

            SelectedWord = (from word in Words
                               where word.Word.Equals("india",StringComparison.OrdinalIgnoreCase)
                               select word).FirstOrDefault();

            GameIsRunning = true;


            StartPauseButtonCaption = "Pause";

            GameCountDown = "00:00:00";

            AcrossAndDownVisible = true;
            
            AddWordsToBoard();

        }
 
    }
}