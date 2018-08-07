using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DifferentialEquationSystem
{
    public enum CalculationTypeNames
    {
        Euler,
        EulerAsyc,
        ForecastCorrection,
        ForecastCorrectionAsync,
        RK2,
        RK2Async,
        RK4,
        RK4Async,
        AdamsExtrapolationOne,
        AdamsExtrapolationOneAsync,
        AdamsExtrapolationTwo,
        AdamsExtrapolationTwoAsync,
        AdamsExtrapolationThree,
        AdamsExtrapolationThreeAsync,
        AdamsExtrapolationFour,
        AdamsExtrapolationFourAsync,
        Miln,
        MilnAsync
    }
}
