using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Surface.Presentation.Controls;
using System.Threading; // Threading
using System.Windows.Threading; // Threading
using System.Windows.Media; 

namespace SurfaceMemory
{

    /*
     * Class which contains functions to
     * control the gameflow
     * of the memorygame
     */
    class Game
    {
        private List<SurfaceMemoryCard> karten = new List<SurfaceMemoryCard>();
        private SurfaceMemoryCard lastButton = null;
        private Canvas plgrCanvas;
        private Random rnd;
        private Dispatcher d;
        private bool isAccessible = true;
        private int size = 6;
        private int count = 0;
        private int position = 1;
        private Label l = new Label();
        private DateTime StartZeit;
        private Grid DynamicGrid;
        private int wait = 500;

        /*
         * Constructor for the GameObject contains
         * the canvas and a dispatcher
         * it gets called with,
         * further of MemoryProperty 
         * the variable rnd and
         * a function to get the sizeproperty of the game,
         * at end the reloadfunction gets called
         */
        public Game(Canvas plgrCanvas, Dispatcher d)
        {
            this.plgrCanvas = plgrCanvas;
            this.rnd = MemoryProberty.rnd;
            this.d = d;
            this.size = MemoryProberty.getSize();
            reload();
        }


        /*
         * Function which creates a surfacebutton
         * on which later the memorycards are placed on
         */
        public Canvas createButton(SurfaceButton surfaceButton, int k, int j, RoutedEventHandler ev)
        {
            surfaceButton.Name = "SurfaceButtonMemory" + j + k;
            surfaceButton.Click += new RoutedEventHandler(ev);

            Canvas x = new Canvas();
            x.Children.Clear();
            //surfaceButton.Margin = MemoryProberty.getThicknes(25);
            surfaceButton.Width = MemoryProberty.getPGWidthSingle()-10;
            surfaceButton.Height = MemoryProberty.getPGHeightSingle()-10;
           
            //x.Background = Brushes.Yellow;
            //Canvas.SetTop(surfaceButton, 0);
            //Canvas.SetRight(surfaceButton, 0);

            x.Children.Add(surfaceButton);
          //  x.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            Grid.SetRow(x, j);
            Grid.SetColumn(x, k);
            return x;
        }

