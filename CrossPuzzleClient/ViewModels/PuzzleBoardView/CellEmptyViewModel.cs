﻿using System;
using CrossPuzzleClient.Common;

namespace CrossPuzzleClient.ViewModels.PuzzleBoardView
{
    public class CellEmptyViewModel : BindableBase
    {

        private string _wordPosition = string.Empty;

        protected bool Equals(CellEmptyViewModel other)
        {

            return _col == other._col && _row == other._row && string.Equals(_value, other._value,StringComparison.OrdinalIgnoreCase) && _isVisible.Equals(other._isVisible) && string.Equals(_enteredvalue, other._enteredvalue, StringComparison.CurrentCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = _col;
                hashCode = (hashCode*397) ^ _row;
                hashCode = (hashCode*397) ^ (_value != null ? _value.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ _isVisible.GetHashCode();
                hashCode = (hashCode*397) ^ (_enteredvalue != null ? _enteredvalue.GetHashCode() : 0);
                return hashCode;
            }
        }

        private int _col;
        private int _row;
        private string _value = string.Empty;
        private CellState _isVisible;
        private string _enteredvalue;

        public CellEmptyViewModel(int col, int row, string value)

        {
            _value = value.ToUpperInvariant();
            _col = col;
            _row = row;
        }

        public int Row
        {
            get { return _row; }
            set { SetProperty(ref _row, value); }
        }

        public int Col
        {
            get { return _col; }
            set { SetProperty(ref _col, value); }
        }

        public string Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        public string EnteredValue
        {
            get { return _enteredvalue; }
            set { SetProperty(ref _enteredvalue, value); }
 
        }
        public string WordPosition
        {
            get { return _wordPosition; }
            set { SetProperty(ref _wordPosition, value); }
        }
        public CellState IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CellEmptyViewModel) obj);
        }
    }

    public enum CellState
    {
        IsEmpty,
        IsUsed,
        IsActive,
        IsError
    }
    public enum Direction
    {
        Down,
        Across
    }
}