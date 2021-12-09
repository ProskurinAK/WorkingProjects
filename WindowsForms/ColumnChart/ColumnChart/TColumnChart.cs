using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Excel = Microsoft.Office.Interop.Excel;
// ************************************************************************************************************

namespace ColumnChart
{
    class TColumnChart
    {
        UForm_ColumnChart Form_ColumnChart = new UForm_ColumnChart();

        List<string> SeriesName = new List<string>();   // Список для хранения имён серий(используется в методе RemoveSeries)
        List<double> XValue = new List<double>();   // Список для храения Поля X точки даннных(используется при передачи в Excel)
        List<double> YValue = new List<double>();   // Список для храения Поля Y точки даннных(используется при передачи в Excel)

        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод добавления серии в диаграмму
        /// </summary>
        /// <param name="Name">Имя серии</param>
        /// <param name="X">Поле X точки данных</param>
        /// <param name="Y">Поле Y точки данных</param>
        /// <param name="Colour">Цвет серии</param>
        /// <param name="Font">Шрифт серии</param>
        public void AddSeries(string Name, double X, double Y, Color Colour, Font Font)
        {
            Series NewSeries = new Series(Name);
            NewSeries.ChartType = SeriesChartType.Column;

            NewSeries.Points.AddXY(X, Y);

            NewSeries.Color = Colour;
            NewSeries.Font = Font;

            SeriesName.Add(Name);
            XValue.Add(X);
            YValue.Add(Y);

            Form_ColumnChart.Chart.Series.Add(NewSeries);
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод удаления всех серий с диаграммы
        /// </summary>
        public void ClearSeries()
        {
            Form_ColumnChart.Chart.Series.Clear();
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод удаляет точку данных по имени
        /// </summary>
        /// <param name="Name">Имя точки</param>
        public void RemoveSeries(string Name)
        {
            int Index = 0;

            for (int i = 0; i < SeriesName.Count; i++)
            {
                if (Name == SeriesName[i])
                {
                    Index = i;
                }
            }

            Form_ColumnChart.Chart.Series.RemoveAt(Index);
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод добавления названия к диаграмме
        /// </summary>
        /// <param name="Title">Текст названия</param>
        /// <param name="Colour">Цвет названия</param>
        /// <param name="Font">Шрифт названия</param>
        public void SetTitle(string Title, Color Colour, Font Font)
        {
            Title NewTitle = new Title();

            NewTitle.Text = Title;
            NewTitle.ForeColor = Colour;
            NewTitle.Font = Font;

            Form_ColumnChart.Chart.Titles.Add(NewTitle);
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод получает или задает флаг, указывающий, отображается ли элемент в легенде.
        /// </summary>
        /// <param name="Enable">Флаг</param>
        public void SetEnableLegend(bool Enable)
        {
            for (int i = 0; i < SeriesName.Count; i++)
            {
                Form_ColumnChart.Chart.Series[i].IsVisibleInLegend = Enable;
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод отображения формы
        /// </summary>
        public void ShowChart()
        {
            Form_ColumnChart.Show();
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод закрытия диаграммы
        /// </summary>
        public void CloseChart()
        {
            Form_ColumnChart.Close();
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод копирования данных в Excel и отображение диаграммы
        /// </summary>
        public void ExportToExcel()
        {
            Excel.Application XlApp = new Excel.Application();
            Excel.Workbook XlWorkBook = XlApp.Workbooks.Add(Type.Missing);
            Excel.Worksheet XlWorkSheet = XlWorkBook.Worksheets.get_Item(1);

            // Заполнение ячеек Excel данными

            List<double> AllXValues = new List<double>();   // Список всех значений оси X

            for (double i = XValue.Min(); i <= XValue.Max(); i++)
            {
                AllXValues.Add(i);
            }

            // Цикл заполнения ячеек сериями
            for (int i = 2; i < SeriesName.Count + 2; i++)
            {
                XlWorkSheet.Cells[1][i] = SeriesName[i - 2];
            }

            // Цикл заполнения ячеек оси X
            for (int i = 2; i < AllXValues.Count + 2; i++)
            {
                XlWorkSheet.Cells[i][1] = AllXValues[i - 2];
            }

            // Цикл заполнения ячеек оси Y
            for (int i = 2; i < SeriesName.Count + 2; i++)
            {
                for (int j = 2; j < AllXValues.Count + 2; j++)
                {
                    if (XValue[i - 2] == AllXValues[j - 2])
                    {
                        XlWorkSheet.Cells[j][i] = YValue[i - 2];
                    }
                }
            }

            // Добавление диаграммы в документ Excel
            Excel.Chart XlChart = XlApp.Charts.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            XlChart.Activate();
            XlApp.ActiveChart.ChartType = Excel.XlChartType.xlColumnClustered;

            // Добавление на диаграмму Excel названия, с аналогичными настройкам, что и в Windows Forms
            XlApp.ActiveChart.HasTitle = true;
            XlApp.ActiveChart.ChartTitle.Text = Form_ColumnChart.Chart.Titles[0].Text;
            XlApp.ActiveChart.ChartTitle.Font.Size = Form_ColumnChart.Chart.Titles[0].Font.Size;
            XlApp.ActiveChart.ChartTitle.Font.Name = Form_ColumnChart.Chart.Titles[0].Font.Name;
            XlApp.ActiveChart.ChartTitle.Font.Color = Form_ColumnChart.Chart.Titles[0].ForeColor;

            // Добавление на диаграмму Excel серий, с аналогичными настройкам, что и в Windows Forms
            Excel.SeriesCollection XlSeriesCollection = XlApp.ActiveChart.SeriesCollection(Type.Missing);

            for (int i = 1; i <= XlSeriesCollection.Count; i++)
            {
                XlApp.ActiveChart.Legend.LegendEntries(i).Font.Size = 20;
                XlApp.ActiveChart.Legend.LegendEntries(i).Font.Name = Form_ColumnChart.Chart.Series[i - 1].Font;
                XlApp.ActiveChart.Legend.LegendEntries(i).LegendKey.Interior.Color = Form_ColumnChart.Chart.Series[i - 1].Color;
            }

            // Сохранение и закрытие документа Excel
            XlWorkBook.SaveAs(@"C:\Users\Andrey\Desktop\Chart.xls", Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            XlWorkBook.Close();
            XlApp.Quit();
            foreach (var proc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
            {
                proc.Kill();
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод сохранения диаграммы в указанный файл
        /// </summary>
        /// <param name="FileName">Имя файла</param>
        public void ExportToPng(string FileName)
        {
            Form_ColumnChart.Chart.SaveImage(FileName, ChartImageFormat.Png);
        }
        // ------------------------------------------------------------------------------------------------------------
    }
}