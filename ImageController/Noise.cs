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

namespace ImageCrusher.ImageController
{
    class Noise
    {
        Image<Bgr, byte> image;
        Image<Gray, byte> mask;
        Image<Gray, byte> maskLoaded;
        Image<Bgr, byte> imageOut;
        public Noise(ImageMenu image)
        {
            this.image = image.GetImageIn();
            maskLoaded = image.GetMask();          
        }
      
        public Image<Bgr, byte> MakeStarsNoise(int trackBarValue, int noiseRange)  // "Stars" Noise
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
                    if ((randPixX + pixNoiseRange >= 0) && (randPixX - pixNoiseRange >= 0) && (randPixX - pixNoiseRange < xPixels) && (randPixX + pixNoiseRange < xPixels) && (randPixY + (pixNoiseRange + 1) < yPixels) && (randPixY + (pixNoiseRange + 1) >= 0) && (randPixY - (pixNoiseRange + 1) >= 0) && (randPixY - (pixNoiseRange + 1) < yPixels) && (randPixY + (pixNoiseRange + 1) >= 0)) // if random number and range is in boundaries of array loop is going
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
        public Image<Gray, byte> GetMask()
        {
            if (imageOut != null)
                mask = image.AbsDiff(imageOut).Convert<Gray, byte>();
            else
                mask = maskLoaded;

            return mask;
        }
    }
}