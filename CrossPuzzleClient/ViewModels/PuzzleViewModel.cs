using System.Collections.Generic;

namespace CrossPuzzleClient.ViewModels
{
    public class PuzzleViewModel
    {
        public string Title { get; set; }
        public int PuzzleId { get; set;  }
        public List<string> Words { get; set; }
    }
}