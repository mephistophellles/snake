﻿using Common;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SnakeWPF.Pages
{
    public partial class Game : Page
    {
        public int StepCadr = 0;
        public Game()
        {
            InitializeComponent();

        }

        public void CreateUI()
        {
            Dispatcher.Invoke(() =>
            {
                if (StepCadr == 0) StepCadr = 1;
                else StepCadr = 0;

                Canvas.Children.Clear();


                for (int iPoint = MainWindow.mainWindow.ViewModelGames.SnakesPlayers.Points.Count - 1; iPoint >= 0; iPoint--)
                {
                    Snakes.Point SnakePoint = MainWindow.mainWindow.ViewModelGames.SnakesPlayers.Points[iPoint];
                    if (iPoint != 0)
                    {
                        Snakes.Point NextSnakePoint = MainWindow.mainWindow.ViewModelGames.SnakesPlayers.Points[iPoint - 1];
                        if (SnakePoint.X > NextSnakePoint.X || SnakePoint.X < NextSnakePoint.X)
                        {
                            if (iPoint % 2 == 0)
                            {
                                if (StepCadr % 2 == 0) SnakePoint.Y -= 1;
                                else SnakePoint.Y += 1;
                            }
                            else
                            {
                                if (StepCadr % 2 == 0) SnakePoint.Y += 1;
                                else SnakePoint.Y -= 1;
                            }
                        }
                        else if (SnakePoint.Y > NextSnakePoint.Y || SnakePoint.Y < NextSnakePoint.Y)
                        {
                            if (iPoint % 2 == 0)
                            {
                                if (StepCadr % 2 == 0) SnakePoint.X -= 1;
                                else SnakePoint.X += 1;
                            }
                            else
                            {
                                if (StepCadr % 2 == 0) SnakePoint.X += 1;
                                else SnakePoint.X -= 1;
                            }
                        }
                    }

                    if (iPoint == 0)
                    {
                        Image headImage = new Image()
                        {
                            Width = 25,
                            Height = 25,
                            Margin = new Thickness(SnakePoint.X - 10, SnakePoint.Y - 10, 0, 0),
                            Source = new BitmapImage(new Uri("pack://application:,,,/SnakeWPF;component/Image/snake-face.png")),
                            Stretch = Stretch.Uniform
                        };

                        headImage.Effect = new DropShadowEffect
                        {
                            Color = Colors.Black,
                            BlurRadius = 5,
                            ShadowDepth = 3,
                            Opacity = 0.4
                        };

                        Canvas.Children.Add(headImage);
                    }
                    else
                    {
                        Brush Color = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 198, 19));
                        Ellipse ellipse = new Ellipse()
                        {
                            Width = 20,
                            Height = 20,
                            Margin = new Thickness(SnakePoint.X - 10, SnakePoint.Y - 10, 0, 0),
                            Fill = Color,
                            Stroke = Brushes.Black
                        };

                        // Добавляем эффект тени
                        ellipse.Effect = new DropShadowEffect
                        {
                            Color = Colors.Black,
                            BlurRadius = 5,
                            ShadowDepth = 3,
                            Opacity = 0.4
                        };

                        Canvas.Children.Add(ellipse);
                    }
                }

                Image points = new Image()
                {
                    Width = 40,
                    Height = 40,
                    Margin = new Thickness(
                    MainWindow.mainWindow.ViewModelGames.Points.X - 20,
                    MainWindow.mainWindow.ViewModelGames.Points.Y - 20, 0, 0),
                    Source = new BitmapImage(new Uri("pack://application:,,,/SnakeWPF;component/Image/Apple.png")),
                    Stretch = Stretch.Uniform
                };
                Canvas.Children.Add(points);

                foreach (var snake in MainWindow.mainWindow.ViewModelGamesList)
                {
                    for (int iPoint = snake.SnakesPlayers.Points.Count - 1; iPoint >= 0; iPoint--)
                    {
                        Snakes.Point SnakePoint = snake.SnakesPlayers.Points[iPoint];

                        if (iPoint != 0)
                        {
                            Snakes.Point NextSnakePoint = snake.SnakesPlayers.Points[iPoint - 1];
                            if (SnakePoint.X > NextSnakePoint.X || SnakePoint.X < NextSnakePoint.X)
                            {
                                if (iPoint % 2 == 0)
                                {
                                    if (StepCadr % 2 == 0) SnakePoint.Y -= 1;
                                    else SnakePoint.Y += 1;
                                }
                                else
                                {
                                    if (StepCadr % 2 == 0) SnakePoint.Y += 1;
                                    else SnakePoint.Y -= 1;
                                }
                            }
                            else if (SnakePoint.Y > NextSnakePoint.Y || SnakePoint.Y < NextSnakePoint.Y)
                            {
                                if (iPoint % 2 == 0)
                                {
                                    if (StepCadr % 2 == 0) SnakePoint.X -= 1;
                                    else SnakePoint.X += 1;
                                }
                                else
                                {
                                    if (StepCadr % 2 == 0) SnakePoint.X += 1;
                                    else SnakePoint.X -= 1;
                                }
                            }
                        }

                        Brush Color;

                        if (iPoint == 0)
                        {
                            Image headImage = new Image()
                            {
                                Width = 25,
                                Height = 25,
                                Margin = new Thickness(SnakePoint.X - 10, SnakePoint.Y - 10, 0, 0),
                                Source = new BitmapImage(new Uri("pack://application:,,,/SnakeWPF;component/Image/snake-face.png")),
                                Stretch = Stretch.Uniform
                            };

                            headImage.Effect = new DropShadowEffect
                            {
                                Color = Colors.Black,
                                BlurRadius = 5,
                                ShadowDepth = 3,
                                Opacity = 0.4
                            };

                            Canvas.Children.Add(headImage);
                        }
                        else
                        {
                            Color = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 198, 19));

                            Ellipse ellipse = new Ellipse()
                            {
                                Width = 20,
                                Height = 20,
                                Margin = new Thickness(SnakePoint.X - 10, SnakePoint.Y - 10, 0, 0),
                                Fill = Color,
                                Stroke = Brushes.Black
                            };
                            Canvas.Children.Add(ellipse);
                        }
                    }
                }
            });
        }
    }
}
