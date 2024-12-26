using Common.Enum;
using Common.EnumExtensions;

namespace Day12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var garden = input.Select(line => line.ToCharArray()).ToArray();
            var processed = garden.Select(x => new bool[x.Length]).ToArray();
            var totalCostPart1 = 0;
            var totalCostPart2 = 0;
            while (true)
            {
                var elementToCheck = processed
                    .Select((row, rowIndex) => new { row, rowIndex })
                    .SelectMany(x => x.row.Select((value, colIndex) => new { x.rowIndex, colIndex, value }))
                    .FirstOrDefault(x => !x.value);
                if (elementToCheck == null)
                {
                    break;
                }
                var context = new ScanContext(garden[elementToCheck.rowIndex][elementToCheck.colIndex]);
                ScanRegion((x: elementToCheck.colIndex, y: elementToCheck.rowIndex), context, garden, processed);
                totalCostPart1 += context.Part1Price;
                totalCostPart2 += context.Part2Price;
            }

            Console.WriteLine($"Part 1: {totalCostPart1}");
            Console.WriteLine($"Part 2: {totalCostPart2}");

        }

        public static void ScanRegion((int x, int y) pointToCheck, ScanContext context, char[][] garden, bool[][] processed)
        {
            if (!IsWithinBounds(pointToCheck, garden))
            {
                return;
            }
            if (processed[pointToCheck.y][pointToCheck.x])
            {
                return;
            }
            if (garden[pointToCheck.y][pointToCheck.x] != context.Plant)
            {
                return;
            }

            context.FenceParts.AddRange(FigureOutFenceParts(pointToCheck, garden));
            context.Area += 1;
            processed[pointToCheck.y][pointToCheck.x] = true;

            foreach (var direction in Enum.GetValues<Direction>())
            {
                var neighbour = direction.GetNeighbour(pointToCheck);
                ScanRegion(neighbour, context, garden, processed);
            }
        }

        public static List<(int x, int y, Orientation orientation, Direction detectDir)> FigureOutFenceParts((int x, int y) point, char[][] garden)
        {
            var result = new List<(int x, int y, Orientation orientation, Direction detectDir)>();
            var neighbour = Direction.Left.GetNeighbour(point);
            if (!IsWithinBounds(neighbour, garden) || garden[point.y][point.x] != garden[neighbour.y][neighbour.x])
            {
                result.Add((point.x, point.y, Orientation.Vertical, Direction.Left));
            }
            neighbour = Direction.Up.GetNeighbour(point);
            if (!IsWithinBounds(neighbour, garden) || garden[point.y][point.x] != garden[neighbour.y][neighbour.x])
            {
                result.Add((point.x, point.y, Orientation.Horizontal, Direction.Up));
            }
            neighbour = Direction.Right.GetNeighbour(point);
            if (!IsWithinBounds(neighbour, garden) || garden[point.y][point.x] != garden[neighbour.y][neighbour.x])
            {
                result.Add((neighbour.x, neighbour.y, Orientation.Vertical, Direction.Right));
            }
            neighbour = Direction.Down.GetNeighbour(point);
            if (!IsWithinBounds(neighbour, garden) || garden[point.y][point.x] != garden[neighbour.y][neighbour.x])
            {
                result.Add((neighbour.x, neighbour.y, Orientation.Horizontal, Direction.Down));
            }

            return result;
        }

        private static bool IsWithinBounds((int x, int y) point, char[][] array)
        {
            return point.y >= 0 && point.x >= 0 & point.y < array.Length && point.x < array[point.y].Length;
        }
    }

    public class ScanContext
    {
        public char Plant { get; init; }
        public int Area { get; set; }
        public List<(int x, int y, Orientation orientation, Direction detectDir)> FenceParts { get; }

        public ScanContext(char plant)
        {
            Plant = plant;
            FenceParts = [];
        }

        public int Part1Price => Area * FenceParts.Count;
        public int Part2Price => Area * CalculateSides();

        public int CalculateSides()
        {
            var result = 0;
            var horizontalGroups = FenceParts
                .Distinct()
                .Where(x => x.orientation == Orientation.Horizontal)
                .GroupBy(x => x.y)
                .ToList();

            foreach (var group in horizontalGroups)
            {
                var ordered = group.OrderBy(x => x.x).ToList();
                result += ordered
                    .Zip(ordered.Skip(1), (first, second) => new { difference = second.x - first.x, sameDirection = second.detectDir == first.detectDir })
                    .Count(x => x.difference != 1 || !x.sameDirection);
                result++;
            }

            var verticalGroups = FenceParts
                .Distinct()
                .Where(x => x.orientation == Orientation.Vertical)
                .GroupBy(x => x.x)
                .ToList();

            foreach (var group in verticalGroups)
            {
                var ordered = group.OrderBy(x => x.y).ToList();
                result += ordered
                    .Zip(ordered.Skip(1), (first, second) => new { difference = second.y - first.y, sameDirection = second.detectDir == first.detectDir })
                    .Count(x => x.difference != 1 || !x.sameDirection);
                result++;
            }

            return result;
        }
    }
}
