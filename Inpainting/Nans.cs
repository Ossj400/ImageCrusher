using System;
using Emgu.CV.Structure;
using Emgu.CV;
using ImageCrusher.ImageController;
using ExtensionMethodsSpace;
using System.Collections.Generic;
using System.Linq;

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
            int nanCount = 0;           
            int n = imageIn.Height;   // Data[x,y,channel]   x = n = rows     y = m = columns   
            int m = imageIn.Width;
            int nm = n*m;
            bool zero = false;

            int[] k = new int[nm]; 

            for (int item = 0; item < nm; item++)  //// new ~~~~~~~~~~~~ !!!!!!!!!!!!!!!!!
            {
                if (imageOut.Data[ij, j, channel] != imageIn.Data[ij, j, channel])
                {
                    k[i] = i;
                    nanCount++;
                    if (imageOut.Data[0, 0, channel] != imageIn.Data[0, 0, channel])
                        zero = true;
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
            i = 0; j = 0;

            int[] nanList2 = new int[nanCount];  // clean list of NaN pixels 
            int[] knownList = new int[nm-nanCount];
            bool zero0 = zero;
            foreach (int item in k)
            {
                if (knownList.Length > 0 && item == 0)
                {
                    if (zero0 == true)
                    {
                        zero0 = false;
                    }
                    else
                    {
                        knownList[j] = i;
                        j++;
                    }
                }
                i++;
            }
            i = 0; j = 0;

            foreach (int item in k)
            {
                if (zero==true)
                {
                    nanList2[0] = i;
                    zero = false;
                    j++;
                }
                if (item > 0)
                {
                    nanList2[j] = i;
                    j++;
                }
                i++;
            }
            i = 0; j = 0;



            int[,] nanList = new int[nanCount, 3];
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
                    nanList[j, 0] = item;  
                    nanList[j, 1] = col;   // w matlabie zle opisano, columna to row. więc col = row, row = col
                    nanList[j, 2] = row;
                    j++;
                }
                i++;
                col++;
            }   
            i = 0;j = 0;row = 0;col = 0; ij = 0;

            int[,] talks_to = new int[4,2] { { -1, 0 },{ 0, -1 }, { -1, 1 }, { 0, 1 } };

            var identifyNeighs = IdentifyNeighbours(n, m, nanList, talks_to);
            var nn = identifyNeighs.Item1;
            var neighboursList = identifyNeighs.Item2;
            // setting all_list 
            int[,] allList = new int[nanCount + neighboursList.GetLength(0), 3];

            for(; i < nanList.GetLength(0); i++)
            {
                allList[i, 0] = nanList[i, 0];
                allList[i, 1] = nanList[i, 1];
                allList[i, 2] = nanList[i, 2];
                ij = i;
            }
            for (; i <= ij + neighboursList.GetLength(0); i++)
            {
                allList[i, 0] = neighboursList[j, 0];
                allList[i, 1] = neighboursList[j, 1];
                allList[i, 2] = neighboursList[j, 2];
                j++;
            }

        }

        public Tuple<int[,], int[,]> IdentifyNeighbours(int n, int m, int[,] nanList, int[,] talks_to)
        {
           // if(nan_list!=null)
            //{
             int nanCount = nanList.GetLength(0);
             int talkCount = talks_to.GetLength(0);
             int[,] nn = new int[(nanCount * talkCount), 2];  
             int[] j = new int[] {0,nanCount};
             int ij = 0;
             int i = 0;
             int ik = 0;
             int row= 0;
             int col = 0;
             ///// Few If's for replicate matrix with additional +1/-1 values for "row" or "column"  and count ones out of boudaries of original matrix (image)
             #region
                if (i <= talkCount)
                { 
                    for (int z = j[0]; z < j[1]; z++)
                    {
                        nn[z, 0] = nanList[ik, 1] - 1;
                        nn[z, 1] = nanList[ik, 2];

                        if ((nanList[ik, 1] - 1) == -1)
                            ij++;
                        
                        ik++;                      
                    }
                    j[0] = j[0] + nanCount;
                    j[1] = j[1] + nanCount;
                }
                if (i <= talkCount)
                {
                    ik = 0;
                    for (int z = j[0]; z < j[1]; z++)
                    {
                        nn[z, 0] = nanList[ik, 1];
                        nn[z, 1] = nanList[ik, 2]-1;

                        if ((nanList[ik, 2] - 1) == -1)
                            ij++;

                        ik++;
                    }
                    j[0] = j[0] + nanCount;
                    j[1] = j[1] + nanCount;
                }
                if (i <= talkCount)
                {
                    ik = 0;
                    for (int z = j[0]; z < j[1]; z++)
                    {
                        nn[z, 0] = nanList[ik, 1] + 1;
                        if ((nanList[ik, 2] + 1) > 10)
                            col++;

                        nn[z, 1] = nanList[ik, 2];

                        ik++;
                    }
                    j[0] = j[0] + nanCount;
                    j[1] = j[1] + nanCount;
                }
                if (i <= talkCount)
                {
                    ik = 0;
                    for (int z = j[0]; z < j[1]; z++)
                    {
                        nn[z, 0] = nanList[ik, 1];

                        nn[z, 1] = nanList[ik, 2]+1;
                        if ((nanList[ik, 1] + 1) > 8)
                            row++;

                        ik++;
                    }
                    j[0] = j[0] + nanCount;
                    j[1] = j[1] + nanCount;
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

            int[,] neigboursList = new int[nn1.GetLength(0), 3];
            col = 0; row = 0; ij = 0; ik = 0;
            i = nn1[0, 0];
            for (int item = 0; item < nn1.GetLength(0); item++)
            {
                if (n == col)
                {
                    row++;
                    col = 0;
                    if(m == row)
                    {
                        row = 0;
                    }
                }
                if (nn1[ik, 1] > 0)
                {
                    neigboursList[ik, 0] = ((nn1[ik, 1]+1) * (n) + (nn1[ik, 0] - (n)));
                    neigboursList[ik, 1] = nn1[ik, 0];
                    neigboursList[ik, 2] = nn1[ik, 1];
                    ik++;
                    if (ik == nn1.Length / 2)
                        break;
                }
                if (nn1[ik, 1] < 1)
                {
                    neigboursList[ik, 0] = nn1[ik, 0];
                    neigboursList[ik, 1] = nn1[ik, 0];
                    neigboursList[ik, 2] = nn1[ik, 1];
                    ik++;
                }                
                i++;
                col++;
            }
            // }
            int[,] neigboursListTemp= new int[nn1.GetLength(0), 3];

            //removing duplicates from neighs list
            for (int item = 0; item < nn1.GetLength(0); item++)
            {
                int val = neigboursList[item, 0];
                int originalOrDup = 0;

                for (int s=0; s<nn1.GetLength(0); s++)
                {
                    if (neigboursList[s, 0] == val)
                    {
                        originalOrDup++;
                        if (originalOrDup > 1)
                        {
                            neigboursListTemp[s, 0] = -1;
                            neigboursListTemp[s, 1] = -1;
                            neigboursListTemp[s, 2] = -1;
                        }
                        else
                        {
                            neigboursListTemp[s, 0] = neigboursList[s, 0];
                            neigboursListTemp[s, 1] = neigboursList[s, 1];
                            neigboursListTemp[s, 2] = neigboursList[s, 2];
                        }
                    }
                }
            }
            // removing duplicates compared to nanlist
            int dupCounter = 0;
            for (int item = 0; item < nanList.GetLength(0); item++)
            {
                int val = nanList[item, 0];

                for (int s = 0; s < neigboursListTemp.GetLength(0); s++)
                {
                    if (neigboursListTemp[s, 0] == val)
                    {
                          neigboursListTemp[s, 0] = -1;
                          neigboursListTemp[s, 1] = -1;
                          neigboursListTemp[s, 2] = -1;
                        dupCounter++;
                    }
                }
            }

            dupCounter = 0;
            i = 0;
            for (int item = 0; item < nn1.GetLength(0); item++)
            {
                if(neigboursListTemp[item,0]>=0)
                {
                    neigboursListTemp[i, 0] = neigboursListTemp[item, 0];
                    neigboursListTemp[i, 1] = neigboursListTemp[item, 1];
                    neigboursListTemp[i, 2] = neigboursListTemp[item, 2];
                    i++;
                }
                else
                {
                    neigboursListTemp[i, 0] = -1;
                    neigboursListTemp[i, 1] = -1;
                    neigboursListTemp[i, 2] = -1;
                    dupCounter++;
                }
            }
            i = 0; ij = 0;

            int[,] neigboursListUnique = new int[nn1.GetLength(0) - dupCounter, 3];            
            for(;   i<neigboursListUnique.GetLength(0); i++)
            { 
                 neigboursListUnique[i, 0] = neigboursListTemp[i, 0];
                 neigboursListUnique[i, 1] = neigboursListTemp[i, 1];
                 neigboursListUnique[i, 2] = neigboursListTemp[i, 2];   
            }
            ///sorting uniqueArray
            neigboursListUnique.SortByFirstColumn();

            return new Tuple<int[,], int[,]>(nn1, neigboursListUnique);

        }


    }
}
