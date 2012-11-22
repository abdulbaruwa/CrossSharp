using System;
using System.Collections.Generic;

namespace CrossPuzzleClient.ViewModels
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
            var puzzleVm = new PuzzleViewModel { Title = title };
            puzzleVm.Words = new List<string>();
            puzzleVm.Words.Add("First");
            puzzleVm.Words.Add("Second");
            puzzleVm.Words.Add("Third");
            puzzleVm.Words.Add("Forth");
            puzzleVm.Words.Add("Fifth");
            puzzleVm.Words.Add("Sixth");
            return puzzleVm;
        }

    }
}