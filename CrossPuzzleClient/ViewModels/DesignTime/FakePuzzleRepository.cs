using System.Collections.Generic;
using System.IO;
using CrossPuzzleClient.DataModel;
using CrossPuzzleClient.Services;
using SQLite;

namespace CrossPuzzleClient.ViewModels.DesignTime
{
    public class FakePuzzleRepository : IPuzzleRepository
    {
        private Dictionary<string,string> _words = null;
        private string _repositoryDbPath;

        public Dictionary<string,string> GetPuzzleWithId(int puzzleId,string userName)
        {
            return _words ?? new Dictionary<string, string>
                                 {
                                     {"Bamidele", "Adetoro's first name"},
                                     {"station", "place where i fit get train"},
                                     {"india", "Origin of my favourite curry"},
                                     {"Adams", "Captain Arsenal"},
                                     {"fards", "show off"},
                                     {"novemb", "like november"},
                                     {"belt", "Tied around my waist"},
                                     {"train", "Mode of transportation"},
                                     {"adeola", "My sister"},
                                     {"amoeba", "Single cell organism"},
                                     {"moscow", "Cold city behind iron curtain"}
                                 };
        }

        public void AddWord(Dictionary<string,string> words)
        {
            _words = words;
        }

        public void AddPuzzleRepositoryPath(string repositoryDbPath)
        {
            _repositoryDbPath = repositoryDbPath;
        }

        public List<PuzzleGroup> GetPuzzles(string userName)
        {
            var puzzleGroups = new List<PuzzleGroup>();
            var path = ! string.IsNullOrEmpty(_repositoryDbPath) ? _repositoryDbPath : Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            using (var db = new SQLiteConnection(Path.Combine(path, "Puzzle.db")))
            {

                foreach (var tabledata in db.Table<PuzzleGroupGameData>().Where(x => x.GameUserName == userName))
                {
                    var puzzleGroup =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<PuzzleGroup>(tabledata.Data);
                    puzzleGroups.Add(puzzleGroup);
                }
            }
            
            return puzzleGroups;

        }

        public void UpdateGameData(List<PuzzleGroup> puzzleGroupData, string userName)
        {
            var serializedPuzzleGroupData = Newtonsoft.Json.JsonConvert.SerializeObject(puzzleGroupData);


        }
    }
}