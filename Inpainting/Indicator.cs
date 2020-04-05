using System;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.Quality;
using ImageCrusher.ImageController;

namespace ImageCrusher.Inpainting
{
    class Indicator
    {
        private Image<Bgr, byte> img;
        private Image<Bgr, byte> imageOut;
        double rmsE;
        double mse;
        public Image<Bgr, byte> Img { get => img; set => img = value; }
        public Image<Bgr, byte> ImageOut { get => imageOut; set => imageOut = value; }

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
        public Indicator(ImageMenu image, Nans NansAlg)
        {
            this.Img = image.Img;
            this.ImageOut = NansAlg.ImageOutNans;
        }
        public Indicator(ImageMenu image, FrequencySelectiveReconstruction FSR)
        {
            this.Img = image.Img;
            this.ImageOut = FSR.ImageOutFSR.ToImage<Bgr, byte>();
        }

        public void RMSE_Algorithm()
        {
            Img = new Image<Bgr, byte>(Img.Data);
            double mean = 0;
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
            rmsE = Math.Sqrt(mean / n);
            mse = mean / n;
        }
        public double RMSE()
        {
            QualityMSE MSE = new QualityMSE(Img);
            double mSE = MSE.Compute(ImageOut).V0 + MSE.Compute(ImageOut).V1 + MSE.Compute(ImageOut).V2;
            double rMSE = Math.Sqrt(mSE / 3);
            return rMSE;
        }

        public double PNSR()
        {
            //QualityPSNR PSNR = new QualityPSNR(Img);
            //double[] pSNR = new double[4];
            //pSNR = PSNR.Compute(ImageOut).ToArray();
            RMSE_Algorithm();
            int bitSignal = 8;
            double myPSNR = 10 * Math.Log10((Math.Pow((Math.Pow(2, bitSignal) - 1), 2) / mse));      // 8 - lb bitów
            return myPSNR;
        }
        public double SSIM()
        {
            QualitySSIM sSIM = new QualitySSIM(Img);
            double ssim = sSIM.Compute(ImageOut).V0 + sSIM.Compute(ImageOut).V1 + sSIM.Compute(ImageOut).V2;
            return ssim/3;
        }

        public double GMSD()
        {
            QualityGMSD gMSD = new QualityGMSD(Img);
            double gmsd = gMSD.Compute(ImageOut).V0 + gMSD.Compute(ImageOut).V1 + gMSD.Compute(ImageOut).V2;
            return gmsd/3;
        }
    }
}
