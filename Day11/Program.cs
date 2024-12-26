namespace Day11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt");
            var part1 = SimulateStones(input, 25);
            Console.WriteLine($"Part 1: {part1}");
            var part2 = SimulateStones(input, 75);
            Console.WriteLine($"Part 2: {part2}");
        }

        private static long SimulateStones(string input, int blinks)
        {
            var stones = input.Split(' ').GroupBy(x => x).ToDictionary(x => x.Key, x => (long)x.Count());
            for (var blink = 0; blink < blinks; blink++)
            {
                var newStones = new Dictionary<string, long>();
                foreach (var st in stones)
                {
                    var processed = ProcessStone(st.Key);
                    foreach (var x in processed)
                    {
                        if (newStones.TryGetValue(x, out var value))
                        {
                            newStones[x] = value + st.Value;
                        }
                        else
                        {
                            newStones[x] = st.Value;
                        }
                    }
                }
                stones = newStones;
            }

            return stones.Sum(x => x.Value);
        }

        private static string[] ProcessStone(string stone)
        {
            if (stone == "0")
            {
                return ["1"];
            }
            else if (stone.Length % 2 == 0)
            {
                var result = new string[2];
                var mid = stone.Length / 2;
                result[0] = stone[..mid];
                result[1] = stone[mid..].TrimStart('0');
                if (string.IsNullOrEmpty(result[1]))
                {
                    result[1] = "0";
                }
                return result;
            }
            else
            {
                var value = long.Parse(stone);
                value *= 2024;
                return [value.ToString()];
            }
        }
    }
}
