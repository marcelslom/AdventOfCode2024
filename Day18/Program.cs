using Common.Data;
using Common.DataExtensions;
using Common.Enum;
using Common.EnumExtensions;

namespace Day18
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var file = File.ReadAllLines("input.txt");
            var input = file.Select(x => x.Split(",")).Select(splitted => new Point(int.Parse(splitted[0]), int.Parse(splitted[1]))).ToArray();

            var corrupted = input.Take(1024).ToHashSet();
            var start = new Point();
            var end = new Point(70, 70);
            Part1(corrupted, start, end);
            Part2(input, corrupted, start, end);
        }

        private static void Part2(Point[] input, HashSet<Point> corrupted, Point start, Point end)
        {
            foreach (var point in input.Skip(1024))
            {
                corrupted.Add(point);
                var nodes = Astart(corrupted, start, end);
                if (!nodes.ContainsKey(end))
                {
                    Console.WriteLine($"Part 2: {point}");
                    break;
                }
            }
        }

        private static void Part1(HashSet<Point> corrupted, Point start, Point end)
        {
            var nodes = Astart(corrupted, start, end);

            var counter = 0;
            var parentPoint = nodes[end].Parent;
            while (parentPoint != null)
            {
                var node = nodes[parentPoint.Value];
                parentPoint = node.Parent;
                counter++;
            }
            Console.WriteLine($"Part 1: {counter}");
        }

        private static Dictionary<Point, Node> Astart(HashSet<Point> corrupted, Point start, Point end)
        {
            var nodes = new Dictionary<Point, Node>
            {
                [start] = new Node()
            };
            nodes[start].G = 0;
            nodes[start].H = start.ManhattanDistance(end);
            var openSet = new PriorityQueue<Point, int>();
            openSet.Enqueue(start, nodes[start].F);
            while (openSet.Count > 0)
            {
                var currentPoint = openSet.Dequeue();
                if (currentPoint == end)
                {
                    break;
                }
                var currentNode = nodes[currentPoint];

                foreach (var direction in Enum.GetValues<Direction>())
                {
                    var neighbour = direction.GetNeighbour(currentPoint);
                    if (neighbour.X < 0 || neighbour.Y < 0 || neighbour.X > end.X || neighbour.Y > end.Y)
                    {
                        continue;
                    }
                    if (corrupted.Contains(neighbour))
                    {
                        continue;
                    }
                    if (!nodes.TryGetValue(neighbour, out var neighbourNode))
                    {
                        neighbourNode = new Node();
                        nodes[neighbour] = neighbourNode;
                    }
                    var newG = currentNode.G + 1;
                    if (newG < neighbourNode.G)
                    {
                        neighbourNode.G = newG;
                        neighbourNode.H = neighbour.ManhattanDistance(end);
                        neighbourNode.Parent = currentPoint;
                        openSet.Remove(neighbour, out _, out _);
                        openSet.Enqueue(neighbour, neighbourNode.F);
                    }
                }
            }

            return nodes;
        }

        class Node
        {
            public Point? Parent { get; set; }
            public int F => G + H;
            public int G { get; set; } = int.MaxValue;
            public int H { get; set; } = int.MaxValue;

        }
    }
}
