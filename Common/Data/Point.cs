using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public readonly struct Point
    {
        public int X { get; init; }
        public int Y { get; init; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point() { }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }

        public static Point operator +(Point a, Point b) =>
            new() { X = a.X + b.X, Y = a.Y + b.Y };

        public static bool operator ==(Point a, Point b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Point a, Point b) => a.X != b.X || a.Y != b.Y;
    }
}
