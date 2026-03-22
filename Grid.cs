using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class Grid
    {
        public List<Cell> Cells { get; } = new List<Cell>();

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
            var neighbourOffsets = new (int dx, int dy)[]
            {
                (-1, -1), (0, -1), (1, -1),
                (-1,  0),          (1,  0),
                (-1,  1), (0,  1), (1,  1)
            };

            // Count how many neighbour-cells satisfy the condition "IsAlive"
            return neighbourOffsets.Count(offset => IsAliveAt(cell.XPos + offset.dx, cell.YPos + offset.dy));
        }

        private bool IsAliveAt(int x, int y)
        {
            // Bounds check
            if (x < 0 || x >= Cols) return false;
            if (y < 0 || y >= Rows) return false;

            // access from cell list in row-major order
            return Cells[y * Cols + x].IsAlive; 
        }
    }
}
