using System.Linq;
using LinqToExcel;
using LinqToExcel.Domain;

namespace GeneratePuzzleFromCsv
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string path = @"c:\documents";
            //Open CSV file
            var excel = new ExcelQueryFactory(@"C:\Development\OnGitHub\CrossSharp\GeneratePuzzleFromCsv\PuzzleGroup");
            excel.DatabaseEngine = DatabaseEngine.Jet;
            var puzzleGroups = from c in excel.Worksheet<PuzzleGroup>("PuzzleGroup")
                       select new PuzzleGroup(){PuzzleGroupId = c.PuzzleGroupId, Name = c.Name};

            

            //var puzzleSubGroups = from c in excel.Worksheet<PuzzleSubGroup>("PuzzleSubGroup")
            //                      select new PuzzleSubGroup() {Title = c.Title, PuzzleGroupId = };

    
            //var puzzleGame = from c in excel.Worksheet<PuzzleGame>("PuzzleGame")
            //                 select new PuzzleGame() {PuzzleGameId = c.PuzzleGameId};


            //foreach (var puzzleSubGroup in puzzleSubGroups)
            //{
            //    puzzleSubGroup.PuzzleGames.AddRange(puzzleGame.Where(x => x.));
            //}



        }
    }
}