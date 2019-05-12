/// <summary>
/// Namespace contains all models which are operated by <see cref="Expressions.Expression" /> class.
/// </summary>
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

        /// <summary>
        /// Private member which saves the content of <see cref="Operator.OperatorName" /> property.
        /// </summary>
        private char operatorName;

        /// <summary>
        /// Initializes static members of the <see cref="Operator" /> class.
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
        /// Initializes a new instance of the <see cref="Operator" /> class.
        /// </summary>
        /// <param name="idx">The position of the operator</param>
        /// <param name="operatorName">The name of the operator</param>
        public Operator(int idx, char operatorName)
        {
            this.Idx = idx;
            this.OperatorName = operatorName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Operator" /> class.
        /// </summary>
        /// <param name="op">Instance of Operator class which is supposed to be copied to the current one</param>
        public Operator(Operator op)
        {
            this.Idx = op.Idx;
            this.OperatorName = op.OperatorName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Operator" /> class.
        /// Default constructor
        /// </summary>
        public Operator()
        {
        }

        /// <summary>
        /// Gets or sets the position of the operator
        /// </summary>
        public int Idx { get; set; }

        /// <summary>
        /// Gets or sets the name of the operator
        /// </summary>
        public char OperatorName
        {
            get
            {
                return this.operatorName;
            }

            set
            {
                if (wellKnownOperators.Contains(value))
                {
                    this.operatorName = value;
                }
                else
                {
                    throw new Exception(errorMessage.Replace("%p%", value.ToString()));
                }
            }
        }

        /// <summary>
        /// Method returns the value of the binary operator
        /// </summary>
        /// <param name="leftOp">Left operand</param>
        /// <param name="rightOp">Right operand</param>
        /// <param name="operatorName">Name of the operator</param>
        /// <returns>The result of operator</returns>
        public static double GetValue(double leftOp, double rightOp, char operatorName)
        {
            switch (operatorName)
            {
                case '+': return leftOp + rightOp;
                case '-': return leftOp - rightOp;
                case '/': return leftOp / rightOp;
                case '*': return leftOp * rightOp;
                case '^': return Math.Pow(leftOp, rightOp);
                default: throw new Exception(errorMessage.Replace("%p%", operatorName.ToString()));
            }
        }

        /// <summary>
        /// Method returns the value of the binary operator
        /// </summary>
        /// <param name="leftOp">Left operand</param>
        /// <param name="rightOp">Right operand</param>
        /// <param name="operator">Name of the operator</param>
        /// <returns>The result of operator</returns>
        public static double GetValue(double leftOp, double rightOp, Operator @operator)
        {
            return Operator.GetValue(leftOp, rightOp, @operator.OperatorName);
        }

        /// <summary>
        /// Method is used to split the expression by operators.
        /// </summary>
        /// <param name="operators">List of operators.</param>
        /// <param name="expression">Initial expression.</param>
        /// <returns>The list of split sub-expressions.</returns>
        public static List<string> SplitExpressionByOperators(List<Operator> operators, string expression)
        {
            if (operators != null && operators.Count > 0)
            {
                List<string> result = new List<string>();

                StringBuilder currentExpression = new StringBuilder();

                int currentIndex = 0;

                foreach (Operator op in operators)
                {
                    currentExpression.Clear();

                    for (int i = currentIndex; i < op.Idx; i++)
                    {
                        currentExpression.Append(expression[i]);
                    }

                    result.Add(currentExpression.ToString());
                    currentIndex = op.Idx + 1;
                }

                result.Add(expression.Substring(currentIndex));

                return result;
            }

            return null;
        }
    }
}
