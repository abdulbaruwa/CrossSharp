using System;
using System.Collections.Generic;
using System.Linq;
using CrossPuzzleClient.ViewModels;
using CrossPuzzleClient.ViewModels.PuzzleBoardView;

namespace CrossPuzzleClient.GameStates
{
    public class GameInProgressState : GameState
    {
        public GameInProgressState( PuzzleBoardViewModel puzzleBoardViewModel ) : base(puzzleBoardViewModel)
        {
        }

        public override IEnumerable<Type> ValidStatesICanBecome
        {
            get
            {
                return new[]
                           {
                               typeof (GameFinishedState), typeof (GameFinishedWithErrorsState), typeof(GamePauseState)
                           };
            }
        }

        public override void Become(IGameState prevState)
        {
            if (PuzzleBoardViewModel.CurrentGameState is GameNotStartedState || PuzzleBoardViewModel.CurrentGameState is GamePauseState)
            {
                PuzzleBoardViewModel.StartPauseButtonCaption = "Pause";
                PuzzleBoardViewModel.GameCountUpCommand.Execute(null);
                base.Become(prevState);
            }
        }
    }
}