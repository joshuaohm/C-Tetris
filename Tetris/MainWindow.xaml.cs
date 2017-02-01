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
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GameArray gameArray;
        private Tetrimino CurrentBlock;
        private Tetrimino NextBlock;
        private Tetrimino Next;
        private double TickRate = 750;
        private DispatcherTimer timer = null;
        private TetriminoFactory tetriminoFactory;
        public GameLogic logic;
        public Boolean paused = false;
        public Boolean gameStarted = false;
        

        public MainWindow()
        {

            InitializeComponent();

        }

        public Tetrimino GetCurrentBlock()
        {
            return this.CurrentBlock;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            descend();
        }

        private void StartTimer()
        {
            if (timer != null)
            {
                timer.Stop();
            }
            
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(TickRate);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!paused)
                {
                    newGame();
                }
            }

            //Check for the game being started
            else if (gameStarted)
            {
                if (e.Key == Key.P)
                {
                    pauseGame();
                }

                if (e.Key == Key.S || e.Key == Key.Down)
                {
                    //Move down if game isn't paused.
                    if (!paused)
                    {
                        if (this.CurrentBlock != null && CurrentBlock.CanMove)
                        {
                            descend();
                        }
                        else
                        {
                            NewBlock();
                        }
                    }
                }

                if (e.Key == Key.A || e.Key == Key.Left)
                {
                    //Move left if game isn't paused.
                    if (!paused)
                    {
                        if (this.CurrentBlock != null && CurrentBlock.CanMove)
                        {

                            CurrentBlock.Move(CurrentBlock.XCoord - 20, CurrentBlock.YCoord, CurrentBlock.Orientation);
                        }
                        else
                        {
                            NewBlock();
                        }
                    }
                }

                if (e.Key == Key.D || e.Key == Key.Right)
                {
                    //Move right if game isn't paused.
                    if (!paused)
                    {
                        if (this.CurrentBlock != null && CurrentBlock.CanMove)
                        {

                            CurrentBlock.Move(CurrentBlock.XCoord + 20, CurrentBlock.YCoord, CurrentBlock.Orientation);
                        }
                        else
                        {
                            NewBlock();
                        }
                    }
                }

                if (e.Key == Key.W || e.Key == Key.Up)
                {
                    //Rotate the current block if game isn't paused.
                    if (!paused)
                    {
                        if (this.CurrentBlock != null && CurrentBlock.CanMove)
                        {
                            if (CurrentBlock.Orientation < 3)
                            {
                                CurrentBlock.Move(CurrentBlock.XCoord, CurrentBlock.YCoord, CurrentBlock.Orientation + 1);
                            }
                            else
                            {
                                CurrentBlock.Move(CurrentBlock.XCoord, CurrentBlock.YCoord, 0);
                            }
                        }
                        else
                        {
                            NewBlock();
                        }
                    }
                }
            }
        }

        private void descend()
        {
            //This is the main function that forces Tetriminos to descend.   
            if (this.CurrentBlock != null && this.CurrentBlock.CanMove)
            {
                this.CurrentBlock.Move(this.CurrentBlock.XCoord, this.CurrentBlock.YCoord + 20, this.CurrentBlock.Orientation);
            }
            else
            {
                NewBlock();
            }
        }

        private void NewBlock()
        {
            if (!this.gameArray.checkForGameLoss())
            {
                CurrentBlock = NextBlock;
                NextBlock = tetriminoFactory.getNewBlock();
                drawNext();
            }
            else
            {
                //Player has lost the game!
                endTheGame();
            }
        }

        private void drawNext()
        {
            //These are necessary for drawing the next block in the side window.

            if (NextBlock.Type == "L")
            {
                Next = new LBlock(nextBlockCanvas, gameArray);
                Next.Move(40, 80, 0);
            }
            else if (NextBlock.Type == "J")
            {
                Next = new JBlock(nextBlockCanvas, gameArray);
                Next.Move(40, 80, 0);
            }
            else if (NextBlock.Type == "T")
            {
                Next = new TBlock(nextBlockCanvas, gameArray);
                Next.Move(40, 80, 0);
            }
            else if (NextBlock.Type == "S")
            {
                Next = new SBlock(nextBlockCanvas, gameArray);
                Next.Move(40, 80, 0);
            }
            else if (NextBlock.Type == "Z")
            {
                Next = new ZBlock(nextBlockCanvas, gameArray);
                Next.Move(40, 80, 0);
            }
            else if (NextBlock.Type == "Square")
            {
                Next = new SquareBlock(nextBlockCanvas, gameArray);
                Next.Move(30, 70, 0);
            }
            else if (NextBlock.Type == "Line")
            {
                Next = new LineBlock(nextBlockCanvas, gameArray);
                Next.Move(40, 80, 0);
            }
        }

        private void endTheGame()
        {
            timer.Stop();
            CurrentBlock.CanMove = false;
            gameStarted = false;
            nextBlockCanvas.Children.Clear();
            MessageBox.Show("You have lost the game! hit Enter to start a new one.");
        }

        private void levelBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Every time the level increases (besides the start of the game)
            //the tick rate gets faster.
            if (this.logic.getLevel() > 1)
            {
                TickRate -= (TickRate*.25);
                timer.Interval = TimeSpan.FromMilliseconds(TickRate);
            }
        }

        public void pauseGame()
        {
            if (!paused)
            {
                this.paused = true;
                this.timer.Stop();
                pauseLabel.Background = Brushes.Gray;
                pauseLabel.Content = "PAUSED";
            }
            else
            {
                this.paused = false;
                this.timer.Start();
                pauseLabel.Background = null;
                pauseLabel.Content = "";
            }
        }

        public void newGame()
        {
            //Resets all necessary variables, then starts a new game.

            gameCanvas.Children.Clear();
            nextBlockCanvas.Children.Clear();
            
            this.logic = new GameLogic(scoreBox, levelBox, rowsBox);
            this.logic.initializeBoxes();
            gameArray = new GameArray(gameCanvas, this.logic);
            tetriminoFactory = new TetriminoFactory(gameCanvas, gameArray);

            gameStarted = true;

            CurrentBlock = tetriminoFactory.getNewBlock();
            NextBlock = tetriminoFactory.getNewBlock();
            drawNext();

            CurrentBlock.Move(CurrentBlock.XCoord, CurrentBlock.YCoord, CurrentBlock.Orientation);
            StartTimer();

        }


        private void ControlsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ControlsWindow controlsWindow = new ControlsWindow();
            controlsWindow.Show();
        }

        private void AboutMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        private void NewGameMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            newGame();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ControlsWindow controlsWindow = new ControlsWindow();
            controlsWindow.Show();
        }

        private void ExitMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
