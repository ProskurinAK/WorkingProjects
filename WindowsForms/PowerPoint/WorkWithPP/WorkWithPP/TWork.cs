using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Core = Microsoft.Office.Core;
// ************************************************************************************************************

namespace WorkWithPP
{
    class TWork
    {
        public void CreatePresentation()
        {
            PowerPoint.Application PptApp = new PowerPoint.Application();
            PowerPoint.Presentation PptPresentation = PptApp.Presentations.Add();
            PowerPoint.Slides PptSlides = PptPresentation.Slides;

            // Add Slides

            PowerPoint.Slide FirstSlide = PptSlides.Add(1, PowerPoint.PpSlideLayout.ppLayoutBlank);
            PowerPoint.Slide SecondSlide = PptSlides.Add(2, PowerPoint.PpSlideLayout.ppLayoutBlank);
            PowerPoint.Slide ThirdSlide = PptSlides.Add(3, PowerPoint.PpSlideLayout.ppLayoutBlank);

            // Add Text

            FirstSlide.Shapes.AddTextbox(Core.MsoTextOrientation.msoTextOrientationHorizontal, 100, 100, 150, 200).TextFrame.TextRange.Text = "New Text";

            // Add Picture

            FirstSlide.Shapes.AddPicture(@"C:\Users\Andrey\Desktop\Image\1.png", Core.MsoTriState.msoFalse, Core.MsoTriState.msoTrue, 200, 250 , 300, 150);

            // Add Table

            SecondSlide.Shapes.AddTable(3, 5, 150, 150, 500, 200);

            // Add Chart

            // ThirdSlide.Shapes.AddChart2(-1, Core.XlChartType.xlPie, 100, 100, 500, 300, false);
            ThirdSlide.Shapes.AddChart(Core.XlChartType.xlPie, 200, 200, 500, 300);

            // Save and Close

            Console.ReadLine();
            // PptPresentation.SaveAs(@"C:\Users\Andrey\Desktop\tmp\pres", PowerPoint.PpSaveAsFileType.ppSaveAsDefault, Core.MsoTriState.msoTriStateMixed);
            PptPresentation.Close();
            PptApp.Quit();
            foreach (var proc in System.Diagnostics.Process.GetProcessesByName("POWERPNT"))
            {
                proc.Kill();
            }
        }
    }
}
