using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMX地图工具 {
    public static class IListExtensions {
        public static T Random<T>( this IList<T> list, int start = 0 ) {
            return list[list.RandomIndex(start)];
        }

        public static int RandomIndex<T>( this IList<T> list, int start = 0 ) {

            var seed = Guid.NewGuid().GetHashCode();
            Random ran = new Random(seed);

            return ran.Next(start, list.Count);
        }
    }
}
