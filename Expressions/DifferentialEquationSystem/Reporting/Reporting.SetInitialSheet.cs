namespace DifferentialEquationSystem
{
    using System.Collections.Generic;
    using Excel = Microsoft.Office.Interop.Excel;

    public partial class DifferentialEquationSystem
    {
        /// <summary>
        /// Method fills the Initial worksheet
        /// </summary>
        /// <param name="xlWorkSheet">Worksheet to fill content there</param>
        /// <param name="calculationTypes">List of calclaulation types</param>
        private void SetInitalSheet(Excel.Worksheet xlWorkSheet, List<CalculationTypeName> calculationTypes)
        {
            xlWorkSheet.Name = "Intial parameters";
            int rowIndex = 1;
            int columnIndex = 1;

            // Adding a header
            xlWorkSheet.Cells[rowIndex, columnIndex] = "Differential Equation System";

            // Adding a differential equation content
            rowIndex++;
            for (int i = 0; i < this.Expressions.Count; i++)
            {
                xlWorkSheet.Cells[rowIndex, columnIndex] = $"{this.LeftVariables[i].Name}' = ";
                xlWorkSheet.Cells[rowIndex, columnIndex + 1] = this.Expressions[i];

                rowIndex++;
            }

            // Adding initial values
            rowIndex++;
            xlWorkSheet.Cells[rowIndex, columnIndex] = "Intial values";
            rowIndex++;

            // Adding initial values content
            for (int i = 0; i < this.LeftVariables.Count; i++)
            {
                xlWorkSheet.Cells[rowIndex, columnIndex] = $"{this.LeftVariables[i].Name}' = ";
                xlWorkSheet.Cells[rowIndex, columnIndex + 1] = this.LeftVariables[i].Value.ToString();

                rowIndex++;
            }

            rowIndex++;
            xlWorkSheet.Cells[rowIndex, columnIndex] = "Time parameters";
            rowIndex++;

            xlWorkSheet.Cells[rowIndex, columnIndex] = $"{this.TimeVariable.Name} = ";
            xlWorkSheet.Cells[rowIndex, columnIndex + 1] = this.TimeVariable.Value.ToString();
            rowIndex++;

            xlWorkSheet.Cells[rowIndex, columnIndex] = "Tau = ";
            xlWorkSheet.Cells[rowIndex, columnIndex + 1] = this.Tau;
            rowIndex++;

            xlWorkSheet.Cells[rowIndex, columnIndex] = "TimeEnd = ";
            xlWorkSheet.Cells[rowIndex, columnIndex + 1] = this.TEnd;
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
