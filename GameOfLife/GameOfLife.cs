using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    // We keep our logic in a partial class in order to seperate it from the auto generated UI code in GameOfLife.Designer.cs
    public partial class GameOfLife : Form
    {
        private Boolean running = false;
        private System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();
        private Grid cellGrid;

        public GameOfLife()
        {
            InitializeComponent();
            // System.Windows.Forms.Timer fires the registered method at regular intervals without blocking the UI Thread
            gameTimer.Interval = 100; // ms between ticks
            gameTimer.Tick += (s, e) => Advance(); // method to be thrown at each tick
        }

        private void Load_GameOfLife(object sender, EventArgs e)
        {
            cellGrid = new Grid(1_000, 1_000);
            cellGrid.InitializeGrid((int)numCSize.Value, true);
            UpdateCellGridView();
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            cellGrid.InitializeGrid((int)numCSize.Value, true);
            UpdateCellGridView();
        }
        private void ClearBtn_Click(object sender, EventArgs e)
        {
            cellGrid.InitializeGrid((int)numCSize.Value, false);
            UpdateCellGridView();
        }

        private void AdvanceBtn_Click(object sender, EventArgs e)
        {
            Advance();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            running = !running;

            startBtn.Text = running ? "Stop" : "Start";

            if (running )
            {
                gameTimer.Start();
            }
            else
            {
                gameTimer.Stop();
            }
        }

        private void gameArea_MouseClick(object sender, MouseEventArgs e)
        {
            int cellIndex;

            // Get cell size 
            int cellSize = (int) numCSize.Value;

            // Get the correct square on the grid, the mouse is hovering on
            int Xloc = e.X / cellSize;
            int YLoc = e.Y / cellSize;


            // Get cell list index from grid index
            cellIndex = (YLoc * cellGrid.Cols) + Xloc;

            // Flip state of cell between dead and alive
            cellGrid.Cells[cellIndex].IsAlive = !cellGrid.Cells[cellIndex].IsAlive;

            UpdateCellGridView();
        }

        private void GameOfLife_FormClosing(object sender, FormClosingEventArgs e)
        {
            // End program when closing window
            running = false;
            Application.Exit();
        }

        private void Advance()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            cellGrid.AdvanceOneGeneration();
            watch.Stop();
            Console.WriteLine("Runtime Advance-Step: " + watch.ElapsedMilliseconds.ToString());
            UpdateCellGridView();
        }

        //REFACTOR: Incorrect display of cells (Tests pass, so generations are correctly computed)
        private void UpdateCellGridView()
        {
            // Limit existense of included objects to the scope of this method
            using (Bitmap bmp = new Bitmap(gameArea.Width, gameArea.Height))
            using (Graphics g = Graphics.FromImage(bmp))
            using (SolidBrush cellBrush = new SolidBrush(Color.Green))
            {
                g.Clear(Color.Black);

                int cellSize = (int)numCSize.Value;
                int visibleCols = Math.Min(gameArea.Width / cellSize, cellGrid.Cols);
                int visibleRows = Math.Min(gameArea.Height / cellSize, cellGrid.Rows);
                Console.WriteLine("Visible rows and columns: " + visibleRows + ", " + visibleCols);

                for (int row = 0; row < visibleRows; row++)
                {
                    for (int col = 0; col < visibleCols; col++)
                    {
                        Cell cell = cellGrid.Cells[row * cellGrid.Cols + col];
                        if (cell.IsAlive)
                        {
                            g.FillRectangle(cellBrush, new Rectangle(cell.UILocation,
                                new Size(cellSize - 1, cellSize - 1)));
                        }
                    }
                }

                gameArea.Image?.Dispose(); // keep in mind, that Image is null before first run
                gameArea.Image = (Bitmap)bmp.Clone();
            }
        }

    }
}
