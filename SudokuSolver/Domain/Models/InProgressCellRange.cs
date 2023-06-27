namespace SudokuSolver.Domain.Models
{
    public class InProgressCellRange : Dictionary<(int Row, int Column), InProgressCell>
    {
        public InProgressCellRange GetUnsolvedCells()
        {
            return ToInProgressCellRange(this.Where(ipc => ipc.Value.CurrentValue == 0));
        }

        private static InProgressCellRange ToInProgressCellRange(IEnumerable<KeyValuePair<(int Row, int Column), InProgressCell>> boardRange)
        {
            InProgressCellRange inProgressCellRange = new();

            foreach (var keyValuePair in boardRange)
            {
                inProgressCellRange.Add(keyValuePair.Key, keyValuePair.Value);
            }

            return inProgressCellRange;
        }
    }
}
