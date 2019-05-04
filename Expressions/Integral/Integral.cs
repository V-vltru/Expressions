namespace Integral
{
    using System;
    using System.Collections.Generic;
    using Expressions;
    using Expressions.Models;

    public partial class Integral
    {
        /// <summary>
        /// Gets or sets the integrand expression.
        /// </summary>
        public Expression Integrand { get; set; }
        
        /// <summary>
        /// Gets or sets the start value of a definite integral.
        /// </summary>
        public double StartValue { get; set; }

        /// <summary>
        /// Gets or sets the end value of a definite integral.
        /// </summary>
        public double EndValue { get; set; }

        /// <summary>
        /// Gets ot sets the amount of iterations for integrtal calculating.
        /// </summary>
        public int IterationsNumber { get; set; }

        /// <summary>
        /// A variable by which the integral is calculated.
        /// </summary>
        public IntegralVariable Variable { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Integral" /> class.
        /// Default constructor.
        /// </summary>
        public Integral()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Integral" /> class.
        /// </summary>
        /// <param name="integrandExpression">Integrand expression of the integral.</param>
        /// <param name="startValue">Start value of the definite integral.</param>
        /// <param name="endValue">End value of the definite integral.</param>
        /// <param name="iterations">Amount of iterations.</param>
        /// <param name="parameterName">Name of the variable by which the integral is calculated.</param>
        public Integral(string integrandExpression, double startValue, double endValue, int iterations, string parameterName)
        {
            this.Variable = new Variable(parameterName, 0.0);
            this.Integrand = new Expression(integrandExpression, new List<Variable>() { this.Variable });

            this.StartValue = startValue;
            this.EndValue = endValue;
            this.IterationsNumber = iterations;
        }

        /// <summary>
        /// Method ia used to calculate a definite integral.
        /// </summary>
        /// <param name="calculationType">Calculation type.</param>
        /// <returns>The result of an integral calculation.</returns>
        public double Calculate(CalculationType calculationType)
        {
            Func<Expression, double, double, int, string, double> method = this.GetMethod(calculationType);

            return method(this.Integrand, this.StartValue, this.EndValue, this.IterationsNumber, this.Variable.Name);
        }

        /// <summary>
        /// Method is used to choose a correct calculation method by the input calculation type.
        /// </summary>
        /// <param name="calculationType">Calculation type.</param>
        /// <returns>The appropriate calculation method.</returns>
        private Func<Expression, double, double, int, string, double> GetMethod(CalculationType calculationType)
        {
            switch (calculationType)
            {
                case CalculationType.LeftRectangle: { return this.CalculateRectangleLeft; }
                case CalculationType.RightRectangle: { return this.CalculationRectangleRight; }
                case CalculationType.AverageRectangle: { return this.CalcualtionRectangleAverage; }
                case CalculationType.Trapezium: { return this.CalcualtionTrapezium; }
                case CalculationType.Simpson: { return this.CalcualtionSimpson; }

                default: throw new Exception("Couldn't identify the method of integral calculation.");
            }
        }
    }
}
