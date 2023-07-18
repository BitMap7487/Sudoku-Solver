using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku_Solver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Loads the Sudoku grid by retrieving data from a web API or using a hardcoded JSON string.
        /// </summary>
        /// <param name="random">Indicates whether to load random Sudoku data</param>
        private void LoadSudokuGrid(bool random)
        {
            string json = string.Empty;

            if (random)
            {
                // Download Sudoku data from a web API
                using (var wc = new System.Net.WebClient())
                    json = wc.DownloadString("https://sudoku-api.vercel.app/api/dosuku");
            }
            else
            {
                // Use a hardcoded JSON string
                json = @"
                {
                    ""newboard"": {
                        ""grids"": [
                            {
                                ""value"": [
                                    [0, 0, 0, 0, 0, 0, 0, 0, 0],
                                    [0, 0, 0, 0, 0, 0, 0, 0, 0],
                                    [0, 0, 0, 0, 0, 0, 0, 0, 0],
                                    [0, 0, 0, 0, 0, 0, 0, 0, 0],
                                    [0, 0, 0, 0, 0, 0, 0, 0, 0],
                                    [0, 0, 0, 0, 0, 0, 0, 0, 0],
                                    [0, 0, 0, 0, 0, 0, 0, 0, 0],
                                    [0, 0, 0, 0, 0, 0, 0, 0, 0],
                                    [0, 0, 0, 0, 0, 0, 0, 0, 0]
                                ]
                            }
                        ],
                        ""results"": 1,
                        ""message"": ""All Ok""
                    }
                }";
            }

            // Deserialize the JSON data into the SudokuData class
            var sudokuData = Newtonsoft.Json.JsonConvert.DeserializeObject<Info.SudokuData>(json);

            // Store the Sudoku grid data
            Info.sudokuGrid = sudokuData.NewBoard.Grids[0].Value;
        }

        /// <summary>
        /// Creates the Sudoku grid UI based on the Sudoku grid data.
        /// </summary>
        private void CreateSudokuGrid()
        {
            // Calculate the width and height of each Sudoku cell
            int cellWidthAndHeight = tlpSudoku.Width / 9;

            // Set the width and height of each column and row in the TableLayoutPanel
            for (int i = 0; i <= 8; i++)
            {
                tlpSudoku.ColumnStyles[i].Width = cellWidthAndHeight;
                tlpSudoku.RowStyles[i].Height = cellWidthAndHeight;
            }

            // Create the Sudoku grid
            for (int row = 0; row < Info.gridSize; row++)
            {
                for (int col = 0; col < Info.gridSize; col++)
                {
                    // Get the value of the current Sudoku cell
                    int cellValue = Info.sudokuGrid[row][col];

                    // Create a TextBox control for the cell
                    TextBox textBox = new TextBox
                    {
                        BorderStyle = BorderStyle.None,
                        Multiline = true,
                        Dock = DockStyle.Fill,
                        Text = (cellValue != 0) ? cellValue.ToString() : "",
                        BackColor = (cellValue != 0) ? Color.DarkGray : Color.White,
                        TextAlign = HorizontalAlignment.Center,
                        Font = new Font("Arial", 35),
                        Width = 40,
                        Height = 40,
                    };

                    // Register the TextChanged event handler for the TextBox
                    textBox.TextChanged += TextBox_TextChanged;

                    // Set the margin of the TextBox based on its position in the Sudoku grid
                    if (col == 2 || col == 5)
                    {
                        textBox.Margin = new Padding(1, 1, 3, 1);
                    }
                    else
                    {
                        textBox.Margin = new Padding(1);
                    }

                    if (row == 2 || row == 5)
                    {
                        textBox.Margin = new Padding(textBox.Margin.Left, textBox.Margin.Top, textBox.Margin.Right, 3);
                    }

                    // Add the TextBox to the TableLayoutPanel at the specified position
                    tlpSudoku.Controls.Add(textBox, col, row);
                }
            }
        }

        /// <summary>
        /// Event handler for the TextChanged event of the TextBoxes in the Sudoku grid.
        /// </summary>
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // Check if the TextBox text is empty or contains invalid input
            if (string.IsNullOrWhiteSpace(textBox.Text))
                return;

            int parsedValue;
            if (!int.TryParse(textBox.Text, out parsedValue) || parsedValue < 1 || parsedValue > 9)
            {
                // Display an error message for invalid input and clear the TextBox text
                MessageBox.Show("Please enter a valid number between 1 and 9.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Text = string.Empty;
            }
        }

        /// <summary>
        /// Event handler for the Form's Load event.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Load the Sudoku grid on form load using hardcoded data
            LoadSudokuGrid(false);
            // Create the Sudoku grid UI
            CreateSudokuGrid();
        }

        /// <summary>
        /// Event handler for the LoadRandomBtn's Click event.
        /// </summary>
        private void LoadRandomBtn_Click(object sender, EventArgs e)
        {
            // Clear the Sudoku grid data
            Info.sudokuGrid.Clear();

            // Remove existing cell controls from the TableLayoutPanel
            for (int row = 0; row < Info.gridSize; row++)
            {
                for (int col = 0; col < Info.gridSize; col++)
                {
                    Control cellControl = tlpSudoku.GetControlFromPosition(col, row);

                    if (cellControl != null)
                    {
                        tlpSudoku.Controls.Remove(cellControl);
                        cellControl.Dispose();
                    }
                }
            }

            // Load random Sudoku data
            LoadSudokuGrid(true);
            // Create the Sudoku grid UI
            CreateSudokuGrid();
        }

        // Constants and external DLL imports for moving the form
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        /// <summary>
        /// Event handler for the panel1's MouseDown event used for moving the form.
        /// </summary>
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            // Allow the form to be moved by capturing mouse input
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        /// <summary>
        /// Event handler for the SolveBtn's Click event.
        /// </summary>
        private void SolveBtn_Click(object sender, EventArgs e)
        {
            // Clear the Sudoku grid data
            Info.sudokuGrid.Clear();

            // Retrieve the values entered in the Sudoku grid
            for (int row = 0; row < tlpSudoku.RowCount; row++)
            {
                List<int> rowValues = new List<int>();

                for (int col = 0; col < tlpSudoku.ColumnCount; col++)
                {
                    Control control = tlpSudoku.GetControlFromPosition(col, row);
                    if (control is TextBox textBox)
                    {
                        int cellValue;
                        if (int.TryParse(textBox.Text, out cellValue))
                        {
                            rowValues.Add(cellValue);
                            control.BackColor = Color.DarkGray;
                        }
                        else
                        {
                            rowValues.Add(0);
                        }
                    }
                }

                // Add the row values to the Sudoku grid data
                Info.sudokuGrid.Add(rowValues);
            }

            // Display the Sudoku grid as a string for debugging
            string sudokuString = string.Join(Environment.NewLine, Info.sudokuGrid.Select(row => string.Join(" ", row)));
            MessageBox.Show(sudokuString);

            // Create a SudokuSolver object
            SudokuSolver solver = new SudokuSolver();
            // Solve the Sudoku puzzle
            if (solver.SolveSudoku(Info.sudokuGrid))
            {
                MessageBox.Show("Sudoku solved:");

                // Update the TextBoxes in the Sudoku grid with the solved values
                for (int row = 0; row < Info.gridSize; row++)
                {
                    for (int col = 0; col < Info.gridSize; col++)
                    {
                        Control control = tlpSudoku.GetControlFromPosition(col, row);
                        if (control is TextBox textBox)
                        {
                            // Modify the text of the TextBox control
                            textBox.Text = Info.sudokuGrid[row][col].ToString();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No solution found for the Sudoku puzzle.");
            }
        }

        /// <summary>
        /// Event handler for the ClearBtn's Click event.
        /// </summary>
        private void ClearBtn_Click(object sender, EventArgs e)
        {
            // Clear the Sudoku grid data
            Info.sudokuGrid.Clear();

            // Remove existing cell controls from the TableLayoutPanel
            for (int row = 0; row < Info.gridSize; row++)
            {
                for (int col = 0; col < Info.gridSize; col++)
                {
                    Control cellControl = tlpSudoku.GetControlFromPosition(col, row);

                    if (cellControl != null)
                    {
                        tlpSudoku.Controls.Remove(cellControl);
                        cellControl.Dispose();
                    }
                }
            }

            // Load the Sudoku grid with default data
            LoadSudokuGrid(false);
            // Create the Sudoku grid UI
            CreateSudokuGrid();
        }
    }
}
