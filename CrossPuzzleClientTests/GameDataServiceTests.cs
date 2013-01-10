using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CrossPuzzleClient.DataModel;
using CrossPuzzleClient.GameDataService;
using CrossPuzzleClient.Infrastructure;
using CrossPuzzleClient.Services;
using CrossPuzzleClient.ViewModels;
using CrossSharp.Core;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SQLite;
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
            IGameDataService gameDataService = new GameDataService(puzzleWebApiService,null);
            string path = ApplicationData.Current.TemporaryFolder.Path;
            Task resultTask = gameDataService.GetGameDataAndStoreInLocalDb(ApplicationData.Current.TemporaryFolder.Path);
            await resultTask;

            bool fileExists = await FileExistInStorageLocation(path, "Puzzle.db");

            Assert.IsTrue(fileExists);
        }

        [TestMethod]
        public async Task Should_load_up_create_puzzle_db_file_and_load_up_new_word_list()
        {
            IPuzzleWebApiService puzzleWebApiService = new FakePuzzleWebApiService();

            IGameDataService gameDataService = new GameDataService(puzzleWebApiService,new PuzzlesService(new PuzzleRepository(), new UserService()));
            var packagePath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "NewWords");
            string path = ApplicationData.Current.TemporaryFolder.Path;
            await gameDataService.ProcessNewWordsIfAny(packagePath,path);
            bool fileExists = await FileExistInStorageLocation(path, "Puzzle.db");

            var rows = 0;
            using (var db = new SQLiteConnection(Path.Combine(path, "Puzzle.db")))
            {
                rows = db.Table<PuzzleGroupData>().Count();
            }

            Assert.IsTrue(rows > 0);
        }

        private static void PrintBoard(string[,] board)
        {
            var result = CoreHorizontal.printboard(board);
            foreach (var str in result)
            {
                Debug.WriteLine(str);
            }
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