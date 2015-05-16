using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media; 

namespace SurfaceMemory
{
    /*
     * Class for the memorycard of the game
     * with functions to control its behavior
     */
    class SurfaceMemoryCard : SurfaceButton
    {

        private MemoryImage bild;
        private Boolean aufgedeckt = false;
        private string id;
        public bool isAccessible = true;

        /*
         * Constructor for MemoryCard
         * gets a string and a memoryimage object
         * as input and references these
         * also calls functions from memoryproperty and image
         * for the border and for the backgroundimage
         */
        public SurfaceMemoryCard(string kid, MemoryImage bild)
        {
            this.bild = bild;
            this.id = kid;
            this.Margin = MemoryProberty.getThicknes(5);
            //this.Padding= MemoryProberty.getThicknes(20); // doesn't work
            //this.BorderThickness = MemoryProberty.getThicknes(20); // doesn't work
            //this.BorderBrush = new SolidColorBrush(Colors.Gray);
            this.Background = Bild.getImage;
            //this.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            //this.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
        }

        /*
         * constructor
         * for the memoryimage
         * which returns depending
         * on if aufgedeckt is true 
         * its own image or
         * when false, the backgroundimage
         */
        public MemoryImage Bild 
        {
            get {
                if (aufgedeckt)
                {
                    return bild;
                }
                else
                {
                    return MemoryProberty.getMemoryBackImage();
                }
            }
        }

        /*
         * Function which returns
         * the boolean variable
         * aufgedeckt which 
         * decides if the memorycard
         * has been uncovered
         */
        public Boolean Aufgedeckt
        {
            get { return aufgedeckt; }
        }

        /*
         * Function which
         * gets a MemoryImage as input parameter
         * and compares its own image
         * with the image gotten as input+
         * and returns a boolean depending on result
         */
        public Boolean hasSameCover(MemoryImage b)
        {
            return bild == b; // Achung Bild != bild !!
        }

        /*
         * Function which 
         * gets as input the boolean variable aufgedeckt
         * and sets it to this objects aufgdeckt
         * and for the background gets called
         * a function to load the backgroundimage
         */
        public void setAufgedeckt(Boolean aufgedeckt)  // <<===================== sollte eigentlich protected bzw. nur im namespace sichtbar sein?!?!?
        {
            this.aufgedeckt = aufgedeckt;
            this.Background = Bild.getImage;
        }

        /*
         * Function which
         * returns a string of memoryimagepath
         * if the boolean variable is true
         * and else returns a empty string
         */
        public override string ToString()
        {
            return aufgedeckt ? bild.Pfad : "-";
        }

    }
}
