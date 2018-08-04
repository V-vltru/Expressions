namespace DifferentialEquationSystem.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Excel = Microsoft.Office.Interop.Excel;

    public static class Reporting
    {
        /// <summary>
        /// Generates the excel report for 
        /// </summary>
        /// <param name="differentialEquationSystem"></param>
        /// <param name="leftVariables"></param>
        /// <param name="constants"></param>
        /// <param name="timeVariable"></param>
        /// <param name="tEnd"></param>
        /// <param name="tau"></param>
        /// <param name="calculationTypes"></param>
        /// <param name="times"></param>
        /// <param name="results"></param>
        /// <param name="allVariables"></param>
        /// <param name="excelPath"></param>
        public static void GenerateExcelReport(List<string> differentialEquationSystem, List<InitVariable> leftVariables, List<InitVariable> constants, InitVariable timeVariable, 
            double tEnd, double tau, List<CalculationTypeNames> calculationTypes, Dictionary<CalculationTypeNames, double> times, Dictionary<CalculationTypeNames, List<InitVariable>> results,
                Dictionary<CalculationTypeNames, List<List<InitVariable>>> allVariables, string excelPath)
        {
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null)
            {
                throw new NullReferenceException("Excel is not properly installed!!");
            }

            Excel.Workbook xlWorkbook = xlApp.Workbooks.Add();
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkbook.Worksheets.get_Item(1);
            throw new NotImplementedException();
        }
    }
}
