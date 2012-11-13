using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossPuzzleClient.GameDataService;
using CrossPuzzleClient.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.Foundation;
using Windows.Storage;

namespace CrossPuzzleClientTests
{
    [TestClass]
    public class GameDataServiceTests
    {
        [TestMethod]
        public async Task When_GetGameDataAndStoreInLocalDb_is_called_should_create_Puzzle_db_file_in_location_provided()
        {
            IPuzzleWebApiService puzzleWebApiService = new FakePuzzleWebApiService();
            IGameDataService gameDataService = new GameDataService(puzzleWebApiService);
            string path = ApplicationData.Current.TemporaryFolder.Path;
            Task resultTask = gameDataService.GetGameDataAndStoreInLocalDb(ApplicationData.Current.TemporaryFolder.Path);
            await resultTask;

            bool fileExists = await FileExistInStorageLocation(path, "Puzzle.db");
            Assert.IsTrue(fileExists);
        }

        private async Task<bool> FileExistInStorageLocation(string filePath, string fileName)
        {
            StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(filePath);
                // Windows.Storage.ApplicationData.Current.LocalFolder;
            IAsyncOperation<IReadOnlyList<StorageFile>> filesTask = storageFolder.GetFilesAsync();
            IReadOnlyList<StorageFile> files = await filesTask;
            return files.Any(x => x.Name == fileName);
        }



    }
}