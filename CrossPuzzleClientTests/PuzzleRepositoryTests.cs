using System.Linq;
using System.Threading.Tasks;
using CrossPuzzleClient.GameDataService;
using CrossPuzzleClient.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.Storage;

namespace CrossPuzzleClientTests
{
    [TestClass]
    public class PuzzleRepositoryTests
    {
        [TestMethod]
        public async Task When_GetPuzzleById_is_called_on_the_Repository_Should_return_the_puzzle_with_said_id()
        {
            string path = ApplicationData.Current.TemporaryFolder.Path;
            IPuzzleWebApiService puzzleWebApiService = new FakePuzzleWebApiService();
            IGameDataService gameDataService = new GameDataService(puzzleWebApiService);
            Task resultTask = gameDataService.GetGameDataAndStoreInLocalDb(ApplicationData.Current.TemporaryFolder.Path);
            await resultTask;

            IPuzzleRepository puzzleRepository = new PuzzleRepository();
            puzzleRepository.AddPuzzleRepositoryPath(ApplicationData.Current.TemporaryFolder.Path);
            var result = puzzleRepository.GetPuzzleWithId(1, "Abdul");

            Assert.AreEqual("Confectioner", result.FirstOrDefault().Key);

        } 
    }
}