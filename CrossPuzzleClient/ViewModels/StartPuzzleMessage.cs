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

    public enum KeyCharType
    {
        Character,
        BackSpace,
        Delete
    }
}