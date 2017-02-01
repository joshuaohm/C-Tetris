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
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            RulesMessage();
        }

        private void RulesMessage()
        {
            MessageLabel.Content = "Tetris v 1.0 \r\n"
                + "By Joshua Ohm, March 2015 \r\n"
                +"\r\n"
                +"This has been a wild ride, but I've learned a lot. \r\n"
                ;
        }

        private void OKButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
