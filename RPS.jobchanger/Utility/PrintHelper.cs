using NLog;
using RPS.jobchanger.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPS.jobchanger.Utility
{
    public class PrintHelper
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        public static void Print(string message, LogLevel lgLevel = null)
        {
            //Console.WriteLine(message);
            if (lgLevel == null)
            {
                lgLevel = LogLevel.Trace;
            }
            //no need to show "Please Wait" in log
            if (message != Messages.PleaseWait)
            {
                logger.Log(lgLevel, message);
            }
        }

        public static void Error(string message)
        {
            //this will hide Query displayed to the console. only appear in log
            logger.Error(message);
        }

        public static void Trace(string message)
        {
            Print(message, LogLevel.Trace);
        }

        public static void Info(string message)
        {
            Print(message, LogLevel.Info);
        }
    }
}
