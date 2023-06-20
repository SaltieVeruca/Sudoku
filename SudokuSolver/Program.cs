// See https://aka.ms/new-console-template for more information

using SudokuSolver.Repository;
using SudokuSolver.Domain;

var puzzleRepository = new PuzzlesRepository();

Console.WriteLine("Sudoku");

var puzzle = puzzleRepository.GetPuzzleByIndex(0);

Console.WriteLine(puzzle);

PuzzleSolver solver = new();

Console.WriteLine(solver.SolvePuzzle(puzzle));

// Force the terminal to stay open for now
Console.Read();
