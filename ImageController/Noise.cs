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

namespace ImageCrusher.ImageController
{
    class Noise
    {
        Image<Bgr, byte> img;
        Image<Gray, byte> mask;
        Image<Gray, byte> maskLoaded;
        Image<Gray, byte> mask2;
        Image<Bgr, byte> imageOut;

        public Image<Bgr, byte> ImageOut { get => imageOut; set => imageOut = value; }
        public Image<Gray, byte> MaskLoaded { get => maskLoaded; set => maskLoaded = value; }
        public Image<Gray, byte> Mask { get => mask; set => mask = value; }
        public Image<Gray, byte> Mask2 { get => mask2; set => mask2 = value; }


        public Noise(ImageMenu image)
        {
            this.img = image.Img;
            MaskLoaded = image.Mask;          
        }     
        public Image<Bgr, byte> SaltAndPepperNoise(int trackBarValue, int noiseRange)  // Salt&Pepper Noise
        {
            ImageOut = img.Copy();
            byte[,,] Data = ImageOut.Data;
            Random random = new Random();
            int xPixels = ImageOut.Size.Width;
            int yPixels = ImageOut.Size.Height;

            for (int r = 0; r <= (Math.Pow(trackBarValue, trackBarValue / 2) / 2); r++)
            {
                int pixNoiseRange = noiseRange;
                int absPixNoiseRange = Math.Abs(pixNoiseRange); // for good loop conditioning
                for (; pixNoiseRange <= absPixNoiseRange; pixNoiseRange++)  // counting from -range to range
                {
                    int randPixX = random.Next(1, (xPixels - 1));  // picking pixel for x axis, from 0-max Size - 1
                    int randPixY = random.Next(1, (yPixels - 1));  // same for y axis
                    if ((randPixX >= 0) && (randPixX < xPixels) && (randPixY >= 0) && (randPixY < yPixels)) // if random number and range is in boundaries of array loop is going
                    {
                        int rand = random.Next(0, 255);
                        Data[randPixY, randPixX, 0] = (byte)rand;
                        Data[randPixY, randPixX, 1] = (byte)rand;
                        Data[randPixY, randPixX, 2] = (byte)rand;
                    }
                }
            }
            return ImageOut;
        }
        public Image<Bgr, byte> DiagonalScratches(int trackBarValue, int noiseRange)
        {
            ImageOut = img.Copy();
            byte[,,] Data = ImageOut.Data;
            Random random = new Random();
            int xPixels = ImageOut.Size.Width-1;
            int yPixels = ImageOut.Size.Height-1;
            //double percentage;
            //double noiseAmount = 30;
            for (int r = 0; r <= (Math.Pow(trackBarValue, trackBarValue / 2) / 2); r++)
            {
                int randPixX = random.Next(1, (xPixels - 1));  // picking pixel for x axis, from 0-max Size - 1
                int randPixY = random.Next(1, (yPixels - 1));  // same for y axis
                int pixNoiseRange = noiseRange;
                int absPixNoiseRange = Math.Abs(pixNoiseRange); // for good loop conditioning
                for (; pixNoiseRange <= absPixNoiseRange; pixNoiseRange++)  // counting from -range to range
                {
                    if ((randPixX + pixNoiseRange >= 1) && (randPixX - pixNoiseRange >= 1) && (randPixX - pixNoiseRange < xPixels) && (randPixX + pixNoiseRange < xPixels) && (randPixY + (pixNoiseRange + 1) < yPixels) && (randPixY + (pixNoiseRange + 1) >= 1) && (randPixY - (pixNoiseRange + 1) >= 1) && (randPixY - (pixNoiseRange + 1) < yPixels) && (randPixY + (pixNoiseRange + 1) >= 1)) // if random number and range is in boundaries of array loop is going
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
                        randPixX = random.Next(1, (xPixels - 1));  // new randoms for same pixNoise range
                        randPixY = random.Next(1, (yPixels - 1));
                    }

                    //if (r == (Math.Pow(trackBarValue, trackBarValue / 2) / 2) - 1)
                    //{
                    //    percentage = CorruptedPercentage();
                    //    if (percentage < noiseAmount)
                    //    {
                    //        if (percentage < noiseAmount)
                    //            r = (int)((double)r - (double)((double)r / 50));
                    //        if (percentage < noiseAmount - 2)
                    //            r = (int)((double)r - (double)((double)r / 20));
                    //        if (percentage < noiseAmount - 5)
                    //            r = (int)((double)r - (double)((double)r / 12));
                    //        if (percentage < noiseAmount - 10)
                    //            r = (int)((double)r - (double)((double)r / 5));
                    //        if (percentage < noiseAmount - 20)
                    //            r = (int)((double)r - (double)((double)r / 2));
                    //        if (percentage < noiseAmount - 30)
                    //            r = (int)((double)r - (double)((double)r / 1));
                    //        if (percentage < noiseAmount - 40)
                    //            r = 0;
                    //    }
                    //    else
                    //        break;
                    //}
                }
            }
            return ImageOut;
        }

