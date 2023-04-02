using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class GameOfLife : Form
    {
        Boolean running = false;
        Grid CellGrid;

        public GameOfLife()
        {
            InitializeComponent();
        }

        private void Load_GameOfLife(object sender, EventArgs e)
        {
            CreateGridSurface(true);
        }

        private void CreateGridSurface(bool randomCells)
        {
            Point locPoint;
            Cell newCell;
            Random random = new Random();

            int rows = (int)(gameArea.Height / numCSize.Value);
            int cols = (int)(gameArea.Width / numCSize.Value);

            //Create Grid Object
            CellGrid = new Grid(rows, cols);


            //using innnerhalb der Klammern limitiert die Existenz der Objekte auf diese Methode -> weniger Memory Müll
            using (Bitmap bmp = new Bitmap(gameArea.Width, gameArea.Height))
            using (Graphics g = Graphics.FromImage(bmp))
            using (SolidBrush cellBrush = new SolidBrush(Color.Green))
            {
                g.Clear(Color.Black);
                gameArea.Image = (Bitmap)bmp.Clone();

                Grid.gridCells.Clear();

                for(int y = 0; y < rows; y++)
                {
                    for(int x = 0; x < cols; x++)
                    {
                        locPoint = new Point((int)(x * numCSize.Value), (int)(y * numCSize.Value));
                        newCell = new Cell(locPoint, x, y);

                        if(!randomCells)
                            newCell.IsAlive = false;
                        else
                            newCell.IsAlive = (random.Next(100) < 15) ? true : false; //true wenn kleiner als 15, false wenn größer

                        Grid.gridCells.Add(newCell);
                    }
                }

                Grid.gridCells = Grid.gridCells.OrderBy(c => c.XPos).OrderBy(c => c.YPos).ToList(); //nach y und x ordnen (wie im for loop vorgegeben)          

                //Alle neuen Cells ins Grid laden:
                foreach (Cell cell in Grid.gridCells)
                {
                    if(cell.IsAlive)
                    {
                        g.FillRectangle(cellBrush, new Rectangle(cell.Location, 
                            new Size((int)numCSize.Value - 1, (int)numCSize.Value - 1)));
                    }
                }

                gameArea.Image.Dispose(); //Befreit Memory von Ressourcen des vorherigen Bildes
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
            GetNextState();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            running = !running;

            startBtn.Text = running ? "Stop" : "Start";

            while(running)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                GetNextState();
                Application.DoEvents(); //Wait for operations to finish
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                if(elapsedMs < 101)
                {
                    int sleepTime = 100 - (int)elapsedMs;
                    Thread.Sleep(sleepTime);
                }
            }
        }

        private void gameArea_MouseClick(object sender, MouseEventArgs e)
        {
            int cellIndex;

            //Get cell size 
            int cellSize = (int) numCSize.Value;

            //Get the correct square on the grid, the mouse is hovering on
            int Xloc = e.X / cellSize;
            int YLoc = e.Y / cellSize;


            //Get cell list index from grid index
            cellIndex = (YLoc * CellGrid.Cols) + Xloc; // ! Formel für index

            //Flip state of cell between dead and alive
            Grid.gridCells[cellIndex].IsAlive = !Grid.gridCells[cellIndex].IsAlive;

            UpdateGrid();
        }

        private void GameOfLife_FormClosing(object sender, FormClosingEventArgs e)
        {
            //End program when closing window
            running = false;
            Application.Exit();
        }

        private void GetNextState()
        {
            //Angrenzende Zellen prüfen auf IsAlive und Position -> Grid Updaten
            /*
            1. Weniger als zwei Nachbarn -> Tod
            2. Zwei oder Drei Nachbarn -> bleibt am Leben
            3. Mehr als drei Nachbarn -> Tod
            4. Jede tote Zelle mit genau 3 Nachbarn wird wiederbelebt
            */

            void ComputeCellState(Cell cell)
            {
                int activeCount = CellGrid.LiveAdjacent(cell);

                if (cell.IsAlive)
                {
                    if ((activeCount < 2) || (activeCount > 3))
                        cell.AllowLiving = false;
                    else
                        cell.AllowLiving = true;
                }
                else
                {
                    if (activeCount == 3)
                        cell.AllowLiving = true;
                }
            }

            foreach(Cell cell in Grid.gridCells)
            {
                //Thread t = new Thread(() => ComputeCellState(cell));
                //t.Start();
                int activeCount = CellGrid.LiveAdjacent(cell);

                if (cell.IsAlive)
                {
                    if ((activeCount < 2) || (activeCount > 3))
                        cell.AllowLiving = false;
                    else
                        cell.AllowLiving = true;
                }
                else
                {
                    if (activeCount == 3)
                        cell.AllowLiving = true;
                }
            }

            foreach(Cell cell in Grid.gridCells)
            {
                cell.IsAlive = cell.AllowLiving;
            }

            UpdateGrid();
        }

        private void UpdateGrid()
        {
            using (Bitmap bmp = new Bitmap(gameArea.Width, gameArea.Height))
            using (Graphics g = Graphics.FromImage(bmp))
            using (SolidBrush cellBrush = new SolidBrush(Color.Green))
            {
                g.Clear(Color.Black);

                foreach (Cell cell in Grid.gridCells)
                {
                    if (cell.IsAlive)
                    {
                        g.FillRectangle(cellBrush, new Rectangle(cell.Location,
                            new Size((int)numCSize.Value - 1, (int)numCSize.Value - 1)));
                    }
                }

                    
                gameArea.Image.Dispose();
                gameArea.Image = (Bitmap)bmp.Clone();

            }
        }

    }

    public class Grid
    {
        public static List<Cell> gridCells = new List<Cell>();

        private int rows;
        private int cols;

        public Grid(int rows, int cols)
        {
            this.Rows = rows;
            this.Cols = cols;
        }

        public int Rows
        {
           get { return rows; } 
           set { rows = value; } 
        }

        public int Cols
        {
            get { return cols; }
            set { cols = value; }
        }

        public int LiveAdjacent(Cell cell)
        {
            int liveAdjacent = 0;

            int cellIndex = cell.YPos * this.Cols + cell.XPos; //formula for index

            //compute index of all neighbours
            int upperLeft = cellIndex - this.Cols - 1;
            int upperMiddle = cellIndex - this.Cols;
            int upperRight = cellIndex - this.Cols +1;
            int middleLeft = cellIndex - 1;
            int middleRight = cellIndex + 1;
            int bottomLeft = cellIndex + this.Cols -1;
            int bottomMiddle = cellIndex + this.Cols;
            int bottomRight = cellIndex + this.Cols +1;

            //check if index still in bounds and if cell is alive -> if all arguments are true = neighbour exists and is alive
            if (upperLeft >= 0 && Grid.gridCells[upperLeft].IsAlive)
                liveAdjacent++;
            if (upperMiddle >= 0 && Grid.gridCells[upperMiddle].IsAlive)
                liveAdjacent++;
            if (upperRight >= 0 && upperRight <= (Grid.gridCells.Count - 1) && Grid.gridCells[upperRight].IsAlive)
                liveAdjacent++;
            if (middleLeft >= 0 && Grid.gridCells[middleLeft].IsAlive)
                liveAdjacent++;
            if (middleRight <= (Grid.gridCells.Count - 1) && Grid.gridCells[middleRight].IsAlive)
                liveAdjacent++;
            if (bottomLeft >= 0 && bottomLeft <= (Grid.gridCells.Count - 1) && Grid.gridCells[bottomLeft].IsAlive)
                liveAdjacent++;
            if (bottomMiddle <= (Grid.gridCells.Count - 1) && Grid.gridCells[bottomMiddle].IsAlive)
                liveAdjacent++;
            if (bottomRight <= (Grid.gridCells.Count - 1) && Grid.gridCells[bottomRight].IsAlive)
                liveAdjacent++;


            return liveAdjacent;
        }
    }

    public class Cell
    {
        private Point location;
        private int xPos;
        private int yPos;
        private Boolean isAlive;
        private Boolean allowLiving;

        public Cell(Point location, int x, int y)
        {
            this.Location = location;
            this.XPos = x;
            this.YPos = y;
        }

        public Point Location
        {
            get { return location; }
            set { location = value; }
        }
        public int XPos
        {
            get { return xPos; }
            set { xPos = value; }
        }
        public int YPos
        {
            get { return yPos; }
            set { yPos = value; }
        }
        public Boolean IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }
        public Boolean AllowLiving
        {
            get { return allowLiving; }
            set { allowLiving = value; }
        }

        public override string ToString()
        {
            //Override the way list is printed out:
            return $"GridX:{this.XPos} GridY:{this.YPos} LocX{this.Location.X} LocY:{this.Location.Y}";
        }
    }
}
