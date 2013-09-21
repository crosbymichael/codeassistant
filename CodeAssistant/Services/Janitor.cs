using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CodeAssistant.Services
{
    public class Janitor
    {
        public Janitor() { }

        public static void CleanDirectory(string directoryPath) 
        {
            if (!directoryPath.Contains("CodeAssistant"))
            {
                return;//Sanity check
            }

            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
        }
    }
}
