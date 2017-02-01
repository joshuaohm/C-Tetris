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
using System.Diagnostics;


namespace Tetris
{
    class TetriminoFactory
    {
        //This is a factory used to generate new Tetriminoes randomly.

        private System.Windows.Controls.Canvas GameCanvas;
        private GameArray GameArray;
        private Random roll = new Random();

        public TetriminoFactory(System.Windows.Controls.Canvas gameCanvas, GameArray gameArray)
        {
            this.GameCanvas = gameCanvas;
            this.GameArray = gameArray;
        }

        public Tetrimino getNewBlock(){

            int choice = roll.Next(0,7);

            Debug.WriteLine("next block roll: " + choice);

            if (choice == 0)
            {
                return new LBlock(GameCanvas, GameArray);
            }

            else if (choice == 1)
            {
                return new JBlock(GameCanvas, GameArray);
            }

            else if (choice == 2)
            {
                return new TBlock(GameCanvas, GameArray);
            }

            else if (choice == 3)
            {
                return new ZBlock(GameCanvas, GameArray);
            }

            else if (choice == 4)
            {
                return new SBlock(GameCanvas, GameArray);
            }

            else if (choice == 5)
            {
                return new SquareBlock(GameCanvas, GameArray);
            }

            else if (choice == 6)
            {
                return new LineBlock(GameCanvas, GameArray);
            }

            else
            {
                return null;
            }

        }
    }
}
