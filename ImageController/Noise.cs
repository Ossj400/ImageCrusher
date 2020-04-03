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
        Image<Bgr, byte> imageOut;

        public Image<Bgr, byte> ImageOut { get => imageOut; set => imageOut = value; }
        public Image<Gray, byte> MaskLoaded { get => maskLoaded; set => maskLoaded = value; }
        public Image<Gray, byte> Mask { get => mask; set => mask = value; }

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
                    int randPixX = random.Next(0, (xPixels - 1));  // picking pixel for x axis, from 0-max Size - 1
                    int randPixY = random.Next(0, (yPixels - 1));  // same for y axis
                    if ((randPixX >= 0) && (randPixX < xPixels) && (randPixY >= 0) && (randPixY < yPixels)) // if random number and range is in boundaries of array loop is going
                    {
                        int rand = random.Next(1, 255);
                        Data[randPixY, randPixX, 0] = (byte)rand;
                        Data[randPixY, randPixX, 1] = (byte)rand;
                        Data[randPixY, randPixX, 2] = (byte)rand;
                    }
                }
            }
            return ImageOut;
        }
        public Image<Bgr, byte> VerticalScratches(int trackBarValue, int noiseRange)
        {
            ImageOut = img.Copy();
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
                        Data[randPixY + (1 + pixNoiseRange), randPixX + pixNoiseRange, 0] = (byte)random.Next(1, 255);
                        Data[randPixY + (1 + pixNoiseRange), randPixX + pixNoiseRange, 1] = (byte)random.Next(1, 255);
                        Data[randPixY + (1 + pixNoiseRange), randPixX + pixNoiseRange, 2] = (byte)random.Next(1, 255);

                        Data[randPixY - (1 + pixNoiseRange), randPixX - pixNoiseRange, 0] = (byte)random.Next(1, 255);
                        Data[randPixY - (1 + pixNoiseRange), randPixX - pixNoiseRange, 1] = (byte)random.Next(1, 255);
                        Data[randPixY - (1 + pixNoiseRange), randPixX - pixNoiseRange, 2] = (byte)random.Next(1, 255);
                    }
                    else
                    {
                        randPixX = random.Next(1, (xPixels - 1));  // new randoms for same pixNoise range
                        randPixY = random.Next(1, (yPixels - 1));
                    }
                }
            }
            return ImageOut;
        }

        public Image<Bgr, byte> Square(int noiseRange)
        {
            noiseRange = 3 * noiseRange;
            ImageOut = img.Copy();
            byte[,,] Data = ImageOut.Data;
            Random random = new Random();
            int xPixels = ImageOut.Size.Width;
            int yPixels = ImageOut.Size.Height;
            int randPixX = random.Next(0, (xPixels -1 + noiseRange));  // picking pixel for in good boundaries for x axis   |  from 0-max Size - 1
            int randPixY = random.Next(0, (yPixels - 1 + 2*noiseRange));  // same for y axis

            for (int r = noiseRange; r <= Math.Abs(noiseRange)/10; r++)
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

            Image<Gray, byte> Mask2 = Mask.Copy();
            Mask2.SetValue(255); 
            Mask2.SetValue(0, Mask);
            return Mask2;
        }
    }
}
