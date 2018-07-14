/// <summary>
/// Namespace contains all models which are operated by <see cref="Expressions.Expression" /> class.
/// </summary>
namespace Expressions.Models
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Entire struct represents the interval
    /// forbidden for the parses
    /// </summary>
    public class Interval
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Interval" /> class.
        /// [idxStart; idxEnd]
        /// </summary>
        /// <param name="idxFrom">The start index of the interval</param>
        /// <param name="idxTo">The end index of the interval</param>
        public Interval(int idxFrom, int idxTo)
        {
            this.IdxFrom = idxFrom;
            this.IdxTo = idxTo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Interval" /> class.
        /// </summary>
        /// <param name="interval">Instance of the interval which will be copied to the current one</param>
        public Interval(Interval interval)
        {
            this.IdxFrom = interval.IdxFrom;
            this.IdxTo = interval.IdxTo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Interval" /> class.
        /// Default constructor
        /// </summary>
        public Interval()
        {
        }

        /// <summary>
        /// Gets or sets the start index of the interval
        /// </summary>
        public int IdxFrom { get; set; }

        /// <summary>
        /// Gets or sets the end index of the interval
        /// </summary>
        public int IdxTo { get; set; }

        /// <summary>
        /// Method defines whether the specified index belongs to at least one interval from the list of them
        /// </summary>
        /// <param name="idx">Index (position in string) to define if it belongs to intervals</param>
        /// <param name="intervals">List of the intervals</param>
        /// <returns>the falg: true - belongs to at least one interval; false - does not belong to any of the intervals</returns>
        public static bool BelongsToIntevals(int idx, List<Interval> intervals)
        {
            int idxInIntervalsCount = (from g in intervals
                                       where idx >= g.IdxFrom && idx <= g.IdxTo
                                       select g).Count();

            if (idxInIntervalsCount == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
