using System.Linq;
using CrossPuzzleClient.ViewModels;
using GalaSoft.MvvmLight.Messaging;

namespace CrossPuzzleClientTests
{
    [TestClass]
    public class PuzzleBoardViewModelTest
    {
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
        public void When_all_chars_for_a_selected_word_are_received_should_activate_the_Tick_button()
        {
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            puzzleBoardVm.SelectedWordAcross = puzzleBoardVm.Words.First();

            foreach (CellEmptyViewModel cell in puzzleBoardVm.SelectedWord.Cells)
            {
                Messenger.Default.Send(new KeyReceivedMessage {KeyChar = "t"});
            }

            Assert.IsTrue(puzzleBoardVm.ShowCompleteTick);
        }

        public void When_a_backspace_is_hit_should_remove_a_letter_from_entered_characters()
        {
            var puzzleBoardVm = new DesignPuzzleBoardViewModel();
            puzzleBoardVm.SelectedWordAcross = puzzleBoardVm.Words.First();
            Messenger.Default.Send(new KeyReceivedMessage {KeyChar = "t"});
            Messenger.Default.Send(new KeyReceivedMessage {KeyChar = "t"});

            Messenger.Default.Send(new KeyReceivedMessage { KeyCharType = KeyCharType.BackSpace });
            
            
        }

    }
}