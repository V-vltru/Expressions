namespace Expressions
{
    using System;
    using System.Collections.Generic;
    using Expressions.Models;

    public partial class Expression
    {
        /// <summary>
        /// Get the specified operators in the specified expression
        /// </summary>
        /// <param name="expression">The expression for searching of operators</param>
        /// <param name="operatorNames">The sequence of operators</param>
        /// <returns>The list of Operator instances</returns>
        public List<Operator> GetOperators(string expression, params char[] operatorNames)
        {
            List<Operator> result = new List<Operator>();

            List<char> operators = new List<char>();
            operators.AddRange(operatorNames);

            int bBalance = 0;
            for (int expressionIndex = 0; expressionIndex < expression.Length; expressionIndex++)
            {
                if (expression[expressionIndex] == '(')
                {
                    bBalance++;
                }
                else if (expression[expressionIndex] == ')')
                {
                    bBalance--;
                    if (bBalance < 0)
                    {
                        throw new Exception("Bracket balance is not observed");
                    }
                }
                else
                {
                    if (bBalance == 0)
                    {
                        if (operators.Contains(expression[expressionIndex]))
                        {
                            result.Add(new Operator(expressionIndex, expression[expressionIndex]));
                        }
                    }
                }
            }

            return result;
        }
    }
}