        /*
         * Function in which first it gets
         * checked if the game is inaccesible,
         * if that is the case the function
         * ends
         * else a new gridfield for the memorycards
         * gets created
         */
        public void reload()
        {
            if (!this.isAccessible) return;
            isAccessible = false;
            // Create the Grid
            DynamicGrid = new Grid();
            // DynamicGrid.ShowGridLines = true; // Linien Anzeigen
            // DynamicGrid.Background = new SolidColorBrush(Colors.LightSteelBlue);
            DynamicGrid.Width = MemoryProberty.getPGWidth();
            DynamicGrid.Height = MemoryProberty.getPGHeight();

            for (int i = 0; i < 8; i++)
            {
                DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
                if (i < 4)
                {
                    DynamicGrid.RowDefinitions.Add(new RowDefinition());
                }
            }

            // Create Memory Cards 
            List<MemoryImage> bilder = MemoryProberty.getPictures();

            List<SurfaceMemoryCard> karten = new List<SurfaceMemoryCard>();
            foreach (MemoryImage bild_karte in bilder)
            {
                for (int i = 0; i < 2; i++)
                {
                    karten.Add(new SurfaceMemoryCard("", bild_karte));
                }
            }
            int index = 0;
            switch (MemoryProberty.getSize())
            {
                case 1: // Klein
                    for (int k = 2; k < 6; k++)
                    {
                        for (int j = 1; j < 3; j++)
                        {
                            int rid = rnd.Next(0, karten.Count);
                            SurfaceMemoryCard surfaceButton = karten.ElementAt(rid);
                            karten.RemoveAt(rid);
                            DynamicGrid.Children.Add(createButton(surfaceButton, k, j, new RoutedEventHandler(surfaceButton_Click_Mem)));
                            index++;
                        }
                    }
                    break;
                case 3: // Groß
                    for (int k = 1; k < 7; k++)
                    {
                        for (int j = 1; j < 4; j++)
                        {
                            int rid = rnd.Next(0, karten.Count);
                            SurfaceMemoryCard surfaceButton = karten.ElementAt(rid);
                            karten.RemoveAt(rid);
                            DynamicGrid.Children.Add(createButton(surfaceButton, k, j, new RoutedEventHandler(surfaceButton_Click_Mem)));
                            index++;
                        }
                    }
                    break;
                default: // Medium
                    for (int k = 1; k < 7; k++)
                    {
                        for (int j = 1; j < 3; j++)
                        {
                            int rid = rnd.Next(0, karten.Count);
                            SurfaceMemoryCard surfaceButton = karten.ElementAt(rid);
                            karten.RemoveAt(rid);
                            DynamicGrid.Children.Add(createButton(surfaceButton, k, j, new RoutedEventHandler(surfaceButton_Click_Mem)));
                            index++;
                        }
                    }
                    break;
            }

            // Manuelle Buttons
            // DynamicGrid.Children.Add(createButton(0, 0, new RoutedEventHandler(surfaceButton_reload), "Reload"));
            // DynamicGrid.Children.Add(createButton(1, 0, new RoutedEventHandler(surfaceButton_reload), "Test"));

            // Add erverything to everything
            Canvas.SetTop(DynamicGrid, 0);
            Canvas.SetLeft(DynamicGrid, 0);
            plgrCanvas.Children.Add(DynamicGrid);
           // plgrCanvas.ToolTip = "Spielfeld";
            lastButton = null;
            StartZeit = DateTime.Now; // Zeit zählen
            l.HorizontalContentAlignment = HorizontalAlignment.Center;
            l.VerticalContentAlignment = VerticalAlignment.Center;
            l.Width = MemoryProberty.getPGWidthSingle() * .9;
            l.Height = MemoryProberty.getPGHeightSingle() * .9;
            //  x.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            Grid.SetRow(l, 0);
            Grid.SetColumn(l, 3);
            DynamicGrid.Children.Add(l);
            isAccessible = true;
        }

        /// <summary>
        ///  Klickevent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*
         * Function which loads the reload function when 
         * the menubutton for reloading gets pressed
         */
        void surfaceButton_reload(object sender, RoutedEventArgs e)
        {
            reload();
        }

        /// <summary>
        ///  Klickevent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        /*
         * Function for the clickevent of a surfacebutton
         * on which a memorycard is placed
         * it gets test if the game and the card is accesible
         * also if the card is already uncovered or if it
         * is the first of the two cards of the memorycards
         * a player may click or the last one
         * Should a player choose the wrong cards a waittime
         * gets added, which lets him wait until he is allowed
         * to pick a card again
         * 
         */
        void surfaceButton_Click_Mem(object sender, RoutedEventArgs e)
        {
            SurfaceMemoryCard target = (SurfaceMemoryCard)sender;
            if (target.isAccessible && this.isAccessible)
            {
                target.setAufgedeckt(!target.Aufgedeckt); // Karte aufdecken
                if (lastButton == null) // Erste Karte aufgedeckt
                {
                    lastButton = target;
                    target.isAccessible = false;
                }
                else // zweite Karte wird aufgedeckt
                {
                    this.isAccessible = false;
                    Thread t = new Thread(new ThreadStart(
                            delegate
                            {
                                d.Invoke(DispatcherPriority.Normal, new Action(() =>
                                {
                                    Canvas p1 = (Canvas)target.Parent;
                                    Canvas p2 = (Canvas)lastButton.Parent;
                                    if (target.hasSameCover(lastButton.Bild)) // Vergleich ob selbes Cover
                                    {
                                        target.isAccessible = false; // blockiert lassen (weil fertig)
                                        count++; // (gewinn Counter hochzählen)
                                        wait = 500;
                                        MemoryProberty.gamePosition();
                                        if (count == MemoryProberty.getSizeAnzahl())
                                        {
                                            //Spieler fertig
                                            gewonnen();
                                        }

                                        p1.Background = Brushes.Green;
                                        p2.Background = Brushes.Green;
                                    }
                                    else
                                    {
                                        /*if (MemoryProberty.getSize() > 1)
                                        {
                                            wait = wait + 500;
                                        }*/
                                        p1.Background = Brushes.Red;
                                        p2.Background = Brushes.Red;
                                    }
                                    //plgrCanvas.IsEnabled = true;
                                }));
                                System.Threading.Thread.Sleep(wait);
                                d.Invoke(DispatcherPriority.Normal, new Action(() =>
                                {
                                    Canvas p1 = (Canvas)target.Parent;
                                    Canvas p2 = (Canvas)lastButton.Parent;
                                    p1.Background = null;
                                    p2.Background = null;
                                    if (target.hasSameCover(lastButton.Bild)) // Vergleich ob selbes Cover
                                    {
                                        // nichts ?
                                    }
                                    else
                                    {
                                        target.isAccessible = true; // wieder klickbar
                                        lastButton.isAccessible = true; // wieder klickbar
                                        target.setAufgedeckt(!target.Aufgedeckt); // umdrehen
                                        lastButton.setAufgedeckt(!lastButton.Aufgedeckt); // umdrehen
                                    }
                                    lastButton = null; // Erste Karte kann wieder gesetzt werden
                                    this.isAccessible = true; // Sperre aufheben
                                    //plgrCanvas.IsEnabled = true;
                                }));
                            }
                            ));
                    t.IsBackground = true;
                    t.Start();
                }
            } // Else nothing
        }

