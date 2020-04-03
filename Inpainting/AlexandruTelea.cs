using System;
using Emgu.CV.Structure;
using Emgu.CV;
using ImageCrusher.ImageController;

namespace ImageCrusher.Inpainting
{
    class AlexandruTeleaInpaint
    {
        Image<Bgr, byte> imageOutTelea;
        Image<Bgr, byte> imageIn;
        Image<Gray, byte> mask;

        public Image<Bgr, byte> ImageOutTelea { get => imageOutTelea; set => imageOutTelea = value; }

        public Image<Bgr, byte> InpaintTel(ImageMenu image, Noise mask, double radius)
        {
            imageIn = image.Img;
            ImageOutTelea = image.Img.Copy();
            this.mask = mask.GetMask();
            CvInvoke.Inpaint(imageIn, this.mask, ImageOutTelea, radius, Emgu.CV.CvEnum.InpaintType.Telea);

            return ImageOutTelea;
        }

    }
}

