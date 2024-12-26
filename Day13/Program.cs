namespace Day13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var machines = new List<ClawMachine>();
            var machine = new ClawMachine();
            foreach(var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    machines.Add(machine);
                    machine = new ClawMachine();
                    continue;
                }
                if (line.StartsWith("Button A: ")) 
                {
                    machine.ButtonA = ParseLine(line, "Button A: ", '+', '+');
                }
                else if (line.StartsWith("Button B: ")) 
                {
                    machine.ButtonB = ParseLine(line, "Button B: ", '+', '+');
                }
                else if (line.StartsWith("Prize: ")) 
                {
                    machine.Prize = ParseLine(line, "Prize: ", '=', '=');
                }
                else throw new Exception(line);
            }
            machines.Add(machine);

            var part1 = machines.Select(x => x.Part1()).Where(x => x != -1).Sum();
            var part2 = machines.Select(x => x.Part2()).Where(x => x != -1).Sum();

            Console.WriteLine($"Part 1: {part1}");
            Console.WriteLine($"Part 2: {part2}");
        }

        private static (int x, int y) ParseLine(string line, string header, char xChar, char yChar)
        {
            var splitted = line.Replace(header, "").Split(", ");
            var x = int.Parse(splitted[0].Replace($"X{xChar}", ""));
            var y = int.Parse(splitted[1].Replace($"Y{yChar}", ""));
            return (x, y);
        }
    }

    public class ClawMachine
    {
        public (int x, int y) ButtonA { get; set; }
        public (int x, int y) ButtonB { get; set; }
        public (int x, int y) Prize { get; set; }

        public long Part1()
        {
            var result = SimulateGame(0);
            if (result.a == -1)
            {
                return -1;
            }
            if (result.a > 100 || result.b > 100)
            {
                return -1;
            }
            return result.a * 3 + result.b;
        }

        public long Part2()
        {
            var result = SimulateGame(10000000000000L);
            if (result.a == -1)
            {
                return -1;
            }
            return result.a * 3 + result.b;
        }

        private (long a, long b) SimulateGame(long adder)
        {
            // Solution based on Cramer's rule to solve 2 exuations with 2 unknowns
            var mainDeterminant = ButtonA.x * ButtonB.y - ButtonA.y * ButtonB.x;
            if (mainDeterminant == 0)
            {
                return (-1, 0);
            }
            var prizeX = Prize.x + adder;
            var prizeY = Prize.y + adder;
            var aDeterminant = prizeX * ButtonB.y - ButtonB.x * prizeY;
            if (aDeterminant % mainDeterminant != 0)
            {
                return (-1, 0);
            }
            var bDeterminant = ButtonA.x * prizeY - ButtonA.y * prizeX;
            if (bDeterminant % mainDeterminant != 0)
            {
                return (-1, 0);
            }

            var a = aDeterminant / mainDeterminant;
            var b = bDeterminant / mainDeterminant;

            return (a, b);
        }
    }
}
