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
            calcTime = differentialEquationSystem.Calculate(CalculationTypeNames.Euler, out resultEuler, perTime);

            double calcTimeForecast = 0;
            List<List<InitVariable>> perTimeForecast = new List<List<InitVariable>>();
            List<InitVariable> resultForecastCorrection;
            calcTimeForecast = differentialEquationSystem.Calculate(CalculationTypeNames.ForecastCorrection, out resultForecastCorrection, perTimeForecast);

            double calcTimeRK2 = 0;
            List<List<InitVariable>> perTimeRK2 = new List<List<InitVariable>>();
            List<InitVariable> resultRK2;
            calcTimeRK2 = differentialEquationSystem.Calculate(CalculationTypeNames.RK2, out resultRK2, perTimeRK2);

            double calcTimeRK4 = 0;
            List<List<InitVariable>> perTimeRK4 = new List<List<InitVariable>>();
            List<InitVariable> resultRK4;
            calcTimeRK4 = differentialEquationSystem.Calculate(CalculationTypeNames.RK4, out resultRK4, perTimeRK4);

            double calcTimeAdams1 = 0;
            List<List<InitVariable>> perTimeAdams1 = new List<List<InitVariable>>();
            List<InitVariable> resultAdams1;
            calcTimeAdams1 = differentialEquationSystem.Calculate(CalculationTypeNames.AdamsExtrapolationOne, out resultAdams1, perTimeAdams1);

            double calcTimeAdams2 = 0;
            List<List<InitVariable>> perTimeAdams2 = new List<List<InitVariable>>();
            List<InitVariable> resultAdams2;
            calcTimeAdams2 = differentialEquationSystem.Calculate(CalculationTypeNames.AdamsExtrapolationTwo, out resultAdams2, perTimeAdams2);

            double calcTimeAdams3 = 0;
            List<List<InitVariable>> perTimeAdams3 = new List<List<InitVariable>>();
            List<InitVariable> resultAdams3;
            calcTimeAdams3 = differentialEquationSystem.Calculate(CalculationTypeNames.AdamsExtrapolationThree, out resultAdams3, perTimeAdams3);

            double calcTimeAdams4 = 0;
            List<List<InitVariable>> perTimeAdams4 = new List<List<InitVariable>>();
            List<InitVariable> resultAdams4;
            calcTimeAdams4 = differentialEquationSystem.Calculate(CalculationTypeNames.AdamsExtrapolationFour, out resultAdams4, perTimeAdams4);

            double calcTimeMiln = 0;
            List<List<InitVariable>> perTimeMiln = new List<List<InitVariable>>();
            List<InitVariable> resultMiln;
            calcTimeMiln = differentialEquationSystem.Calculate(CalculationTypeNames.Miln, out resultMiln, perTimeMiln);

            #region CalculateWithGroupOfMethodsSync

            List<CalculationTypeNames> calculationTypes = new List<CalculationTypeNames>
            {
                CalculationTypeNames.Euler,
                CalculationTypeNames.EulerAsyc,
                CalculationTypeNames.ForecastCorrection,
                CalculationTypeNames.ForecastCorrectionAsync,
                CalculationTypeNames.RK2,
                CalculationTypeNames.RK2Async,
                CalculationTypeNames.RK4,
                CalculationTypeNames.RK4Async,
                CalculationTypeNames.AdamsExtrapolationOne,
                CalculationTypeNames.AdamsExtrapolationOneAsync,
                CalculationTypeNames.AdamsExtrapolationTwo,
                CalculationTypeNames.AdamsExtrapolationTwoAsync,
                CalculationTypeNames.AdamsExtrapolationThree,
                CalculationTypeNames.AdamsExtrapolationThreeAsync,
                CalculationTypeNames.AdamsExtrapolationFour,
                CalculationTypeNames.AdamsExtrapolationFourAsync,
                CalculationTypeNames.Miln,
                CalculationTypeNames.MilnAsync
            };

            Dictionary<CalculationTypeNames, List<InitVariable>> results;
            Dictionary<CalculationTypeNames, List<List<InitVariable>>> variablesAtAllSteps = new Dictionary<CalculationTypeNames, List<List<InitVariable>>>();

            Dictionary<CalculationTypeNames, double> calcTimes = differentialEquationSystem.CalculateWithGroupOfMethodsSync(calculationTypes, out results, variablesAtAllSteps);

            differentialEquationSystem.GenerateExcelReport(calculationTypes, calcTimes, results, variablesAtAllSteps, null);
            #endregion
        }
    }
}
