﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// ************************************************************************************************************

namespace WorkWithPP
{
    public partial class UTest : Form
    {
        public UTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TWork Work = new TWork();

            Work.CreatePresentation();
        }
    }
}
