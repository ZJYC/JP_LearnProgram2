using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ;
        }
    }
    namespace ZJYC
    {

        public class RANDOM
        {
            public double Random(double Min, double Max)
            {
                int Tem = new Random(Guid.NewGuid().GetHashCode()).Next();
                double Res = Tem * 1.0 / int.MaxValue;
                return Res * (Max - Min) + Min;
            }
            public int Random()
            {
                int Res = new Random(Guid.NewGuid().GetHashCode()).Next();
                return Res;
            }
            public int Random(int Min, int Max)
            {
                int Res = new Random(Guid.NewGuid().GetHashCode()).Next(Min, Max + 1);
                return Res;
            }
        }

        public class GENLIST
        {
            private int Random(int Min, int Max)
            {
                int Res = new Random(Guid.NewGuid().GetHashCode()).Next(Min, Max + 1);
                return Res;
            }
            public List<int> OrderList(int Start, int End)
            {
                int Add = End >= Start ? 1 : -1;
                int Len = Math.Abs(Start - End) + 1;
                List<int> Res = new List<int>();
                for (int i = 0; i < Len; i++) Res.Add(Start + Add * i);
                return Res;
            }
            public List<int> List(int Start, int End, int Count)
            {
                List<int> Res = new List<int>();

                for (int i = 0; i < Count; i++)
                {
                    Res.Add(Random(Start, End));
                }

                return Res;
            }
            public List<int> List(int Start, int End)
            {
                List<int> Tem = new List<int>();
                List<int> Res = new List<int>();
                for (int i = 0; i < End - Start + 1; i++)
                {
                    Tem.Add(Start + i);
                }

                for (int i = 0; i < End - Start + 1; i++)
                {
                    int DstIndex = Random(0, Tem.Count - 1);
                    Res.Insert(i, Tem[DstIndex]);
                    Tem.Remove(Tem[DstIndex]);
                }


                return Res;
            }
            public List<int> List(List<int> Tem)
            {
                List<int> Res = new List<int>();
                int Count = Tem.Count;
                for (int i = 0; i < Count; i++)
                {
                    int DstIndex = Random(0, Tem.Count - 1);
                    Res.Insert(i, Tem[DstIndex]);
                    Tem.Remove(Tem[DstIndex]);
                }
                return Res;
            }
        }

        namespace TextOpt
        {
            //产生日语干扰项
            public class GenJpInterferenceItems
            {
                class ExchangeItem
                {
                    public string Sen = string.Empty;
                    public string Rai = string.Empty;
                    public double Ritu = 0.5;
                    public ExchangeItem(string Sen, string Rai, double Ritu)
                    {
                        this.Sen = Sen;
                        this.Rai = Rai;
                        this.Ritu = Ritu;
                    }
                }
                List<ExchangeItem> Items = new List<ExchangeItem>()
                        {
                            new ExchangeItem("た","だ",0.20),
                            new ExchangeItem("だ","た",0.20),
                            new ExchangeItem("て","で",0.20),
                            new ExchangeItem("で","て",0.20),
                            new ExchangeItem("が","か",0.20),
                            new ExchangeItem("か","が",0.20),
                            new ExchangeItem("い",""  ,0.10),
                            new ExchangeItem("う",""  ,0.25),
                            new ExchangeItem("や","ゃ",0.50),
                            new ExchangeItem("ゆ","ゅ",0.50),
                            new ExchangeItem("よ","ょ",0.50),
                            new ExchangeItem("つ","っ",0.25),
                            new ExchangeItem("つ",""  ,0.20),
                        };
                public double Random(double Min, double Max)
                {
                    int Tem = new Random(Guid.NewGuid().GetHashCode()).Next();
                    double Res = Tem * 1.0 / int.MaxValue;
                    return Res * (Max - Min) + Min;
                }
                private string ExchangeByRate(string Input)
                {
                    foreach (ExchangeItem Item in Items)
                    {
                        if (Input.Contains(Item.Sen) && Random(0.0, 1.0) < Item.Ritu)
                        {
                            Input = Input.Replace(Item.Sen, Item.Rai);
                        }
                    }
                    return Input;
                }
                public List<string> Gen(string Input, int Count)
                {
                    List<string> Res = new List<string>();

                    for (int j = 0; j < 10; j++)
                    {
                        for (int i = 0; i < Count; i++)
                        {
                            string Temp = Gen(Input);
                            if (Res.Contains(Temp) == false) Res.Add(Temp);
                        }
                        if (Res.Count >= Count) break;
                    }
                    //没有生成足够的项目就复制原字符串充数
                    for (int i = Res.Count; i < Count; i++)
                    {
                        Res.Add(Input);
                    }
                    return Res;

                }
                public string Gen(string Input)
                {
                    string Res = ExchangeByRate(Input);
                    return Res;
                }
            }
        }
    }

    class ITEMS
    {
        public string JsonFileName = string.Empty;
        public List<ITEM> Internal = new List<ITEM>();
        //public List<ITEM> External = new List<ITEM>();

        public void CopyItems(ref List<ITEM> Src, ref List<ITEM> Dst)
        {
            Dst.Clear();
            for (int i = 0; i < Src.Count; i++) Dst.Add(Src[i]);
        }

        private void ReadStringFrJsonFile(string JsonFileName, ref string Content)
        {
            try
            {
                using (StreamReader Reader = new StreamReader(JsonFileName))
                {
                    Content = Reader.ReadToEnd();
                }
            }
            catch
            {
                ;//MessageBox.Show("ReadStringFrJsonFile" + JsonFileName);
            }
        }
        private void WritStringToJsonFile(string JsonFileName, ref string Content)
        {
            try
            {
                using (StreamWriter Writer = new StreamWriter(JsonFileName))
                {
                    Writer.Write("");
                    Writer.Write(Content);
                }
            }
            catch
            {
                ;// MessageBox.Show("WritJsonFile" + JsonFileName);
            }
        }

        public void Read()
        {
            string TempString = string.Empty;
            ReadStringFrJsonFile(JsonFileName, ref TempString);
            Internal = JsonConvert.DeserializeObject<List<ITEM>>(TempString);
            //CopyItems(ref Internal,ref External);
        }
        public void Writ()
        {
            int i = 0;
            foreach (ITEM Item in Internal)
            {
                Item.UniqueID = i++;
                Item.ErrorRate = Item.ErrorCount * 1.0 / Item.LearnCount;
            }
            string TempString = JsonConvert.SerializeObject(Internal);
            WritStringToJsonFile(this.JsonFileName, ref TempString);
            //CopyItems(ref Internal, ref External);
        }

        private int GetIndexByID(int ID)
        {
            for (int i = 0; i < Internal.Count; i++)
            {
                if (Internal[i].UniqueID == ID) return i;
            }
            return Internal.Count;
        }

        public void Add(ITEM NewItem)
        {
            Internal.Add(NewItem);
        }

        public void Add(List<ITEM> NewItems)
        {
            Internal.AddRange(NewItems);
        }

        public void Mod(int ID,string [] Keys,string [] Vals)
        {
            if (Keys.Length != Vals.Length) return;
            int Index = GetIndexByID(ID);
            string Temp = string.Empty;
            for(int i = 0;i < Keys.Length;i++)
            {
                if(Internal[i].MainBody.TryGetValue(Keys[i],out Temp))
                {
                    Internal[i].MainBody[Keys[i]] = Vals[i];
                }
                else
                {
                    Internal[i].MainBody.Add(Keys[i], Vals[i]);

                }
            }
        }

        public void Del(int ID)
        {
            int Index = GetIndexByID(ID);
            Internal.Remove(Internal[Index]);
        }

        public void Del(int ID,string [] Keys)
        {
            int Index = GetIndexByID(ID);
            string Temp = string.Empty;
            for (int i = 0; i < Keys.Length; i++)
            {
                if (Internal[i].MainBody.TryGetValue(Keys[i], out Temp))
                {
                    Internal[i].MainBody.Remove(Keys[i]);
                }
            }
        }

    }

    class ITEM
    {
        public int UniqueID = 0;
        public double ErrorRate = 0.0;
        public int LearnCount = 1, ErrorCount = 0;
        public DateTime LastLearnTime = DateTime.Now;
        public Dictionary<string, string> MainBody = new Dictionary<string, string>();
    }
}
