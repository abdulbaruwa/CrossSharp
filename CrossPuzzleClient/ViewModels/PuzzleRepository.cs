using System.Collections.Generic;
using System.IO;
using System.Linq;
using CrossPuzzleClient.DataModel;
using SQLite;

namespace CrossPuzzleClient.ViewModels
{
    public class PuzzleRepository : IPuzzleRepository
    {
        private string _repositoryDbPath;

        public void AddPuzzleRepositoryPath(string repositoryDbPath)
        {
            _repositoryDbPath = repositoryDbPath;
        }

        public Dictionary<string, string> GetPuzzleWithId(int puzzleId, string userName)
        {
            var path = !string.IsNullOrEmpty(_repositoryDbPath) ? _repositoryDbPath : Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            var puzzles = GetPuzzles(userName);

            var res = from pgromp in puzzles
                      from subgroup in pgromp.Puzzles
                      where subgroup.PuzzleSubGroupId == puzzleId
                      select subgroup.Words;

            return res.FirstOrDefault();

        }


        public void UpdateGameData(List<PuzzleGroup> puzzleGroupData, string userName)
        {
            var serializedPuzzleGroupData = Newtonsoft.Json.JsonConvert.SerializeObject(puzzleGroupData);
            var path = GetDatabasePath();
            using (var db = new SQLiteConnection(Path.Combine(path, "Puzzle.db")))
            {
                var puzzGroupData = db.Table<PuzzleGroupGameData>().FirstOrDefault(x => x.GameUserName == userName);
                if (puzzGroupData == null) return;
                puzzGroupData.Data = serializedPuzzleGroupData;
                db.Update(puzzGroupData);
                db.Commit();
            }
        }

        
        public List<PuzzleGroup> GetPuzzles(string userName)
        {
            var puzzleGroups = new List<PuzzleGroup>();
            var path = GetDatabasePath();
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

        private string GetDatabasePath()
        {
            var path = !string.IsNullOrEmpty(_repositoryDbPath)
                           ? _repositoryDbPath
                           : Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            return path;
        }
    }
}