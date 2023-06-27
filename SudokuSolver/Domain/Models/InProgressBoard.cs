using SudokuSolver.Repository.Entities;
using System.Collections.Concurrent;

namespace SudokuSolver.Domain.Models
{
    public class InProgressBoard : ConcurrentDictionary<(int Row, int Column), InProgressCell>
    {
        private Puzzle _puzzleState;
        private int _currentPossibilities = 0;

        private readonly BoardZones _boardZones = new();

        public InProgressBoard(Puzzle puzzleToSolve)
        {
            _puzzleState = puzzleToSolve;
            for (var r = 0; r < 9; r++)
            {
                for (var c = 0; c < 9; c++)
                {
                    this[(r, c)] = new InProgressCell()
                    {
                        CurrentValue = _puzzleState.Board[r][c]
                    };
                    if (this[(r, c)].CurrentValue == 0)
                    {
                        this[(r, c)].Possibilities = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    }
                    _currentPossibilities += this[(r, c)].Possibilities.Count;
                }
            }
        }

        public Puzzle GetPuzzleState()
        {
            return _puzzleState;
        }

        public InProgressCellRange GetUnsolvedCells()
        {
            return ToInProgressCellRange(this.Where(ipc => ipc.Value.CurrentValue == 0));
        }

        public InProgressCellRange GetRow(int row)
        {
            return ToInProgressCellRange(this.Where(ipc => ipc.Key.Row == row));
        }

        public List<InProgressCellRange> GetRows()
        {
            var rows = new List<InProgressCellRange>();
            for (var r = 0; r < 9; r++)
            {
                rows.Add(ToInProgressCellRange(this.Where(ipc => ipc.Key.Row == r)));
            }
            return rows;
        }

        public InProgressCellRange GetColumn(int column)
        {
            return ToInProgressCellRange(this.Where(ipc => ipc.Key.Column == column));
        }

        public List<InProgressCellRange> GetColumns()
        {
            var columns = new List<InProgressCellRange>();
            for (var c = 0; c < 9; c++)
            {
                columns.Add(ToInProgressCellRange(this.Where(ipc => ipc.Key.Column == c)));
            }
            return columns;
        }

        public InProgressCellRange GetZone(int zone)
        {
            var matchedZone = _boardZones.GetZone(zone);
            return ToInProgressCellRange(this.Where(ipc => matchedZone.Rows.Contains(ipc.Key.Row) && matchedZone.Columns.Contains(ipc.Key.Column)));
        }

        public List<InProgressCellRange> GetZones()
        {
            var zones = new List<InProgressCellRange>();
            for (var z = 0; z < 9; z++)
            {
                var matchedZone = _boardZones.GetZone(z);
                zones.Add(ToInProgressCellRange(this.Where(ipc => matchedZone.Rows.Contains(ipc.Key.Row) && matchedZone.Columns.Contains(ipc.Key.Column))));
            }
            return zones;
        }

        public InProgressCellRange GetAdjacentZones(int zone)
        {
            var matchedZone = _boardZones.GetZone(zone);
            return ToInProgressCellRange(this.Where(ipc => matchedZone.Rows.Contains(ipc.Key.Row) || matchedZone.Columns.Contains(ipc.Key.Column)));
        }

        public int CommitChanges()
        {
            int currentEmpty = _puzzleState.GetValueCount(0);
            int newEmpty = this.Where(ipc => ipc.Value.CurrentValue == 0).Count();
            int newPossibilities = 0;

            for (var r = 0; r < 9; r++)
            {
                for (var c = 0; c < 9; c++)
                {
                    _puzzleState.Board[r][c] = this[(r, c)].CurrentValue;
                    newPossibilities += this[(r, c)].Possibilities.Count;
                }
            }
            int changeCount = (currentEmpty - newEmpty) + (_currentPossibilities - newPossibilities);
            _currentPossibilities = newPossibilities;

            return changeCount;
        }

        public bool IsSolved()
        {
            return GetUnsolvedCells().Count == 0;
        }

        public bool IsValid()
        {
            var isValid = true;
            foreach (var row in GetRows())
            {
                var cells = row.Where(ipc => ipc.Value.CurrentValue != 0);
                HashSet<int> distinctPossibilities = new();
                foreach (var cell in cells)
                {
                    if (!distinctPossibilities.Add(cell.Value.CurrentValue))
                    {
                        Console.Error.WriteLine($"Row {cell.Key.Row} is invalid.");
                        isValid = false;
                        break;
                    }
                }
                if (!isValid) { break; }
            }

            foreach (var column in GetColumns())
            {
                var cells = column.Where(ipc => ipc.Value.CurrentValue != 0);
                HashSet<int> distinctPossibilities = new();
                foreach (var cell in cells)
                {
                    if (!distinctPossibilities.Add(cell.Value.CurrentValue))
                    {
                        Console.Error.WriteLine($"Column {cell.Key.Column} is invalid.");
                        isValid = false;
                        break;
                    }
                }
                if (!isValid) { break; }
            }

            foreach (var zone in GetZones())
            {
                var cells = zone.Where(ipc => ipc.Value.CurrentValue != 0);
                HashSet<int> distinctPossibilities = new();
                foreach (var cell in cells)
                {
                    if (!distinctPossibilities.Add(cell.Value.CurrentValue))
                    {
                        Console.Error.WriteLine($"Zone {_boardZones.GetZoneIdByCoordinate(cell.Key.Row, cell.Key.Column)} is invalid.");
                        isValid = false;
                        break;
                    }
                }
                if (!isValid) { break; }
            }

            return isValid;
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
