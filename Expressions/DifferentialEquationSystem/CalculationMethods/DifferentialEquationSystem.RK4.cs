namespace DifferentialEquationSystem
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Expressions.Models;

    public partial class DifferentialEquationSystem
    {
        /// <summary>
        /// Method calculates a differential equation system with RK4 method
        /// </summary>
        /// <param name="variablesAtAllStep">Container where the intermediate parameters are supposed to be saved</param>
        /// <param name="async">Flag which specifies if calculation should be performed in parallel mode</param>
        /// <returns>List of result variables</returns>
        public List<InitVariable> RK4Calculation(List<List<InitVariable>> variablesAtAllStep = null, bool async = false)
        {
            // Checking the correctness of input variables
            DifferentialEquationSystemHelpers.CheckVariables(this.ExpressionSystem, this.LeftVariables, this.TimeVariable, this.Tau, this.TEnd);

            if (!async)
            {
                return this.RK4Sync(variablesAtAllStep);
            }
            else
            {
                return this.RK4Async(variablesAtAllStep);
            }
        }

        /// <summary>
        /// Method calculates a differential equation system with RK4 method
        /// </summary>
        /// <param name="calculationTime">Referenced parameter where calculation time is supposed to be loacted</param>
        /// <param name="variablesAtAllStep">Container where the intermediate parameters are supposed to be saved</param>
        /// <param name="async">Flag which specifies if calculation should be performed in parallel mode</param>
        /// <returns>List of result variables</returns>
        public List<InitVariable> RK4Calculation(out double calculationTime, List<List<InitVariable>> variablesAtAllStep = null, bool async = false)
        {
            Stopwatch stopwatch = new Stopwatch();
            // Checking the correctness of input variables
            DifferentialEquationSystemHelpers.CheckVariables(this.ExpressionSystem, this.LeftVariables, this.TimeVariable, this.Tau, this.TEnd);

            // Start time recording
            stopwatch.Start();

            List<InitVariable> result;
            if (!async)
            {
                result = this.RK4Sync(variablesAtAllStep);
            }
            else
            {
                result = this.RK4Async(variablesAtAllStep);
            }

            // Stop time recording
            stopwatch.Stop();
            calculationTime = stopwatch.ElapsedMilliseconds / 1000.0;

            return result;
        }

        /// <summary>
        /// Method calculates a differential equation system with RK4 method
        /// </summary>
        /// <param name="variablesAtAllStep">Container where the intermediate parameters are supposed to be saved</param>
        /// <returns>List of result variables</returns>
        private List<InitVariable> RK4Sync(List<List<InitVariable>> variablesAtAllStep)
        {
            // Put left variables, constants and time variable in the one containier
            List<Variable> allVars;
            List<Variable> currentLeftVariables = new List<Variable>();
            List<Variable> leftVariablesK1 = new List<Variable>();
            List<Variable> leftVariablesK2 = new List<Variable>();
            List<Variable> leftVariablesK3 = new List<Variable>();
            List<Variable> nextLeftVariables = new List<Variable>();

            // Copy this.LeftVariables to the current one and to the nex one
            // To leave this.LeftVariables member unchanged (for further calculations)
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, currentLeftVariables);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, leftVariablesK1);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, leftVariablesK2);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, leftVariablesK3);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, nextLeftVariables);

            // Setting of current time (to leave this.TimeVariable unchanged)
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
                // Preparation of variables for K1 calculation
                allVars = new List<Variable>();
                allVars.AddRange(currentLeftVariables);
                if (this.Constants != null && this.Constants.Count > 0)
                {
                    allVars.AddRange(this.Constants);
                }

                allVars.Add(currentTime);

                // K1 calculation
                double[] K1 = new double[currentLeftVariables.Count];
                for (int i = 0; i < K1.Length; i++)
                {
                    K1[i] = this.ExpressionSystem[i].GetResultValue(allVars);
                }

                // Preparation of variables for K2 calculation
                allVars.Clear();
                for (int i = 0; i < currentLeftVariables.Count; i++)
                {
                    leftVariablesK1[i].Value = currentLeftVariables[i].Value + this.Tau / 2 * K1[i];
                }

                allVars.AddRange(leftVariablesK1);
                if (this.Constants != null && this.Constants.Count > 0)
                {
                    allVars.AddRange(this.Constants);
                }

                allVars.Add(new Variable(currentTime.Name, currentTime.Value + this.Tau / 2));


                // K2 calculation
                double[] K2 = new double[currentLeftVariables.Count];
                for (int i = 0; i < K2.Length; i++)
                {
                    K2[i] = this.ExpressionSystem[i].GetResultValue(allVars);
                }

                allVars.Clear();
                for (int i = 0; i < currentLeftVariables.Count; i++)
                {
                    leftVariablesK2[i].Value = currentLeftVariables[i].Value + this.Tau / 2 * K2[i];
                }

                allVars.AddRange(leftVariablesK2);
                if (this.Constants != null && this.Constants.Count > 0)
                {
                    allVars.AddRange(this.Constants);
                }

                allVars.Add(new Variable(currentTime.Name, currentTime.Value + this.Tau / 2));


                // K3 calculation
                double[] K3 = new double[currentLeftVariables.Count];
                for (int i = 0; i < K3.Length; i++)
                {
                    K3[i] = this.ExpressionSystem[i].GetResultValue(allVars);
                }

                allVars.Clear();
                for (int i = 0; i < currentLeftVariables.Count; i++)
                {
                    leftVariablesK3[i].Value = currentLeftVariables[i].Value + this.Tau * K3[i];
                }

                allVars.AddRange(leftVariablesK3);
                if (this.Constants != null && this.Constants.Count > 0)
                {
                    allVars.AddRange(this.Constants);
                }

                allVars.Add(new Variable(currentTime.Name, currentTime.Value + this.Tau));

                // K4 calculation
                double[] K4 = new double[currentLeftVariables.Count];
                for (int i = 0; i < K4.Length; i++)
                {
                    K4[i] = this.ExpressionSystem[i].GetResultValue(allVars);
                }

                for (int i = 0; i < currentLeftVariables.Count; i++)
                {
                    nextLeftVariables[i].Value = currentLeftVariables[i].Value + this.Tau / 6 * (K1[i] + 2 * K2[i] + 2 * K3[i] + K4[i]);
                }

                // Saving of all variables at current iteration
                if (variablesAtAllStep != null)
                {
                    List<InitVariable> varsAtIteration = new List<InitVariable>();
                    DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, varsAtIteration);
                    varsAtIteration.Add(new Variable(currentTime.Name, currentTime.Value + this.Tau));

                    variablesAtAllStep.Add(varsAtIteration);
                }

                // Next variables are becoming the current ones for the next iteration
                DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, currentLeftVariables);

                currentTime.Value += this.Tau;
            } while (currentTime.Value < this.TEnd);

            List<InitVariable> result = new List<InitVariable>();
            DifferentialEquationSystemHelpers.CopyVariables(currentLeftVariables, result);
            return result;
        }

        /// <summary>
        /// Method calculates a differential equation system with RK4 method
        /// </summary>
        /// <param name="variablesAtAllStep">Container where the intermediate parameters are supposed to be saved</param>
        /// <returns>List of result variables</returns>
        private List<InitVariable> RK4Async(List<List<InitVariable>> variablesAtAllStep)
        {
            // Put left variables, constants and time variable in the one containier
            List<Variable> allVars;
            List<Variable> currentLeftVariables = new List<Variable>();
            List<Variable> leftVariablesK1 = new List<Variable>();
            List<Variable> leftVariablesK2 = new List<Variable>();
            List<Variable> leftVariablesK3 = new List<Variable>();
            List<Variable> nextLeftVariables = new List<Variable>();

            // Copy this.LeftVariables to the current one and to the nex one
            // To leave this.LeftVariables member unchanged (for further calculations)
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, currentLeftVariables);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, leftVariablesK1);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, leftVariablesK2);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, leftVariablesK3);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, nextLeftVariables);

            // Setting of current time (to leave this.TimeVariable unchanged)
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
                // Preparation of variables for K1 calculation
                allVars = new List<Variable>();
                allVars.AddRange(currentLeftVariables);
                if (this.Constants != null && this.Constants.Count > 0)
                {
                    allVars.AddRange(this.Constants);
                }

                allVars.Add(currentTime);

                // K1 calculation
                double[] K1 = new double[currentLeftVariables.Count];
                Parallel.For(0, K1.Length, (i) => 
                {
                    K1[i] = this.ExpressionSystem[i].GetResultValue(allVars);
                });

                // Preparation of variables for K2 calculation
                allVars.Clear();
                Parallel.For(0, currentLeftVariables.Count, (i) => 
                {
                    leftVariablesK1[i].Value = currentLeftVariables[i].Value + this.Tau / 2 * K1[i];
                });

                allVars.AddRange(leftVariablesK1);
                if (this.Constants != null && this.Constants.Count > 0)
                {
                    allVars.AddRange(this.Constants);
                }

                allVars.Add(new Variable(currentTime.Name, currentTime.Value + this.Tau / 2));


                // K2 calculation
                double[] K2 = new double[currentLeftVariables.Count];
                Parallel.For(0, K2.Length, (i) => 
                {
                    K2[i] = this.ExpressionSystem[i].GetResultValue(allVars);
                });

                allVars.Clear();
                Parallel.For(0, currentLeftVariables.Count, (i) => 
                {
                    leftVariablesK2[i].Value = currentLeftVariables[i].Value + this.Tau / 2 * K2[i];
                });

                allVars.AddRange(leftVariablesK2);
                if (this.Constants != null && this.Constants.Count > 0)
                {
                    allVars.AddRange(this.Constants);
                }

                allVars.Add(new Variable(currentTime.Name, currentTime.Value + this.Tau / 2));


                // K3 calculation
                double[] K3 = new double[currentLeftVariables.Count];
                Parallel.For(0, K3.Length, (i)=>
                {
                    K3[i] = this.ExpressionSystem[i].GetResultValue(allVars);
                });

                allVars.Clear();
                Parallel.For(0, currentLeftVariables.Count, (i) => 
                {
                    leftVariablesK3[i].Value = currentLeftVariables[i].Value + this.Tau * K3[i];
                });

                allVars.AddRange(leftVariablesK3);
                if (this.Constants != null && this.Constants.Count > 0)
                {
                    allVars.AddRange(this.Constants);
                }

                allVars.Add(new Variable(currentTime.Name, currentTime.Value + this.Tau));

                // K4 calculation
                double[] K4 = new double[currentLeftVariables.Count];
                Parallel.For(0, K4.Length, (i) => 
                {
                    K4[i] = this.ExpressionSystem[i].GetResultValue(allVars);
                });

                Parallel.For(0, currentLeftVariables.Count, (i) => 
                {
                    nextLeftVariables[i].Value = currentLeftVariables[i].Value + this.Tau / 6 * (K1[i] + 2 * K2[i] + 2 * K3[i] + K4[i]);
                });

                // Saving of all variables at current iteration
                if (variablesAtAllStep != null)
                {
                    List<InitVariable> varsAtIteration = new List<InitVariable>();
                    DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, varsAtIteration);
                    varsAtIteration.Add(new Variable(currentTime.Name, currentTime.Value + this.Tau));

                    variablesAtAllStep.Add(varsAtIteration);
                }

                // Next variables are becoming the current ones for the next iteration
                DifferentialEquationSystemHelpers.CopyVariables(nextLeftVariables, currentLeftVariables);

                currentTime.Value += this.Tau;
            } while (currentTime.Value < this.TEnd);

            List<InitVariable> result = new List<InitVariable>();
            DifferentialEquationSystemHelpers.CopyVariables(currentLeftVariables, result);
            return result;
        }
    }
}
