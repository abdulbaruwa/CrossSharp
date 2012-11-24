namespace CrossPuzzleClient.ViewModels.PuzzleBoardView
{
    public class CellValueChangedMessage
    {
        public  int Row { get; set; }
        public  int Col { get; set; }
        public string Character { get; set; }
    }
}