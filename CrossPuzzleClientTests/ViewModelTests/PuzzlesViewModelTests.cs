using System;
using System.Collections.Generic;
using CrossPuzzleClient.Infrastructure;
using CrossPuzzleClient.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Controls;

namespace CrossPuzzleClientTests.ViewModelTests
{
    [TestClass]
    public class PuzzlesViewModelTests
    {
         
        [TestMethod]
        public void when_StartPuzzleCommand_is_fired_it_should_send_a_StartPuzzleMessage_with_the_selected_puzzleId_set_in_it()
        {
            var puzzlesViewModel = new PuzzlesViewModel(new FakeNavigationService(), new FakePuzzleRepository());
            puzzlesViewModel.SelectedPuzzleGroupViewModel = new PuzzleViewModel()
                                                                {
                                                                    PuzzleId = 1,
                                                                    Title = "Science",
                                                                    Words = new List<string>()
                                                                };

            int testResult = 0;
            Messenger.Default.Register<StartPuzzleMessage>(this,m=> testResult = m.PuzzleId);


            puzzlesViewModel.StartPuzzleCommand.Execute(null);

            Assert.AreEqual(1,testResult);


        }

       

    }

    class FakeNavigationService : INavigationService
    {
        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void GoForward()
        {
            throw new NotImplementedException();
        }

        public bool Navigate<T>(object parameter = null)
        {
            return true;
        }

        public bool Navigate(Type source, object parameter = null)
        {
            return true;
        }
    }
}