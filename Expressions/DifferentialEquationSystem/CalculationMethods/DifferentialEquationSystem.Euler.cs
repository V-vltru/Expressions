namespace DifferentialEquationSystem
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Expressions.Models;

    public partial class DifferentialEquationSystem
    {        
        /// <summary>
        /// Sync Euler calculation body
        /// </summary>
        /// <param name="variablesAtAllStep">Container where the intermediate parameters are supposed to be saved</param>
        /// <returns>List of result variables</returns>
        private List<DEVariable> EulerSync(List<List<DEVariable>> variablesAtAllStep = null)
        {
            // Put left variables, constants and time variable in the one containier
            List<Variable> allVars;
            List<Variable> currentLeftVariables = new List<Variable>();
            List<Variable> nextLeftVariables = new List<Variable>();

            // Copy this.LeftVariables to the current one and to the nex one
            // To leave this.LeftVariables member unchanged (for further calculations)
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, currentLeftVariables);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, nextLeftVariables);

            // Setting of current time (to leave this.TimeVariable unchanged)
            Variable currentTime = new Variable(this.TimeVariable);

            // If it is required to save intermediate calculations - save the start values
            if (variablesAtAllStep != null)
            {
                // This is the first record for intermediate calculations containier
                // It has to be clear
                variablesAtAllStep.Clear();

                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, this.LeftVariables, currentTime);
            }

            do
            {
                // Combinig of variables
                allVars = DifferentialEquationSystemHelpers.CollectVariables(currentLeftVariables, this.Constants, currentTime);

                // Calculation 
                for (int i = 0; i < nextLeftVariables.Count; i++)
                {
                    nextLeftVariables[i].Value = currentLeftVariables[i].Value + this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
                }

                // Saving of all variables at current iteration
                if (variablesAtAllStep != null)
                {
                    DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, nextLeftVariables, currentTime);
                }

                // Next variables are becoming the current ones for the next iteration
                DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, currentLeftVariables);

                // calculation time incrimentation
                currentTime.Value += this.Tau;
            } while (currentTime.Value < this.TEnd);

            List<DEVariable> result = new List<DEVariable>();
            DifferentialEquationSystemHelpers.CopyVariables(currentLeftVariables, result);
            return result;
        }

        /// <summary>
        /// Async Euler calculation body
        /// </summary>
        /// <param name="variablesAtAllStep">Container where the intermediate parameters are supposed to be saved</param>
        /// <returns>List of result variables</returns>
        private List<DEVariable> EulerAsync(List<List<DEVariable>> variablesAtAllStep = null)
        {
            // Put left variables, constants and time variable in the one containier
            List<Variable> allVars;
            List<Variable> currentLeftVariables = new List<Variable>();
            List<Variable> nextLeftVariables = new List<Variable>();

            // Copy this.LeftVariables to the current one and to the nex one
            // To leave this.LeftVariables member unchanged (for further calculations)
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, currentLeftVariables);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, nextLeftVariables);

            // Setting of current time (to leave this.TimeVariable unchanged)
            Variable currentTime = new Variable(this.TimeVariable);

            // If it is required to save intermediate calculations - save the start values
            if (variablesAtAllStep != null)
            {
                // This is the first record for intermediate calculations containier
                // It has to be clear
                variablesAtAllStep.Clear();

                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, this.LeftVariables, currentTime);
            }

            do
            {                
                // Combinig of variables
                allVars = DifferentialEquationSystemHelpers.CollectVariables(currentLeftVariables, this.Constants, currentTime);

                Parallel.For(0, nextLeftVariables.Count, (i) =>
                {
                    nextLeftVariables[i].Value += this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
                });

                // Saving of all variables at current iteration
                if (variablesAtAllStep != null)
                {
                    DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, nextLeftVariables, currentTime);
                }

                // Next variables are becoming the current ones for the next iteration
                DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, currentLeftVariables);

                // calculation time incrimentation
                currentTime.Value += this.Tau;
            } while (currentTime.Value < this.TEnd);

            List<DEVariable> result = new List<DEVariable>();
            DifferentialEquationSystemHelpers.CopyVariables(currentLeftVariables, result);
            return result;
        }
    }
}
