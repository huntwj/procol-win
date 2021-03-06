﻿using ProcolBridge;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace ProcolWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IUserInterface
    {
        public ProcolBridge.Bridge Bridge { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Bridge = new ProcolBridge.Bridge(this);

            Bridge.OnAppendTerminal += AddToTerminal;
        }

        public delegate void AddToTerminalDelegate(string message);

        public void AddToTerminal(object sender, string message)
        {
            MainTerminal.Dispatcher.Invoke(
                new AddToTerminalDelegate(this.SimpleAddToTerminal),
                message
               );
        }

        private void SimpleAddToTerminal(string message)
        {
            MainTerminal.Document.Blocks.Add(new Paragraph(new Run(message)));
            MainTerminal.ScrollToEnd();

            while (MainTerminal.Document.Blocks.Count > 100)
            {
                MainTerminal.Document.Blocks.Remove(MainTerminal.Document.Blocks.FirstBlock);
            }
        }

        private void OnKeyUp_UserInput(object sender, KeyEventArgs a)
        {
            if (a.Key == Key.Return)
            {
                Bridge.SendUserInput(input.Text);
                input.Select(0, input.Text.Length);
            }
            else if (a.Key == Key.Escape)
            {
                input.Text = "";
            }
        }

        delegate void ExitDelegate();

        public void Exit()
        {
            MainTerminal.Dispatcher.Invoke(
                new ExitDelegate(Application.Current.Shutdown)
                );
        }
    }
}
