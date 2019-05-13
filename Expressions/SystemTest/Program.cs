using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DifferentialEquationSystem;
using LinearAlgebraicEquationsSystem;
using Integral;
using Expressions.Models;
using Expressions;

namespace SystemTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //VerifyExpressions();
            // VerifyIntegrals();
            //VerifyDifferentialEquations();

            MatrixT<int> mat = new MatrixT<int>(new int[3, 4] { { 2, -1, 1, 1 }, { 1, 2, -1, 1 }, { 1, 7, -4, 2 } });
            double det = MatrixT<int>.GetRang(mat);

            Console.WriteLine(det);

            MatrixT<int> mat2 = new MatrixT<int>(new int[3, 5] { { 2, -1, 1, 1, 1 }, { 1, 2, -1, 1, 2 }, { 1, 7, -4, 2, 5 } });
            double det2 = MatrixT<int>.GetRang(mat2);

            Console.WriteLine(det2);

            MatrixT<double> mat3 = new MatrixT<double>(new double[3, 3] { { 2, 1, 1 }, { 1, -1, 0 }, { 3, -1, 2} });
            MatrixT<double> reversed = MatrixT<double>.GetInverseMatrix(mat3);

            MatrixT<double> result = reversed * mat3;

            for (int i = 0; i < reversed.Rows; i++)
            {
                for (int j = 0; j < reversed.Columns; j++)
                {
                    Console.Write(reversed[i, j] + " ");
                }

                Console.WriteLine();
            }

            MatrixT<double> c = 2 * mat3;

            List<string> leftParts = new List<string>
            {
                "2 * x1 + x2 + x3",
                "x1 - x2",
                "3 * x1 - x2 + 2 * x3"
            };

            List<LAEVariable> lAEVariables = new List<LAEVariable>
            {
                new LAEVariable("x1", 0),
                new LAEVariable("x2", 0),
                new LAEVariable("x3", 0)
            };

            List<double> rightParts = new List<double>
            {
                2, -2, 2
            };

            LinearAlgebraicEquationSystem linearAlgebraicEquationSystem = new LinearAlgebraicEquationSystem(leftParts, rightParts, lAEVariables, null);

            List<LAEVariable> res = linearAlgebraicEquationSystem.CalculateMatrixMethod();

            Console.ReadKey();
        }

        static void VerifyIntegrals()
        {
            string integrand = "x*x";
            double startValue = -1;
            double endValue = 1;
            int iterationsNumber = 1000;
            string parameterName = "x";

            Integral.Integral integral = new Integral.Integral(integrand, startValue, endValue, iterationsNumber, parameterName);

            double resultLeftRectangle = integral.Calculate(CalculationType.LeftRectangle);
            double resultRightRectangle = integral.Calculate(CalculationType.RightRectangle);
            double resultAverageRectangle = integral.Calculate(CalculationType.AverageRectangle);
            double resultTrapezium = integral.Calculate(CalculationType.Trapezium);
            double resultSimpson = integral.Calculate(CalculationType.Simpson);

            Console.WriteLine($"Left rectangle: {resultLeftRectangle}");
            Console.WriteLine($"Right rectangle: {resultRightRectangle}");
            Console.WriteLine($"Average rectangle: {resultAverageRectangle}");
            Console.WriteLine($"Trapezium: {resultTrapezium}");
            Console.WriteLine($"Simpson: {resultSimpson}");

            Console.ReadKey();
        }

        static void VerifyDifferentialEquations()
        {
            List<string> expressions = new List<string>
            {
                "2 * y1 - y + time * exp(time)",
                "y1"
            };

            List<DEVariable> leftVariables = new List<DEVariable>
            {
                new DEVariable("y", 2),
                new DEVariable("y1", 1),
            };

            DEVariable timeVariable = new DEVariable("time", 0);

            DifferentialEquationSystem.DifferentialEquationSystem differentialEquationSystem = new DifferentialEquationSystem.DifferentialEquationSystem(expressions, leftVariables, null, timeVariable, 1.5, 0.001);
            List<List<DEVariable>> perTime = new List<List<DEVariable>>();
            double calcTime = 0;
            List<DEVariable> resultEuler;
            calcTime = differentialEquationSystem.Calculate(CalculationTypeName.Euler, out resultEuler, perTime);

            double calcTimeForecast = 0;
            List<List<DEVariable>> perTimeForecast = new List<List<DEVariable>>();
            List<DEVariable> resultForecastCorrection;
            calcTimeForecast = differentialEquationSystem.Calculate(CalculationTypeName.ForecastCorrection, out resultForecastCorrection, perTimeForecast);

            double calcTimeRK2 = 0;
            List<List<DEVariable>> perTimeRK2 = new List<List<DEVariable>>();
            List<DEVariable> resultRK2;
            calcTimeRK2 = differentialEquationSystem.Calculate(CalculationTypeName.RK2, out resultRK2, perTimeRK2);

            double calcTimeRK4 = 0;
            List<List<DEVariable>> perTimeRK4 = new List<List<DEVariable>>();
            List<DEVariable> resultRK4;
            calcTimeRK4 = differentialEquationSystem.Calculate(CalculationTypeName.RK4, out resultRK4, perTimeRK4);

            double calcTimeAdams1 = 0;
            List<List<DEVariable>> perTimeAdams1 = new List<List<DEVariable>>();
            List<DEVariable> resultAdams1;
            calcTimeAdams1 = differentialEquationSystem.Calculate(CalculationTypeName.AdamsExtrapolationOne, out resultAdams1, perTimeAdams1);

            double calcTimeAdams2 = 0;
            List<List<DEVariable>> perTimeAdams2 = new List<List<DEVariable>>();
            List<DEVariable> resultAdams2;
            calcTimeAdams2 = differentialEquationSystem.Calculate(CalculationTypeName.AdamsExtrapolationTwo, out resultAdams2, perTimeAdams2);

            double calcTimeAdams3 = 0;
            List<List<DEVariable>> perTimeAdams3 = new List<List<DEVariable>>();
            List<DEVariable> resultAdams3;
            calcTimeAdams3 = differentialEquationSystem.Calculate(CalculationTypeName.AdamsExtrapolationThree, out resultAdams3, perTimeAdams3);

            double calcTimeAdams4 = 0;
            List<List<DEVariable>> perTimeAdams4 = new List<List<DEVariable>>();
            List<DEVariable> resultAdams4;
            calcTimeAdams4 = differentialEquationSystem.Calculate(CalculationTypeName.AdamsExtrapolationFour, out resultAdams4, perTimeAdams4);

            double calcTimeMiln = 0;
            List<List<DEVariable>> perTimeMiln = new List<List<DEVariable>>();
            List<DEVariable> resultMiln;
            calcTimeMiln = differentialEquationSystem.Calculate(CalculationTypeName.Miln, out resultMiln, perTimeMiln);

            #region CalculateWithGroupOfMethodsSync

            List<CalculationTypeName> calculationTypes = new List<CalculationTypeName>
            {
                CalculationTypeName.Euler,
                CalculationTypeName.EulerAsyc,
                CalculationTypeName.ForecastCorrection,
                CalculationTypeName.ForecastCorrectionAsync,
                CalculationTypeName.RK2,
                CalculationTypeName.RK2Async,
                CalculationTypeName.RK4,
                CalculationTypeName.RK4Async,
                CalculationTypeName.AdamsExtrapolationOne,
                CalculationTypeName.AdamsExtrapolationOneAsync,
                CalculationTypeName.AdamsExtrapolationTwo,
                CalculationTypeName.AdamsExtrapolationTwoAsync,
                CalculationTypeName.AdamsExtrapolationThree,
                CalculationTypeName.AdamsExtrapolationThreeAsync,
                CalculationTypeName.AdamsExtrapolationFour,
                CalculationTypeName.AdamsExtrapolationFourAsync,
                CalculationTypeName.Miln,
                CalculationTypeName.MilnAsync
            };

            Dictionary<CalculationTypeName, List<DEVariable>> results;
            Dictionary<CalculationTypeName, List<List<DEVariable>>> variablesAtAllSteps = new Dictionary<CalculationTypeName, List<List<DEVariable>>>();

            Dictionary<CalculationTypeName, double> calcTimes = differentialEquationSystem.CalculateWithGroupOfMethodsSync(calculationTypes, out results, variablesAtAllSteps);

            Reporting.GenerateExcelReport(calculationTypes, calcTimes, results, variablesAtAllSteps, "csharp-Excel.xls", differentialEquationSystem);
            #endregion
        }

        static void VerifyExpressions()
        {
            string expression = "2 * x - 3 * y + 5 * z";

            List<Variable> variables = new List<Variable>
            {
                new Variable("x", 0),
                new Variable("y", 0),
                new Variable("z", 1)
            };

            Expression exp = new Expression(expression, variables);
            double result = exp.GetResultValue(variables);

            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
