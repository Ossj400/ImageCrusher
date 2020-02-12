using System;
using Emgu.CV.Structure;
using Emgu.CV;
using ImageCrusher.ImageController;

namespace ImageCrusher.Inpainting
{
    class Nans
    {
        Image<Rgb, byte> imageOutNans;
        Image<Rgb, byte> imageOut;
        Image<Rgb, byte> imageIn;

        public Image<Rgb, byte> ImageOutNans { get => imageOutNans; set => imageOutNans = value; }

        public Nans(ImageMenu image, Noise noise)
        {
            imageIn = image.Img;
            imageOut = noise.ImageOut;
            ImageOutNans = new Image<Rgb, byte>(image.Img.ToBitmap());
        }
        public Nans(ImageMenu image)
        {
            imageIn = image.Img;
            imageOut = image.ImageOut;
            ImageOutNans = new Image<Rgb, byte>(image.Img.ToBitmap());
        }

        public Nans()
        {
        }

        public void Compute(int channel) //  channel = 0-2; // red=0, green=1, blue=2
        {
            int i = 0;
            int j = 0;
            int ij = 0;
           

            int nan_count = 0;
            
            int n = imageIn.Height;   // Data[x,y,channel]   x = n = rows     y = m = columns   
            int m = imageIn.Width;
            int nm = n*m;

            int[] k = new int[nm]; 

            for (int item = 0; item < nm; item++)  //// new ~~~~~~~~~~~~ !!!!!!!!!!!!!!!!!
            {
                if (imageOut.Data[ij, j, channel] != imageIn.Data[ij, j, channel])
                {
                    k[i] = i;
                    nan_count++;                   
                }
                i++;
                ij++;
                if (ij == imageOut.Rows)
                {
                    ij = 0;
                    j++;

                    if (j == imageOut.Cols)
                        j = 0;
                }
            }

            int[] nan_list_2 = new int[nan_count];  // clean list of NaN pixels  BUT + 1 to index wont go out of range ...
            int[] known_list = new int[nm-nan_count + 1];   // +1 beacuse is array = 0 and there can be item == 0 and error xD
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
                if (known_list.Length > 0 && item == 0)
                {
                    known_list[j] = i;
                    j++;
                }
                i++;
            }
            i = 0;j = 0;

            int[,] nan_list = new int[nan_count,3];    //// nan+1 
            int col = 0;
            int row = 0;
            foreach (int item in k)
            {
                //if (i > 3 && row < imageIn.Height && col == imageIn.Width)
                //{
                //    // if((i+1) % imageIn.Height == 1)
                //    row++;
                //}
                if (imageIn.Height == col)
                {
                    row++;
                    col = 0;
                }

                if(item != 0)
                {
                    nan_list[j, 0] = item;  
                    nan_list[j, 1] = col;   // w matlabie zle opisano, columna to row. więc col = row, row = col
                    nan_list[j, 2] = row;
                    j++;
                }
                i++;
                col++;
            }   
            i = 0;j = 0;row = 0;col = 0;

            int[,] talks_to = new int[4,2] { { -1, 0 },{ 0, -1 }, { -1, 1 }, { 0, 1 } };

            IdentifyNeighbours(n, m, nan_list, talks_to);
        }

        public void IdentifyNeighbours(int n, int m, int[,] nan_list, int[,] talks_to)
        {
            if(nan_list!=null)
            {
                int nan_count = nan_list.GetLength(0);
                int talk_count = talks_to.GetLength(0);
                int[,] nn = new int[(nan_count * talk_count), 2];  
                int[] j = new int[] {0,nan_count};
                int ij = 0;
                for(int i=0; i< talk_count;i++)
                {
                    int ik = 0;
                    for(int z=j[0]; z<j[1]; z++)
                    {
                        nn[z,0] = nan_list[ik, 1];        //co to za zapis, co to ten repmat i co to te przecinki?  nn(j(1):j(2),:)=nan_list(:,2:3) + repmat(talks_to(i,:),nan_count,1);    >>>  A(:,2:3) bierze kolumny 2 i 3 z Tab A
                        nn[z, 1] = nan_list[ik, 2];    
                        ik++;
                    }

                    j[0]= j[0]+ nan_count;
                    j[1]= j[1]+nan_count;
                }
            }   
        }

    }
}
