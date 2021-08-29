using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
