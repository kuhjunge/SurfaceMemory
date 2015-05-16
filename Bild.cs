using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SurfaceMemory
{
    /*
     * Class for the memoryimage
     * which is used for the front-
     * or background image of the memorycards

     */
    class MemoryImage
    {
        private string pfad;
        private ImageBrush image = null;

        public MemoryImage(String pfad)
        {
            this.pfad = pfad;
            image = loadPictures(pfad);
        }

        /*
         * GetterFunction to return
         * filepath of the imagefile
         * as string
         */
        public string Pfad
        {
            get { return pfad; }
        }

        /*
         * Getterfunction to return
         * the name of the imagefile
         */
        public  ImageBrush getImage
        {
            get { return image;}
        }

        #region LoadBilder

        /*
         * Function in with the help
         * of the imagefilename and
         * a URI for the right
         * filepath the image gets created
         */
        private ImageBrush loadPictures(String uri)
        {
            try
            {
                var brush = new ImageBrush();
                /* Bildeigenschaften
                 * Buildvorgang : Inhalt
                 * Im Ausgabeverzeichnis: Immer kopieren
                 */
               // brush.ImageSource = new BitmapImage(new Uri("Resources/" + uri, UriKind.Relative));
                brush.ImageSource = new BitmapImage(new Uri(uri, UriKind.Relative));
                return brush;
            }
            catch (Exception )
            {
                // bla
                return null;
            }
        }
        #endregion
    }
}
