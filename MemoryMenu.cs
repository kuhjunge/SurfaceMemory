using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.IO;
using System.Windows.Threading; // Threading
using System.Threading;

namespace SurfaceMemory
{

    /*
     * Class which describes how the memorygamefield is built
     * and contains functions to control the start of the game
     */
    class GameMenu
    {
        public static readonly RoutedCommand MenuItemCommand = new RoutedCommand();
        private Canvas mainCanvas;
        private List<Canvas> plgrCanvasstart = new List<Canvas>(); // 4 Spielfelder
        private List<SurfaceButton> updateButton = new List<SurfaceButton>(); // 4 Spielfelder
        private Dispatcher d;
        private Thread t = null;
        private int angle = 0;
        private ElementMenu testEMenu;

        /*
         * Constructor for the GameMenu contains
         * the canvas and a dispatcher
         * it gets called with
         */
        public GameMenu(Canvas mc, Dispatcher dp)
        {
            mainCanvas = mc;
            d = dp;
        }

        #region selecter
        /*
         * Function in which a ÉlementMenu
         * for the choosing of the difficulty
         * gets created and adjusted
         */
        public void selectGame()
        {
            testEMenu = new ElementMenu();
            testEMenu.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/menu.png")));
            testEMenu.Width = MemoryProberty.getPGWidthSingle()*2;
            testEMenu.Height = MemoryProberty.getPGHeightSingle()*2;
            ElementMenuItem testEItem1 = new ElementMenuItem();
            ElementMenuItem testEItem2 = new ElementMenuItem();
            ElementMenuItem testEItem3 = new ElementMenuItem();

            // adjust ElementMenu
            //testEMenu.Height = MemoryProberty.getPGHeight();
            //testEMenu.Width = MemoryProberty.getGesWidth()/ 2;
            testEMenu.ActivationMode = ElementMenuActivationMode.AlwaysActive;

            // Sets position of ElementMenu inside Canvas
            Canvas.SetTop(testEMenu, (MemoryProberty.getGesHeight() / 2) - testEMenu.Height /2 );
            Canvas.SetRight(testEMenu, (MemoryProberty.getGesWidth() / 2) - (testEMenu.Width / 2));

            // adjust ElemenmenuItems
            testEItem1.Height = MemoryProberty.getPGHeightSingle() * .8;
            testEItem1.Width = MemoryProberty.getPGWidthSingle() * .8;
            testEItem1.Header = "Einfach";
            testEItem1.Name = "ElementMenuItem1";
            testEItem1.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/low.png")));

            testEItem2.Height = MemoryProberty.getPGHeightSingle() * .8;
            testEItem2.Width = MemoryProberty.getPGWidthSingle() * .8;
            testEItem2.Header = "Mittel";
            testEItem2.Name = "ElementMenuItem2";
            testEItem2.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/medium.png")));

            testEItem3.Height = MemoryProberty.getPGHeightSingle() * .8;
            testEItem3.Width = MemoryProberty.getPGWidthSingle() * .8;
            testEItem3.Header = "Schwer";
            testEItem3.Name = "ElementMenuItem3";
            testEItem3.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/high.png")));


            // set a Clickevent for the ElementMenuItems
            testEItem1.Click += ElementMenuItem1_Click;
            testEItem2.Click += ElementMenuItem2_Click;
            testEItem3.Click += ElementMenuItem3_Click;

            // add ElementMenuItems to ElementMenu
            testEMenu.Items.Add(testEItem2);
            testEMenu.Items.Add(testEItem1);
            testEMenu.Items.Add(testEItem3);

            // add elements to canvas
            mainCanvas.Children.Add(testEMenu);

        }


        /*
         * Function for the clickEvent of ElementMenuItem1
         * it sets the size of the memorycardfield to small size
         * and after that removes the ElementMenu from the gamefield and
         * calls a function for the gamestart
         */
        private void ElementMenuItem1_Click(object sender, RoutedEventArgs e)
        {
            MemoryProberty.setSize(1);
            mainCanvas.Children.Remove(testEMenu);
            startGame();
        }


