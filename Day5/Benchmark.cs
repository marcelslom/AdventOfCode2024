using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Day5
{
    public class Benchmark
    {
        private string[] input;

        [GlobalSetup]
        public void Setup()
        {
            input = File.ReadAllLines("input.txt");
        }

        [Benchmark]
        public int Straightforward()
        {
            var rules = input
                .TakeWhile(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Split('|'))
                .Select(x => (x[0], x[1]))
                .ToArray();

            var result = input
                .SkipWhile(x => !string.IsNullOrWhiteSpace(x))
                .Skip(1)
                .Select(x => x.Split(','))
                .Where(x => IsUpdateValid(rules, x))
                .Select(x => x[x.Length / 2])
                .Select(int.Parse)
                .Sum();

            return result;
        }

        [Benchmark]
        public int Optimized()
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

            return result;
        }

        public static bool IsUpdateValid(FrozenDictionary<string, string[]> rules, string[] update)
        {
            for (var i = 0; i < update.Length; i++)
            {
                var item = update[i];
                if (!rules.TryGetValue(item, out var rule))
                {
                    continue;
                }
                foreach (var x in rule)
                {
                    var index = Array.IndexOf(update, x);
                    if (index == -1)
                    {
                        continue;
                    }
                    if (index > i)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsUpdateValid((string, string)[] rules, string[] update)
        {
            foreach (var rule in rules)
            {
                var indexOfLeft = Array.IndexOf(update, rule.Item1);
                var indexOfRight = Array.IndexOf(update, rule.Item2);
                if (indexOfLeft == -1 || indexOfRight == -1)
                {
                    continue;
                }
                if (indexOfLeft > indexOfRight)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
