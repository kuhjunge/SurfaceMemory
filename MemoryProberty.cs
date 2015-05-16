using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Diagnostics;
using System.IO;

namespace SurfaceMemory
{

    /*
     * Class which defines how the
     * memorycards in the game look
     * and has methods to load
     * the pictures and change the
     * look of them
     */
    static class MemoryProberty
    {
        private static List<MemoryImage> images = null;
        private static List<Game> games = new List<Game>();
        private static MemoryImage backgroundImage;
        private static int width = 1920;
        private static int height = 1080;
        private static int widthformat = 16;
        private static int heightformat = 9;
        private static int playgroundSizeWidth = 8;
        private static int playgroundSizeHeight = 4;
        private static String actDir = Environment.CurrentDirectory + "\\Images";
        private static int gameSize = 2;
        public static Random rnd = new Random(); // Random Number
        public static int platz = 0;
      //  private static int gridSizeWidth = 8;
      //  private static int gridSizeHeight = 4;

        /*
         * Function which resetz the game
         * for that it sets back the variables
         * regulating size and playerposition
         * and sets a new list containing gameobjects
         */
        public static void reset()
        {
            platz = 0;
            gameSize = 2;
            games = new List<Game>();
        }

        /*
         * Function which tests at the beginning if there
         * exists a subdirectory for the memorycard images
         * if it exists, pictures get loaded from it 
         * else a error message gets created
         */
        public static void checkStart()
        {
            if (Directory.Exists(actDir))
            {
                loadPictures();
            }
            else
            {
                DirectoryInfo info = Directory.CreateDirectory(actDir);
                System.Windows.MessageBox.Show("Dieses Programm benötigt mindestens 10 Bilder (*.jpg, *.png) im Images Ordner um zu starten!\r\n Bitte genügend Bilder ins Verzeichnis einfügen und erneut starten! (F01)");
                if (Directory.Exists(actDir))
                    Process.Start("explorer.exe", actDir);
                System.Windows.Application.Current.Shutdown();
            }
        }


        /* 
         * Function in which the memoryimages from
         * the given directorypath get loaded
         * and added to list of memoryimages,
         * the first card in the directory will be
         * loaded as the backgroundimage
         * if the images which are in the directory
         * have a count of less than ten, another
         * error message gets displayed
         */
        public static void loadPictures()
        {
            Console.WriteLine("ausgeführt");
            backgroundImage = null;
            images = new List<MemoryImage>();
            String[] imageExtensions = { ".jpg", ".png" };
            DirectoryInfo dir = new System.IO.DirectoryInfo(actDir);
         /*   DirectoryInfo[] kartensammlungen = dir.GetDirectories();

            foreach (DirectoryInfo ks in kartensammlungen)
            {*/

                // Console.WriteLine("### Kartensammlung: " + ks.Name);
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo karte in files)
                {
                    if (Array.IndexOf(imageExtensions, karte.Extension.ToLower()) >= 0)
                    {
                        if (backgroundImage == null) // Erstes eingelesenes Bild wird Hintergrundbild
                        {
                            backgroundImage = new MemoryImage(karte.FullName);
                        }
                        else // rest einlesen
                        {
                            images.Add(new MemoryImage(karte.FullName));
                        }
                    }
                }
          //  }
            if (images.Count < 10)
            {
                System.Windows.MessageBox.Show("Dieses Programm benötigt mindestens 10 Bilder (*.jpg, *.png) im Images Ordner um zu starten!\r\n Bitte genügend Bilder ins Verzeichnis einfügen und erneut starten! (F02)");
                if (Directory.Exists(actDir))
                    Process.Start("explorer.exe", actDir);
                System.Windows.Application.Current.Shutdown();
            }
            //Console.ReadKey();
          //  return new List<MemoryImage>(images); // Clone ausgeben und nicht Referenz
        }

        /*
         * Class which creates a list of memoryimages
         * if the list is empty, pictures get loaded
         * else a new list of memoryimages gets created
         * and the pictures get in a random order
         * added to it
         * returns at end the new list of memoryimages
         */ 
        public static List<MemoryImage> getPictures() 
        {
            if (images == null)
            {
                loadPictures();
            }
            List<MemoryImage>image = new List<MemoryImage>();
           List<MemoryImage> pool = new List<MemoryImage>(images); // Kopie Gesamtbilder

           for (int i = 0; i < getSizeAnzahl(); i++)
           {
               int rid = rnd.Next(0, pool.Count);
               image.Add(pool.ElementAt(rid));
               pool.RemoveAt(rid);
           }
            return image; // Clone ausgeben und nicht Referenz
        }

        /*
         * Function in which the thickness of the border of the
         * memorygame gets adjusted
         * and returns at end this thicknessobject
         */
        public static System.Windows.Thickness getThicknes(int thicknes)
        {
            System.Windows.Thickness myThickness = new System.Windows.Thickness();
            myThickness.Bottom = thicknes;
            myThickness.Left = thicknes;
            myThickness.Right = thicknes;
            myThickness.Top = thicknes;
            return myThickness;
        }

