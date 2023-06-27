using SudokuSolver.Domain.Models;

namespace SudokuSolver.Domain.Techniques
{
    internal class OnlyAvailableRowTechnique : Technique
    {
        internal override void DeducePossibilities(InProgressBoard board)
        {
            var possibilities = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            foreach (var zone in board.GetZones())
            {
                foreach (var possibility in possibilities)
                {
                    if (zone.Any(kvp => kvp.Value.CurrentValue == possibility))
                    {
                        continue;
                    }

                    var unsolvedCells = zone.GetUnsolvedCells()
                                            .Where(kvp => kvp.Value.Possibilities.Contains(possibility))
                                            .ToList();

                    // If all cell rows are the same
                    if (unsolvedCells.Any() && !unsolvedCells.Any(kvp => kvp.Key.Row != unsolvedCells[0].Key.Row))
                    {
                        var zoneColumns = unsolvedCells.Select(kvp => kvp.Key.Column).ToList();
                        var row = board.GetRow(unsolvedCells[0].Key.Row).Where(kvp => !zoneColumns.Contains(kvp.Key.Column)).ToList();

                        foreach (var rowCell in row)
                        {
                            if (rowCell.Value.Possibilities.Contains(possibility))
                            {
                                rowCell.Value.Possibilities.Remove(possibility);
                            }
                        }
                    }
                }
            }
        }
    }
}
