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
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '(')
                {
                    bBalance++;
                }
                else if (expression[i] == ')')
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
                        if (operators.Contains(expression[i]))
                        {
                            result.Add(new Operator(i, expression[i]));
                        }
                    }
                }
            }

            return result;
        }
    }
}
