using System;
using System.Drawing;

namespace GameOfLife
{
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

        public void ComputeCellState(int activeCount)
        {
            // Rules: 
            /*
            1. Less than two neighbours -> Death
            2. Two or three neighbours -> Stay alive
            3. More than three neighbours -> Death
            4. A dead cell with exactly three neighbours is getting revived
            */
            if (this.IsAlive)
            {
                if ((activeCount < 2) || (activeCount > 3))
                    this.AllowLiving = false;
                else
                    this.AllowLiving = true;
            }
            else
            {
                if (activeCount == 3)
                    this.AllowLiving = true;
            }
        }

        public override string ToString()
        {
            // Override the way list is printed out:
            return $"GridX:{this.XPos} GridY:{this.YPos} LocX{this.Location.X} LocY:{this.Location.Y}";
        }
    }
}
