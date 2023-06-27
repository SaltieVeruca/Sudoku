namespace SudokuSolver.Domain.Models
{
    public class InProgressCell
    {
        public int CurrentValue { get; set; }

        public List<int> Possibilities { get; set; } = new();

        public bool IsCurrentValueImplicit()
        {
            if (Possibilities.Count == 1)
            {
                CurrentValue = Possibilities[0];
                return true;
            }
            return false;
        }
    }
}