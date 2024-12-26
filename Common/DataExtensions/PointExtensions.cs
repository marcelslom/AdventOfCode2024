using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataExtensions
{
    public static class PointExtensions
    {

        public static bool InBounds<T>(this Point point, T[][] array)
        {
            return point.Y >= 0 && point.Y < array.Length && point.X >= 0 && point.X < array[point.Y].Length;
        }

    }
}
