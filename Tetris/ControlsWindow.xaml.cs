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

using System.Windows;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for ControlsWindow.xaml
    /// </summary>
    public partial class ControlsWindow : Window
    {
        public ControlsWindow()
        {
            InitializeComponent();
            RulesMessage();
        }

        public void RulesMessage()
        {
            MessageLabel.Content = "Thank you for playing Tetris! \r\n\r\n"
                +"Controls:\r\n"
                +"Enter:                         Start a new game. \r\n"
                +"P:                               Pause the game. \r\n"
                +"W or Up Arrow:         Rotate Tetrimino. \r\n"
                +"A or Left Arrow:         Move Tetrimino to the left. \r\n"
                +"D or Right Arrow:      Move Tetrimino to the right. \r\n"
                +"S or Down Arrow:      Move Tetrimino downward. \r\n"
                +"\r\n"
                +"To access this menu again, click the Help Button in the top menu of the main window,\r\n"
                +"then clck the Rules button.";
        }

        private void OKButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
