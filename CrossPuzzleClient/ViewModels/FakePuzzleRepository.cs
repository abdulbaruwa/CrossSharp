using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CrossPuzzleClient.DataModel;
using SQLite;

namespace CrossPuzzleClient.ViewModels
{
    class FakePuzzleRepository : IPuzzleRepository
    {
        public Dictionary<string,string> GetPuzzleWithId(int puzzleId)
        {
            var words = new Dictionary<string, string>
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
            return words;

        }

        public List<PuzzleGroup> GetPuzzles()
        {
            var puzzleGroups = new List<PuzzleGroup>();
            using (var db = new SQLiteConnection(Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Puzzle.db")))
            {

                foreach (var tabledata in db.Table<PuzzleGroupData>())
                {
                    var puzzleGroup =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<PuzzleGroup>(tabledata.Data);
                    puzzleGroups.Add(puzzleGroup);
                }
            }
            return puzzleGroups;

        }
    }
}