namespace DifferentialEquationSystem
{
    using System.Collections.Generic;
    using Expressions.Models;
    
    public partial class DifferentialEquationSystem
    {
        public List<InitVariable> ForecastCorrectionSync(List<List<InitVariable>> variablesAtAllStep = null)
        {
            List<Variable> allVars;
            List<Variable> currentLeftVariables = new List<Variable>();
            List<Variable> nextLeftVariables = new List<Variable>();

            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, currentLeftVariables);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, nextLeftVariables);

            Variable currentTime = new Variable(this.TimeVariable);

            if (variablesAtAllStep != null)
            {
                // This is the first record for intermediate calculations containier
                // It has to be clear
                variablesAtAllStep.Clear();

                // Copying of the initial left variables to the separate list which when is going to "variablesAtAllStep" containier
                List<InitVariable> initLeftVariables = new List<InitVariable>();
                DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, initLeftVariables);

                // Current time is also required to be saved in the intermediate vlues
                initLeftVariables.Add(currentTime);
                variablesAtAllStep.Add(initLeftVariables);               
            }

            do
            {
                currentTime.Value += this.Tau;
                // To Do: Calculation body

            } while (currentTime.Value < this.TEnd);

            List<InitVariable> result = new List<InitVariable>();
            DifferentialEquationSystemHelpers.CopyVariables(currentLeftVariables, result);
            return result;
        }
    }
}
