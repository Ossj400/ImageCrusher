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
            int j = 0;
            
            var A = imageIn;
            var mas = mask;
            int n = A.Height;
            int m = A.Width+1;
            int c = 1;
            int nmc = n*m*c;

            int[,] nan_rc = new int[mas.Height, mas.Width + 1];
            int[] k = new int[nmc]; //Any value in mask is "failure"/NaN so mask values can be 'k' values but converted in 1 dim arr
            foreach (int item in mas.Data)  // convert to 1 dim and sets where are Nans in rows and cols.
            {
                k[i] = item;
                i++;
            }
            i = 0;

            string nan_list_string = mas.CountNonzero().GetValue(0).ToString();
            Int32.TryParse(nan_list_string, out int nan_list_1);   // nan_list_1 is int value of NaNs

            int[] nan_list_2 = new int[nan_list_1];  // clean list of NaN pixels
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
            i = 0; j = 0;
            foreach (int item in k)
            {
                if (item == 0)
                {
                    known_list[j] = i;
                    j++;
                }
                i++;
            }
            i = 0;j = 0;

            int nan_count = nan_list_1;

            int[,] nan_list = new int[nan_count+1,3]; 
            int col = 0;
            int row = 0;
            foreach (int item in k)
            {
                if (mas.Height == col)
                {
                    col = 0;
                }
                if (i>3)
                {
                    if((i+1) % mas.Width == 1)
                    row++;
                }
              if(item != 0)
                {
                    nan_list[j, 0] = item;    // dobrze ?
                    j++;
                    {
                        nan_list[j, 1] = row;
                    }
                    {
                        nan_list[j, 2] = col;
                    }
                }
               i++;
               col++;
            }
            i = 0;j = 0;row = 0;col = 0;


            int[,] talks_to = new int[4,2] { { -1, 0 },{ 0, -1 }, { -1, 1 }, { 0, 1 } };  // dobrze ?

            IdentifyNeighbours(n, m, nan_list, talks_to);
        }

        public void IdentifyNeighbours(int n, int m, int[,] nan_list, int[,] talks_to)
        {
            if(nan_list!=null)
            {
                int nan_count = nan_list.GetLength(0);
                int talk_count = talks_to.GetLength(0);
                int[,] nn = new int[(nan_count * talk_count), 1]; 
                int[] j = new int[] {1,nan_count};

                for(int i=0; i< talk_count;i++)
                {
                   // nn[j[0],j[1]] = nan_list(            //co to za zapis, co to ten repmat i co to te przecinki?  nn(j(1):j(2),:)=nan_list(:,2:3) + repmat(talks_to(i,:),nan_count,1);    >>>  A(:,2:3) bierze kolumny 2 i 3 z Tab A



                    j[0]= (j[0] + nan_count);
                    j[1]= j[1]+nan_count;
                }
            }   
        }

    }
}
