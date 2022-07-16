using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crawl_WebLearnChooseAnswer
{
    class Func_Utilities
    {
        public static void ExportQuestion(string filename = "Result", List<Question> listque = null)
        {
            StreamWriter stream = new StreamWriter(Application.StartupPath + filename + ".txt");
            foreach (var item in listque)
            {
                stream.WriteLine(item.question + "\t\t2\t" + item.ans1 + "\t" + item.ans2 + "\t" + item.ans3 + "\t" + item.ans4 + "\t" + item.ans5 + "\t" + Func_Utilities.CovertAnswer(item));
            }            
            stream.Close();
        }

        public static string CovertAnswer(Question q)
        {
            if (q.ans3 == "")
            {
                if (q.correctAns.ToUpper() == "A") return "1 0";
                if (q.correctAns.ToUpper() == "B") return "0 1";
            }                
            if (q.ans4 == "")
            {
                if (q.correctAns.ToUpper() == "A") return "1 0 0";
                if (q.correctAns.ToUpper() == "B") return "0 1 0";
                if (q.correctAns.ToUpper() == "C") return "0 0 1";
            }
            if (q.ans5 == "")
            {
                if (q.correctAns.ToUpper() == "A") return "1 0 0 0";
                if (q.correctAns.ToUpper() == "B") return "0 1 0 0";
                if (q.correctAns.ToUpper() == "C") return "0 0 1 0";
                if (q.correctAns.ToUpper() == "D") return "0 0 0 1";
            }
            if (q.ans5 != "")
            {
                if (q.correctAns.ToUpper() == "A") return "1 0 0 0 0";
                if (q.correctAns.ToUpper() == "B") return "0 1 0 0 0";
                if (q.correctAns.ToUpper() == "C") return "0 0 1 0 0";
                if (q.correctAns.ToUpper() == "D") return "0 0 0 1 0";
                if (q.correctAns.ToUpper() == "E") return "0 0 0 0 1";
            }
            return "Error";
        }
    }
}
