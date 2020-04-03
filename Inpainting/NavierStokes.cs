using System;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Diagnostics;

namespace ImageCrusher.ImageController
{
    class NavierStokesInpaint
    {
        Image<Bgr, byte> imageOutNav;
        Image<Bgr, byte> imageIn;
        Image<Gray, byte> mask;

        public Image<Bgr, byte> ImageOutNav { get => imageOutNav; set => imageOutNav = value; }

        public Image<Bgr, byte> InpaintNav(ImageMenu image, Noise mask, double radius)
        {
            imageIn = image.Img;
            ImageOutNav = image.Img.Copy();
            this.mask = mask.GetMask();            
            CvInvoke.Inpaint(imageIn, this.mask, ImageOutNav, radius, 0);
            
            return ImageOutNav;
        }
    }
}
