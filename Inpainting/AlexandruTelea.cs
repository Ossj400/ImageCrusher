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
        Image<Bgr, byte> imageOutTelea;
        Image<Bgr, byte> imageIn;
        Image<Gray, byte> mask;

        public Image<Bgr, byte> InpaintTel(ImageMenu image, Noise mask)
        {
            imageIn = image.GetImageIn();
            imageOutTelea = new Image<Bgr, byte>(image.GetImageIn().ToBitmap());
            this.mask = mask.GetMask();
            CvInvoke.Inpaint(imageIn, this.mask, imageOutTelea, 1, Emgu.CV.CvEnum.InpaintType.Telea);

            return imageOutTelea;
        }
        public Image<Bgr, byte> GetImage()
        {
            return imageOutTelea;
        }

    }
}

