using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class NullChecker
    {
        public static void ThrowIfNull<T>(this T o, string parameName) where T : class
        {
            if (o == null)
                throw new ArgumentNullException(parameName);
        }
    }
}
