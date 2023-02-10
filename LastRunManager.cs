using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFileCopy
{
    internal class LastRunManager
    {
        public static DateTime GetLastRun()
        {
            try
            {
                using (var sr = new StreamReader(".lastrun"))
                {
                    // Read the stream as a string, and write the string to the console.
                    var lastRun = sr.ReadToEnd();
                    if (string.IsNullOrEmpty(lastRun))
                        return DateTime.MinValue;
                    else
                        return DateTime.Parse(lastRun);
                }
            } 
            catch
            {
                Console.WriteLine("Last run file not found.");
                return DateTime.MinValue;
            }
        }

        internal static void Update()
        {
            File.WriteAllText(".lastrun", DateTime.Now.ToString());            
        }
    }
}
