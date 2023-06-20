
namespace SudokuSolver.Repository.Entities
{
    public struct Puzzle
    {
        public int[][] Board { get; set; }

        public Puzzle(string puzzle)
        {
            if (puzzle.Length != 81)
            {
                throw new ArgumentException("Puzzle must be 81 characters in length");
            }

            Board = new int[9][];

            for (var r = 0 ; r < 9; r++)
            {
                Board[r] = new int[9];
                for (var c = 0; c < 9; c++)
                {
                    Board[r][c] = int.Parse(puzzle[..1]);
                    puzzle = puzzle[1..];
                }
            }
        }

        public readonly int GetValueCount(int value)
        {
            int[] range = Enumerable.Range(0, 10).ToArray();
            var count = 0;

            if (!range.Contains(value)) {
                throw new ArgumentOutOfRangeException();
            }

            for (var r = 0; r < 9; r++)
            {
                for (var c = 0; c < 9; c++)
                {
                    if (Board[r][c] == value)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public override readonly string ToString()
        {
            string prettyPuzzle = "";

            for (var r = 0; r < 9; r++)
            {
                if (r > 0 && r % 3 == 0)
                {
                    prettyPuzzle += "\n| - - - + - - - + - - - |";
                }
                prettyPuzzle += "\n| ";

                for (var c = 0; c < 9; c++)
                {
                    if (c > 0 && c % 3 == 0)
                    {
                        prettyPuzzle += "| ";
                    }
                    prettyPuzzle += Board[r][c] == 0 ? "  " : $"{Board[r][c]} ";
                }

                prettyPuzzle += "|";
            }

            prettyPuzzle += "\n\n    1 2 3 4 5 6 7 8 9\n";

            return prettyPuzzle;
        }
    }
}
