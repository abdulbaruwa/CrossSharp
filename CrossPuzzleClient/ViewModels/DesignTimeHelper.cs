using System;

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
}