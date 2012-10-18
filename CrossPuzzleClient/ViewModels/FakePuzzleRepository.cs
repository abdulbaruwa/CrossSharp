using System.Collections.Generic;

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
    }
}