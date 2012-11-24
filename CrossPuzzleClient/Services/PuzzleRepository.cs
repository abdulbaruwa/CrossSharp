using SQLite;
using System.IO;
using System.Linq;
using CrossPuzzleClient.DataModel;
using System.Collections.Generic;

namespace CrossPuzzleClient.Services
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
            var path = GetDatabasePath();
            using (var db = new SQLiteConnection(Path.Combine(path, "Puzzle.db")))
            {

                var puzzleGroupData = db.Table<PuzzleGroupGameData>().FirstOrDefault(x => x.GameUserName == userName);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<PuzzleGroup>>(puzzleGroupData.Data);
            }
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