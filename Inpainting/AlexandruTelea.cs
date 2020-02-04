using System;
using Emgu.CV.Structure;
using Emgu.CV;
using ImageCrusher.ImageController;

namespace ImageCrusher.Inpainting
{
    class AlexandruTeleaInpaint
    {
        Image<Rgb, byte> imageOutTelea;
        Image<Rgb, byte> imageIn;
        Image<Gray, byte> mask;

        public Image<Rgb, byte> ImageOutTelea { get => imageOutTelea; set => imageOutTelea = value; }

        public Image<Rgb, byte> InpaintTel(ImageMenu image, Noise mask, double radius)
        {
            imageIn = image.Img;
            ImageOutTelea = new Image<Rgb, byte>(image.Img.ToBitmap());
            this.mask = mask.GetMask();
            CvInvoke.Inpaint(imageIn, this.mask, ImageOutTelea, radius, Emgu.CV.CvEnum.InpaintType.Telea);

            return ImageOutTelea;
        }

    }
}

