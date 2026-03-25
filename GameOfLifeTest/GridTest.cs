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
        public void Advance_LiveCellWithTwoNeighbours_Survives()
        {
            Grid grid = CreateGrid(3, 3, aliveCells: [(0, 0), (1, 0), (2, 0)]);
            // (1,0) has exactly 2 neighbours

            grid.AdvanceOneGeneration();

            Assert.True(grid.Cells[1].IsAlive);
        }

        [Fact]
        public void Advance_LiveCellWithNoNeighbours_Dies()
        {
            Grid grid = CreateGrid(3, 3, aliveCells: [(1, 1)]);

            grid.AdvanceOneGeneration();

            Assert.False(grid.Cells[4].IsAlive);
        }

        [Fact]
        public void Advance_DeadCellWithThreeNeighbours_BecomesAlive()
        {
            // Classic birth rule
            Grid grid = CreateGrid(3, 3, aliveCells: [(0, 0), (2, 0), (1, 1)]);
            // (1,0) is dead but has 3 live neighbours

            grid.AdvanceOneGeneration();

            Assert.True(grid.Cells[1].IsAlive);
        }

        [Fact]
        public void Advance_TwoPassUpdate_DoesNotAffectSameGeneration()
        {
            // Verifies that cells updated early in the loop don't influence
            // cells updated later — the two-pass design must hold
            Grid grid = CreateGrid(3, 3, aliveCells: [(0, 0), (1, 0), (2, 0)]);

            grid.AdvanceOneGeneration();

            Assert.False(grid.Cells[0].IsAlive); // (0,0) dies
            Assert.True(grid.Cells[1].IsAlive);  // (1,0) survives
            Assert.False(grid.Cells[2].IsAlive); // (2,0) dies
        }

        [Fact]
        public void Advance_NonSquareGrid_AdvancesCorrectly()
        {
            // Grid with only one row
            Grid grid = CreateGrid(rows: 1, cols: 5, aliveCells: [(0, 0), (1,0), (2,0)]);

            grid.AdvanceOneGeneration();

            Assert.True(grid.Cells[1].IsAlive);
        }
    }
}
