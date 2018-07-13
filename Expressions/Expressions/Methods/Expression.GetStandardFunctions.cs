namespace Expressions
{
    using System.Collections.Generic;
    using Expressions.Models;

    public partial class Expression
    {
        /// <summary>
        /// Gets all available (not in the brackets) functions.
        /// </summary>
        /// <param name="expression">The expression to get all vailable functions there</param>
        /// <param name="intervals">List of intervals, where functions are not available</param>
        /// <returns>List of standard functions instances</returns>
        public List<StandardFunction> GetStandardFunctions(string expression, List<Interval> intervals)
        {
            List<StandardFunction> standardFunctions = new List<StandardFunction>();

            foreach (string function in StandardFunction.WellKnownFunctions)
            {
                List<int> funcIndexes = ExpressionParsingHelpers.GetAllSubstringIndexes(expression, function);

                foreach (int idx in funcIndexes)
                {
                    if (!Interval.BelongsToIntevals(idx, intervals))
                    {
                        standardFunctions.Add(new StandardFunction(idx, function));
                    }
                }
            }

            return standardFunctions;
        }
    }
}
