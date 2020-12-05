using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFileCleanUp
{
    class Config
    {
        private static RegistryKey OpenRegPath()
        {
            var reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\ProjectFileCleanUp", true);
            if (reg == null)
            {
                reg = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\ProjectFileCleanUp", true);
            }
            return reg;
        }
        public static void SaveScanPath(string strBaseDir)
        {
            do
            {
                var reg = OpenRegPath();
                if(reg == null)
                {
                    break;
                }

                string[] oldHistory = (string[])reg.GetValue("ScanHistory");
                var repeatCheck = from history in oldHistory where history.Equals(strBaseDir, StringComparison.OrdinalIgnoreCase) == true select history;
                if(repeatCheck.Count() > 0)
                {
                    break;
                }

                List<string> newHistory = new List<string>();
                newHistory.Add(strBaseDir);

                if(oldHistory != null)
                {
                    int index = 0;
                    foreach (var item in oldHistory)
                    {
                        if (index > 9)
                        {
                            break;
                        }

                        newHistory.Add(item);
                        ++index;
                    }
                }
                
                reg.SetValue("ScanHistory", newHistory.ToArray());
            } while (false);
        }

        public static string[] LoadScanPath()
        {
            string[] historyScan = new string[0];

            do
            {
                var reg = OpenRegPath();
                if (reg == null)
                {
                    break;
                }

                historyScan = (string[])reg.GetValue("ScanHistory", historyScan);
            } while (false);

            return historyScan;
        }
    }
}
