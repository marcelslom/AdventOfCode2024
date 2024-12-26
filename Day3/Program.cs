
using System.Text.RegularExpressions;

namespace Day3
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
            var mulRegex = new Regex(@"(mul){1}\([0-9]{1,3},[0-9]{1,3}\)");
            var doRegex = new Regex(@"do\(\)");
            var dontRegex = new Regex(@"don't\(\)");
            var line = string.Join("", input);
            var matches = mulRegex.Matches(line);
            var doMatches = doRegex.Matches(line);
            var dontMatches = dontRegex.Matches(line);
            var concatenated = doMatches.Concat(dontMatches).OrderBy(x => x.Index).ToList();
            var result = 0;
            foreach (var match in matches.Where(x => IsEnabled(x, dontMatches.First(), concatenated)))
            {
                var factors = match.Value
                    .Replace("mul(", "")
                    .Replace(")", "")
                    .Split(',')
                    .Select(int.Parse)
                    .ToList();
                result += factors[0] * factors[1];
            }

            return result.ToString();
        }

        private static bool IsEnabled(Match match, Match firstDontMatch, List<Match> allDoDontMatches)
        {
            if (match.Index < firstDontMatch.Index)
            {
                return true;
            }

            var matchBefore = allDoDontMatches.Last(x => x.Index < match.Index);
            return matchBefore.Value == "do()";
        }

        private static string Part1(string[] input)
        {
            var r = new Regex(@"(mul){1}\([0-9]{1,3},[0-9]{1,3}\)");
            var result = 0;
            foreach (var line in input)
            {
                var matches = r.Matches(line);
                foreach (Match match in matches)
                {
                    var factors = match.Value
                        .Replace("mul(", "")
                        .Replace(")", "")
                        .Split(',')
                        .Select(int.Parse)
                        .ToList();
                    result += factors[0] * factors[1];
                }
            }

            return result.ToString();
        }
    }
}
