using System.Numerics;

namespace Day7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var parsed = input
                .Select(input => input.Split(": "))
                .Select(splitted => new { Result = long.Parse(splitted[0]), Operands = splitted[1].Split(" ").Select(long.Parse).ToArray() })
                .ToList();
             var part1 = parsed   
                .Select(x => new Line(x.Result, x.Operands, ['+', '*']))
                .Select(x => x.TryCalculate(out var result) ? result : 0)
                .Sum();
            Console.WriteLine($"Part 1: {part1}");
            var part2 = parsed
               .Select(x => new Line(x.Result, x.Operands, ['+', '*', '|']))
               .Select(x => x.TryCalculate(out var result) ? result : 0)
               .Sum();
            Console.WriteLine($"Part 2: {part2}");
        }
    }

    class Line
    {
        private long expectedValue;
        private long[] operands;
        private readonly char[] operators;

        public Line(long expectedValue, long[] operands, char[] operators)
        {
            this.expectedValue = expectedValue;
            this.operands = operands;
            this.operators = operators;
        }

        public bool TryCalculate(out long result)
        {
            var operatorsList = GenerateOperatorsCombinations(operands.Length -  1);
            foreach(var operators in operatorsList)
            {
                var c = Calculate(operators);
                if (c == expectedValue)
                {
                    result = c;
                    return true;
                }
            }
            result = 0;
            return false;
        }

        public List<string> GenerateOperatorsCombinations(int n)
        {
            var result = new List<string>();
            int totalCombinations = (int)Math.Pow(operators.Length, n);

            for (int i = 0; i < totalCombinations; i++)
            {
                string combination = "";
                int temp = i;

                for (int j = 0; j < n; j++)
                {
                    combination = operators[temp % operators.Length] + combination;
                    temp /= operators.Length;
                }

                result.Add(combination);
            }
            return result;
        }

        public long Calculate(string operators)
        {
            if (operators.Length != operands.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(operators));
            }
            var result = operands[0];
            for (var i = 0; i < operators.Length; i++)
            {
                result = operators[i] switch
                {
                    '+' => result + operands[i + 1],
                    '*' => result * operands[i + 1],
                    '|' => Concatenate(result, operands[i+1]),
                    _ => throw new NotImplementedException()
                };
            }
            return result;
        }

        private static long Concatenate(long left, long right)
        {
            return long.Parse(left.ToString() + right.ToString());
        }

    }
}
