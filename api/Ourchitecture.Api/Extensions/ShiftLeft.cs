using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ourchitecture.Api
{
    public static partial class Extensions
    {
        public static List<T> ShiftLeft<T>(this List<T> list, int shift)
        {
            var shiftedList = new List<T>();

            for (int i = 0; i < list.Count; i++)
            {
                var pos = (i + shift) % list.Count;

                shiftedList.Add(list[pos]);
            }

            return shiftedList;
        }
    }
}