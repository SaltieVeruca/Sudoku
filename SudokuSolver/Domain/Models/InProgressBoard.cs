using SudokuSolver.Repository.Entities;
using System.Collections.Concurrent;

namespace SudokuSolver.Domain.Models
{
    public class InProgressBoard : ConcurrentDictionary<(int Row, int Column), InProgressCell>
    {
        private Puzzle _puzzleState;

        private readonly BoardZones _boardZones = new();

        public InProgressBoard(Puzzle puzzleToSolve)
        {
            _puzzleState = puzzleToSolve;
            for (var r = 0; r < 9; r++)
            {
                for (var c = 0; c < 9; c++)
                {
                    this[(r,c)] = new InProgressCell()
                    {
                        CurrentValue = _puzzleState.Board[r][c]
                    };
                    if (this[(r, c)].CurrentValue != 0)
                    {
                        this[(r, c)].Possibilities = this[(r, c)].Possibilities.Where(p => p == this[(r, c)].CurrentValue).ToList();
                    }
                }
            }
        }

        public Puzzle GetPuzzleState()
        {
            return _puzzleState;
        }

        public InProgressCellRange GetUnsolvedCells()
        {
            return ToInProgressCellRange(this.Where(c => c.Value.CurrentValue == 0));
        }

        public InProgressCellRange GetRow(int row)
        {
            return ToInProgressCellRange(this.Where(c => c.Key.Row == row));
        }

        public InProgressCellRange GetColumn(int column)
        {
            return ToInProgressCellRange(this.Where(c => c.Key.Column == column));
        }

        public InProgressCellRange GetZone(int zone)
        {
            var matchedZone = _boardZones.GetZone(zone);
            return ToInProgressCellRange(this.Where(c => matchedZone.Rows.Contains(c.Key.Row) && matchedZone.Columns.Contains(c.Key.Column)));
        }

        public InProgressCellRange GetAdjacentZones(int zone)
        {
            var matchedZone = _boardZones.GetZone(zone);
            return ToInProgressCellRange(this.Where(c => matchedZone.Rows.Contains(c.Key.Row) || matchedZone.Columns.Contains(c.Key.Column)));
        }

        public int CommitChanges()
        {
            int currentEmpty = _puzzleState.GetValueCount(0);
            int newEmpty = this.Where(p => p.Value.CurrentValue == 0).Count();

            if (currentEmpty != newEmpty)
            {
                for (var r = 0; r < 9; r++)
                {
                    for (var c = 0; c < 9; c++)
                    {
                        _puzzleState.Board[r][c] = this[(r, c)].CurrentValue;
                    }
                }
            }

            return currentEmpty - newEmpty;
        }

        private static InProgressCellRange ToInProgressCellRange(IEnumerable<KeyValuePair<(int Row, int Column), InProgressCell>> boardRange)
        {
            InProgressCellRange inProgressCellRange = new();

            foreach (var element in boardRange)
            {
                inProgressCellRange.Add(element.Key, element.Value);
            }

            return inProgressCellRange;
        }
    }
}
