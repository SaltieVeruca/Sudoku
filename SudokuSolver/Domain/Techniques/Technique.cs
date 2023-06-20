using SudokuSolver.Domain.Models;

namespace SudokuSolver.Domain.Techniques
{
    public abstract class Technique
    {
        public void ApplyTechnique(InProgressBoard board)
        {
            DeducePossibilities(board);
            SetSolvedCells(board);
        }

        internal abstract void DeducePossibilities(InProgressBoard board);

        private static void SetSolvedCells(InProgressBoard board)
        {
            foreach (var cell in board)
            {
                if (cell.Value.Possibilities.Count == 1)
                {
                    cell.Value.CurrentValue = cell.Value.Possibilities.First();
                }
            }
        }
    }
}
