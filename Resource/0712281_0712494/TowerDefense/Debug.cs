using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace TowerDefense
{
    public class Debug
    {
        public static bool IsDebugging = true;
        static string FileName = DateTime.Now.ToString("yyyy.MM.dd HH.mm.ss.ff", null);
        static string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static void Logging(string str)
        {
            if (!IsDebugging) return;
            Log("TRACK: ", ", Message: " + str);
        }

        public static void ErrorLogging(string str)
        {
            if (!IsDebugging) return;
            Log("ERR: ", ", Message: " + str);
        }

        static void Log(string strBeforeFunction,
            string strAfterFunction)
        {//http://www.csharp-examples.net/reflection-calling-method-name/
            if (!IsDebugging) return;

            string strFullFileName = path + "\\Debug\\" + FileName + ".txt";

            StackTrace stackTrace = new StackTrace();           // get call stack
            StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

            if (stackFrames.Count() >= 2)
            {
                StringBuilder strMsg = new StringBuilder(200);
                strMsg.Append(strBeforeFunction);
                strMsg.Append(stackFrames[4].GetMethod().Name);
                strMsg.Append(" >> ");
                strMsg.Append(stackFrames[3].GetMethod().Name);
                strMsg.Append(" >> ");
                strMsg.Append(stackFrames[2].GetMethod().Name);
                strMsg.Append(strAfterFunction);
                strMsg.Append(Environment.NewLine);
                strMsg.Append("---------------------------");
                strMsg.Append(Environment.NewLine);

                WriteFile(strFullFileName, strMsg.ToString());
            }
        }
        static void WriteFile(string strFullFileName,
            string strMsg)
        {//http://bytes.com/topic/net/insights/673819-write-read-text-file-c
            string str = Directory.GetParent(strFullFileName).FullName;
            if (!Directory.Exists(str))
            {
                Directory.CreateDirectory(str);
            }

            if (!File.Exists(strFullFileName))
            {
                FileStream fs = File.Create(strFullFileName);
                fs.Close();
            }
            File.AppendAllText(strFullFileName, strMsg, Encoding.UTF8);
        }
    }
}
