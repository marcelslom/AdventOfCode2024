
namespace Day6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var loadedSituation = LoadData(input);
            var part1Situation = loadedSituation.Copy();
            var part1 = part1Situation.Part1();
            Console.WriteLine($"Part 1: {part1}");
            var part1Map = part1Situation.Map;
            var counter = 0;
            for (var y = 0; y < part1Map.Length; y++)
            {
                for (var x = 0; x < part1Map[y].Length; x++)
                {
                    if ((x, y) == loadedSituation.StartGuardPosition || part1Map[y][x] != MapTile.Visited)
                    {
                        continue;
                    }
                    var situation = loadedSituation.Copy();
                    situation.Map[y][x] = MapTile.Obstruction;
                    var looped = situation.SimulateCheckIfLooped();
                    if (looped)
                    {
                        counter++;
                    }
                }
            }

            Console.WriteLine($"Part 2: {counter}");
        }

        private static Situation LoadData(string[] input)
        {
            var situation = new Situation
            {
                Map = new MapTile[input.Length][]
            };
            for (var y = 0; y < input.Length; y++)
            {
                situation.Map[y] = new MapTile[input[y].Length];
                for (var x = 0; x < situation.Map[y].Length; x++)
                {
                    switch (input[y][x])
                    {
                        case '^':
                            situation.StartGuardPosition = (x, y);
                            break;
                        case '.':
                            situation.Map[y][x] = MapTile.NotVisited;
                            break;
                        case '#':
                            situation.Map[y][x] = MapTile.Obstruction;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
            return situation;
        }
    }

    class Situation
    {
        private Direction guardDirection = Direction.Up;
        private (int x, int y) guardPosition;
        private (int x, int y) startGuardPosition;

        public (int x, int y) StartGuardPosition
        {
            get => startGuardPosition; set
            {
                startGuardPosition = value;
                GuardPosition = value;
            }
        }

        public (int x, int y) GuardPosition
        {
            get => guardPosition; set
            {
                guardPosition = value;
                Map[guardPosition.y][guardPosition.x] = MapTile.Visited;
            }
        }

        public MapTile[][] Map { get; set; }

        public int Part1()
        {
            while (!ReachedMapEdge())
            {
                Move();
            }
            return CountVisitedTiles();
        }

        public bool SimulateCheckIfLooped()
        {
            var tileLeaveDir = Enumerable.Range(0, Map.Length).Select(i => new Direction?[Map[i].Length]).ToArray();
            while (true)
            {
                if (ReachedMapEdge())
                {
                    return false;
                }
                var dirVec = GetDirectionVector(guardDirection);
                var tileToMoveIntoCached = Map[GuardPosition.y + dirVec.y][GuardPosition.x + dirVec.x];
                if (tileToMoveIntoCached != MapTile.Obstruction)
                {
                    if (tileLeaveDir[GuardPosition.y][GuardPosition.x] == guardDirection)
                    {
                        return true;
                    }
                    tileLeaveDir[GuardPosition.y][GuardPosition.x] = guardDirection;
                    GuardPosition = (GuardPosition.x + dirVec.x, GuardPosition.y + dirVec.y);
                }
                else
                {
                    guardDirection = Rotate(guardDirection);
                }
            }
        }

        private bool ReachedMapEdge()
        {
            var dirVec = GetDirectionVector(guardDirection);
            var newPosition = (x: GuardPosition.x + dirVec.x, y: GuardPosition.y + dirVec.y);
            return newPosition.y < 0 || newPosition.y == Map.Length || newPosition.x < 0 || newPosition.x == Map[newPosition.y].Length;
        }

        private void Move()
        {
            var dirVec = GetDirectionVector(guardDirection);
            var tileToMoveInto = Map[GuardPosition.y + dirVec.y][GuardPosition.x + dirVec.x];
            if (tileToMoveInto != MapTile.Obstruction)
            {
                GuardPosition = (GuardPosition.x + dirVec.x, GuardPosition.y + dirVec.y);
            }
            else
            {
                guardDirection = Rotate(guardDirection);
            }

        }

        public Situation Copy()
        {
            var situation = new Situation { Map = this.Map.Select(x => (MapTile[])x.Clone()).ToArray() };
            situation.guardDirection = guardDirection;
            situation.StartGuardPosition = startGuardPosition;
            return situation;
        }

        private int CountVisitedTiles()
        {
            return Map
                .SelectMany(x => x)
                .Count(x => x == MapTile.Visited);
        }

        public void PrintMap()
        {
            for (int y = 0; y < Map.Length; y++)
            {
                for (int x = 0; x < Map[y].Length; x++)
                {
                    if (x == guardPosition.x && y == guardPosition.y)
                    {
                        // Print guard based on direction
                        char guardChar = guardDirection switch
                        {
                            Direction.Left => '<',
                            Direction.Right => '>',
                            Direction.Up => '^',
                            Direction.Down => '|',
                            _ => ' ' // Fallback, should not happen
                        };
                        Console.Write(guardChar);
                    }
                    else
                    {
                        // Print map tiles
                        char tileChar = Map[y][x] switch
                        {
                            MapTile.NotVisited => '.',
                            MapTile.Visited => 'X',
                            MapTile.Obstruction => '#',
                            _ => ' ' // Fallback, should not happen
                        };
                        Console.Write(tileChar);
                    }
                }
                Console.WriteLine(); // Newline after each row
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private static (int x, int y) GetDirectionVector(Direction direction)
        {
            return direction switch
            {
                Direction.Left => (-1, 0),
                Direction.Right => (1, 0),
                Direction.Up => (0, -1),
                Direction.Down => (0, 1),
                _ => throw new NotImplementedException(),
            };
        }

        private static Direction Rotate(Direction direction)
        {
            return direction switch
            {
                Direction.Left => Direction.Up,
                Direction.Right => Direction.Down,
                Direction.Up => Direction.Right,
                Direction.Down => Direction.Left,
                _ => throw new NotImplementedException(),
            };
        }

    }

    public enum Direction
    {
        Left, Right, Up, Down
    }

    public enum MapTile
    {
        NotVisited, Visited, Obstruction
    }

}
