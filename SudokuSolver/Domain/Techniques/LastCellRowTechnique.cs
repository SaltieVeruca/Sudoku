using SudokuSolver.Domain.Models;

namespace SudokuSolver.Domain.Techniques
{
    internal class LastCellRowTechnique : Technique
    {
        internal override void DeducePossibilities(InProgressBoard board)
        {
            var zones = new BoardZones();
            var possibilities = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            foreach (var row in board.GetRows())
            {
                foreach (var possibility in possibilities)
                {
                    var unsolvedCells = row.GetUnsolvedCells()
                                           .Where(kvp => kvp.Value.Possibilities.Contains(possibility))
                                           .ToList();

                    if (unsolvedCells.Count is 1)
                    {
                        var unsolvedCell = unsolvedCells.First();
                        var zone = board.GetZone(zones.GetZoneIdByCoordinate(unsolvedCell.Key.Row, unsolvedCell.Key.Column)).Where(kvp => kvp.Key != unsolvedCell.Key).ToList();
                        var column = board.GetColumn(unsolvedCell.Key.Column).Where(kvp => kvp.Key != unsolvedCell.Key).ToList();

                        if (!row.Any(kvp => kvp.Value.CurrentValue == possibility) &&
                            !column.Any(kvp => kvp.Value.CurrentValue == possibility) &&
                            !zone.Any(kvp => kvp.Value.CurrentValue == possibility))
                        {
                            foreach (var p in possibilities)
                            {
                                if (p == possibility) { continue; }

                                unsolvedCell.Value.Possibilities.Remove(p);
                            }
                        }
                    }
                }
            }
        }
    }
}
