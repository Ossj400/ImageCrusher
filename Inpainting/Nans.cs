using System;
using Emgu.CV.Structure;
using Emgu.CV;
using ImageCrusher.ImageController;
using ExtensionMethodsSpace;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MNET = MathNet.Numerics.LinearAlgebra.Double;


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
            for (int item = 0; item < nm; item++) 
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

            int[] nanList2 = new int[nanCount];                  // clean list of NaN pixels 
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
                                                                            // setting all_list - list of corrupted pix and yheir neighbours
            int[,] allList = new int[nanCount + neighboursList.GetLength(0), 3];
            for (; i < nanList.GetLength(0); i++)
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
            i = 0;j = 0;

            var InpRows = CreateInputsForSparseM(allList, n, 1);
            var InpCols = CreateInputsForSparseM(allList, m, 2);
            var r1 = InpRows.Item1;
            var r2 = InpRows.Item2;
            var r3 = InpRows.Item3;
            var c1 = InpCols.Item1;
            var c2 = InpCols.Item2;
            var c3 = InpCols.Item3;
                         //~~~!~~~~~~~~~~~~~!!!v                   ~~~~~~~~~~~~~~~~~~~~           sparse matrix
            string sparseString;
            int nL = r1.GetLength(0);
            if(nL>0)
            {
                MNET.SparseMatrix M = new MNET.SparseMatrix(m * n);
                for (i = 0; i < r1.GetLength(0); i++) 
                {
                    M[r1[i, 0], r2[i, 0]] = r3[i, 0];
                    M[r1[i, 1], r2[i, 1]] = r3[i, 1];
                    M[r1[i, 2], r2[i, 2]] = r3[i, 2];
                }
                sparseString = M.ToString();

                var M1 = MNET.SparseMatrix.OfMatrix(M);
                for (i = 0; i < c1.GetLength(0); i++)
                {
                    M1[c1[i, 0], c2[i, 0]] = c3[i, 0];
                    M1[c1[i, 1], c2[i, 1]] = c3[i, 1];
                    M1[c1[i, 2], c2[i, 2]] = c3[i, 2];
                }
                sparseString = M1.ToString();

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
            // creating list of neighbours
            #region
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
            #endregion
            // }

            //removing duplicates from neighs list
            #region
            int[,] neigboursListTemp = new int[neigboursList.GetLength(0), 3];
            for (int item = 0; item < neigboursList.GetLength(0); item++)
            {
                int val = neigboursList[item, 0];
                int originalOrDup = 0;

                for (int s=0; s< neigboursList.GetLength(0); s++)
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
            #endregion
            // removing duplicates compared to nanlist
            #region
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
            #endregion
            //making proper neighs list
            #region
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
            #endregion
            // making another arroy just for clean, non duplicated neigboursList
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

        public Tuple<int[,], int[,], int[,]> CreateInputsForSparseM(int[,] allList, int n_or_m, int row_or_cols)  // int n_or_m = n || m   int row_or_cols = 1 || 2 for rows 1 for cols 2  (if n then 1, if m then 2)
        {
            int n = 1;
            if (row_or_cols>1)
                n = imageIn.Height;

            int lCounter=0;
            int i;
            int[] lCounterArr = new int[allList.GetLength(0)];

            for (i =0; i<allList.GetLength(0); i++)
            {
                if (allList[i, row_or_cols] > 0 && allList[i, row_or_cols] < n_or_m - 1)
                {
                    lCounterArr[lCounter] = i;
                    lCounter++;
                }
            }

            int[] L = new int[lCounter];                            
            for (i=0; i < lCounter; i++)
                L[i] = lCounterArr[i];

            int[,] inp1 = new int[lCounter, 3];
            for (i=0; i < lCounter; i++)
            {
                inp1[i, 0] = allList[L[i], 0];
                inp1[i, 1] = allList[L[i], 0];
                inp1[i, 2] = allList[L[i], 0];
            }

            int[,] inp2 = new int[lCounter, 3];
            for (i = 0; i < lCounter; i++)
            {
                inp2[i, 0] = inp1[i, 2] - n;
                inp2[i, 1] = inp1[i, 2];
                inp2[i, 2] = inp1[i, 2] + n;
            }

            int[,] inp3 = new int[lCounter, 3];
            for (i = 0; i < lCounter; i++)
            {
                inp3[i, 0] = 1;
                inp3[i, 1] = -2;
                inp3[i, 2] = 1;
            }
            return new Tuple<int[,], int[,], int[,]>(inp1, inp2, inp3);
        }

    }
}
