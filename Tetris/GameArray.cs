/* Tetris, By Joshua Ohm 2015
 * 
 * Notes: My project differs from Schimpf's in the following ways.
 * 
 *        - My grid is 10 wide x 22 tall, with the top 2 rows being invisible.
 *        - I start at a 750 tick rate instead of 500.
 *        - No home cheat code.
 *        - My controls are different (see About->Controls).
 *        
 * Known Issues: 
 * 
 *        - Moving a tetrimino horizontally into another locks it into place.
 *          I would prefer it keep descending, but ran out of time to figure this out.
 *        - Line blocks sometimes collide incorrectly, usually at the top of the screen right before you
 *          lose the game. I ran out of time to figure that out as well.
 */
using System;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Tetris
{
   public class GameArray
   {
       //The GameArray contains a 2D grid and the logic used for collision checking.
        private Rectangle [,] Map;
        public GameLogic Logic;
        public Canvas GameCanvas;

       public GameArray(Canvas canvas, GameLogic logic)
        {
            this.Map = new Rectangle[22, 10];
            this.GameCanvas = canvas;
            this.Logic = logic;
            
        }

       public void ClearArray()
       {
           for (int y = 0; y < this.Map.GetLength(0); y++)
           {
               for (int x = 0; x < this.Map.GetLength(1); x++)
               {
                   this.Map[y, x] = null;
               }
           }
       }

       public Rectangle[,] GetMap()
       {
           return this.Map;
       }

       public Rectangle GetBlock(int x, int y)
       {
           x = GridToArray(x);
           y = GridToArray(y);

           return this.Map[y,x];
       }
        public int GridToArray(int coord)
        {
            coord = coord/20;
            return coord;
        }

        public int ArrayToGrid(int coord)
        {
            coord = coord*20;
            return coord;
        }

       public void AddBlock(Rectangle rect, int x, int y)
       {
          
           x = GridToArray(x);
           y = GridToArray(y);

           this.Map[y,x] = rect;
       }

       public void ClearCurrent()
       {
           //Clears all blocks with "Current" in their uid from the array.
           for (int y = 0; y < this.Map.GetLength(0); y++)
           {
               for (int x = 0; x < this.Map.GetLength(1); x++)
               {
                   if (this.Map[y,x] != null && this.Map[y,x].Uid.StartsWith("Current"))
                   {
                       this.Map[y,x] = null;
                   }
               }
           }
       }

       public void RemoveBlock(int x, int y)
       {
           x = GridToArray(x);
           y = GridToArray(y);
           this.Map[y,x] = null;
       }
        public Boolean CheckIfOccupied(int x, int y)
        {
            //returns false if the spot is free, true if the spot is occupied
            x = GridToArray(x);
            y = GridToArray(y);

            if (this.Map[y,x] != null)
            {
                
                return false;
            }
            else
            {
                return true;
            }
        }

       public Boolean CheckBounds(int x, int y)
       {
           if (GridToArray(x) >= 0 && GridToArray(x) < 10 && GridToArray(y) >= 0 && GridToArray(y) < 22)
           {
               return true;
           }
           else
           {
               
               return false;
           }
       
       }
       public Boolean CheckBounds(int[] xArray, int[] yArray)
       {
           //Returns false if the coordinate 
           if (xArray.Length == yArray.Length)
           {
               for (int i = 0; i < xArray.Length; i++)
               {
                   if ((GridToArray(xArray[i]) >= 0 && GridToArray(xArray[i]) < 10) && (GridToArray(yArray[i]) >= 0 && GridToArray(yArray[i]) < 22))
                   {
                       
                   }
                   else
                   {
                       
                       return false;
                   }
               }
               return true;
           }
           else
           {
               return false;
           }
       }
        public Boolean CheckIfOccupied(int[] xArray, int[] yArray)
        {
            //returns true if the spot is free, false if the spot is occupied
            if (xArray.Length == yArray.Length)
            {
                for(int i = 0; i < xArray.Length; i++)
                {
                    if (GridToArray(yArray[i]) > 0)
                    {
                        try
                        {
                            if (this.Map[GridToArray(yArray[i]), GridToArray(xArray[i])] != null && this.Map[GridToArray(yArray[i]), GridToArray(xArray[i])].Uid.StartsWith("Placed"))
                            {
                                return false;
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
            
        }

        public void CheckRowsForTetris()
        {
            Boolean rowFull = true;
            int lastRowCleared = -1;

            //check each row, if all squares in row are occupied, call ClearRow()
            for (int y = 0; y < this.Map.GetLength(0); y++)
            {
                for (int x = 0; x < this.Map.GetLength(1); x++)
                {
                    if (this.Map[y, x] == null)
                    {
                        rowFull = false;
                    }
                }

                if (rowFull)
                {
                    ClearRow(y);
                    lastRowCleared = y;
                }

                rowFull = true;
            }

        }

        public Boolean checkForGameLoss()
        {
            //Check the first 3 rows for placed blocks (the first two rows are invisible)
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < this.Map.GetLength(1); x++)
                {
                    if (this.Map[y,x] != null && this.Map[y, x].Uid.StartsWith("Placed"))
                    {
                        //Player has lost the game.
                        return true;
                    }
                }
            }

            return false;
        }

        public void ClearRow(int y)
        {
            //clear the row that needs to be cleared.
            for (int x = 0; x < this.Map.GetLength(1); x++)
            {
                this.GameCanvas.Children.Remove(this.Map[y,x]);
                this.Map[y, x] = null;
                
            }
            this.Logic.updateRows();
            CheckRowsForEmptiness();
        }

       public void CheckRowsForEmptiness()
       {
           Boolean FoundNonEmptyRow = false;
           int firstNonEmptyRow = -1;

           
           

           for (int y = 0; y < this.Map.GetLength(0); y++)
           {
               Boolean rowEmpty = true;
               for (int x = 0; x < this.Map.GetLength(1); x++)
               {
                   if (!FoundNonEmptyRow)
                   {
                       if (this.Map[y, x] != null)
                       {
                           FoundNonEmptyRow = true;
                           firstNonEmptyRow = y;
                       }
                   }
                   else
                   {
                       if (this.Map[y, x] == null)
                       {

                       }
                       else
                       {
                           rowEmpty = false;
                       }
                   }
               }

               if (rowEmpty && FoundNonEmptyRow)
               {
                   if (firstNonEmptyRow != -1 && y > firstNonEmptyRow)
                   {
                       DropPlacedBlocks(firstNonEmptyRow, y);
                   }
               }
           }
       }

        public void DropPlacedBlocks(int firstNonEmptyRow, int row)
        {
            for (int y = row; y > 0; y--)
            {
                for (int x = 0; x < this.Map.GetLength(1); x++)
                {
                    this.Map[y, x] = this.Map[(y - 1), x];
                }
            }

            RefreshCanvas();
        }

        public void RefreshCanvas()
        {
            this.GameCanvas.Children.Clear();

            for (int y = 0; y < this.Map.GetLength(0); y++)
            {
                for (int x = 0; x < this.Map.GetLength(1); x++)
                {
                    if (this.Map[y,x] != null && this.Map[y, x].Uid.StartsWith("Placed"))
                    {
                        this.GameCanvas.Children.Add(this.Map[y, x]);
                        Canvas.SetTop(this.Map[y, x], ArrayToGrid(y)-40);
                        Canvas.SetLeft(this.Map[y, x], ArrayToGrid(x));
                    }
                }
            }
        }
    }
}
