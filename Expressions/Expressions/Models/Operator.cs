namespace Expressions.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Operator in expression may contain
    /// '+' '-' '*' '/' '^'
    /// </summary>
    public class Operator
    {
        /// <summary>
        /// The list of acceptable operators
        /// </summary>
        private static List<char> wellKnownOperators = new List<char>
        {
            '+', '-', '*', '/', '^'
        };

        /// <summary>
        /// Error message which will be printed when user attempts to specify 
        /// not-accessable operator
        /// </summary>
        private static string errorMessage;

        private int _idx;
        private char _operatorName;

        /// <summary>
        /// Method returns the value of the binary operator
        /// </summary>
        /// <param name="leftOp">Left operand</param>
        /// <param name="rightOp">Right operand</param>
        /// <param name="OperatorName">Name of the operator</param>
        /// <returns>The result of operator</returns>
        public static double GetValue(double leftOp, double rightOp, char OperatorName)
        {
            switch (OperatorName)
            {
                case '+': return leftOp + rightOp;
                case '-': return leftOp - rightOp;
                case '/': return leftOp / rightOp;
                case '*': return leftOp * rightOp;
                case '^': return Math.Pow(leftOp, rightOp);
                default: throw new Exception(errorMessage.Replace("%p%", OperatorName.ToString()));
            }
        }

        /// <summary>
        /// Operator generates the error message in case of addition of not accessable parameter.
        /// It shows the list of allowable parameters to help 
        /// </summary>
        static Operator()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Operator %p% is not accessable. Here is the list of allowable operators:");

            foreach (char op in wellKnownOperators)
            {
                stringBuilder.Append(string.Format("\n{0}", op));
            }

            errorMessage = stringBuilder.ToString();
        }

        /// <summary>
        /// The constructor sets the parameters of the class
        /// </summary>
        /// <param name="idx">The position of the operator</param>
        /// <param name="operatorName">The name of the operator</param>
        public Operator(int idx, char operatorName)
        {
            this.Idx = idx;
            this.OperatorName = operatorName;
        }

        /// <summary>
        /// Gets or sets the position of the operator
        /// </summary>
        public int Idx
        {
            get
            {
                return _idx;
            }
            set
            {
                _idx = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the operator
        /// </summary>
        public char OperatorName
        {
            get
            {
                return _operatorName;
            }
            set
            {
                if (wellKnownOperators.Contains(value))
                {
                    _operatorName = value;
                }
                else
                {
                    throw new Exception(errorMessage.Replace("%p%", value.ToString()));
                }
            }
        }
    }
}
