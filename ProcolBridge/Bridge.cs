using System;

namespace ProcolBridge
{
    public class Bridge
    {
        public event EventHandler<string> OnAppendTerminal;

        public void SendUserInput(string text)
        {
            Log(text);
        }

        public void Log(string message)
        {
            OnAppendTerminal?.Invoke(this, message);
        }
    }
}
