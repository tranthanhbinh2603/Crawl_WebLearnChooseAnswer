﻿using System.Collections.Generic;
using xNet;
using System.Net;
using System.Text.RegularExpressions;

namespace Crawl_WebLearnChooseAnswer
{
    class Func_WebRequest
    {
        public bool isNot404 (string link)
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

        public List<Question> Crawler_Khoahocdotvietjack(string link)
        {
            #region Setting list & Bắt request
            List<Question> list = new List<Question>();
            HttpRequest httpre = new HttpRequest();
            httpre.Cookies = new CookieDictionary()
            {
                {"cross-site-cookie", "bar" },
                {"_ga", "GA1.2.724910773.1637054252" },
                {"_fbp", "fb.1.1656583363610.990893404" },
                {"XSRF-TOKEN", "eyJpdiI6IkR0SXNTOTZLc0xSbTdcL3p6MjZ6ZHJRPT0iLCJ2YWx1ZSI6IjdKcUlSTDNJWGlGZnErVVdMY0U3NzYxZDJvWUMyOUt1WVpUZSt2bEhQNWtIb3RmY1QxMEphYkZpWjgzbnVvcCsiLCJtYWMiOiIzZDI1MTEwYTE0OGM2NTQ3M2M2NTQwMWE2MGMyNjU2MGI4MTlmM2YxZDgzOTNjN2Q3OGM2ZjlhMzgyMTgzYzAyIn0" },
                {"khoahocvietjackcom_session", "eyJpdiI6Ik8rS2RTRndDTkRMVmVqYU5oWFVSbXc9PSIsInZhbHVlIjoicFE3ZDlTM2JuRllLSnRhazJhSnVCdHR3ZTB4eTVWZG04NnQzUDJZN0EzVW1JUENTbThSMys5YXpVOUhGeGljd0hWWnRJNnFzcXM1aW5QY1FvRTY5OXFpazBwZWV0QXF0TE5IOXd5cWlxVXVyTzRLYTMzeG0xWW9LM0E1ZllQOGIiLCJtYWMiOiJlMzQyMTIyMDBkYzMwMzQxNzU0Y2E1ZTE2MzNkZjdjYWQyNDg5N2U0NWQ0NTc5NzBiNTA0ZmNiZDYxZTVmNzliIn0" }
            };
            string res = WebUtility.HtmlDecode(httpre.Get(link).ToString());
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
                    Regex regex2 = new Regex(@"A.(?<a>.*?)B.(?<b>.*?)C.(?<c>.*?)D.(?<d>.*?)$", RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
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
        
        public List<Question> Crawler_Hoc247(string link)
        {
            #region Setting list và crawler link
            List<Question> list = new List<Question>();
            xNetStandard.HttpRequest httpre = new xNetStandard.HttpRequest();
            string res = WebUtility.HtmlDecode(httpre.Get(link).ToString());
            Regex reg = new Regex(@"<li class=""box-1 lch"">.*?<a(?<Ques>.*?)</a>.*?id=""dstl(?<id>.*?)"">(?<a>.*?)</li>(?<b>.*?)</li>(?<c>.*?)</li>(?<d>.*?)</li>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
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

        public List<Question> Crawler_DeThi_Hoc247(string link)
        {
            #region Setting list và crawler link
            List<Question> list = new List<Question>();
            xNetStandard.HttpRequest httpre = new xNetStandard.HttpRequest();
            string res = WebUtility.HtmlDecode(httpre.Get(link).ToString());
            #endregion
            #region Xử lí số liệu
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

        public List<Question> Crawler_Tracnghiemdotnet(string link)
        {
            #region Setting list và crawler
            List<Question> list = new List<Question>();
            xNetStandard.HttpRequest httpre = new xNetStandard.HttpRequest();
            string res = WebUtility.HtmlDecode(httpre.Get(link).ToString());
            #endregion
            #region Xử lí dữ liệu
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
                res = WebUtility.HtmlDecode(httpre.Get(link +"?p=" + i3).ToString());        
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
                        list[i1].ans1 = i.ToString().Replace("A. ", "").Trim();
                    foreach (Capture i in item.Groups["b"].Captures)
                        list[i1].ans2 = i.ToString().Replace("B. ", "").Trim();
                    foreach (Capture i in item.Groups["c"].Captures)
                        list[i1].ans3 = i.ToString().Replace("C. ", "").Trim();
                    foreach (Capture i in item.Groups["d"].Captures)
                        list[i1].ans4 = i.ToString().Replace("D. ", "").Trim();
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

        public List<Question> Crawler_DeThi_Tracnghiemdotnet(string link)
        {
            #region Setting list và crawler
            List<Question> list = new List<Question>();
            xNetStandard.HttpRequest http = new xNetStandard.HttpRequest();
            string res = http.Get(link).ToString();
            #endregion
            #region Xử lí dữ liệu
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
                    list[i1].ans1 = i.ToString().Replace("A. ", "").Trim();
                foreach (Capture i in item.Groups["b"].Captures)
                    list[i1].ans2 = i.ToString().Replace("B. ", "").Trim();
                foreach (Capture i in item.Groups["c"].Captures)
                    list[i1].ans3 = i.ToString().Replace("C. ", "").Trim();
                foreach (Capture i in item.Groups["d"].Captures)
                    list[i1].ans4 = i.ToString().Replace("D. ", "").Trim();
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

        public List<Question> Crawler_cungthionline(string link)
        {
            #region Setting list và crawer data
            List<Question> list = new List<Question>();
            HttpRequest http = new HttpRequest();
            string res = http.Get(link).ToString();
            #endregion
            #region Xử lí dữ liệu
            Regex regex = new Regex(@"class=""ContainerIndent"">(?<ques>.*?)</span>.*?=""answer_unselected"">(?<a>.*?)=""answer_unselected""(?<b>.*?)=""answer_unselected""(?<c>.*?)=""answer_unselected""(?<d>.*?)</div>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            int i1 = 0;
            foreach (Match item in regex.Matches(res))
            {
                #region Câu hỏi
                foreach (Capture i in item.Groups["ques"].Captures)
                    list.Add(new Question() { question = i.ToString().Replace("</b>", "").Replace("<br>","").Replace("<b>","").Trim() });
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

        public string Crawler_TuHoc365(string link)
        {
            List<Question> list = new List<Question>();
            HttpRequest http = new HttpRequest();
            string res = http.Get(link).ToString();
            Regex regex = new Regex(@"<a href=""(?<link>https://tuhoc365.vn/question/.*?/)"" class=""col-md-12"">", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            foreach (Match item in regex.Matches(res))
            {
                foreach (Capture i in item.Groups["link"].Captures)
                    list.Add(new Question() { question = i.ToString().Replace("</b>", "").Replace("<br>", "").Replace("<b>", "").Trim() });
            }
            return "";
        }
        
    }
}
