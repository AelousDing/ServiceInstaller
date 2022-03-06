using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInstaller.Helper
{
    public static class FileExtension
    {
        public static string GetDirectoryName(this string exePath)
        {
            int index = exePath.LastIndexOf('\\');
            return exePath.Substring(index + 1);

        }
        public static string GetFileName(this string exePath)
        {
            var name = exePath.Split('\\').LastOrDefault();
            if (string.IsNullOrEmpty(name))
            {
                return "";
            }
            int index = name.LastIndexOf(".");
            return name.Substring(0, index);
        }
    }
}
