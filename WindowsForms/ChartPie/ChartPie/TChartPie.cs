using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;  // Содержит методы и свойства для элемента управления форм Windows Chart.
using Excel = Microsoft.Office.Interop.Excel;   // Пространство имён для работы с Excel(Необходимо подключить ссылку в обозревателе решений)
// ************************************************************************************************************

namespace ChartPie
{
    class TChartPie
    {
        UForm_ChartPie Form_ChartPie = new UForm_ChartPie();
        Series NewSeries;

        List<string> PointsName = new List<string>();   // Список для хранения имён точек данных(используется при передачи в Excel)
        List<double> PointsValue = new List<double>();  // Список для хранения размера точек данных(используется при передачи в Excel)
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод добавления серии в диаграмму
        /// </summary>
        /// <param name="Name">Имя серии</param>
        /// <param name="Font">Шрифт серии</param>
        public void AddSeries(string Name, Font Font)
        {
            NewSeries = new Series(Name);
            NewSeries.Font = Font;
            NewSeries.ChartType = SeriesChartType.Pie;
            Form_ChartPie.Chart.Series.Add(NewSeries);
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод добавления точки данных на диаграмму
        /// </summary>
        /// <param name="Name">Имя точки данных</param>
        /// <param name="Size">Размер точки данных</param>
        /// <param name="Color">Цвет области на схеме</param>
        /// <param name="Index">Индекс точки данных в коллекции DataPointCollection</param>
        public void AddDataPoint(string Name, int Size, Color Color, int Index)
        {
            NewSeries.Points.AddXY(Name, Size);     // Добавление точки DataPoint в коллекцию DataPointCollection
            NewSeries.Points[Index].Color = Color;  // Для присваивания цвета необходимо обращаться к точке из коллекции DataPointCollection, по индексу

            PointsName.Add(Name);
            PointsValue.Add(Size);

            // Альтернативой методу AddXY является создание объекта DataPoint(Но его полю X нельзя присваивать значение string)
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод удаления всех серий с диаграммы 
        /// </summary>
        public void ClearSeries()
        {
            Form_ChartPie.Chart.Series.Clear();
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод удаляет точку данных по имени 
        /// </summary>
        /// <param name="Name">Имя точки</param>
        public void RemovePoint(string Name)
        {
            int Index = 0;

            for (int i = 0; i < PointsName.Count; i++)
            {
                if (Name == PointsName[i])
                {
                    Index = i;
                }
            }

            NewSeries.Points.RemoveAt(Index);
            // Form_ChartPie.Chart.Series.RemoveAt(Index); // Метод удаления серии по имени
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

            Form_ChartPie.Chart.Titles.Add(NewTitle);
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод получает или задает флаг, указывающий, отображается ли элемент в легенде.
        /// </summary>
        /// <param name="Enable">Флаг</param>
        public void SetEnableLegend(bool Enable)
        {
            Form_ChartPie.Chart.Series["FirstSeries"].IsVisibleInLegend = Enable;
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод отображения формы
        /// </summary>
        public void ShowChart()
        {
            Form_ChartPie.Show();
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод закрытия формы
        /// </summary>
        public void CloseChart()
        {
            Form_ChartPie.Close();
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод копирования данных в Excel и отображение диаграммы
        /// </summary>
        public void ExportToExcel()
        {
            Excel.Application XlApp = new Excel.Application(); // Запуск приложения Excel
            Excel.Workbook XlWorkBook = XlApp.Workbooks.Add(Type.Missing); // Добавление рабочей книги в приложение Excel
            Excel.Worksheet XlWorkSheet = XlWorkBook.Worksheets.get_Item(1); // // Получение первого листа документа(счёт начинается с 1)

            // XlApp.Visible = true;   // Отображение приложения Excel
            // XlApp.Workbooks.Open(@"C:\Users\Andrey\Desktop\TestChart.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing); // Открытие существующего документа

            // Заполнение ячеек Excel данными
            for (int i = 1; i < PointsName.Count + 1; i++)
            {
                XlWorkSheet.Cells[1][i] = PointsName[i - 1];
            }
            for (int i = 1; i < PointsValue.Count + 1; i++)
            {
                XlWorkSheet.Cells[2][i] = PointsValue[i - 1];
            }

            // Добавление диаграммы в документ Excel
            Excel.Chart XlChart = XlApp.Charts.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            XlChart.Activate();
            // XlChart.Select(Type.Missing);
            XlApp.ActiveChart.ChartType = Excel.XlChartType.xlPie;

            // Добавление на диаграмму Excel названия, с аналогичными настройкам, что и в Windows Forms
            XlApp.ActiveChart.HasTitle = true;
            XlApp.ActiveChart.ChartTitle.Text = Form_ChartPie.Chart.Titles[0].Text;
            XlApp.ActiveChart.ChartTitle.Font.Size = Form_ChartPie.Chart.Titles[0].Font.Size;
            XlApp.ActiveChart.ChartTitle.Font.Name = Form_ChartPie.Chart.Titles[0].Font.Name;
            XlApp.ActiveChart.ChartTitle.Font.Color = Form_ChartPie.Chart.Titles[0].ForeColor;

            // Добавление на диаграмму Excel легенды, с аналогичными настройками, что и в Windows Forms
            XlApp.ActiveChart.HasLegend = true;
            XlApp.ActiveChart.Legend.Position = Excel.XlLegendPosition.xlLegendPositionRight;

            for (int i = 1; i <= Form_ChartPie.Chart.Series[0].Points.Count; i++)
            {
                XlApp.ActiveChart.Legend.LegendEntries(i).Font.Size = 25;
                // XlApp.ActiveChart.Legend.LegendEntries(i).Font.Name = Form_ChartPie.Chart.Series[0].Font;
                XlApp.ActiveChart.Legend.LegendEntries(i).LegendKey.Interior.Color = Form_ChartPie.Chart.Series[0].Points[i - 1].Color;
            }

            // Сохранение и закрытие документа Excel
            XlWorkBook.SaveAs(@"C:\Users\Andrey\Desktop\Chart.xls", Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing); // Сохранение документа
            XlWorkBook.Close();
            XlApp.Quit(); // Закрытие приложения Excel
            foreach (var proc in System.Diagnostics.Process.GetProcessesByName("EXCEL")) // Цикл уничтожения процессов Excel в диспетчере задач
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
            Form_ChartPie.Chart.SaveImage(FileName, ChartImageFormat.Png);
        }
        // ------------------------------------------------------------------------------------------------------------
    }
}