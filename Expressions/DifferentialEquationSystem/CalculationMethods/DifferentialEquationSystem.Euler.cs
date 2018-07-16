namespace DifferentialEquationSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Expressions;
    using Expressions.Models;

    public partial class DifferentialEquationSystem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variablesAtAllStep"></param>
        /// <returns></returns>
        public List<InitVariable> EulerCalculation(List<List<InitVariable>> variablesAtAllStep = null)
        {
            DifferentialEquationSystemHelpers.CheckVariables(this.ExpressionSystem, this.LeftVariables, this.TimeVariable, this.Tau, this.TEnd);

            List<Variable> allVars;
            List<Variable> currentLeftVariables = new List<Variable>();
            List<Variable> nextLeftVariables = new List<Variable>();

            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, currentLeftVariables);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, nextLeftVariables);

            Variable currentTime = new Variable(this.TimeVariable);

            if (variablesAtAllStep != null)
            {
                variablesAtAllStep.Clear();
                List<InitVariable> initLeftVariables = new List<InitVariable>();
                foreach (Variable leftVariable in this.LeftVariables)
                {
                    initLeftVariables.Add(leftVariable);
                }

                initLeftVariables.Add(currentTime);
                variablesAtAllStep.Add(initLeftVariables);
            }

            do
            {
                // calculation time incrimentation
                currentTime.Value += this.Tau;

                // Combinig of variables
                allVars = new List<Variable>();
                allVars.AddRange(currentLeftVariables);
                if (this.Constants != null && this.Constants.Count > 0)
                {
                    allVars.AddRange(this.Constants);
                }

                allVars.Add(currentTime);

                // Calculation 
                for (int i = 0; i < nextLeftVariables.Count; i++)
                {
                    nextLeftVariables[i].Value += this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
                }

                // Saving of all variables at current iteration
                if (variablesAtAllStep != null)
                {
                    List<InitVariable> varsAtIteration = new List<InitVariable>();
                    DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, varsAtIteration);
                    varsAtIteration.Add(new Variable(currentTime));
                }

                // Next variables are becoming the current ones for the next iteration
                DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, currentLeftVariables);

            } while (currentTime.Value < this.TEnd);

            List<InitVariable> result = new List<InitVariable>();
            foreach(Variable var in currentLeftVariables)
            {
                result.Add(new InitVariable(var.Name, var.Value));
            }

            return result;
        }
    }
}
