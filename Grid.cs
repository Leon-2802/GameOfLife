using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Grid
    {
        // REFACTOR: better private?
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

        // REFACTOR METHOD
        public int LiveAdjacent(Cell cell)
        {
            int liveAdjacent = 0;

            int cellIndex = cell.YPos * this.Cols + cell.XPos; //formula for index

            //compute index of all neighbours
            int upperLeft = cellIndex - this.Cols - 1;
            int upperMiddle = cellIndex - this.Cols;
            int upperRight = cellIndex - this.Cols + 1;
            int middleLeft = cellIndex - 1;
            int middleRight = cellIndex + 1;
            int bottomLeft = cellIndex + this.Cols - 1;
            int bottomMiddle = cellIndex + this.Cols;
            int bottomRight = cellIndex + this.Cols + 1;

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
}
