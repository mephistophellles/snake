using Newtonsoft.Json;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace SnakeWPF.Pages
{
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.receivingUdpClient?.Close(); // Закрываем
            MainWindow.mainWindow.tRec?.Abort(); // Отлючаем
            if (!IPAddress.TryParse(ip.Text, out IPAddress UserIPAdress))
            {
                MessageBox.Show("Please use the IP address in the format X.X.X.X");
                return;
            }
            if (!int.TryParse(port.Text, out int UserPort))
            {
                MessageBox.Show("Please use th port as a number.");
                return;
            }

            MainWindow.mainWindow.StartReceiver();
            MainWindow.mainWindow.ViewModelUserSettings.IPAdress = ip.Text;
            MainWindow.mainWindow.ViewModelUserSettings.Port = port.Text;
            MainWindow.mainWindow.ViewModelUserSettings.Name = name.Text;
            MainWindow.Send("/start|" + JsonConvert.SerializeObject(MainWindow.mainWindow.ViewModelUserSettings));
        }

    }
}
