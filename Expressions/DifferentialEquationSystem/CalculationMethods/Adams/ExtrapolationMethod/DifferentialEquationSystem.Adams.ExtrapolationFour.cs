namespace DifferentialEquationSystem
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Expressions.Models;

    public partial class DifferentialEquationSystem
    {
        /// <summary>
        /// Method calculates a differential equation system with Extrapolation Adams Four method
        /// </summary>
        /// <param name="variablesAtAllStep">Container where the intermediate parameters are supposed to be saved</param>
        /// <returns>List of result variables</returns>
        private List<DEVariable> AdamsExtrapolationFourSync(List<List<DEVariable>> variablesAtAllStep = null)
        {
            #region Calculation preparation

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

            #endregion

            #region First variables

            DifferentialEquationSystem differentialEquationSystem = new DifferentialEquationSystem(this.ExpressionSystem, this.LeftVariables, this.Constants,
                this.TimeVariable, this.TimeVariable.Value + 4 * this.Tau, this.Tau);
            List<List<DEVariable>> firstVariables = new List<List<DEVariable>>();

            differentialEquationSystem.Calculate(CalculationTypeName.Euler, out List<DEVariable> bufer, firstVariables);

            List<Variable> firstLeftVariables;
            List<Variable> secondLeftVariables;
            List<Variable> thirdLeftVariables;
            List<Variable> forthLeftVariables;

            firstLeftVariables = DifferentialEquationSystemHelpers.ConvertDEVariablesToVariables(firstVariables[1]);
            secondLeftVariables = DifferentialEquationSystemHelpers.ConvertDEVariablesToVariables(firstVariables[2]);
            thirdLeftVariables = DifferentialEquationSystemHelpers.ConvertDEVariablesToVariables(firstVariables[3]);
            forthLeftVariables = DifferentialEquationSystemHelpers.ConvertDEVariablesToVariables(firstVariables[4]);

            firstLeftVariables.RemoveAt(firstLeftVariables.Count - 1);
            secondLeftVariables.RemoveAt(secondLeftVariables.Count - 1);
            thirdLeftVariables.RemoveAt(thirdLeftVariables.Count - 1);
            forthLeftVariables.RemoveAt(forthLeftVariables.Count - 1);

            if (variablesAtAllStep != null)
            {
                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, firstLeftVariables, new Variable(currentTime.Name, currentTime.Value + this.Tau));
                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, secondLeftVariables, new Variable(currentTime.Name, currentTime.Value + 2 * this.Tau));
                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, thirdLeftVariables, new Variable(currentTime.Name, currentTime.Value + 3 * this.Tau));
                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, forthLeftVariables, new Variable(currentTime.Name, currentTime.Value + 4 * this.Tau));
            }

            #endregion

            double[,] Q = new double[5, this.ExpressionSystem.Count];

            allVars = DifferentialEquationSystemHelpers.CollectVariables(currentLeftVariables, this.Constants, currentTime);
            for (int i = 0; i < this.ExpressionSystem.Count; i++)
            {
                Q[0, i] = this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
            }

            currentTime.Value += this.Tau;
            allVars = DifferentialEquationSystemHelpers.CollectVariables(firstLeftVariables, this.Constants, currentTime);

            for (int i = 0; i < this.ExpressionSystem.Count; i++)
            {
                Q[1, i] = this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
            }

            currentTime.Value += this.Tau;
            allVars = DifferentialEquationSystemHelpers.CollectVariables(secondLeftVariables, this.Constants, currentTime);

            for (int i = 0; i < this.ExpressionSystem.Count; i++)
            {
                Q[2, i] = this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
            }

            currentTime.Value += this.Tau;
            allVars = DifferentialEquationSystemHelpers.CollectVariables(thirdLeftVariables, this.Constants, currentTime);
            for (int i = 0; i < this.ExpressionSystem.Count; i++)
            {
                Q[3, i] = this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
            }

            currentTime.Value += this.Tau;
            allVars = DifferentialEquationSystemHelpers.CollectVariables(forthLeftVariables, this.Constants, currentTime);
            for (int i = 0; i < this.ExpressionSystem.Count; i++)
            {
                Q[4, i] = this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
            }

            DifferentialEquationSystemHelpers.CopyVariables(forthLeftVariables, currentLeftVariables);

            do
            {
                for (int i = 0; i < nextLeftVariables.Count; i++)
                {
                    nextLeftVariables[i].Value = currentLeftVariables[i].Value + 1.0 / 720 * (1901 * Q[4, i] - 2774 * Q[3, i] + 2616 * Q[2, i] - 1274 * Q[1, i] + 251 * Q[0, i]);
                }

                allVars = DifferentialEquationSystemHelpers.CollectVariables(nextLeftVariables, this.Constants, new Variable(currentTime.Name, currentTime.Value + this.Tau));

                for (int i = 0; i < nextLeftVariables.Count; i++)
                {
                    Q[0, i] = Q[1, i];
                    Q[1, i] = Q[2, i];
                    Q[2, i] = Q[3, i];
                    Q[3, i] = Q[4, i];
                    Q[4, i] = this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
                }

                if (variablesAtAllStep != null)
                {
                    DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, nextLeftVariables,
                        new Variable(currentTime.Name, currentTime.Value + this.Tau));
                }

                DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, currentLeftVariables);

                currentTime.Value += this.Tau;
            } while (currentTime.Value < this.TEnd);

            List<DEVariable> result = new List<DEVariable>();
            DifferentialEquationSystemHelpers.CopyVariables(currentLeftVariables, result);
            return result;
        }

        /// <summary>
        /// Method calculates a differential equation system with Extrapolation Adams Four method
        /// </summary>
        /// <param name="variablesAtAllStep">Container where the intermediate parameters are supposed to be saved</param>
        /// <returns>List of result variables</returns>
        private List<DEVariable> AdamsExtrapolationFourAsync(List<List<DEVariable>> variablesAtAllStep = null)
        {
            #region Calculation preparation

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

            #endregion

            #region First variables

            DifferentialEquationSystem differentialEquationSystem = new DifferentialEquationSystem(this.ExpressionSystem, this.LeftVariables, this.Constants,
                this.TimeVariable, this.TimeVariable.Value + 4 * this.Tau, this.Tau);
            List<List<DEVariable>> firstVariables = new List<List<DEVariable>>();

            differentialEquationSystem.Calculate(CalculationTypeName.Euler, out List<DEVariable> bufer, firstVariables);

            List<Variable> firstLeftVariables;
            List<Variable> secondLeftVariables;
            List<Variable> thirdLeftVariables;
            List<Variable> forthLeftVariables;

            firstLeftVariables = DifferentialEquationSystemHelpers.ConvertDEVariablesToVariables(firstVariables[1]);
            secondLeftVariables = DifferentialEquationSystemHelpers.ConvertDEVariablesToVariables(firstVariables[2]);
            thirdLeftVariables = DifferentialEquationSystemHelpers.ConvertDEVariablesToVariables(firstVariables[3]);
            forthLeftVariables = DifferentialEquationSystemHelpers.ConvertDEVariablesToVariables(firstVariables[4]);

            firstLeftVariables.RemoveAt(firstLeftVariables.Count - 1);
            secondLeftVariables.RemoveAt(secondLeftVariables.Count - 1);
            thirdLeftVariables.RemoveAt(thirdLeftVariables.Count - 1);
            forthLeftVariables.RemoveAt(forthLeftVariables.Count - 1);

            if (variablesAtAllStep != null)
            {
                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, firstLeftVariables, new Variable(currentTime.Name, currentTime.Value + this.Tau));
                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, secondLeftVariables, new Variable(currentTime.Name, currentTime.Value + 2 * this.Tau));
                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, thirdLeftVariables, new Variable(currentTime.Name, currentTime.Value + 3 * this.Tau));
                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, forthLeftVariables, new Variable(currentTime.Name, currentTime.Value + 4 * this.Tau));
            }

            #endregion

            double[,] Q = new double[5, this.ExpressionSystem.Count];

            allVars = DifferentialEquationSystemHelpers.CollectVariables(currentLeftVariables, this.Constants, currentTime);
            Parallel.For(0, this.ExpressionSystem.Count, (i) => 
            {
                Q[0, i] = this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
            });

            currentTime.Value += this.Tau;
            allVars = DifferentialEquationSystemHelpers.CollectVariables(firstLeftVariables, this.Constants, currentTime);

            Parallel.For(0, this.ExpressionSystem.Count, (i) => 
            {
                Q[1, i] = this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
            });            

            currentTime.Value += this.Tau;
            allVars = DifferentialEquationSystemHelpers.CollectVariables(secondLeftVariables, this.Constants, currentTime);

            Parallel.For(0, this.ExpressionSystem.Count, (i) => 
            {
                Q[2, i] = this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
            });

            currentTime.Value += this.Tau;
            allVars = DifferentialEquationSystemHelpers.CollectVariables(thirdLeftVariables, this.Constants, currentTime);

            Parallel.For(0, this.ExpressionSystem.Count, (i) => 
            {
                Q[3, i] = this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
            });

            currentTime.Value += this.Tau;
            allVars = DifferentialEquationSystemHelpers.CollectVariables(forthLeftVariables, this.Constants, currentTime);

            Parallel.For(0, this.ExpressionSystem.Count, (i) => 
            {
                Q[4, i] = this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
            });

            DifferentialEquationSystemHelpers.CopyVariables(forthLeftVariables, currentLeftVariables);

            do
            {
                Parallel.For(0, nextLeftVariables.Count, (i) => 
                {
                    nextLeftVariables[i].Value = currentLeftVariables[i].Value + 1.0 / 720 * (1901 * Q[4, i] - 2774 * Q[3, i] + 2616 * Q[2, i] - 1274 * Q[1, i] + 251 * Q[0, i]);
                });

                allVars = DifferentialEquationSystemHelpers.CollectVariables(nextLeftVariables, this.Constants, new Variable(currentTime.Name, currentTime.Value + this.Tau));

                Parallel.For(0, nextLeftVariables.Count, (i) => 
                {
                    Q[0, i] = Q[1, i];
                    Q[1, i] = Q[2, i];
                    Q[2, i] = Q[3, i];
                    Q[3, i] = Q[4, i];
                    Q[4, i] = this.Tau * this.ExpressionSystem[i].GetResultValue(allVars);
                });                

                if (variablesAtAllStep != null)
                {
                    DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, nextLeftVariables,
                        new Variable(currentTime.Name, currentTime.Value + this.Tau));
                }

                DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, currentLeftVariables);

                currentTime.Value += this.Tau;
            } while (currentTime.Value < this.TEnd);

            List<DEVariable> result = new List<DEVariable>();
            DifferentialEquationSystemHelpers.CopyVariables(currentLeftVariables, result);
            return result;
        }
    }
}
