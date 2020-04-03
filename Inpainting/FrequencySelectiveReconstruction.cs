using System;
using Emgu.CV.Structure;
using Emgu.CV;
using ImageCrusher.ImageController;
using Emgu.CV.XPhoto;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace ImageCrusher.Inpainting
{
    class FrequencySelectiveReconstruction
    {
        Mat imageOutFSR = new Mat();
        //Image<Bgr, byte> imageOutFSRIM;
        Mat imageIn;
        Image<Gray,Byte> mask;
        Mat maskM;

        public Image<Bgr,byte> InpaintFSR(ImageMenu image, Noise mask1)
        {
            //imageIn = image.Img.Mat;
            imageIn = new Mat(image.imgPath); // File.Open(image.imgPath, FileMode.Open).To;
            mask = mask1.GetMaskFSR();
            maskM = new Mat(mask.Mat, mask.ROI);
            try
            {
                XPhotoInvoke.Inpaint(imageIn,maskM, imageOutFSR, XPhotoInvoke.InpaintType.FsrFast);
            }
            catch(Exception e)
            {
                string exp = e.ToString();
            }
            return imageOutFSR.ToImage<Bgr,byte>();
        }
    }
}

