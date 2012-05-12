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
            Assert.AreEqual(true, result.Item1);
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
            Assert.AreEqual(false, result.Item1);
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
        public void Should_fail_to_insert_vertical_word_if_last_char_of_word_has_a_horizontal_neighbour()
         {
             var board = GetBoardWithFirstAndSecondWords("Bamidele", "india");
             board[4, 4] = "d";
             board[4, 5] = "a";
             board[4, 6] = "m";
             board[4, 7] = "s";
             board[6, 6] = "x";

             //0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 
             //0B a m i d e l e _ _ _ _ _ _ _ 
             //1_ _ _ n _ _ _ _ _ _ _ _ _ _ _ 
             //2_ _ _ d _ r _ _ _ _ _ _ _ _ _ 
             //3_ _ _ i _ e _ _ _ _ _ _ _ _ _ 
             //4_ _ _ a d a m s _ _ _ _ _ _ _  
             //5_ _ _ _ _ d _ _ _ _ _ _ _ _ _ 
             //6_ _ _ _ _ y x _ _ _ _ _ _ _ _ 
             //7_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
             //8_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
             //9_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
             //0_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
             //1_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
             //2_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
             //3_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
             //4_ _ _ _ _ _ _ _ _ _ _ _ _ _ _

             //An attempt to add ready as depicted should fail
             var result = CoreVertical.AddWordVertically("readk", board);
             PrintBoard(board);
             Assert.IsFalse(result.inserted);
         }

        [Test]
        public void Should_fail_to_insert_vertical_word_if_there_are_matching_chars_and_at_least_one_non_matching_char()
        {
            var board = GetBoardWithFirstAndSecondWords("Bamidele", "india");
            board[4, 4] = "d";
            board[4, 5] = "a";
            board[4, 6] = "m";
            board[4, 7] = "s";

            board[7, 5] = "X";

            //0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 
            //0B a m i d e l e _ _ _ _ _ _ _ 
            //1_ _ _ n _ _ _ _ _ _ _ _ _ _ _ 
            //2_ _ _ d _ r _ _ _ _ _ _ _ _ _ 
            //3_ _ _ i _ e _ _ _ _ _ _ _ _ _ 
            //4_ _ _ a d a m s _ _ _ _ _ _ _  
            //5_ _ _ _ _ d _ _ _ _ _ _ _ _ _ 
            //6_ _ _ _ _ y _ _ _ _ _ _ _ _ _ 
            //7_ _ _ _ _ X _ _ _ _ _ _ _ _ _ 
            //8_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //9_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //0_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //1_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //2_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //3_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //4_ _ _ _ _ _ _ _ _ _ _ _ _ _ _

            //An attempt to add ready as depicted should fail
            PrintBoard(board);
            var result = CoreVertical.AddWordVertically("ready", board);
            PrintBoard(board);

            Assert.IsFalse(result.inserted);

        }


        [Test]
        public void Should_fail_to_insert_horizontal_word_if_there_are_matching_chars_and_at_least_one_non_matching_char()
        {
            var board = GetBoardWithFirstAndSecondWords("Bamidele", "india");

            board[3, 5] = "x";
            //B a m i d e l e _ _ _ _ _ _ _ 
            //_ _ _ n _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ d _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ i _ x _ _ _ _ _ _ _ _ _  //Target row with matching value 'i'
            //_ _ _ a _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _
            var result = CoreHorizontal.AddWordHorizontally("fines", board);
            PrintBoard(board);
            Assert.IsFalse(result.inserted);
            

        }
        [Test]
        public void Given_a_set_words_should_iterate_and_apply_valid_words_starting_from_the_largest()
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


            var board = CoreHorizontal.GetBoard(15,15);
            var result = (CoreVertical.AddWordsAttempts(words.ToArray(), board));
            foreach (var s in result.Item1 )
            {
                Assert.IsTrue(s.inserted);
            }
            PrintBoard(result.Item2);

            //B A m i d e l e _ _ _ _ _ _ _ 
            //_ d _ n _ _ _ _ _ _ _ _ _ _ _ 
            //f a r d s _ _ _ _ _ _ _ _ _ _ 
            //_ m _ i _ _ n _ _ _ _ _ _ _ _ 
            //_ s t a t i o n _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ v _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ b e l t _ _ _ _ _ _ 
            //_ _ _ _ _ _ m _ r _ _ _ _ _ _ 
            //_ _ _ _ _ _ b _ a d e o l a _ 
            //_ _ _ _ _ _ _ _ i _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ n _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _
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
