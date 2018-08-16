namespace Expressions
{
    using System.Collections.Generic;
    using Expressions.Models;

    public partial class Expression
    {
        /// <summary>
        /// Method defines the intervals in the expression
        /// </summary>
        /// <param name="expression">Initial expression</param>
        /// <returns>The list of Interval instances</returns>
        public List<Interval> GetIntervals(string expression)
        {
            List<Interval> result = new List<Interval>();

            int idxFrom = -1;
            int idxTo = -1;
            int bracketBalance = 0;

            for (int expressionIndex = 0; expressionIndex < expression.Length; expressionIndex++)
            {
                if (expression[expressionIndex] == '(')
                {
                    bracketBalance++;
                    if (idxFrom == -1)
                    {
                        idxFrom = expressionIndex;
                    }
                }
                else if (expression[expressionIndex] == ')')
                {
                    bracketBalance--;
                    if (bracketBalance == 0)
                    {
                        idxTo = expressionIndex;
                    }
                }

                if (idxFrom >= 0 && idxTo >= 0)
                {
                    result.Add(new Interval(idxFrom, idxTo));

                    idxFrom = -1;
                    idxTo = -1;
                }
            }

            return result;
        }
    }
}
