using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Forms;
using Emgu.CV.Quality;
using System.Drawing.Imaging;
using ImageCrusher.ImageController;

namespace ImageCrusher.ImageController
{
    class NavierStokesInpaint
    {
        Image<Rgb, byte> imageOutNav;
        Image<Rgb, byte> imageIn;
        Image<Gray, byte> mask;

        public Image<Rgb, byte> InpaintNav(ImageMenu image, Noise mask)
        {
            imageIn = image.GetImageIn();
            imageOutNav = new Image<Rgb, byte>(image.GetImageIn().ToBitmap());
            this.mask = mask.GetMask(); 
            CvInvoke.Inpaint(imageIn, this.mask, imageOutNav, 1, 0);
            
            return imageOutNav;
        }
        public Image<Rgb, byte> GetImage()
        {
           return imageOutNav;
        }

    }
}
