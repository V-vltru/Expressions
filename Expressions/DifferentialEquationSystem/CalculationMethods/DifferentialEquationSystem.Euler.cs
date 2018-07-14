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
        public List<Variable> EulerCalculation(List<List<Variable>> variablesAtAllStep = null)
        {
            List<Variable> allVars;
            List<Variable> currentLeftVariables = new List<Variable>();
            List<Variable> nextLeftVariables = new List<Variable>();

            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, currentLeftVariables);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, nextLeftVariables);

            Variable currentTime = new Variable(this.TimeVariable.Name, this.TimeVariable.Value);

            if (variablesAtAllStep != null)
            {
                variablesAtAllStep.Clear();
                variablesAtAllStep.Add(currentLeftVariables);
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
                    List<Variable> varsAtIteration = new List<Variable>();
                    foreach(Variable variable in nextLeftVariables)
                    {
                        varsAtIteration.Add(new Variable(variable.Name, variable.Value));
                    }

                    varsAtIteration.Add(new Variable(currentTime.Name, currentTime.Value));
                }

                // Next variables are becoming the current ones for the next iteration
                DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, currentLeftVariables);

            } while (currentTime.Value < this.TEnd);

            return currentLeftVariables;
        }
    }
}
