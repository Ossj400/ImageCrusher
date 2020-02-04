using System;

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
