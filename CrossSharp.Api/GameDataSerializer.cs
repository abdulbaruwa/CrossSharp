using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Web;
using LinqToExcel;
using LinqToExcel.Domain;
using Newtonsoft.Json;

namespace CrossSharp.Api
{
    public class GameDataSerializer
    {
        public async Task<string> GetPuzzleGroupJson()
        {
            Task<List<PuzzleGroup>> returnedTaskResult = GetPuzzles_MethodAsync();
            List<PuzzleGroup> returnedPuzzles = await returnedTaskResult;
            var serializedData =JsonConvert.SerializeObject(returnedPuzzles);

            return serializedData;
        }

        private async Task<List<PuzzleGroup>> GetPuzzles_MethodAsync()
        {
            var excelDataFilePath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(),"PuzzleGroup.xls");
            var excel = new ExcelQueryFactory(excelDataFilePath);
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

        private PuzzleGroupData SerializePuzzleGroupIntoPuzzleGroupData(PuzzleGroup puzzleGroup, int index)
        {
            var puzzleGameData = JsonConvert.SerializeObject(puzzleGroup);
            return new PuzzleGroupData
                       {
                           PuzzleGroupDataId = index,
                           Data = puzzleGameData
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