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
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>

    /*
     * The main class which
     * loads all objects and
     * creates the surfacewindow
     */
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        private Canvas mainCanvas = new Canvas(); // Wrapper
        Canvas menu;
        GameMenu m;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// 
        /*
         * Constructor for the surfacewindow
         * in which first the game with
         * size parameters, background
         * and a gamemenu gets created
         * 
         * 
         */
        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            // generate Window
            mainCanvas.Width = MemoryProberty.getGesWidth();
            mainCanvas.Height = MemoryProberty.getGesWidth();
            mainCanvas.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/background.jpg")));
            m = new GameMenu(mainCanvas, this.Dispatcher);
            genMenu();
            m.selectGame();
            //  startGame(mainCanvas); // 4 Spielfelder hinzufÅEen
            // root Window (See XAML in SurfaceWindow1.xaml)
            RootWindow.Content = mainCanvas;
            RootWindow.Title = "Memory Game!";
            RootWindow.Show();
            MemoryProberty.checkStart();
        }

        #region menu

        /*
         * Function to create
         * the menu of the game
         * and sets it position
         */
        public void genMenu()
        {
            menu = new Canvas();
            menu.Width = MemoryProberty.getPGWidth();
            menu.Height = MemoryProberty.getPGHeightSingle();
            Canvas.SetRight(menu, (MemoryProberty.getGesWidth() / 2) - (menu.Width / 2));
            Canvas.SetTop(menu, (MemoryProberty.getGesHeight() / 2) - (menu.Height / 2));

            Label l = new Label();
            l.HorizontalContentAlignment = HorizontalAlignment.Center;
            l.VerticalContentAlignment = VerticalAlignment.Center;
            l.Width = MemoryProberty.getPGWidthSingle() * 3;
            l.Height = MemoryProberty.getPGHeightSingle() * 0.5;
            TextBlock tb = new TextBlock();
            tb.Text = "Touch Memory (c) 2015\r\n Chris , Antonia, Sumi, Steffen, Julian, Notker ";
            tb.Foreground = Brushes.Black;
            tb.FontSize = 16;
            // tb.LineHeight = Double.NaN;
            tb.Width = l.Width;
            tb.Height = l.Height;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.TextAlignment = TextAlignment.Center;
            tb.TextWrapping = TextWrapping.Wrap;
            l.Content = tb;
            Canvas.SetTop(l, MemoryProberty.getGesHeight() - l.Height);
            Canvas.SetLeft(l, MemoryProberty.getGesWidth() - l.Width);// mainCanvas.Width - l.Width);
            mainCanvas.Children.Add(l);

            SurfaceButton sb = new SurfaceButton();
            sb.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/reload.png")));
            // sb.Content = "R";
            //surfaceButton.Margin = MemoryProberty.getThicknes(25);
            sb.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            sb.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            sb.Name = "SurfaceButtonMemory";
            sb.Click += new RoutedEventHandler(reload_Click);
            //surfaceButton.Margin = MemoryProberty.getThicknes(25);
            sb.Width = MemoryProberty.getPGWidthSingle() / 2 - 10;
            sb.Height = MemoryProberty.getPGHeightSingle() / 2 - 10;
            Canvas.SetTop(sb, 0);
            Canvas.SetLeft(sb, MemoryProberty.getPGWidthSingle());
            menu.Children.Add(sb);

          /*  SurfaceButton sbc = new SurfaceButton();
            sbc.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/delete.png")));
            //surfaceButton.Margin = MemoryProberty.getThicknes(25);
            sbc.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            sbc.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            sbc.Name = "SurfaceButtonMemoryClose";
            sbc.Click += new RoutedEventHandler(preclose_Click);
            //surfaceButton.Margin = MemoryProberty.getThicknes(25);
            sbc.Width = MemoryProberty.getPGWidthSingle() / 2 - 10;
            sbc.Height = MemoryProberty.getPGHeightSingle() / 2 - 10;
            Canvas.SetTop(sbc, 0);
            Canvas.SetRight(sbc, MemoryProberty.getPGWidthSingle());
            menu.Children.Add(sbc);*/

           /* Label l = new Label();
            l.HorizontalContentAlignment = HorizontalAlignment.Center;
            l.VerticalContentAlignment = VerticalAlignment.Center;
            l.Width = MemoryProberty.getPGWidthSingle() * 4;
            l.Height = MemoryProberty.getPGHeightSingle() * 1;
            TextBlock tb = new TextBlock();
            tb.Text = "Schwierigkeitsgrad ausw‰hlen\r\nund Spiel starten";
            tb.Foreground = Brushes.Gray;
            tb.FontSize = 25;
            tb.FontStretch = FontStretches.UltraExpanded;
            tb.FontWeight = FontWeights.UltraBold;
            // tb.LineHeight = Double.NaN;
            tb.Width = l.Width;
            tb.Height = l.Height;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.TextAlignment = TextAlignment.Center;
            tb.TextWrapping = TextWrapping.Wrap;
            l.Content = tb;
            Canvas.SetTop(l, 0);
            Canvas.SetLeft(l, menu.Width / 2 - (l.Width / 2));
            menu.Children.Add(l); */

            // Menu
            mainCanvas.Children.Add(menu);
        }

        /*
         * Function for the exit button
         * creates a message for the player
         * in a window and buttons
         * to confirm if he really wants to close the game
         */
        private void preclose_Click(object sender, RoutedEventArgs e)
        {
            menu.Children.Clear();
            menu.Background = Brushes.White;
            menu.Opacity = .75;
            menu.Width = MemoryProberty.getPGWidth();
            menu.Height = MemoryProberty.getPGHeightSingle()*2;
            Canvas.SetZIndex(menu, 5);
            Canvas.SetRight(menu, (MemoryProberty.getGesWidth() / 2) - (menu.Width / 2));
            Canvas.SetTop(menu, (MemoryProberty.getGesHeight() / 2) - (menu.Height / 2));

            Label l = new Label();
            l.HorizontalContentAlignment = HorizontalAlignment.Center;
            l.VerticalContentAlignment = VerticalAlignment.Center;
            l.Width = MemoryProberty.getPGWidthSingle() * 3;
            l.Height = MemoryProberty.getPGHeightSingle() * 3;
            TextBlock tb = new TextBlock();
            tb.Text = "Spiel wirklich beenden?";
            tb.Foreground = Brushes.Gray;
            tb.FontSize = 25;
            tb.FontStretch = FontStretches.UltraExpanded;
            tb.FontWeight = FontWeights.UltraBold;
            // tb.LineHeight = Double.NaN;
            tb.Width = l.Width;
            tb.Height = l.Height;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.TextAlignment = TextAlignment.Center;
            tb.TextWrapping = TextWrapping.Wrap;
            l.Content = tb;
            Canvas.SetTop(l, MemoryProberty.getPGHeightSingle() /1.5);
            Canvas.SetLeft(l, menu.Width / 2 - (l.Width / 2));
            menu.Children.Add(l);

            SurfaceButton sb = new SurfaceButton();
            sb.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/delete.png")));
            // sb.Content = "R";
            //surfaceButton.Margin = MemoryProberty.getThicknes(25);
            sb.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            sb.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            sb.Name = "SurfaceButtonMemory";
            sb.Click += new RoutedEventHandler(menu_Click);
            //surfaceButton.Margin = MemoryProberty.getThicknes(25);
            sb.Width = MemoryProberty.getPGWidthSingle() / 2 - 10;
            sb.Height = MemoryProberty.getPGHeightSingle() / 2 - 10;
            Canvas.SetTop(sb, MemoryProberty.getPGHeightSingle() / 1.5);
            Canvas.SetLeft(sb, MemoryProberty.getPGWidthSingle() * 1.5);
            menu.Children.Add(sb);

            SurfaceButton sbc = new SurfaceButton();
            sbc.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/check.png")));
            //surfaceButton.Margin = MemoryProberty.getThicknes(25);
            sbc.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            sbc.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            sbc.Name = "SurfaceButtonMemoryClose";
            sbc.Click += new RoutedEventHandler( close_Click);
            //surfaceButton.Margin = MemoryProberty.getThicknes(25);
            sbc.Width = MemoryProberty.getPGWidthSingle() / 2 - 10;
            sbc.Height = MemoryProberty.getPGHeightSingle() / 2 - 10;
            Canvas.SetTop(sbc, MemoryProberty.getPGHeightSingle() / 1.5);
            Canvas.SetRight(sbc, MemoryProberty.getPGWidthSingle() * 1.5);
            menu.Children.Add(sbc);
        }

        /*
         * Function for clickevent 
         * when the user aborts the closing of game 
         */
        private void menu_Click(object sender, RoutedEventArgs e)
        {
            mainCanvas.Children.Remove(menu);
            genMenu(); // setzt das Menu
        }

        /*
         * Function for clickevent when
         * the user confirms
         * the closing of game
         */
        private void close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        /*
         * function for the clickfunction
         * of the reloadbutton
         * old threads and gamescreen get removed
         * from window and the gamemenu gets created
         */
        private void reload_Click(object sender, RoutedEventArgs e)
        {
            mainCanvas.Children.Clear(); // Leert das MainCanvas
            MemoryProberty.gameClear();
            genMenu(); // setzt das Menu
            m = new GameMenu(mainCanvas, this.Dispatcher); // altes Spiel lˆschen
            m.selectGame(); // Startet ein neues Spiel
            MemoryProberty.reset();
        }

        #endregion

        #region standardEvents
        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }
        #endregion
    }
}

/**
 *  ToDo:
 *  - Hilfe Menu einfÅEen
 *  - Gestaltung: Start Screen gestalten (Anleitung / willkommen) (Credits, Kachelview, schwarzer Hintergrund)
 *  
 * Optional:
 *  - Timer mit Spielerzeit (Schwierig)
 *  - Quellcode Kommentare
 * 
 * 
 * History:
 * - BugFix: Reload Button (9.1)
 * - aktuell besten Spieler anzeigen (14.1)
 * 
 * 
 * WindowState="Maximized"
        WindowStyle="None"
*/