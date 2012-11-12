using System;
using CrossPuzzleClient.Common;

namespace CrossPuzzleClient.ViewModels
{
    public class CellViewModel : CellEmptyViewModel
    {
        private WordViewModel _wordViewModel;
        public CellViewModel(int col, int row, string value, WordViewModel wordViewModel, string wordPosition) : base(col, row, value)
        {
            _wordViewModel = wordViewModel;
            base.WordPosition = wordPosition;
            IsVisible = CellState.IsUsed;
        }

        public WordViewModel Word
        {
            get { return _wordViewModel; }
            set { SetProperty(ref _wordViewModel, value); }
        }

    }

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

    public class DesignCellViewModel : BindableBase
    {
        private int _col;
        private int _row;
        private string _value = string.Empty;
        private string _startPosition;
        private bool _isFirtCharacter;
        private Direction _direction;
        private WordViewModel _wordViewModel;
        private bool _isVisible;

        public DesignCellViewModel()
        {
            _value = DesignTimeHelper.GetRandomChar();
            _col = 2;
            _row = 0;
            _isFirtCharacter = true;
            _direction = Direction.Down;
            _startPosition = 3.ToString();
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

        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }
    }
}