namespace DifferentialEquationSystem
{
    using System;
    using System.Collections.Generic;
    using Excel = Microsoft.Office.Interop.Excel;

    public partial class DifferentialEquationSystem
    {
        /// <summary>
        /// Method sets the sheet with common results
        /// </summary>
        /// <param name="worksheet">Worksheet where the information will be saved</param>
        /// <param name="calculationTimes">Time results for each calculation types</param>
        /// <param name="results">results for each calculation types</param>
        public void SetCommonResults(Excel.Worksheet worksheet, Dictionary<CalculationTypeName, double> calculationTimes, 
            Dictionary<CalculationTypeName, List<InitVariable>> results)
        {
            worksheet.Name = "Common results";
            int rowIndex = 1;
            int columnIndex = 1;

            worksheet.Cells[rowIndex, columnIndex] = "Calculation results";
            rowIndex++;

            int i = 0;
            
            foreach(KeyValuePair<CalculationTypeName, List<InitVariable>> item in results)
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
            foreach(KeyValuePair<CalculationTypeName, double> calculationTime in calculationTimes)
            {
                worksheet.Cells[rowIndex, columnIndex + i] = calculationTime.Key.ToString();
                worksheet.Cells[rowIndex + 1, columnIndex + i] = calculationTime.Value;

                i++;
            }

            string leftTopTimeChart = GetExcelColumnName(1) + rowIndex.ToString();
            string rightDownTimeChart = GetExcelColumnName(calculationTimes.Count) + (rowIndex + 1).ToString();

            Excel.Range chartRange;
            Excel.ChartObjects xlCharts = (Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
            Excel.ChartObject mychart = xlCharts.Add(10, 80, 500, 450);
            Excel.Chart chartPage = mychart.Chart;

            chartRange = worksheet.get_Range(leftTopTimeChart, rightDownTimeChart);
            chartPage.SetSourceData(chartRange);
            chartPage.ChartType = Excel.XlChartType.xl3DColumnClustered;

            chartPage.SeriesCollection(1).Name = "Calculation times";
        }
    }
}
