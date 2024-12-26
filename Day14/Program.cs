namespace Day14
{
    internal class Program
    {
        public const int BoardWidth = 101; //11;
        public const int BoardHeight = 103; // 7;

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            Part1(input);
            var robots = input.Select(Robot.Parse).ToList();
            var isolated = new int[10000];
            for (var i = 0; i < 10_000; i++)
            {
                foreach (var robot in robots)
                {
                    robot.Move();
                }
                isolated[i] = CountIsolatedPositions(robots.Select(x => x.Position));
            }
            var indices = isolated
                .Select((value, index) => new { Value = value, Index = index })
                .OrderBy(x => x.Value)
                .Take(5)
                .Select(x => x.Index)
                .ToArray();

            robots = input.Select(Robot.Parse).ToList();
            for (var i = 0; i < indices[4] + 10; i++)
            {
                foreach (var robot in robots)
                {
                    robot.Move();
                }
                if (indices.Any(x => i > x - 3 && i < x + 3))
                {
                    PrintToFile(robots, i);
                }
            }

        }

        static int CountIsolatedPositions(IEnumerable<(int x, int y)> positions)
        {
            var positionSet = new HashSet<(int, int)>(positions);
            var count = 0;

            foreach (var (x, y) in positions)
            {
                var hasNeighbor = false;

                for (var dx = -1; dx <= 1; dx++)
                {
                    for (var dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;

                        if (positionSet.Contains((x + dx, y + dy)))
                        {
                            hasNeighbor = true;
                            break;
                        }
                    }

                    if (hasNeighbor) break;
                }

                if (!hasNeighbor) count++;
            }

            return count;
        }

        private static void Part1(string[] input)
        {
            var robots = input.Select(Robot.Parse).ToList();
            for (var i = 0; i < 100; i++)
            {
                foreach (var robot in robots)
                {
                    robot.Move();
                }
            }

            var first = robots.Count(robot => robot.Position.x < BoardWidth / 2 && robot.Position.y < BoardHeight / 2);
            var second = robots.Count(robot => robot.Position.x > BoardWidth / 2 && robot.Position.y < BoardHeight / 2);
            var third = robots.Count(robot => robot.Position.x < BoardWidth / 2 && robot.Position.y > BoardHeight / 2);
            var fourth = robots.Count(robot => robot.Position.x > BoardWidth / 2 && robot.Position.y > BoardHeight / 2);

            var part1 = first * second * third * fourth;
            Console.WriteLine($"Part 1: {part1}");
        }

        private static void PrintToFile(List<Robot> robots, int i)
        {
            var board = Enumerable.Range(0, BoardHeight).Select(x => new char[BoardWidth]).ToArray();
            for (var y = 0; y < board.Length; y++)
            {
                board[y] = new char[BoardWidth];
                for (var x = 0; x < board[y].Length; x++)
                {
                    board[y][x] = '.';
                }
            }
            foreach (var position in robots.Select(x => x.Position))
            {
                board[position.y][position.x] = '#';
            }
            var lines = board.Select(x => new string(x)).ToArray();
            string[] header = [$"\n\nAfter {i + 1} seconds:\n"];
            File.AppendAllLines("output.txt", header);
            File.AppendAllLines("output.txt", lines);
        }

        internal class Robot
        {
            public (int x, int y) Position { get; set; }
            public (int x, int y) Velocity { get; init; }

            public void Move()
            {
                var position = (x: Position.x + Velocity.x, y: Position.y + Velocity.y);
                if (position.x < 0)
                {
                    position = (x: position.x + BoardWidth, y: position.y);
                }
                if (position.x >= BoardWidth)
                {
                    position = (x: position.x - BoardWidth, y: position.y);
                }
                if (position.y < 0)
                {
                    position = (x: position.x, y: position.y + BoardHeight);
                }
                if (position.y >= BoardHeight)
                {
                    position = (x: position.x, y: position.y - BoardHeight);
                }
                Position = position;
            }

            public static Robot Parse(string input)
            {
                var splitted = input.Split(' ');
                var position = splitted[0].Replace("p=", "").Split(',').Select(int.Parse).ToArray();
                var velocity = splitted[1].Replace("v=", "").Split(',').Select(int.Parse).ToArray();
                return new Robot { Position = (position[0], position[1]), Velocity = (velocity[0], velocity[1]) };
            }
        }
    }
}
