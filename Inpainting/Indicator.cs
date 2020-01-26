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
    class Indicator
    {
        private Image<Rgb, byte> img;
        private Image<Rgb, byte> imageOut;

        public Image<Rgb, byte> Img { get => img; set => img = value; }
        public Image<Rgb, byte> ImageOut { get => imageOut; set => imageOut = value; }

        public Indicator (ImageMenu image, NavierStokesInpaint navierStokesInpaint)
        {
            this.Img = image.Img;
            this.ImageOut = navierStokesInpaint.ImageOutNav;
        }
        public Indicator(ImageMenu image, AlexandruTeleaInpaint alexandruTeleaInpaint)
        {
            this.Img = image.Img;
            this.ImageOut = alexandruTeleaInpaint.ImageOutTelea;
        }

        public void RMSE_Algorithm()
        {
            Img = new Image<Rgb, byte>(Img.ToBitmap());
            double mean = 0;
            double totalRms;
            int n = ImageOut.Width * ImageOut.Height * 3;
            int orgVal;
            int newVal;
            byte[,,] Data = Img.Data;
            byte[,,] Data2 = ImageOut.Data;

            for (int i = 0; i < (Img.Height); i++)
            {
                for (int j = 0; j < (Img.Width); j++)
                {
                    orgVal = Data[i, j, 0];
                    newVal = Data2[i, j, 0];
                    mean += Math.Pow(newVal - orgVal, 2);
                    orgVal = Data[i, j, 1];
                    newVal = Data2[i, j, 1];
                    mean += Math.Pow(newVal - orgVal, 2);
                    orgVal = Data[i, j, 2];
                    newVal = Data2[i, j, 2];
                    mean += Math.Pow(newVal - orgVal, 2);
                }
            }
            totalRms = Math.Sqrt(mean / n);
        }
        public double RMSE()
        {
            QualityMSE MSE = new QualityMSE(Img);
            double mSE = MSE.Compute(ImageOut).V0 + MSE.Compute(ImageOut).V1 + MSE.Compute(ImageOut).V2;
            double rMSE = Math.Sqrt(mSE / 3);
            return rMSE;
        }

        public double[] PNSR()
        {
            QualityPSNR PSNR = new QualityPSNR(Img);
            double[] pSNR = new double[4];
            pSNR = PSNR.Compute(ImageOut).ToArray();
            return pSNR;
        }
    }
}
