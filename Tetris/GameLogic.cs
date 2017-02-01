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

namespace Tetris
{
    public class GameLogic
    {
        //This object contains the logic and methods for keeping track of the level, rows cleared, and score
        //As well as updating the appropriate labels on the main window.

        private int points;
        private int level;
        private int rows;

        private System.Windows.Controls.TextBox ScoreBox;
        private System.Windows.Controls.TextBox LevelBox;
        private System.Windows.Controls.TextBox RowsBox;

        public GameLogic(System.Windows.Controls.TextBox scoreBox, System.Windows.Controls.TextBox levelBox, System.Windows.Controls.TextBox rowsBox)
        {
            this.points = 0;
            this.level = 1;
            this.rows = 0;

            this.ScoreBox = scoreBox;
            this.LevelBox = levelBox;
            this.RowsBox = rowsBox;
        }

        public void initializeBoxes()
        {
            //this should ONLY be called after a new GameLogic object is instantiated.
            this.ScoreBox.Text = points.ToString();
            this.LevelBox.Text = level.ToString();
            this.RowsBox.Text = rows.ToString();
        }
        public void updateScore()
        {
            this.points += level * 10;
            this.ScoreBox.Text = points.ToString();
        }

        public void updateLevel()
        {
            this.level++;
            this.LevelBox.Text = level.ToString();
        }

        public void updateRows()
        {
            this.rows++;
            this.RowsBox.Text = rows.ToString();
            updateScore();

            //Every 5 + level rows = a new level i.e. (5, 11, 18...)
            if (this.rows >= ((this.level - 1) + (5 * this.level)))
            {
                updateLevel();
            }
        }

        public int getLevel()
        {
            return this.level;
        }
    }
}
