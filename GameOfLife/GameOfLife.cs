using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections.Concurrent;  // Partitioner
using System.Threading.Tasks;          // Parallel
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
            var watch = System.Diagnostics.Stopwatch.StartNew();
            InitializeCellGrid(true);
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds.ToString());
            UpdateCellGridView();
        }

        private void InitializeCellGrid(bool randomCells)
        {
            int rows = 10_000;
            int cols = 10_000;

            cellGrid = new Grid(rows, cols);
            cellGrid.Cells.Clear();
            // Allocate the row's cells upfront for index access
            Cell[] rowCells  = new Cell[cols];

            // Create and add cells to the Grid object in row-major order
            for(int y = 0; y < rows; y++)
            {
                Parallel.For(0, cols, x =>
                {
                    Cell newCell = new Cell(
                        new Point((int)(x * numCSize.Value), (int)(y * numCSize.Value)),
                        x,
                        y);

                    newCell.IsAlive = randomCells && (Random.Shared.Next(100) < 15);
                    rowCells[x] = newCell;
                });

                // Single-threaded add after parallel work is done
                foreach (Cell cell in rowCells)
                {
                    cellGrid.Cells.Add(cell);
                }
                rowCells = new Cell[cols];
            }
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            InitializeCellGrid(true);
        }
        private void clearBtn_Click(object sender, EventArgs e)
        {
            InitializeCellGrid(false);
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
            Console.WriteLine(watch.ElapsedMilliseconds.ToString());
            UpdateCellGridView();
        }

        //REFACTOR: Încorrect display of cells (Tests pass, so generations are correctly computed)
        private void UpdateCellGridView()
        {
            // Limit existense of included objects to the scope of this method
            using (Bitmap bmp = new Bitmap(gameArea.Width, gameArea.Height))
            using (Graphics g = Graphics.FromImage(bmp))
            using (SolidBrush cellBrush = new SolidBrush(Color.Green))
            {
                g.Clear(Color.Black);

                // Starts in top-right corner
                for (int y = 0;  y < gameArea.Height; y+=(int)numCSize.Value) // REFACTOR: casting to int probably unsafe here
                {
                    for (int x = 0; x < gameArea.Width; x+=(int)numCSize.Value)
                    {
                        Cell cell = cellGrid.Cells[y * cellGrid.Rows + x];
                        if (cell.IsAlive)
                        {
                            // PARALLELIZABLE (maybe not worth it due to too much overhead)
                            g.FillRectangle(cellBrush, new Rectangle(cell.UILocation,
                                new Size((int)numCSize.Value - 1, (int)numCSize.Value - 1)));
                        }
                    }
                }

                gameArea.Image?.Dispose(); // keep in mind, that Image is null before first run
                gameArea.Image = (Bitmap)bmp.Clone();
            }
        }

    }
}
