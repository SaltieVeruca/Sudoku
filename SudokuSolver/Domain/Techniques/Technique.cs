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

        private void SetSolvedCells(InProgressBoard board)
        {
            foreach (var cell in board.GetUnsolvedCells())
            {
                _ = cell.Value.IsCurrentValueImplicit();
            }
        }
    }
}
