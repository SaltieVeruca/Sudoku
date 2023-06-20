using SudokuSolver.Domain.Models;

namespace SudokuSolver.Domain.Techniques
{
    public class SimpleRowTechnique : Technique
    {
        internal override void DeducePossibilities(InProgressBoard board)
        {
            foreach (var unsolvedCell in board.GetUnsolvedCells())
            {
                foreach (var solvedCell in board.GetRow(unsolvedCell.Key.Row).Where(c => c.Value.CurrentValue is not 0))
                {
                    if (unsolvedCell.Value.Possibilities.Contains(solvedCell.Value.CurrentValue))
                    {
                        unsolvedCell.Value.Possibilities.Remove(solvedCell.Value.CurrentValue);
                    }
                }
            }
        }
    }
}
