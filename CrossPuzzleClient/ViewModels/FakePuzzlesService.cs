using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CrossSharp.Core;

namespace CrossPuzzleClient.ViewModels
{
    public class FakePuzzlesService : IPuzzlesService
    {
        public ObservableCollection<WordViewModel> GetOrdereredWordsForPuzzle(int puzzleId)
        {
            var wordviewmodels = GetWordsWordviewmodels();

            return SortWordsByPositionOnBoard(wordviewmodels);
        }

        private ObservableCollection<WordViewModel> SortWordsByPositionOnBoard(List<WordViewModel> wordviewmodels)
        {
            var orderedWords = wordviewmodels.OrderBy(x => x.Index).ToList();
            int lastindex = orderedWords[0].Index;
            var sortedWordViewModel = new ObservableCollection<WordViewModel>();

            for (int i = 0; i < orderedWords.Count; i++)
            {
                if (i > 0)
                {
                    if (orderedWords[i].Index == lastindex)
                    {
                        lastindex = orderedWords[i].Index;
                        orderedWords[i].Index = orderedWords[i - 1].Index;
                    }
                    else
                    {
                        lastindex = orderedWords[i].Index;
                        orderedWords[i].Index = orderedWords[i - 1].Index + 1;
                    }
                }
                else
                {
                    orderedWords[i].Index = 1;
                }
                sortedWordViewModel.Add(orderedWords[i]);
            }
            return sortedWordViewModel;
        }

        public List<WordViewModel> GetWordsWordviewmodels()
        {
            var words = new List<string>();
            words.Add("Bamidele");
            words.Add("station");
            words.Add("india");
            words.Add("Adams");
            words.Add("fards");
            words.Add("novemb");
            words.Add("belt");
            words.Add("train");
            words.Add("adeola");
            words.Add("amoeba");
            words.Add("moscow");

            var board = CoreHorizontal.GetBoard(12, 12);
            var result = (CoreVertical.AddWordsAttempts(words.ToArray(), board));

            var wordsInserted = result.Item1.Where(x => x.inserted);
            var wordviewmodels = new List<WordViewModel>();
            foreach (var word in wordsInserted)
            {
                var position = (word.row * 12) + word.col;

                var wordViewModel = new WordViewModel()
                                        {
                                            Cells = new ObservableCollection<CellEmptyViewModel>(),
                                            Direction = GetDirection(word.orientation),
                                            Word = word.word,
                                            WordHint = " Hint for " + word.word,
                                            WordLength = "(" + word.word.Length.ToString() + ")",
                                            Index = position
                                        };

                var row = word.row;
                var col = word.col;
                foreach (var character in word.word)
                {
                    var cell = new CellViewModel(col, row, character.ToString(), wordViewModel, string.Empty);
                    if (word.orientation == CoreHorizontal.Orientation.horizontal)
                        col += 1;
                    else
                        row += 1;

                    wordViewModel.Cells.Add(cell);
                }
                wordviewmodels.Add((wordViewModel));
            }
            return wordviewmodels;
        }

        private Direction GetDirection(CoreHorizontal.Orientation orientation)
        {
            return orientation == CoreHorizontal.Orientation.horizontal ? Direction.Across : Direction.Down;
        }

    }
}