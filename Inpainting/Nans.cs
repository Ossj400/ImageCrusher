using System;
using Emgu.CV.Structure;
using Emgu.CV;
using ImageCrusher.ImageController;

namespace ImageCrusher.Inpainting
{
    class Nans
    {
        Image<Rgb, byte> imageOutNans;
        Image<Rgb, byte> imageIn;
        Image<Gray, byte> mask;
        public Image<Rgb, byte> ImageOutNans { get => imageOutNans; set => imageOutNans = value; }

        public Nans(ImageMenu image, Noise mask)
        {
            imageIn = image.Img;
            ImageOutNans = new Image<Rgb, byte>(image.Img.ToBitmap());
            this.mask = mask.GetMask();
        }

        public Nans()
        {
        }

        public void Compute()
        {
            int i = 0;

            var A = imageIn;
            var mas = mask;
            var n = A.Height;
            var m = A.Width;
            int c = 3;
            int nmc = n*m*c;

            int[] k = new int[nmc]; // finding NaNs in one dim. array and making it 1 if finds it. Any value in mask is failure so mask values can be 'k' values but converted in 1 dim arr
            foreach (int item in mas.Data)  // convert to 1 dim
            { 
                k[i] = item;
                i++;
            }
            i = 0;
            int j = 0;

            string nan_list_int = mas.CountNonzero().GetValue(0).ToString();
            Int32.TryParse(nan_list_int, out int nan_list_1);

            int[] nan_list_2 = new int[nan_list_1];
            int[] known_list = new int[nmc-nan_list_1];
            foreach(int item in k)
            {
                if(item > 0)
                {
                    nan_list_2[j] = i;
                    j++;
                }
                i++;
            }
            i = 0;
            j = 0;

            foreach (int item in k)
            {
                if (item == 0)
                {
                    known_list[j] = i;
                    j++;
                }
                i++;
            }
            i = 0;
            j = 0;
            int ij = 0;

            int nan_count = nan_list_2.Length;
            int[,] nan_list_3 = new int[1 + nan_count/60, 60];  // add 1 and trim.

            foreach (int item in nan_list_2)
            {
                nan_list_3[i, j] = item;
                if (j < 60)
                {
                    j++;
                }
                if (j==60)
                {
                    i++;
                    j = 0;
                }
                if (i == 1+(nan_count / 60))
                    i = 0;                   
            }

            i = 0;

        }

    }
}
