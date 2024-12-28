using Common.Data;
using Common.DataExtensions;
using Common.Enum;
using Common.EnumExtensions;

namespace Day16
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var (map, start, end) = LoadInput(input);
            
            var result = Solve(map, start, end);
            Console.WriteLine(result);
        }

        private static (int part1, int part2)  Solve(Tile[][] map, Point start, Point end)
        {
            var queue = new SortedDictionary<int, List<(Point point, Direction dir)>>();
            Enqueue(queue, (start, Direction.Right), 0);
            var costs = new Dictionary<(Point point, Direction dir), int>
            {
                [(start, Direction.Right)] = 0
            };

            while (queue.Count > 0)
            {
                var current = Dequeue(queue, out var cost);

                if (costs.TryGetValue((current.point, current.dir), out var oldCost) && cost > oldCost)
                {
                    continue;
                }

                var pointsToCheck = new[]
                {
                    ((point: current.dir.GetNeighbour(current.point), dir: current.dir), cost + 1),
                    ((point: current.point, dir: current.dir.RotateClockwise()), cost + 1000),
                    ((point :current.point, dir: current.dir.RotateCounterclockwise()), cost + 1000)
                };

                foreach (var pointToCheck in pointsToCheck)
                {
                    if (!pointToCheck.Item1.point.InBounds(map) ||
                        map[pointToCheck.Item1.point.Y][pointToCheck.Item1.point.X] != Tile.Empty)
                    {
                        continue;
                    }

                    if (!costs.TryGetValue(pointToCheck.Item1, out oldCost) || oldCost > pointToCheck.Item2)
                    {
                        costs[pointToCheck.Item1] = pointToCheck.Item2;
                        Enqueue(queue, pointToCheck.Item1, pointToCheck.Item2);
                    }
                }
            }

            var part1 = costs.Where(kvp => kvp.Key.point == end).MinBy(kvp => kvp.Value);
            var part1result = part1.Value;

            var visited = new HashSet<Point>();
            var pathQueue = new Queue<(Point point, Direction direction)>();
            pathQueue.Enqueue((part1.Key.point, part1.Key.dir));

            while (pathQueue.Count > 0)
            {
                var (currentPoint, currentDir) = pathQueue.Dequeue();
                var currentCost = costs[(currentPoint, currentDir)];

                visited.Add(currentPoint);

                foreach (var dir in Enum.GetValues<Direction>())
                {
                    if (dir == currentDir) continue;
                    var neighbour = dir.GetNeighbour(currentPoint);
                    var costIncrease = currentDir.IsHorizontal() ^ dir.IsHorizontal() ? 1001 : 1;
                    if (costs.TryGetValue((neighbour, dir.GetOpposedDirection()), out var neighbourCost) && neighbourCost + costIncrease == currentCost)
                    {
                        pathQueue.Enqueue((neighbour, dir.GetOpposedDirection()));
                    }
                }
            }

            var part2result = visited.Count;

            return (part1result, part2result);
        }

        static void Enqueue(SortedDictionary<int, List<(Point point, Direction dir)>> dict, (Point point, Direction dir) item, int priority)
        {
            if (!dict.ContainsKey(priority))
            {
                dict[priority] = [];
            }
            dict[priority].Add(item);
        }

        static (Point point, Direction dir) Dequeue(SortedDictionary<int, List<(Point point, Direction dir)>> dict, out int priority)
        {
            if (dict.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty.");
            }

            var (p, list) = dict.First();

            var item = list[0];
            list.RemoveAt(0);

            if (list.Count == 0)
            {
                dict.Remove(p);
            }
            priority = p;
            return item;
        }

        private static (Tile[][] map, Point start, Point end) LoadInput(string[] input)
        {
            var map = new Tile[input.Length][];
            var start = new Point();
            var end = new Point();
            for (var y = 0; y < input.Length; y++)
            {
                map[y] = new Tile[input[y].Length];
                for (var x = 0; x < input[y].Length; x++)
                {
                    var element = input[y][x];
                    map[y][x] = element == '#' ? Tile.Wall : Tile.Empty;
                    if (element == 'S')
                    {
                        start = new Point(x, y);
                    }
                    else if (element == 'E')
                    {
                        end = new Point(x, y);
                    }
                }
            }

            return (map, start, end);
        }
    }

    internal enum Tile
    {
        Empty, Wall
    }
}
