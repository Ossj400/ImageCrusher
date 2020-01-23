using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethodsSpace
{
    public static class ExtensionMethods
    {
        public static string DisplayDouble(this double value, int precision)
        {
            return value.ToString("N" + precision);
        }
    }
}
