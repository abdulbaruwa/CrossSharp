using CrossPuzzleClient.ViewModels.PuzzlesView;

namespace CrossPuzzleClient.ViewModels.PuzzleBoardView
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
}