using System;
using System.Collections.Generic;
using CrossPuzzleClient.ViewModels;

namespace CrossPuzzleClient.GameStates
{
    public interface IGameState
    {
        void Become(IGameState prevState);
        bool CanBecome(IGameState newState);
        PuzzleBoardViewModel PuzzleBoardViewModel { get; set; }
        string StateName { get;}
        IEnumerable<Type> ValidStatesICanBecome { get; }
        bool EqualsOrHasBeen(IGameState newStateRequired); 
    }
}