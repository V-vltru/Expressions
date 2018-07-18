namespace DifferentialEquationSystem
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Expressions.Models;

    public partial class DifferentialEquationSystem
    {
        /// <summary>
        /// Method calculates a differential equation system with Euler method
        /// </summary>
        /// <param name="variablesAtAllStep">Container where the intermediate parameters are supposed to be saved</param>
        /// <param name="async">Flag which specifies if calculation should be performed in parallel mode</param>
        /// <returns>List of result variables</returns>
        public List<InitVariable> EulerCalculation(List<List<InitVariable>> variablesAtAllStep = null, bool async = false)
        {
            // Checking the correctness of input variables
            DifferentialEquationSystemHelpers.CheckVariables(this.ExpressionSystem, this.LeftVariables, this.TimeVariable, this.Tau, this.TEnd);

            if (!async)
            {
                return this.EulerSync(variablesAtAllStep);
            }
            else
            {
                return this.EulerAsync(variablesAtAllStep);
            }
        }

        /// <summary>
        /// Method calculates a differential equation system with Euler method
        /// </summary>
        /// <param name="calculationTime">Referenced parameter where calculation time is supposed to be loacted</param>
        /// <param name="variablesAtAllStep">Container where the intermediate parameters are supposed to be saved</param>
        /// <returns>List of result variables</returns>
        public List<InitVariable> EulerCalculation(out double calculationTime, List<List<InitVariable>> variablesAtAllStep = null, bool async = false)
        {
            Stopwatch stopwatch = new Stopwatch();
            // Checking the correctness of input variables
            DifferentialEquationSystemHelpers.CheckVariables(this.ExpressionSystem, this.LeftVariables, this.TimeVariable, this.Tau, this.TEnd);

            // Start time recording
            stopwatch.Start();

            List<InitVariable> result;
            if (!async)
            {
                result = this.EulerSync(variablesAtAllStep);
            }
            else
            {
                result = this.EulerAsync(variablesAtAllStep);
            }

            // Stop time recording
            stopwatch.Stop();
            calculationTime = stopwatch.ElapsedMilliseconds / 1000.0;

            return result;
        }

        /// <summary>
        /// Sync Euler calculation body
        /// </summary>
        /// <param name="variablesAtAllStep">Container where the intermediate parameters are supposed to be saved</param>
        /// <returns>List of result variables</returns>
        private List<InitVariable> EulerSync(List<List<InitVariable>> variablesAtAllStep = null)
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

                // Copying of the initial left variables to the separate list which when is going to "variablesAtAllStep" containier
                List<InitVariable> initLeftVariables = new List<InitVariable>();
                DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, initLeftVariables);

                // Current time is also required to be saved in the intermediate vlues
                initLeftVariables.Add(currentTime);
                variablesAtAllStep.Add(initLeftVariables);
            }

            do
            {
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
                    nextLeftVariables[i].Value = currentLeftVariables[i].Value + this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
                }

                // Saving of all variables at current iteration
                if (variablesAtAllStep != null)
                {
                    List<InitVariable> varsAtIteration = new List<InitVariable>();
                    DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, varsAtIteration);
                    varsAtIteration.Add(new Variable(currentTime));

                    variablesAtAllStep.Add(varsAtIteration);
                }

                // Next variables are becoming the current ones for the next iteration
                DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, currentLeftVariables);

                // calculation time incrimentation
                currentTime.Value += this.Tau;
            } while (currentTime.Value < this.TEnd);

            List<InitVariable> result = new List<InitVariable>();
            DifferentialEquationSystemHelpers.CopyVariables(currentLeftVariables, result);
            return result;
        }

        /// <summary>
        /// Async Euler calculation body
        /// </summary>
        /// <param name="variablesAtAllStep">Container where the intermediate parameters are supposed to be saved</param>
        /// <returns>List of result variables</returns>
        private List<InitVariable> EulerAsync(List<List<InitVariable>> variablesAtAllStep = null)
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

                // Copying of the initial left variables to the separate list which when is going to "variablesAtAllStep" containier
                List<InitVariable> initLeftVariables = new List<InitVariable>();
                DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, initLeftVariables);

                // Current time is also required to be saved in the intermediate vlues
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

                Parallel.For(0, nextLeftVariables.Count, (i) =>
                {
                    nextLeftVariables[i].Value += this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
                });

                // Saving of all variables at current iteration
                if (variablesAtAllStep != null)
                {
                    List<InitVariable> varsAtIteration = new List<InitVariable>();
                    DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, varsAtIteration);
                    varsAtIteration.Add(new Variable(currentTime));

                    variablesAtAllStep.Add(varsAtIteration);
                }

                // Next variables are becoming the current ones for the next iteration
                DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, currentLeftVariables);

            } while (currentTime.Value < this.TEnd);

            List<InitVariable> result = new List<InitVariable>();
            DifferentialEquationSystemHelpers.CopyVariables(currentLeftVariables, result);
            return result;
        }
    }
}
