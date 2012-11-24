using System;
using System.Collections.Generic;
using System.Linq;
using CrossPuzzleClient.ViewModels;
using CrossPuzzleClient.ViewModels.PuzzleBoardView;

namespace CrossPuzzleClient.GameStates
{
    public abstract class GameState : IGameState
    {
        protected GameState(PuzzleBoardViewModel puzzleBoardViewModel)
        {
            PuzzleBoardViewModel = puzzleBoardViewModel;
        }

        public virtual void Become(IGameState prevState)
        {
            SetNewGameState(this);
        }

        private void SetNewGameState(GameState gameState)
        {
            PuzzleBoardViewModel.CurrentGameState = gameState;
        }

        public virtual bool CanBecome(IGameState newState)
        {
            if(ValidStatesICanBecome == null) return false;
            if ( ValidStatesICanBecome.Any())
            {
                return ValidStatesICanBecome.Contains(newState.GetType());
            }

            return true;
        }

        public PuzzleBoardViewModel PuzzleBoardViewModel { get; set; }

        public string StateName { get; private set; }

        public abstract IEnumerable<Type> ValidStatesICanBecome { get; }

        public bool EqualsOrHasBeen(IGameState newStateRequired)
        {
            return (Equals(newStateRequired) || PreviousGameStateContains(newStateRequired));
        }

        private bool PreviousGameStateContains(IGameState newStateRequired)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return (StateName != null ? StateName.GetHashCode() : 0);
        }

        protected bool Equals(GameState other)
        {
            return string.Equals(StateName, other.StateName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType().FullName == GetType().FullName) return true;
            if (obj.GetType() != this.GetType()) return false;
            if (obj.GetType() != typeof(GameState)) return false;
            return Equals((GameState) obj);
        }
    }
}