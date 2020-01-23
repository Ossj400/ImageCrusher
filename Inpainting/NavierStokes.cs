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
        Image<Bgr, byte> imageOutNav;
        Image<Bgr, byte> imageIn;
        Image<Gray, byte> mask;

        public Image<Bgr, byte> InpaintNav(ImageMenu image, Noise mask)
        {
            imageIn = image.GetImageIn();
            imageOutNav = new Image<Bgr, byte>(image.GetImageIn().ToBitmap());
            this.mask = mask.GetMask(); 
            CvInvoke.Inpaint(imageIn, this.mask, imageOutNav, 1, 0);
            
            return imageOutNav;
        }
        public Image<Bgr, byte> GetImage()
        {
           return imageOutNav;
        }

    }
}
