/* Tetris, By Joshua Ohm 2015
 * 
 * Notes: My project differs from Schimpf's in the following ways.
 * 
 *        - My grid is 10 wide x 22 tall, with the top 2 rows being invisible.
 *        - I start at a 750 tick rate instead of 500.
 *        - No cheat code.
 *        - My controls are different (see About->Controls).
 *        
 * Known Issues: 
 * 
 *        - Moving a tetrimino horizontally into another locks it into place.
 *          I would prefer it keep descending, but ran out of time to figure this out.
 *        - Line blocks sometimes collide incorrectly, usually at the top of the screen right before you
 *          lose the game. I ran out of time to figure that out as well.
 *        - Random isn't random enough for my liking.
 */
using System;
using System.Diagnostics;


namespace Tetris
{
    public abstract class Tetrimino
    {
        public string Type;
        public Boolean CanMove;
        public System.Windows.Controls.Canvas GameCanvas;
        public GameArray GameArray;
        public int Timer, XCoord, YCoord, Orientation;

        //The method for drawing the entire Tetrimino.
        public abstract void Draw();
        //The method for drawing the blocks that make up the Tetrimino while it's still moving.
        public abstract void DrawBlock(int x, int y);
        //The method for drawing the blocks that make up the Tetrimino once it's been placed.
        public abstract void PlaceBlock(int x, int y);
        public abstract void Clear();

        //Removes the Tetrimino from the screen.
        public void UnDraw()
        {
            
            for (int i = 0; i < this.GameCanvas.Children.Count; i++)
            {
                //because removing a child will mess with the position of the other elements, this while loop is necessary.
                while(i < this.GameCanvas.Children.Count && this.GameCanvas.Children[i].Uid.StartsWith("Current"))
                {
                    
                    this.GameCanvas.Children.RemoveAt(i);
                }
            }
        }
        //Moves the Tetrimino (logically and visually).
        public void Move(int x, int y, int orientation)
        {
            if (this.Type == "Line")
            {
                string str = "true";
                if (this.CanMove == false)
                {
                    str = "false";
                }
                Debug.WriteLine("moving to "+x+", "+y+" from "+this.XCoord+", "+this.YCoord+" "+str);

            }
            {
                if (this.CanMove == true)
                {
                    
                    
                    if (AttemptToMove(x, y, orientation))
                    {
                        
                        this.Clear();
                        this.XCoord = x;
                        this.YCoord = y;
                        this.Orientation = orientation;
                        this.Draw();
                    }
                    else
                    {
                        if ((x > 0 && x < 180) || y >= 400)
                        {

                            this.CanMove = false;
                        }
                        this.Clear();
                        this.Draw();
                    }
                }
                else
                {
                    
                    this.Clear();
                    this.Draw();
                }
            }
        }

        //this function will generate an array of a Tetrimino's block's x coordinates, used to check for collision.
        public abstract int[] GenerateXArray(int x, int orientation);

        //this function will generate an array of a Tetrimino's block's x coordinates, used to check for collision.
        public abstract int[] GenerateYArray(int y, int orientation);

        //Contains the logic to check for collisions before actually moving a Tetrimino.
        public Boolean AttemptToMove(int x, int y, int orientation)
        {
            int[] xArray = GenerateXArray(x, orientation);
            int[] yArray = GenerateYArray(y, orientation);

            if (this.GameArray.CheckBounds(xArray, yArray))
            {
                if (this.GameArray.CheckIfOccupied(xArray, yArray))
                {
                    return true;
                }
                else
                {

                        this.CanMove = false;
                        return false;
                    

                }
            }
            else
            {
                return false;
            }
        }
    }
}
