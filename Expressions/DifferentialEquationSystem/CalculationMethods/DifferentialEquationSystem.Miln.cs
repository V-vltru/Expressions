namespace DifferentialEquationSystem
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Expressions.Models;

    public partial class DifferentialEquationSystem
    {
        /// <summary>
        /// Method calculates a differential equation system with Miln method
        /// </summary>
        /// <param name="variablesAtAllStep">Container where the intermediate parameters are supposed to be saved</param>
        /// <returns>List of result variables</returns>
        private List<InitVariable> MilnSync(List<List<InitVariable>> variablesAtAllStep = null)
        {
            #region Calculation preparation

            List<Variable> allVars;
            List<Variable> currentLeftVariables = new List<Variable>();
            List<Variable> milnPredicted = new List<Variable>();
            List<Variable> nextLeftVariables = new List<Variable>();

            // Copy this.LeftVariables to the current one and to the nex one
            // To leave this.LeftVariables member unchanged (for further calculations)
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, currentLeftVariables);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, milnPredicted);
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
                this.TimeVariable, this.TimeVariable.Value + 3 * this.Tau, this.Tau);
            List<List<InitVariable>> firstVariables = new List<List<InitVariable>>();
            List<InitVariable> bufer;

            differentialEquationSystem.Calculate(CalculationTypeName.Euler, out bufer, firstVariables);

            List<Variable> firstLeftVariables;
            List<Variable> secondLeftVariables;
            List<Variable> thirdLeftVariables;

            firstLeftVariables = DifferentialEquationSystemHelpers.ConvertInitVariablesToVariables(firstVariables[1]);
            secondLeftVariables = DifferentialEquationSystemHelpers.ConvertInitVariablesToVariables(firstVariables[2]);
            thirdLeftVariables = DifferentialEquationSystemHelpers.ConvertInitVariablesToVariables(firstVariables[3]);

            firstLeftVariables.RemoveAt(firstLeftVariables.Count - 1);
            secondLeftVariables.RemoveAt(secondLeftVariables.Count - 1);
            thirdLeftVariables.RemoveAt(thirdLeftVariables.Count - 1);

            if (variablesAtAllStep != null)
            {
                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, firstLeftVariables, new Variable(currentTime.Name, currentTime.Value + this.Tau));
                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, secondLeftVariables, new Variable(currentTime.Name, currentTime.Value + 2 * this.Tau));
                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, thirdLeftVariables, new Variable(currentTime.Name, currentTime.Value + 3 * this.Tau));
            }

            #endregion

            double[,] Q = new double[4, this.ExpressionSystem.Count];
            double[,] prevValues = new double[4, this.ExpressionSystem.Count];

            allVars = DifferentialEquationSystemHelpers.CollectVariables(currentLeftVariables, this.Constants, currentTime);
            for (int i = 0; i < this.ExpressionSystem.Count; i++)
            {
                Q[0, i] = this.ExpressionSystem[i].GetResultValue(allVars);
                prevValues[0, i] = currentLeftVariables[i].Value;
            }

            currentTime.Value += this.Tau;
            allVars = DifferentialEquationSystemHelpers.CollectVariables(firstLeftVariables, this.Constants, currentTime);

            for (int i = 0; i < this.ExpressionSystem.Count; i++)
            {
                Q[1, i] = this.ExpressionSystem[i].GetResultValue(allVars);
                prevValues[1, i] = firstLeftVariables[i].Value;
            }

            currentTime.Value += this.Tau;
            allVars = DifferentialEquationSystemHelpers.CollectVariables(secondLeftVariables, this.Constants, currentTime);

            for (int i = 0; i < this.ExpressionSystem.Count; i++)
            {
                Q[2, i] = this.ExpressionSystem[i].GetResultValue(allVars);
                prevValues[2, i] = secondLeftVariables[i].Value;
            }

            currentTime.Value += this.Tau;
            allVars = DifferentialEquationSystemHelpers.CollectVariables(thirdLeftVariables, this.Constants, currentTime);
            for (int i = 0; i < this.ExpressionSystem.Count; i++)
            {
                Q[3, i] = this.ExpressionSystem[i].GetResultValue(allVars);
                prevValues[3, i] = thirdLeftVariables[i].Value;
            }

            DifferentialEquationSystemHelpers.CopyVariables(thirdLeftVariables, currentLeftVariables);

            do
            {
                for (int i = 0; i < milnPredicted.Count; i++)
                {
                    milnPredicted[i].Value = prevValues[0, i] + 4 * this.Tau / 3 * (2 * Q[1, i] - Q[2, i] + 2 * Q[3, i]);
                }

                double[] predictedValues = new double[milnPredicted.Count];
                allVars = DifferentialEquationSystemHelpers.CollectVariables(milnPredicted, this.Constants, new Variable(currentTime.Name, currentTime.Value + this.Tau));
                for(int i = 0; i < predictedValues.Length; i++)
                {
                    predictedValues[i] = this.ExpressionSystem[i].GetResultValue(allVars);
                }

                for (int i = 0; i < nextLeftVariables.Count; i++)
                {
                    nextLeftVariables[i].Value = prevValues[2, i] + this.Tau / 3 * (Q[2, i] + 4 * this.ExpressionSystem[i].GetResultValue(DifferentialEquationSystemHelpers.CollectVariables(currentLeftVariables, this.Constants, currentTime)) + predictedValues[i]);
                }

                allVars = DifferentialEquationSystemHelpers.CollectVariables(nextLeftVariables, this.Constants, new Variable(currentTime.Name, currentTime.Value + this.Tau));

                for (int i = 0; i < nextLeftVariables.Count; i++)
                {
                    Q[0, i] = Q[1, i];
                    Q[1, i] = Q[2, i];
                    Q[2, i] = Q[3, i];
                    Q[3, i] = this.ExpressionSystem[i].GetResultValue(allVars);

                    prevValues[0, i] = prevValues[1, i];
                    prevValues[1, i] = prevValues[2, i];
                    prevValues[2, i] = prevValues[3, i];
                    prevValues[3, i] = nextLeftVariables[i].Value;
                }

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

        /// <summary>
        /// Method calculates a differential equation system with Miln method
        /// </summary>
        /// <param name="variablesAtAllStep">Container where the intermediate parameters are supposed to be saved</param>
        /// <returns>List of result variables</returns>
        private List<InitVariable> MilnAsync(List<List<InitVariable>> variablesAtAllStep = null)
        {
            #region Calculation preparation

            List<Variable> allVars;
            List<Variable> currentLeftVariables = new List<Variable>();
            List<Variable> milnPredicted = new List<Variable>();
            List<Variable> nextLeftVariables = new List<Variable>();

            // Copy this.LeftVariables to the current one and to the nex one
            // To leave this.LeftVariables member unchanged (for further calculations)
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, currentLeftVariables);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, milnPredicted);
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
                this.TimeVariable, this.TimeVariable.Value + 3 * this.Tau, this.Tau);
            List<List<InitVariable>> firstVariables = new List<List<InitVariable>>();
            List<InitVariable> bufer;

            differentialEquationSystem.Calculate(CalculationTypeName.Euler, out bufer, firstVariables);

            List<Variable> firstLeftVariables;
            List<Variable> secondLeftVariables;
            List<Variable> thirdLeftVariables;

            firstLeftVariables = DifferentialEquationSystemHelpers.ConvertInitVariablesToVariables(firstVariables[1]);
            secondLeftVariables = DifferentialEquationSystemHelpers.ConvertInitVariablesToVariables(firstVariables[2]);
            thirdLeftVariables = DifferentialEquationSystemHelpers.ConvertInitVariablesToVariables(firstVariables[3]);

            firstLeftVariables.RemoveAt(firstLeftVariables.Count - 1);
            secondLeftVariables.RemoveAt(secondLeftVariables.Count - 1);
            thirdLeftVariables.RemoveAt(thirdLeftVariables.Count - 1);

            if (variablesAtAllStep != null)
            {
                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, firstLeftVariables, new Variable(currentTime.Name, currentTime.Value + this.Tau));
                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, secondLeftVariables, new Variable(currentTime.Name, currentTime.Value + 2 * this.Tau));
                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, thirdLeftVariables, new Variable(currentTime.Name, currentTime.Value + 3 * this.Tau));
            }

            #endregion

            double[,] Q = new double[4, this.ExpressionSystem.Count];
            double[,] prevValues = new double[4, this.ExpressionSystem.Count];

            allVars = DifferentialEquationSystemHelpers.CollectVariables(currentLeftVariables, this.Constants, currentTime);
            Parallel.For(0, this.ExpressionSystem.Count, (i) => 
            {
                Q[0, i] = this.ExpressionSystem[i].GetResultValue(allVars);
                prevValues[0, i] = currentLeftVariables[i].Value;
            });

            currentTime.Value += this.Tau;
            allVars = DifferentialEquationSystemHelpers.CollectVariables(firstLeftVariables, this.Constants, currentTime);

            Parallel.For(0, this.ExpressionSystem.Count, (i) => 
            {
                Q[1, i] = this.ExpressionSystem[i].GetResultValue(allVars);
                prevValues[1, i] = firstLeftVariables[i].Value;
            });            

            currentTime.Value += this.Tau;
            allVars = DifferentialEquationSystemHelpers.CollectVariables(secondLeftVariables, this.Constants, currentTime);

            Parallel.For(0, this.ExpressionSystem.Count, (i) => 
            {
                Q[2, i] = this.ExpressionSystem[i].GetResultValue(allVars);
                prevValues[2, i] = secondLeftVariables[i].Value;
            });
            
            currentTime.Value += this.Tau;
            allVars = DifferentialEquationSystemHelpers.CollectVariables(thirdLeftVariables, this.Constants, currentTime);

            Parallel.For(0, this.ExpressionSystem.Count, (i) => 
            {
                Q[3, i] = this.ExpressionSystem[i].GetResultValue(allVars);
                prevValues[3, i] = thirdLeftVariables[i].Value;
            });

            DifferentialEquationSystemHelpers.CopyVariables(thirdLeftVariables, currentLeftVariables);

            do
            {
                Parallel.For(0, milnPredicted.Count, (i) => 
                {
                    milnPredicted[i].Value = prevValues[0, i] + 4 * this.Tau / 3 * (2 * Q[1, i] - Q[2, i] + 2 * Q[3, i]);
                });
                
                double[] predictedValues = new double[milnPredicted.Count];
                allVars = DifferentialEquationSystemHelpers.CollectVariables(milnPredicted, this.Constants, new Variable(currentTime.Name, currentTime.Value + this.Tau));

                Parallel.For(0, predictedValues.Length, (i) => 
                {
                    predictedValues[i] = this.ExpressionSystem[i].GetResultValue(allVars);
                });

                Parallel.For(0, nextLeftVariables.Count, (i) => 
                {
                    nextLeftVariables[i].Value = prevValues[2, i] + this.Tau / 3 * (Q[2, i] + 4 * this.ExpressionSystem[i].GetResultValue(DifferentialEquationSystemHelpers.CollectVariables(currentLeftVariables, this.Constants, currentTime)) + predictedValues[i]);
                });
                
                allVars = DifferentialEquationSystemHelpers.CollectVariables(nextLeftVariables, this.Constants, new Variable(currentTime.Name, currentTime.Value + this.Tau));

                Parallel.For(0, nextLeftVariables.Count, (i) => 
                {
                    Q[0, i] = Q[1, i];
                    Q[1, i] = Q[2, i];
                    Q[2, i] = Q[3, i];
                    Q[3, i] = this.ExpressionSystem[i].GetResultValue(allVars);

                    prevValues[0, i] = prevValues[1, i];
                    prevValues[1, i] = prevValues[2, i];
                    prevValues[2, i] = prevValues[3, i];
                    prevValues[3, i] = nextLeftVariables[i].Value;
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
