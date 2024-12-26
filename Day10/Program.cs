namespace Day10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var board = new int[input.Length][];
            var startingPoints = new List<(int x, int y)>();
            for (var y = 0; y < input.Length; y++)
            {
                board[y] = new int[input[y].Length];
                for (var x = 0; x < input[y].Length; x++)
                {
                    var value = input[y][x] - '0';
                    board[y][x] = value;
                    if(value == 0)
                    {
                        startingPoints.Add((x, y));
                    }
                }
            }
            var part1Result = 0;
            var part2Result = 0;
            foreach (var startingPoint in startingPoints)
            {
                var reachedEnds = new List<(int x, int y)>();
                LookForEndNode(startingPoint, 0, board, reachedEnds);
                part1Result += reachedEnds.Distinct().Count();
                part2Result += reachedEnds.Count;
            }

            Console.WriteLine($"Part 1: {part1Result}");
            Console.WriteLine($"Part 2: {part2Result}");
        }

        public static void LookForEndNode((int x, int y) point, int expectedValue, int[][] board, List<(int x, int y)> reachedEnds)
        {
            if (point.x < 0 || point.y < 0 || point.y >= board.Length || point.x >= board[point.y].Length)
            {
                return;
            }
            if (board[point.y][point.x] != expectedValue)
            {
                return;
            }
            if (expectedValue == 9)
            {
                reachedEnds.Add(point);
                return;
            }
            LookForEndNode(Move(point, Direction.Left), expectedValue + 1, board, reachedEnds);
            LookForEndNode(Move(point, Direction.Up), expectedValue + 1, board, reachedEnds);
            LookForEndNode(Move(point, Direction.Right), expectedValue + 1, board, reachedEnds);
            LookForEndNode(Move(point, Direction.Down), expectedValue + 1, board, reachedEnds);
        }

        private static (int x, int y) Move((int x, int y) origin, Direction direction)
        {
            var dirVec = GetDirectionVector(direction);
            return (x: origin.x + dirVec.x, y: origin.y + dirVec.y);
        }

        private static (int x, int y) GetDirectionVector(Direction direction)
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

    public enum Direction
    {
        Left, Right, Up, Down
    }
}
