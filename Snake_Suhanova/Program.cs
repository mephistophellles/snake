using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Snake_Suhanova
{
    internal class Program
    {
        public static List<Leaders> Leaders = new List<Leaders>();
        public static List<ViewModelUserSettings> remoteIPAdress = new List<ViewModelUserSettings>();
        public static List<ViewModelGames> viewModelGames = new List<ViewModelGames>();
        public static List<ViewModelGamesPlayer> viewModelGamesPlayer = new List<ViewModelGamesPlayer>();
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
        public static int AddSnake()
        {
            ViewModelGames viewModelGames = new ViewModelGames();
            viewModelGamesPlayer.SnakePlayers = new Snakes()
            {
                Points = new List<Snakes.Point>()
                {
                    new Snakes.Point(){X=30,Y=10},
                    new Snakes.Point(){X=20,Y=10},
                    new Snakes.Point(){X=10,Y=10}
                },
                direction = Snakes.Direction.Start
            };
            viewModelGamesPlayer.Points=new Snakes.Point(new Random().Next(10,783),new Random().Next(10,410));
            viewModelGames.Add(viewModelGamesPlayer);
            return viewModelGames.FindIndex(x=>x==viewModelGamesPlayer);
        }
        public static void Timer()
        {
            while (true)
            {
                // Останавливаем на 100 миллисекунд
                Thread.Sleep(100);

                List<ViewModelGames> RemoteSnakes = viewModelGames.FindAll(x => x.SnakesPlayers.GameOver);
                if (RemoteSnakes.Count > 0)
                {
                    foreach (ViewModelGames DeadSnake in RemoteSnakes)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Отключён пользователь: {remoteIPAddress.Find(x => x.IdSnake == DeadSnake.IdSnake).IPAddress}" +
                            $": {remoteIPAddress.Find(x => x.IdSnake == DeadSnake.IdSnake).Port}");

                        // Удаляем пользователя
                        remoteIPAddress.RemoveAll(x => x.IdSnake == DeadSnake.IdSnake);
                    }
                    viewModelGames.RemoveAll(x => x.SnakesPlayers.GameOver);
                }

                foreach (ViewModelUserSettings User in remoteIPAddress)
                {
                    Snakes Snake = viewModelGames.Find(x => x.IdSnake == User.IdSnake).SnakesPlayers;

                    for (int i = Snake.Points.Count - 1; i >= 0; i--)
                    {
                        if (i != 0)
                        {
                            Snake.Points[i] = Snake.Points[i - 1];
                        }
                        else
                        {
                            int Speed = 10 + (int)Math.Round(Snake.Points.Count / 20f);
                            if (Speed > MaxSpeed) Speed = MaxSpeed;
                            if (Snake.direction == Snakes.Direction.Right)
                            {
                                Snake.Points[i] = new Snakes.Point() { X = Snake.Points[i].X + Speed, Y = Snake.Points[i].Y };
                            }
                            else if (Snake.direction == Snakes.Direction.Down)
                            {
                                Snake.Points[i] = new Snakes.Point() { X = Snake.Points[i].X, Y = Snake.Points[i].Y + Speed };
                            }
                            
                            else if (Snake.direction == Snakes.Direction.Up)
                            {
                                
                                Snake.Points[i] = new Snakes.Point() { X = Snake.Points[i].X, Y = Snake.Points[i].Y - Speed };
                            }
                            
                            else if (Snake.direction == Snakes.Direction.Left)
                            {
                                Snake.Points[i] = new Snakes.Point() { X = Snake.Points[i].X - Speed, Y = Snake.Points[i].Y };
                            }
                        }
                    }
                    if (Snake.Points[0].X <= 0 || Snake.Points[0].X >= 793)
                    {
                        Snake.GameOver = true;
                    }
                    else if (Snake.Points[0].Y <= 0 || Snake.Points[0].Y >= 420)
                    {
                        Snake.GameOver = true;
                    }
                }
            }
        }
        public static void SaveLeaders()
        {
            string json= JsonConvert.SerializeObject(Leaders);
            StreamWriter SW = new StreamWriter("./leaders.txt");
            SW.WriteLine(json);
            SW.Close();
        }
    }
}
