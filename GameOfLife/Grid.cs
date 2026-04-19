using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Grid
    {
        public List<Cell> Cells { get; } = new List<Cell>();

        private int rows;
        private int cols;
        private readonly int batchSize;

        public Grid(int rows, int cols)
        {
            this.Rows = rows;
            this.Cols = cols;
            this.batchSize = (rows*cols) / Environment.ProcessorCount;
            Console.WriteLine("Batch size for parallel advance-step: " + this.batchSize);
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

        public void InitializeGrid(int cellSize, bool random)
        {
            this.Cells.Clear();

            // Allocate the row's cells upfront for index access
            Cell[] rowCells = new Cell[cols];

            var watch = System.Diagnostics.Stopwatch.StartNew();
            // Create and add cells to the Grid object in row-major order
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    Cell newCell = new Cell(x, y);

                    newCell.IsAlive = random && (Random.Shared.Next(100) < 15);
                    rowCells[x] = newCell;
                }
                // Single-threaded add after parallel work is done
                foreach (Cell cell in rowCells)
                {
                    this.Cells.Add(cell);
                }
                rowCells = new Cell[cols];
            }

            watch.Stop();
            Console.WriteLine("Runtime grid initialization: " + watch.ElapsedMilliseconds.ToString() + "ms");
        }

        public void AdvanceOneGeneration()
        {
            Parallel.ForEach(
                Partitioner.Create(0, Cells.Count, this.batchSize),
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
