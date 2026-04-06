using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class CellTest
    {
        private Cell CreateCell(int x, int y)
        {
            return new Cell(x, y);
        }

        [Fact]
        public void ComputeCellState_FirstRule()
        {
            Cell cell = CreateCell(1, 2);
            cell.IsAlive = true;
            cell.ComputeCellState(1);
            Assert.False(cell.AllowLiving);
        }

        [Fact]
        public void ComputeCellState_SecondRule()
        {
            Cell cell = CreateCell(1, 2);
            cell.IsAlive = true;
            cell.ComputeCellState(2);
            Assert.True(cell.AllowLiving);
            cell.ComputeCellState(3);
            Assert.True(cell.AllowLiving);
        }

        [Fact]
        public void ComputeCellState_ThirdRule()
        {
            Cell cell = CreateCell(1, 2);
            cell.IsAlive = true;
            cell.ComputeCellState(4);
            Assert.False(cell.AllowLiving);
        }

        [Fact]
        public void ComputeCellState_FourthRule()
        {
            Cell cell = CreateCell(1, 2);
            cell.IsAlive = false;
            cell.ComputeCellState(2);
            Assert.False(cell.AllowLiving);
            cell.ComputeCellState(3);
            Assert.True(cell.AllowLiving);
        }
    }

}
