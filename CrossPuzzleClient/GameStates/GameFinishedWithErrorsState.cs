using System;
using System.Collections.Generic;
using CrossPuzzleClient.ViewModels;
using CrossPuzzleClient.ViewModels.PuzzleBoardView;

namespace CrossPuzzleClient.GameStates
{
    public class GameFinishedWithErrorsState : GameState
    {
        public GameFinishedWithErrorsState(PuzzleBoardViewModel puzzleBoardViewModel) : base(puzzleBoardViewModel)
        {
        }

        public override IEnumerable<Type> ValidStatesICanBecome
        {
            get { return null; }
        }

        public override void Become(IGameState prevState)
        {
            PuzzleBoardViewModel.StartPauseButtonCaption = "Game Over :-(";
            PuzzleBoardViewModel.PauseTimer();
            PuzzleBoardViewModel.FireGameCompleteMessage();
            base.Become(prevState);
        }
    }
}