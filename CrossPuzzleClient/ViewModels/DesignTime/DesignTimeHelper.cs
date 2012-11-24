using System;
using System.Collections.Generic;
using CrossPuzzleClient.ViewModels.PuzzlesView;

namespace CrossPuzzleClient.ViewModels.DesignTime
{
    public static class DesignTimeHelper
    {
        public static string GetRandomChar()
        {
            var x = Convert.ToChar(new Random().Next(65, 90)).ToString();
            return x;
        } 
    }

    public static class ViewModelHelper
    {
        public static PuzzleViewModel FakePuzzleBuilder(string title)
        {
            var puzzleVm = new PuzzleViewModel
                               {Title = title,
                                   Words = 
                                   new List<string>
                                           {
                                               "First",
                                               "Second",
                                               "Third",
                                               "Forth",
                                               "Fifth",
                                               "Sixth"
                                           }
                               };
            return puzzleVm;
        }

    }
}