using ProcolBridge;
using System.Windows;

namespace ProcolWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Bridge Bridge { get; set; }

        public App()
        {
            Bridge = new Bridge();
        }
    }
}
