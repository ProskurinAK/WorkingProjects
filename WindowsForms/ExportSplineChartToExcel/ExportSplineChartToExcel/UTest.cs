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
// ************************************************************************************************************

namespace ExportSplineChartToExcel
{
    public partial class UTest : Form
    {
        public UTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TExport Export = new TExport();
            Font Font = new Font("Monotype Corsiva", 15f);

            Export.AddSeries("FirstSeries", Color.Blue, 3, ChartDashStyle.Dash, MarkerStyle.Triangle, Color.Brown, 10);
            Console.ReadLine();
            Export.AddSeries("SecondSeries", Color.Purple, 3, ChartDashStyle.Solid, MarkerStyle.Square, Color.Brown, 10);
            Export.SetLegend(Font, Color.Yellow);
            Export.SetTitle("New Title", Color.Blue, Font);
            Export.SetAxec(ChartDashStyle.Solid, ChartDashStyle.Solid);
            Export.SetBackGround(ChartDashStyle.Solid, ChartDashStyle.Solid);
            Export.ShowChart();
            Export.ExportToExcel();
        }
    }
}
