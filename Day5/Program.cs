using System.Collections.Frozen;

namespace Day5
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
            var rules = input
                .TakeWhile(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Split('|'))
                .Select(x => (x[0], x[1]))
                .GroupBy(x => x.Item2)
                .ToFrozenDictionary(x => x.Key, x => x.Select(xx => xx.Item1).ToArray());


            var result = input
                .SkipWhile(x => !string.IsNullOrWhiteSpace(x))
                .Skip(1)
                .Select(x => x.Split(','))
                .Where(x => !IsUpdateValid(rules, x))
                .Select(x => CorrectUpdate(rules, x))
                .Select(x => x[x.Count / 2])
                .Select(int.Parse)
                .Sum();

            return result.ToString();
        }

        private static List<string> CorrectUpdate(FrozenDictionary<string, string[]> rules, string[] update)
        {
            var result = update.ToList();

            for (var i = 0; i < result.Count; i++)
            {
                if (!rules.TryGetValue(result[i], out var rule))
                {
                    continue;
                }

                for (var j = 0; j < i; j++)
                {
                    if (rule.Contains(result[j]))
                    {
                        (result[i], result[j]) = (result[j], result[i]);
                        i--;
                    }
                }
            }

            result.Reverse();
            return result;
        }

        public static void MoveElement<T>(List<T> list, int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= list.Count || toIndex < 0 || toIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException("Indices are out of range.");
            }

            var item = list[fromIndex];
            list.RemoveAt(fromIndex);

            if (toIndex > fromIndex)
            {
                toIndex--;
            }

            list.Insert(toIndex, item);
        }

        private static string Part1(string[] input)
        {
            var rules = input
                .TakeWhile(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Split('|'))
                .Select(x => (x[0], x[1]))
                .GroupBy(x => x.Item2)
                .ToFrozenDictionary(x => x.Key, x => x.Select(xx => xx.Item1).ToArray());

            var result = input
                .SkipWhile(x => !string.IsNullOrWhiteSpace(x))
                .Skip(1)
                .Select(x => x.Split(','))
                .Where(x => IsUpdateValid(rules, x))
                .Select(x => x[x.Length / 2])
                .Select(int.Parse)
                .Sum();

            return result.ToString();
        }

        public static bool IsUpdateValid(FrozenDictionary<string, string[]> rules, string[] update)
        {
            for (var i = 0; i < update.Length; i++)
            {
                if (!rules.TryGetValue(update[i], out var rule))
                {
                    continue;
                }
                if (rule.Any(x => Array.IndexOf(update, x) > i))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
