using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFileCopy
{
    internal class SmartFileCopyConfig
    {
        public string SourceFolder { get; set; }
        public string DestinationFolder { get; set; }
        public int IntervalInSeconds { get; set; }
        public string AfterCopyPostUrl { get; set; }
        public string AfterCopyPostBody { get; set; }
        public static SmartFileCopyConfig Load()
        {
            var config = new SmartFileCopyConfig();
            try
            {
                string configString;
                using (var sr = new StreamReader(".config"))
                {
                    // Read the stream as a string, and write the string to the console.
                    configString = sr.ReadToEnd();
                }

                if(string.IsNullOrEmpty(configString))
                {
                    Console.Error.WriteLine(".config is empty");
                    Environment.Exit(1);
                }

                var configLines = configString.Split(Environment.NewLine);
                foreach(var line in configLines) 
                {
                    var configLine = line.Split('=');
                    if(configLine.Length != 2)
                    {
                        Console.WriteLine($"Error in config for {line}: must be setting:value");
                    } else
                    {
                        switch(configLine[0].ToUpper())
                        {
                            case "SOURCE":
                                config.SourceFolder = configLine[1];
                                break;
                            case "DESTINATION":
                                config.DestinationFolder= configLine[1];
                                break;
                            case "INTERVAL":
                                config.IntervalInSeconds= Convert.ToInt32(configLine[1]);
                                break;
                            case "AFTERCOPYPOSTURL":
                                config.AfterCopyPostUrl = configLine[1];
                                break;
                            case "AFTERCOPYPOSTBODY":
                                config.AfterCopyPostBody  = configLine[1];
                                break;
                        }
                    }
                }
                return config;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
