﻿using System;
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
        private Image<Bgr, byte> image;
        private Image<Bgr, byte> imageOut;

       public Indicator (ImageMenu image, NavierStokesInpaint navierStokesInpaint)
        {
            this.image = image.GetImageIn();
            this.imageOut = navierStokesInpaint.GetImage();
        }
        public Indicator(ImageMenu image, AlexandruTeleaInpaint alexandruTeleaInpaint)
        {
            this.image = image.GetImageIn();
            this.imageOut = alexandruTeleaInpaint.GetImage();
        }

        public void RMSE_Algorithm()
        {
            image = new Image<Bgr, byte>(image.ToBitmap());
            double mean = 0;
            double totalRms;
            int n = imageOut.Width * imageOut.Height * 3;
            int orgVal;
            int newVal;
            byte[,,] Data = image.Data;
            byte[,,] Data2 = imageOut.Data;

            for (int i = 0; i < (image.Height); i++)
            {
                for (int j = 0; j < (image.Width); j++)
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
            QualityMSE MSE = new QualityMSE(image);
            double mSE = MSE.Compute(imageOut).V0 + MSE.Compute(imageOut).V1 + MSE.Compute(imageOut).V2;
            double rMSE = Math.Sqrt(mSE / 3);
            return rMSE;
        }

        public double[] PNSR()
        {
            QualityPSNR PSNR = new QualityPSNR(image);
            double[] pSNR = new double[4];
            pSNR = PSNR.Compute(imageOut).ToArray();
            return pSNR;
        }
    }
}