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
            if (MainWindow.mainWindow.receivingUdpClient != null) // Если есть соединение 
                MainWindow.mainWindow.receivingUdpClient.Close(); // Закрываем
            if (MainWindow.mainWindow.tRec != null) // Если есть поток
                MainWindow.mainWindow.tRec.Abort(); // Отлючаем
            IPAddress UserIPAdress;
            if (!IPAddress.TryParse(ip.Text, out UserIPAdress))
            {
                MessageBox.Show("Please use the IP address in the format X.X.X.X");
                return;
            }
            int UserPort;
            if (!int.TryParse(port.Text, out UserPort))
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
