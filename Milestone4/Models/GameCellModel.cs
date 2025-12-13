/*
 * GameCell class by Danielle DeSilvio
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone4.Models
{
    public class GameCellModel
    {
        public string Id { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public bool Flagged { get; set; }
        public bool Visited { get; set; }
        public bool Live { get; set; }
        public int LiveNeighbors { get; set; }
        public string Image { get; set; }

        // Default constructor
        public GameCellModel()
        {
            this.Id = "";
            this.Row = -1;
            this.Col = -1;
            this.Flagged = false;
            this.Visited = false;
            this.Live = false;
            this.LiveNeighbors = 0;
            this.Image = "/img/unexplored.png";
        }

        // Constructor with Row/Column coordinates
        public GameCellModel(int row, int col)
        {
            this.Id = row + "," + col;
            this.Row = row;
            this.Col = col;
            this.Flagged = false;
            this.Visited = false;
            this.Live = false;
            this.LiveNeighbors = 0;
            this.Image = "/img/unexplored.png";
        }
    }
}
