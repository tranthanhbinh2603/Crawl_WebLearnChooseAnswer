using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Crawl_WebLearnChooseAnswer;

namespace Main_Project_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                List<string> listlink = new List<string>();
                StreamReader streread = new StreamReader(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\input.txt");
                string res = streread.ReadLine();
                while (res != null)
                {
                    listlink.Add(res);
                    res = streread.ReadLine();
                }
                foreach (var item in listlink)
                {
                    Console.WriteLine("[----->]" + item);
                    int i = Func_Utilities.crawlerOneWeb(item);
                    if (i == 1)
                        Console.WriteLine("[!]DONE");
                    if (i == 0)
                        Console.WriteLine("[!]ERROR: LINK NOT SUPPORT OR ERROR IN DSA OR NOT FIND FILE cookie_vietjack.txt");
                    if (i == -1)
                        Console.WriteLine("[!]ERROR: THOWN EXCEPTION");
                }
                Console.WriteLine("[----->]FINISH");
                Console.ReadLine();
            }            
            catch (FileNotFoundException)
            {
                Console.WriteLine("Khong tim thay file input.txt. Vui long dat link cua ban vao 1 file co ten input.txt trong thu muc cai dat");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[----->] ERROR: THOWN EXCEPTION: " + ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
