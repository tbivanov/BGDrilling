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

        public static decimal[] Prod(decimal[,] A, decimal[] y)
        {
            decimal[] res = new decimal[A.GetLength(0)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                res[i] = 0;
                for (int j = 0; j < y.Length; j++)
                   res[i] += A[i,j]*y[j];
            }
            return res;
        }

        
        public static decimal[,] Prod(decimal[,] A, decimal[,] B)
        {
            int rows = A.GetLength(0);
            int cols = B.GetLength(1);
            decimal[,] res = new decimal[rows,cols];
            for(int i=0; i<rows; i++)
                for(int j=0; j<cols; j++)
                {
                    decimal sum = 0;
                    for (int k = 0; k < A.GetLength(1); k++)
                        sum += A[i, k] * B[k, j];
                    res[i, j] = sum;
                }
            
            return res;
        }

        public static decimal[,] Transpose(decimal[,] A)
        {
            int rows = A.GetLength(0);
            int cols = A.GetLength(1);
            decimal[,] res = new decimal [cols, rows];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    res[j, i] = A[i, j];
            return res;
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

        public static decimal Pow2(decimal x)
        {
            return x * x;
        }

        public static decimal[] Negative(decimal[] x)
        {
            for (int i = 0; i < x.Length; i++)
            {
                x[i]*=-1;
            }
            return x;
        }

        public static decimal Abs(decimal x)
        {
            if (x < 0)
                return -x;
            return x;
        }

    }
}
