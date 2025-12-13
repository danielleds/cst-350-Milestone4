using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone4.Models
{
    public class GameBoardViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Size { get; set; }
        public decimal Difficulty { get; set; }
        public int Score { get; set; }
        public int Time { get; set; }
        public bool FirstMove { get; set; }
        public bool GameFinished { get; set; }
        public bool GameWon { get; set; }
        public GameCellModel[,] Grid { get; set; }
        public int SafeCells { get; set; }
        public int CellsRevealed { get; set; }

        public GameBoardViewModel(int size, decimal difficulty)
        {
            this.UserId = 0;
            this.Size = size;
            this.Difficulty = difficulty;
            this.Score = 0;
            this.FirstMove = true;
            this.GameFinished = false;
            this.GameWon = false;
            this.Grid = new GameCellModel[this.Size, this.Size];
            this.SafeCells = this.Size * this.Size;
            this.CellsRevealed = 0;

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    this.Grid[x, y] = new GameCellModel(x, y);
                }
            }
            this.SetupLiveNeighbors();
        }

        public GameBoardViewModel()
        {

        }

        public void EndGame(bool gameWon)
        {
            this.GameFinished = true;
            this.GameWon = gameWon;
            if (!gameWon)
            {
                this.RevealMines();
            }

            // will potentially revisit the scoring system; it doesn't feel very balanced
            decimal multiplier = 1.0M;

            // size multiplier
            if (this.Size == 12)
                multiplier += 0.1M;
            else if (this.Size == 24)
                multiplier += 0.2M;
            else
                multiplier += 0.3M;

            // Difficulty Multiplier
            if (this.Difficulty == 0.10M)
                multiplier += 0.1M;
            else if (this.Difficulty == 0.16M)
                multiplier += 0.2M;
            else
                multiplier += 0.3M;

            if (this.GameWon)
            {
                this.Score = Convert.ToInt32(50 * multiplier);

                // time bonus; add additional points if players finished under a time
                // determined by size and difficulty choice
                int timeThreshold = 0;
                if (this.Size == 32)
                {
                    if (this.Difficulty == 0.22M)
                        timeThreshold = (20 * 60000);
                    else if (this.Difficulty == 0.16M)
                        timeThreshold = (15 * 60000);
                    else
                        timeThreshold = (10 * 60000);
                }
                else if (this.Size == 24)
                {
                    if (this.Difficulty == 0.22M)
                        timeThreshold = (10 * 60000);
                    else if (this.Difficulty == 0.16M)
                        timeThreshold = (7 * 60000);
                    else
                        timeThreshold = (5 * 60000);
                }
                else
                {
                    if (this.Difficulty == 0.22M)
                        timeThreshold = (7 * 60000);
                    else if (this.Difficulty == 0.16M)
                        timeThreshold = (5 * 60000);
                    else
                        timeThreshold = (3 * 60000);
                }
                if (this.Time <= timeThreshold)
                    this.Score += 20;
            }
        }

        public void SetupLiveNeighbors()
        {
            Random random = new Random();

            foreach (GameCellModel cell in Grid)
            {
                /* To determine whether the cell is live:
                 * Generate float with random.NextSingle(), which generates a number between 0.0 and 1.0
                 * Pass it as a parameter to a single use new Decimal object
                 * Compare the decimal object against the difficulty rate
                 * if less, the cell is live
                 */
                if (new Decimal(random.NextSingle()) < Difficulty)
                {
                    cell.Live = true;
                    this.SafeCells -= 1;
                }
            }
        }

        public void CalculateLiveNeighbors()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    // Call another method to loop through the neighbors and find out whether they are live or not
                    this.Grid[x, y].LiveNeighbors = FindLiveNeighbors(x, y);
                }
            }
        }

        private int FindLiveNeighbors(int r, int c)
        {
            int count = 0; // keep track of the live neighbors

            // Loops start at previous coordinates and ends at the next coordinates
            for (int x = (r - 1); x < (r + 2); x++)
            {
                // Ensure coordinates aren't out of bounds
                if (x < 0 || x == Size) continue;

                for (int y = (c - 1); y < (c + 2); y++)
                {
                    // Ensure coordinates aren't out of bounds
                    if (y < 0 || y == Size) continue;

                    // Add to the live neighbor count if the neighbor is live
                    if (Grid[x,y].Live)
                        count++;
                }
            }

            return count;
        }

        // Make the cell at the given coordinates and all of its neighbors safe.
        // This is done for the first move of the game
        public void EnsureSafe(int r, int c)
        {
            // Loops start at previous coordinates and ends at the next coordinates
            for (int x = (r - 1); x < (r + 2); x++)
            {
                // Ensure coordinates aren't out of bounds
                if (x < 0 || x == Size) continue;

                for (int y = (c - 1); y < (c + 2); y++)
                {
                    // Ensure coordinates aren't out of bounds
                    if (y < 0 || y == Size) continue;

                    if (Grid[x, y].Live)
                    {
                        this.SafeCells += 1;
                        Grid[x, y].Live = false;
                    }
                }
            }
            this.CalculateLiveNeighbors();
        }

        public void FloodFill(GameCellModel cell)
        {
            // reveal the cell first and set its image
            cell.Visited = true;
            this.CellsRevealed += 1;

            if (cell.Live)
            {
                cell.Image = "/img/mine_hit.png";
            }
            else
            {
                cell.Image = "/img/" + cell.LiveNeighbors + ".png";
            }

            // Don't proceed if the cell is live or has live neighbors
            if (cell.Live || cell.LiveNeighbors > 0)
                return;

            int r = cell.Row;
            int c = cell.Col;

            // Loops start at previous coordinates and ends at the next coordinates
            for (int x = (r - 1); x < (r + 2); x++)
            {
                // Ensure coordinates aren't out of bounds
                if (x < 0 || x == Size) continue;

                for (int y = (c - 1); y < (c + 2); y++)
                {
                    // Ensure coordinates aren't out of bounds
                    if (y < 0 || y == Size) continue;

                    // perform a recursive call on the cell if it isn't revealed or live
                    if (!Grid[x, y].Visited && !Grid[x, y].Live)
                        FloodFill(Grid[x, y]);
                }
            }
        }

        public bool SafeCellsRevealed()
        {
            return this.CellsRevealed == this.SafeCells;
        }

        // Called upon losing a game to reveal all mine locations
        public void RevealMines()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (Grid[x, y].Live)
                    {
                        Grid[x, y].Image = "/img/mine_hit.png";
                    }
                }
            }
        }
    }
}
