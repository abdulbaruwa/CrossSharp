using System.Collections.Generic;

namespace CrossPuzzleClient.ViewModels.PuzzlesView
{
    public class PuzzleViewModel
    {
        public string Title { get; set; }
        public int PuzzleId { get; set;  }
        public List<string> Words { get; set; }
        public int GameScore { get; set; }
        public string GameScoreDisplay
        {
            get
            {
                if (GameScore == 0) return "_";
                return GameScore + "%";
            }
        }
    }
}