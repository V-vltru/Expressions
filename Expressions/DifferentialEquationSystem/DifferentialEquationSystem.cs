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
                this.LeftVariables = DifferentialEquationSystemHelpers.ConvertInitVariablesToVariables(leftVariables);
            }

            if (constants != null)
            {
                this.Constants = DifferentialEquationSystemHelpers.ConvertInitVariablesToVariables(constants);
            }

            if (timeVariable != null)
            {
                this.TimeVariable = timeVariable;
            }

            // Setting up of all variables
            this.AllVariables = new List<Variable>();
            if (this.LeftVariables != null)
            {
                this.AllVariables.AddRange(this.LeftVariables);
            }

            if (this.Constants != null && this.Constants.Count > 0)
            {
                this.AllVariables.AddRange(this.Constants);
            }

            if (this.TimeVariable != null)
            {
                this.AllVariables.Add(this.TimeVariable);
            }

            // Setting up of all expressions
            if (expressions == null || expressions.Count == 0)
            {
                throw new ArgumentException("Container 'expressions' of the constructor cannot be null or empty! Nothing in the differential equation system.");
            }
            else
            {
                List<Expression> expressionSystem = new List<Expression>();
                foreach (string expression in expressions)
                {
                    expressionSystem.Add(new Expression(expression, this.AllVariables));
                }

                this.ExpressionSystem = expressionSystem;
            }

            this.TEnd = tEnd;
            this.Tau = tau;

            DifferentialEquationSystemHelpers.CheckVariables(this.ExpressionSystem, this.LeftVariables, this.TimeVariable, this.Tau, this.TEnd);
        }

        /// <summary>
        /// Gets or sets the list of the constant variables in the right part
        /// </summary>
        private List<Variable> Constants { get; set; }

        /// <summary>
        /// Gets or sets all variables considered in differential equation system
        /// </summary>
        private List<Variable> AllVariables { get; set; }

        /// <summary>
        /// Gets or sets the end time of the differental equation system calculation
        /// </summary>
        private double TEnd { get; set; }

        /// <summary>
        /// Gets or sets the calculation step
        /// </summary>
        private double Tau { get; set; }

        /// <summary>
        /// Gets or sets the list of Expressions
        /// </summary>
        private List<Expression> ExpressionSystem { get; set; }

        /// <summary>
        /// Gets or sets the list of left variables, presented in the differential equation system
        /// </summary>
        private List<Variable> LeftVariables { get; set; }        

        /// <summary>
        /// Gets or sets the time parameter if it exists in at least one differential equation
        /// </summary>
        public Variable TimeVariable { get; set; }
    }
}
