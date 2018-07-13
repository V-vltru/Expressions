namespace ExpressionUnitTesting
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;    
    using Expressions;

    [TestClass]
    public class ExpressionParsingHelpersTesting
    {
        [TestMethod]
        public void ExpressionParsingHelpersDeleteEmptyBracketsTest()
        {
            string initialStr = "()a + b() - c()";
            string resultStr = "a + b - c";

            Assert.AreEqual(resultStr, ExpressionParsingHelpers.DeleteEmptyBrackets(initialStr));
        }

        [TestMethod]
        public void ExpressionParsingHelpersRemoveSpacesTest()
        {
            string initialStr = "a + b - c             ";
            string resultStr = "a+b-c";

            Assert.AreEqual(resultStr, ExpressionParsingHelpers.RemoveSpaces(initialStr));
        }

        [TestMethod]
        public void ExpressionParsingHelpersCheckBracketBalanceTest()
        {
            string correctStr = "(((a + b) - c) / (2))";
            string wrongStr = "(a + b) - c) / 2";
            string wrongStr2 = "(((a + b) - c) / 2";

            Assert.AreEqual(true, ExpressionParsingHelpers.CheckBracketBalance(correctStr));
            Assert.AreEqual(false, ExpressionParsingHelpers.CheckBracketBalance(wrongStr));
            Assert.AreEqual(false, ExpressionParsingHelpers.CheckBracketBalance(wrongStr2));
        }

        [TestMethod]
        public void ExpressionParsingHelpersAddMinusOneTest()
        {
            string minusTwo = "-2";
            Assert.AreEqual(minusTwo, ExpressionParsingHelpers.AddMinusOne(minusTwo));

            string minusA = "-A + b";
            string minusAResult = "(-1)*A + b";
            Assert.AreEqual(minusAResult, ExpressionParsingHelpers.AddMinusOne(minusA));
        }
        
        [TestMethod]
        public void ExpressionParsingHelpersGetAllSubstringIndexesTest()
        {
            string initialString = "sin(x) + cos(a) - 2 * sin(y)";
            string substring = "sin";

            List<int> expected = new List<int>
            {
                0, 22
            };

            CollectionAssert.AreEquivalent(expected, ExpressionParsingHelpers.GetAllSubstringIndexes(initialString, substring));
        }

        [TestMethod]
        public void ExpressionParsingHelpersCOPYTest()
        {
            string source = "abcdefghijk";
            string result = "bcde";

            Assert.AreEqual(result, ExpressionParsingHelpers.COPY(source, 1, 4));
        }

        [TestMethod]
        public void ExpressionParsingHelpersRemoveWrappedBracketsTest()
        {
            string errorMessage = "Bracket balance is not observed!";

            List<string> initialExpressions = new List<string>
            {
                "(a + b)",
                "((a + b))",
                "((a + b - (c * 2)))",
                "a - b",
                "(a - b) + c"
            };

            List<string> resultExpressions = new List<string>
            {
                "a + b",
                "a + b",
                "a + b - (c * 2)",
                "a - b",
                "(a - b) + c"
            };

            string fakeExpression = "(a - b + c";

            for (int i = 0; i < initialExpressions.Count; i++)
            {
                Assert.AreEqual(resultExpressions[i], ExpressionParsingHelpers.RemoveWrappedBrackets(initialExpressions[i]));
            }

            try
            {
                ExpressionParsingHelpers.RemoveWrappedBrackets(fakeExpression);
            }
            catch (Exception e)
            {
                Assert.AreEqual(errorMessage, e.Message);
            }
        }
    }
}
