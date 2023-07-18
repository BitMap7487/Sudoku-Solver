using System.Collections.Generic;

class SudokuSolver
{
    /// <summary>
    /// Solves the Sudoku puzzle using backtracking.
    /// </summary>
    /// <param name="sudokuGrid">The Sudoku grid to solve.</param>
    /// <returns>True if the Sudoku puzzle is solved, False otherwise.</returns>
    public bool SolveSudoku(List<List<int>> sudokuGrid)
    {
        int row, col;

        // Find the next empty cell in the Sudoku grid
        if (!FindEmptyCell(sudokuGrid, out row, out col))
            return true; // Sudoku solved

        // Try each number from 1 to 9 in the current empty cell
        for (int num = 1; num <= 9; num++)
        {
            if (sudokuGrid[row][col] == 0 && IsSafe(sudokuGrid, row, col, num))
            {
                // Assign the number to the current cell
                sudokuGrid[row][col] = num;

                // Recursively solve the Sudoku puzzle
                if (SolveSudoku(sudokuGrid))
                    return true;

                // If the solution is not found, backtrack and reset the current cell
                sudokuGrid[row][col] = 0;
            }
        }

        return false;
    }

    /// <summary>
    /// Finds the next empty cell in the Sudoku grid.
    /// </summary>
    /// <param name="sudokuGrid">The Sudoku grid.</param>
    /// <param name="row">The row index of the empty cell.</param>
    /// <param name="col">The column index of the empty cell.</param>
    /// <returns>True if an empty cell is found, False otherwise.</returns>
    private bool FindEmptyCell(List<List<int>> sudokuGrid, out int row, out int col)
    {
        for (row = 0; row < 9; row++)
        {
            for (col = 0; col < 9; col++)
            {
                if (sudokuGrid[row][col] == 0)
                    return true; // Found an empty cell
            }
        }

        row = -1;
        col = -1;
        return false; // No empty cell found
    }

    /// <summary>
    /// Checks if it is safe to assign a number to the given cell in the Sudoku grid.
    /// </summary>
    /// <param name="sudokuGrid">The Sudoku grid.</param>
    /// <param name="row">The row index of the cell.</param>
    /// <param name="col">The column index of the cell.</param>
    /// <param name="num">The number to be assigned.</param>
    /// <returns>True if it is safe to assign the number, False otherwise.</returns>
    private bool IsSafe(List<List<int>> sudokuGrid, int row, int col, int num)
    {
        // Check if the number already exists in the same row
        for (int i = 0; i < 9; i++)
        {
            if (sudokuGrid[row][i] == num)
                return false; // Number already exists in the same row
        }

        // Check if the number already exists in the same column
        for (int i = 0; i < 9; i++)
        {
            if (sudokuGrid[i][col] == num)
                return false; // Number already exists in the same column
        }

        // Check if the number already exists in the same 3x3 box
        int boxStartRow = row - row % 3;
        int boxStartCol = col - col % 3;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (sudokuGrid[boxStartRow + i][boxStartCol + j] == num)
                    return false; // Number already exists in the same 3x3 box
            }
        }

        return true; // Number can be safely assigned to the cell
    }
}
