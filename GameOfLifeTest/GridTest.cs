using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameOfLife
{
    public class GridTest
    {
        private Grid CreateGrid(int rows, int cols, List<(int x, int y)> aliveCells)
        {
            Grid grid = new Grid(rows, cols);
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    grid.Cells.Add(new Cell(new Point(x, y), x, y));
                }
            }

            foreach (var (x, y) in aliveCells)
            {
                grid.Cells[y * cols + x].IsAlive = true;
            }

            return grid;
        }

        [Fact]
        public void LiveAdjacent_CellWithThreeNeighbours_ReturnsThree()
        {
            Grid grid = CreateGrid(3, 3, aliveCells: [(0, 0), (1, 0), (2, 0)]);
            Cell middleCell = grid.Cells[4]; // centre cell at (1,1)

            int result = grid.LiveAdjacent(middleCell);

            Assert.Equal(3, result);
        }

        [Fact]
        public void LiveAdjacent_IsolatedCell_ReturnsZero()
        {
            Grid grid = CreateGrid(3, 3, aliveCells: []);
            Cell middleCell = grid.Cells[4];

            int result = grid.LiveAdjacent(middleCell);

            Assert.Equal(0, result);
        }
    }
}
