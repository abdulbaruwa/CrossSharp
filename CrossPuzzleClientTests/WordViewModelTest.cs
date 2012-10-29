using System.Collections.ObjectModel;
using System.Linq;
using CrossPuzzleClient.GameStates;
using CrossPuzzleClient.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CrossPuzzleClientTests
{
    [TestClass]
    public class WordViewModelTest
    {
        [TestMethod]
        public void When_all_correct_values_for_all_cells_are_added_should_return_true_if_IsWordAnswerCorrect_is_called()
        {
            var wordViewModel = new WordViewModel();
            wordViewModel.Cells = new ObservableCollection<CellEmptyViewModel>();
            const string word = "testword";
            int row = 0;
            foreach (object character in word)
            {
                var cell = new CellViewModel(0, row, character.ToString(), wordViewModel, string.Empty);
                row += 1;
                wordViewModel.Cells.Add(cell);
            }

            foreach (CellEmptyViewModel cell in wordViewModel.Cells)
            {
                var keyReceivedMessage = new KeyReceivedMessage {KeyChar = cell.Value};
                var cellValueChangedMesage = new CellValueChangedMessage()
                        {
                            Character = keyReceivedMessage.KeyChar,
                            Col = cell.Col,
                            Row = cell.Row
                        };

                Messenger.Default.Send(cellValueChangedMesage);
            }

            Assert.IsTrue(wordViewModel.IsWordAnswerCorrect);
        }

   
    }
}