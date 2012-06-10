using System.Collections.ObjectModel;
using CrossPuzzleClient.Common;

namespace CrossPuzzleClient.ViewModels
{
    public class WordViewModel : BindableBase
    {

        private ObservableCollection<CellEmptyViewModel> _cells;
        private Direction _direction;
        private string _word;
        private int _index;

        public ObservableCollection<CellEmptyViewModel> Cells
        {
            get { return _cells; }
            set { SetProperty(ref _cells, value); }
        }

        
        public string Word
        {
            get { return _word; }
            set { SetProperty(ref _word, value); }
        }

        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }

        public Direction Direction
        {
            get { return _direction; }
            set { SetProperty(ref _direction, value); }
        }
    }
}