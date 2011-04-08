using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace DiabloExRes
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                if (args[0] == "0")
                {
                    CreateScript cc = new CreateScript();
                    cc.ReadFromFile("input.txt");

                    cc.StartAcceptIndexMons = 0;
                    cc.EndAcceptIndexMons = 1000;

                    cc.RunScript(Directory.GetCurrentDirectory() + @"\Sprites");
                }

                if (args[0] == "1")
                {
                    BatchImageTrimmer bit = new BatchImageTrimmer(Directory.GetCurrentDirectory() + @"\Sprites", null);
                    bit.Run();
                }
            }
        }
    }
}
