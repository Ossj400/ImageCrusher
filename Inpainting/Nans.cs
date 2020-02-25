using System;
using Emgu.CV.Structure;
using Emgu.CV;
using ImageCrusher.ImageController;
using ExtensionMethodsSpace;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MNET = MathNet.Numerics.LinearAlgebra.Double;
using ACMATH = Accord.Math;
using System.Threading.Tasks;

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
            ImageOutNans = new Image<Rgb, byte>(image.ImageOut.ToBitmap());
        }

        public Nans()
        {
        }

        public void Compute(int channel) //  channel = 0-2; // red=0, green=1, blue=2     public async Task Compute(int channel)
        {
            int i = 0;
            int j = 0;
            int ij = 0;         
            int nanCount = 0;           
            int n = imageIn.Height;   // Data[x,y,channel]   x = n = rows     y = m = columns   
            int m = imageIn.Width;
            int nm = n*m;
            bool zero = false;

            int[] a = new int[nm];
            int[] k = new int[nm]; 
            for(;i<nm; i++)
            {
                a[i] = imageOut.Data[ij, j, channel];
                ij++;
                if (ij == imageOut.Rows)
                {
                    ij = 0;
                    j++;

                    if (j == imageOut.Cols)
                        j = 0;
                }
            }
            i = 0; j = 0; ij = 0;

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
            string sparseString;  // for compare
            int nL = r1.GetLength(0);
            MNET.SparseMatrix M = new MNET.SparseMatrix(m * n);    // = fda
            
            for (i = 0; i < r1.GetLength(0); i++) 
            {
                M[r1[i, 0], r2[i, 0]] = r3[i, 0];
                M[r1[i, 1], r2[i, 1]] = r3[i, 1];
                M[r1[i, 2], r2[i, 2]] = r3[i, 2];
            }
            sparseString = M.ToString();
            var fda = MNET.SparseMatrix.OfMatrix(M);        // = fda
            for (i = 0; i < c1.GetLength(0); i++)
            {
                fda[c1[i, 0], c2[i, 0]] = c3[i, 0];
                fda[c1[i, 1], c2[i, 1]] = c3[i, 1];
                fda[c1[i, 2], c2[i, 2]] = c3[i, 2];
            }
            row = 0; col = 0;

            var fdaReal = new MNET.SparseMatrix(nm);
            for(row=0; row<fdaReal.ColumnCount*fdaReal.RowCount;row++)
            {
                if (fda[row,col] ==-2 && M[row, col] ==-2 && row>n && row<fda.RowCount - n) 
                {                   
                    fda[row,col]= M[row, col] + fda[row, col];
                }
                if (row == fdaReal.RowCount - 1)
                {
                    row = 0; col++;
                    if (col == fdaReal.ColumnCount - 1)
                        break;
                }
            }
            i = 0;
            sparseString = fda.ToString();  // just for comparing with matlab         
           
            var rhsSparse = fda.SubMatrix(0, m * n, 0, knownList.Length);  // to make a minus : -M1.SubMatrix(0, m * n, 0, knownList.Length);
            rhsSparse.Clear();
            
            
            var rhsSparseArr = rhsSparse.ToArray();
            for (i=0; i<rhsSparse.RowCount; i++)
            {
                rhsSparseArr[i,j] = fda[i, knownList[j]];    
                if(i==rhsSparse.RowCount-1)
                {
                    i = -1;
                    j++;
                    if (j == rhsSparse.ColumnCount)
                        break;
                }
            }
            j = 0; col = 0; row = 0;
            double value = 0;
            
            double[] rhs = new double[nm];
            for (i = 0; i < nm; i++)
            {
                for(row = 0; row<rhsSparseArr.GetLength(1); row++)
                {
                    value += (-rhsSparseArr[col, row]) *a[knownList[row]];
                    if (row == rhsSparseArr.GetLength(1)-1)
                        col++; 
                }
                rhs[i] = value;                                
                value = 0; 
            }
            i = 0;j = 0;

            MNET.SparseMatrix fdaNan = new MNET.SparseMatrix(nm, nanList2.Length);    
            for (; i < fda.RowCount * fda.ColumnCount; i++)
            {
                fdaNan[i, j] = fda[i, nanList2[j]];
                if (i == fdaNan.RowCount - 1)
                {
                    i = -1;
                    j++;
                    if (j == nanList2.Length)
                        break;
                }
            }
            i = 0; j = 0; ij = 0;

            MNET.SparseMatrix fdaAny = new MNET.SparseMatrix(nm, 1);
            for (; i < fdaNan.RowCount * fdaNan.ColumnCount ; i++)
            {
                if (i == fdaNan.RowCount - 1)
                {
                    i = 0;
                    j++;
                    if (j == nanList2.Length)
                        break;
                }
                if(fdaNan[i, j] !=0)
                {
                    fdaAny[i, 0] = 1;
                        ij++;
                }
            }
            j = 0;

            int[] kNew = new int[fdaAny.NonZerosCount];
            for(i=0; i<fdaAny.RowCount; i++)
            {
                if (fdaAny[i, 0] > 0)
                {
                    kNew[j] = i;
                    j++;
                }
            }
            j = 0;

            var solvingInputA = new MNET.SparseMatrix(kNew.Length, nanCount);
            for(i=0; j<solvingInputA.RowCount ;i++)
            {
                solvingInputA[i,j] = fda[kNew[i], nanList[j, 0]];
                if(i==solvingInputA.RowCount-1)
                {
                    i = -1;                                       
                    if (j == solvingInputA.ColumnCount - 1)
                        break;
                    j++;
                }
            }

            // same as pseudoinverse (solvingInputA.Transpose() * solvingInputA).Inverse();
               //var solvingInputA_Arr = solvingInputA.PseudoInverse().ToArray();
            var solvingInputA_Arr = ACMATH.Matrix.PseudoInverse(solvingInputA.ToArray());
            int[] solvingInputB = new int[kNew.Length]; 
            for (i=0; i<kNew.Length;i++)
            {
                solvingInputB[i] = (int)rhs[kNew[i]];                  /// cast int is ok coz all rhs are Integers 
            }
            //solve
            for (i = 0; i < kNew.Length; i++)
            {
                solvingInputB[i] = (int)rhs[kNew[i]];                  /// cast int is ok coz all rhs are Integers 
            }
            value = 0; row = 0; col = 0; ij = 0;
            
            int[] solve = new int[a.Length];
            for (i = 0; i < a.Length; i++)
            {
                solve[i] = a[i];
                if (k[i] > 0)
                {
                    for (row = 0; row < solvingInputB.Length; row++)
                    {
                        value += solvingInputA_Arr[ij, row] * solvingInputB[row];
                    }
                    ij++;
                    solve[i] = (int)Math.Round(value,MidpointRounding.ToEven);
                    value = 0;
                }
            }

            ij = 0; j = 0;
            for (i=0; i < nm; i++)
            {
                imageOutNans.Data[ij, j, channel] = (byte) solve[i];
                ij++;
                if (ij == imageOut.Rows)
                {
                    ij = 0;
                    j++;

                    if (j == imageOut.Cols)
                        j = 0;
                }
            }

        }

        public Tuple<int[,], int[,]> IdentifyNeighbours(int n, int m, int[,] nanList, int[,] talks_to)
        {
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
                if (ik == nn1.GetLength(0))                  
                    break;
                if (nn1[ik, 1] > 0)
                {
                    neigboursList[ik, 0] = ((nn1[ik, 1]+1) * (n) + (nn1[ik, 0] - (n)));
                    neigboursList[ik, 1] = nn1[ik, 0];
                    neigboursList[ik, 2] = nn1[ik, 1];
                    ik++;
                    
                }
                if (ik == nn1.GetLength(0))                  
                    break;
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
