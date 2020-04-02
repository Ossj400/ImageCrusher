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
using System.Diagnostics;

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
            ImageOutNans = new Image<Rgb, byte>(imageIn.Width, imageIn.Height);
        }
        public Nans(ImageMenu image)
        {
            imageIn = image.Img;
            imageOut = image.ImageOut;
            ImageOutNans = new Image<Rgb, byte>(imageIn.Width, imageIn.Height);
        }
        public Nans()
        {
        }

        public async Task /*void*/ Compute(int channel) //  channel = 0-2; // red=0, green=1, blue=2    
        {
            Process.GetCurrentProcess().MaxWorkingSet = new IntPtr(262144000);
            Process.GetCurrentProcess().MinWorkingSet = new IntPtr(209715200);
            int i = 0;
            int j = 0;
            int ij = 0;
            int nanCount = 0;
            int n = imageIn.Height;   // Data[x,y,channel]   x = n = rows     y = m = columns   
            int m = imageIn.Width;
            int nm = n * m;
            bool zero = false;

            int[] a = new int[nm];
            int[] k = new int[nm];
            for (; i < nm; i++)
            {
                a[i] = imageOut.Data[ij, j, channel];
                ij++;
                if (ij == imageOut.Height)
                {
                    ij = 0;
                    j++;

                    if (j == imageOut.Width)
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
                if (ij == imageOut.Height)
                {
                    ij = 0;
                    j++;
                    if (j == imageOut.Width)
                        j = 0;
                }
            }
            i = 0; j = 0;

            int[] nanList2 = new int[nanCount];                  // clean list of NaN pixels 
            double[] knownList = new double[nm - nanCount];
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
                if (zero == true)
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
                if (item != 0)
                {
                    nanList[j, 0] = item;
                    nanList[j, 1] = col;   
                    nanList[j, 2] = row;
                    j++;
                }
                i++;
                col++;
            }
            i = 0; j = 0; row = 0; col = 0; ij = 0;

            int[,] talks_to = new int[4, 2] { { -1, 0 }, { 0, -1 }, { -1, 1 }, { 0, 1 } };

            //setting all list
            int[,] allList = IdentifyNeighbours(n, m, nanList, talks_to);

            var InpRows = CreateInputsForSparseM(allList, n, 1);
            var InpCols = CreateInputsForSparseM(allList, m, 2);
            var r1 = InpRows.Item1;
            var r2 = InpRows.Item2;
            var r3 = InpRows.Item3;
            var c1 = InpCols.Item1;
            var c2 = InpCols.Item2;
            var c3 = InpCols.Item3;
            //~~~!~~~~~~~~~~~~~!!!v                   ~~~~~~~~~~~~~~~~~~~~           sparse matrix

            alglib.sparsematrix M;
            alglib.sparsecreate(m*n, m*n, out M);
            for (i = 0; i < r1.GetLength(0); i++)
            {
                alglib.sparseset(M, (r1[i, 0]), (r2[i, 0]), r3[i, 0]);
                alglib.sparseset(M, (r1[i, 1]), (r2[i, 1]), r3[i, 1]);
                alglib.sparseset(M, (r1[i, 2]), (r2[i, 2]), r3[i, 2]);
            }

            alglib.sparsematrix fda;
            alglib.sparsecopy(M, out fda);
            for (i = 0; i < c1.GetLength(0); i++)
            {
                alglib.sparseset(fda, (c1[i, 0]), (c2[i, 0]), c3[i, 0]);
                alglib.sparseset(fda, (c1[i, 1]), (c2[i, 1]), c3[i, 1]);
                alglib.sparseset(fda, (c1[i, 2]), (c2[i, 2]), c3[i, 2]);
            }
            row = 0; col = 0;

            int uselessCounter1 = 0;
            int uselessCounter2 = 0;
            row = 0;
            col = 0;
            double uselessNumb;

            while(alglib.sparseenumerate(fda, ref uselessCounter1, ref uselessCounter2, out row, out col, out uselessNumb) == true)
            {
                 if (alglib.sparseget(fda, row, col) == -2 && alglib.sparseget(M, row, col) == -2 && row > n && row < alglib.sparsegetnrows(fda) - n)
                     alglib.sparseset(fda, (row), col, (alglib.sparseget(M, row, col) + alglib.sparseget(fda, row, col)));                   
            }
            i = 0; j = 0;

            double value = 0;
            row = 0; col = 0;
            int nCols = alglib.sparsegetncols(fda);
            // transpose(fda);

            double[] colFda = new double[nCols];
            double[] rhs = new double[nm];
            for (col = 0; col < nCols; col++)
            {
                // sparsegetrow(col, colFda), 
                for (row = 0; row < knownList.Length; row++)
                    rhs[col] += a[(int)knownList[row]] * -colFda[(int)knownList[row]]; //-alglib.sparseget(fda, col, (int)knownList[row]);
            }
            i = 0; row = 0;col = 0;


            // transpose(fda);
            double[] rhs2 = new double[nm];
            alglib.sparsemtv(fda, knownList,ref rhs2);

            int fda0Count = 0;
            while (alglib.sparseenumerate(fda, ref uselessCounter1, ref uselessCounter2, out row, out col, out uselessNumb))
                fda0Count++;

            i = 0; j = 0; row = 0; col = 0;
            double[,] FdaNon0Arr = new double[fda0Count, 3];
            while (alglib.sparseenumerate(fda, ref uselessCounter1, ref uselessCounter2, out row, out col, out uselessNumb))
            {
                FdaNon0Arr[i, 2] = uselessNumb;
                FdaNon0Arr[i, 1] = row;
                FdaNon0Arr[i, 0] = col;
                i++;
            }
            FdaNon0Arr.SortByFirstColumn();
            double temp1 = 0;
            double temp2 = 0;
            double temp3 = 0;
            for (i = 0; i < fda0Count - 1; i++)   // sorting 
            {
                if (FdaNon0Arr[i, 0] == FdaNon0Arr[i + 1, 0] && FdaNon0Arr[i, 1] > FdaNon0Arr[i + 1, 1])
                {
                    temp1 = FdaNon0Arr[i, 0];
                    temp2 = FdaNon0Arr[i, 1];
                    temp3 = FdaNon0Arr[i, 2];
                    FdaNon0Arr[i, 0] = FdaNon0Arr[i + 1, 0];
                    FdaNon0Arr[i, 1] = FdaNon0Arr[i + 1, 1];
                    FdaNon0Arr[i, 2] = FdaNon0Arr[i + 1, 2];
                    FdaNon0Arr[i + 1, 0] = temp1;
                    FdaNon0Arr[i + 1, 1] = temp2;
                    FdaNon0Arr[i + 1, 2] = temp3;
                    if (i > 1)
                        i = i - 1;
                    if (i > 2)
                        i = i - 1;
                    if (i > 3)
                        i = i - 3;
                }
            }
            i = 0; j = 0;
            alglib.sparsematrix fdaNan;
            alglib.sparsecreate(m * n, nanList2.Length, out fdaNan);
            for (; i < 11; i++)
                //for (; i < alglib.sparsegetnrows(fda) * alglib.sparsegetncols(fda); i++)
            {
                alglib.sparseset(fdaNan, i, j, alglib.sparseget(fda, i, nanList2[j]));
                if (i == alglib.sparsegetnrows(fdaNan) - 1)
                {
                    i = -1;
                    j++;
                    if (j == nanList2.Length)
                        break;
                }
            }      

            alglib.sparsematrix fdaNan2;
            alglib.sparsecreate(m * n, nanList2.Length, out fdaNan2);
            ij = 0;
            for(i=0;i<FdaNon0Arr.GetLength(0);i++)
            {
                if (FdaNon0Arr[i, 0] == nanList2[ij])
                {
                    alglib.sparseset(fdaNan2, (int)FdaNon0Arr[i, 1], ij, alglib.sparseget(fda, (int)FdaNon0Arr[i, 1], nanList2[ij]));
                }
                if (FdaNon0Arr[i, 0] > nanList2[ij] && ij < nanList2.Length)
                {
                    if (ij < nanList2.Length-1)
                        ij++;                           
                } 
            }
            row = 0; ij = 0;
            ////////// Comparing FdaNan with FdaNan2
            while (alglib.sparseenumerate(fdaNan, ref uselessCounter1, ref uselessCounter2, out row, out col, out uselessNumb))
            {
                ij++;
            }
            ij = 0;
            while (alglib.sparseenumerate(fdaNan2, ref uselessCounter1, ref uselessCounter2, out row, out col, out uselessNumb))
            {
                ij++;
            }
            ij = 0;
       
            row = 0;
            int fdaNonZero =0;
            i = 0; j = 0;
            alglib.sparsematrix fdaAny;
            alglib.sparsecreate(m * n, 1, out fdaAny);
            while (alglib.sparseenumerate(fdaNan2, ref uselessCounter1, ref uselessCounter2, out row, out col, out uselessNumb))
            {
                alglib.sparseset(fdaAny, row, 0, 1);
            }
            while (alglib.sparseenumerate(fdaAny, ref uselessCounter1, ref uselessCounter2, out row, out col, out uselessNumb))
            {
                fdaNonZero++;
            }

            int[] kNew = new int[fdaNonZero];
            for (i = 0; i < alglib.sparsegetnrows(fdaAny); i++)
            {
                if (alglib.sparseget(fdaAny,i, 0) > 0)
                {
                    kNew[j] = i;
                   j++;
                }
            }
            j = 0;

            // ---------------------------------------------------------------------------------------------------==================
            alglib.sparse.sparsematrix solvingInputA = new alglib.sparse.sparsematrix();
            alglib.sparse.sparsecreate(kNew.Length, nanCount, fdaNonZero, solvingInputA, default);
            int nRows = alglib.sparse.sparsegetnrows(solvingInputA, default);
            nCols = alglib.sparse.sparsegetncols(solvingInputA, default);
            for (col = 0; col < nCols; col++)
            {
                for (row = 0; row < nRows; row++)                
                   alglib.sparse.sparseset(solvingInputA, row, col, (alglib.sparseget(fda, kNew[row], nanList[col, 0])), default);
            }
            ij = 0; j = 0;
            //------------------------------------------------------------------====================================================

            double[] solvingInputB = new double[kNew.Length];
            for (i = 0; i < kNew.Length; i++)
            {
                solvingInputB[i]= rhs[kNew[i]];               
            }

            double[] solve = new double[kNew.Length];
            alglib.sparsematrix solvingInputA1 = new alglib.sparsematrix(solvingInputA);
            alglib.linlsqrstate linlsqrstate;
            alglib.linlsqrcreate(solvingInputA.m, nanCount, out linlsqrstate);
            alglib.xparams _params = default;
            alglib.sparse.sparseconverttocrs(solvingInputA, default);
            alglib.linlsqrsolvesparse(linlsqrstate, solvingInputA1, solvingInputB, _params: _params);
            alglib.linlsqrreport report = new alglib.linlsqrreport();
            alglib.linlsqrresults(linlsqrstate, out solve, out report);

            i = 0;
            foreach(double item in solve)
            {
                solve[i] = Math.Round(item);
                if (solve[i] > 255)
                    solve[i] = 255;
                if (solve[i] < 0)
                    solve[i] = 0;
                i++;
            }

            double[] finalImgArr = new double[a.Length];
            int ijk = 0;
            ij = 0; j = 0;
            for (i = 0; i < nm; i++)
            {               
                if (i != nanList2[ijk])
                {
                    imageOutNans.Data[ij, j, channel] = imageOut.Data[ij, j, channel];
                    finalImgArr[i] = imageOutNans.Data[ij, j, channel];                       
                    ij++;
                }
                if (ij == imageOut.Height)
                {
                    ij = 0;
                    j++;
                    if (j == imageOut.Width)
                        j = 0;
                }
                if (i == nanList2[ijk])
                {
                    imageOutNans.Data[ij, j, channel] = (byte)solve[ijk];
                    finalImgArr[i] = imageOutNans.Data[ij, j, channel];
                    ij++;
                    ijk++;
                    if (ijk == nanList2.Length)
                        ijk = 0;
                }
                if (ij == imageOut.Height)
                {
                    ij = 0;
                    j++;
                    if (j == imageOut.Width)
                        j = 0;
                }

            }

            ///// testing solve                         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            ///// 
            i = 0; j = 0; ij = 0;
            double[] originalInputImg = new double[a.Length];
            for (; i < nm; i++)
            {
                originalInputImg[i] = imageIn.Data[ij, j, channel];
                ij++;
                if (ij == imageOut.Height)
                {
                    ij = 0;
                    j++;

                    if (j == imageOut.Width)
                        j = 0;
                }
            }
            solve.SaveArrayAsCSV("C:/Users/Artur/Downloads/zdj/SolvedArrChannel_" + channel + ".csv");
            finalImgArr.SaveArrayAsCSV("C:/Users/Artur/Downloads/zdj/FinalImgArray" + channel + ".csv");
            a.SaveArrayAsCSV("C:/Users/Artur/Downloads/zdj/A_" + channel + ".csv");
            originalInputImg.SaveArrayAsCSV("C:/Users/Artur/Downloads/zdj/OriginalInpuArray_" + channel + ".csv");
        }

        public int[,] IdentifyNeighbours(int n, int m, int[,] nanList, int[,] talks_to)
        {
            int nanCount = nanList.GetLength(0);
            int talkCount = talks_to.GetLength(0);
            int[,] nn = new int[(nanCount * talkCount), 2];
            int[] j = new int[] { 0, nanCount };
            int ij = 0;
            int i = 0;
            int ik = 0;
            int row = 0;
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
                    nn[z, 1] = nanList[ik, 2] - 1;

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
                    if ((nanList[ik, 2] + 1) > m)
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

                    nn[z, 1] = nanList[ik, 2] + 1;
                    if ((nanList[ik, 1] + 1) > n)
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

            for (; ik < nn.Length / 2; ik++)
            {
                if (nn[ik, 0] >= 0 && nn[ik, 1] >= 0 && nn[ik, 0] < n && nn[ik, 1] < m)
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
                    if (m == row)
                    {
                        row = 0;
                    }
                }
                if (ik == nn1.GetLength(0))
                    break;
                if (nn1[ik, 1] > 0)
                {
                    neigboursList[ik, 0] = ((nn1[ik, 1] + 1) * (n) + (nn1[ik, 0] - (n)));
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


            int[] newNeighs = new int[nn1.GetLength(0)];
            for (i=0; i < newNeighs.Length; i++)
            {
                newNeighs[i] = neigboursList[i, 0];
            }
            int[] dist0 = newNeighs.Distinct().ToArray();
            Array.Sort(dist0);

            int[] newNeighs2 = new int[nn1.GetLength(0)+nanList.GetLength(0)];
            ij = 0;
            for (i = 0; i < newNeighs2.Length; i++)
            {
                if (i >= nanList.GetLength(0) && ij <dist0.Length)
                {
                    newNeighs2[i] = dist0[ij];
                    ij++;
                }
                if(i<nanList.GetLength(0))
                    newNeighs2[i] = nanList[i, 0];
            }
            int[] dist2 = newNeighs2.Distinct().ToArray();

            int[,] neigboursListSorted = new int[dist2.Length, 3];
            row = 0; col = 0;
            for (i=0; i < dist2.Length; i++)
            {
                if (i >= nanList.GetLength(0))
                {

                    if (dist2[i] > imageOut.Height)
                    {
                        col = (int)dist2[i] / imageOut.Height;
                    }
                    if (dist2[i] > imageOut.Height)
                    {                       
                        row = Math.Abs((col * imageOut.Height) - dist2[i]);
                    }
                    else
                        row = dist2[i];

                    neigboursListSorted[i, 0] = dist2[i];
                    neigboursListSorted[i, 1] = row;
                    neigboursListSorted[i, 2] = col;                
                }
                else
                {
                    neigboursListSorted[i, 0] = nanList[i, 0];
                    neigboursListSorted[i, 1] = nanList[i, 1];
                    neigboursListSorted[i, 2] = nanList[i, 2];
                }

            }

            return neigboursListSorted;
        }

        public Tuple<int[,], int[,], int[,]> CreateInputsForSparseM(int[,] allList, int n_or_m, int row_or_cols)  // int n_or_m = n || m   int row_or_cols = 1 || 2 for rows 1 for cols 2  (if n then 1, if m then 2)
        {
            int n = 1;
            if (row_or_cols > 1)
                n = imageIn.Height;

            int lCounter = 0;
            int i;
            int[] lCounterArr = new int[allList.GetLength(0)];

            for (i = 0; i < allList.GetLength(0); i++)
            {
                if (allList[i, row_or_cols] > 0 && allList[i, row_or_cols] < n_or_m - 1)
                {
                    lCounterArr[lCounter] = i;
                    lCounter++;
                }
            }

            int[] L = new int[lCounter];
            for (i = 0; i < lCounter; i++)
                L[i] = lCounterArr[i];

            int[,] inp1 = new int[lCounter, 3];
            for (i = 0; i < lCounter; i++)
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

        public void Compute2(int channel) //  channel = 0-2; // red=0, green=1, blue=2     public async Task Compute(int channel)
        {
            int i = 0;
            int j = 0;
            int ij = 0;
            int nanCount = 0;
            int n = imageIn.Height;   // Data[x,y,channel]   x = n = rows     y = m = columns   
            int m = imageIn.Width;
            int nm = n * m;
            double value = 0;

            int[] a = new int[nm];
            int[] k = new int[nm];
            for (int item = 0; item < nm; item++)
            {
                if (imageOut.Data[ij, j, channel] != imageIn.Data[ij, j, channel])
                {
                    value = 0;
                    int toBoundCol = imageOut.Width - j;
                    int toBoundRow = imageOut.Height - ij;
                    if (ij > 0 && ij < imageOut.Height - 1 && j > 0 && j < imageOut.Width - 1)
                    {
                        int valCount = 1;
                        if ((imageOut.Data[ij + 1, j, channel]) == imageIn.Data[ij + 1, j, channel] && (imageOut.Data[ij - 1, j, channel]) == imageIn.Data[ij - 1, j, channel] && imageOut.Data[ij, 1 + j, channel] == imageIn.Data[ij, 1 + j, channel] && (imageOut.Data[ij, j - 1, channel]) == imageIn.Data[ij, j - 1, channel] && (imageOut.Data[ij-1, j - 1, channel]) == imageIn.Data[ij-1, j - 1, channel] && imageOut.Data[ij + 1, j - 1, channel] == imageIn.Data[ij + 1, j - 1, channel])
                        {
                            value += imageIn.Data[ij + 1, j - 1, channel] + imageIn.Data[ij - 1, j - 1, channel] + imageOut.Data[ij + 1, j, channel] + imageOut.Data[ij - 1, j, channel] + imageOut.Data[ij, j + 1, channel] + imageOut.Data[ij, j - 1, channel];
                            value = value / 6/ valCount;
                            valCount++;
                        }

                        else if((imageOut.Data[ij + 1, j, channel]) == imageIn.Data[ij + 1, j, channel] && (imageOut.Data[ij - 1, j, channel]) == imageIn.Data[ij - 1, j, channel] && imageOut.Data[ij, 1 + j, channel] == imageIn.Data[ij, 1 + j, channel] && (imageOut.Data[ij, j - 1, channel]) == imageIn.Data[ij, j - 1, channel] && (imageOut.Data[ij - 1, j - 1, channel]) == imageIn.Data[ij - 1, j - 1, channel] && imageOut.Data[ij -1, j + 1, channel] == imageIn.Data[ij - 1, j + 1, channel])
                        {
                            value += imageIn.Data[ij - 1, j + 1, channel] + imageIn.Data[ij - 1, j - 1, channel] + imageOut.Data[ij + 1, j, channel] + imageOut.Data[ij - 1, j, channel] + imageOut.Data[ij, j + 1, channel] + imageOut.Data[ij, j - 1, channel];
                            value =( value / 6 )/ valCount;
                            valCount++;
                        }

                        else if ((imageOut.Data[ij + 1, j, channel]) == imageIn.Data[ij + 1, j, channel] && (imageOut.Data[ij - 1, j, channel]) == imageIn.Data[ij - 1, j, channel] && imageOut.Data[ij, 1 + j, channel] == imageIn.Data[ij, 1 + j, channel] && (imageOut.Data[ij, j - 1, channel]) == imageIn.Data[ij, j - 1, channel])
                        {
                            value += imageOut.Data[ij + 1, j, channel] + imageOut.Data[ij - 1, j, channel] + imageOut.Data[ij, j + 1, channel] + imageOut.Data[ij, j - 1, channel];
                            value = (value / 4)/ valCount;
                            valCount++;
                        }
                        else if (imageOut.Data[ij + 1, j, channel] == imageIn.Data[ij + 1, j, channel] && imageOut.Data[ij - 1, j, channel] == imageIn.Data[ij - 1, j, channel] && imageOut.Data[ij, 1 + j, channel] == imageIn.Data[ij, 1 + j, channel])
                        {
                            value += imageOut.Data[ij + 1, j, channel] + imageOut.Data[ij - 1, j, channel] + imageOut.Data[ij, j + 1, channel];
                            value = value / 3/ valCount;
                            valCount++;
                        }
                        else if (imageOut.Data[ij + 1, j, channel] == imageIn.Data[ij + 1, j, channel] && imageOut.Data[ij - 1, j, channel] == imageIn.Data[ij - 1, j, channel] && imageOut.Data[ij, j - 1, channel] == imageIn.Data[ij, j - 1, channel])
                        {
                            value += imageOut.Data[ij + 1, j, channel] + imageOut.Data[ij - 1, j, channel] + imageOut.Data[ij, j - 1, channel];
                            value = value / 3 / valCount;
                            valCount++; 
                        }
                        else if (imageOut.Data[ij + 1, j, channel] == imageIn.Data[ij + 1, j, channel] && imageOut.Data[ij - 1, j, channel] == imageIn.Data[ij - 1, j, channel] && imageOut.Data[ij, j + 1, channel] == imageIn.Data[ij, j + 1, channel])
                        {
                            value += imageOut.Data[ij + 1, j, channel] + imageOut.Data[ij - 1, j, channel] + imageOut.Data[ij, j - 1, channel];
                            value = value / 3 / valCount;
                            valCount++; ;
                        }
                        else if (imageOut.Data[ij + 1, j, channel] == imageIn.Data[ij + 1, j, channel] && imageOut.Data[ij - 1, j, channel] == imageIn.Data[ij - 1, j, channel])
                        {
                            value += imageOut.Data[ij + 1, j, channel] + imageOut.Data[ij - 1, j, channel];
                            value = value / 2 / valCount;
                            valCount++; ;
                        }
                        else if (imageOut.Data[ij + 1, j, channel] == imageIn.Data[ij + 1, j, channel] && imageOut.Data[ij + 1, j, channel] == imageIn.Data[ij + 1, j, channel])
                        {
                            value += imageOut.Data[ij + 1, j, channel] + imageOut.Data[ij + 1, j, channel];
                            value = value / 2 / valCount;
                            valCount++; 
                        }
                        else if (imageOut.Data[ij - 1, j, channel] == imageIn.Data[ij - 1, j, channel] && imageOut.Data[ij + 1, j, channel] == imageIn.Data[ij + 1, j, channel])
                        {
                            value += imageOut.Data[ij - 1, j, channel] + imageOut.Data[ij + 1, j, channel];
                            value = value / 2 / valCount;
                            valCount++; 
                        }
                        else if (imageOut.Data[ij + 1, j, channel] == imageIn.Data[ij + 1, j, channel] && imageOut.Data[ij - 1, j, channel] == imageIn.Data[ij - 1, j, channel])
                        {
                            value += imageOut.Data[ij + 1, j, channel] + imageOut.Data[ij - 1, j, channel];
                            value = value / 2 / valCount;
                            valCount++; 
                        }
                        else if (imageOut.Data[ij + 1, j, channel] == imageIn.Data[ij + 1, j, channel])
                        {
                            value += imageOut.Data[ij + 1, j, channel];
                            value = value / valCount;
                            valCount++;
                        }
                        else if (imageOut.Data[ij - 1, j, channel] == imageIn.Data[ij - 1, j, channel])
                        {
                            value += imageOut.Data[ij - 1, j, channel];
                            value = value / valCount;
                            valCount++;
                        }
                        else if ( imageOut.Data[ij , j-1, channel] == imageIn.Data[ij, j-1, channel])
                        {
                            value += imageOut.Data[ij, j-1, channel];
                            value = value / valCount;
                            valCount++;
                        }
                        else if (imageOut.Data[ij, j + 1, channel] == imageIn.Data[ij, j + 1, channel])
                        {
                            value +=  imageOut.Data[ij, j + 1, channel];
                            value = value / valCount;
                            valCount++;
                        }
                        if (valCount > 2)
                            valCount = 0;
                        else if(valCount==1)
                        {
                            value = 0;
                            valCount = 0;
                            double c = toBoundCol / m;
                            double r = toBoundRow / n;
                            double de = toBoundCol * c;
                            double df = (toBoundRow * r);
                            int e = (int)de;
                            int f = (int)df;                          

                            for (int bound = 0; bound < toBoundCol; bound++)
                                if (imageOut.Data[ij, j + bound, channel] == imageIn.Data[ij, j + bound, channel] && valCount < (m / 10))
                                {
                                    value += imageOut.Data[ij, j + bound, channel];
                                    valCount++;
                                    break;
                                }
                            for (int bound = 0; bound < toBoundRow;  bound++)
                            {
                                if (imageOut.Data[ij + bound, j, channel] == imageIn.Data[ij + bound, j, channel] && valCount < (n / 10))
                                {
                                    value += imageOut.Data[ij + bound, j, channel];
                                    valCount++;
                                    break;
                                }
                            }
                            for (int bound = 0; bound < f; bound++)
                            {
                                if (imageOut.Data[ij + bound, j, channel] == imageIn.Data[ij + bound, j, channel])
                                {
                                    value += imageOut.Data[ij + bound, j, channel];
                                    valCount++;
                                }
                            }
                            for (int bound = 0; bound < e; bound++)
                            {
                                if (imageOut.Data[ij, j + bound, channel] == imageIn.Data[ij, j + bound, channel])
                                {
                                    value += imageOut.Data[ij, j + bound, channel];
                                    valCount++;
                                }
                            }
                            for (int bound = 0; bound < f; bound++)
                            {
                                if (imageOut.Data[ij - bound, j, channel] == imageIn.Data[ij - bound, j, channel])
                                {
                                    value += imageOut.Data[ij - bound, j, channel];
                                    valCount++;
                                }
                            }
                            for (int bound = 0; bound < e; bound++)
                            {
                                if (imageOut.Data[ij, j - bound, channel] == imageIn.Data[ij, j - bound, channel])
                                {
                                    value += imageOut.Data[ij, j - bound, channel];
                                    valCount++;
                                }
                            }
                            value = value / valCount;
                        }
                    }
                    else
                    {
                        value = 0;
                        int valCount = 0;
                        double c = toBoundCol / m;
                        double r = toBoundRow / n;
                        double de = toBoundCol * c;
                        double df = (toBoundRow *r );
                        int e = (int)de;
                        int f = (int)df;
                        for (int bound = 0; bound < f; bound++)
                            if (imageOut.Data[ij + bound, j, channel] == imageIn.Data[ij + bound, j, channel])
                            { 
                                value += imageOut.Data[ij + bound, j, channel]; valCount++;
                                valCount++;
                            }
                        for (int bound = 0; bound < e; bound++)
                            if (imageOut.Data[ij, j + bound, channel] == imageIn.Data[ij, j + bound, channel])
                            {
                                value += imageOut.Data[ij, j + bound, channel];
                                valCount++;
                            }

                        for (int bound = 0; bound < toBoundCol; bound++)
                        {
                            if (imageOut.Data[ij, j + bound, channel] == imageIn.Data[ij, j + bound, channel] && valCount < (m / 10))
                            {
                                value += imageOut.Data[ij, j + bound, channel];
                                valCount++;
                                break;
                            }
                        }

                        value = value / valCount;
                    }
                
                    imageOutNans.Data[ij, j, channel] = (byte)(value);
                   
                    
                }
                i++;
                ij++;
                if (ij == imageOut.Height)
                {
                    ij = 0;
                    j++;
                    if (j == imageOut.Width)
                        j = 0;
                }
            }
            i = 0; j = 0;
        }
    }
}
