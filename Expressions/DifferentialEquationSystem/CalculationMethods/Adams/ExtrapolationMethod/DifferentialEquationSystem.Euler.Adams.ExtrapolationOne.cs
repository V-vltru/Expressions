namespace DifferentialEquationSystem
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Expressions.Models;

    public partial class DifferentialEquationSystem
    {
        public List<InitVariable> AdamsExtrapolationOneCalculation(List<List<InitVariable>> variablesAtAllStep = null, bool async = false)
        {
            // Checking the correctness of input variables
            DifferentialEquationSystemHelpers.CheckVariables(this.ExpressionSystem, this.LeftVariables, this.TimeVariable, this.Tau, this.TEnd);

            if (!async)
            {
                return this.AdamsExtrapolationOneSync(variablesAtAllStep);
            }
            else
            {
                return this.AdamsExtrapolationOneAsync(variablesAtAllStep);
            }
        }

        public List<InitVariable> AdamsExtrapolationOneCalculation(out double calculationTime, List<List<InitVariable>> variablesAtAllStep = null, bool async = false)
        {
            Stopwatch stopwatch = new Stopwatch();
            // Checking the correctness of input variables
            DifferentialEquationSystemHelpers.CheckVariables(this.ExpressionSystem, this.LeftVariables, this.TimeVariable, this.Tau, this.TEnd);

            // Start time recording
            stopwatch.Start();

            List<InitVariable> result;
            if (!async)
            {
                result = this.AdamsExtrapolationOneSync(variablesAtAllStep);
            }
            else
            {
                result = this.AdamsExtrapolationOneAsync(variablesAtAllStep);
            }

            // Stop time recording
            stopwatch.Stop();
            calculationTime = stopwatch.ElapsedMilliseconds / 1000.0;

            return result;
        }

        public List<InitVariable> AdamsExtrapolationOneSync(List<List<InitVariable>> variablesAtAllStep = null)
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
            // Varables at "timestart + tau" is supposed to be calculated with other method
            // It was chosen to use Euler method
            // Generated a new instance for its calculation
            DifferentialEquationSystem differentialEquationSystem = new DifferentialEquationSystem
            {
                ExpressionSystem = this.ExpressionSystem,
                LeftVariables = this.LeftVariables,
                Constants = this.Constants,
                TimeVariable = this.TimeVariable,
                TEnd = this.TimeVariable.Value + this.Tau,
                Tau = this.Tau
            };

            // Calculation
            List<Variable> firstLeftVariables = DifferentialEquationSystemHelpers.ConvertInitVariablesToVariables(differentialEquationSystem.EulerCalculation());

            // Save the second variables calculated with Euler method
            if (variablesAtAllStep != null)
            {
                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, firstLeftVariables, new Variable(currentTime.Name, currentTime.Value + this.Tau));
            }
            #endregion

            allVars = DifferentialEquationSystemHelpers.CollectVariables(currentLeftVariables, this.Constants, currentTime);

            double[,] Q = new double[2, this.ExpressionSystem.Count];
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

            do
            {
                for (int i = 0; i < nextLeftVariables.Count; i++)
                {
                    nextLeftVariables[i].Value = currentLeftVariables[i].Value + 0.5 * (3 * Q[1, i] - Q[0, i]);
                }

                allVars = DifferentialEquationSystemHelpers.CollectVariables(nextLeftVariables, this.Constants, new Variable(currentTime.Name, currentTime.Value + this.Tau));

                for (int i = 0; i < nextLeftVariables.Count; i++)
                {
                    Q[0, i] = Q[1, i];
                    Q[1, i] = this.ExpressionSystem[i].GetResultValue(allVars);
                }

                if (variablesAtAllStep != null)
                {
                    DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, nextLeftVariables, new Variable(currentTime.Name, currentTime.Value + this.Tau));
                }

                DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, currentLeftVariables);

                // calculation time incrimentation
                currentTime.Value += this.Tau;
            } while (currentTime.Value < this.TEnd);

            List<InitVariable> result = new List<InitVariable>();
            DifferentialEquationSystemHelpers.CopyVariables(currentLeftVariables, result);
            return result;
        }

        public List<InitVariable> AdamsExtrapolationOneAsync(List<List<InitVariable>> variablesAtAllStep = null)
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
            // Varables at "timestart + tau" is supposed to be calculated with other method
            // It was chosen to use Euler method
            // Generated a new instance for its calculation
            DifferentialEquationSystem differentialEquationSystem = new DifferentialEquationSystem
            {
                ExpressionSystem = this.ExpressionSystem,
                LeftVariables = this.LeftVariables,
                Constants = this.Constants,
                TimeVariable = this.TimeVariable,
                TEnd = this.TimeVariable.Value + this.Tau,
                Tau = this.Tau
            };

            // Calculation
            List<Variable> firstLeftVariables = DifferentialEquationSystemHelpers.ConvertInitVariablesToVariables(differentialEquationSystem.EulerCalculation());

            // Save the second variables calculated with Euler method
            if (variablesAtAllStep != null)
            {
                DifferentialEquationSystemHelpers.SaveLeftVariableToStatistics(variablesAtAllStep, firstLeftVariables, new Variable(currentTime.Name, currentTime.Value + this.Tau));
            }
            #endregion

            allVars = DifferentialEquationSystemHelpers.CollectVariables(currentLeftVariables, this.Constants, currentTime);

            double[,] Q = new double[2, this.ExpressionSystem.Count];
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

            do
            {
                for (int i = 0; i < nextLeftVariables.Count; i++)
                {
                    nextLeftVariables[i].Value = currentLeftVariables[i].Value + 0.5 * (3 * Q[1, i] - Q[0, i]);
                }

                allVars = DifferentialEquationSystemHelpers.CollectVariables(nextLeftVariables, this.Constants, new Variable(currentTime.Name, currentTime.Value + this.Tau));

                for (int i = 0; i < nextLeftVariables.Count; i++)
                {
                    Q[0, i] = Q[1, i];
                    Q[1, i] = this.ExpressionSystem[i].GetResultValue(allVars);
                }

                DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, currentLeftVariables);

                // calculation time incrimentation
                currentTime.Value += this.Tau;
            } while (currentTime.Value < this.TEnd);

            List<InitVariable> result = new List<InitVariable>();
            DifferentialEquationSystemHelpers.CopyVariables(currentLeftVariables, result);
            return result;
        }
    }
}
