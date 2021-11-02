using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class LmaoOkay
    {
        public static string ExecuteCommandSync(object command)
        {
            string result = "";
            try
            {
                string output = "";
                string error = "";

                var p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = "/c " + command;

                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardInput = false;

                p.OutputDataReceived += (a, b) =>
                {
                    output += b.Data;
                    output += Environment.NewLine;
                };

                p.ErrorDataReceived += (a, b) =>
                {
                    error += b.Data;
                    error += Environment.NewLine;
                };

                p.Start();
                p.BeginErrorReadLine();
                p.BeginOutputReadLine();
                p.WaitForExit();

                if(output.Length > 0)
                {
                    result += output;
                }
                if(error.Length > 0)
                {
                    result += error;
                }
            }
            catch (Exception objException)
            {
                // Log the exception
                result += $"Exception: {objException}";
            }

            if (result == null || result.Length < 3 || (result.Count(a => a == '\n' || a == '\t' || a == ' ' || a == '\r') == result.Length)) result = "null";
            // String result = "null" IF the result is empty or full of empty whitespaces.
            return result;
        }

        public static void ExecuteCommandAsync(string command, Action<string> afterTask)
        {
            Thread worker = new Thread(() =>
            {
                afterTask(ExecuteCommandSync(command));
            });

            worker.Start();
        }

        static void Main(string[] args)
        {
        
        }
    }
}
