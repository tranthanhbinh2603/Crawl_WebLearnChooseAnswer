﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crawl_WebLearnChooseAnswer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Func_WebRequest func = new Func_WebRequest();
            string link = "https://khoahoc.vietjack.com/thi-online/trac-nghiem-tieng-anh-11-unit-11-sources-of-energy-co-dap-an/69869/ket-qua";
            richTextBox1.Text = func.Crawler_Khoahocdotvietjack(link);
            
        }
    }
}
