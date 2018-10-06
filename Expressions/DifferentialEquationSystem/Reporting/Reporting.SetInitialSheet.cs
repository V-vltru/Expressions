namespace DifferentialEquationSystem
{
    using Expressions;
    using Expressions.Models;
    using System.Collections.Generic;
    using Excel = Microsoft.Office.Interop.Excel;

    public static partial class Reporting
    {
        /// <summary>
        /// Method fills the Initial worksheet
        /// </summary>
        /// <param name="xlWorkSheet">Worksheet to fill content there</param>
        /// <param name="calculationTypes">List of calclaulation types</param>
        private static void SetInitalSheet(Excel.Worksheet xlWorkSheet, List<CalculationTypeName> calculationTypes, List<Variable> leftVariables, Variable timeVariable, double tau, double tEnd, List<Expression> expressions)
        {
            xlWorkSheet.Name = "Intial parameters";
            int rowIndex = 1;
            int columnIndex = 1;

            // Adding a header
            xlWorkSheet.Cells[rowIndex, columnIndex] = "Differential Equation System";

            // Adding a differential equation content
            rowIndex++;
            for (int i = 0; i < expressions.Count; i++)
            {
                xlWorkSheet.Cells[rowIndex, columnIndex] = $"{leftVariables[i].Name}' = ";
                xlWorkSheet.Cells[rowIndex, columnIndex + 1] = expressions[i];

                rowIndex++;
            }

            // Adding initial values
            rowIndex++;
            xlWorkSheet.Cells[rowIndex, columnIndex] = "Intial values";
            rowIndex++;

            // Adding initial values content
            for (int i = 0; i < leftVariables.Count; i++)
            {
                xlWorkSheet.Cells[rowIndex, columnIndex] = $"{leftVariables[i].Name}' = ";
                xlWorkSheet.Cells[rowIndex, columnIndex + 1] = leftVariables[i].Value.ToString();

                rowIndex++;
            }

            rowIndex++;
            xlWorkSheet.Cells[rowIndex, columnIndex] = "Time parameters";
            rowIndex++;

            xlWorkSheet.Cells[rowIndex, columnIndex] = $"{timeVariable.Name} = ";
            xlWorkSheet.Cells[rowIndex, columnIndex + 1] = timeVariable.Value.ToString();
            rowIndex++;

            xlWorkSheet.Cells[rowIndex, columnIndex] = "Tau = ";
            xlWorkSheet.Cells[rowIndex, columnIndex + 1] = tau;
            rowIndex++;

            xlWorkSheet.Cells[rowIndex, columnIndex] = "TimeEnd = ";
            xlWorkSheet.Cells[rowIndex, columnIndex + 1] = tEnd;
            rowIndex++;

            rowIndex++;
            xlWorkSheet.Cells[rowIndex, columnIndex] = "Calculate with next methods:";
            rowIndex++;

            for (int i = 0; i < calculationTypes.Count; i++)
            {
                xlWorkSheet.Cells[rowIndex, columnIndex] = calculationTypes[i].ToString();
                rowIndex++;
            }
        }
    }
}
