using System;
using System.Collections.Generic;
using System.IO;

namespace ExtensionMethodsSpace
{
    public static class ExtensionMethods
    {
        public static string DisplayDouble(this double value, int precision)
        {
            return value.ToString("N" + precision);
        }
        public static void SortByFirstColumn<T>(this T[,] array, IComparer<T> comparer = null)
        {
            // Indirect sorting
            var sortIndex = new int[array.GetLength(0)];
            for (int i = 0; i < sortIndex.Length; i++)
                sortIndex[i] = i;
            if (comparer == null) comparer = Comparer<T>.Default;
            Array.Sort(sortIndex, (a, b) => comparer.Compare(array[a, 0], array[b, 0]));
            // Reorder the array using "in situ" algorithm
            var temp = new T[array.GetLength(1)];
            for (int i = 0; i < sortIndex.Length; i++)
            {
                if (sortIndex[i] == i) continue;
                for (int c = 0; c < temp.Length; c++)
                    temp[c] = array[i, c];
                int j = i;
                while (true)
                {
                    int k = sortIndex[j];
                    sortIndex[j] = j;
                    if (k == i) break;
                    for (int c = 0; c < temp.Length; c++)
                        array[j, c] = array[k, c];
                    j = k;
                }
                for (int c = 0; c < temp.Length; c++)
                    array[j, c] = temp[c];
            }
        }

        public static void SaveArrayAsCSV(this Array arrayToSave, string filePath)
        {
            using (StreamWriter file = new StreamWriter(filePath))
            {
                foreach (object item in arrayToSave)
                {
                    file.Write(item + "," + Environment.NewLine);
                }
            }
        }
    }
}