        /*
         * Function in which a choicevariable for the
         * chosen memorygamefieldsize
         * gets set with the variable it gets called
         * as input
         */
        public static void setSize(int i)
        {
            if (i > 0 && i < 4)
            {
                gameSize = i;
            }
        }

        /* Function which 
         * Returns the choicevariable for
         * the chosen size of the 
         * memorygamefield
         */
        public static int getSize(){
            return gameSize;
        }

        /*
         * Function in which depending
         * on the choicevariable for
         * the memorygamefieldsize
         * a different fieldsize
         * gets created
         * 
         */
        public static int getSizeAnzahl()
        {
            if (gameSize == 1)
            {
                return 4;
            }
            else  if (gameSize == 3)
            {
                return 9;
            }
            else 
            {
                return 6;
            }
        }

        /*
         * Function which returns
         * the backgroundimage of the memorycards
         */
        public static MemoryImage getMemoryBackImage()
        {
            return backgroundImage;
        }


        // Getter & Setter

        /*
         * Function which returns the 
         * total width of the memorygame
         */
        public static int getGesWidth()
        {
             return width; 
        }

        /*
         * Function which returns the
         * total height of the memorygame
         */
        public static int getGesHeight()
        {
            return height;
        }

        /*
         * Function which returns
         * the width of the memorygamefield
         */
        public static int getPGWidth()
        {
            return getPGWidthSingle() * playgroundSizeWidth;
        }

        /*
         * Function which returns
         * the height of the memorygamefield
         */
        public static int getPGHeight()
        {
            return getPGHeightSingle() * playgroundSizeHeight;
        }

        /*
         * Function which returns
         * the width of the memorygamefield
         * of a single player
         */
        public static int getPGWidthSingle()
        {
            return (width / widthformat);
        }

        /*
        * Function which returns
        * the height of the memorygamefield
        * of a single player
        */
        public static int getPGHeightSingle()
        {
            return (height / heightformat);
        }

        /*
         * Function in which
         * depending on Integer with which it
         * gets called, a different Imagebrushobject 
         * which represents a win or loss message for the player
         * is returned
         */
        public static ImageBrush getWin(int i)
        {
            switch (i)
            {
                case 1:
                    return new ImageBrush(new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Gold.png")));
              
                case 2:
                    return new ImageBrush(new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Silber.png")));
                 
                case 3:
                    return new ImageBrush(new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Bronze.png")));
                  
                default:
                    return new ImageBrush(new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Verloren.png")));
                    
            }
        }

        /*
         * Function in which
         * depending on Integer with which it
         * gets called, a different Imagebrushobject 
         * which represents a medal depending on if the player won
         * or lost, gets returned at end
         */
        public static ImageBrush getMWin(int i)
        {
            switch (i)
            {
                case 1:
                    return new ImageBrush(new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/mgold.png")));

                case 2:
                    return new ImageBrush(new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/msilver.png")));

                case 3:
                    return new ImageBrush(new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/mbronze.png")));

                default:
                    return new ImageBrush();

            }
        }

        /*
         * function in which the gameobject
         * which was gotten as a inputparameter
         * gets added to a list of gameobjects
         * registering with that a new player
         */
        public static void gameRegistrer(Game g)
        {
            games.Add(g);
        }

        /*
         * When the game has ended
         * this function creates
         * a new list for Gameobjects
         * 
         */
        public static void gameClear()
        {
            games = new List<Game>();
        }

        /*
         * function which decides
         * which player has momentary which
         * position of the ranglist
         * depending on how many 
         * memorycardpairs have already
         * been uncovered
         */
        public static void gamePosition()
        {
            List<Game> gamelist = new List<Game>(games);
            int platz = 1;
            int raus = 0;
            while (gamelist.Count > 0)
            {
                Game max = gamelist.ElementAt(0);
                foreach (Game c in gamelist)
                {
                    if (c.getAnzahlAufgedeckteSteine() > max.getAnzahlAufgedeckteSteine())
                    {
                        max = c;
                    }
                }
                List<Game> gamesublist = new List<Game>(gamelist);
                for (int i = 0; i < gamelist.Count; i++ )
                {
                    Game c = gamelist.ElementAt(i);
                    if (c.getAnzahlAufgedeckteSteine() == max.getAnzahlAufgedeckteSteine())
                    {
                        gamesublist.Remove(c);
                        c.setPosition(platz);
                        raus++;
                    }
                }
                gamelist = gamesublist;
                platz += raus;
                raus = 0;
            }
        }

        /*
         * function in which 
         * a boolean variable which was
         * gotten as input is used
         * to find out if a player has entered
         * the game and prints a message on the
         * players gamefield if he has entered or
         * a place is still free
         * also a short message to inform the player
         * about the gameflow gets added at end
         */
        public static  String getStartText(Boolean gestartet)
        {
            String txt = "\r\n" + (gestartet ? "Spieler ist jetzt im Spiel!" : "Klicken um dem Spiel beizutreten?") + "\r\n";
            txt += " \r\n1. Finde gleiche Memorykarten \r\n2. Falsch aufgedeckte Karten geben eine Strafzeit\r\n3. Kartenpaare sollen schnellstmöglich gefunden werden.\r\n";
            
            return txt +"\r\n";
        }

        
    }
}
