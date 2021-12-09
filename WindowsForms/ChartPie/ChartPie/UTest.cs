using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// ************************************************************************************************************

namespace ChartPie
{
    /// <summary>
    /// Класс описывающий форму содержащую кнопку(Вызов основной формы с диаграммой)
    /// </summary>
    public partial class UTest : Form
    {
        public UTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TChartPie ChartPie = new TChartPie();
            Font Font = new Font("Monotype Corsiva", 20f);

            ChartPie.AddSeries("FirstSeries", Font);
            ChartPie.AddDataPoint("FirstPoint", 5, Color.Gray, 0);
            ChartPie.AddDataPoint("SecondPoint", 15, Color.Yellow, 1);
            ChartPie.AddDataPoint("ThirdPoint", 15, Color.BlueViolet, 2);
            // ChartPie.ClearSeries();
            // ChartPie.RemovePoint("SecondPoint");
            ChartPie.SetTitle("My Chart", Color.DarkRed, Font);
            // ChartPie.SetEnableLegend(false);
            ChartPie.ShowChart();
            // ChartPie.CloseChart();
            // ChartPie.ExportToExcel();
            // ChartPie.ExportToPng(@"C:\Users\Andrey\Desktop\Img\Chart.png");
        }
    }
}