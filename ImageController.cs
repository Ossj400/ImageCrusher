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



/// <summary>
///  Podzielic na klasy. 
///  1. Do ładowania obrazu i przechowywania
///  2. Do zaszumiania
///  3. Do oceny 
///  4. Do inpaintaingu
///  
/// SZUM
/// Pojedyncze piksele szum i dziury kwadratowe na objętności %
///  
/// Przetestować te metody inpaintingu (można opisać)
/// 
/// 
/// </summary>

namespace ImageCrusher
{
    class ImageController
    {
        Image<Bgr, byte> image;
        Image<Bgr, byte> imageOut;
       public Image<Gray, byte> mask;  // delete this ?
        public Bitmap LoadImage()
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                image = new Image<Bgr, byte>(OpenFile.FileName);
            }
            return image.ToBitmap();
        }
        public Bitmap LoadMask()
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                mask = new Image<Gray, byte>(OpenFile.FileName);
            }
            return mask.ToBitmap();
        }
        public void SaveImage()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Image Files ( *.bmp)| *.bmp";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                imageOut.Save(saveFile.FileName);
            }
        }
        public Image<Bgr, Byte> GetImageOut()
        {
            return imageOut;
        }
        public Image<Bgr, Byte> GetImageIn()
        {
            return image;
        }

        public Image<Gray, byte> GetMask()
        {
            if(imageOut!=null)
            mask = image.AbsDiff(imageOut).Convert<Gray,byte>();
            return mask;
        }

        public void RMSE_Algorithm()
        {
            image = new Image<Bgr, byte>(image.ToBitmap());
            double mean = 0;
            double totalRms;
            int n= imageOut.Width * imageOut.Height*3;
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
             totalRms = Math.Sqrt(mean/n);
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

        public Image<Bgr, byte> MakeStarsNoise(int trackBarValue, int noiseRange)  // "Stars" Noise
        {
            imageOut = new Image<Bgr, byte>(image.ToBitmap());
            byte[,,] Data = imageOut.Data;
            Random random = new Random();
            int xPixels = imageOut.Size.Width;
            int yPixels = imageOut.Size.Height;

            for (int r = 0; r <= (Math.Pow(trackBarValue, trackBarValue/2)/2); r++) 
            {
                int randPixX = random.Next(0, (xPixels-1));  // picking pixel for x axis, from 0-max Size - 1
                int randPixY = random.Next(0, (yPixels-1));  // same for y axis
                int pixNoiseRange = noiseRange;
                int absPixNoiseRange = Math.Abs(pixNoiseRange); // for good loop conditioning
                for (; pixNoiseRange <= absPixNoiseRange ; pixNoiseRange++)  // counting from -range to range
                {
                         if ((randPixX + pixNoiseRange >= 0) && (randPixX + pixNoiseRange < xPixels) && (randPixY + pixNoiseRange >= 0) && (randPixY + pixNoiseRange < yPixels)) // if random number and range is in boundaries of array loop is going
                         {
                                int rand = random.Next(0, 255);
                                Data[randPixY, randPixX + pixNoiseRange, 0] = (byte)rand;
                                Data[randPixY, randPixX + pixNoiseRange, 1] = (byte)rand;
                                Data[randPixY, randPixX + pixNoiseRange, 2] = (byte)rand;

                                Data[randPixY + pixNoiseRange, randPixX + pixNoiseRange, 0] = (byte)rand;
                                Data[randPixY + pixNoiseRange, randPixX + pixNoiseRange, 1] = (byte)rand;
                                Data[randPixY + pixNoiseRange, randPixX + pixNoiseRange, 2] = (byte)rand;

                                Data[randPixY + pixNoiseRange, randPixX, 0] = (byte)rand;
                                Data[randPixY + pixNoiseRange, randPixX, 1] = (byte)rand;
                                Data[randPixY + pixNoiseRange, randPixX, 2] = (byte)rand;
                         }
                         else
                         {
                                randPixX = random.Next(0, (xPixels - 1));  // new randoms for same pixNoise range
                                randPixY = random.Next(0, (yPixels - 1));
                         }
                }               
            }
            return imageOut;
        }
        public Image<Bgr, byte> VerticalScratches(int trackBarValue, int noiseRange)
        {
            imageOut = new Image<Bgr, byte>(image.ToBitmap());
            byte[,,] Data = imageOut.Data;
            Random random = new Random();
            int xPixels = imageOut.Size.Width;
            int yPixels = imageOut.Size.Height;

            for (int r = 0; r <= (Math.Pow(trackBarValue, trackBarValue / 2) / 2); r++)
            {
                int randPixX = random.Next(0, (xPixels - 1));  // picking pixel for x axis, from 0-max Size - 1
                int randPixY = random.Next(0, (yPixels - 1));  // same for y axis
                int pixNoiseRange = noiseRange;
                int absPixNoiseRange = Math.Abs(pixNoiseRange); // for good loop conditioning
                for (; pixNoiseRange <= absPixNoiseRange; pixNoiseRange++)  // counting from -range to range
                {
                    if ((randPixX + pixNoiseRange >= 0) && (randPixX - pixNoiseRange >= 0) && (randPixX - pixNoiseRange  < xPixels) && (randPixX + pixNoiseRange < xPixels) && (randPixY + (pixNoiseRange + 1) < yPixels) && (randPixY + (pixNoiseRange + 1) >= 0) && (randPixY - (pixNoiseRange+1)>= 0) && (randPixY - (pixNoiseRange + 1) < yPixels) && (randPixY + (pixNoiseRange + 1) >= 0)) // if random number and range is in boundaries of array loop is going
                    {
                        Data[randPixY + (1 + pixNoiseRange), randPixX + pixNoiseRange, 0] = (byte)random.Next(0, 255);
                        Data[randPixY + (1 + pixNoiseRange), randPixX + pixNoiseRange, 1] = (byte)random.Next(0, 255);
                        Data[randPixY + (1 + pixNoiseRange), randPixX + pixNoiseRange, 2] = (byte)random.Next(0, 255);

                        Data[randPixY - (1 + pixNoiseRange), randPixX - pixNoiseRange, 0] = (byte)random.Next(0, 255);
                        Data[randPixY - (1 + pixNoiseRange), randPixX - pixNoiseRange, 1] = (byte)random.Next(0, 255);
                        Data[randPixY - (1 + pixNoiseRange), randPixX - pixNoiseRange, 2] = (byte)random.Next(0, 255);
                    }
                    else
                    {
                        randPixX = random.Next(0, (xPixels - 1));  // new randoms for same pixNoise range
                        randPixY = random.Next(0, (yPixels - 1));
                    }
                }
            }
            return imageOut;
        }

    }
}
