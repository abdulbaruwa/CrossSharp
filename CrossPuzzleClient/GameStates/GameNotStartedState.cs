using System;
using System.Collections.Generic;
using CrossPuzzleClient.ViewModels;
using CrossPuzzleClient.ViewModels.PuzzleBoardView;

namespace CrossPuzzleClient.GameStates
{
    public class GameNotStartedState : GameState
    {
        public GameNotStartedState(PuzzleBoardViewModel puzzleBoardViewModel)
            : base(puzzleBoardViewModel)
        {
        }

        public override IEnumerable<Type> ValidStatesICanBecome
        {
            get
            {
                return new[]
                           {
                               typeof (GameInProgressState)
                           };
            }
        }

        
    }
}