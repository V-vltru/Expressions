using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expressions;
using Expressions.Models;

namespace Integral
{
    public partial class Integral
    {
        public Expression Integrand { get; set; }

        public double StartValue { get; set; }

        public double EndValue { get; set; }

        public int IterationsNumber { get; set; }

        public IntegralVariable Variable { get; set; }

        public Integral()
        {
        }

        public Integral(string integrandExpression, double startValue, double endValue, int iterations, string parameterName)
        {
            this.Variable = new Variable(parameterName, 0.0);
            this.Integrand = new Expression(integrandExpression, new List<Variable>() { this.Variable });

            this.StartValue = startValue;
            this.EndValue = endValue;
            this.IterationsNumber = iterations;
        }

        public double Calculate(CalculationType calculationType)
        {
            Func<Expression, double, double, int, string, double> method = this.GetMethod(calculationType);

            return method(this.Integrand, this.StartValue, this.EndValue, this.IterationsNumber, this.Variable.Name);
        }

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
