using System.Collections.ObjectModel;
using System.Linq;
using CrossPuzzleClient.Common;
using GalaSoft.MvvmLight.Messaging;

namespace CrossPuzzleClient.ViewModels
{
    public class SelectedWordViewModel : WordViewModel
    {
        private int _cursor;

        public int Cursor
        {
            get { return _cursor; }
            set { SetProperty(ref _cursor, value); }
        }
    }

    public class WordViewModel : BindableBase
    {
        private ObservableCollection<CellEmptyViewModel> _cells;
        private Direction _direction;
        private int _index;
        private string _word;
        private bool _enteredValueAddedToBoard;
        private string _wordHint;

        public WordViewModel()
        {
            Messenger.Default.Register<CellValueChangedMessage>(this, m => HandleChangedCellValue(m));
        }

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

        public string WordHint
        {
            get { return _wordHint; }
            set { SetProperty(ref _wordHint, value); }
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

        public bool EnteredValueAddedToBoard
        {
            get { return _enteredValueAddedToBoard; }
            set { SetProperty(ref _enteredValueAddedToBoard, value); }
        }

        public void AcceptCellValueChanges()
        {
            Messenger.Default.Register<CellValueChangedMessage>(this, m => HandleChangedCellValue(m));
        }

        public void RejectCellValueChanges()
        {
            Messenger.Default.Unregister<CellValueChangedMessage>(this);
        }

        private void HandleChangedCellValue(CellValueChangedMessage cellValueChangedMessage)
        {
            //If this word has the changed cell passed in, modify the instance of the cell to reflect it's new value
            CellEmptyViewModel cell =
                Cells.FirstOrDefault(x => x.Col == cellValueChangedMessage.Col && x.Row == cellValueChangedMessage.Row);
            if (cell != null)
            {
                cell.EnteredValue = cellValueChangedMessage.Character;
            }
        }
    }
}