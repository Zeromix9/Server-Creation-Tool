using System;
using System.Text;

namespace Server_Creation_Tool.myClasses
{
    public class log
    {
        public static StringBuilder sb = new StringBuilder();
        public static void LogAppend(string toAppend)
        {
            sb.AppendFormat(DateTime.Now.ToString("hh:mm tt") + ">>> " + toAppend + Environment.NewLine);
        }
    }
}
