using Common.Data;
using Common.DataExtensions;
using Common.Enum;
using Common.EnumExtensions;

namespace Day20
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var file = File.ReadAllLines("input.txt");
            var (map, start, end) = LoadInput(file);

            var (route, nodes) = FindRoute(map, start, end);
            Part1(map, route, nodes);
            Part2(route, nodes);
        }

        private static void Part2(List<Point> route, Dictionary<Point, Node> nodes)
        {
            List<int> possibleCheats = [];
            foreach (var point in route)
            {
                var node = nodes[point];
                foreach (var anotherPoint in route.Where(x => x != point))
                {
                    var dist = point.ManhattanDistance(anotherPoint);
                    if (dist > 20)
                    {
                        continue;
                    }
                    var anotherNode = nodes[anotherPoint];
                    if (anotherNode.Cost < node.Cost)
                    {
                        continue;
                    }
                    if (dist >= anotherNode.Cost - node.Cost)
                    {
                        continue;
                    }
                    possibleCheats.Add(anotherNode.Cost - node.Cost - dist);
                }
            }

            var part2result = possibleCheats.Count(x => x >= 100);
            Console.WriteLine($"Part 2: {part2result}");
        }

        private static void Part1(Tile[][] map, List<Point> route, Dictionary<Point, Node> nodes)
        {
            var savedPicoseconds = CountSavedPicosecondsPart1(map, route, nodes);
            var part1result = savedPicoseconds.Count(x => x >= 100);
            Console.WriteLine($"Part 1: {part1result}");
        }

        private static List<int> CountSavedPicosecondsPart1(Tile[][] map, List<Point> route, Dictionary<Point, Node> nodes)
        {
            var pointsHashSet = new HashSet<Point>(route);
            var savedPicoseconds = new List<int>();
            foreach (var point in route)
            {
                var node = nodes[point];
                foreach (var direction in Enum.GetValues<Direction>())
                {
                    var neighbour = direction.GetNeighbour(point);
                    if (neighbour.InBounds(map) && neighbour.FindIn(map) == Tile.Wall)
                    {
                        var secondNeighbour = direction.GetNeighbour(neighbour);
                        if (secondNeighbour.InBounds(map) && secondNeighbour.FindIn(map) == Tile.Empty && pointsHashSet.Contains(secondNeighbour))
                        {
                            var secondNode = nodes[secondNeighbour];
                            if (secondNode.Cost > node.Cost)
                            {
                                savedPicoseconds.Add(secondNode.Cost - node.Cost - 2);
                            }
                        }
                    }
                }
            }

            return savedPicoseconds;
        }

        private static (List<Point> route, Dictionary<Point, Node> nodes) FindRoute(Tile[][] map, Point start, Point end)
        {
            var nodes = new Dictionary<Point, Node>
            {
                [start] = new Node { Cost = 0 }
            };
            var queue = new PriorityQueue<Point, int>();
            queue.Enqueue(start, 0);
            while (queue.Count > 0)
            {
                var currentPoint = queue.Dequeue();
                if (currentPoint == end)
                {
                    break;
                }
                var currentNode = nodes[currentPoint];
                foreach (var direction in Enum.GetValues<Direction>())
                {
                    var neighbour = direction.GetNeighbour(currentPoint);
                    if (!neighbour.InBounds(map) || neighbour.FindIn(map) == Tile.Wall)
                    {
                        continue;
                    }

                    if (!nodes.TryGetValue(neighbour, out var neighbourNode))
                    {
                        neighbourNode = new Node();
                        nodes[neighbour] = neighbourNode;
                    }
                    var newCost = currentNode.Cost + 1;
                    if (neighbourNode.Cost > newCost)
                    {
                        neighbourNode.Cost = newCost;
                        neighbourNode.Parent = currentPoint;
                        queue.Enqueue(neighbour, newCost);
                    }
                }
            }

            List<Point> route = [];
            route.Add(end);
            var endNode = nodes[end];
            var p = endNode.Parent;
            while (p.HasValue)
            {
                route.Add(p.Value);
                p = nodes[p.Value].Parent;
            }

            return (route, nodes);
        }

        public static (Tile[][] map, Point start, Point end) LoadInput(string[] fileContent)
        {
            var map = new Tile[fileContent.Length][];
            var start = new Point();
            var end = new Point();
            for (var y = 0; y < fileContent.Length; y++)
            {
                map[y] = new Tile[fileContent[y].Length];
                for (var x = 0; x < fileContent[y].Length; x++)
                {
                    var element = fileContent[y][x];
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

    internal class Node
    {
        public Point? Parent { get; set; }
        public int Cost { get; set; } = int.MaxValue;
    }

    internal enum Tile
    {
        Empty, Wall
    }
}
