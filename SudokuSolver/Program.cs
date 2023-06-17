// See https://aka.ms/new-console-template for more information

using SudokuSolver.Repository;

var puzzleRepository = new PuzzlesRepository();

Console.WriteLine("Sudoku");

Console.WriteLine(puzzleRepository.GetPuzzleByIndex(0));

Console.Read();
