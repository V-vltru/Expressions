namespace Expressions.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Entire struct represents the interval
    /// forbidden for the parses
    /// </summary>
    public class Interval
    {
        private int _idxFrom;
        private int _idxTo;

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

        /// <summary>
        /// Constructor identifies the parameters of the class:
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
        /// Gets or sets the start index of the interval
        /// </summary>
        public int IdxFrom
        {
            get
            {
                return _idxFrom;
            }
            set
            {
                _idxFrom = value;
            }
        }

        /// <summary>
        /// Gets or sets the end index of the interval
        /// </summary>
        public int IdxTo
        {
            get
            {
                return _idxTo;
            }
            set
            {
                _idxTo = value;
            }
        }
    }
}
