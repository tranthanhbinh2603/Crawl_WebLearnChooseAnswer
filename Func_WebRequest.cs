using System.Collections.Generic;
using xNet;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;

namespace Crawl_WebLearnChooseAnswer
{
    class Func_WebRequest
    {
        public bool isNot404(string link)
        {
            try
            {
                xNetStandard.HttpRequest httpre = new xNetStandard.HttpRequest();
                httpre.Get(link);
                return true;
            }
            catch (xNetStandard.HttpException)
            {
                return false;
            }

        }

        public List<Question> Crawler_Khoahocdotvietjack(string link, ref string title)
        {
            #region Setting list & Bắt request
            List<Question> list = new List<Question>();
            HttpRequest httpre = new HttpRequest();
            //Cookie. Bắt buộc phải truyền vào
            httpre.Cookies = new CookieDictionary();
            StreamReader streread = new StreamReader(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\cookie_vietjack.txt");
            string res1 = streread.ReadToEnd();
            string[] cokkies = res1.Split(";");
            foreach (var item in cokkies)
            {
                string key = item.Substring(0, item.IndexOf('='));
                string value = item.Substring(item.IndexOf('=') + 1, item.Length - item.IndexOf('=') - 1);
                if (httpre.Cookies.ContainsKey(key.Trim()) == false)
                    httpre.Cookies.Add(key.Trim(), value.Trim());
            }            
            string res = WebUtility.HtmlDecode(httpre.Get(link).ToString());
            #endregion
            #region Lấy tựa đề trang web
            Regex a1 = new Regex(@"<title>(?<res>.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            title = a1.Matches(res)[0].Groups["res"].Captures[0].ToString();
            #endregion
            #region Regex lấy câu hỏi
            Regex a = new Regex(@"<div class=""question-name"">(?<q>.*?)</div>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            foreach (Match item in a.Matches(res))
            {
                foreach (Capture i in item.Groups["q"].Captures)
                {
                    string s1 = "";
                    Regex regex1 = new Regex(@">(?<q1>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match match in regex1.Matches(i.ToString()))
                    {
                        foreach (Capture i2 in match.Groups["q1"].Captures)
                        {
                            if (i2.ToString().Trim() != "") s1 += i2.ToString().Trim() + "<br>";
                        }
                    }
                    list.Add(new Question { question = s1 });
                }
            }
            #endregion
            #region Regex lấy câu trả lời
            int i1 = 0;
            Regex r2 = new Regex(@"<div class=""question-anwsews-list row"">(?<res1>.*?)<div class=""col-xs-12"">", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            foreach (Match match1 in r2.Matches(res))
            {
                foreach (Capture capture in match1.Groups["res1"].Captures)
                {
                    string s1 = "";
                    Regex regex1 = new Regex(@">(?<q1>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match match in regex1.Matches(capture.ToString()))
                    {
                        foreach (Capture i2 in match.Groups["q1"].Captures)
                        {
                            if (i2.ToString().Trim() != "") s1 += i2.ToString().Trim();
                        }
                    }
                    Regex regex2 = new Regex(@"A\.(?<a>.*?)B\.(?<b>.*?)C\.(?<c>.*?)D\.(?<d>.*?)$", RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match match2 in regex2.Matches(s1))
                    {
                        list[i1].ans1 = match2.Groups["a"].Captures[0].ToString().Trim();
                        list[i1].ans2 = match2.Groups["b"].Captures[0].ToString().Trim();
                        list[i1].ans3 = match2.Groups["c"].Captures[0].ToString().Trim();
                        list[i1].ans4 = match2.Groups["d"].Captures[0].ToString().Trim();
                    }
                    i1++;
                }
            }
            #endregion
            #region Regex lấy đáp án chính xác & return list
            i1 = 0;
            Regex reg = new Regex(@"<div class=""question-reason"">(?<res1>.*?)</div", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            foreach (Match item in reg.Matches(res))
            {
                foreach (Capture i in item.Groups["res1"].Captures)
                {
                    string s1 = "";
                    Regex regex1 = new Regex(@">(?<q1>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match match in regex1.Matches(i.ToString()))
                    {
                        foreach (Capture i2 in match.Groups["q1"].Captures)
                        {
                            if (i2.ToString().Trim() != "") s1 += i2.ToString().Trim();
                        }
                    }

                    string result = "";
                    Regex regex2 = new Regex(@"Đáp án.*?(?<1>[ABCD])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match match in regex2.Matches(s1.ToString()))
                    {
                        foreach (Capture i2 in match.Groups["1"].Captures)
                        {
                            result = i2.ToString();
                        }
                    }
                    Regex regex3 = new Regex(@"Đáp án cần chọn là.*?(?<1>[ABCD])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match match in regex3.Matches(s1.ToString()))
                    {
                        foreach (Capture i2 in match.Groups["1"].Captures)
                        {
                            result = i2.ToString();
                        }
                    }
                    list[i1].correctAns = result;
                    i1++;
                }
            }
            return list;
            #endregion
        }

        public List<Question> Crawler_Hoc247(string link, ref string title)
        {
            #region Setting list và crawler link
            List<Question> list = new List<Question>();
            xNetStandard.HttpRequest httpre = new xNetStandard.HttpRequest();
            string res = WebUtility.HtmlDecode(httpre.Get(link).ToString());
            Regex reg = new Regex(@"<li class=""box-1 lch"">.*?<a(?<Ques>.*?)</a>.*?id=""dstl(?<id>.*?)"">(?<a>.*?)</li>(?<b>.*?)</li>(?<c>.*?)</li>(?<d>.*?)</li>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            #endregion
            #region Lấy title
            Regex a1 = new Regex(@"<title>(?<res>.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            title = a1.Matches(res)[0].Groups["res"].Captures[0].ToString();
            #endregion
            #region Xử lí số liệu
            int z = 0;
            foreach (Match item in reg.Matches(res))
            {
                #region Câu hỏi
                foreach (Capture i in item.Groups["Ques"].Captures)
                {
                    string ques = "";
                    Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match item2 in reg2.Matches(i.ToString()))
                        foreach (Capture i2 in item2.Groups["que"].Captures)
                            if ((i2.ToString().Trim() != " ") && (i2.ToString().Trim().Contains("\t") == false)) ques += i2.ToString().Trim() + " ";
                    list.Add(new Question() { question = ques.Trim() });
                }
                #endregion
                #region Cầu trả lời đúng                
                foreach (Capture i in item.Groups["id"].Captures)
                {
                    xNetStandard.HttpRequest http = new xNetStandard.HttpRequest();
                    if (http.Post("https://hoc247.net/test/checkquestion?question_id=" + i.ToString() + "&answer_choice=1").ToString().Contains(@"ok") == false)
                        list[z].correctAns = "A";
                    else if (http.Post("https://hoc247.net/test/checkquestion?question_id=" + i.ToString() + "&answer_choice=2").ToString().Contains(@"ok") == false)
                        list[z].correctAns = "B";
                    else if (http.Post("https://hoc247.net/test/checkquestion?question_id=" + i.ToString() + "&answer_choice=3").ToString().Contains(@"ok") == false)
                        list[z].correctAns = "C";
                    else list[z].correctAns = "D";
                }
                #endregion
                #region 4 đáp án
                foreach (Capture i in item.Groups["a"].Captures)
                {
                    string ques = "";
                    Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match item2 in reg2.Matches(i.ToString()))
                        foreach (Capture i2 in item2.Groups["que"].Captures)
                            if ((i2.ToString().Trim() != " ") && (i2.ToString().Trim().Contains("\t") == false)) ques += i2.ToString().Trim() + " ";
                    list[z].ans1 = ques.Trim().Replace("A.", "").Replace("  ", "");
                }
                foreach (Capture i in item.Groups["b"].Captures)
                {
                    string ques = "";
                    Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match item2 in reg2.Matches(i.ToString()))
                        foreach (Capture i2 in item2.Groups["que"].Captures)
                            if ((i2.ToString().Trim() != " ") && (i2.ToString().Trim().Contains("\t") == false)) ques += i2.ToString().Trim() + " ";
                    list[z].ans2 = ques.Trim().Replace("B.", "").Replace("  ", "");
                }
                foreach (Capture i in item.Groups["c"].Captures)
                {
                    string ques = "";
                    Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match item2 in reg2.Matches(i.ToString()))
                        foreach (Capture i2 in item2.Groups["que"].Captures)
                            if ((i2.ToString().Trim() != " ") && (i2.ToString().Trim().Contains("\t") == false)) ques += i2.ToString().Trim() + " ";
                    list[z].ans3 = ques.Trim().Replace("C.", "").Replace("  ", "");
                }
                foreach (Capture i in item.Groups["d"].Captures)
                {
                    string ques = "";
                    Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match item2 in reg2.Matches(i.ToString()))
                        foreach (Capture i2 in item2.Groups["que"].Captures)
                            if ((i2.ToString().Trim() != " ") && (i2.ToString().Trim().Contains("\t") == false)) ques += i2.ToString().Trim() + " ";
                    list[z].ans4 = ques.Trim().Replace("D.", "").Replace("  ", "");
                }
                #endregion
                z++;
            }
            return list;
            #endregion
        }

        public List<Question> Crawler_DeThi_Hoc247(string link, ref string title)
        {
            #region Setting list và crawler link
            List<Question> list = new List<Question>();
            xNetStandard.HttpRequest httpre = new xNetStandard.HttpRequest();
            string res = WebUtility.HtmlDecode(httpre.Get(link).ToString());
            #endregion
            #region Xử lí số liệu
            #region Lấy tựa đề trang web
            Regex a1 = new Regex(@"<title>(?<res>.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            title = a1.Matches(res)[0].Groups["res"].Captures[0].ToString();
            #endregion
            Regex reg = new Regex(@"<li class=""lch 336"">.*?<a(?<Ques>.*?)</a>.*?id=""dstl(?<id>.*?)"">(?<a>.*?)</li>(?<b>.*?)</li>(?<c>.*?)</li>(?<d>.*?)</li>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            int z = 0;
            foreach (Match item in reg.Matches(res))
            {
                #region Câu hỏi
                foreach (Capture i in item.Groups["Ques"].Captures)
                {
                    string ques = "";
                    Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match item2 in reg2.Matches(i.ToString()))
                        foreach (Capture i2 in item2.Groups["que"].Captures)
                            if ((i2.ToString().Trim() != " ") && (i2.ToString().Trim().Contains("\t") == false)) ques += i2.ToString().Trim() + " ";
                    list.Add(new Question() { question = ques.Trim() });
                }
                #endregion
                #region Cầu trả lời đúng                
                foreach (Capture i in item.Groups["id"].Captures)
                {
                    xNetStandard.HttpRequest http = new xNetStandard.HttpRequest();
                    if (http.Post("https://hoc247.net/test/checkquestion?question_id=" + i.ToString() + "&answer_choice=1").ToString().Contains(@"ok") == false)
                        list[z].correctAns = "A";
                    else if (http.Post("https://hoc247.net/test/checkquestion?question_id=" + i.ToString() + "&answer_choice=2").ToString().Contains(@"ok") == false)
                        list[z].correctAns = "B";
                    else if (http.Post("https://hoc247.net/test/checkquestion?question_id=" + i.ToString() + "&answer_choice=3").ToString().Contains(@"ok") == false)
                        list[z].correctAns = "C";
                    else list[z].correctAns = "D";
                }
                #endregion
                #region 4 đáp án
                foreach (Capture i in item.Groups["a"].Captures)
                {
                    string ques = "";
                    Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match item2 in reg2.Matches(i.ToString()))
                        foreach (Capture i2 in item2.Groups["que"].Captures)
                            if ((i2.ToString().Trim() != " ") && (i2.ToString().Trim().Contains("\t") == false)) ques += i2.ToString().Trim() + " ";
                    list[z].ans1 = ques.Trim().Replace("A.", "").Replace("  ", "");
                }
                foreach (Capture i in item.Groups["b"].Captures)
                {
                    string ques = "";
                    Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match item2 in reg2.Matches(i.ToString()))
                        foreach (Capture i2 in item2.Groups["que"].Captures)
                            if ((i2.ToString().Trim() != " ") && (i2.ToString().Trim().Contains("\t") == false)) ques += i2.ToString().Trim() + " ";
                    list[z].ans2 = ques.Trim().Replace("B.", "").Replace("  ", "");
                }
                foreach (Capture i in item.Groups["c"].Captures)
                {
                    string ques = "";
                    Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match item2 in reg2.Matches(i.ToString()))
                        foreach (Capture i2 in item2.Groups["que"].Captures)
                            if ((i2.ToString().Trim() != " ") && (i2.ToString().Trim().Contains("\t") == false)) ques += i2.ToString().Trim() + " ";
                    list[z].ans3 = ques.Trim().Replace("C.", "").Replace("  ", "");
                }
                foreach (Capture i in item.Groups["d"].Captures)
                {
                    string ques = "";
                    Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match item2 in reg2.Matches(i.ToString()))
                        foreach (Capture i2 in item2.Groups["que"].Captures)
                            if ((i2.ToString().Trim() != " ") && (i2.ToString().Trim().Contains("\t") == false)) ques += i2.ToString().Trim() + " ";
                    list[z].ans4 = ques.Trim().Replace("D.", "").Replace("  ", "");
                }
                #endregion
                z++;
            }
            return list;
            #endregion
        }

        public List<Question> Crawler_Tracnghiemdotnet(string link, ref string title)
        {
            #region Setting list và crawler
            List<Question> list = new List<Question>();
            xNetStandard.HttpRequest httpre = new xNetStandard.HttpRequest();
            string res = WebUtility.HtmlDecode(httpre.Get(link).ToString());
            #endregion
            #region Xử lí dữ liệu
            #region Lấy title
            Regex a1 = new Regex(@"<title>(?<res>.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            title = a1.Matches(res)[0].Groups["res"].Captures[0].ToString();
            #endregion
            #region Check có bao nhiêu trang
            int countPage = 2;
            while (true)
            {
                if (isNot404(link + "?p=" + countPage)) countPage++;
                else { countPage--; break; }
            }
            #endregion
            #region Thực hiện crawler từng trang
            int i1 = 0;
            for (int i3 = 1; i3 <= countPage; i3++)
            {
                res = WebUtility.HtmlDecode(httpre.Get(link + "?p=" + i3).ToString());
                Regex regex = new Regex(@"<li><a.*?=""(?<link>.*?)"".*?<h2>.*?</h2>(?<ques>.*?)</a>.*?<p>(?<a>.*?)</p>.*?<p>(?<b>.*?)</p>.*?<p>(?<c>.*?)</p>.*?<p>(?<d>.*?)</p>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                foreach (Match item in regex.Matches(res))
                {
                    #region Câu hỏi
                    foreach (Capture i in item.Groups["ques"].Captures)
                    {
                        string ques = "";
                        Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                        foreach (Match item2 in reg2.Matches(i.ToString()))
                            foreach (Capture i2 in item2.Groups["que"].Captures)
                                if ((i2.ToString().Trim() != " ") && (i2.ToString().Trim().Contains("\t") == false)) ques += i2.ToString().Trim() + " ";
                        list.Add(new Question() { question = ques.Trim() });
                    }
                    #endregion
                    #region 4 đáp án và câu trả lời đúng
                    foreach (Capture i in item.Groups["a"].Captures)
                        list[i1].ans1 = i.ToString().Replace("A. ", "").Replace("&nbsp;", "").Trim();
                    foreach (Capture i in item.Groups["b"].Captures)
                        list[i1].ans2 = i.ToString().Replace("B. ", "").Replace("&nbsp;", "").Trim();
                    foreach (Capture i in item.Groups["c"].Captures)
                        list[i1].ans3 = i.ToString().Replace("C. ", "").Replace("&nbsp;", "").Trim();
                    foreach (Capture i in item.Groups["d"].Captures)
                        list[i1].ans4 = i.ToString().Replace("D. ", "").Replace("&nbsp;", "").Trim();
                    foreach (Capture i in item.Groups["link"].Captures)
                        list[i1].correctAns = Get_TrueAnswer_InTracnghiemdotnet(i.ToString());
                    i1++;
                    #endregion
                }
            }
            #endregion
            return list;
            #endregion
        }

        public List<Question> Crawler_DeThi_Tracnghiemdotnet(string link, ref string title)
        {
            #region Setting list và crawler
            List<Question> list = new List<Question>();
            xNetStandard.HttpRequest http = new xNetStandard.HttpRequest();
            string res = http.Get(link).ToString();
            #endregion
            #region Xử lí dữ liệu
            #region Lấy title
            Regex a1 = new Regex(@"<title>(?<res>.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            title = a1.Matches(res)[0].Groups["res"].Captures[0].ToString();
            #endregion
            int i1 = 0;
            Regex regex = new Regex(@"<h4>.*?</h4>.*?<a href=""(?<link>.*?)""><h4>.*?<p>(?<ques>.*?)</p>.*?<p>(?<a>.*?)</p>.*?<p>(?<b>.*?)</p>.*?<p>(?<c>.*?)</p>.*?<p>(?<d>.*?)</p>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            foreach (Match item in regex.Matches(res))
            {
                #region Câu hỏi
                foreach (Capture i in item.Groups["ques"].Captures)
                    list.Add(new Question() { question = i.ToString() });
                #endregion
                #region 4 đáp án và câu trả lời đúng
                foreach (Capture i in item.Groups["a"].Captures)
                    list[i1].ans1 = i.ToString().Replace("A. ", "").Replace("&nbsp;","").Trim();
                foreach (Capture i in item.Groups["b"].Captures)
                    list[i1].ans2 = i.ToString().Replace("B. ", "").Replace("&nbsp;", "").Trim();
                foreach (Capture i in item.Groups["c"].Captures)
                    list[i1].ans3 = i.ToString().Replace("C. ", "").Replace("&nbsp;", "").Trim();
                foreach (Capture i in item.Groups["d"].Captures)
                    list[i1].ans4 = i.ToString().Replace("D. ", "").Replace("&nbsp;", "").Trim();
                foreach (Capture i in item.Groups["link"].Captures)
                    list[i1].correctAns = Get_TrueAnswer_InTracnghiemdotnet(i.ToString());
                i1++;
                #endregion
            }
            return list;
            #endregion
        }

        public string Get_TrueAnswer_InTracnghiemdotnet(string link)
        {
            xNetStandard.HttpRequest httpre = new xNetStandard.HttpRequest();
            string res = WebUtility.HtmlDecode(httpre.Get(link).ToString());
            Regex regex = new Regex(@"<span class=""right-answer""><b>(?<res>.*?)</b> là đáp án đúng</span>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            return regex.Matches(res)[0].Groups["res"].Captures[0].ToString();

        }

        public List<Question> Crawler_cungthionline(string link, ref string title)
        {
            #region Setting list và crawer data
            List<Question> list = new List<Question>();
            HttpRequest http = new HttpRequest();
            string res = http.Get(link).ToString();
            #endregion
            #region Xử lí dữ liệu
            #region Lấy title
            Regex a1 = new Regex(@"<title>(?<res>.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            title = a1.Matches(res)[0].Groups["res"].Captures[0].ToString();
            #endregion
            Regex regex = new Regex(@"class=""ContainerIndent"">(?<ques>.*?)</span>.*?=""answer_unselected"">(?<a>.*?)=""answer_unselected""(?<b>.*?)=""answer_unselected""(?<c>.*?)=""answer_unselected""(?<d>.*?)</div>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            int i1 = 0;
            foreach (Match item in regex.Matches(res))
            {
                #region Câu hỏi
                foreach (Capture i in item.Groups["ques"].Captures)
                    list.Add(new Question() { question = i.ToString().Replace("</b>", "").Replace("<br>", "").Replace("<b>", "").Trim() });
                #endregion
                #region 4 đáp án và câu trả lời đúng
                foreach (Capture i in item.Groups["a"].Captures)
                {
                    string ques = "";
                    Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match item2 in reg2.Matches(i.ToString()))
                        foreach (Capture i2 in item2.Groups["que"].Captures)
                            if ((i2.ToString().Trim() != " ") && (i2.ToString().Trim().Contains("\t") == false)) ques += i2.ToString().Trim() + " ";
                    list[i1].ans1 = ques.Replace("A.", "").Trim();
                }
                foreach (Capture i in item.Groups["b"].Captures)
                {
                    string ques = "";
                    Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match item2 in reg2.Matches(i.ToString()))
                        foreach (Capture i2 in item2.Groups["que"].Captures)
                            if ((i2.ToString().Trim() != " ") && (i2.ToString().Trim().Contains("\t") == false)) ques += i2.ToString().Trim() + " ";
                    list[i1].ans2 = ques.Replace("B.", "").Trim();
                }
                foreach (Capture i in item.Groups["c"].Captures)
                {
                    string ques = "";
                    Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match item2 in reg2.Matches(i.ToString()))
                        foreach (Capture i2 in item2.Groups["que"].Captures)
                            if ((i2.ToString().Trim() != " ") && (i2.ToString().Trim().Contains("\t") == false)) ques += i2.ToString().Trim() + " ";
                    list[i1].ans3 = ques.Replace("C.", "").Trim();
                }
                foreach (Capture i in item.Groups["d"].Captures)
                {
                    string ques = "";
                    Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match item2 in reg2.Matches(i.ToString()))
                        foreach (Capture i2 in item2.Groups["que"].Captures)
                            if ((i2.ToString().Trim() != " ") && (i2.ToString().Trim().Contains("\t") == false)) ques += i2.ToString().Trim() + " ";
                    list[i1].ans4 = ques.Replace("D.", "").Trim();
                }
                i1++;
                #endregion
            }
            #region Đáp án đúng
            i1 = 0;
            Regex regex1 = new Regex(@"<a href=""(?<res>https://cungthi.online/cau-hoi/.*?)"">", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            foreach (Match item in regex1.Matches(res))
            {
                foreach (Capture i in item.Groups["res"].Captures)
                {
                    list[i1].correctAns = Get_TrueAnswer_cungthionline(i.ToString());
                }
                i1++;
            }
            return list;
            #endregion
            #endregion
        }

        public string Get_TrueAnswer_cungthionline(string link)
        {
            xNetStandard.HttpRequest http = new xNetStandard.HttpRequest();
            string res = http.Get(link).ToString();
            Regex regex = new Regex(@"<span class=""label_result"">Đáp án:</span>(?<res>\w{1})", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            return regex.Matches(res)[0].Groups["res"].Captures[0].ToString();

        }

        public List<Question> Crawler_TuHoc365(string link, ref string title)
        {
            #region Setting List và Crawler dâta
            List<Question> list = new List<Question>();
            HttpRequest http = new HttpRequest();
            string res = http.Get(link).ToString();
            #endregion
            #region Xử lí dữ liệu
            #region Lấy title
            Regex a1 = new Regex(@"<title>(?<res>.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            title = a1.Matches(res)[0].Groups["res"].Captures[0].ToString();
            #endregion
            int i1 = 0;
            Regex regex = new Regex(@"<a href=""(?<link>https://tuhoc365.vn/question/.*?/)"" class=""col-md-12"">", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            foreach (Match item in regex.Matches(res))
            {
                foreach (Capture i in item.Groups["link"].Captures)
                {
                    #region Crawler từng trang
                    HttpRequest http1 = new HttpRequest();
                    string res1 = http1.Get(i.ToString()).ToString();
                    #endregion
                    Regex regex1 = new Regex(@"<div class=""content-quiz mg-top-10"">.*?<p><p>(?<ques>.*?)</p></p>.*?<span class=""text-uppercase""></span>(?<a>.*?)</div>.*?<span class=""text-uppercase""></span>(?<b>.*?)</div>.*?<span class=""text-uppercase""></span>(?<c>.*?)</div>.*?<span class=""text-uppercase""></span>(?<d>.*?)</div>.*?<span class=""text-uppercase"">(?<ans>.*?)</span></span>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match item1 in regex1.Matches(res1))
                    {
                        #region Lấy các thông tin cần thiết
                        foreach (Capture i2 in item1.Groups["ques"].Captures)
                            list.Add(new Question() { question = i2.ToString().Replace("<span style=\"font-weight: 400\">", "").Replace("</span>", "").Replace("&nbsp;", "").Trim() });
                        foreach (Capture i2 in item1.Groups["a"].Captures)
                            list[i1].ans1 = i2.ToString().Replace("<span style=\"font-weight: 400\">", "").Replace("</span>", "").Replace("&nbsp;", "").Trim();
                        foreach (Capture i2 in item1.Groups["b"].Captures)
                            list[i1].ans2 = i2.ToString().Replace("<span style=\"font-weight: 400\">", "").Replace("</span>", "").Replace("&nbsp;", "").Trim();
                        foreach (Capture i2 in item1.Groups["c"].Captures)
                            list[i1].ans3 = i2.ToString().Replace("<span style=\"font-weight: 400\">", "").Replace("</span>", "").Replace("&nbsp;", "").Trim();
                        foreach (Capture i2 in item1.Groups["d"].Captures)
                            list[i1].ans4 = i2.ToString().Replace("<span style=\"font-weight: 400\">", "").Replace("</span>", "").Replace("&nbsp;", "").Trim();
                        foreach (Capture i2 in item1.Groups["ans"].Captures)
                            list[i1].correctAns = i2.ToString().Replace("<span style=\"font-weight: 400\">", "").Replace("</span>", "").Replace("&nbsp;", "").Trim().ToUpper();
                        #endregion
                        i1++;
                    }

                }
            }
            return list;
            #endregion
        }

        #region Hàm crawler Vừng Ơi (chưa hoàn thành)
        //public List<Question> Crawler_VungOi(string link) //Để sau
        //{
        //    List<Question> list = new List<Question>();
        //    //List<Question> list = new List<Question>();
        //    //HttpRequest http = new HttpRequest();
        //    //string res = http.Get(link).ToString();
        //    //return res;
        //    //Regex regex = new Regex(@"<a href=""(?<link>cau-hoi-.*?)""", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
        //    //foreach (Match item in regex.Matches(res))
        //    //{
        //    //    foreach (Capture i in item.Groups["link"].Captures)
        //    //    {

        //    //    }
        //    //}

        //    HttpRequest http = new HttpRequest();
        //    string res = http.Get(link).ToString();
        //    //@"""answers"".*?""content"":"".*?""}],""answer_key"":"".*?"",""correct"":(true|false)" - Đáp án đầu tiên
        //    //@"""content"":"".*?""}],""answer_key"":"".*?"",""correct"":(true|false)" - 3 đáp án còn lại
        //    //Lợi dụng true/false tìm ra đáp án đúng

        //    //Câu hỏi
        //    Regex regex = new Regex(@"<meta property=""og:description"" content=""(?<ques>.*?)""/>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
        //    list.Add(new Question() { question = regex.Matches(res)[0].Groups["ques"].Captures[0].ToString() });

        //    //Lấy kết quả:            
        //    Regex getJson = new Regex(@"window.__INITIAL_DATA__ = (?<res>.*?)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
        //    var myDeserializedClass = JsonConvert.DeserializeObject<Root>(getJson.Matches(res)[0].Groups["res"].Captures[0].ToString());
        //    //bool valid = Root.IsValid(getJson.Matches(res)[0].Groups["res"].Captures[0].ToString());

        //    return list;
        //}
        #endregion

        public List<Question> Crawler_CungHocVui(string link, ref string title)
        {
            #region Setting list và Crawler 
            List<Question> list = new List<Question>();
            xNetStandard.HttpRequest http = new xNetStandard.HttpRequest();
            string res = http.Get(link).ToString();
            #endregion
            #region Crawler từng trang
            #region Lấy title
            Regex a1 = new Regex(@"<title>(?<res>.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            title = a1.Matches(res)[0].Groups["res"].Captures[0].ToString();
            #endregion
            int i1 = 0;
            Regex regex = new Regex(@"href=""(?<link>https://cunghocvui.com/de-thi-kiem-tra/cau-hoi/.*?)""", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            foreach (Match item in regex.Matches(res))
            {
                foreach (Capture i in item.Groups["link"].Captures)
                {
                    #region Thực hiện crawler từng trang
                    HttpRequest http1 = new HttpRequest();
                    string res1 = http1.Get(i.ToString()).ToString();
                    Regex regex1 = new Regex(@"Câu hỏi: (?<ques></span>.*?)A\.(?<a>.*?)</p>.*?B\.(?<b>.*?)</p>.*?C\.(?<c>.*?)</p>.*?D\.(?<d>.*?)</p>.*?Đáp án.*?<p>(?<ans>A|B|C|D)</p>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    foreach (Match item1 in regex1.Matches(res1))
                    {
                        #region Câu hỏi
                        foreach (Capture i2 in item1.Groups["ques"].Captures)
                        {
                            string ques = "";
                            Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                            foreach (Match item2 in reg2.Matches(i2.ToString()))
                                foreach (Capture i3 in item2.Groups["que"].Captures)
                                    if ((i3.ToString().Trim() != " ") && (i3.ToString().Trim().Contains("\t") == false)) ques += i3.ToString().Trim() + " ";
                            if (ques.Trim() != "") list.Add(new Question { question = ques }); else list.Add(new Question { question = i2.ToString().Trim() });
                        }
                        #endregion
                        #region Câu a
                        foreach (Capture i2 in item1.Groups["a"].Captures)
                        {
                            string ques = "";
                            Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                            foreach (Match item2 in reg2.Matches(i2.ToString()))
                                foreach (Capture i3 in item2.Groups["que"].Captures)
                                    if ((i3.ToString().Trim() != " ") && (i3.ToString().Trim().Contains("\t") == false)) ques += i3.ToString().Trim() + " ";
                            if (ques.Trim() != "") list[i1].ans1 = ques.Replace("\n", "").Trim(); else list[i1].ans1 = i2.ToString().Replace("\n", "").Trim();
                        }
                        #endregion
                        #region Câu b
                        foreach (Capture i2 in item1.Groups["b"].Captures)
                        {
                            string ques = "";
                            Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                            foreach (Match item2 in reg2.Matches(i2.ToString()))
                                foreach (Capture i3 in item2.Groups["que"].Captures)
                                    if ((i3.ToString().Trim() != " ") && (i3.ToString().Trim().Contains("\t") == false)) ques += i3.ToString().Trim() + " ";
                            if (ques.Trim() != "") list[i1].ans2 = ques.Replace("\n", "").Trim(); else list[i1].ans2 = i2.ToString().Replace("\n", "").Trim();
                        }
                        #endregion
                        #region Câu c
                        foreach (Capture i2 in item1.Groups["c"].Captures)
                        {
                            string ques = "";
                            Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                            foreach (Match item2 in reg2.Matches(i2.ToString()))
                                foreach (Capture i3 in item2.Groups["que"].Captures)
                                    if ((i3.ToString().Trim() != " ") && (i3.ToString().Trim().Contains("\t") == false)) ques += i3.ToString().Trim() + " ";
                            if (ques.Trim() != "") list[i1].ans3 = ques.Replace("\n", "").Trim(); else list[i1].ans3 = i2.ToString().Replace("\n", "").Trim();
                        }
                        #endregion
                        #region Câu d
                        foreach (Capture i2 in item1.Groups["d"].Captures)
                        {
                            string ques = "";
                            Regex reg2 = new Regex(@">(?<que>.*?)<", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                            foreach (Match item2 in reg2.Matches(i2.ToString()))
                                foreach (Capture i3 in item2.Groups["que"].Captures)
                                    if ((i3.ToString().Trim() != " ") && (i3.ToString().Trim().Contains("\t") == false)) ques += i3.ToString().Trim() + " ";
                            if (ques.Trim() != "") list[i1].ans4 = ques.Replace("\n", "").Trim(); else list[i1].ans4 = i2.ToString().Replace("\n", "").Trim();
                        }
                        #endregion
                        #region Đáp án
                        foreach (Capture i2 in item1.Groups["ans"].Captures)
                            list[i1].correctAns = i2.ToString().Trim().ToUpper();
                        #endregion
                        i1++;
                    }
                    #endregion
                }
            }
            return list;
            #endregion
        }

        public List<Question> Crawler_Tech12h(string link, ref string title)
        {
            #region Setting list and crawler
            List<Question> list = new List<Question>();
            HttpRequest http = new HttpRequest();
            string res = http.Get(link).ToString();
            #endregion
            #region Xử lý dữ liệu
            #region Lấy title
            Regex a1 = new Regex(@"<title>(?<res>.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            title = a1.Matches(res)[0].Groups["res"].Captures[0].ToString();
            #endregion
            res = res.Replace("</li></ul>", "</li></ul>$end"); //Nhằm regex được
            int i1 = 0;
            Regex regex = new Regex(@"<p>(?<ques>C.*?)<li>(?<a>.*?)</li><li>(?<b>.*?)</li><li>(?<c>.*?)</li><li>(?<d>.*?)</li></ul>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            foreach (Match item in regex.Matches(res))
            {
                #region Câu hỏi
                foreach (Capture i in item.Groups["ques"].Captures)
                    list.Add(new Question() { question = i.ToString().Replace("&nbsp", "").Replace("</p><ul>", "").Remove(0, 7).Replace(";", "").Trim() });
                #endregion
                #region Câu a
                foreach (Capture i in item.Groups["a"].Captures)
                    if (i.ToString().Contains("h6"))
                    {
                        list[i1].correctAns = "A";
                        list[i1].ans1 = i.ToString().Replace("<h6>", "").Replace("</h6>", "").Replace("&nbsp", "").Replace("A.","").Replace(";","").Trim();
                    }
                    else list[i1].ans1 = i.ToString().Replace("&nbsp", "").Replace("A.", "").Replace(";", "").Trim();
                #endregion
                #region Câu b
                foreach (Capture i in item.Groups["b"].Captures)
                    if (i.ToString().Contains("h6"))
                    {
                        list[i1].correctAns = "B";
                        list[i1].ans2 = i.ToString().Replace("<h6>", "").Replace("</h6>", "").Replace("&nbsp", "").Replace("B.", "").Replace(";", "").Trim();
                    }
                    else list[i1].ans2 = i.ToString().Replace("&nbsp", "").Replace("B.", "").Replace(";", "").Trim();
                #endregion
                #region Câu c
                foreach (Capture i in item.Groups["c"].Captures)
                    if (i.ToString().Contains("h6"))
                    {
                        list[i1].correctAns = "C";
                        list[i1].ans3 = i.ToString().Replace("<h6>", "").Replace("</h6>", "").Replace("&nbsp", "").Replace("C.", "").Replace(";", "").Trim();
                    }
                    else list[i1].ans3 = i.ToString().Replace("&nbsp", "").Replace("C.", "").Replace(";", "").Trim();
                #endregion
                #region Câu d
                foreach (Capture i in item.Groups["d"].Captures)
                    if (i.ToString().Contains("h6"))
                    {
                        list[i1].correctAns = "D";
                        list[i1].ans4 = i.ToString().Replace("<h6>", "").Replace("</h6>", "").Replace("&nbsp", "").Replace("D.", "").Replace(";", "").Trim();
                    }
                    else list[i1].ans4 = i.ToString().Replace("&nbsp", "").Replace("D.", "").Replace(";", "").Trim();
                #endregion
                i1++;
            }
            return list;
            #endregion
        }

        #region Hàm crawler Đọc Tài Liệu chưa hoàn thành
        //public string Crawler_DocTaiLieu(string link)
        //{
        //    List<Question> list = new List<Question>();
        //    xNetStandard.HttpRequest http = new xNetStandard.HttpRequest();
        //    string res = http.Get(link).ToString();
        //    return res;
        //    //@"<div class=box-van-dap><a href=(https://doctailieu.com/trac-nghiem/.*?) title"
        //    //@"<div class=""box-van-dap"">.*?<a href=""(https://doctailieu.com/trac-nghiem/.*?)"""

        //    //Crawler từng link:
        //    //@"<div class=info-2><div class=form-group>(.*?)</div></div>.*?<label class=label-radio> A. (.*?)<input class=.*?<label class=label-radio> B. (.*?)<input class=.*?<label class=label-radio> C. (.*?) <input class=.*?<label class=label-radio> D. (.*?) <input class=.*?đáp án đúng: (A|B|C|D)"

        //}
        #endregion
    }
}
