using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3
{
    internal class Program
    {
        static string totalTxt = "";
        static int linelen = 0;
        static int nrLines = 0;
        static string nonSymbols = "0123456789.";
        static Dictionary<int, List<int>> allGears = new Dictionary<int, List<int>>();
        static Dictionary<int, int> allNrs = new Dictionary<int, int>();
        static void Main(string[] args)
        {
            Day3b();   
        }

        static void Day3b()
        {
            var txt = File.ReadAllText("input.txt");

            txt = txt.Replace("\r", "");
            // Make an array
            foreach (var lin in txt.Split('\n'))
            {
                // Hack to prevent last nrs from being skipped
                totalTxt += lin + ".";
                if (linelen == 0)
                    linelen = lin.Length + 1;
                
                nrLines++;
            }
            int totalSum = 0;
            int startIdx = 0;
            List<int> gears = new List<int>();
            for (int y = 0; y < nrLines; y++)
            {
                var tmpNr = "";
                startIdx = -1;
                bool isValid = false;
                for (int x = 0; x < linelen; x++)
                {
                    var ltr = totalTxt[x + y * linelen].ToString();
                    int dig;

                    if (int.TryParse(ltr, out dig))
                    {
                        if (startIdx == -1)
                        {
                            startIdx = x + y * linelen;
                            gears = new List<int>();
                        }
                        tmpNr += ltr;
                        isValid |= CheckAroundB(x, y, gears);
                    }
                    else
                    {
                        // Check if end of nr
                        if (tmpNr != "")
                        {
                            if (isValid)
                            {
                                int nr = int.Parse(tmpNr);
                                totalSum += nr;
                                allNrs.Add(startIdx, nr);
                                foreach(var g in gears)
                                {
                                    if(!allGears.ContainsKey(g))
                                    {
                                        allGears.Add(g, new List<int>());
                                    }
                                    if (!allGears[g].Contains(startIdx))
                                    {
                                        allGears[g].Add(startIdx);
                                    }
                                }
                                startIdx = -1;
                            }
                            tmpNr = "";
                            isValid = false;
                        }
                    }
                }
            }

            // Loop through all gears that have 2 nrs
            var validGears = allGears.Where(g => g.Value.Count == 2);
            long totalRatio = 0;
            foreach(var g in validGears)
            {
                var ratio = allNrs[g.Value[0]] * allNrs[g.Value[1]];
                totalRatio += ratio;
            }

            Console.WriteLine(totalRatio);
            Console.ReadLine();
        }

        

        static bool CheckAroundB(int x, int y, List<int> gears)
        {
            // Look around this nr
            string aroundChars = "";
            if (x > 0) aroundChars +=  getChar((x - 1) + y * linelen,gears);
            if (x < linelen - 1) aroundChars += getChar((x + 1) + y * linelen,gears);
            if (y > 0) aroundChars += getChar(x + (y - 1) * linelen,gears);
            if (y < nrLines - 1) aroundChars += getChar(x + (y + 1) * linelen, gears);

            if (x > 0 && y > 0) aroundChars += getChar((x - 1) + (y - 1) * linelen, gears);
            if (x < linelen - 1 && y > 0) aroundChars += getChar((x + 1) + (y - 1) * linelen, gears);
            if (x < linelen - 1 && y < nrLines - 1) aroundChars += getChar((x + 1) + (y + 1) * linelen, gears);
            if (x > 0 && y < nrLines - 1) aroundChars += getChar((x - 1) + (y + 1) * linelen, gears);

            int nrSymb = 0;
            foreach (var l in aroundChars)
            {
                if (!nonSymbols.Contains(l))
                {
                    nrSymb++;
                }
            }

            return nrSymb > 0;
        }

        static string getChar(int charNr, List<int> gears)
        {
            var ltr = totalTxt[charNr].ToString();
            if(ltr == "*")
            {
                if (!gears.Contains(charNr))
                    gears.Add(charNr);
            }
            return ltr;
        }

        static bool CheckAround(int x, int y)
        {
            // Look around this nr
            string aroundChars = "";
            if (x > 0) aroundChars += totalTxt[(x - 1) + y * linelen];
            if (x < linelen - 1) aroundChars += totalTxt[(x + 1) + y * linelen];
            if (y > 0) aroundChars += totalTxt[x + (y - 1) * linelen];
            if (y < nrLines - 1) aroundChars += totalTxt[x + (y + 1) * linelen];

            if (x > 0 && y > 0) aroundChars += totalTxt[(x - 1) + (y - 1) * linelen];
            if (x < linelen - 1 && y > 0) aroundChars += totalTxt[(x + 1) + (y - 1) * linelen];
            if (x < linelen - 1 && y < nrLines - 1) aroundChars += totalTxt[(x + 1) + (y + 1) * linelen];
            if (x > 0 && y < nrLines - 1) aroundChars += totalTxt[(x - 1) + (y + 1) * linelen];

            int nrSymb = 0;
            foreach (var l in aroundChars)
            {
                if (!nonSymbols.Contains(l))
                {
                    nrSymb++;
                }
            }

            return nrSymb > 0;
        }

        static void Day3a()
        {
            var txt = File.ReadAllText("input.txt");

            txt = txt.Replace("\r", "");
            // Make an array
            foreach (var lin in txt.Split('\n'))
            {
                if (linelen == 0)
                    linelen = lin.Length + 1;
                totalTxt += lin + ".";
                nrLines++;
            }
            int totalSum = 0;
            for (int y = 0; y < nrLines; y++)
            {
                var tmpNr = "";
                bool isValid = false;
                for (int x = 0; x < linelen; x++)
                {
                    var ltr = totalTxt[x + y * linelen].ToString();
                    int dig;

                    if (int.TryParse(ltr, out dig))
                    {
                        tmpNr += ltr;
                        isValid |= CheckAround(x, y);
                    }
                    else
                    {
                        // Check if end of nr
                        if (tmpNr != "")
                        {
                            if (isValid)
                            {
                                int nr = int.Parse(tmpNr);
                                totalSum += nr;
                            }
                            tmpNr = "";
                            isValid = false;
                        }
                    }
                }
            }

            Console.WriteLine(totalSum);
            Console.ReadLine();
        }
    }
}
