﻿using System.Collections.Generic;

namespace Common
{
    public class ViewModelGames
    {
        public Snakes SnakesPlayers = new Snakes();
        public Snakes.Point Points = new Snakes.Point();
        public int Top = 0;
        public int IdSnake { get; set; }
        public List<ViewModelGames> AllSnakes { get; set; } = new List<ViewModelGames>();

    }
}
