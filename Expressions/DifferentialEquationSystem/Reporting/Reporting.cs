namespace DifferentialEquationSystem
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Excel = Microsoft.Office.Interop.Excel;

    public partial class DifferentialEquationSystem
    {
        /// <summary>
        /// Generates the excel report for 
        /// </summary>
        /// <param name="calculationTypes"></param>
        /// <param name="times"></param>
        /// <param name="results"></param>
        /// <param name="allVariables"></param>
        /// <param name="excelPath"></param>
        public void GenerateExcelReport(List<CalculationTypeNames> calculationTypes, Dictionary<CalculationTypeNames, double> times, Dictionary<CalculationTypeNames, List<InitVariable>> results,
                Dictionary<CalculationTypeNames, List<List<InitVariable>>> allVariables, string excelPath)
        {
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null)
            {
                throw new NullReferenceException("Excel is not properly installed!!");
            }

            Excel.Workbook xlWorkbook = xlApp.Workbooks.Add();

            // Adding variables per each steps
            for (int i = calculationTypes.Count - 1; i >= 0; i--)
            {            
                Excel.Worksheet itemWorkSheet = (Excel.Worksheet)xlWorkbook.Worksheets.Add();
                SetCalculationResults(itemWorkSheet, calculationTypes[i], results[calculationTypes[i]], allVariables[calculationTypes[i]], times[calculationTypes[i]]);             
            }

            Excel.Worksheet commonResultsWorksheet = (Excel.Worksheet)xlWorkbook.Worksheets.Add();
            SetCommonResults(commonResultsWorksheet, times, results);
            Excel.Worksheet initialXlWorkSheet = (Excel.Worksheet)xlWorkbook.Worksheets.Add();
            SetInitalSheet(initialXlWorkSheet, calculationTypes);

            xlWorkbook.SaveAs("csharp-Excel.xls", Excel.XlFileFormat.xlWorkbookNormal);                   
            xlWorkbook.Close();
            xlApp.Quit();

            Marshal.ReleaseComObject(initialXlWorkSheet);
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlApp);
        }

        /// <summary>
        /// Method gets column name by its index
        /// </summary>
        /// <param name="columnNumber">Number of a column</param>
        /// <returns>Column name</returns>
        private static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}
