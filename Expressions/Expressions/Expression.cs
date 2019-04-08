namespace Expressions
{
    using System;
    using System.Collections.Generic;
    using Expressions.Models;

    public partial class Expression
    {
        /// <summary>
        /// Pointer to the parent Tree
        /// </summary>
        private Tree parent;

        /// <summary>
        /// Expression as string to parse
        /// </summary>
        public string ExpressionString { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<StandardFunction> StandardFunctions { get; set; }

        /// <summary>
        /// List of variables in the expression
        /// </summary>
        public List<Variable> Variables { get; set; }

        public double GetResultValue(List<Variable> variables)
        {
            return this.GetExpressionResult(this.parent, variables);
        }

        public double GetResultValue(Variable variable)
        {
            return this.GetExpressionResult(this.parent, new List<Variable> { variable });
        }

        /// <summary>
        /// Constructor validates the input expression and defines the expression tree
        /// </summary>
        /// <param name="expression">Initial expression</param>
        /// <param name="variables"></param>
        public Expression(string expression, List<Variable> variables)
        {
            if (ExpressionParsingHelpers.CheckBracketBalance(expression))
            {
                this.parent = new Tree();
                this.Variables = variables;

                expression = ExpressionParsingHelpers.RemoveSpaces(expression);
                expression = ExpressionParsingHelpers.DeleteEmptyBrackets(expression);
                expression = ExpressionParsingHelpers.AddMinusOne(expression);

                this.ExpressionString = expression;
                this.DefineLeaves(parent, this.ExpressionString);
            }
            else
            {
                throw new Exception("Ballance of brackets is invalid");
            }
        }

        /// <summary>
        /// Constructor validates the input expression and defines the expression tree
        /// </summary>
        /// <param name="expression">Initial expression</param>
        public Expression(string expression): this(expression, new List<Variable>())
        {}

        public Expression() { }
    }
}
