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
            Regex reg = new Regex(@"<div class=""question-name"">.*?</div>");
            foreach (Match item in reg.Matches(res))
            {
                foreach (Capture i in item.Groups["q"].Captures)
                {
                    //Muốn lấy kết quả ra thì dùng (Tên biến chứa kết quả trong từng group).ToString();
                    list.Add(new Question { question = i.ToString() });
                }
            }
            return list;
        }
        
    }
}
