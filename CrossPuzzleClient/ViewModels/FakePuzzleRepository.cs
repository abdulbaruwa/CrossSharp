using System.Collections.Generic;

namespace CrossPuzzleClient.ViewModels
{
    class FakePuzzleRepository : IPuzzleRepository
    {
        public List<string> GetPuzzleWithId(int puzzleId)
        {
            var words = new List<string>();
            words.Add("Bamidele");
            words.Add("station");
            words.Add("india");
            words.Add("Adams");
            words.Add("fards");
            words.Add("novemb");
            words.Add("belt");
            words.Add("train");
            words.Add("adeola");
            words.Add("amoeba");
            words.Add("moscow");
            return words;

        }
    }
}