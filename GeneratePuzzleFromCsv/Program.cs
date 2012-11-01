using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToExcel;
using LinqToExcel.Domain;
using Newtonsoft.Json;

namespace GeneratePuzzleFromCsv
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string path = @"c:\documents";
            //Open CSV file
            //GetPuzzle();
            //Task<PuzzleGroupData> er = GetFirstPuzzleGame();
            //var result = er.Result;
            var renew =new  GameDataSerializer().GetPuzzles();
            var result = renew.Result;

            //var deserializeddata = JsonConvert.DeserializeObject<List<PuzzleGroup>>(result.Data);
        }

        private static List<PuzzleGroup> GetPuzzle()
        {
            var excel = new ExcelQueryFactory(@"PuzzleGroup");
            excel.DatabaseEngine = DatabaseEngine.Jet;
            var puzzleGroups = from c in excel.Worksheet("PuzzleGroup")
                               select new PuzzleGroup
                                       {
                                           PuzzleGroupId = c["PuzzleGroupId"].Cast<int>(),
                                           Name = c["Name"],
                                           Puzzles = GetPuzzleSubGroup(excel, c["PuzzleGroupId"].Cast<int>())
                                       };
            return puzzleGroups.ToList();
        }

        private static async Task<PuzzleGroupData> GetFirstPuzzleGame()
        {
            var puzzleGameData = JsonConvert.SerializeObjectAsync(GetPuzzle().First());
            return new PuzzleGroupData()
                       {
                           PuzzleGroupDataId = 1,
                           Data = await puzzleGameData
                       };
        }

        private static List<PuzzleSubGroup> GetPuzzleSubGroup(ExcelQueryFactory excel, int puzzleGroupId)
        {

            var puzzleSubGroups = from c in excel.Worksheet("PuzzleSubGroup")
                                  where c["PuzzleGroupId"].Cast<int>() == puzzleGroupId
                                  select new PuzzleSubGroup()
                                             {
                                                 Title = c["Title"],
                                                 PuzzleSubGroupId = c["PuzzleSubGroupId"].Cast<int>(),
                                                 PuzzleGames =
                                                     GetGamesFromPuzzleGameWorksheet(excel, c["PuzzleSubGroupId"].Cast<int>())
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
                          select new {PuzzleGameId = g.Key, Games = g};

            var puzzleGames = new List<PuzzleGame>();
            foreach (var gamesSet in grouped)
            {
                var puzzleGame = new PuzzleGame()
                                     {PuzzleGameId = gamesSet.PuzzleGameId, Words = new Dictionary<string, string>()};
                foreach (var game in gamesSet.Games)
                {
                    puzzleGame.Words.Add(game.Word, game.Hint);
                    Console.WriteLine(game.Word);
                }
                puzzleGames.Add(puzzleGame);
            }

            return puzzleGames;
        }
    }
}