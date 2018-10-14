namespace Expressions
{
    using System;
    using System.Collections.Generic;
    using Expressions.Models;

    public partial class Expression
    {
        public void DefineLeaves(Tree parent, string expression)
        {
            string expressionPrev;
            do
            {
                expressionPrev = expression;
                expression = ExpressionParsingHelpers.RemoveWrappedBrackets(expression);
            } while (expression != expressionPrev);

            List<StandardFunction> funcs = new List<StandardFunction>();
            List<Operator> operators = this.GetOperators(expression, '-', '+');
            if (operators.Count == 0)
            {
                operators = this.GetOperators(expression, '*');

                if (operators.Count == 0)
                {
                    operators = this.GetOperators(expression, '/');

                    if (operators.Count == 0)
                    {
                        operators = this.GetOperators(expression, '^');
                        if (operators.Count == 0)
                        {
                            List<Interval> intervals = this.GetIntervals(expression);
                            funcs = this.GetStandardFunctions(expression, intervals);

                            if (funcs.Count == 0 && ExpressionParsingHelpers.NumberOrVariable(this.Variables, expression) == EssenceType.Nothing)
                            {
                                throw new Exception("No operators, no functions, no variables were found in string: " + expression);
                            }
                        }
                    }
                }
            }

            EssenceType type;

            if (operators.Count > 0)
            {
                type = EssenceType.Operator;
            }
            else if (funcs.Count > 0)
            {
                type = EssenceType.StandardFunction;
            }
            else if (ExpressionParsingHelpers.NumberOrVariable(this.Variables, expression) != EssenceType.Nothing)
            {
                type = ExpressionParsingHelpers.NumberOrVariable(this.Variables, expression);
            }
            else
            {
                throw new Exception("Can't determine type of operator/function/Variable/Number");
            }

            if (type == EssenceType.Operator)
            {
                parent.DataType = type;
                parent.Data = operators[0].OperatorName.ToString();
                parent.StringLeft = ExpressionParsingHelpers.COPY(expression, 0, operators[0].Idx - 1);
                parent.StringRight = ExpressionParsingHelpers.COPY(expression, operators[0].Idx + 1, expression.Length - 1);
                parent.LeftOperand = 0;
                parent.RightOperand = 0;

                parent.LeftLeave = new Tree();
                parent.RightLeave = new Tree();

                this.DefineLeaves(parent.LeftLeave, parent.StringLeft);
                this.DefineLeaves(parent.RightLeave, parent.StringRight);
            }
            else if (type == EssenceType.StandardFunction)
            {
                parent.DataType = type;
                parent.Data = funcs[0].Name;
                parent.StringLeft = null;
                parent.StringRight = ExpressionParsingHelpers.COPY(expression, funcs[0].Idx + funcs[0].Name.Length, expression.Length - 1);
                parent.LeftOperand = 0;
                parent.RightOperand = 0;

                parent.LeftLeave = null;
                parent.RightLeave = new Tree();

                this.DefineLeaves(parent.RightLeave, parent.StringRight);
            }
            else if (type == EssenceType.Variable || type == EssenceType.Number)
            {
                parent.DataType = type;
                parent.Data = expression;
                parent.StringLeft = null;
                parent.StringRight = null;
                parent.LeftOperand = 0;
                parent.RightOperand = 0;

                parent.LeftLeave = null;
                parent.RightLeave = null;
            }
        }
    }
}
