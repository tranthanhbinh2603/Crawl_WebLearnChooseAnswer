using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;
using System.Net;
using System.Text.RegularExpressions;

namespace Crawl_WebLearnChooseAnswer
{
    class Func_WebRequest
    {
        public List<Question> Crawler_Khoahocdotvietjack(string link)
        {
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
                            if (i2.ToString().Trim() != "") s1 += i2.ToString().Trim() + "\n";
                        }
                    }
                    list.Add(new Question { question = s1 });
                }
            }
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

            return list;
        }
        
    }
}
