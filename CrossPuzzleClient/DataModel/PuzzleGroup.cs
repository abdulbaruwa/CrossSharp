﻿using System.Collections.Generic;
using SQLite;

namespace CrossPuzzleClient.DataModel
{
    //Science
    public class PuzzleGroup
    {
        public int PuzzleGroupId { get; set; }
        public string Name { get; set; }
        public List<PuzzleSubGroup> Puzzles { get; set; }
    }

    //KS2 - Science
    public class PuzzleSubGroup
    {
        public string Title { get; set; }
        public int PuzzleSubGroupId { get; set; }
        public bool GamePlayed { get; set; }
        public int GameScore { get; set; }
        public Dictionary<string, string> Words { get; set; }
        public List<PuzzleGame> PuzzleGames { get; set; }
    }

    public class PuzzleGame
    {
        public int PuzzleGameId { get; set; }
        public bool GamePlayed { get; set; }
        public int GameScore { get; set; }
        public Dictionary<string, string> Words { get; set; }
    }

    public class PuzzleGroupData
    {
        public int PuzzleGroupDataId { get; set; }
        public string Data { get; set; }
    }

    public class PuzzleGroupGameData
    {
        [PrimaryKey]
        public int PuzzleGroupDataId { get; set; }
        public string GameUserName { get; set; }
        public string Data { get; set; }
    }
}