
namespace SudokuSolver.Repository.Entities
{
    public class Puzzle
    {
        public int[][] Board { get; private set; }

        public Puzzle(string puzzle)
        {
            if (puzzle.Length != 81)
            {
                throw new ArgumentException("Puzzle must be 81 characters in length");
            }

            Board = new int[9][];

            for (var i = 0 ; i < 9; i++)
            {
                Board[i] = new int[9];
                for (var j = 0; j < 9; j++)
                {
                    Board[i][j] = int.Parse(puzzle[..1]);
                    puzzle = puzzle.Substring(1);
                }
            }
        }

        public override string ToString()
        {
            string prettyBoard = "";

            for (var i = 0; i < 9; i++)
            {
                if (i > 0 && i % 3 == 0)
                {
                    prettyBoard += "\n| - - - + - - - + - - - |";
                }
                prettyBoard += "\n| ";

                for (var j = 0; j < 9; j++)
                {
                    if (j > 0 && j % 3 == 0)
                    {
                        prettyBoard += "| ";
                    }
                    prettyBoard += $"{Board[i][j]} ";
                }

                prettyBoard += "|";
            }

            prettyBoard += "\n\n    1 2 3 4 5 6 7 8 9\n";

            return prettyBoard;
        }
    }
}
