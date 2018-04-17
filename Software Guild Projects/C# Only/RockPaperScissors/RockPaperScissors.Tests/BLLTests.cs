using NUnit.Framework;
using RockPaperScissors.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPaperScissors.Tests
{
    [TestFixture]
    public class BLLTests
    {
        [TestCase(RPSChoice.Paper, RPSChoice.Paper, 0)]
        [TestCase(RPSChoice.Paper, RPSChoice.Rock, -1)]
        [TestCase(RPSChoice.Paper, RPSChoice.Scissors, 1)]
        [TestCase(RPSChoice.Rock, RPSChoice.Paper, 1)]
        [TestCase(RPSChoice.Rock, RPSChoice.Rock, 0)]
        [TestCase(RPSChoice.Rock, RPSChoice.Scissors, -1)]
        [TestCase(RPSChoice.Scissors, RPSChoice.Paper, -1)]
        [TestCase(RPSChoice.Scissors, RPSChoice.Rock, 1)]
        [TestCase(RPSChoice.Scissors, RPSChoice.Scissors, 0)]
        public void TestCompareThrows(RPSChoice a, RPSChoice b, int expected)
        {
            int actual = GameEngine.CompareThrows(a, b);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RockBeatsScissors()
        {
            IChoiceGetter p1 = new AlwaysRock();
            IChoiceGetter p2 = new AlwaysScissors();

            GameEngine testEngine = new GameEngine(p1, p2);

            var result = testEngine.PlayRound();

            Assert.AreEqual(-1, result.Result);

            Assert.AreEqual(RPSChoice.Rock, result.P1Choice);
            Assert.AreEqual(RPSChoice.Scissors, result.P2Choice);
        }
    }
}
