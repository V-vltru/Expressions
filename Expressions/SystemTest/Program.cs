using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DifferentialEquationSystem;

namespace SystemTest
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> expressions = new List<string>
            {
                "2 * y1 - y + time * exp(time)",
                "y1"
            };

            List<InitVariable> leftVariables = new List<InitVariable>
            {
                new InitVariable("y", 2),
                new InitVariable("y1", 1),
            };

            InitVariable timeVariable = new InitVariable("time", 0);

            DifferentialEquationSystem.DifferentialEquationSystem differentialEquationSystem = new DifferentialEquationSystem.DifferentialEquationSystem(expressions, leftVariables, null, timeVariable, 1.5, 0.001);
            List<List<InitVariable>> perTime = new List<List<InitVariable>>();
            double calcTime = 0;
            List<InitVariable> resultEuler;
            calcTime = differentialEquationSystem.Calculate(CalculationTypeName.Euler, out resultEuler, perTime);

            double calcTimeForecast = 0;
            List<List<InitVariable>> perTimeForecast = new List<List<InitVariable>>();
            List<InitVariable> resultForecastCorrection;
            calcTimeForecast = differentialEquationSystem.Calculate(CalculationTypeName.ForecastCorrection, out resultForecastCorrection, perTimeForecast);

            double calcTimeRK2 = 0;
            List<List<InitVariable>> perTimeRK2 = new List<List<InitVariable>>();
            List<InitVariable> resultRK2;
            calcTimeRK2 = differentialEquationSystem.Calculate(CalculationTypeName.RK2, out resultRK2, perTimeRK2);

            double calcTimeRK4 = 0;
            List<List<InitVariable>> perTimeRK4 = new List<List<InitVariable>>();
            List<InitVariable> resultRK4;
            calcTimeRK4 = differentialEquationSystem.Calculate(CalculationTypeName.RK4, out resultRK4, perTimeRK4);

            double calcTimeAdams1 = 0;
            List<List<InitVariable>> perTimeAdams1 = new List<List<InitVariable>>();
            List<InitVariable> resultAdams1;
            calcTimeAdams1 = differentialEquationSystem.Calculate(CalculationTypeName.AdamsExtrapolationOne, out resultAdams1, perTimeAdams1);

            double calcTimeAdams2 = 0;
            List<List<InitVariable>> perTimeAdams2 = new List<List<InitVariable>>();
            List<InitVariable> resultAdams2;
            calcTimeAdams2 = differentialEquationSystem.Calculate(CalculationTypeName.AdamsExtrapolationTwo, out resultAdams2, perTimeAdams2);

            double calcTimeAdams3 = 0;
            List<List<InitVariable>> perTimeAdams3 = new List<List<InitVariable>>();
            List<InitVariable> resultAdams3;
            calcTimeAdams3 = differentialEquationSystem.Calculate(CalculationTypeName.AdamsExtrapolationThree, out resultAdams3, perTimeAdams3);

            double calcTimeAdams4 = 0;
            List<List<InitVariable>> perTimeAdams4 = new List<List<InitVariable>>();
            List<InitVariable> resultAdams4;
            calcTimeAdams4 = differentialEquationSystem.Calculate(CalculationTypeName.AdamsExtrapolationFour, out resultAdams4, perTimeAdams4);

            double calcTimeMiln = 0;
            List<List<InitVariable>> perTimeMiln = new List<List<InitVariable>>();
            List<InitVariable> resultMiln;
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

            Dictionary<CalculationTypeName, List<InitVariable>> results;
            Dictionary<CalculationTypeName, List<List<InitVariable>>> variablesAtAllSteps = new Dictionary<CalculationTypeName, List<List<InitVariable>>>();

            Dictionary<CalculationTypeName, double> calcTimes = differentialEquationSystem.CalculateWithGroupOfMethodsSync(calculationTypes, out results, variablesAtAllSteps);

            differentialEquationSystem.GenerateExcelReport(calculationTypes, calcTimes, results, variablesAtAllSteps, "csharp-Excel.xls");
            #endregion
        }
    }
}
