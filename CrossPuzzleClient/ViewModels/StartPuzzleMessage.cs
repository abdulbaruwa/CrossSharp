using System;

namespace CrossPuzzleClient.ViewModels
{
    internal class StartPuzzleMessage 
    {
        public int PuzzleId { get; set; }
    }

    public class KeyReceivedMessage
    {
        public string KeyChar { get; set; }

        public KeyCharType KeyCharType { get; set; }
    }

    class CellValueChangedMessage
    {
        public  int Row { get; set; }
        public  int Col { get; set; }
        public string Character { get; set; }
    }
    public enum KeyCharType
    {
        Character,
        BackSpace,
        Delete
    }
}