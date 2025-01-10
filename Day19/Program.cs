using System.Xml.Linq;

namespace Day19
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var file = File.ReadAllLines("input.txt");
            var towels = file[0].Split(", ").ToHashSet();
            var maxTowelLength = towels.Select(x => x.Length).Max();
            var possiblePatterns = file.Skip(2).Where(x => IsDesignPossible(x, towels, maxTowelLength)).ToList();
            Console.WriteLine($"Part 1: {possiblePatterns.Count}");
            var part2result = possiblePatterns.Select(x => CountPossibilities(x, towels, maxTowelLength)).Sum();
            Console.WriteLine($"Part 2: {part2result}");
        }


        public static long CountPossibilities(string design, HashSet<string> towels, int maxTowelsLength)
        {
            var alreadyFound = new Dictionary<int, long>();
            return CountPossibilities(design, 0, towels, maxTowelsLength, alreadyFound);
        }


        public static long CountPossibilities(string design, int position, HashSet<string> towels, int maxTowelsLength, Dictionary<int, long> alreadyFound)
        {
            if (position == design.Length)
            {
                return 1;
            }
            if (alreadyFound.TryGetValue(position, out var count))
            {
                return count;
            }
            var ways = 0L;
            for (var i = 1; i <= Math.Min(maxTowelsLength, design.Length - position); i++)
            {
                var possibleTowel = design.Substring(position, i);
                if (towels.Contains(possibleTowel))
                {
                    ways += CountPossibilities(design, position + i, towels, maxTowelsLength, alreadyFound);
                }
            }
            alreadyFound[position] = ways;
            return ways;
        }

        public static bool IsDesignPossible(string design, HashSet<string> towels, int maxTowelsLength)
        {
            var stack = new Stack<(int position, int length)>();
            var previouslyBacktracked = new HashSet<(int position, int length)>();
            var position = 0;
            var lengthToStart = Math.Min(maxTowelsLength, design.Length - position);
            while (position < design.Length)
            {
                var towelFoundLength = -1;
                for (var i = lengthToStart; i >= 1; i--)
                {
                    var possibleTowel = design.Substring(position, i);
                    if (towels.Contains(possibleTowel))
                    {
                        towelFoundLength = i;
                        break;
                    }
                }

                if (towelFoundLength == -1)
                {
                    while (true)
                    {
                        if (stack.Count == 0)
                        {
                            return false;
                        }
                        var element = stack.Pop();
                        if (!previouslyBacktracked.Contains(element) && element.length > 1)
                        {
                            position = element.position;
                            lengthToStart = element.length - 1;
                            previouslyBacktracked.Add(element);
                            break;
                        }
                    }
                }
                else
                {
                    stack.Push((position, towelFoundLength));
                    position += towelFoundLength;
                    lengthToStart = Math.Min(maxTowelsLength, design.Length - position);
                }
            }
            return true;
        }
    }
}
