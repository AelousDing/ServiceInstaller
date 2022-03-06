using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInstaller.Helper
{
    public class ProcessExtension
    {
        public static bool Start(string fileName, string args, int milliseconds, out string output)
        {
            Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = fileName;
            p.StartInfo.Arguments = args;

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = false;
            p.Start();
            output = p.StandardOutput.ReadToEnd();
            return p.WaitForExit(milliseconds);
        }
        public static bool Cmd(string fileName, string args, int milliseconds, out string output)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/S /C " + args)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true
            };

            var process = new Process { StartInfo = processInfo };
            process.Start();
            output = process.StandardOutput.ReadToEnd();

            return process.WaitForExit(milliseconds);
        }
    }
}
