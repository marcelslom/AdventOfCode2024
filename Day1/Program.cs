using System.Collections.Frozen;

namespace Day1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var part1 = Part1(input);
            Console.WriteLine($"Part 1: {part1}");
            var part2 = Part2(input);
            Console.WriteLine($"Part 2: {part2}");
        }

        private static string Part2(string[] input)
        {
            var parsed = input
                .Select(x => x.Split("   "))
                .Select(x => new { LeftElement = int.Parse(x[0]), RightElement = int.Parse(x[1]) });

            var similarities = parsed
                .Select(x => x.RightElement)
                .GroupBy(x => x)
                .ToFrozenDictionary(x => x.Key, x => x.Count());

            var result = parsed
                .Select(x => x.LeftElement)
                .Select(x => x * (similarities.TryGetValue(x, out var value) ? value : 0))
                .Sum();

            return result.ToString();
        }

        private static string Part1(string[] input)
        {
            var parsed = input
                .Select(x => x.Split("   "))
                .Select(x => new { LeftElement = int.Parse(x[0]), RightElement = int.Parse(x[1]) });

            var leftColumnSorted = parsed
                .Select(x => x.LeftElement)
                .Order()
                .ToList();

            var rightColumnSorted = parsed
                .Select(x => x.RightElement)
                .Order()
                .ToList();

            var result = leftColumnSorted
                .Zip(rightColumnSorted, (left, right) => Math.Abs(left - right))
                .Sum();
                
            return result.ToString();
        }
    }
}
