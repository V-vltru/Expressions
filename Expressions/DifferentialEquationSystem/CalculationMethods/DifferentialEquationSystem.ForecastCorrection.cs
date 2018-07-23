namespace DifferentialEquationSystem
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Expressions.Models;
    
    public partial class DifferentialEquationSystem
    {        
        /// <summary>
        /// Method calculates a differential equation system with Forecast-Correction method
        /// </summary>
        /// <param name="variablesAtAllStep">Container where the intermediate parameters are supposed to be saved</param>
        /// <returns>List of result variables</returns>
        private List<InitVariable> ForecastCorrectionSync(List<List<InitVariable>> variablesAtAllStep = null)
        {
            // Put left variables, constants and time variable in the one containier
            List<Variable> allVars;
            List<Variable> currentLeftVariables = new List<Variable>();
            List<Variable> predictedLeftVariables = new List<Variable>();
            List<Variable> nextLeftVariables = new List<Variable>();

            // Copy this.LeftVariables to the current one and to the nex one
            // To leave this.LeftVariables member unchanged (for further calculations)
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, currentLeftVariables);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, predictedLeftVariables);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, nextLeftVariables);

            // Setting of current time (to leave this.TimeVariable unchanged)
            Variable currentTime = new Variable(this.TimeVariable);

            if (variablesAtAllStep != null)
            {
                // This is the first record for intermediate calculations containier
                // It has to be clear
                variablesAtAllStep.Clear();

                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, this.LeftVariables, currentTime);
            }

            do
            {
                // Combinig of variables to calculate the next step results
                allVars = DifferentialEquationSystemHelpers.CollectVariables(currentLeftVariables, this.Constants, currentTime);

                // Calculation of functions values for the next steps
                List<double> FCurrent = new List<double>();
                for (int i = 0; i < currentLeftVariables.Count; i++)
                {
                    FCurrent.Add(this.ExpressionSystem[i].GetResultValue(allVars));
                }

                // Calculation of variables for the next steps
                for (int i = 0; i < predictedLeftVariables.Count; i++)
                {
                    predictedLeftVariables[i].Value = currentLeftVariables[i].Value + this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
                }

                // Combinig of variables with ones taken from the previous iteration (variables for the next step)
                allVars = DifferentialEquationSystemHelpers.CollectVariables(predictedLeftVariables, this.Constants, 
                    new Variable(currentTime.Name, currentTime.Value + this.Tau));

                // Calculation of predicted variables 
                List<double> FPredicted = new List<double>();
                for (int i = 0; i < predictedLeftVariables.Count; i++)
                {
                    FPredicted.Add(this.ExpressionSystem[i].GetResultValue(allVars));
                }

                // Calculation of the next variables
                for(int i = 0; i < predictedLeftVariables.Count; i++)
                {
                    nextLeftVariables[i].Value = currentLeftVariables[i].Value + this.Tau * (FCurrent[i] + FPredicted[i]) / 2;
                }

                // Saving of all variables at current iteration
                if (variablesAtAllStep != null)
                {
                    DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, nextLeftVariables, 
                        new Variable(currentTime.Name, currentTime.Value + this.Tau));
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
        /// Method calculates a differential equation system with Forecast-Correction method
        /// </summary>
        /// <param name="variablesAtAllStep">Container where the intermediate parameters are supposed to be saved</param>
        /// <returns>List of result variables</returns>
        private List<InitVariable> ForecastCorrectionAsync(List<List<InitVariable>> variablesAtAllStep = null)
        {
            // Put left variables, constants and time variable in the one containier
            List<Variable> allVars;
            List<Variable> currentLeftVariables = new List<Variable>();
            List<Variable> predictedLeftVariables = new List<Variable>();
            List<Variable> nextLeftVariables = new List<Variable>();

            // Copy this.LeftVariables to the current one and to the nex one
            // To leave this.LeftVariables member unchanged (for further calculations)
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, currentLeftVariables);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, predictedLeftVariables);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, nextLeftVariables);

            Variable currentTime = new Variable(this.TimeVariable);

            if (variablesAtAllStep != null)
            {
                // This is the first record for intermediate calculations containier
                // It has to be clear
                variablesAtAllStep.Clear();

                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, this.LeftVariables, currentTime);
            }

            do
            {
                // Combinig of variables to calculate the next step results
                allVars = DifferentialEquationSystemHelpers.CollectVariables(currentLeftVariables, this.Constants, currentTime);

                // Calculation of functions values for the next steps
                double[] FCurrent = new double[this.ExpressionSystem.Count];
                Parallel.For(0, currentLeftVariables.Count, (i) => 
                {
                    FCurrent[i] = this.ExpressionSystem[i].GetResultValue(allVars);
                });

                // Calculation of variables for the next steps
                Parallel.For(0, predictedLeftVariables.Count, (i) =>
                {
                    predictedLeftVariables[i].Value = currentLeftVariables[i].Value + this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
                });

                // Combinig of variables with ones taken from the previous iteration (variables for the next step)
                allVars = DifferentialEquationSystemHelpers.CollectVariables(predictedLeftVariables, this.Constants,
                    new Variable(currentTime.Name, currentTime.Value + this.Tau));

                // Calculation of the next variables
                double[] FPredicted = new double[this.ExpressionSystem.Count];
                Parallel.For(0, predictedLeftVariables.Count, (i) => 
                {
                    FPredicted[i] = this.ExpressionSystem[i].GetResultValue(allVars);
                });

                // Calculation of the next variables
                Parallel.For(0, predictedLeftVariables.Count, (i) => 
                {
                    nextLeftVariables[i].Value = currentLeftVariables[i].Value + this.Tau * (FCurrent[i] + FPredicted[i]) / 2;
                });


                if (variablesAtAllStep != null)
                {
                    DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, nextLeftVariables,
                        new Variable(currentTime.Name, currentTime.Value + this.Tau));
                }

                DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, currentLeftVariables);

                currentTime.Value += this.Tau;
            } while (currentTime.Value < this.TEnd);

            List<InitVariable> result = new List<InitVariable>();
            DifferentialEquationSystemHelpers.CopyVariables(currentLeftVariables, result);
            return result;
        }
    }
}
