using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CrossPuzzleClient.DataModel;
using CrossPuzzleClient.Infrastructure;
using CrossPuzzleClient.Services;
using CrossPuzzleClient.ViewModels.PuzzlesView;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using SQLite;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;

namespace CrossPuzzleClient.GameDataService
{
    public class GameDataService : IGameDataService
    {
        private IPuzzleWebApiService _puzzleWebApiService;
        private readonly IPuzzlesService _puzzlesService;

        public GameDataService(IPuzzleWebApiService puzzleWebApiService, IPuzzlesService puzzlesService)
        {
            _puzzleWebApiService = puzzleWebApiService;
            _puzzlesService = puzzlesService;
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
            var user = await SimpleIoc.Default.GetInstance<IUserService>().GetCurrentUserAsync();
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

        public async Task ProcessNewWordsIfAny(string filePath, string dbPath)
        {
            var user = await SimpleIoc.Default.GetInstance<IUserService>().GetCurrentUserAsync();
            await CreateDbIfNotExist(dbPath);


            var puzzleGroupDatasTaskResponse = GetPuzzleGroupDataFromFile(filePath, dbPath);
            var puzzleGroupDatas = await puzzleGroupDatasTaskResponse;

            var puzzleGroupGameDatas = GenerateUserGameDataFromPuzzleGroupData(puzzleGroupDatas, user);
        }

        private async Task CreateDbIfNotExist(string filePath)
        {
            var fileExists = await FileExistInStorageLocation(filePath, PuzzleDb);
            if (!fileExists)
            {
                using (var db = new SQLiteConnection(Path.Combine(filePath, PuzzleDb)))
                {
                    db.CreateTable<PuzzleGroupData>();
                    db.CreateTable<PuzzleGroupGameData>();
                    db.CreateTable<PuzzleFilesHistory>();
                }
            }
        }

        private async Task<PuzzleGroupData> GetPuzzleGroupDataFromFile(string filePath,string dbPath)
        {
            //Get list of data files.
            var datafileFolder = await StorageFolder.GetFolderFromPathAsync(filePath);
            var files = await datafileFolder.GetFilesAsync(CommonFileQuery.OrderByName);

            var datafiles = files.Where(x => x.Name.StartsWith("puzzledata", StringComparison.CurrentCultureIgnoreCase));

            IEnumerable<PuzzleFilesHistory> puzzleFilesHistories;
            using (var db = new SQLiteConnection(Path.Combine(dbPath, PuzzleDb)))
            {
                puzzleFilesHistories = db.Table<PuzzleFilesHistory>().ToList();
            }

            //Get files not processed
            var newFilesToProcess = from f in datafiles
                                    join h in puzzleFilesHistories on f.Name.ToLower() equals h.FileName.ToLower() into outer
                                    from h in outer.DefaultIfEmpty()
                                    select f;

            //var newFilesToProcess = datafiles.Where(x => 
            //                             puzzleFilesHistories.Any(y => y.FileName.ToLower() == x.Name.ToLower()) == false
            //                        );

            //process the files into db
            var puzzleGroups = ProcessNewFilesAsnc(newFilesToProcess);
            return new PuzzleGroupData();
        }

        private async Task<IEnumerable<PuzzleGroup>> ProcessNewFilesAsnc(IEnumerable<StorageFile> newFilesToProcess)
        {
            var puzzleGroup = new PuzzleGroup();
            var wordsDictionary = await GetWordsFromsFiles(newFilesToProcess);
            IList<WordViewModel> insertedWords = new List<WordViewModel>();
            while (true)
            {

                var wordsNotInserted = from w in wordsDictionary
                                       join wi in insertedWords on w.Key.ToLower() equals wi.Word.ToLower() into outer
                                       from wi in outer.DefaultIfEmpty()
                                       select w;

                insertedWords = _puzzlesService.GetWordsInsertableIntoPuzzle((Dictionary<string, string>) wordsNotInserted);
                
                //Iterate through words add them to a board



                var puzzleSubGroup = new PuzzleSubGroup() {Words = new Dictionary<string, string>()};
                foreach (var wordDic in insertedWords)
                {
                    puzzleSubGroup.Words.Add(wordDic.Word, wordDic.WordHint);
                }
            }
            throw new NotImplementedException();
        }

        private static async Task<Dictionary<string,string>> GetWordsFromsFiles(IEnumerable<StorageFile> newFilesToProcess)
        {
            var allWordsDic = new Dictionary<string, string>();
            foreach (var storageFile in newFilesToProcess)
            {
                var fileDataInLines = await FileIO.ReadLinesAsync(storageFile);
                var wordDic = fileDataInLines.Select(fileDataInLine => fileDataInLine.Split(new[] {'|'}))
                                                 .ToDictionary(lineArray => lineArray[0].Trim(), lineArray => lineArray[1].Trim());

                foreach (var item in wordDic)
                {
                    if (!allWordsDic.ContainsKey(item.Key))
                    {
                        allWordsDic.Add(item.Key, item.Value);
                    }
                }

            }
            return allWordsDic;
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

    public class PuzzleFilesHistory
    {
        public string FileName { get; set; }
        public DateTime ProcessedDate { get; set; }
    }
}