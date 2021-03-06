using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CrossPuzzleClient.Infrastructure;
using CrossPuzzleClient.ViewModels.PuzzleBoardView;
using CrossPuzzleClient.ViewModels.PuzzlesView;
using CrossSharp.Core;

namespace CrossPuzzleClient.Services
{
    public class PuzzlesService : IPuzzlesService
    {
        private IPuzzleRepository puzzlesRepository;
        private IUserService _userService;

        public PuzzlesService(IPuzzleRepository puzzlesRepository, IUserService userService)
        {
            this.puzzlesRepository = puzzlesRepository;
            _userService = userService;
        }

        public ObservableCollection<WordViewModel> GetOrdereredWordsForPuzzle(int puzzleId, string user)
        {
            var words = puzzlesRepository.GetPuzzleWithId(puzzleId, user);

            var wordviewmodels = GetWordsWordviewmodels(words);

            return SortWordsByPositionOnBoard(wordviewmodels);

        }
        
        public IList<WordViewModel> GetWordsInsertableIntoPuzzle(Dictionary<string,string> words )
        {

            try
            {
                var wordviewmodels = GetWordsWordviewmodels(words);

                return SortWordsByPositionOnBoard(wordviewmodels);
            }
            catch (Exception e)
            {
                throw;
            }
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

        public string[,] GetEmptyBoard()
        {
            return CoreHorizontal.GetBoard(12, 12);
        }

        private static void PrintBoard(string[,] board)
        {
            var result = CoreHorizontal.printboard(board);
            foreach (var str in result)
            {
                Debug.WriteLine(str);
            }
        }

        public List<WordViewModel> GetWordsWordviewmodels(Dictionary<string,string> words)
        {

            ////var words = new List<string>();
            //words.Add("Bamidele");
            //words.Add("station");
            //words.Add("india");
            //words.Add("Adams");
            //words.Add("fards");
            //words.Add("novemb");
            //words.Add("belt");
            //words.Add("train");
            //words.Add("adeola");
            //words.Add("amoeba");
            //words.Add("moscow");
            var board = CoreHorizontal.GetBoard(12, 12);
            //var wordKeys = words. .Select(x => x.Key).ToArray();
            var result = (CoreVertical.AddWordsAttempts(words.Keys.ToArray(), board));
            PrintBoard(board);
            var wordsInserted = result.Item1.Where(x => x.inserted);
            var wordviewmodels = new List<WordViewModel>();
            foreach (var word in wordsInserted)
            {
                Debug.WriteLine(word.word);
                var position = (word.row * 12) + word.col;
                CoreHorizontal.resultCell word1 = word;
                var wordViewModel = new WordViewModel()
                {
                    Cells = new ObservableCollection<CellEmptyViewModel>(),
                    Direction = GetDirection(word.orientation),
                    Word = word.word,
                    WordHint = words.First(x =>x.Key == word1.word).Value,
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