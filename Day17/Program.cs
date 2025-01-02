using System.Linq;
using System.Reflection.PortableExecutable;

namespace Day17
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var file = File.ReadAllLines("input.txt");
            var input = ParseInput(file);

            Part1(input);

            Part2(input);
        }

        private static void Part2((int[] program, long a, long b, long c) input)
        {
            List<long> possibleAs = [0L];
            for (var i = 0; i < input.program.Length; i++)
            {
                List<long> longs = [];
                foreach (var possibleA in possibleAs)
                {
                    for (var adder = 0; adder < 8; adder++)
                    {
                        var newA = possibleA * 8 + adder;
                        var machine = new Machine(newA, 0, 0);
                        machine.Execute(input.program);
                        var canWork = input.program.Skip(input.program.Length - machine.Outputs.Count).SequenceEqual(machine.Outputs);
                        if (canWork)
                        {
                            longs.Add(newA);
                        }
                    }
                }
                possibleAs = longs;
            }

            var result = possibleAs.Min();
            Console.WriteLine($"Part 2: {result}");
        }

        private static void Part1((int[] program, long a, long b, long c) input)
        {
            var machine = new Machine(input.a, input.b, input.c);
            machine.Execute(input.program);
            Console.WriteLine($"Part 1: {string.Join(',', machine.Outputs)}");
        }

        private static (int[] program, long a, long b, long c) ParseInput(string[] input)
        {
            var a = 0L;
            var b = 0L;
            var c = 0L;
            int[] program = [];
            foreach (var line in input)
            {
                if (line.StartsWith("Register A"))
                {
                    a = long.Parse(line.Replace("Register A: ", "").Trim());
                }
                else if (line.StartsWith("Register B"))
                {
                    b = long.Parse(line.Replace("Register B: ", "").Trim());
                }
                else if (line.StartsWith("Register C"))
                {
                    c = long.Parse(line.Replace("Register C: ", "").Trim());
                }
                else if (line.StartsWith("Program"))
                {
                    program = line.Replace("Program: ", "").Split(",").Select(int.Parse).ToArray();
                }
            }
            return (program, a, b, c);
        }
    }

    class Machine
    {
        private long a;
        private long b;
        private long c;
        private int pc = 0;
        public List<int> Outputs { get; } = [];

        public Machine(long a, long b, long c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        internal void Execute(int[] program)
        {
            //PrintProcessorState();
            while (pc < program.Length - 1)
            {
                var instruction = program[pc];
                var operand = program[pc + 1];
                switch (instruction)
                {
                    case 0:
                    {
                        //Console.WriteLine($"ADV A {GetComboName(operand)} - combo");
                        var decoded = DecodeComboOperand(operand);
                        a = a >> (int)decoded;
                        break;
                    }
                    case 1:
                        //Console.WriteLine($"BXL B {operand} - literal");
                        b ^= operand;
                        break;
                    case 2:
                        //Console.WriteLine($"BST {GetComboName(operand)} - combo");
                        b = DecodeComboOperand(operand) % 8;
                        break;
                    case 3:
                        if (a != 0)
                        {
                            //Console.WriteLine($"JNZ {operand} - literal");
                            pc = operand;
                            //PrintProcessorState();
                            continue;
                        }
                        break;
                    case 4:
                        //Console.WriteLine($"BXC B C");
                        b ^= c;
                        break;
                    case 5:
                        var outValue = DecodeComboOperand(operand) % 8;
                        //Console.WriteLine($"OUT {GetComboName(operand)} - combo; {outValue} - outValue");
                        Outputs.Add((int)outValue);
                        break;
                    case 6:
                    {
                        //Console.WriteLine($"BDV B {GetComboName(operand)} - combo");
                        var decoded = DecodeComboOperand(operand);
                        b = a >> (int) decoded;
                        break;
                    }
                    case 7:
                    {
                        //Console.WriteLine($"CDV C {GetComboName(operand)} - combo");
                        var decoded = DecodeComboOperand(operand);
                        c = a >> (int) decoded;
                        break;
                    }
                }
                //PrintProcessorState();
                pc += 2;
            }
        }

        public void PrintProcessorState()
        {
            var state = $"A: {Convert.ToString(a, 8)}, B: {Convert.ToString(b, 8)}, C: {Convert.ToString(c, 8)}";
            Console.WriteLine(state);
        }

        private string GetComboName(int operand)
        {
            return operand switch
            {
                >= 0 and <= 3 => "Returning value " + operand.ToString(),
                4 => "a",
                5 => "b",
                6 => "c",
                7 => throw new Exception(),
                _ => throw new ArgumentOutOfRangeException(nameof(operand))
            };
        }

        private long DecodeComboOperand(int operand)
        {
            return operand switch
            {
                >= 0 and <= 3 => operand,
                4 => a,
                5 => b,
                6 => c,
                7 => throw new Exception(),
                _ => throw new ArgumentOutOfRangeException(nameof(operand))
            };
        }
    }
}
