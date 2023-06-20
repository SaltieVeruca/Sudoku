namespace SudokuSolver.Domain.Models
{
    public class InProgressCell
    {
        public int CurrentValue { get; set; }
        public List<int> Possibilities { get; set; } = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    }
}