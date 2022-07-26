using System;
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
            string link = textBox1.Text;

            List<Question> list = new List<Question>();
            list = func.Crawler_DeThi_Hoc247(link);
            Func_Utilities.ExportQuestion("Test", list);

            //richTextBox1.Text = func.Crawler_DeThi_Hoc247("https://hoc247.net/de-thi-hk2-mon-tin-hoc-11-nam-2021-2022-truong-thpt-tran-phu-ktdt15934.html");
        }
    }
}
