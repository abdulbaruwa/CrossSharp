﻿using System;
using System.Collections.Generic;
using System.Linq;
using CrossPuzzleClient.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Reactive.Linq;

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
            Assert.AreEqual(puzzleBoardVm.StartPauseButtonCaption,"Pause");
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
            puzzleBoardVm.AddWordToBoardCommand.Execute(null);

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
            puzzleBoardVm.AddWordToBoardCommand.Execute(null); //Add word to board

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