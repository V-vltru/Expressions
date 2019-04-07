namespace Integral
{
    using System;
    using Expressions;
    using Expressions.Models;

    public partial class Integral
    {
        public double CalculateRectangleLeft(Expression integrand, double startValue, double endValue, int numberOfSteps, string variableName)
        {
            double result = 0.0;
            double calculationStep = GetStep(startValue, endValue, numberOfSteps);

            Variable currentVariable = new Variable(variableName, startValue);
            
            for (int i = 0; i < numberOfSteps; i++)
            {
                result += calculationStep * integrand.GetResultValue(currentVariable);
                currentVariable.Value += calculationStep;
            }

            return result;
        }

        public double CalculationRectangleRight(Expression integrand, double startValue, double endValue, int numberOfSteps, string variableName)
        {
            double result = 0.0;
            double calculationStep = GetStep(startValue, endValue, numberOfSteps);

            Variable currentVariable = new Variable(variableName, startValue);

            currentVariable.Value += calculationStep;

            for (int i = 0; i < numberOfSteps; i++)
            {
                result += calculationStep * integrand.GetResultValue(currentVariable);
                currentVariable.Value += calculationStep;
            }

            return result;
        }

        public double CalcualtionRectangleAverage(Expression integrand, double startValue, double endValue, int numberOfSteps, string variableName)
        {
            double result = 0.0;
            double calculationStep = GetStep(startValue, endValue, numberOfSteps);

            Variable currentVariable = new Variable(variableName, startValue);

            for (int i = 0; i < numberOfSteps; i++)
            {
                currentVariable.Value += calculationStep / 2.0;
                result += calculationStep * integrand.GetResultValue(currentVariable);

                currentVariable.Value += calculationStep / 2.0;
            }

            return result;
        }

        public double CalcualtionTrapezium(Expression integrand, double startValue, double endValue, int numberOfSteps, string variableName)
        {
            double result = 0.0;
            double calculationStep = GetStep(startValue, endValue, numberOfSteps);

            Variable currentVariable = new Variable(variableName, startValue + calculationStep);
            Variable prevVariable = new Variable(variableName, startValue);

            for (int i = 0; i < numberOfSteps; i++)
            {
                result += (integrand.GetResultValue(currentVariable) + integrand.GetResultValue(prevVariable)) / 2 * calculationStep;

                currentVariable.Value += calculationStep;
                prevVariable.Value += calculationStep;
            }

            return result;
        }

        public double CalcualtionSimpson(Expression integrand, double startValue, double endValue, int numberOfSteps, string variableName)
        {
            double result = 0.0;
            double calculationStep = GetStep(startValue, endValue, numberOfSteps);

            Variable currentVariable = new Variable(variableName, startValue);

            result += integrand.GetResultValue(currentVariable);
            for (int i = 1; i < numberOfSteps; i++)
            {
                currentVariable.Value += calculationStep;
                result += 2 * integrand.GetResultValue(currentVariable);
            }

            currentVariable.Value += calculationStep;
            result += integrand.GetResultValue(currentVariable);

            result *= calculationStep / 2.0;

            return result;
        }

        private double GetStep(double startValue, double endValue, int iterationNumber)
        {
            if (startValue > endValue)
            {
                throw new ArgumentException($"Parameter 'startValue' is greater than 'endValue' which is wrong! {startValue} > {endValue}");
            }

            if (iterationNumber <= 0)
            {
                throw new ArgumentException($"Parameter 'iterationNumber' is expected to be more than zero. Now it is {iterationNumber}");
            }

            return (endValue - startValue) / iterationNumber;
        }
    }
}
