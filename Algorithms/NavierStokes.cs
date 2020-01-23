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

namespace ImageCrusher.Algorithms
{
    class NavierStokesInpaint
    {
        Image<Bgr, byte> imageOutNav;
        Image<Bgr, byte> imageIn;
        Image<Gray, byte> mask;

        public Image<Bgr, byte> InpaintNav(ImageController image)
        {
            imageIn = image.GetImageIn();
            imageOutNav = new Image<Bgr, byte>(image.GetImageIn().ToBitmap());
            mask = image.GetMask(); 
            CvInvoke.Inpaint(imageIn, mask, imageOutNav, 3, 0);
            
            return imageOutNav;
        }
    }
}
