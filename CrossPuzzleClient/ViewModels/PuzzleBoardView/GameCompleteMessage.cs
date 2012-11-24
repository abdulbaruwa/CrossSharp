namespace CrossPuzzleClient.ViewModels.PuzzleBoardView
{
    public class GameCompleteMessage
    {
        public string UserName { get; set; }
        public int GameId { get; set; }
        public int ScorePercentage { get; set; }
    }
}