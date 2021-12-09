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

namespace ColumnChart
{
    public partial class UTest : Form
    {
        public UTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TColumnChart ColumnChart = new TColumnChart();
            Font Font = new Font("Monotype Corsiva", 20f);

            ColumnChart.AddSeries("FirstSeries", 2, 7, Color.Red, Font);
            ColumnChart.AddSeries("SecondSeries", 5, 5, Color.Yellow, Font);
            ColumnChart.AddSeries("ThirdSeries", 8, 6, Color.Purple, Font);
            ColumnChart.AddSeries("FourthSeries", -2, 4, Color.Green, Font);
            ColumnChart.AddSeries("FithSeries", 5, 9, Color.Blue, Font);
            // ColumnChart.ClearSeries();
            // ColumnChart.RemoveSeries("SecondSeries");
            ColumnChart.SetTitle("Title", Color.DarkRed, Font);
            // ColumnChart.SetEnableLegend(false);
            ColumnChart.ShowChart();
            // ColumnChart.CloseChart();
            ColumnChart.ExportToExcel();
            // ColumnChart.ExportToPng(@"C:\Users\Andrey\Desktop\Img\1.png");
        }
    }
}
