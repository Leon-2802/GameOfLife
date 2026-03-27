using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public void AdvanceOneGeneration()
        {
            Parallel.ForEach(
                Partitioner.Create(0, Cells.Count, 100),
                range =>
                {
                    // loop over the indeces included in the range
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        // Check if each cell is allowed to live according to the adjacency rules (see rules in Cell class)
                        Cell cell = this.Cells[i];
                        int activeCount = this.LiveAdjacent(cell);
                        cell.ComputeCellState(activeCount);
                    }
                });

            foreach (Cell cell in Cells)
            {
                // Only update the IsAlive properties after the parallel computation of the new cell states (AllowLiving)
                // That way the new generation is clearly seperated from the state of the prior one
                cell.IsAlive = cell.AllowLiving;
            }
        }

        private int LiveAdjacent(Cell cell)
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
