using Common.Enum;
using Common.EnumExtensions;
using System.Runtime.CompilerServices;

namespace Day15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            Part1(input);
            Part2(input);
        }

        private static void Part2(string[] input)
        {
            var newInput = input.Select(x => x.Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@.")).ToArray();
            var (map, moves, robotPosition) = LoadData(newInput);
            foreach (var move in moves)
            {
                var direction = ToDirection(move);
                var destination = direction.GetNeighbour(robotPosition);
                if (map[destination.y][destination.x] == Tile.Empty)
                {
                    robotPosition = destination;
                }
                else if (map[destination.y][destination.x] == Tile.Wall)
                {
                    continue;
                }
                else if (map[destination.y][destination.x] == Tile.BoxLeft || map[destination.y][destination.x] == Tile.BoxRight)
                {
                    var vector = direction.GetDirectionVector();
                    if (direction.IsHorizontal())
                    {
                        var gap = FindPlaceToMoveBoxes(map, destination, direction);
                        if (gap.x == -1)
                        {
                            continue;
                        }
                        var pos = gap.x;
                        while (pos != robotPosition.x)
                        {
                            map[gap.y][pos] = map[gap.y][pos - vector.x];
                            pos -= vector.x;
                        }
                        robotPosition = destination;
                    }
                    else
                    {
                        var boxesToMove = FindBoxesToMove(map, destination, vector);
                        if (boxesToMove == null)
                        {
                            continue;
                        }
                        foreach (var box in boxesToMove.Reverse())
                        {
                            map[box.y + vector.y][box.x] = map[box.y][box.x];
                            map[box.y][box.x] = Tile.Empty;
                        }
                        robotPosition = destination;
                    }
                }
            }
            var part2result = 0L;
            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == Tile.BoxLeft)
                    {
                        part2result += 100 * y + x;
                    }
                }
            }
            Console.WriteLine($"Part 2: {part2result}");
        }

        private static IEnumerable<(int x, int y)>? FindBoxesToMove(Tile[][] map, (int x, int y) destination, (int x, int y) dirVector)
        {
            var boxesToMove = new List<(int x, int y)>();

            var boxesFromRowToCheck = new List<(int x, int y)>
            {
                destination
            };
            if (map[destination.y][destination.x] == Tile.BoxLeft)
            {
                boxesFromRowToCheck.Add(Direction.Right.GetNeighbour(destination));
            }
            else
            {
                boxesFromRowToCheck.Add(Direction.Left.GetNeighbour(destination));
            }
            var rowToCheck = destination.y + dirVector.y;
            while (true)
            {
                var scannedRowBoxes = new List<(int x, int y)>();
                foreach(var box in boxesFromRowToCheck)
                {
                    var posToCheck = (x: box.x, y: box.y + dirVector.y);
                    if (map[posToCheck.y][posToCheck.x] == Tile.BoxLeft)
                    {
                        scannedRowBoxes.Add(posToCheck);
                        scannedRowBoxes.Add(Direction.Right.GetNeighbour(posToCheck));
                    }
                    else if (map[posToCheck.y][posToCheck.x] == Tile.BoxRight)
                    {
                        scannedRowBoxes.Add(posToCheck);
                        scannedRowBoxes.Add(Direction.Left.GetNeighbour(posToCheck));
                    }
                    else if (map[posToCheck.y][posToCheck.x] == Tile.Wall)
                    {
                        return null;
                    }
                }
                boxesToMove.AddRange(boxesFromRowToCheck.Distinct());
                if (scannedRowBoxes.Count == 0)
                {
                    return boxesToMove;
                }
                boxesFromRowToCheck = scannedRowBoxes;
                rowToCheck += dirVector.y;
            }
        }

        private static void Part1(string[] input)
        {
            var (map, moves, robotPosition) = LoadData(input);
            foreach (var move in moves)
            {
                var direction = ToDirection(move);
                var destination = direction.GetNeighbour(robotPosition);
                if (map[destination.y][destination.x] == Tile.Empty)
                {
                    robotPosition = destination;
                }
                else if (map[destination.y][destination.x] == Tile.Wall)
                {
                    continue;
                }
                else if (map[destination.y][destination.x] == Tile.Box)
                {
                    var gap = FindPlaceToMoveBoxes(map, destination, direction);
                    if (gap.x == -1)
                    {
                        continue;
                    }
                    map[gap.y][gap.x] = Tile.Box;
                    map[destination.y][destination.x] = Tile.Empty;
                    robotPosition = destination;
                }
            }

            var part1result = 0L;
            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == Tile.Box)
                    {
                        part1result += 100 * y + x;
                    }
                }
            }
            Console.WriteLine($"Part 1: {part1result}");
        }

        public static (int x, int y) FindPlaceToMoveBoxes(Tile[][] map, (int x, int y) startPoint, Direction direction)
        {
            var vector = direction.GetDirectionVector();
            if (direction.IsHorizontal())
            {
                for (var i = startPoint.x; i < map[startPoint.y].Length || i >= 0; i += vector.x) 
                {
                    if (map[startPoint.y][i] == Tile.Empty)
                    {
                        return (i, startPoint.y);
                    }
                    else if (map[startPoint.y][i] == Tile.Wall)
                    {
                        return (-1, -1);
                    }
                }
            }
            else
            {
                for (var i = startPoint.y; i < map.Length || i >= 0; i += vector.y)
                {
                    if (map[i][startPoint.x] == Tile.Empty)
                    {
                        return (startPoint.x, i);
                    }
                    else if (map[i][startPoint.x] == Tile.Wall)
                    {
                        return (-1, -1);
                    }
                }
            }
            throw new Exception();
        }

        private static Direction ToDirection(char move)
        {
            return move switch
            {
                '^' => Direction.Up,
                'v' => Direction.Down,
                '<' => Direction.Left,
                '>' => Direction.Right,
                _ => throw new Exception()
            };
        }

        private static (Tile[][] map, string moves, (int x, int y) robotPosition) LoadData(string[] input)
        {
            List<Tile[]> map = [];
            var moves = string.Empty;
            var parsingBoard = true;
            (int x, int y) robotPosition = (0, 0);
            for (var i = 0; i < input.Length; i++)
            {
                var line = input[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    parsingBoard = false;
                    continue;
                }

                if (parsingBoard)
                {
                    var parsed = new Tile[line.Length];
                    for (var j = 0; j < parsed.Length; j++)
                    {
                        parsed[j] = line[j] switch
                        {
                            '@' => Tile.Empty,
                            '#' => Tile.Wall,
                            'O' => Tile.Box,
                            '.' => Tile.Empty,
                            '[' => Tile.BoxLeft,
                            ']' => Tile.BoxRight,
                            _ => throw new Exception()
                        };
                        if (line[j] == '@')
                        {
                            robotPosition = (j, i);
                        }
                    }
                    map.Add(parsed);
                }
                else
                {
                    moves += line;
                }
            }

            return (map.ToArray(), moves, robotPosition);
        }
    }

    internal enum Tile
    {
        Empty, Box, Wall, BoxLeft, BoxRight
    }
}