        public Image<Bgr, byte> Square(int noiseRange)
        {
            ImageOut = img.Copy();
            byte[,,] Data = ImageOut.Data;
            Random random = new Random();
            int xPixels = ImageOut.Size.Width;
            int yPixels = ImageOut.Size.Height;
            int randPixX = random.Next(0, (xPixels - 1 + noiseRange));  // picking pixel for in good boundaries for x axis   |  from 0-max Size - 1
            int randPixY = random.Next(0, (yPixels - 1 + 2 * noiseRange));  // same for y axis

            for (int r = noiseRange; r <= Math.Abs(noiseRange); r++)
            {
                randPixY++;

                int pixNoiseRange = noiseRange;     // make a new variable from noiseRange to use new variable in loop
                int absPixNoiseRange = Math.Abs(pixNoiseRange); // for good loop conditioning
                for (; pixNoiseRange <= absPixNoiseRange; pixNoiseRange++)  // counting from -range to range
                {
                    if (randPixY < yPixels && randPixX + pixNoiseRange < xPixels) // if random number and range is in boundaries of array loop is going
                    {
                        Data[randPixY, randPixX + pixNoiseRange, 0] = 0;
                        Data[randPixY, randPixX + pixNoiseRange, 1] = 0;
                        Data[randPixY, randPixX + pixNoiseRange, 2] = 0;
                    }
                }

            }
            return ImageOut;
        }
        public Image<Bgr, byte> Square2(int noiseRange)
        {
            noiseRange = 5 * noiseRange;
            ImageOut = img.Copy();
            byte[,,] Data = ImageOut.Data;
            Random random = new Random();
            int xPixels = ImageOut.Size.Width-1;
            int yPixels = ImageOut.Size.Height-1;
            int randPixX = random.Next(1, (xPixels -1 + 300));  // picking pixel for in good boundaries for x axis   |  from 0-max Size - 1
            int randPixY = random.Next(1, (yPixels - 1 + 450));  // same for y axis
            int pix = (int)HowManyPixelsNeedToBeCorrupted(35);
            for (int r = noiseRange; r <= Math.Abs(noiseRange) / 10; r++)
            {
                randPixY++;
                int pixNoiseRange = noiseRange;     // make a new variable from noiseRange to use new variable in loop
                int absPixNoiseRange = Math.Abs(pixNoiseRange); // for good loop conditioning
                for (; pixNoiseRange <= absPixNoiseRange; pixNoiseRange++)  // counting from -range to range
                {
                    if (randPixY < yPixels) // if random number and range is in boundaries of array loop is going
                    {
                        Data[randPixY, randPixX + (int)(0.5*pixNoiseRange), 0] = 0;
                        Data[randPixY, randPixX + (int)(0.5 * pixNoiseRange), 1] = 0;
                        Data[randPixY, randPixX + (int)(0.5 * pixNoiseRange), 2] = 0;
                    }
                }
            }
            return ImageOut;
        }
       
        public Image<Bgr, byte> SaltAndPepperNoiseNumbed(int noiseAmount)
        {
            ImageOut = img.Copy();
            byte[,,] Data = ImageOut.Data;
            Random random = new Random();
            int xPixels = ImageOut.Size.Width-1;
            int yPixels = ImageOut.Size.Height-1;
            double percentage;
            int pixNoiseRange = (int)HowManyPixelsNeedToBeCorrupted(noiseAmount);

                for (int i=0; i <= pixNoiseRange; i++)  
                {
                    int randPixX = random.Next(1, (xPixels - 1));  // picking pixel for x axis, from 0-max Size - 1
                    int randPixY = random.Next(1, (yPixels - 1));  // same for y axis                    {
                        int rand = random.Next(0, 255);
                        Data[randPixY, randPixX, 0] = (byte)rand;
                        Data[randPixY, randPixX, 1] = (byte)rand;
                        Data[randPixY, randPixX, 2] = (byte)rand;

                    if (i == pixNoiseRange - 1)
                    {
                        percentage = CorruptedPercentage();
                    if (percentage < noiseAmount)
                    {
                        if (percentage < noiseAmount)
                            i = i - 500;
                        if (percentage < noiseAmount - 2)
                            i = i - 1000;
                        if (percentage < noiseAmount - 5)
                            i = i - 10000;
                        if (percentage < noiseAmount - 10)
                            i = i - 25000;
                        if (percentage < noiseAmount - 20)
                            i = i - 55000;
                        if (percentage < noiseAmount - 30)
                            i = i - 95000;
                        if (percentage < noiseAmount - 40)
                            i = 0;
                    }
                    else
                        break;

                    }
                }

           percentage = CorruptedPercentage();
           return ImageOut;
        }

        public double CorruptedPercentage()
        {
            int ij = 0; int j = 0; double nanCount = 0;
            for (int i = 0; i < ImageOut.Height * ImageOut.Width; i++)
            {
                if (ImageOut.Data[ij, j, 0] != img.Data[ij, j, 0] || ImageOut.Data[ij, j, 1] != img.Data[ij, j, 1] || ImageOut.Data[ij, j, 2] != img.Data[ij, j, 2])
                {
                    nanCount++;
                }
                i++;
                ij++;
                if (ij == ImageOut.Height)
                {
                    ij = 0;
                    j++;
                    if (j == ImageOut.Width)
                        j = 0;
                }
            }
            return (nanCount/(ImageOut.Height*ImageOut.Width)) * 100;
        }

        public double HowManyPixelsNeedToBeCorrupted(double percentOfcorrupted)
        {
            double pxCount = 0;
            pxCount = (0.01 * percentOfcorrupted) * (ImageOut.Height * ImageOut.Width);
            return pxCount;
        }

        public Image<Gray, byte> GetMask()
        {
            if (ImageOut != null && MaskLoaded == null)
                Mask = img.AbsDiff(ImageOut).Convert<Gray, byte>();
            else
                Mask = MaskLoaded;

            return Mask;
        }
        public Image<Gray, byte> GetMaskFSR()
        {
            if (ImageOut != null && MaskLoaded == null)
                Mask = img.AbsDiff(ImageOut).Convert<Gray, byte>();
            else
                Mask = MaskLoaded;
            if(Mask==null)
                Mask = img.AbsDiff(ImageOut).Convert<Gray, byte>();

            Mask2 = Mask.Copy();
            Mask2.SetValue(255); 
            Mask2.SetValue(0, Mask);
            return Mask2;
        }
    }
}
