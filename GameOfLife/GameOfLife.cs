using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
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
            CreateGridSurface(true);
        }

        private void CreateGridSurface(bool randomCells)
        {
            Random random = new Random();

            int rows = (int)(gameArea.Height / numCSize.Value);
            int cols = (int)(gameArea.Width / numCSize.Value);

            cellGrid = new Grid(rows, cols);


            // Limit existense of included objects to the scope of this method
            using (Bitmap bmp = new Bitmap(gameArea.Width, gameArea.Height))
            using (Graphics g = Graphics.FromImage(bmp))
            using (SolidBrush cellBrush = new SolidBrush(Color.Green))
            {
                g.Clear(Color.Black);
                gameArea.Image = (Bitmap)bmp.Clone();

                cellGrid.Cells.Clear();
                Cell newCell;

                // Create and add cells to the Grid object in row-major order
                for(int y = 0; y < rows; y++)
                {
                    for(int x = 0; x < cols; x++)
                    {
                        newCell = new Cell(
                            new Point((int)(x * numCSize.Value), (int)(y * numCSize.Value)), 
                            x, 
                            y);

                        if(!randomCells)
                            newCell.IsAlive = false;
                        else
                            newCell.IsAlive = (random.Next(100) < 15); // true if smaller than 15

                        cellGrid.Cells.Add(newCell);
                    }
                }
        
                // Load all new cells into the grid
                foreach (Cell cell in cellGrid.Cells)
                {
                    if(cell.IsAlive)
                    {
                        // PARALLELIZABLE (maybe not worth it due to too much overhead)
                        g.FillRectangle(cellBrush, new Rectangle(cell.UILocation, 
                            new Size((int)numCSize.Value - 1, (int)numCSize.Value - 1)));
                    }
                }

                gameArea.Image.Dispose(); // Frees memory of resources of prior image
                gameArea.Image = (Bitmap)bmp.Clone();
            }
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            CreateGridSurface(true);
        }
        private void clearBtn_Click(object sender, EventArgs e)
        {
            CreateGridSurface(false);
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

            UpdateGrid();
        }

        private void GameOfLife_FormClosing(object sender, FormClosingEventArgs e)
        {
            // End program when closing window
            running = false;
            Application.Exit();
        }

        private void Advance()
        {
            cellGrid.AdvanceOneGeneration();
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            using (Bitmap bmp = new Bitmap(gameArea.Width, gameArea.Height))
            using (Graphics g = Graphics.FromImage(bmp))
            using (SolidBrush cellBrush = new SolidBrush(Color.Green))
            {
                g.Clear(Color.Black);

                foreach (Cell cell in cellGrid.Cells)
                {
                    if (cell.IsAlive)
                    {
                        // PARALLELIZABLE (maybe not worth it due to too much overhead)
                        g.FillRectangle(cellBrush, new Rectangle(cell.UILocation,
                            new Size((int)numCSize.Value - 1, (int)numCSize.Value - 1)));
                    }
                }

                    
                gameArea.Image.Dispose();
                gameArea.Image = (Bitmap)bmp.Clone();
            }
        }

    }
}
