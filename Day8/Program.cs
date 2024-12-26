using System.Numerics;

namespace Day8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var antennas = ParseInput(input);
            var width = input[0].Length;
            var height = input.Length;
            var part1Antinodes = Part1Antinodes(antennas, width, height);
            var part1 = part1Antinodes
                .SelectMany(x => x.Value)
                .Distinct()
                .Count();
            Console.WriteLine($"Part 1: {part1}");
            
            var part2Antinodes = Part2Antinodes(antennas, width, height);
            var part2 = part2Antinodes
                .SelectMany(x => x.Value)
                .Distinct()
                .Count();
            Console.WriteLine($"Part 2: {part2}");
        }

        private static Dictionary<char, List<(int x, int y)>> Part2Antinodes(Dictionary<char, List<(int x, int y)>> antennas, int width, int height)
        {
            var antinodes = new Dictionary<char, List<(int x, int y)>>();
            foreach (var antennaGroup in antennas)
            {
                antinodes[antennaGroup.Key] = [];
                foreach (var antenna in antennaGroup.Value)
                {
                    foreach (var another in antennaGroup.Value.Where(x => x != antenna))
                    {
                        var vector = (x: another.x - antenna.x, y: another.y - antenna.y);
                        var antinode = another;
                        while (true)
                        {
                            antinode = (x: antinode.x + vector.x, y: antinode.y + vector.y);
                            if (antinode.x < 0 || antinode.y < 0 || antinode.x >= width || antinode.y >= height)
                            {
                                break;
                            }
                            antinodes[antennaGroup.Key].Add(antinode);
                        }

                        if (!antinodes[antennaGroup.Key].Contains(antenna))
                        {
                            antinodes[antennaGroup.Key].Add(antenna);
                        }
                        if (!antinodes[antennaGroup.Key].Contains(another))
                        {
                            antinodes[antennaGroup.Key].Add(another);
                        }
                    }
                }
            }
            return antinodes;
        }

        private static Dictionary<char, List<(int x, int y)>> Part1Antinodes(Dictionary<char, List<(int x, int y)>> antennas, int width, int height)
        {
            var antinodes = new Dictionary<char, List<(int x, int y)>>();
            foreach (var antennaGroup in antennas)
            {
                antinodes[antennaGroup.Key] = [];
                foreach (var antenna in antennaGroup.Value)
                {
                    foreach (var another in antennaGroup.Value.Where(x => x != antenna))
                    {
                        var antinode = (x: another.x + another.x - antenna.x, y: another.y + another.y - antenna.y);
                        if (antinode.x < 0 || antinode.y < 0 || antinode.x >= width || antinode.y >= height)
                        {
                            continue;
                        }
                        antinodes[antennaGroup.Key].Add(antinode);
                    }
                }
            }
            return antinodes;
        }

        private static Dictionary<char, List<(int x, int y)>> ParseInput(string[] input)
        {
            var antennas = new Dictionary<char, List<(int x, int y)>>();
            for (var y = 0; y < input.Length; y++)
            {
                for (var x = 0; x < input[y].Length; x++)
                {
                    var item = input[y][x];
                    if (item == '.')
                    {
                        continue;
                    }
                    if (!antennas.ContainsKey(item))
                    {
                        antennas[item] = [];
                    }
                    antennas[item].Add((x, y));
                }
            }
            return antennas;
        }
    }
}
