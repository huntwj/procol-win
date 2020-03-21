using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ProcolBridge
{
    public class Bridge
    {
        public event EventHandler<string> OnAppendTerminal;

        private Process CoreProcess { get; set; }

        private readonly Stream stdinToCore;

        public Bridge()
        {
            CoreProcess = new Process();

            CoreProcess.StartInfo.FileName = "C:\\Users\\whunt\\source\\repos\\ProcolWin\\procol.exe";
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

            CoreProcess.Start();
            CoreProcess.BeginErrorReadLine();
            CoreProcess.BeginOutputReadLine();

            stdinToCore = CoreProcess.StandardInput.BaseStream;
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
            Log(data);
        }

        public void SendUserInput(string text)
        {
            var data = Encoding.UTF8.GetBytes($"{text}\n");
            stdinToCore.Write(data, 0, data.Length);
            stdinToCore.Flush();

            Log(text);
        }

        public void Log(string message)
        {
            OnAppendTerminal?.Invoke(this, message);
        }
    }
}
