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
        private Point panOffset = Point.Empty;
        private Point panStart = Point.Empty;
        private bool isPanning = false;

        public GameOfLife()
        {
            InitializeComponent();
            // System.Windows.Forms.Timer fires the registered method at regular intervals without blocking the UI Thread
            this.gameTimer.Interval = 100; // ms between ticks
            this.gameTimer.Tick += (s, e) => Advance(); // method to be thrown at each tick
        }

        private void Load_GameOfLife(object sender, EventArgs e)
        {
            // ADJUST: Lower the grid size, if your system does not support the load of handling the current grid size
            this.cellGrid = new Grid(1500, 1500);
            this.cellGrid.InitializeGrid((int)this.numCSize.Value, true);
            UpdateCellGridView();
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            this.cellGrid.InitializeGrid((int)this.numCSize.Value, true);
            UpdateCellGridView();
        }
        private void ClearBtn_Click(object sender, EventArgs e)
        {
            this.cellGrid.InitializeGrid((int)this.numCSize.Value, false);
            UpdateCellGridView();
        }

        private void AdvanceBtn_Click(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            this.cellGrid.AdvanceOneGeneration();
            watch.Stop();
            Console.WriteLine("Runtime Advance-Step: " + watch.ElapsedMilliseconds.ToString() + "ms");
            UpdateCellGridView();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            this.running = !this.running;

            this.startBtn.Text = running ? "Stop" : "Start";

            if (running )
            {
                this.gameTimer.Start();
            }
            else
            {
                this.gameTimer.Stop();
            }
        }

        private void gameArea_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int cellSize = (int)this.numCSize.Value;

                int startCol = Math.Clamp(-this.panOffset.X / cellSize, 0, this.cellGrid.Cols - 1);
                int startRow = Math.Clamp(-this.panOffset.Y / cellSize, 0, this.cellGrid.Rows - 1);

                // Get the correct square on the grid, the mouse is hovering on
                int col = startCol + e.X / cellSize;
                int row = startRow + e.Y / cellSize;

                if (col < this.cellGrid.Cols || row < this.cellGrid.Rows) // Bounds check
                {
                    int cellIndex = row * this.cellGrid.Cols + col;
                    // Switch the state of the clicked cell
                    this.cellGrid.Cells[cellIndex].IsAlive = !this.cellGrid.Cells[cellIndex].IsAlive;

                    UpdateCellGridView();
                }
            }
        }

        private void gameArea_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                this.running = false;
                this.isPanning = true;
                this.panStart = e.Location;
                this.gameArea.Cursor = Cursors.SizeAll; // visual feedback
            }
        }

        private void gameArea_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                this.running = true;
                this.isPanning = false;
                this.gameArea.Cursor = Cursors.Default;
            }
        }

        private void gameArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isPanning)
            {
                this.panOffset.X += e.X - this.panStart.X;
                this.panOffset.Y += e.Y - this.panStart.Y;
                this.panStart = e.Location;
                UpdateCellGridView();
            }
        }

        private void GameOfLife_FormClosing(object sender, FormClosingEventArgs e)
        {
            // End program when closing window
            this.running = false;
            Application.Exit();
        }

        private void Advance()
        {
            if (this.running)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                this.cellGrid.AdvanceOneGeneration();
                watch.Stop();
                Console.WriteLine("Runtime Advance-Step: " + watch.ElapsedMilliseconds.ToString() + "ms");
                UpdateCellGridView();
            }
        }

        private void UpdateCellGridView()
        {
            // Limit existense of included objects to the scope of this method
            using (Bitmap bmp = new Bitmap(this.gameArea.Width, this.gameArea.Height))
            using (Graphics g = Graphics.FromImage(bmp))
            using (SolidBrush cellBrush = new SolidBrush(Color.Green))
            {
                g.Clear(Color.Black);

                int cellSize = (int)this.numCSize.Value;

                int startCol = Math.Clamp(-this.panOffset.X / cellSize, 0, this.cellGrid.Cols - 1);
                int startRow = Math.Clamp(-this.panOffset.Y / cellSize, 0, this.cellGrid.Rows - 1);

                // Limits visible range when panning out of bounds
                int visibleCols = Math.Min(this.gameArea.Width / cellSize, this.cellGrid.Cols - startCol);
                int visibleRows = Math.Min(this.gameArea.Height / cellSize, this.cellGrid.Rows - startRow);

                // FYI: Parallelization is not a good idea in the case of drawing the cells, even though it may seem like it:
                // - GDI+ is not thread safe
                // - The loop operations are very cheap and only limited to a small subarea of the grid matrix
                for (int row = 0; row < visibleRows; row++)
                {
                    for (int col = 0; col < visibleCols; col++)
                    {
                        Cell cell = this.cellGrid.Cells[(startRow + row) * this.cellGrid.Cols + (startCol + col)];
                        if (cell.IsAlive)
                        {
                            // Draw at screen-relative position
                            g.FillRectangle(cellBrush, new Rectangle(
                                new Point(col * cellSize, row * cellSize),
                                new Size(cellSize - 1, cellSize - 1)));
                        }
                    }
                }

                this.gameArea.Image?.Dispose();
                this.gameArea.Image = (Bitmap)bmp.Clone();
            }
        }

    }
}
