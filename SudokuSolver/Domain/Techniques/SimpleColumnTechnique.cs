using SudokuSolver.Domain.Models;

namespace SudokuSolver.Domain.Techniques
{
    internal class SimpleColumnTechnique : Technique
    {
        internal override void DeducePossibilities(InProgressBoard board)
        {
            foreach (var unsolvedCell in board.GetUnsolvedCells())
            {
                foreach (var solvedCell in board.GetColumn(unsolvedCell.Key.Column).Where(c => c.Value.CurrentValue is not 0))
                {
                    unsolvedCell.Value.Possibilities.Remove(solvedCell.Value.CurrentValue);
                }
            }
        }
    }
}
