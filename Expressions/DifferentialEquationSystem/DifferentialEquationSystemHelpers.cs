using System;
using System.Collections.Generic;
using Expressions;
using Expressions.Models;

namespace DifferentialEquationSystem
{
    public static class DifferentialEquationSystemHelpers
    {
        /// <summary>
        /// Method converts List<InitVariable> items to List<Variable> items
        /// </summary>
        /// <param name="initVariables"></param>
        /// <returns></returns>
        public static List<Variable> GetVariablesFromInitVariables(List<InitVariable> initVariables)
        {
            List<Variable> result = new List<Variable>();
            foreach (InitVariable initVariable in initVariables)
            {
                result.Add(initVariable);
            }

            return result;
        }

        /// <summary>
        /// Method validates initial parameters.
        /// It is recommended to invoke this method before differentail equation system calculation
        /// </summary>
        /// <param name="expressionSystem">List of expression of the differential equation system</param>
        /// <param name="leftVariables">List of left variables of the differential equation system</param>
        /// <param name="timeVariable">Time variable</param>
        /// <param name="Tau">Calculation step</param>
        /// <param name="tEnd">The end time of the calculation process</param>
        public static void CheckVariables(List<Expression> expressionSystem, List<Variable> leftVariables, Variable timeVariable,
            double Tau, double tEnd)
        {
            // Validation of the Expression list of the differential equation system
            if (expressionSystem == null || expressionSystem.Count == 0)
            {
                throw new ArgumentException("Container 'expressions' of the constructor cannot be null or empty! Nothing in the differential equation system.");
            }
            else if (expressionSystem.Count != leftVariables.Count)
            {
                throw new ArgumentException($"Number of expressions must be equal to the number of left variables! Number of expressions:{expressionSystem.Count}; Number of left variables: {leftVariables.Count}");
            }

            // Validation of the left variables of the differential equation system
            if (leftVariables == null || leftVariables.Count == 0)
            {
                throw new ArgumentException("Container 'leftVariables' of the constructor cannot be null or empty! Nothing in the left part.");
            }

            // Validation of the time parameter
            if (timeVariable == null)
            {
                throw new ArgumentNullException("Start time cannot be null!");
            }

            // Validation of the end time parameter
            if (timeVariable.Value > tEnd)
            {
                throw new ArgumentException($"End time should be more or equal than start one! Start time: {timeVariable.Value}; End time: {tEnd}");
            }

            // Validation of the calculation step
            if (Tau <= 0)
            {
                throw new ArgumentException($"Tau is required to be more than zero! Yours: {Tau}");
            }
            else if (Tau > tEnd - timeVariable.Value)
            {
                throw new ArgumentException($"Tau cannot be more than calculation interval! Tau: {Tau}; Start time: {timeVariable.Value}; End time: {tEnd}; Interval: {tEnd - timeVariable.Value}");
            }
        }

        /// <summary>
        /// Method copies Variables from source containier to the destination one
        /// </summary>
        /// <param name="sourceVariables">Source list with Variables</param>
        /// <param name="destVariables">Destination list where Variables from source are expected to be copied to</param>
        public static void CopyVariables(List<Variable> sourceVariables, List<Variable> destVariables)
        {
            destVariables.Clear();
            foreach(Variable oneFromSource in sourceVariables)
            {
                destVariables.Add(new Variable(oneFromSource.Name, oneFromSource.Value));
            }
        }
    }
}
