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
            list = func.Crawler_Khoahocdotvietjack(link);
            foreach (var item in list)
            {
                richTextBox1.Text = item.question + "\n";
            }
        }
    }
}