        /*
         * Function for the clickEvent of ElementMenuItem2
         * it sets the size of the memorycardfield to middle size
         * and after that removes the ElementMenu from the gamefield and
         * calls a function for the gamestart
         */
        private void ElementMenuItem2_Click(object sender, RoutedEventArgs e)
        {
            MemoryProberty.setSize(2);
            mainCanvas.Children.Remove(testEMenu);
            startGame();
           // menu();
        }

        /*
         * Function for the clickEvent of ElementMenuItem3
         * it sets the size of the memorycardfield to large field
         * and after that removes the ElementMenu from the gamefield and
         * calls a function for the gamestart
         */
        private void ElementMenuItem3_Click(object sender, RoutedEventArgs e)
        {
            MemoryProberty.setSize(3);
            mainCanvas.Children.Remove(testEMenu);
            startGame();
        //    menu();
        }
        #endregion

        #region start
        /*
         * Function which decides how the memorygamefields
         * are positioned at the start of the game
         * a memorygamefield for each player gets created
         */
        public void startGame()
        {
            List<Canvas> plgrCanvas = new List<Canvas>(); // 4 Spielfelder
            for (int i = 0; i < 4; i++)
            {
                plgrCanvas.Add(new Canvas());
            }
            // Create a canvas playground for every Player
            mainCanvas.Children.Add(createPlayground(plgrCanvas.ElementAt(0), 90, (MemoryProberty.getPGHeightSingle() / 2), MemoryProberty.getPGHeight())); // left
            mainCanvas.Children.Add(createPlayground(plgrCanvas.ElementAt(1), 180, MemoryProberty.getPGHeight(), MemoryProberty.getPGHeight() + MemoryProberty.getPGWidth())); // top
            mainCanvas.Children.Add(createPlayground(plgrCanvas.ElementAt(2), 270, (MemoryProberty.getPGHeightSingle() / 2) + MemoryProberty.getPGWidth(), MemoryProberty.getPGHeight() + MemoryProberty.getPGWidth())); // right
            mainCanvas.Children.Add(createPlayground(plgrCanvas.ElementAt(3), 0, MemoryProberty.getPGHeight() + MemoryProberty.getPGHeightSingle(), MemoryProberty.getPGHeight())); // bottom

            /* foreach (Canvas c in plgrCanvas)
             {
                 Game game = new Game(c, Dispatcher);
             }*/
        }

        /// <summary>
        /// Create the Playground for one Player
        /// </summary>
        /// <param name="angle">the angle</param>
        /*
         * Function which decides
         * how each single memorygamefield
         * is built and returns the adjusted memorygamefield at end
         */
        private Canvas createPlayground(Canvas plgrCanvas, int angle, int top, int left)
        {
            this.angle = angle;
            plgrCanvas.Width = MemoryProberty.getPGWidth();
            plgrCanvas.Height = MemoryProberty.getPGHeight();

            Canvas.SetTop(plgrCanvas, top);
            Canvas.SetLeft(plgrCanvas, left);
            plgrCanvas.RenderTransform = new RotateTransform(angle); // Drehung
            // Sub Canvas
            SurfaceButton subcanvas = new SurfaceButton();
            subcanvas.Width = MemoryProberty.getPGWidth() - 100;
            subcanvas.Height = MemoryProberty.getPGHeight() - 100;
            subcanvas.Margin = new Thickness(50, 50, 50, 50);
           // subcanvas.Padding = new Thickness((subcanvas.Width / 2) - 50, (subcanvas.Height / 2), 0, 0);
            // Farbe Subcanvas
            subcanvas.Content = getTB(subcanvas.Width, subcanvas.Height, false,"");
            subcanvas.Background = Brushes.White;
            subcanvas.Opacity = .75;
            subcanvas.Click += startgame_Click;

            plgrCanvas.Children.Add(subcanvas);
            updateButton.Add(subcanvas);
            // Return
            return plgrCanvas;
        }

