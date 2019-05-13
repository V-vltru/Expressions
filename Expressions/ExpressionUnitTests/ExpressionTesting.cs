namespace ExpressionUnitTesting
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Expressions;
    using Expressions.Models;

    [TestClass]
    public class ExpressionTesting
    {
        [TestMethod]
        public void ExpressionGetStandardFunctionsTest()
        {
            string expression = "sin(a) + cos(a + b) / (2 + sin(x))";
            List<Interval> intervals = new List<Interval>
            {
                new Interval(22, 33)
            };

            List<StandardFunction> expected = new List<StandardFunction>
            {
                new StandardFunction(0, "sin"),
                new StandardFunction(9, "cos")
            };


            Expression e = new Expression();
            List<StandardFunction> actual = e.GetStandardFunctions(expression, intervals);

            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Name, actual[i].Name);
                Assert.AreEqual(expected[i].Idx, actual[i].Idx);
            }
        }

        [TestMethod]
        public void ExpressionGetOperatorsTest1()
        {
            string expression = "a + b - c / (2 + a)";
            List<Operator> expected = new List<Operator>()
            {
                new Operator(2, '+'),
                new Operator(6, '-')
            };
            Expression exp = new Expression();

            List<Operator> actual = exp.GetOperators(expression, '+', '-');

            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Idx, actual[i].Idx);
                Assert.AreEqual(expected[i].OperatorName, actual[i].OperatorName);
            }
        }

        [TestMethod]
        public void ExpressionGetOperatorsTest2()
        {
            string expression = "a * b - c / (2 + a)";
            List<Operator> expected = new List<Operator>()
            {
                new Operator(2, '*'),
                new Operator(10, '/')
            };
            Expression exp = new Expression();

            List<Operator> actual = exp.GetOperators(expression, '*', '^', '/');

            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Idx, actual[i].Idx);
                Assert.AreEqual(expected[i].OperatorName, actual[i].OperatorName);
            }
        }

        [TestMethod]
        public void ExpressionGetOperatorsTest3()
        {
            string expression = "a ^ b - c / (2 + a)";
            List<Operator> expected = new List<Operator>()
            {
                new Operator(2, '^'),
                new Operator(10, '/')
            };
            Expression exp = new Expression();

            List<Operator> actual = exp.GetOperators(expression, '*', '^', '/');

            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Idx, actual[i].Idx);
                Assert.AreEqual(expected[i].OperatorName, actual[i].OperatorName);
            }
        }
    }
}
