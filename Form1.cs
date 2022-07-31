using System;
using System.Collections.Generic;
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
            //List<Question> list = func.Crawler_DeThi_Tracnghiemdotnet(link);
            //foreach (var item in list)
            //{
            //    richTextBox1.Text += item.ans1 + "\n\n\n";
            //}

            richTextBox1.Text = func.Crawler_cungthionline(link);
            
        }
    }
}
