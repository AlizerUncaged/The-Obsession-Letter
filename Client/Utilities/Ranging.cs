using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilities
{
    public static class Ranging
    {
        public static bool IsInEnumRange<T>(int index) {
            return Enum.IsDefined(typeof(T), index);
        }
    }
}
