using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CrossPuzzleClient.DataModel;
using CrossPuzzleClient.Infrastructure;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using SQLite;

namespace CrossPuzzleClient.GameDataService
{
    public class GameDataService : IGameDataService
    {
        private IPuzzleWebApiService _puzzleWebApiService;

        public GameDataService(IPuzzleWebApiService puzzleWebApiService)
        {
            _puzzleWebApiService = puzzleWebApiService;
        }

        private const string PuzzleDb = "Puzzle.db";

        private async Task<bool> FileExistInStorageLocation(string filePath, string fileName)
        {
            var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(filePath);// Windows.Storage.ApplicationData.Current.LocalFolder;
            var filesTask = storageFolder.GetFilesAsync();
            var files = await filesTask;
            return files.Any(x => x.Name == fileName);
        }

        private async Task<PuzzleGroupData> GetPuzzleGroupDataFromServiceAsync()
        {
            var responseStreamTask = _puzzleWebApiService.GetPuzzleDataFromApi();
            var responseStream = await responseStreamTask;

            List<PuzzleGroup> puzzleGroups = null;
            if (responseStream != null)
            {
                puzzleGroups = CreatePuzzleGroupFromJson(responseStream);
            }

            var puzzleGroupData = new PuzzleGroupData();
            puzzleGroupData.Data = JsonConvert.SerializeObject(puzzleGroups);
            puzzleGroupData.PuzzleGroupDataId = 1;
            return puzzleGroupData;
        }

        private List<PuzzleGroup> CreatePuzzleGroupFromJson(Stream responseStream)
        {
            List<PuzzleGroup> resultPuzzleGroup = null;
            using (var reader = new StreamReader(responseStream))
            {
                var res = reader.ReadToEnd();
                var result = JsonConvert.DeserializeObject(res);
                resultPuzzleGroup = JsonConvert.DeserializeObject<List<PuzzleGroup>>(result.ToString());
            }
            return resultPuzzleGroup;
        }

        public async Task GetGameDataAndStoreInLocalDb(string filePath)
        {
            var user = await SimpleIoc.Default.GetInstance<IUserService>().GetCurrentUser();
            var fileExists = await FileExistInStorageLocation(filePath,PuzzleDb);
            if (!fileExists)
            {
                var puzzleGroupDatasTaskResponse = GetPuzzleGroupDataFromServiceAsync();
                var puzzleGroupDatas = await puzzleGroupDatasTaskResponse;
                var puzzleGroupGameDatas = GenerateUserGameDataFromPuzzleGroupData(puzzleGroupDatas, user);
                using (var db = new SQLiteConnection(Path.Combine(filePath,PuzzleDb)))
                {
                    db.CreateTable<PuzzleGroupData>();
                    db.CreateTable<PuzzleGroupGameData>();
                    db.Insert(puzzleGroupDatas);
                    db.Insert(puzzleGroupGameDatas);
                }
            }
        }

        private static PuzzleGroupGameData GenerateUserGameDataFromPuzzleGroupData(PuzzleGroupData puzzleGroupData, string user)
        {
   
              return new PuzzleGroupGameData()
                                             {
                                                 Data = puzzleGroupData.Data,
                                                 PuzzleGroupDataId = puzzleGroupData.PuzzleGroupDataId,
                                                 GameUserName = user
                                             };
            
        }
    }
}