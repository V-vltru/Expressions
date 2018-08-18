namespace DifferentialEquationSystem
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Expressions;
    using Expressions.Models;    

    public partial class DifferentialEquationSystem
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DifferentialEquationSystem()
        { }

        /// <summary>
        /// Sets the initial parameters of the DifferentialEquationSystem class.
        /// </summary>
        /// <param name="expressions">List of expressions</param>
        /// <param name="leftVariables">List of left variables</param>
        /// <param name="constants">List of constants</param>
        /// <param name="timeVariable">Start time (presents in the expressions)</param>
        /// <param name="tEnd">End time</param>
        /// <param name="tau">Calculation step</param>
        public DifferentialEquationSystem(List<string> expressions, List<InitVariable> leftVariables,
            List<InitVariable> constants, InitVariable timeVariable, double tEnd, double tau)
        {
            // Setting up of variables and constants
            if (leftVariables != null)
            {
                this.LeftVariables = DifferentialEquationSystem.ConvertInitVariablesToVariables(leftVariables);
            }

            if (constants != null)
            {
                this.Constants = DifferentialEquationSystem.ConvertInitVariablesToVariables(constants);
            }

            if (timeVariable != null)
            {
                this.TimeVariable = timeVariable;
            }

            // Setting up of all variables
            List<Variable> allVariables = new List<Variable>();
            if (this.LeftVariables != null)
            {
                allVariables.AddRange(this.LeftVariables);
            }

            if (this.Constants != null && this.Constants.Count > 0)
            {
                allVariables.AddRange(this.Constants);
            }

            if (this.TimeVariable != null)
            {
                allVariables.Add(this.TimeVariable);
            }

            // Setting up of all expressions
            if (expressions == null || expressions.Count == 0)
            {
                throw new ArgumentException("Container 'expressions' of the constructor cannot be null or empty! Nothing in the differential equation system.");
            }
            else
            {
                this.Expressions = expressions;
                List<Expression> expressionSystem = new List<Expression>();
                foreach (string expression in expressions)
                {
                    expressionSystem.Add(new Expression(expression, allVariables));
                }

                this.ExpressionSystem = expressionSystem;
            }

            this.TEnd = tEnd;
            this.Tau = tau;

            DifferentialEquationSystem.CheckVariables(this.ExpressionSystem, this.LeftVariables, this.TimeVariable, this.Tau, this.TEnd);
        }

        /// <summary>
        /// Sets the initial parameters of the DifferentialEquationSystem class.
        /// </summary>
        /// <param name="expressionSystem">List of expressions</param>
        /// <param name="leftVariables">List of left variables</param>
        /// <param name="constants">List of constants</param>
        /// <param name="timeVariable">Start time (presents in the expressions)</param>
        /// <param name="tEnd">End time</param>
        /// <param name="tau">Calculation step</param>
        private DifferentialEquationSystem(List<Expression> expressionSystem, List<Variable> leftVariables,
            List<Variable> constants, Variable timeVariable, double tEnd, double tau)
        {
            this.ExpressionSystem = expressionSystem;
            this.LeftVariables = leftVariables;
            this.Constants = constants;
            this.TimeVariable = timeVariable;
            this.TEnd = tEnd;
            this.Tau = tau;
        }

        /// <summary>
        /// Sets the initial parameters of the DifferentialEquationSystem class.
        /// </summary>
        /// <param name="differentialEquationSystem">Input instance</param>
        private DifferentialEquationSystem(DifferentialEquationSystem differentialEquationSystem):
            this(differentialEquationSystem.ExpressionSystem, differentialEquationSystem.LeftVariables, differentialEquationSystem.Constants,
                differentialEquationSystem.TimeVariable, differentialEquationSystem.TEnd, differentialEquationSystem.Tau)
        {
        }

        /// <summary>
        /// Gets or sets the list of the constant variables in the right part
        /// </summary>
        public List<Variable> Constants { get; set; }

        /// <summary>
        /// Gets or sets the end time of the differental equation system calculation
        /// </summary>
        public double TEnd { get; set; }

        /// <summary>
        /// Gets or sets the calculation step
        /// </summary>
        public double Tau { get; set; }

        /// <summary>
        /// Gets or sets the list of Expressions
        /// </summary>
        public List<Expression> ExpressionSystem { get; set; }

        /// <summary>
        /// Gets or sets the list of left variables, presented in the differential equation system
        /// </summary>
        public List<Variable> LeftVariables { get; set; }

        /// <summary>
        /// Gets or sets the time parameter if it exists in at least one differential equation
        /// </summary>
        public Variable TimeVariable { get; set; }

        /// <summary>
        /// Gets or sets a list of expressions in the right part of the system.
        /// </summary>
        public List<string> Expressions;

        /// <summary>
        /// Main method which performs a calculation
        /// </summary>
        /// <param name="calculationType">Name of the calculation method</param>
        /// <param name="results">Containier where result variables are supposed to be saved</param>
        /// <param name="variablesAtAllStep">Container of variables at each calculation step</param>
        /// <param name="async">Flag which specifies if it is calculated in parallel mode</param>
        /// <returns>Calculation time</returns>
        public double Calculate(CalculationTypeName calculationType, out List<InitVariable> results, List<List<InitVariable>> variablesAtAllStep = null)
        {
            // Checking the correctness of input variables
            DifferentialEquationSystem.CheckVariables(this.ExpressionSystem, this.LeftVariables, this.TimeVariable, this.Tau, this.TEnd);

            Func<List<List<InitVariable>>, List<InitVariable>> F = this.DefineSuitableMethod(calculationType);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            results = F(variablesAtAllStep);
            stopwatch.Stop();

            return stopwatch.ElapsedMilliseconds / 1000.0;
        }

        /// <summary>
        /// Method which calculates a differential equation system with several specified methods.
        /// </summary>
        /// <param name="calculationTypes">List of calculation types which the system is supposed to be calculated with</param>
        /// <param name="results">Container where results will be saved</param>
        /// <param name="variablesAtAllSteps">Container of variables at each calculation step for all methods</param>
        /// <param name="async">Flag which specifies if it is calculated in parallel mode</param>
        /// <returns>Containier with calculation time</returns>
        public Dictionary<CalculationTypeName, double> CalculateWithGroupOfMethodsSync(List<CalculationTypeName> calculationTypes, out Dictionary<CalculationTypeName,
            List<InitVariable>> results, Dictionary<CalculationTypeName, List<List<InitVariable>>> variablesAtAllSteps = null)
        {
            results = new Dictionary<CalculationTypeName, List<InitVariable>>();
            Dictionary<CalculationTypeName, double> timeResults = new Dictionary<CalculationTypeName, double>();
            
            // Checking the correctness of input variables
            DifferentialEquationSystem.CheckVariables(this.ExpressionSystem, this.LeftVariables, this.TimeVariable, this.Tau, this.TEnd);

            List<List<InitVariable>> variablesForEachMethod = null; 
            if (variablesAtAllSteps != null)
            {
                variablesForEachMethod = new List<List<InitVariable>>();
            }

            foreach(CalculationTypeName calculationType in calculationTypes)
            {
                Func<List<List<InitVariable>>, List<InitVariable>> F = this.DefineSuitableMethod(calculationType);

                List<InitVariable> methodResults;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                methodResults = F(variablesForEachMethod);
                stopwatch.Stop();

                results.Add(calculationType, methodResults);
                timeResults.Add(calculationType, stopwatch.ElapsedMilliseconds / 1000.0);

                if (variablesAtAllSteps != null)
                {
                    variablesAtAllSteps.Add(calculationType, variablesForEachMethod);
                }
            }

            return timeResults;
        }

        /// <summary>
        /// Method which calculates a differential equation system with several specified methods in async mode
        /// </summary>
        /// <param name="calculationTypes">List of calculation types which the system is supposed to be calculated with</param>
        /// <param name="results">Container where results will be saved</param>
        /// <param name="variablesAtAllSteps">Container of variables at each calculation step for all methods</param>
        /// <param name="async">Flag which specifies if it is calculated in parallel mode</param>
        /// <returns>Containier with calculation time</returns>
        public async Task<Dictionary<CalculationTypeName, double>> CalculatewithGroupOfMethodsAsync(List<CalculationTypeName> calculationTypes, Dictionary<CalculationTypeName,
            List<InitVariable>> results, Dictionary<CalculationTypeName, List<List<InitVariable>>> variablesAtAllSteps = null)
        {
            var timeResults = new Dictionary<CalculationTypeName, double>();

            var calcTimes = new ConcurrentDictionary<CalculationTypeName, double>();
            var calcResults = new ConcurrentDictionary<CalculationTypeName, List<InitVariable>>();

            ConcurrentDictionary<CalculationTypeName, List<List<InitVariable>>> calcVariablesAtAllSteps = null;
            if (variablesAtAllSteps != null)
            {
                calcVariablesAtAllSteps = new ConcurrentDictionary<CalculationTypeName, List<List<InitVariable>>>();
            }

            calcTimes = await this.CalculateForMethodGroupAsync(calculationTypes, calcResults, calcVariablesAtAllSteps);

            timeResults = new Dictionary<CalculationTypeName, double>(calcTimes);

            foreach(var item in calcResults)
            {
                results.Add(item.Key, item.Value);
            }

            foreach (var item in calcVariablesAtAllSteps)
            {
                variablesAtAllSteps.Add(item.Key, item.Value);
            }

            return timeResults;
        }

        private async Task<ConcurrentDictionary<CalculationTypeName, double>> CalculateForMethodGroupAsync(List<CalculationTypeName> calculationTypes, ConcurrentDictionary<CalculationTypeName,
            List<InitVariable>> results, ConcurrentDictionary<CalculationTypeName, List<List<InitVariable>>> variablesAtAllSteps = null, bool async = false)
        {
            ConcurrentDictionary<CalculationTypeName, double> timeResults = new ConcurrentDictionary<CalculationTypeName, double>();

            // Checking the correctness of input variables
            DifferentialEquationSystem.CheckVariables(this.ExpressionSystem, this.LeftVariables, this.TimeVariable, this.Tau, this.TEnd);

            List<Task> calculationTasks = new List<Task>();

            foreach (CalculationTypeName calculationType in calculationTypes)
            {
                Func<List<List<InitVariable>>, List<InitVariable>> F = this.DefineSuitableMethod(calculationType);

                Task calculationTask = new Task(() =>
                {
                    Stopwatch stopwatch = new Stopwatch();
                    List<InitVariable> localResult;
                    List<List<InitVariable>> variablesAtAllStepsForMethod = new List<List<InitVariable>>();

                    stopwatch.Start();
                    localResult = F(variablesAtAllStepsForMethod);
                    stopwatch.Stop();

                    results.TryAdd(calculationType, localResult);
                    timeResults.TryAdd(calculationType, stopwatch.ElapsedMilliseconds / 1000.0);

                    if (variablesAtAllSteps != null)
                    {
                        variablesAtAllSteps.TryAdd(calculationType, variablesAtAllStepsForMethod);
                    }
                });

                calculationTask.Start();
                calculationTasks.Add(calculationTask);
            }

            await Task.WhenAll(calculationTasks);

            return timeResults;
        }
    }
}
