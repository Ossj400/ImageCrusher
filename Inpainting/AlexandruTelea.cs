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

namespace ImageCrusher.Inpainting
{
    class AlexandruTeleaInpaint
    {
        Image<Rgb, byte> imageOutTelea;
        Image<Rgb, byte> imageIn;
        Image<Gray, byte> mask;

        public Image<Rgb, byte> InpaintTel(ImageMenu image, Noise mask)
        {
            imageIn = image.GetImageIn();
            imageOutTelea = new Image<Rgb, byte>(image.GetImageIn().ToBitmap());
            this.mask = mask.GetMask();
            CvInvoke.Inpaint(imageIn, this.mask, imageOutTelea, 1, Emgu.CV.CvEnum.InpaintType.Telea);

            return imageOutTelea;
        }
        public Image<Rgb, byte> GetImage()
        {
            return imageOutTelea;
        }

    }
}

