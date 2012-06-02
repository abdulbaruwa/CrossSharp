using CrossSharpClient.Common;

namespace CrossSharpClient.DataModel
{
    public class CellViewModel : BindableBase
    {
        private int _col;
        private int _row;
        private string _value = string.Empty;
        private bool _isFirtCharacter;
        private string _startPosition;
        private Direction _direction;

        public CellViewModel(int col, int row, string value,bool isFirtCharacter, Direction direction )
        {
            _value = value;
            _col = col;
            _row = row;
            _isFirtCharacter = isFirtCharacter;
            _direction = direction;
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

        public string StartPosition
        {
            get
            {
                return _isFirtCharacter ? _startPosition : string.Empty;
            }
        }

        public bool ShowVertical
        {
            get { return _isFirtCharacter && (Direction == Direction.Down || Direction == Direction.Both); }
        }

        public bool ShowHorizontal
        {
            get { return _isFirtCharacter && (Direction == Direction.Across || Direction == Direction.Both); }
        }

        public Direction Direction
        {
            get { return _direction; }
        }
    }

    public enum Direction
    {
        Down,
        Across,
        Both
    }

    public class DesignCellViewModel : BindableBase
    {
        private int _col;
        private int _row;
        private string _value = string.Empty;
        private string _startPosition;
        private bool _isFirtCharacter;
        private Direction _direction;
        public DesignCellViewModel()
        {
            _value = BoardViewModel.GetRandomChar();
            _col = 2;
            _row = 0;
            _isFirtCharacter = true;
            _direction = Direction.Down;
            _startPosition = 3.ToString();
        }

        public string StartPosition
        {
            get
            {
                return _isFirtCharacter ? _startPosition : string.Empty;
            }
        }

        public bool ShowVertical
        {
            get { return _isFirtCharacter && (Direction == Direction.Down || Direction == Direction.Both); }
        }

        public bool ShowHorizontal
        {
            get { return _isFirtCharacter && ( Direction == Direction.Across || Direction == Direction.Both); }
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

        public bool IsFirtCharacter
        {
            get { return _isFirtCharacter; }
        }

        public Direction Direction
        {
            get { return _direction; }
        }
    }
}