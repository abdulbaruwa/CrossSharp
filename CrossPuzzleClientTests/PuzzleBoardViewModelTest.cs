using CrossPuzzleClient.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

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
    }
}
