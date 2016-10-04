using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGDrilling
{
    class MathDecimal
    {
        public const decimal PI = 3.14159265358979323846264338327950288419716939937510582097494459230781640628M;
        public static decimal Sqrt (decimal x)
        {
            return (decimal)Math.Sqrt((double)x);
        }
        public static decimal ASin(decimal x)
        {
            return (decimal)Math.Asin((double)x);
        }
        public static decimal ACos(decimal x)
        {
            return (decimal)Math.Acos((double)x);
        }
        public static decimal ATan2(decimal x, decimal y)
        {
            return (decimal)Math.Atan2((double)y, (double) x);
        }

        public static decimal[] Sum (decimal[] x, decimal[] y)
        {
            for (int i = 0; i < y.Length; i++)
                x[i] += y[i];
            return x;
        }

        public static decimal[] Prod(decimal a, decimal[] y)
        {
            for (int i = 0; i < y.Length; i++)
                y[i] *= a;
            return y;
        }

        public static decimal Norm2 (decimal[] x)
        {
            decimal res = 0;
            for (int i=0; i<x.Length; i++)
            {
                res += x[i] * x[i];
            }
            return Sqrt(res);
        }
    }
}
