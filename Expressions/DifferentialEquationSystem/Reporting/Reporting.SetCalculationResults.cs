namespace DifferentialEquationSystem
{
    using System;
    using System.Collections.Generic;
    using Excel = Microsoft.Office.Interop.Excel;

    public partial class DifferentialEquationSystem
    {
        /// <summary>
        /// Method fills the sheet with calculation results
        /// </summary>
        /// <param name="worksheet">Worksheet to save the results there</param>
        /// <param name="calculationTypeName">CalculationType</param>
        /// <param name="result">Container with result variables</param>
        /// <param name="allVariables">Cantainer with variables values for each time step</param>
        /// <param name="calcTime">Time reqiured for calculation</param>
        public void SetCalculationResults(Excel.Worksheet worksheet, CalculationTypeName calculationTypeName, 
            List<DEVariable> result, List<List<DEVariable>> allVariables, double calcTime)
        {
            worksheet.Name = calculationTypeName.ToString();
            int rowIndex = 1;
            int columnIndex = 1;

            for (int i = 0; i < result.Count; i++)
            {
                worksheet.Cells[rowIndex, columnIndex] = $"{result[i].Name} = ";
                worksheet.Cells[rowIndex, columnIndex + 1] = result[i].Value;
                rowIndex++;
            }

            rowIndex++;
            worksheet.Cells[rowIndex, columnIndex] = "Calculation time = ";
            worksheet.Cells[rowIndex, columnIndex + 1] = calcTime;
            rowIndex++;

            if (allVariables != null)
            {
                rowIndex++;
                worksheet.Cells[rowIndex, columnIndex] = "Detailed results";
                rowIndex++;

                for (int i = 0; i < allVariables[0].Count; i++)
                {
                    worksheet.Cells[rowIndex, columnIndex + i] = allVariables[0][i].Name;
                }

                rowIndex++;

                string leftTopCellforChart = GetExcelColumnName(columnIndex) + rowIndex.ToString();
                string horinzontalAxisTop = GetExcelColumnName(columnIndex + allVariables[0].Count - 1) + rowIndex.ToString();
                int idxStart = rowIndex;

                for (int i = 0; i < allVariables.Count; i++)
                {
                    for (int j = 0; j < allVariables[0].Count; j++)
                    {
                        worksheet.Cells[rowIndex, columnIndex + j] = allVariables[i][j].Value;
                    }

                    rowIndex++;
                }

                string rightDownCellForChart = GetExcelColumnName(columnIndex + allVariables[0].Count - 2) + (allVariables.Count - 1 + idxStart).ToString();
                string horizontalAlignmentDown = GetExcelColumnName(columnIndex + allVariables[0].Count - 1) + (allVariables.Count - 1 + idxStart).ToString();
                Excel.Range chartRange;
                Excel.Range horizontalAlignmentRange;
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
                Excel.ChartObject mychart = xlCharts.Add(10, 80, 500, 450);
                Excel.Chart chartPage = mychart.Chart;

                chartRange = worksheet.get_Range(leftTopCellforChart, rightDownCellForChart);
                chartPage.SetSourceData(chartRange);
                chartPage.ChartType = Excel.XlChartType.xlLine;
                horizontalAlignmentRange = worksheet.get_Range(horinzontalAxisTop, horizontalAlignmentDown);

                chartPage.SeriesCollection(1).XValues = horizontalAlignmentRange;

                for (int i = 0; i < this.LeftVariables.Count; i++)
                {
                    chartPage.SeriesCollection(i + 1).Name = this.LeftVariables[i].Name;
                }
            }
        }
    }
}
