using ProcolBridge.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ProcolBridge.Models
{
    public class CoreBridge
    {
        public ProcolSession Session { get; set; }

        private Process CoreProcess { get; set; }

        private readonly Stream stdinToCore;

        public CoreBridge(ProcolSession session)
        {
            Session = session;

            CoreProcess = new Process();

            CoreProcess.StartInfo.FileName = "C:\\Users\\whunt\\source\\repos\\ProcolWin\\procol-core\\target\\debug\\procol.exe";
            CoreProcess.StartInfo.CreateNoWindow = true;
            CoreProcess.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            CoreProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            CoreProcess.StartInfo.RedirectStandardError = true;
            CoreProcess.StartInfo.RedirectStandardOutput = true;
            CoreProcess.StartInfo.RedirectStandardInput = true;
            CoreProcess.StartInfo.UseShellExecute = false;

            CoreProcess.EnableRaisingEvents = true;
            CoreProcess.ErrorDataReceived += HandleErrorData;
            CoreProcess.OutputDataReceived += HandleStdoutData;
            CoreProcess.Exited += HandleProcessExisted;

            CoreProcess.Start();
            CoreProcess.BeginErrorReadLine();
            CoreProcess.BeginOutputReadLine();

            stdinToCore = CoreProcess.StandardInput.BaseStream;
        }

        private void HandleProcessExisted(object sender, EventArgs e)
        {
            Session.Exit();
        }

        private void HandleErrorData(object sender, DataReceivedEventArgs e)
        {
            HandleData(e.Data);
        }

        private void HandleStdoutData(object sender, DataReceivedEventArgs e)
        {
            HandleData(e.Data);
        }

        private void HandleData(string data)
        {
            Session.Log(data);
        }

        public void SendUserInput(string text)
        {
            var data = Encoding.UTF8.GetBytes($"{text}\n");
            stdinToCore.Write(data, 0, data.Length);
            stdinToCore.Flush();
        }
    }
}
