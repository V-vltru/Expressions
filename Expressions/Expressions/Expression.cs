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
        private string expr;

        /// <summary>
        /// 
        /// </summary>
        public List<StandardFunction> StandardFunctions { get; set; }

        /// <summary>
        /// Dictionary of variables in the expression
        /// </summary>
        public Dictionary<string, double> Variables { get; set; }        

        public double GetResultValue(Dictionary<string, double> variables)
        {
            return this.GetExpressionResult(this.parent, variables);
        }

        /// <summary>
        /// Constructor validates the input expression and defines the expression tree
        /// </summary>
        /// <param name="expression">Initial expression</param>
        /// <param name="variables"></param>
        public Expression(string expression, Dictionary<string, double> variables)
        {
            if (ExpressionParsingHelpers.CheckBracketBalance(expression))
            {
                this.parent = new Tree();
                this.Variables = variables;

                expression = ExpressionParsingHelpers.RemoveSpaces(expression);
                expression = ExpressionParsingHelpers.DeleteEmptyBrackets(expression);
                expression = ExpressionParsingHelpers.AddMinusOne(expression);

                this.expr = expression;
                this.DefineLeaves(parent, this.expr);
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
        public Expression(string expression): this(expression, new Dictionary<string, double>())
        {}

        public Expression() { }
    }
}
