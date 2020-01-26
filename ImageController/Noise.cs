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
        Image<Rgb, byte> img;
        Image<Gray, byte> mask;
        Image<Gray, byte> maskLoaded;
        Image<Rgb, byte> imageOut;

        public Image<Rgb, byte> ImageOut { get => imageOut; set => imageOut = value; }
        public Image<Gray, byte> MaskLoaded { get => maskLoaded; set => maskLoaded = value; }
        public Image<Gray, byte> Mask { get => mask; set => mask = value; }

        public Noise(ImageMenu image)
        {
            this.img = image.Img;
            MaskLoaded = image.Mask;          
        }
      
        public Image<Rgb, byte> MakeStarsNoise(int trackBarValue, int noiseRange)  // "Stars" Noise
        {
            ImageOut = new Image<Rgb, byte>(img.ToBitmap());
            byte[,,] Data = ImageOut.Data;
            Random random = new Random();
            int xPixels = ImageOut.Size.Width;
            int yPixels = ImageOut.Size.Height;

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
            return ImageOut;
        }
        public Image<Rgb, byte> VerticalScratches(int trackBarValue, int noiseRange)
        {
            ImageOut = new Image<Rgb, byte>(img.ToBitmap());
            byte[,,] Data = ImageOut.Data;
            Random random = new Random();
            int xPixels = ImageOut.Size.Width;
            int yPixels = ImageOut.Size.Height;

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
            return ImageOut;
        }
        public Image<Gray, byte> GetMask()
        {
            if (ImageOut != null && MaskLoaded == null)
                Mask = img.AbsDiff(ImageOut).Convert<Gray, byte>();
            else
                Mask = MaskLoaded;

            return Mask;
        }
    }
}
