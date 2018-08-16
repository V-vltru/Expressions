namespace DifferentialEquationSystem
{
    using System;
    using System.Collections.Generic;
    using Excel = Microsoft.Office.Interop.Excel;

    public partial class DifferentialEquationSystem
    {
        public void SetCommonResults(Excel.Worksheet worksheet, Dictionary<CalculationTypeNames, double> calculationTimes, 
            Dictionary<CalculationTypeNames, List<InitVariable>> results)
        {
            worksheet.Name = "Common results";
            int rowIndex = 1;
            int columnIndex = 1;

            worksheet.Cells[rowIndex, columnIndex] = "Calculation results";
            rowIndex++;

            int i = 0;
            
            foreach(KeyValuePair<CalculationTypeNames, List<InitVariable>> item in results)
            {
                int j = 0;
                List<InitVariable> result;

                if (i == 0)
                {                
                    result = item.Value;
                    foreach (InitVariable variable in result)
                    {
                        worksheet.Cells[rowIndex + j + 1, columnIndex] = variable.Name;
                        j++;
                    }
                }

                worksheet.Cells[rowIndex, columnIndex + i + 1] = item.Key.ToString();

                j = 0;
                result = item.Value;
                foreach (InitVariable variable in result)
                {
                    worksheet.Cells[rowIndex + 1 + j, columnIndex + i + 1] = variable.Value;
                    j++;
                }

                i++;
            }

            rowIndex += 3;
            worksheet.Cells[rowIndex, columnIndex] = "Time results";
            rowIndex++;

            i = 0;
            foreach(KeyValuePair<CalculationTypeNames, double> calculationTime in calculationTimes)
            {
                worksheet.Cells[rowIndex, columnIndex + i] = calculationTime.Key.ToString();
                worksheet.Cells[rowIndex + 1, columnIndex + i] = calculationTime.Value;

                i++;
            }

        }
    }
}
