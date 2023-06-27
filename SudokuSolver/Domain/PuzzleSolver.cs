using SudokuSolver.Domain.Models;
using SudokuSolver.Domain.Techniques;
using SudokuSolver.Repository.Entities;

namespace SudokuSolver.Domain
{
    public class PuzzleSolver
    {
        private bool _isBoardValid;

        public Puzzle SolvePuzzle(Puzzle puzzleToSolve)
        {
            _isBoardValid = true;
            SolverSummary summary = new();
            var inProgressBoard = new InProgressBoard(puzzleToSolve);
            List<Technique> techniques = new()
            {
                new SimpleColumnTechnique(),
                new SimpleRowTechnique(),
                new SimpleZoneTechnique(),
                new LastCellColumnTechnique(),
                new LastCellRowTechnique(),
                new LastCellZoneTechnique(),
                new OnlyAvailableColumnTechnique(),
                new OnlyAvailableRowTechnique(),
                new IsolatedMatchesColumnTechnique(),
                new IsolatedMatchesRowTechnique(),
                new IsolatedMatchesZoneTechnique()
            };

            // While each iteration commits changes to the board, keep attempting to solve puzzle
            int changes = 0;
            do
            {
                foreach (var t in techniques)
                {
                    t.ApplyTechnique(inProgressBoard);
                };
                changes = inProgressBoard.CommitChanges();
                _isBoardValid = inProgressBoard.IsValid();
                summary.Iterations++;
                Console.WriteLine(inProgressBoard.GetPuzzleState());
            }
            while (_isBoardValid && changes > 0 && !inProgressBoard.IsSolved());

            summary.EndTime = DateTime.Now;

            if (!_isBoardValid)
            {
                Console.WriteLine(summary.PuzzleInvalid);
            }
            else if (inProgressBoard.Any(c => c.Value.CurrentValue == 0))
            {
                Console.WriteLine(summary.PuzzleUnsolved);
            }
            else
            {
                Console.WriteLine(summary.PuzzleSolved);
            }

            return inProgressBoard.GetPuzzleState();
        }
    }

    internal class SolverSummary
    {
        private string Summary => $"Started at: {StartTime}\nEnded at:   {EndTime}\nDuration:   {Duration}\nIterations: {Iterations}";

        internal DateTime StartTime { get; } = DateTime.Now;

        internal DateTime EndTime { get; set; }

        internal TimeSpan Duration => EndTime.Subtract(StartTime);

        internal int Iterations { get; set; }

        internal string PuzzleSolved => $"Puzzle solved\n\n{Summary}";

        internal string PuzzleUnsolved => $"Puzzle could not be solved\n\n{Summary}";

        internal string PuzzleInvalid => $"Puzzle could not be solved because one or more techniques were invalid\n\n{Summary}";
    }
}
