using SudokuSolver.Domain.Models;

namespace SudokuSolver.Domain.Techniques
{
    internal class OnlyAvailableColumnTechnique : Technique
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

                    // If all cell columns are the same
                    if (unsolvedCells.Any() && !unsolvedCells.Any(kvp => kvp.Key.Column != unsolvedCells[0].Key.Column))
                    {
                        var zoneRows = unsolvedCells.Select(kvp => kvp.Key.Row).ToList();
                        var column = board.GetColumn(unsolvedCells[0].Key.Column).Where(kvp => !zoneRows.Contains(kvp.Key.Row)).ToList();

                        foreach (var columnCell in column)
                        {
                            if (columnCell.Value.Possibilities.Contains(possibility))
                            {
                                columnCell.Value.Possibilities.Remove(possibility);
                            }
                        }
                    }
                }
            }
        }
    }
}
