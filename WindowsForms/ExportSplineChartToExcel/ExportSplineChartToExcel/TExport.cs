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
using Core = Microsoft.Office.Core;
// ************************************************************************************************************

namespace ExportSplineChartToExcel
{
    class TExport
    {
        UForm_SplineChart SplineChart = new UForm_SplineChart();

        List<string> SeriesName = new List<string>();   // Список для хранения имён серий
        List<double> XValue = new List<double>();   // Список для храения Поля X точки даннных
        List<List<double>> YValues = new List<List<double>>();  // Список для храения Поля Y точки даннных

        int CountOfSeries = 0;  // Переменная для хранения количества серий
        List<Core.MsoLineDashStyle> ExcelLineStyle = new List<Core.MsoLineDashStyle>(); // Список для хранения стиля линий при передаче в Excel

        List<Excel.XlMarkerStyle> ExcelMarkerStyle = new List<Excel.XlMarkerStyle>();   // Список для хранения стиля маркеров при передаче в Excel

        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод преобразования стиля линий Windows forms в стиль линий Excel
        /// </summary>
        /// <param name="LineStyle">Стиль линии в Windows Forms</param>
        public void ConvertLineStyle(ChartDashStyle LineStyle)
        {
            switch (LineStyle)
            {
                case ChartDashStyle.Dash:
                    ExcelLineStyle.Add(Core.MsoLineDashStyle.msoLineDash);
                    break;
                case ChartDashStyle.DashDot:
                    ExcelLineStyle.Add(Core.MsoLineDashStyle.msoLineDashDot);
                    break;
                case ChartDashStyle.DashDotDot:
                    ExcelLineStyle.Add(Core.MsoLineDashStyle.msoLineDashDotDot);
                    break;
                case ChartDashStyle.Dot:
                    ExcelLineStyle.Add(Core.MsoLineDashStyle.msoLineRoundDot);
                    break;
                case ChartDashStyle.Solid:
                    ExcelLineStyle.Add(Core.MsoLineDashStyle.msoLineSolid);
                    break;
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод добавления серии и маркеров в диаграмму
        /// </summary>
        /// <param name="Name">Имя серии</param>
        /// <param name="Colour">Цвет серии</param>
        /// <param name="LineWidth">Толщина Линии</param>
        /// <param name="LineStyle">Стиль Линии</param>
        /// <param name="MarkStyle">Стиль маркера</param>
        /// <param name="MarkColor">Цвет маркера</param>
        /// <param name="MarkSize">Размер маркера</param>
        public void AddSeries(string Name, Color Colour, int LineWidth, ChartDashStyle LineStyle, MarkerStyle MarkStyle, Color MarkColor, int MarkSize)
        {
            Series NewSeries = new Series(Name);
            NewSeries.ChartType = SeriesChartType.Spline;
            NewSeries.Color = Colour;
            NewSeries.BorderWidth = LineWidth;
            NewSeries.BorderDashStyle = LineStyle;
            ConvertLineStyle(LineStyle);

            NewSeries.MarkerStyle = MarkStyle;
            NewSeries.MarkerColor = MarkColor;
            NewSeries.MarkerSize = MarkSize;

            // преобразования стиля маркеров Windows forms в стиль маркеров Excel
            switch (MarkStyle)
            {
                case MarkerStyle.Circle:
                    ExcelMarkerStyle.Add(Excel.XlMarkerStyle.xlMarkerStyleCircle);
                    break;
                case MarkerStyle.Diamond:
                    ExcelMarkerStyle.Add(Excel.XlMarkerStyle.xlMarkerStyleDiamond);
                    break;
                case MarkerStyle.Square:
                    ExcelMarkerStyle.Add(Excel.XlMarkerStyle.xlMarkerStyleSquare);
                    break;
                case MarkerStyle.Triangle:
                    ExcelMarkerStyle.Add(Excel.XlMarkerStyle.xlMarkerStyleTriangle);
                    break;
            }

            List<double> YValue = new List<double>();

            Random Rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                int X = i;
                int Y = Rnd.Next(0, 10);

                XValue.Add(X);
                YValue.Add(Y);

                NewSeries.Points.AddXY(X, Y);
                Console.WriteLine("X - " + X + "\tY - " + Y);
            }

            YValues.Add(YValue);
            SeriesName.Add(Name);

            SplineChart.Chart.Series.Add(NewSeries);
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Добавление легенды к диаграмме
        /// </summary>
        /// <param name="Font">Шрифт легенды</param>
        /// <param name="Colour">Цвет фона</param>
        public void SetLegend(Font Font, Color Colour)
        {
            Legend Legend = new Legend("Legend");

            Legend.Font = Font;
            Legend.BackColor = Colour;

            SplineChart.Chart.Legends.Add(Legend);

            for (int i = 0; i < SeriesName.Count; i++)
            {
                SplineChart.Chart.Series[i].Legend = "Legend";
            }
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
            NewTitle.Position.X = 50;
            NewTitle.Position.Y = 2;


            SplineChart.Chart.Titles.Add(NewTitle);
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод настройки осей графика
        /// </summary>
        /// <param name="XAxisLineStyle">Стиль оси X</param>
        /// <param name="YAxisLineStyle">Стиль оси Y</param>
        public void SetAxec(ChartDashStyle XAxisLineStyle, ChartDashStyle YAxisLineStyle)
        {
            SplineChart.Chart.ChartAreas[0].AxisX.Title = "Axis X";
            SplineChart.Chart.ChartAreas[0].AxisX.LineColor = Color.Red;
            SplineChart.Chart.ChartAreas[0].AxisX.LineWidth = 3;
            SplineChart.Chart.ChartAreas[0].AxisX.LineDashStyle = XAxisLineStyle;
            ConvertLineStyle(XAxisLineStyle);
            SplineChart.Chart.ChartAreas[0].AxisX.Minimum = -1;
            SplineChart.Chart.ChartAreas[0].AxisX.Maximum = 10;
            SplineChart.Chart.ChartAreas[0].AxisX.Interval = 1;

            SplineChart.Chart.ChartAreas[0].AxisY.Title = "Axis Y";
            SplineChart.Chart.ChartAreas[0].AxisY.LineColor = Color.Red;
            SplineChart.Chart.ChartAreas[0].AxisY.LineWidth = 3;
            SplineChart.Chart.ChartAreas[0].AxisY.LineDashStyle = YAxisLineStyle;
            ConvertLineStyle(YAxisLineStyle);
            SplineChart.Chart.ChartAreas[0].AxisY.Minimum = 0;
            SplineChart.Chart.ChartAreas[0].AxisY.Maximum = 10;
            SplineChart.Chart.ChartAreas[0].AxisY.Interval = 1;
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод настройки фона графика
        /// </summary>
        /// <param name="XAxisOfGrid">Стиль оси X сетки фона</param>
        /// <param name="YAxisOfGrid">Стиль оси Y сетки фона</param>
        public void SetBackGround(ChartDashStyle XAxisOfGrid, ChartDashStyle YAxisOfGrid)
        {
            SplineChart.Chart.ChartAreas[0].BackColor = Color.Black;

            SplineChart.Chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LawnGreen;
            SplineChart.Chart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 2;
            SplineChart.Chart.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = XAxisOfGrid;
            ConvertLineStyle(XAxisOfGrid);

            SplineChart.Chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LawnGreen;
            SplineChart.Chart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 4;
            SplineChart.Chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = YAxisOfGrid;
            ConvertLineStyle(YAxisOfGrid);
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод отображения формы
        /// </summary>
        public void ShowChart()
        {
            SplineChart.Show();
        }
        // ------------------------------------------------------------------------------------------------------------
        public void ExportToExcel()
        {
            Excel.Application XlApp = new Excel.Application();
            Excel.Workbook XlWorkBook = XlApp.Workbooks.Add(Type.Missing);
            Excel.Worksheet XlWorkSheet = XlWorkBook.Worksheets.get_Item(1);

            // Заполнение ячеек Excel данными

            // Цикл заполнения ячеек сериями
            for (int i = 2; i < SeriesName.Count + 2; i++)
            {
                XlWorkSheet.Cells[1][i] = SeriesName[i - 2];
            }

            List<double> AllXValues = new List<double>();   // Список всех значений оси X
            for (double i = XValue.Min(); i <= XValue.Max(); i++)
            {
                AllXValues.Add(i);
            }

            // Цикл заполнения ячеек оси X
            for (int i = 2; i < AllXValues.Count + 2; i++)
            {
                XlWorkSheet.Cells[i][1] = AllXValues[i - 2];
            }

            // Заполнение ячеек серий данными(Y)
            List<double> TmpYValue = new List<double>();

            for (int i = 2; i < YValues.Count + 2; i++)
            {
                TmpYValue = YValues[i - 2];
                for (int j = 2; j < YValues[i - 2].Count + 2; j++)
                {
                    XlWorkSheet.Cells[j][i] = TmpYValue[j - 2];
                }
            }

            // Добавление диаграммы в документ Excel
            Excel.Chart XlChart = XlApp.Charts.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            XlChart.Activate();
            XlApp.ActiveChart.ChartType = Excel.XlChartType.xlLine;

            for (int i = 0; i < SeriesName.Count; i++)
            {
                Excel.Series XlSeries = XlApp.ActiveChart.SeriesCollection(i + 1);

                // Добавление на диаграмму Excel серий, с аналогичными настройкам, что и в Windows Forms

                XlSeries.Format.Line.ForeColor.RGB = ColorTranslator.ToOle(SplineChart.Chart.Series[i].Color);
                XlSeries.Format.Line.Weight = SplineChart.Chart.Series[i].BorderWidth;
                XlSeries.Format.Line.DashStyle = ExcelLineStyle[i];

                // Добавление на диаграмму Excel маркеров для серий, с аналогичными настройкам, что и в Windows Forms

                XlSeries.MarkerStyle = ExcelMarkerStyle[i];
                XlSeries.MarkerForegroundColor = ColorTranslator.ToOle(SplineChart.Chart.Series[i].MarkerColor);
                XlSeries.MarkerBackgroundColor = ColorTranslator.ToOle(SplineChart.Chart.Series[i].MarkerColor);
                XlSeries.MarkerSize = SplineChart.Chart.Series[i].MarkerSize;

                CountOfSeries++;
            }

            // Добавление на диаграмму Excel легенды, с аналогичными настройкам, что и в Windows Forms

            XlApp.ActiveChart.Legend.Format.Fill.ForeColor.RGB = ColorTranslator.ToOle(Color.Yellow);
            XlApp.ActiveChart.Legend.Font.Size = SplineChart.Chart.Legends[1].Font.Size;
            XlApp.ActiveChart.Legend.Font.Name = SplineChart.Chart.Legends[1].Font.Name;

            // Добавление на диаграмму Excel названия, с аналогичными настройкам, что и в Windows Forms

            XlApp.ActiveChart.HasTitle = true;
            XlApp.ActiveChart.ChartTitle.Text = SplineChart.Chart.Titles[0].Text;
            XlApp.ActiveChart.ChartTitle.Font.Size = SplineChart.Chart.Titles[0].Font.Size;
            XlApp.ActiveChart.ChartTitle.Font.Name = SplineChart.Chart.Titles[0].Font.Name;
            XlApp.ActiveChart.ChartTitle.Font.Color = SplineChart.Chart.Titles[0].ForeColor;
            XlApp.ActiveChart.ChartTitle.Left = SplineChart.Chart.Titles[0].Position.X;
            XlApp.ActiveChart.ChartTitle.Top = SplineChart.Chart.Titles[0].Position.Y;

            // Добавление на диаграмму Excel осей, с аналогичными настройкам, что и в Windows Forms

            Excel.Axis XAxis = (Excel.Axis)XlApp.ActiveChart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
            Excel.Axis YAxis = (Excel.Axis)XlApp.ActiveChart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);

            XAxis.HasTitle = true;
            XAxis.AxisTitle.Text = SplineChart.Chart.ChartAreas[0].AxisX.Title;
            XAxis.Border.Weight = SplineChart.Chart.ChartAreas[0].AxisX.LineWidth;
            XAxis.Border.Color = SplineChart.Chart.ChartAreas[0].AxisX.LineColor;
            XAxis.Format.Line.DashStyle = ExcelLineStyle[CountOfSeries++];

            YAxis.MinimumScale = SplineChart.Chart.ChartAreas[0].AxisY.Minimum;
            YAxis.MaximumScale = SplineChart.Chart.ChartAreas[0].AxisY.Maximum;
            YAxis.MajorUnit = SplineChart.Chart.ChartAreas[0].AxisY.Interval;
            YAxis.HasTitle = true;
            YAxis.AxisTitle.Text = SplineChart.Chart.ChartAreas[0].AxisY.Title;
            YAxis.Border.Weight = SplineChart.Chart.ChartAreas[0].AxisY.LineWidth;
            YAxis.Border.Color = SplineChart.Chart.ChartAreas[0].AxisY.LineColor;
            YAxis.Format.Line.DashStyle = ExcelLineStyle[CountOfSeries++];

            // Добавление на диаграмму Excel фона, с аналогичными настройкам, что и в Windows Forms

            XlApp.ActiveChart.PlotArea.Format.Fill.ForeColor.RGB = ColorTranslator.ToOle(Color.Black);

            XlApp.ActiveChart.Axes(Excel.XlAxisType.xlCategory).HasMinorGridlines = true;
            XlApp.ActiveChart.Axes(Excel.XlAxisType.xlCategory).MinorGridLines.Border.Color = SplineChart.Chart.ChartAreas[0].AxisX.MajorGrid.LineColor;
            XlApp.ActiveChart.Axes(Excel.XlAxisType.xlCategory).MinorGridLines.Format.Line.DashStyle = ExcelLineStyle[CountOfSeries++];
            XlApp.ActiveChart.Axes(Excel.XlAxisType.xlCategory).MinorGridLines.Border.Weight = SplineChart.Chart.ChartAreas[0].AxisX.MajorGrid.LineWidth;

            XlApp.ActiveChart.Axes(Excel.XlAxisType.xlValue).HasMajorGridlines = true;
            XlApp.ActiveChart.Axes(Excel.XlAxisType.xlValue).MajorGridLines.Border.Color = SplineChart.Chart.ChartAreas[0].AxisY.MajorGrid.LineColor;
            XlApp.ActiveChart.Axes(Excel.XlAxisType.xlValue).MajorGridLines.Format.Line.DashStyle = ExcelLineStyle[CountOfSeries];
            XlApp.ActiveChart.Axes(Excel.XlAxisType.xlValue).MajorGridLines.Border.Weight = SplineChart.Chart.ChartAreas[0].AxisY.MajorGrid.LineWidth;

            XlWorkBook.SaveAs(@"C:\Users\Andrey\Desktop\Chart.xls", Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            XlWorkBook.Close();
            XlApp.Quit();
            foreach (var proc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
            {
                proc.Kill();
            }
        }
    }
}
