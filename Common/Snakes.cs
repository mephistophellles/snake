using System.Collections.Generic;

namespace Common
{
    public class Snakes
    {
        /// <summary> Класс Point </summary>
        public class Point
        {
            /// <summary> Координата X </summary>
            public int X { get; set; }
            /// <summary> Координата Y </summary>
            public int Y { get; set; }

            public Point(int X, int Y)
            {
                this.X = X;
                this.Y = Y;
            }

            public Point() { }
        }

        /// <summary> Направление движения змеи </summary>
        public enum Direction
        {
            Left,
            Right,
            Up,
            Down,
            Start
        }

        /// <summary> Точки, из которых состоит змея </summary>
        public List<Point> Points = new List<Point>();

        /// <summary> Направление движения, в котором движется змея </summary>
        public Direction direction = Direction.Start;

        /// <summary> Переменная, говорящая о том, что игрок закончил игру </summary>
        public bool GameOver = false;
    }
}
