namespace Day4
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
            var counter = 0;
            for (var y = 1; y < input.Length - 1; y++)
            {
                for (var x = 1; x < input[y].Length - 1; x++)
                {
                    if (input[y][x] != 'A')
                    {
                        continue;
                    }
                    if (CheckLeftDiagonal(input, x, y) && CheckRightDiagonal(input, x, y))
                    {
                        counter++;
                    }
                }
            }
            return counter.ToString();
        }

        private static bool CheckLeftDiagonal(string[] input, int x, int y)
        {
            return (input[y - 1][x - 1] == 'M' && input[y + 1][x + 1] == 'S')
                || (input[y - 1][x - 1] == 'S' && input[y + 1][x + 1] == 'M');
        }

        private static bool CheckRightDiagonal(string[] input, int x, int y)
        {
            return (input[y + 1][x - 1] == 'S' && input[y - 1][x + 1] == 'M')
                || (input[y + 1][x - 1] == 'M' && input[y - 1][x + 1] == 'S');
        }

        private static string Part1(string[] input)
        {
            var counter = 0;
            for (var y = 0; y < input.Length; y++)
            {
                for (var x = 0; x < input[y].Length; x++)
                {
                    counter += CheckPosition(input, x, y);
                }
            }
            return counter.ToString();
        }

        private static int CheckPosition(string[] input, int x, int y)
        {
            if (input[y][x] != 'X')
            {
                return 0;
            }
            var counter = 0;
            for (var yDir = -1; yDir <= 1; yDir++)
            {
                for (var xDir = -1; xDir <= 1; xDir++)
                {
                    if (xDir == 0 && yDir == 0)
                    {
                        continue;
                    }
                    if (FindWord(input, x, y, xDir, yDir))
                    {
                        counter++;
                    }
                }
            }
            return counter;
        }

        private static readonly char[] Word = ['X', 'M', 'A', 'S'];

        private static bool FindWord(string[] input, int x, int y, int xDir, int yDir)
        {
            for (var i = 1; i < Word.Length; i++)
            {
                var xToCheck = x + xDir * i;
                if (xToCheck < 0 || xToCheck >= input[y].Length)
                {
                    return false;
                }
                var yToCheck = y + yDir * i;
                if (yToCheck < 0 || yToCheck >= input.Length)
                {
                    return false;
                }
                if (input[yToCheck][xToCheck] != Word[i])
                {
                    return false;
                }
            }
            return true;
        }

    }
}
