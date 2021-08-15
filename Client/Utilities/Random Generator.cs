using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilities
{
    public class Random_Generator
    {
        public static byte[] GetByteArray(int size)
        {
            byte[] b = new byte[size]; // convert kb to byte
            Constants.Rand.NextBytes(b);
            return b;
        }
    }
}
