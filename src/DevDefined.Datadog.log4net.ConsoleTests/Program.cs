using System;
using System.Reflection;
using log4net;

namespace Tests
{
    internal class Program
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            log.Debug("This is a debug message");
            log.Info("This is an info message");
            log.Warn("this is a warning message");
            try
            {
                int i = 1;
                int j = 0;
                int a = i/j;
            }
            catch (Exception ex)
            {
                log.Error("This is an error with an exception", ex);
            }
        }
    }
}