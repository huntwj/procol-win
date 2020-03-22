using ProcolBridge.Models;
using System;

namespace ProcolBridge.ViewModels
{

    public class ProcolSession
    {
        public event EventHandler<string> OnAppendTerminal;


        private CoreBridge Bridge { get; set; }

        private IUserInterface UI { get; set; }


        public ProcolSession(IUserInterface ui)
        {
            UI = ui;

            Bridge = new CoreBridge(this);
        }

        public void Exit()
        {
            UI.Exit();
        }

        public void SendUserInput(string text)
        {
            Bridge.SendUserInput(text);
        }

        public void Log(string message)
        {
            OnAppendTerminal?.Invoke(this, message);
        }
    }
}