        /*
         * Function for the ClickEvent of the
         * Startbutton for each player
         * the player who clicks on his startbutton
         * gets added to the game
         * and a thread gets created to read his actions on
         * his own field
         */
        private void startgame_Click(object sender, RoutedEventArgs e)
        {
            SurfaceButton target = (SurfaceButton)sender;
            target.Content = getTB(target.Width, target.Height, true,"");
            Canvas temp = (Canvas)target.Parent;
            target.Background = Brushes.Green;
            Boolean fertig = false;
            foreach (Canvas plgr in plgrCanvasstart)
            {
                if (plgr == temp)
                {
                    fertig = true;
                }
            }
            if (!fertig)
            {
                plgrCanvasstart.Add(temp);
                if (plgrCanvasstart.Count == 2)
                {
                    
                    t = new Thread(new ThreadStart(
                   delegate
                   {
                       System.Threading.Thread.Sleep(1000);
                       d.Invoke(DispatcherPriority.Normal, new Action(() =>
                       {
                           // Code
                           foreach (SurfaceButton b in updateButton)
                           {
                               b.Content = getTB(b.Width, b.Height, true," (noch 3 Sekunden!)");
                            //   showToolTip(b);
                           }
                       }));
                       System.Threading.Thread.Sleep(1000);
                       d.Invoke(DispatcherPriority.Normal, new Action(() =>
                       {
                           // Code
                           foreach (SurfaceButton b in updateButton)
                           {
                               b.Content = getTB(b.Width, b.Height, true, " (noch 2 Sekunden!)");
                           }
                       }));
                       System.Threading.Thread.Sleep(1000);
                       d.Invoke(DispatcherPriority.Normal, new Action(() =>
                       {
                           // Code
                           foreach (SurfaceButton b in updateButton)
                           {
                               b.Content = getTB(b.Width, b.Height, true, " (noch 1 Sekunde!)");
                           }
                       }));
                       System.Threading.Thread.Sleep(1000);
                       d.Invoke(DispatcherPriority.Normal, new Action(() =>
                       {
                           // -Alle 4 Felder löschen
                           foreach (SurfaceButton b in updateButton)
                           {
                               Canvas del = (Canvas)b.Parent;
                               del.Children.Clear();
                           }
                           // Spielfelder initialisieren
                           foreach (Canvas plgr in plgrCanvasstart)
                           {
                               Game game = new Game(plgr, d);
                               MemoryProberty.gameRegistrer(game);
                           }
                       }));
                   }
                   ));
                    t.IsBackground = true;
                    t.Start();
                }
            }
        }

        /*
        private void showToolTip(SurfaceButton target)
        {
            if (target.Background == Brushes.White)
            {
                //Validation failed, so set an appropriate error message
                ToolTip tt = new ToolTip();
                Canvas x = (Canvas)target.Parent;

                tt.Content = "Bitte klicken sie auf das Element!";
                tt.PlacementTarget = target;
                tt.Placement = System.Windows.Controls.Primitives.PlacementMode.Center;
                tt.IsOpen = true;
                tt.StaysOpen = false;

               // x.Children.Add(tt);
                target.ToolTip = tt;
                tt.RenderTransform = new RotateTransform(angle); // Drehung
            }
            else
            {
                //Clear previous error message
                ToolTip tt = (ToolTip)target.ToolTip;
                if (tt != null)
                {
                    tt.IsOpen = false;
                }
            }
        }
         * */

        /*
         * Function in which a textblock gets created
         * it is used to contains the starting message
         * on each memorygamefield which is shown
         * when a difficulty has been chosen
         * but the player hasnt joined yet the game
         * or short after the player joined
         */
        private TextBlock getTB(double w, double h, bool gst, string txt)
        {
            TextBlock tb = new TextBlock();
            tb.Text = MemoryProberty.getStartText(gst) +txt;
            tb.Foreground = Brushes.Gray;
            tb.FontSize = 25;
            tb.FontStretch = FontStretches.UltraExpanded;
            tb.FontWeight = FontWeights.UltraBold;
            // tb.LineHeight = Double.NaN;
            tb.Width =w;
            tb.Height = h;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.TextAlignment = TextAlignment.Center;
            tb.TextWrapping = TextWrapping.Wrap;
            return tb;
        }

        #endregion
    }
}
