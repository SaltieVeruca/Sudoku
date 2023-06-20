namespace SudokuSolver.Domain.Models
{
    public class InProgressCellRange : Dictionary<(int Row, int Column), InProgressCell> { }
}
