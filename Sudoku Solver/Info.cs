using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{
    /// <summary>
    /// Contains information and data related to the Sudoku puzzle.
    /// </summary>
    internal class Info
    {
        /// <summary>
        /// The size of the Sudoku grid.
        /// </summary>
        public static int gridSize = 9;

        /// <summary>
        /// The size of the subgrid within the Sudoku grid.
        /// </summary>
        public static int subGridSize = 3;

        /// <summary>
        /// The representation of the Sudoku grid.
        /// </summary>
        public static List<List<int>> sudokuGrid;

        // Classes for deserializing JSON

        /// <summary>
        /// Represents the deserialized Sudoku data.
        /// </summary>
        public class SudokuData
        {
            /// <summary>
            /// The new Sudoku board.
            /// </summary>
            public NewBoard NewBoard { get; set; }
        }

        /// <summary>
        /// Represents the new Sudoku board.
        /// </summary>
        public class NewBoard
        {
            /// <summary>
            /// The grids of the Sudoku board.
            /// </summary>
            public List<Grid> Grids { get; set; }

            /// <summary>
            /// The number of results.
            /// </summary>
            public int Results { get; set; }

            /// <summary>
            /// The message associated with the Sudoku board.
            /// </summary>
            public string Message { get; set; }
        }

        /// <summary>
        /// Represents a grid in the Sudoku board.
        /// </summary>
        public class Grid
        {
            /// <summary>
            /// The value of the grid.
            /// </summary>
            public List<List<int>> Value { get; set; }
        }
    }
}
