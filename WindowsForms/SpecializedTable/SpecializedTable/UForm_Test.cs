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

namespace SpecializedTable
{
    public partial class UForm_Test : Form
    {
        public UForm_Test()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TTableDynamicList TableDynamicList = new TTableDynamicList();

            TableDynamicList.CreateDataGrid();
            TableDynamicList.CustomizeDataGrid();

            TableDynamicList.FillNestedRowsDictionary();

            // TableDynamicList.TableReorder();
            // TableDynamicList.HideColumns("Name", "Age");
            // TableDynamicList.FreezeFirstRowAndColumn();
            TableDynamicList.RollUpAndExpandRows();
            // TableDynamicList.ExportToExcel(@"C:\Users\Andrey\Desktop\TextFile.xls");

            TableDynamicList.ShowForm();
        }
    }
}
