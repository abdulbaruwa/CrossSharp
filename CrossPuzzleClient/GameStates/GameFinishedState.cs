using System;
using System.Collections.Generic;
using CrossPuzzleClient.ViewModels;

namespace CrossPuzzleClient.GameStates
{
    public class GameFinishedState : GameState
    {
        public GameFinishedState(PuzzleBoardViewModel puzzleBoardViewModel) : base(puzzleBoardViewModel)
        {
        }

        public override IEnumerable<Type> ValidStatesICanBecome
        {
            get { return null; }
        }

        public override void Become(IGameState prevState)
        {
            PuzzleBoardViewModel.StartPauseButtonCaption = "Game Over :-)";
            PuzzleBoardViewModel.PauseTimer();
            PuzzleBoardViewModel.FireGameCompleteMessage();
            base.Become(prevState);
        }
    }
}