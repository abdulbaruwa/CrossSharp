using System;
using System.Linq;
using System.Collections.Generic;
using CrossPuzzleClient.ViewModels;

namespace CrossPuzzleClient.GameStates
{
    public class GamePauseState   : GameState
    {
        public GamePauseState(PuzzleBoardViewModel puzzleBoardViewModel) : base(puzzleBoardViewModel)
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

        public override void Become(IGameState prevState)
        {
            //Pause by dispossing current Observable.
            //StopTime();
            //GameIsRunning = !_gameIsRunning;

            //SetStartPauseDisplayCommand();
            //if(_gameIsRunning) GameCountUpCommand.Execute(null);
            if (prevState.ValidStatesICanBecome.Contains(this.GetType()))
            {

                PuzzleBoardViewModel.StartPauseButtonCaption = "Start";
                PuzzleBoardViewModel.PauseTimer();
                base.Become(prevState);

            }
        }
    }
}