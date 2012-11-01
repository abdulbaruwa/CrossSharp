using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToExcel;
using LinqToExcel.Domain;
using Newtonsoft.Json;

namespace GeneratePuzzleFromCsv
{
    public class GameDataSerializer
    {
        public async Task<List<PuzzleGroupData>> GetPuzzles()
        {
            var puzzleGroupDatas = new List<PuzzleGroupData>();
            Task<List<PuzzleGroup>> returnedTaskResult = GetPuzzles_MethodAsync();
            List<PuzzleGroup> returnedPuzzles = await returnedTaskResult;

            var stu = JsonConvert.SerializeObject(returnedPuzzles);

            foreach (PuzzleGroup returnedPuzzle in returnedPuzzles)
            {
                Task<PuzzleGroupData> puzzleGroupDataTaskResult = SerializePuzzleGroupIntoPuzzleGroupData(returnedPuzzle);
                var puzzleGroupData = await puzzleGroupDataTaskResult;
                puzzleGroupDatas.Add(puzzleGroupData);
            }
            return puzzleGroupDatas;
        }

        private async Task<List<PuzzleGroup>> GetPuzzles_MethodAsync()
        {
            var excel = new ExcelQueryFactory(@"PuzzleGroup");
            excel.DatabaseEngine = DatabaseEngine.Jet;

            IQueryable<PuzzleGroup> puzzleGroups = from c in excel.Worksheet("PuzzleGroup")
                                                   select new PuzzleGroup
                                                              {
                                                                  PuzzleGroupId = c["PuzzleGroupId"].Cast<int>(),
                                                                  Name = c["Name"],
                                                                  Puzzles = GetPuzzleSubGroup(excel, c["PuzzleGroupId"].Cast<int>())
                                                              };

            return puzzleGroups.ToList();
        }

        private async Task<PuzzleGroupData> SerializePuzzleGroupIntoPuzzleGroupData(PuzzleGroup puzzleGroup)
        {
            Task<string> puzzleGameData = JsonConvert.SerializeObjectAsync(puzzleGroup);
            return new PuzzleGroupData
                       {
                           PuzzleGroupDataId = 1,
                           Data = await puzzleGameData
                       };
        }

        private static List<PuzzleSubGroup> GetPuzzleSubGroup(ExcelQueryFactory excel, int puzzleGroupId)
        {
            IQueryable<PuzzleSubGroup> puzzleSubGroups = from c in excel.Worksheet("PuzzleSubGroup")
                                                         where c["PuzzleGroupId"].Cast<int>() == puzzleGroupId
                                                         select new PuzzleSubGroup
                                                                    {
                                                                        Title = c["Title"],
                                                                        PuzzleSubGroupId = c["PuzzleSubGroupId"].Cast<int>(),
                                                                        PuzzleGames = GetGamesFromPuzzleGameWorksheet(excel, c["PuzzleSubGroupId"].Cast<int>())
                                                                    };

            return puzzleSubGroups.ToList();
        }

        private static List<PuzzleGame> GetGamesFromPuzzleGameWorksheet(ExcelQueryFactory excel, int puzzleSubGroupId)
        {
            //Get all games for a sub group
            var games = from c in excel.Worksheet("PuzzleGame")
                        select new
                                   {
                                       PuzzleGameId = c["PuzzleGameId"].Cast<int>(),
                                       PuzzleSubGroupId = c["PuzzleSubGroupId"].Cast<int>(),
                                       Word = c["Word"].Cast<string>(),
                                       Hint = c["Hint"].Cast<string>()
                                   };

            var gamelist = games.ToList();
            var grouped = from c in gamelist
                          where c.PuzzleSubGroupId == puzzleSubGroupId
                          group c by c.PuzzleGameId
                              into g
                              select new { PuzzleGameId = g.Key, Games = g };

            var puzzleGames = new List<PuzzleGame>();
            foreach (var gamesSet in grouped)
            {
                var puzzleGame = new PuzzleGame { PuzzleGameId = gamesSet.PuzzleGameId, Words = new Dictionary<string, string>() };
                foreach (var game in gamesSet.Games)
                {
                    puzzleGame.Words.Add(game.Word, game.Hint);
                }
                puzzleGames.Add(puzzleGame);
            }

            return puzzleGames;
        }
    }
}