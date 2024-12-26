
namespace Day9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt");
            var part1 = Part1(input);
            Console.WriteLine($"Part 1: {part1}");
            var part2 = Part2(input);
            Console.WriteLine($"Part 2: {part2}");
        }

        private static long Part2(string input)
        {
            var memoryMap = input
                .Select(x => int.Parse(x.ToString()))
                .Select((x, i) => new { x, i })
                .Select(tuple => (size: tuple.x, id: tuple.i % 2 == 0 ? tuple.i / 2 : -1, wasMoved: false))
                .ToList();

            for (var i = memoryMap.Count - 1; i >= 0; i--)
            {
                var fileToCheck = memoryMap[i];
                if (fileToCheck.id == -1 || fileToCheck.wasMoved)
                {
                    continue;
                }
                memoryMap[i] = (size: fileToCheck.size, id: fileToCheck.id, true);
                for (var j = 0; j < i; j++)
                {
                    var file = memoryMap[j];
                    if (file.id != -1 || file.size < fileToCheck.size)
                    {
                        continue;
                    }
                    memoryMap[j] = fileToCheck;
                    memoryMap[i] = (size: fileToCheck.size, id: -1, wasMoved: true);
                    if (file.size > fileToCheck.size)
                    {
                        memoryMap.Insert(j + 1, (size: file.size - fileToCheck.size, id: -1, wasMoved: false));
                    }
                    break;
                }
            }

            var checksum = 0L;
            var offset = 0;
            foreach (var file in memoryMap)
            {
                var newOffset = offset + file.size;
                if (file.id == -1)
                {
                    offset += file.size;
                    continue;
                }
                long sumOfIndexes = (newOffset - 1 - offset + 1) * (offset + newOffset - 1) / 2;
                offset = newOffset;
                checksum += sumOfIndexes * file.id;
            }

            return checksum;
        }

        private static long Part1(string input)
        {
            var memoryMap = input
                .Select(x => int.Parse(x.ToString()))
                .Select((x, i) => new { x, i })
                .Select(tuple => (size: tuple.x, id: tuple.i % 2 == 0 ? tuple.i / 2 : -1))
                .ToList();

            for (var i = 0; i < memoryMap.Count; i++)
            {
                var processedFile = memoryMap[i];
                if (processedFile.id == -1)
                {
                    var fileToPutInto = memoryMap[^1];
                    if (processedFile.size >= fileToPutInto.size)
                    {
                        memoryMap[i] = fileToPutInto;
                        if (processedFile.size > fileToPutInto.size)
                        {
                            memoryMap.Insert(i + 1, (size: processedFile.size - fileToPutInto.size, id: -1));
                        }
                        memoryMap.RemoveAt(memoryMap.Count - 1);
                        memoryMap.RemoveAt(memoryMap.Count - 1);
                    }
                    else
                    {
                        memoryMap[i] = (size: processedFile.size, id: fileToPutInto.id);
                        memoryMap[^1] = (size: fileToPutInto.size - processedFile.size, id: fileToPutInto.id);
                    }
                }
            }

            var checksum = 0L;
            var offset = 0;
            foreach (var file in memoryMap)
            {
                var newOffset = offset + file.size;
                long sumOfIndexes = (newOffset - 1 - offset + 1) * (offset + newOffset - 1) / 2;
                offset = newOffset;
                checksum += sumOfIndexes * file.id;
            }

            return checksum;
        }
    }
}
