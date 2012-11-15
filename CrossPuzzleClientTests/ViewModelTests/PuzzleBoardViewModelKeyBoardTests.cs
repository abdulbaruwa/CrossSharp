using System.Diagnostics;
using System.Linq;
using CrossPuzzleClient.GameStates;
using CrossPuzzleClient.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CrossPuzzleClientTests.ViewModelTests
{
    [TestClass]
    public class PuzzleBoardViewModelKeyBoardTests
    {
        [TestMethod]
        public void Should_stop_running_game_when_last_word_is_inserted_onto_the_board()
        {
            var puzzleBoardVm = GetDesignPuzzleBoardViewModelWithAllWordsInserted(new DesignPuzzleBoardViewModel());

            //Assert
            Assert.IsInstanceOfType(puzzleBoardVm.CurrentGameState, typeof(GameFinishedState));
            //Assert.IsFalse(puzzleBoardVm.GameIsRunning);
        }

        [TestMethod]
        public void Should_save_game_score_when_last_word_is_inserted_into_board_and_GameFinishedState_is_entered()
        {
            var puzzleBoardVm = GetDesignPuzzleBoardViewModelWithAllWordsInserted(new DesignPuzzleBoardViewModel());
            
                        
        }
        [TestMethod]
        public void Should_execute_GameFinishedEvent_with_100_percent_result_when_last_word_is_inserted_onto_the_board()
        {
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            Messenger.Default.Register<GameCompleteMessage>(this, m => Assert.AreEqual(100, m.ScorePercentage));
            GetDesignPuzzleBoardViewModelWithAllWordsInserted(puzzleBoardVm);
        }

        [TestMethod]
        public void When_a_delete_is_hit_should_remove_a_letter_from_entered_characters()
        {
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            puzzleBoardVm.StartPauseButtonCaption = "Start";
            puzzleBoardVm.GameIsRunning = false;
            puzzleBoardVm.StartPauseCommand.Execute(null);

            puzzleBoardVm.SelectedWordAcross = puzzleBoardVm.Words.First();
            Messenger.Default.Send(new KeyReceivedMessage { KeyChar = "t" });
            Messenger.Default.Send(new KeyReceivedMessage { KeyChar = "t" });

            Messenger.Default.Send(new KeyReceivedMessage { KeyCharType = KeyCharType.Delete });
            var result = puzzleBoardVm.SelectedWord.Cells.Count(x => x.EnteredValue == "t");
            puzzleBoardVm.StartPauseCommand.Execute(null);
            Debug.WriteLine("Cell Entered count {0}", result);

            Assert.AreEqual(1, puzzleBoardVm.SelectedWord.Cells.Count(x => x.EnteredValue == "t"));
        }

        private static DesignPuzzleBoardViewModel GetDesignPuzzleBoardViewModelWithAllWordsInserted(DesignPuzzleBoardViewModel puzzleBoardVm)
        {
            puzzleBoardVm.StartPauseButtonCaption = "Start";
            puzzleBoardVm.GameIsRunning = false;
            puzzleBoardVm.StartPauseCommand.Execute(null);

            //Act
            puzzleBoardVm.SelectedWordAcross = puzzleBoardVm.Words.First();

            foreach (var word in puzzleBoardVm.Words)
            {
                if (word.Direction == Direction.Down)
                {
                    puzzleBoardVm.SelectedWordDown = word;
                }
                else
                {
                    puzzleBoardVm.SelectedWordAcross = word;
                }

                foreach (var cell in word.Cells)
                {
                    Messenger.Default.Send(new KeyReceivedMessage() { KeyChar = cell.Value });
                }

                puzzleBoardVm.AddEnteredWordOnToBoardCommand.Execute(null);
            }
            return puzzleBoardVm;
        }   
    }
}