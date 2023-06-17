using Newtonsoft.Json;
using SudokuSolver.Repository.Entities;

namespace SudokuSolver.Repository
{
    public class PuzzlesRepository
    {
        private string _filePath = $"{AppContext.BaseDirectory}Repository\\Data\\Puzzles.json";
        private Puzzle[] _puzzles;

        public PuzzlesRepository()
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"{_filePath} does not exist");
            }
            using StreamReader r = new(_filePath);
            string json = r.ReadToEnd();
            var stringPuzzles = JsonConvert.DeserializeObject<string[]>(json) ?? throw new ArgumentNullException($"{_filePath} does not contain any puzzles");
            _puzzles = stringPuzzles.Select(p => new Puzzle(p)).ToArray();
        }

        public Puzzle GetPuzzleByIndex(int index)
        {
            return _puzzles[index];
        }
    }
}
