using Common.Data;
using Common.Enum;

namespace Common.EnumExtensions
{
    public static class DirectionExtensions
    {
        public static (int x, int y) GetNeighbour(this Direction direction, (int x, int y) origin)
        {
            var (x, y) = direction.GetDirectionVector();
            return (x: origin.x + x, y: origin.y + y);
        }

        public static Point GetNeighbour(this Direction direction, Point origin)
        {
            var vector = direction.GetDirectionVectorPoint();
            return origin + vector;
        }

        public static bool IsHorizontal(this Direction direction)
        {
            return direction == Direction.Left || direction == Direction.Right;
        }

        public static Direction RotateClockwise(this Direction direction)
        {
            return direction switch
            {
                Direction.Left => Direction.Up,
                Direction.Right => Direction.Down,
                Direction.Up => Direction.Right,
                Direction.Down => Direction.Left,
                _ => throw new NotImplementedException(),
            };
        }

        public static Direction RotateCounterclockwise(this Direction direction)
        {
            return direction switch
            {
                Direction.Left => Direction.Down,
                Direction.Right => Direction.Up,
                Direction.Up => Direction.Left,
                Direction.Down => Direction.Right,
                _ => throw new NotImplementedException(),
            };
        }

        public static Direction Rotate180(this Direction direction)
        {
            return direction switch
            {
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                _ => throw new NotImplementedException(),
            };
        }

        public static Point GetDirectionVectorPoint(this Direction direction)
        {
            return direction switch
            {
                Direction.Left => new Point { X = -1, Y = 0 },
                Direction.Right => new Point { X = 1, Y = 0 },
                Direction.Up => new Point { X = 0, Y = -1 },
                Direction.Down => new Point { X = 0, Y = 1 },
                _ => throw new NotImplementedException(),
            };
        }

        public static (int x, int y) GetDirectionVector(this Direction direction)
        {
            return direction switch
            {
                Direction.Left => (-1, 0),
                Direction.Right => (1, 0),
                Direction.Up => (0, -1),
                Direction.Down => (0, 1),
                _ => throw new NotImplementedException(),
            };
        }

    }
}
