using System.Linq;
using NUnit.Framework;

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
            var board = Core.board;
            return Core.AddFirstWord(word, board);
        }

        [Test]
        public void Given_board_with_first_word_and_word_with_matching_first_letter_Should_return_position_of_matching_first_letter()
        {
            var board = GetBoardWithFirstWord("Bamidele");
            var position = Core.MatchFirstLetterOfWord("india".ToCharArray(), board);

            Assert.AreEqual(3,position);
        }


        [Test]
        public void Given_board_with_first_word_and_word_with_non_matching_first_letter_Should_return_position_minus_one()
        {
            var board = GetBoardWithFirstWord("Bamidele");
            var position = Core.MatchFirstLetterOfWord("queen".ToCharArray(), board);

            Assert.AreEqual(-1, position);
        }

        [Test]
        public void Given_board_with_first_word_and_second_word_with_matching_first_letter_Should_return_matching_position_of_first_letter()
        {
            var board = GetBoardWithFirstWord("Bamidele");
            var secondWordChars = "india".ToCharArray();
            var position = Core.MatchFirstLetterOfWord(secondWordChars, board);
            Assert.AreEqual(3, position);
        }
        [Test]
        public void Given_board_with_first_word_and_second_word_with_non_matching_first_letter_Should_return_minus_one()
        {
            var board = GetBoardWithFirstWord("Bamidele");
            var secondWordChars = "nonexisting".ToCharArray();
            var position = Core.MatchFirstLetterOfWord(secondWordChars, board);
            Assert.AreEqual(-1, position);
        }

        [Test]
        public void Given_board_with_first_word_and_second_word_with_matching_first_letter_Should_add_word_to_board()
        {
            var board = GetBoardWithFirstWord("Bamidele");
            var secondWordChars = "india".ToCharArray();
            var result = Core.AddSecondWord(secondWordChars, board);
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
            var secondWordChars = "nonexisting".ToCharArray();
            var result = Core.AddSecondWord(secondWordChars, board);
            Assert.AreEqual(false, result);
        }

    }
}
