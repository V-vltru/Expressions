namespace Expressions
{
    using System;
    using System.Collections.Generic;
    using Expressions.Models;

    public partial class Expression
    {
        /// <summary>
        /// Method renders the tree and returns the result of the expression
        /// </summary>
        /// <param name="parent">Parent leave of the expression</param>
        /// <param name="variables">List of variables</param>
        /// <returns>The result of the expression</returns>
        public double GetExpressionResult(Tree parent, List<Variable> variables)
        {
            double leftOp = 0;
            double rightOp = 0;

            double result = 0;

            if (parent.DataType == EssenceType.Cascade)
            {
                List<double> cascadeResults = new List<double>();

                for (int i = 0; i < parent.Cascade.Count; i++)
                {
                    double cascadeResult = this.GetExpressionResult(parent.Cascade[i], variables);
                    cascadeResults.Add(cascadeResult);
                }

                double totalResult = cascadeResults[0];

                for (int i = 0; i < parent.CascadeOperators.Count; i++)
                {
                    totalResult = Operator.GetValue(totalResult, cascadeResults[i + 1], parent.CascadeOperators[i].OperatorName);
                }

                return totalResult;
            }

            if (parent.DataType != EssenceType.Variable && parent.DataType != EssenceType.Number)
            {
                if (parent.DataType == EssenceType.Operator)
                {
                    leftOp = this.GetExpressionResult(parent.LeftLeave, variables);
                }

                rightOp = this.GetExpressionResult(parent.RightLeave, variables);

                if (parent.DataType == EssenceType.Operator)
                {
                    result = Operator.GetValue(leftOp, rightOp, parent.Data[0]);
                }
                else
                {
                    result = StandardFunction.GetResultOfStandardFunction(rightOp, parent.Data);
                }
            }
            else if (parent.DataType == EssenceType.Variable)
            {
                result = Variable.GetVariableValue(parent.Data, variables);
            }
            else
            {
                result = Convert.ToDouble(parent.Data);
            }

            return result;
        }
    }
}
