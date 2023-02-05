using System.Collections.Generic;
using System.Linq;

namespace YapartMarket.Core.Extensions
{
    public static class EnumerableExpression
    {
        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }
    }
}
