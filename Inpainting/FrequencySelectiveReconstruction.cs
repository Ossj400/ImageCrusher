using System;
using Emgu.CV.Structure;
using Emgu.CV;
using ImageCrusher.ImageController;
using Emgu.CV.XPhoto;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ImageCrusher.Inpainting
{
    class FrequencySelectiveReconstruction
    {
        Mat imageOutFSR;
        public Mat ImageOutFSR { get => imageOutFSR; set => imageOutFSR = value; }

        public void InpaintFSR(ImageMenu image, Noise mask1, XPhotoInvoke.InpaintType inpaintType)
        {
            Image<Gray, Byte> mask = mask1.GetMaskFSR();
            Mat imageIn = new Mat(image.imgPath);
            Mat maskM = new Mat(mask.Mat, mask.ROI);
            imageOutFSR = new Mat();
            XPhotoInvoke.Inpaint(imageIn, maskM, imageOutFSR, inpaintType);
            ImageOutFSR = imageOutFSR;       
        }

    }
}

