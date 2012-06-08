using CrossPuzzleClient.Common;

namespace CrossPuzzleClient.ViewModels
{
    public class CellViewModel : CellEmptyViewModel
    {
        private WordViewModel _wordViewModel;

        public CellViewModel(int col, int row, string value, WordViewModel wordViewModel) : base(col, row, value)
        {
            _wordViewModel = wordViewModel;
        }

        public WordViewModel Word
        {
            get { return _wordViewModel; }
            set { SetProperty(ref _wordViewModel, value); }
        }
    }

    public class CellEmptyViewModel : BindableBase
    {
        private int _col;
        private int _row;
        private string _value = string.Empty;
        private bool _isVisible;

        public CellEmptyViewModel(int col, int row, string value)

        {
            _value = value;
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

        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }
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