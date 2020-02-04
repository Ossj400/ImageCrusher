using System;
using Emgu.CV.Structure;
using Emgu.CV;

namespace ImageCrusher.ImageController
{
    class NavierStokesInpaint
    {
        Image<Rgb, byte> imageOutNav;
        Image<Rgb, byte> imageIn;
        Image<Gray, byte> mask;

        public Image<Rgb, byte> ImageOutNav { get => imageOutNav; set => imageOutNav = value; }

        public Image<Rgb, byte> InpaintNav(ImageMenu image, Noise mask, double radius)
        {
            imageIn = image.Img;
            ImageOutNav = new Image<Rgb, byte>(image.Img.ToBitmap());
            this.mask = mask.GetMask(); 
            CvInvoke.Inpaint(imageIn, this.mask, ImageOutNav, radius, 0);
            
            return ImageOutNav;
        }
    }
}
