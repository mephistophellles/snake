using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Snake_Suhanova
{
    internal class Program
    {
        public static List<Leaders> Leaders = new List<Leaders>();
        public static List<ViewModelUserSettings> remoteIPAdress = new List<ViewModelUserSettings>();
        public static List<ViewModelGames> viewModelGames = new List<ViewModelGames>();
        private static int localPort = 5001;
        public static int MaxSpeed = 15;
        static void Main(string[] args)
        {
        }

        private static void Send()
        {
            foreach (ViewModelUserSettings User in remoteIPAdress)
            {
                UdpClient sender = new UdpClient();
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(User.IPAdress), int.Parse(User.Port));
                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(viewModelGames.Find(x => x.IdSnake == User.IdSnake)));
                    sender.Send(bytes, bytes.Length, endPoint);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Отправил данные пользователя: {User.IPAdress}:{User.Port}");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n" + ex.Message);
                }
                finally
                {
                    sender.Close();
                }
            }
        }
        public static void Receiver()
        {
            // Создаем UdpClient для чтения входящих данных
            UdpClient receivingUdpClient = new UdpClient(localPort);
            IPEndPoint RemoteIpEndPoint = null; // Конечная точка

            try
            {
                Console.WriteLine("Команды сервера:");

                // Запускаем бесконечный цикл для обработки входящих сообщений
                while (true)
                {
                    // Ожидание байт данных
                    byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);

                    // Преобразуем и отображаем данные
                    string returnData = Encoding.UTF8.GetString(receiveBytes);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Получили команду: " + returnData);

                    // Проверяем, является ли команда стартовой
                    if (returnData.Contains("/start"))
                    {
                        // Делим данные на команду и JSON
                        string[] dataMessage = returnData.Split('|');

                        // Конвертируем данные в модель
                        ViewModelUserSettings viewModelUserSettings =
                            JsonConvert.DeserializeObject<ViewModelUserSettings>(dataMessage[1]);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Подключился пользователь: {viewModelUserSettings.IPAddress}:{viewModelUserSettings.Port}");

                        // Добавляем данные в коллекцию
                        remoteIPAddress.Add(viewModelUserSettings);

                        // Добавляем змею
                        viewModelUserSettings.IdSnake = AddSnake();

                        // Создаём запись для игры
                        viewModelGames[viewModelUserSettings.IdSnake].IdSnake = viewModelUserSettings.IdSnake;
                    }
                    else
                    {
                        // Если команда не является стартовой
                        string[] dataMessage = returnData.Split('|');

                        // Конвертируем данные в модель
                        ViewModelUserSettings viewModelUserSettings =
                            JsonConvert.DeserializeObject<ViewModelUserSettings>(dataMessage[1]);

                        // Получаем ID игрока
                        int IdPlayer = remoteIPAddress.FindIndex(x =>
                            x.IPAddress == viewModelUserSettings.IPAddress &&
                            x.Port == viewModelUserSettings.Port);

                        // Если игрок найден
                        if (IdPlayer != -1)
                        {
                            // Обрабатываем команды управления
                            if (dataMessage[0] == "Up" && viewModelGames[IdPlayer].SnakesPlayers.direction != Snakes.Direction.Down)
                            {
                                viewModelGames[IdPlayer].SnakesPlayers.direction = Snakes.Direction.Up;
                            }
                            else if (dataMessage[0] == "Down" && viewModelGames[IdPlayer].SnakesPlayers.direction != Snakes.Direction.Up)
                            {
                                viewModelGames[IdPlayer].SnakesPlayers.direction = Snakes.Direction.Down;
                            }
                            else if (dataMessage[0] == "Left" && viewModelGames[IdPlayer].SnakesPlayers.direction != Snakes.Direction.Right)
                            {
                                viewModelGames[IdPlayer].SnakesPlayers.direction = Snakes.Direction.Left;
                            }
                            else if (dataMessage[0] == "Right" && viewModelGames[IdPlayer].SnakesPlayers.direction != Snakes.Direction.Left)
                            {
                                viewModelGames[IdPlayer].SnakesPlayers.direction = Snakes.Direction.Right;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n" + ex.Message);
            }
        }
    }
}
