using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;
using System.Net;

namespace Crawl_WebLearnChooseAnswer
{
    class Func_WebRequest
    {
        public string Crawler_Khoahocdotvietjack(string link)
        {
            HttpRequest httpre = new HttpRequest();
            httpre.Cookies = new CookieDictionary()
            {
                {"cross-site-cookie", "bar" },
                {"_ga", "GA1.2.713753729.1653561215" },
                {"_fbp", "fb.1.1656583363610.990893404" },
                {"XSRF-TOKEN", "eyJpdiI6ImREd2VWNjlxYmVUZHJqdUIzazRvbXc9PSIsInZhbHVlIjoibHJ4MmFpYXVpakROSDFZS3VCVkQ4UXFcL092QTVDeHhwTWl1Rm94NmZoMmJZWmk0RlwvbktaZ0thS25OQTl1SDQ2IiwibWFjIjoiZDljNmU3MTZiMzM2ODllYzNjNjI1ZTQxMTIxZWI2MmFlNzEyM2YzNjgwZWNlZmY2NTZjYzEwZjMwYTExOGQxMCJ9" },
                {"khoahocvietjackcom_session", "eyJpdiI6IkdFYUlmdUdpNjc5b3dpT1IyXC8zcDRBPT0iLCJ2YWx1ZSI6Imo0S003OVZYdmlKUDNkekdQelM5KzNxWkVTNnVQSVcrUVc2UE01ZlRkR3RNQytjZ1I4aGdFQzhod3dSMWZ6RVFxZUNYa25XNndXVENpNGllSUtcLzlwd05lREd3K21yeThzbHJiNmdLZEhhc1llN2loREg5cGhCcjdlVGdGOG9wbCIsIm1hYyI6ImQ1YjdmZWZjYjI0YmY3NDMzMTBiMzJkNzc5MWFkNWE0MWE1MmU0YzViMGZkNDBkZmQ1MTc4ZWJhYTY5YmU0NDAifQ%3D%3D" }
            };
            string res = httpre.Get(link).ToString();
            res = WebUtility.HtmlDecode(res);
            return res;
        }
        
    }
}
