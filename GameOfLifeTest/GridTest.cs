using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameOfLife
{
    public class GridTest
    {
        private static Grid CreateGrid(int rows, int cols, List<(int x, int y)> aliveCells)
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
        public void InitializeGrid_PopulatesCorrectCellCount()
        {
            var grid = new Grid(5, 10);
            grid.InitializeGrid(cellSize: 10, random: false);

            Assert.Equal(50, grid.Cells.Count);
        }

        [Fact]
        public void InitializeGrid_CellsHaveCorrectGridCoordinates()
        {
            int rows = 3, cols = 4;
            var grid = new Grid(rows, cols);
            grid.InitializeGrid(cellSize: 10, random: false);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    var cell = grid.Cells[y * cols + x];
                    Assert.Equal(x, cell.XPos);
                    Assert.Equal(y, cell.YPos);
                }
            }
        }

        [Fact]
        public void InitializeGrid_CellsHaveCorrectPixelPositions()
        {
            int cellSize = 16;
            var grid = new Grid(2, 3);
            grid.InitializeGrid(cellSize, random: false);

            Assert.Equal(new Point(0, 0), grid.Cells[0].UILocation);
            Assert.Equal(new Point(cellSize, 0), grid.Cells[1].UILocation);
            Assert.Equal(new Point(2 * cellSize, 0), grid.Cells[2].UILocation);
            Assert.Equal(new Point(0, cellSize), grid.Cells[3].UILocation);
        }

        [Fact]
        public void InitializeGrid_NonRandom_AllCellsAreDead()
        {
            var grid = new Grid(4, 4);
            grid.InitializeGrid(cellSize: 10, random: false);

            Assert.All(grid.Cells, cell => Assert.False(cell.IsAlive));
        }

        [Fact]
        public void InitializeGrid_Random_ProducesAtLeastOneLiveCell()
        {
            // With a 5x5 grid (25 cells) and a 15% spawn rate,
            // the probability of zero live cells is 0.85^25 ≈ 1.7% —
            // retry a few times to make the test reliable.
            bool anyAlive = false;
            for (int attempt = 0; attempt < 5 && !anyAlive; attempt++)
            {
                var grid = new Grid(5, 5);
                grid.InitializeGrid(cellSize: 10, random: true);
                anyAlive = grid.Cells.Any(c => c.IsAlive);
            }

            Assert.True(anyAlive);
        }

        [Fact]
        public void InitializeGrid_ClearsExistingCells()
        {
            var grid = new Grid(2, 2);
            grid.InitializeGrid(cellSize: 10, random: false);
            grid.InitializeGrid(cellSize: 10, random: false); // second call

            Assert.Equal(4, grid.Cells.Count);
        }

        [Fact]
        public void InitializeGrid_CellsAreInRowMajorOrder()
        {
            int rows = 3, cols = 3;
            var grid = new Grid(rows, cols);
            grid.InitializeGrid(cellSize: 10, random: false);

            for (int i = 0; i < grid.Cells.Count; i++)
            {
                Assert.Equal(i % cols, grid.Cells[i].XPos);
                Assert.Equal(i / cols, grid.Cells[i].YPos);
            }
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
