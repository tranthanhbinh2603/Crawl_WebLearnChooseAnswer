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
            string title = "";
            List<Question> list = func.Crawler_Tech12h(link, ref title);
            /*foreach (var item in list)
            {
                richTextBox1.Text += item.question + "\n";
                richTextBox1.Text += item.ans1 + "\n" + item.ans2 + "\n" + item.ans3 + "\n" + item.ans4 + "\n";
                richTextBox1.Text += item.correctAns + "\n\n";
            }*/

            //richTextBox1.Text = func.Crawler_DocTaiLieu(link);

            //https://cunghocvui.com/de-thi-kiem-tra/y97vzx94-trac-nghiem-toan-11-bai-3-mot-so-phuong-trinh-luong-giac-thuong-gap.html
            //richTextBox1.Text = func.Get_TrueAnswer_cungthionline(link);

            //Func_Utilities.ExportQuestion("111", list);
        }
    }
}
