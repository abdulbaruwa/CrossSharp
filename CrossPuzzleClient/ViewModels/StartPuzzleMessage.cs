namespace CrossPuzzleClient.ViewModels
{
    internal class StartPuzzleMessage 
    {
        public int PuzzleId { get; set; }
    }

    public class KeyReceivedMessage
    {
        public string KeyChar { get; set; }
    }
}