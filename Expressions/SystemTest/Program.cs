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
            List<InitVariable> resultEuler = differentialEquationSystem.EulerCalculation(out calcTime, perTime, true);

            double calcTimeForecast = 0;
            List<List<InitVariable>> perTimeForecast = new List<List<InitVariable>>();
            List<InitVariable> resultForecastCorrection = differentialEquationSystem.ForecastCorrectionCalculation(out calcTimeForecast, perTimeForecast, true);
        }
    }
}
