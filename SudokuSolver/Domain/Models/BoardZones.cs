using System.Collections.ObjectModel;

namespace SudokuSolver.Domain.Models
{
    public class BoardZones : ReadOnlyCollection<BoardZone>
    {
        public BoardZones() : base(new List<BoardZone>
            {
                new BoardZone(new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }),
                new BoardZone(new int[] { 0, 1, 2 }, new int[] { 3, 4, 5 }),
                new BoardZone(new int[] { 0, 1, 2 }, new int[] { 6, 7, 8 }),
                new BoardZone(new int[] { 3, 4, 5 }, new int[] { 0, 1, 2 }),
                new BoardZone(new int[] { 3, 4, 5 }, new int[] { 3, 4, 5 }),
                new BoardZone(new int[] { 3, 4, 5 }, new int[] { 6, 7, 8 }),
                new BoardZone(new int[] { 6, 7, 8 }, new int[] { 0, 1, 2 }),
                new BoardZone(new int[] { 6, 7, 8 }, new int[] { 3, 4, 5 }),
                new BoardZone(new int[] { 6, 7, 8 }, new int[] { 6, 7, 8 })
            })
        { }

        public BoardZone GetZone(int zone) => zone switch
        {
            0 => this[0],
            1 => this[1],
            2 => this[2],
            3 => this[3],
            4 => this[4],
            5 => this[5],
            6 => this[6],
            7 => this[7],
            8 => this[8],
            _ => throw new ArgumentOutOfRangeException(),
        };

        public BoardZone GetZoneByCoordinate(int row, int column)
        {
            return this.First(c => c.Rows.Contains(row) && c.Columns.Contains(column));
        }

        public int GetZoneIdByCoordinate(int row, int column)
        {
            return IndexOf(GetZoneByCoordinate(row, column));
        }
    }

    public class BoardZone
    {
        public int[] Rows { get; }
        public int[] Columns { get; }

        public BoardZone(int[] rows, int[] columns)
        {
            Rows = rows; Columns = columns;
        }
    }
}