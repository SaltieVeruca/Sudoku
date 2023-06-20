using SudokuSolver.Domain.Models;

namespace SudokuSolver.Domain.Techniques
{
    internal class SimpleZoneTechnique : Technique
    {
        internal override void DeducePossibilities(InProgressBoard board)
        {
            var zones = new BoardZones();

            foreach (var unsolvedCell in board.GetUnsolvedCells())
            {
                foreach (var solvedCell in board.GetZone(zones.GetZoneIdByCoordinate(unsolvedCell.Key.Row, unsolvedCell.Key.Column)).Where(c => c.Value.CurrentValue is not 0))
                {
                    unsolvedCell.Value.Possibilities.Remove(solvedCell.Value.CurrentValue);
                }
            }
        }
    }
}
