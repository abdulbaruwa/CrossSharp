using System.Collections.Generic;

namespace CrossPuzzleClient.DataModel
{
    //Science
    public class PuzzleGroup
    {
        public int Id { get; set; } 
        public int Name { get; set; }
        public List<PuzzleSubGroup> Puzzles { get; set; }
    }

    //KS2 - Science
    public class PuzzleSubGroup
    {
        public string Title { get; set; }
        public List<PuzzleGame> PuzzleGames { get; set; }
    }

    public class PuzzleGame
    {
        public int PuzzleGameId { get; set; }
        public List<string> Words { get; set; }
    }

    public class PuzzleGroupData
    {
        public int PuzzleGroupDataId { get; set; }
        public string Data { get; set; }
    }

}