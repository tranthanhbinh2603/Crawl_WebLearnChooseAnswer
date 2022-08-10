using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Crawl_WebLearnChooseAnswer
{
    class Func_Utilities
    {
        public static void ExportQuestion(string filename = "Result", List<Question> listque = null)
        {
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FINAL_RESULT\\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            StreamWriter stream = new StreamWriter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FINAL_RESULT\\" + filename + ".txt");
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
            return "0 0 0 0";
        }

        public static int crawlerOneWeb(string link)
        {
            try
            {

                Uri myUri = new Uri(link);
                string host = myUri.Host;
                if (host == "khoahoc.vietjack.com")
                {
                    Func_WebRequest func = new Func_WebRequest();
                    string title = "";
                    List<Question> list = func.Crawler_Khoahocdotvietjack(link, ref title);
                    title = title.Replace(":", "").Replace("|", "");
                    ExportQuestion(title, list);
                    return 1;
                }
                if (host == "hoc247.net")
                {
                    if (link.Contains("de-thi"))
                    {
                        Func_WebRequest func = new Func_WebRequest();
                        string title = "";
                        List<Question> list = func.Crawler_DeThi_Hoc247(link, ref title);
                        title = title.Replace(":", "").Replace("|", "");
                        ExportQuestion(title, list);
                    }
                    else
                    {
                        Func_WebRequest func = new Func_WebRequest();
                        string title = "";
                        List<Question> list = func.Crawler_Hoc247(link, ref title);
                        title = title.Replace(":", "").Replace("|", "");
                        ExportQuestion(title, list);
                    }
                    return 1;
                }
                if (host == "tracnghiem.net")
                {
                    if (link.Contains("de-thi"))
                    {
                        Func_WebRequest func = new Func_WebRequest();
                        string title = "";
                        List<Question> list = func.Crawler_DeThi_Tracnghiemdotnet(link, ref title);
                        title = title.Replace(":", "").Replace("|", "");
                        ExportQuestion(title, list);
                    }
                    else
                    {
                        Func_WebRequest func = new Func_WebRequest();
                        string title = "";
                        List<Question> list = func.Crawler_Tracnghiemdotnet(link, ref title);
                        title = title.Replace(":", "").Replace("|", "");
                        ExportQuestion(title, list);
                    }
                    return 1;
                }
                if (host == "cungthi.online")
                {
                    Func_WebRequest func = new Func_WebRequest();
                    string title = "";
                    List<Question> list = func.Crawler_cungthionline(link, ref title);
                    title = title.Replace(":", "").Replace("|", "");
                    ExportQuestion(title, list);
                    return 1;
                }
                if (host == "tuhoc365.vn")
                {
                    Func_WebRequest func = new Func_WebRequest();
                    string title = "";
                    List<Question> list = func.Crawler_TuHoc365(link, ref title);
                    title = title.Replace(":", "").Replace("|", "");
                    ExportQuestion(title, list);
                    return 1;
                }
                if (host == "cunghocvui.com")
                {
                    Func_WebRequest func = new Func_WebRequest();
                    string title = "";
                    List<Question> list = func.Crawler_CungHocVui(link, ref title);
                    title = title.Replace(":", "").Replace("|", "");
                    ExportQuestion(title, list);
                    return 1;
                }
                if (host == "tech12h.com")
                {
                    Func_WebRequest func = new Func_WebRequest();
                    string title = "";
                    List<Question> list = func.Crawler_Tech12h(link, ref title);
                    title = title.Replace(":", "").Replace("|", "");
                    ExportQuestion(title, list);
                    return 1;
                }
                return 0;
            }
            catch
            {
                return -1;
            }
        }
    }
}
