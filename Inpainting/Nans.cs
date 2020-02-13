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

            int[] nan_list_2 = new int[nan_count];  // clean list of NaN pixels 
            int[] known_list = new int[nm-nan_count];   
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

            int[,] nan_list = new int[nan_count, 3];
            int col = 0;
            int row = 0;
            foreach (int item in k)
            {
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
           // IdentifyNeighbours(n, m, nan_list, talks_to);
            var Nn = IdentifyNeighbours(n, m, nan_list, talks_to).Item1;
            var io = IdentifyNeighbours(n, m, nan_list, talks_to).Item2;
        }

        public Tuple<int[,],int> IdentifyNeighbours(int n, int m, int[,] nan_list, int[,] talks_to)
        {
           // if(nan_list!=null)
            //{
                int nan_count = nan_list.GetLength(0);
                int talk_count = talks_to.GetLength(0);
                int[,] nn = new int[(nan_count * talk_count), 2];  
                int[] j = new int[] {0,nan_count};
                int ij = 0;
                int i = 0;
                int ik = 0;
                int row= 0;
                int col = 0;
                ///// Few If's for replicate matrix with additional +1/-1 values for "row" or "column"  and count ones out of boudaries of original matrix (image)
                #region
                if (i <= talk_count)
                { 
                    for (int z = j[0]; z < j[1]; z++)
                    {
                        nn[z, 0] = nan_list[ik, 1] - 1;
                        nn[z, 1] = nan_list[ik, 2];

                        if ((nan_list[ik, 1] - 1) == -1)
                            ij++;
                        
                        ik++;                      
                    }
                    j[0] = j[0] + nan_count;
                    j[1] = j[1] + nan_count;
                }
                if (i <= talk_count)
                {
                    ik = 0;
                    for (int z = j[0]; z < j[1]; z++)
                    {
                        nn[z, 0] = nan_list[ik, 1];
                        nn[z, 1] = nan_list[ik, 2]-1;

                        if ((nan_list[ik, 2] - 1) == -1)
                            ij++;

                        ik++;
                    }
                    j[0] = j[0] + nan_count;
                    j[1] = j[1] + nan_count;
                }
                if (i <= talk_count)
                {
                    ik = 0;
                    for (int z = j[0]; z < j[1]; z++)
                    {
                        nn[z, 0] = nan_list[ik, 1] + 1;
                        if ((nan_list[ik, 2] + 1) > 10)
                            col++;

                        nn[z, 1] = nan_list[ik, 2];

                        ik++;
                    }
                    j[0] = j[0] + nan_count;
                    j[1] = j[1] + nan_count;
                }
                if (i <= talk_count)
                {
                    ik = 0;
                    for (int z = j[0]; z < j[1]; z++)
                    {
                        nn[z, 0] = nan_list[ik, 1];

                        nn[z, 1] = nan_list[ik, 2]+1;
                        if ((nan_list[ik, 1] + 1) > 8)
                            row++;

                        ik++;
                    }
                    j[0] = j[0] + nan_count;
                    j[1] = j[1] + nan_count;
                }
                #endregion

                // removing ones which are not matching to original matrix boundaries
                int nn_length = nn.GetLength(0) - (row + col + ij);
                int[,] nn1 = new int[nn_length, 2];
                i = 0; ik = 0;

                for(; ik < nn.Length/2; ik++)
                {
                    if(nn[ik, 0]>= 0 && nn[ik, 1] >=0 && nn[ik, 0] < 9 && nn[ik, 1] < 11 ) 
                    {
                        nn1[i, 0] = nn[ik, 0];
                        nn1[i, 1] = nn[ik, 1];
                        i++;
                    }
                }





           // }
            return new Tuple<int[,], int>(nn1, i);

        }

    }
}
