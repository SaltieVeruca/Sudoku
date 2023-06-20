using SudokuSolver.Domain.Models;
using SudokuSolver.Domain.Techniques;
using SudokuSolver.Repository.Entities;

namespace SudokuSolver.Domain
{
    public class PuzzleSolver
    {
        public Puzzle SolvePuzzle(Puzzle puzzleToSolve)
        {
            SolverSummary summary = new();

            var inProgressBoard = new InProgressBoard(puzzleToSolve);

            List<Technique> techniques = new()
            {
                new SimpleColumnTechnique(),
                new SimpleRowTechnique(),
                new SimpleZoneTechnique()

            };

            // While each iteration commits changes to the board, keep attempting to solve puzzle
            do
            {
                Parallel.ForEach(techniques, (t) =>
                {
                    t.ApplyTechnique(inProgressBoard);
                });
                summary.Iterations++;
            }
            while (inProgressBoard.CommitChanges() != 0);

            summary.EndTime = DateTime.Now;

            if (inProgressBoard.Any(c => c.Value.CurrentValue == 0))
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
        private string _summary => $"Started at: {StartTime}\nEnded at: {EndTime}\nDuration: {Duration}\nTechnique iterations: {Iterations}";
        internal DateTime StartTime { get; } = DateTime.Now;

        internal DateTime EndTime { get; set; }

        internal TimeSpan Duration => EndTime.Subtract(StartTime);

        internal int Iterations { get; set; }

        internal string PuzzleSolved => $"Puzzle solved\n\n{_summary}";

        internal string PuzzleUnsolved => $"Puzzle could not be solved\n\n{_summary}";
    }
}
