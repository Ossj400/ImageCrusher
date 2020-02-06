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
            int ij = 0;
            int l = 0;
            
            var A = imageIn;
            var mas = mask;
            int n = A.Height;
            int m = A.Width+1;
            int c = 1;
            int nmc = n*m*c;

            int[,] nan_rc = new int[mas.Height, mas.Width + 1];
            int[] k = new int[nmc]; // finding NaNs in one dim. array and making it 1 if finds it. Any value in mask is failure so mask values can be 'k' values but converted in 1 dim arr
            foreach (int item in mas.Data)  // convert to 1 dim and set where are Nans in rows and cols.
            {
                //if (ij == mas.Height-1)
                //    ij = 0;
                //if (j == mas.Width)
                //    j = 0;

                //if (mas.Data[ij,j,0]>0)
                //{
                //    nan_rc[ij, j] = j;
                //}
                k[i] = item;
                i++;
                //j++;
                //ij++;
            }
            i = 0;
            j = 0;

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
            ij = 0;

            int nan_count = nan_list_2.Length;

            int[,] nan_list_3 = new int[nan_count+1,3]; 
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
                    nan_list_3[j, 0] = item;// nan_list_2[j];     // dobrze ?
                    j++;
                    {
                        nan_list_3[j, 1] = row;
                    }
                    {
                        nan_list_3[j, 2] = col;
                    }
                }
               i++;
               col++;
            }
            i = 0;
            j = 0;
            row = 0;
            col = 0;

            //int[,,] nan_list_4 = new int[nan_count, 2 , 2];
            //foreach(int item in nan_list_3)
            //{
            //    if (mas.Height == col)
            //    {
            //        col = 0;
            //    }
            //    if (i > 3)
            //    {
            //        if ((i + 1) % mas.Width == 1)
            //            row++;
            //    }
            //    if (item != 0)
            //    {
            //        nan_list_4[i,0,0] = item;      //val                      
            //        {
            //            nan_list_4[0, i, 0] = row;
            //        }


            //        i++;
            //    }
            //    col++;
            //}

            int[,] talks_to = new int[4,2] { { -1, 0 },{ 0, -1 }, { -1, 1 }, { 0, 1 } };  // dobrze ?

            IdentifyNeighbours(n, m, nan_list_3, talks_to);
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
