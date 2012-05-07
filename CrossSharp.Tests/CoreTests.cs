using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MS.Internal.Xml.XPath;
using NUnit.Framework;
using CrossSharp.Core;
namespace CrossSharp.Tests
{
    [TestFixture]
    public class CoreTests
    {
        [Test]
        public void AddFirstWord()
        {
            string newWord = "Bamidele";
            var result = GetBoardWithFirstWord(newWord);


            for (int i = 0; i < newWord.Count()-1; i++)
            {
                Assert.AreEqual(result[0, i], newWord[i].ToString());
            }            
        }

        private string[,] GetBoardWithFirstWord(string word)
        {
            var board = CoreHorizontal.GetBoard(15,15);
            return CoreHorizontal.AddFirstWord(word, board);
        }

        private string[,] GetBoardWithFirstAndSecondWords(string wordOne, string wordTwo)
        {
            var board = GetBoardWithFirstWord(wordOne);
            CoreHorizontal.AddSecondWord(wordTwo, board);
            return board;
        }


        [Test]
        public void Given_board_with_first_word_and_word_with_matching_first_letter_Should_return_position_of_matching_first_letter()
        {
            var board = GetBoardWithFirstWord("Bamidele");
            var position = CoreHorizontal.MatchFirstLetterOfWord("india".ToCharArray(), board);

            Assert.AreEqual(3,position);
        }


        [Test]
        public void Given_board_with_first_word_and_word_with_non_matching_first_letter_Should_return_position_minus_one()
        {
            var board = GetBoardWithFirstWord("Bamidele");
            var position = CoreHorizontal.MatchFirstLetterOfWord("queen".ToCharArray(), board);

            Assert.AreEqual(-1, position);
        }

        [Test]
        public void Given_board_with_first_word_and_second_word_with_matching_first_letter_Should_return_matching_position_of_first_letter()
        {
            var board = GetBoardWithFirstWord("Bamidele");
            var secondWordChars = "india".ToCharArray();
            var position = CoreHorizontal.MatchFirstLetterOfWord(secondWordChars, board);
            Assert.AreEqual(3, position);
        }
        [Test]
        public void Given_board_with_first_word_and_second_word_with_non_matching_first_letter_Should_return_minus_one()
        {
            var board = GetBoardWithFirstWord("Bamidele");
            var secondWordChars = "nonexisting".ToCharArray();
            var position = CoreHorizontal.MatchFirstLetterOfWord(secondWordChars, board);
            Assert.AreEqual(-1, position);
        }

        [Test]
        public void Given_board_with_first_word_and_second_word_with_matching_first_letter_Should_add_word_to_board()
        {
            var board = GetBoardWithFirstWord("Bamidele");
            var secondWordChars = "india";
            var result = CoreHorizontal.AddSecondWord(secondWordChars, board);
            PrintBoard(board);
            Assert.AreEqual(true, result);
            for (int i = 0; i < 14; i++)
            {
                Assert.AreEqual(i < secondWordChars.Length ? secondWordChars[i].ToString() : "_", board[i, 3]);
            }
        }

        [Test]
        public void Given_board_with_first_word_and_second_word_with_non_matching_first_letter_Should_fail_not_add_word_to_board()
        {
            var board = GetBoardWithFirstWord("Bamidele");
            var secondWordChars = "nonexisting";
            var result = CoreHorizontal.AddSecondWord(secondWordChars, board);
            Assert.AreEqual(false, result);
        }
    
        //Vertical Tests

        [Test]
        public void Given_base_board_Should_return_true_if_a_vertical_word_can_be_added()
        {
            var board = GetBoardWithFirstAndSecondWords("Bamidele","india");
            CoreHorizontal.AddWordHorizontally( "adamsandler", board);
            var vertword = "station";

            PrintBoard(board);
            CoreVertical.AddWordVertically(vertword, board);
            PrintBoard(board);

            
            for (int i = 0; i < vertword.Length -1; i++)
            {
                Assert.AreEqual((vertword[i]).ToString(), board[2+i,(6)]);
            }
        }

        [Test]
        public void Given_base_board_Should_return_fail_to_insert_if_horizontal_neighbouring_cells_have_letters_in_them()
        {
            var board = GetBoardWithFirstAndSecondWords("Bamidele", "india");
            CoreHorizontal.AddWordHorizontally("adamsandler", board);
            board[6, 7] = "x";
            board[9, 5] = "x";
            PrintBoard(board);
            var vertword = "station";
                      
            var result = CoreVertical.AddWordVertically(vertword, board);
            Assert.IsFalse(result.inserted);
            //Check from second char - first char exists via another word
            for (int i = 1; i < vertword.Length - 1; i++)
            {
                Assert.AreEqual(CoreHorizontal.emptyCell, board[2 + i, (6)]);
            }
            PrintBoard(board);
        }

        [Test]
        public void Given_base_board_Should_return_fail_to_insert_if_vertical_neighbouring_cells_have_letters_in_them()
        {
            var board = GetBoardWithFirstAndSecondWords("Bamidele", "india");
            CoreHorizontal.AddWordHorizontally("adamsandler", board);
            //Add spurious char to board to tail of ensure i
            board[9, 6] = "x";

            var vertword = "station";

            var result = CoreVertical.AddWordVertically(vertword, board);
            Assert.IsFalse(result.inserted);
            //Check from second char - first char exists via another word
            for (int i = 1; i < vertword.Length - 1; i++)
            {
                Assert.AreEqual(CoreHorizontal.emptyCell, board[2 + i, (6)]);
            }
            PrintBoard(board);
        }

        [Test]
        public void Given_a_set_words_should_iterate_and_apply_valid_words_start_from_the_largest()
        {
            var words = new List<string>();
            words.Add("Bamidele");
            words.Add("station");
            words.Add("india");
            words.Add("Adams");
            //var sortedwords = CoreHorizontal.sortWords(words.ToArray());
            //CoreHorizontal.AddWords(words);


        }

        private static void PrintBoard(string[,] board)
        {
            var result = CoreHorizontal.printboard(board);
            foreach (var str in result)
            {
                Debug.Print(str);
            }
        }
    }
}
