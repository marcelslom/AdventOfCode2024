
namespace Day2
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
            var result = input
                .Select(x => x.Split(" ").Select(xx => int.Parse(xx)).ToArray())
                .Count(IsSafePart2);
            return result.ToString();
        }

        private static string Part1(string[] input)
        {
            var result = input
                .Select(x => x.Split(" ").Select(xx => int.Parse(xx)).ToArray())
                .Count(IsSafe);

            return result.ToString();
        }

        private static bool IsSafePart2(int[] report)
        {
            if (IsSafe(report))
            {
                return true;
            }

            for (var i = 0; i < report.Length; i++)
            {
                if (IsSafe(SkipIndex(report, i)))
                {
                    return true;
                }
            }

            return false;
        }

        private static int[] SkipIndex(int[] report, int index)
        {
            return report.Take(index).Concat(report.Skip(index + 1)).ToArray();
        }

        private static bool IsSafe(int[] report)
        {
            var differences = report
                .Zip(report.Skip(1), (prev, next) => next - prev);

            return differences.All(diff => diff >= 1 && diff <= 3) || differences.All(diff => diff >= -3 && diff <= -1);
        }
    }
}