        /// <summary>
        ///  Klickevent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*
         * Function in which depending on if a player
         * won or lost, shows time and a different victor
         * message
         */
        private void gewonnen()
        {
            DateTime EndZeit = DateTime.Now;
            TimeSpan GemessendeZeit = EndZeit - StartZeit;
            SurfaceButton subcanvas = new SurfaceButton();
            MemoryProberty.platz++; // Platz hochzählen
            subcanvas.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
            subcanvas.Content = "Zeit: "
                   + GemessendeZeit.Hours + ":" + +GemessendeZeit.Minutes + ":" + GemessendeZeit.Seconds + ":" + GemessendeZeit.Milliseconds + "";
            subcanvas.Width = MemoryProberty.getPGWidth() - 100;
            subcanvas.Height = MemoryProberty.getPGHeight() - 100;
            subcanvas.Margin = new Thickness(50, 50, 50, 50);
            //subcanvas.Padding = new Thickness((subcanvas.Width / 2) - 50, (subcanvas.Height / 2), 0, 0);
            
            // Farbe Subcanvas

            subcanvas.Background = MemoryProberty.getWin(MemoryProberty.platz);
            l.Opacity = 0;
            plgrCanvas.Children.Add(subcanvas);
        }

        /*
         * returns the number of the uncovered
         * memorycardpairs
         */
        public int getAnzahlAufgedeckteSteine()
        {
            return count;
        }

        /*
         * Function which creates
         * the message containing which
         * rank the momentary player has
         * depending on how many pairs he uncovered
         * and how many pairs other players have uncovered
         */
        public void setPosition(int i)
        {
            if (count != MemoryProberty.getSizeAnzahl())
            {
                this.position = i;
            }

            TextBlock tb = new TextBlock();
            tb.Text = "" + position;
            tb.Foreground = Brushes.Gray;
            tb.FontSize = 25;
            tb.Opacity = 0.80;
            tb.FontStretch = FontStretches.UltraExpanded;
            tb.FontWeight = FontWeights.UltraBold;
           // tb.LineHeight = Double.NaN;
            tb.Width = MemoryProberty.getPGWidthSingle() * .9;
            tb.Height = MemoryProberty.getPGHeightSingle() * .9;
            tb.Padding = new Thickness(0, tb.Height / 2,0, tb.Height / 12);
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.TextAlignment = TextAlignment.Center;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Typography.NumeralStyle = FontNumeralStyle.OldStyle;
            tb.Typography.SlashedZero = true;
            l.Content = tb;
            l.Background = MemoryProberty.getMWin(position);
            
        }
        // Ende
    }
}
