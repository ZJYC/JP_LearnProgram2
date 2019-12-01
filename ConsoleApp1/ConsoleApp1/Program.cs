using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ZJYC;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ITEMS Items = new ITEMS("Temp.json");
            MultiChoice multi = new MultiChoice(ref Items);
            multi.ImportItems(ref Items.Internal, "随机");
            multi.GenOneQuestion("LearnCount", "ErrorRate", new List<string>() { "LearnCount" }, new List<string>() { "ErrorRate" });
        }
    }
}
