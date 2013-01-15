using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using System.Diagnostics;
using System.Linq;
using CrossSharp.Core;
using System.Collections.Generic;

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
            var board = CoreHorizontal.GetBoard(12,12);
            return CoreHorizontal.AddFirstWord(word, board);
        }

        private string[,] GetBoardWithFirstAndSecondWords(string wordOne, string wordTwo)
        {
            var board = GetBoardWithFirstWord(wordOne);
            CoreHorizontal.AddSecondWord(wordTwo, board);
            return board;
        }

        [Test]
        public void Cell_validation_should_fail_for_word_if_prefix_or_post_fix_of_target_location_contains_a_cell_with_a_character_in_it()
        {
            var board = GetBoardWithFirstWord("Bamidele");

            board[5, 0] = "x";
            var shouldBeFalse = CoreHorizontal.HorizontalWordHasNoLetterAtStartOrEnd(board, new CoreHorizontal.cell(5, 1), 4);

            Assert.IsFalse(shouldBeFalse);
            board[7, 6] = "x";
            
            var shouldBeTrue = CoreHorizontal.HorizontalWordHasNoLetterAtStartOrEnd(board, new CoreHorizontal.cell(7, 0), 6);
            Assert.IsFalse(shouldBeTrue);

            Assert.IsTrue(CoreHorizontal.HorizontalWordHasNoLetterAtStartOrEnd(board, new CoreHorizontal.cell(8, 1), 11));

            board[9, 0] = "x";
            Assert.IsFalse(CoreHorizontal.HorizontalWordHasNoLetterAtStartOrEnd(board, new CoreHorizontal.cell(9, 1), 11));

            board[11, 11] = "x";
            Assert.IsFalse(CoreHorizontal.HorizontalWordHasNoLetterAtStartOrEnd(board, new CoreHorizontal.cell(11, 1), 10));
        }

        [Test]
        public void Give_a_cell_on_a_board_check_if_the_cell_directly_below_it_is_empty()
        {
            var board = GetBoardWithFirstWord("Bamidele");
            Assert.IsFalse(CoreHorizontal.hasbottomchar(board, 1, 0));
            Assert.IsFalse(CoreHorizontal.hasbottomchar(board, 0, 0));
            Assert.IsFalse(CoreHorizontal.hasbottomchar(board, 1, 0));
            Assert.IsFalse(CoreHorizontal.hasbottomchar(board, 11, 0));
            Assert.IsTrue(CoreHorizontal.hastopchar(board, 1, 0));
            Assert.IsFalse(CoreHorizontal.hastopchar(board, 0, 0));
            Assert.IsFalse(CoreHorizontal.hastopchar(board, 11, 0));

            board[4, 0] = "x";
            Assert.IsTrue(CoreHorizontal.hastopchar(board, 5, 0));
            Assert.IsTrue(CoreHorizontal.hasbottomchar(board, 3, 0));

        }

        [Test]
        public void When_told_to_add_the_second_word_to_a_board_with_first_word_of_12_chars_inserted_Should_add_the_word_as_expected()
        {
            var board = GetBoardWithFirstWord("Confectioner");

            var result = CoreHorizontal.AddSecondWord("Rhubarb", board);
            PrintBoard(board);
            Assert.IsTrue(result.Item1);

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
            for (int i = 0; i < board.GetLength(1); i++)
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
            PrintBoard(board);

            CoreHorizontal.AddWordHorizontally( "adamsandler", board);
            var vertword = "station";

            PrintBoard(board);
            CoreVertical.AddWordVertically(vertword, board);
            PrintBoard(board);

            //B a m i d e l e _ _ _ _ 
            //_ _ _ n _ _ _ _ _ _ _ _ 
            //_ s _ d _ _ _ _ _ _ _ _ 
            //_ t _ i _ _ _ _ _ _ _ _ 
            //_ a d a m s a n d l e r 
            //_ t _ _ _ _ _ _ _ _ _ _ 
            //_ i _ _ _ _ _ _ _ _ _ _ 
            //_ o _ _ _ _ _ _ _ _ _ _ 
            //_ n _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _

            for (int i = 0; i < vertword.Length -1; i++)
            {
                Assert.AreEqual((vertword[i]).ToString(), board[2+i,(1)]);
            }
        }

        [Test]
        public void Given_base_board_Should_return_fail_to_insert_if_horizontal_neighbouring_cells_have_letters_in_them()
        {
            var board = GetBoardWithFirstAndSecondWords("Bamidele", "india");
            CoreHorizontal.AddWordHorizontally("adamsandler", board);
            board[8, 1] = "x";
            board[8, 6] = "x";
            board[9, 5] = "x";
            PrintBoard(board);
            var vertword = "station";
                      
            var result = CoreVertical.AddWordVertically(vertword, board);
            PrintBoard(board);

            Assert.IsFalse(result.inserted);
            
            PrintBoard(board);
            //B a m i d e l e _ _ _ _ 
            //_ _ _ n _ _ _ _ _ _ _ _ 
            //_ _ _ d _ _ _ _ _ _ _ _ 
            //_ _ _ i _ _ _ _ _ _ _ _ 
            //_ a d a m s a n d l e r 
            //_ _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ 
            //x _ _ _ _ _ x _ _ _ _ _ 
            //_ _ _ _ _ x _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _
        }

        [Test]
        public void Given_base_board_Should_return_fail_to_insert_if_vertical_neighbouring_cells_have_letters_in_them()
        {
            var board = GetBoardWithFirstAndSecondWords("Bamidele", "india");
            CoreHorizontal.AddWordHorizontally("adamsandler", board);
            //Add spurious char to board to tail of ensure i
            board[8, 0] = "x";
            board[8, 6] = "x";
            board[9, 5] = "x";

            var vertword = "station";

            var result = CoreVertical.AddWordVertically(vertword, board);
            PrintBoard(board);
            Assert.IsFalse(result.inserted);
            //Check from second char - first char exists via another word
           
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
        public void Given_a_set_words_should_iterate_and_apply_valid_words_starting_from_the_largesta()
        {
            var words = new List<string>();
            words.Add("Bamidele");
            words.Add("station");
            words.Add("india");
            words.Add("Adams");
            words.Add("novemb");

            var board = CoreHorizontal.GetBoard(12, 12);
            var result = (CoreVertical.AddWordsAttempts(words.ToArray(), board));
            foreach (var s in result.Item1)
            {
                Assert.IsTrue(s.inserted);
            }

            PrintBoard(result.Item2);


            //B a m i d e l e _ _ _ _ 
            //_ _ _ n _ _ _ _ _ _ _ _ 
            //_ _ A d a m s _ _ _ _ _ 
            //_ _ _ i _ _ _ _ _ _ _ _ 
            //_ s t a t i o n _ _ _ _ 
            //_ _ _ _ _ _ _ o _ _ _ _ 
            //_ _ _ _ _ _ _ v _ _ _ _ 
            //_ _ _ _ _ _ _ e _ _ _ _ 
            //_ _ _ _ _ _ _ m _ _ _ _ 
            //_ _ _ _ _ _ _ b _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ 
            //_ _ _ _ _ _ _ _ _ _ _ _ 
        }


        [Test]
        public void Test_a_set_of_words()
        {
            var packagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "puzzledataEnglishWordsList1.txt");
            var wordsAndHints = File.ReadLines(packagePath);
            var wordDic = wordsAndHints.Select(fileDataInLine => fileDataInLine.Split(new[] { '|' }))
                                 .ToDictionary(lineArray => lineArray[0], lineArray => lineArray[1]);


            
            var words = new List<string>();
            //words.Add("Kettledrum");
            //words.Add("Embezzler");
            //words.Add("Gladioli");
            //words.Add("Sieve");
            //words.Add("Ewes");
            //words.Add("Retort");
            //words.Add("Rescuing");
            //words.Add("Retrieve");

            //words.Add("Kettledrum");
            //words.Add("Cabaret");
            //words.Add("words.Add(Shopping");
            //words.Add("Embezzler");
            //words.Add("Automatic");
            //words.Add("Gladioli");
            //words.Add("Competition");
            //words.Add("Sargent");
            //words.Add("Sieve");
            //words.Add("Compass");
            //words.Add("Playwright");
            //words.Add("Trombone");
            //words.Add("Encyclopaedia");
            //words.Add("Ewes");
            //words.Add("Rand");
            //words.Add("Plotted");
            //words.Add("Perennials");
            //words.Add("Catapult");
            //words.Add("Retort");
            //words.Add("Dubbin");
            //words.Add("Separating");
            //words.Add("Accelerator");
            //words.Add("Aisle");
            //words.Add("Castanets");
            //words.Add("Rescuing");
            //words.Add("Retrieve");
            //words.Add("Aphid");
            //words.Add("Goddess");
            //words.Add("Periscope");
            //words.Add("Zipfastener");
            //words.Add("Peninsula");
            //words.Add("Glacier");
            //words.Add("Poliomyelitis");
            //words.Add("Transistor");
            //words.Add("TapeMeasure");
            //words.Add("Alligator");
            //words.Add("Buries");
            //words.Add("Retirement");
            //words.Add("Dyed");
            //words.Add("Pliers");
            //words.Add("Suggest");
            //words.Add("Fulfil");
            //words.Add("Virtue");
            //words.Add("Museum");
            //words.Add("Salmon");
            //words.Add("Absurd");
            //words.Add("Warriors");
            //words.Add("Ceremony");
            //words.Add("Procession");
            //words.Add("Theatre");
            //words.Add("Issue");
            //words.Add("Pitiful");
            //words.Add("Succession");
            //words.Add("Welfare");
            //words.Add("Courageous");
            //words.Add("Skilful");
            //words.Add("Rescue");
            //words.Add("Siege");
            //words.Add("Unusual");
            //words.Add("Egypt");
            //words.Add("Positive");
            //words.Add("Spaniard");
            //words.Add("Stubborn");
            //words.Add("Column");
            //words.Add("Courteous");
            //words.Add("Christian");
            //words.Add("Appreciation"); ;
            var board = CoreHorizontal.GetBoard(12, 12);
            var result1 = (CoreVertical.AddWords(wordDic.Keys.ToArray(), board));
           // var result = (CoreVertical.AddWordsAttempts(wordDic.Keys.ToArray(), board));
            //PrintBoard(result.Item2);
            PrintBoard(board);
        }

        [Test]
        public void Test_Words_from_a_file()
        {
            var packagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "puzzledataEnglishWordsList1.txt");
            var wordsAndHints = File.ReadLines(packagePath);
            var wordDic = wordsAndHints.Select(fileDataInLine => fileDataInLine.Split(new[] { '|' }))
                                 .ToDictionary(lineArray =>  lineArray[0].Trim(), lineArray => lineArray[1].Trim());
            var board = CoreHorizontal.GetBoard(12, 12);
            var words = wordDic.Keys.ToArray();
            var result1 = (CoreVertical.AddWordsInThreeIterations(words, board));
            PrintBoard(board);
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
            words.Add("amoeba");
            words.Add("moscow");

            var board = CoreHorizontal.GetBoard(12,12);
            var result = (CoreVertical.AddWordsAttempts(words.ToArray(), board));
            //var result2 = (CoreVertical.AddMinimumTenWords(words.ToArray(), board));
            PrintBoard(result.Item2);
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
