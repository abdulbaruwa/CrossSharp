using System.Linq;
using CrossPuzzleClient.GameStates;
using CrossPuzzleClient.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CrossPuzzleClientTests
{
    [TestClass]
    public class PuzzleBoardViewModelTest
    {

        [TestMethod]
        public void TestMethod()
        {
            //var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            //puzzleBoardVm.StartPauseButtonCaption = "Start";

            //((TestScheduler)puzzleBoardVm.SchedulerProvider.ThreadPool).AdvanceBy(2);

            //puzzleBoardVm.StartPauseCommand.Execute(null);

            ////var expectedValues = new long[] {0, 1, 2, 3, 4};
            ////var actualValues = new List<long>();

            ////var scheduler = new TestScheduler();
            ////var interval = Observable.Interval(TimeSpan.FromSeconds(1), scheduler)
            ////    .Take(5);

            ////interval.Subscribe();
            ////scheduler.Start();
            //((TestScheduler)puzzleBoardVm.SchedulerProvider.ThreadPool).Start();
            //((TestScheduler) puzzleBoardVm.SchedulerProvider.Dispatcher).Start();
            

        }


        [TestMethod]
        public void Should_show_game_finish_flyout_when_last_word_is_inserted_onto_the_board()
        {
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            Messenger.Default.Register<GameCompleteMessage>(this, m => Assert.AreEqual(44, m.ScorePercentage));
            GetDesignPuzzleBoardViewModelWithAllWordsInsertedButSomeAnswersWrong(puzzleBoardVm);
            Assert.AreEqual("You scored 44%", puzzleBoardVm.GameScoreDisplay);
            Assert.IsTrue(puzzleBoardVm.ShowGameOverPopup);
        }


        [TestMethod]
        public void Should_execute_GameFinishedEvent_with_44_percent_result_when_last_word_is_inserted_onto_the_board()
        {
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            Messenger.Default.Register<GameCompleteMessage>(this, m => Assert.AreEqual(44, m.ScorePercentage));
            GetDesignPuzzleBoardViewModelWithAllWordsInsertedButSomeAnswersWrong(puzzleBoardVm);
        }

        [TestMethod]
        public void Should_execute_GameFinishedEvent_with_100_percent_result_when_last_word_is_inserted_onto_the_board()
        {
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            Messenger.Default.Register<GameCompleteMessage>(this, m => Assert.AreEqual(100, m.ScorePercentage));
            GetDesignPuzzleBoardViewModelWithAllWordsInserted(puzzleBoardVm);
        }

        [TestMethod]
        public void Should_stop_running_game_when_last_word_is_inserted_onto_the_board()
        {
            var puzzleBoardVm = GetDesignPuzzleBoardViewModelWithAllWordsInserted(new DesignPuzzleBoardViewModel());

            //Assert
            Assert.IsInstanceOfType(puzzleBoardVm.CurrentGameState, typeof(GameFinishedState));
            //Assert.IsFalse(puzzleBoardVm.GameIsRunning);
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
                    Messenger.Default.Send(new KeyReceivedMessage() {KeyChar = cell.Value});
                }

                puzzleBoardVm.AddEnteredWordOnToBoardCommand.Execute(null);
            }
            return puzzleBoardVm;
        }        
        
        private static DesignPuzzleBoardViewModel GetDesignPuzzleBoardViewModelWithAllWordsInsertedButSomeAnswersWrong(DesignPuzzleBoardViewModel puzzleBoardVm)
        {
            puzzleBoardVm.StartPauseButtonCaption = "Start";
            puzzleBoardVm.GameIsRunning = false;
            puzzleBoardVm.StartPauseCommand.Execute(null);

            //Act
            puzzleBoardVm.SelectedWordAcross = puzzleBoardVm.Words.First();

            for (int index = 0; index < puzzleBoardVm.Words.Count; index++)
            {
                var word = puzzleBoardVm.Words[index];
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
                    var newValue = cell.Value;

                    if (index % 2 == 0) newValue = "x";
                    Messenger.Default.Send(new KeyReceivedMessage() {KeyChar = newValue});

                }

                puzzleBoardVm.AddEnteredWordOnToBoardCommand.Execute(null);
            }
            return puzzleBoardVm;
        }

        [TestMethod]
        public void Should_fire_success_message_if_all_answers_are_correct()
        {
                
        }

        //public void When_game_is_finished_with_success_should_reset_board_and_

        [TestMethod]
        public void Board_should_be_disabled_if_game_is_not_running()
        {

            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            puzzleBoardVm.StartPauseButtonCaption = "Start";
            puzzleBoardVm.GameIsRunning = false;
            puzzleBoardVm.StartPauseCommand.Execute(null);
            Assert.IsTrue(puzzleBoardVm.GameIsRunning);
        }

        [TestMethod]
        public void Should_highlight_the_the_most_likely_word_match_on_the_Board_if_a_cell_is_clicked_on_the_Board()
        {
           //CurrentSelectedCell 
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            puzzleBoardVm.StartPauseButtonCaption = "Start";
            puzzleBoardVm.GameIsRunning = false;
            puzzleBoardVm.StartPauseCommand.Execute(null);

            //Act
            var wordToAdd = puzzleBoardVm.Words.FirstOrDefault(x => x.Direction == Direction.Down);
            puzzleBoardVm.CurrentSelectedCell = wordToAdd.Cells[1];
            
            foreach (var cell in puzzleBoardVm.SelectedWordDown.Cells)
            {
                Assert.IsTrue(cell.IsVisible == CellState.IsUsed);
            }
        }


        [TestMethod]
        public void Last_selected_word_should_be_unhighlighted_on_the_board_when_another_word_is_selected()
        { 

            //Arrange
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            puzzleBoardVm.StartPauseCommand.Execute(null);
            puzzleBoardVm.SelectedWordDown = puzzleBoardVm.Words[1];
            var pvCells = puzzleBoardVm.SelectedWord.Cells.Select(x => new {Col = x.Col, Row = x.Row});

            puzzleBoardVm.SelectedWordDown = puzzleBoardVm.Words[2];

            foreach (var cell in pvCells)
            {
                var cell1 = cell;
                Assert.IsTrue(puzzleBoardVm.Cells.First(x => x.Row == cell1.Row && x.Col == cell1.Col).IsVisible ==
                              CellState.IsUsed);
            }

        }

        [TestMethod]
        public void Selected_word_should_be_highlighted_on_the_board_and_the_previous_selected_word_unhighlighted()
        {

            //Arrange
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            puzzleBoardVm.StartPauseCommand.Execute(null);

            puzzleBoardVm.SelectedWordDown = puzzleBoardVm.Words[1];

            foreach (var cell in puzzleBoardVm.SelectedWord.Cells)
            {
                var cell1 = cell;
                Assert.IsTrue(puzzleBoardVm.Cells.First(x => x.Row == cell1.Row && x.Col == cell1.Col).IsVisible ==
                              CellState.IsActive);
            }

        }


        [TestMethod]
        public void Selected_word_values_should_not_be_persisted_if_the_word_is_not_added_to_the_board()
        {

            //Arrange
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            puzzleBoardVm.GameIsRunning = true;
            puzzleBoardVm.SelectedWordDown = puzzleBoardVm.Words[1];

            //Act
            foreach (var letter in puzzleBoardVm.SelectedWord.Cells)
            {
                Messenger.Default.Send(new KeyReceivedMessage() { KeyChar = letter.Value });
            }

            puzzleBoardVm.SelectedWordDown = puzzleBoardVm.Words[4];

            //Assert
            Assert.IsNull(puzzleBoardVm.Words[1].Cells[1].EnteredValue);                
        }

        [TestMethod]
        public void Should_instatiate_with_dummy_words()
        {
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();

            Assert.IsTrue(puzzleBoardVm.Words.Count > 0);
        }


        [TestMethod]
        public void SelectedWord_should_be_set_after_an_item_is_selected_on_the_Down_list()
        {
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
        }

        [TestMethod]
        public void When_keymessage_is_received_and_word_is_selected_should_display_key_in_selected_word()
        {
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            puzzleBoardVm.SelectedWordAcross = puzzleBoardVm.Words.First();

            Messenger.Default.Send(new KeyReceivedMessage {KeyChar = "t"});
            Assert.AreEqual(puzzleBoardVm.SelectedWord.Cells.First().EnteredValue, "t");
        }

        [TestMethod]
        public void When_the_start_button_is_hit_the_button_display_should_switch_to_Pause()
        {
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            puzzleBoardVm.StartPauseButtonCaption = "Start";
            puzzleBoardVm.StartPauseCommand.Execute(null);

            //Act
            puzzleBoardVm.StartPauseCommand.Execute(null);

            //Assert
            Assert.IsInstanceOfType(puzzleBoardVm.CurrentGameState, typeof(GamePauseState));
        }

        [TestMethod]
        public void When_start_is_hit_the_Count_up_Timer_should_commence()
        {
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            
        }

        [TestMethod]
        public void When_all_chars_for_a_selected_word_are_received_should_activate_the_Tick_button()
        {
            var puzzleBoardVm = PuzzleWithFirstLetterTypedIn();

            Assert.IsTrue(puzzleBoardVm.ShowCompleteTick);
        }

        private static DesignPuzzleBoardViewModel PuzzleWithFirstLetterTypedIn()
        {
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            puzzleBoardVm.SelectedWordAcross = puzzleBoardVm.Words.First();

            foreach (CellEmptyViewModel cell in puzzleBoardVm.SelectedWord.Cells)
            {
                Messenger.Default.Send(new KeyReceivedMessage {KeyChar = "t"});
            }
            return puzzleBoardVm;
        }

        [TestMethod]
        public void When_a_backspace_is_hit_should_remove_a_letter_from_entered_characters()
        {
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            puzzleBoardVm.SelectedWordAcross = puzzleBoardVm.Words.First();
            Messenger.Default.Send(new KeyReceivedMessage {KeyChar = "t"});
            Messenger.Default.Send(new KeyReceivedMessage {KeyChar = "t"});

            Messenger.Default.Send(new KeyReceivedMessage { KeyCharType = KeyCharType.BackSpace });
            Assert.AreEqual(puzzleBoardVm.SelectedWord.Cells.Count(x => x.EnteredValue == "t"), 1);
        }

        [TestMethod]
        public void When_a_backspace_is_hit_after_all_characters_have_been_entered_should_remove_a_letter_from_entered_characters()
        {
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            puzzleBoardVm.SelectedWordAcross = puzzleBoardVm.Words.First();
            foreach (var cell in puzzleBoardVm.SelectedWord.Cells)
            {
                Messenger.Default.Send(new KeyReceivedMessage { KeyChar = cell.Value });
            }

            //Act
            Messenger.Default.Send(new KeyReceivedMessage { KeyCharType = KeyCharType.BackSpace });

            //Assert
            Assert.AreEqual(1,puzzleBoardVm.SelectedWord.Cells.Count(x => string.IsNullOrEmpty(x.EnteredValue)));
        }


        [TestMethod]
        public void When_a_delete_is_hit_should_remove_a_letter_from_entered_characters()
        {
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            puzzleBoardVm.SelectedWordAcross = puzzleBoardVm.Words.First();
            Messenger.Default.Send(new KeyReceivedMessage { KeyChar = "t" });
            Messenger.Default.Send(new KeyReceivedMessage { KeyChar = "t" });

            Messenger.Default.Send(new KeyReceivedMessage { KeyCharType = KeyCharType.Delete });
            Assert.AreEqual(puzzleBoardVm.SelectedWord.Cells.Count(x => x.EnteredValue == "t"), 1);
        }

        [TestMethod]
        public void When_the_all_chars_entered_and_tick_button_is_clicked_Should_move_entered_word_on_to_the_Puzzle_Board()
        {
            var puzzleBoardVm = PuzzleWithFirstLetterTypedIn();
            puzzleBoardVm.AddEnteredWordOnToBoardCommand.Execute(null);

            foreach (var cell in puzzleBoardVm.SelectedWord.Cells)
            {
                var boardcell = puzzleBoardVm.Cells.First(x => x.Row == cell.Row && x.Col == cell.Col);
                Assert.AreEqual(boardcell.EnteredValue, cell.EnteredValue);
            }
        }

        [TestMethod]
        public void When_a_cell_belonging_to_two_words_is_changed_the_change_Should_reflect_in_both_words()
        {
            var puzzleBoardVm = PuzzleWithFirstLetterTypedIn(); //First word typed in
            puzzleBoardVm.SelectedWordAcross = puzzleBoardVm.Words.First();
            puzzleBoardVm.AddEnteredWordOnToBoardCommand.Execute(null); //Add word to board

            puzzleBoardVm.SelectedWordDown = puzzleBoardVm.Words[1];

            foreach (CellEmptyViewModel cell in puzzleBoardVm.SelectedWord.Cells)
            {
                Messenger.Default.Send(new KeyReceivedMessage { KeyChar = "a" });
            }

            //Assert the shared cell have switched with 
            Assert.AreEqual(puzzleBoardVm.SelectedWordDown.Cells[0].EnteredValue,puzzleBoardVm.Words[0].Cells[3].EnteredValue);
        }
    }
}