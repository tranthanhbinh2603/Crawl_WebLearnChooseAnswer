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
            //List<Question> list = func.Crawler_TuHoc365(link);
            //foreach (var item in list)
            //{
            //    richTextBox1.Text += item.question + "\n";
            //    richTextBox1.Text += item.ans1 + "\n" + item.ans2 + "\n" + item.ans3 + "\n" + item.ans4 + "\n";
            //    richTextBox1.Text += item.correctAns + "\n\n";
            //}

            richTextBox1.Text = func.Crawler_TuHoc365(link);

            //https://cungthi.online/cau-hoi/chuong-trinh-viet-bang-ngon-ngu-bac-cao-khong-co-dac--279970-9414.html
            //richTextBox1.Text = func.Get_TrueAnswer_cungthionline(link);

            //Func_Utilities.ExportQuestion("Hacker", list);
        }
    }
}
