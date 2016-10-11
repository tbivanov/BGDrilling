using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGDrilling
{
    public static class LinearAlgebra
    {
        public static decimal[] Gauss (decimal[,] A, decimal[] y)
        {
            for (int i = 0; i < A.GetLength(0)-1; i++)
            {
                
            }

        }

        public static decimal[,] Cholesky (decimal[,] A)
        {
            int rows = A.GetLength(0);
            int cols = A.GetLength(1);
            decimal[,] L = new decimal[rows, cols];/*lower triangular*/

            /*if (rows==cols & MathDecimal.Transpose(B)=B)*/
            for (int k = 0; k < cols; k++)
            {
                decimal sum = 0;

                for (int p=0; p<=k-2; p++)
                    sum += L[k, p] * L[k, p];

                L[k, k] = MathDecimal.Sqrt(A[k, k] - sum);

                for( int j=k; j<cols; j++)
                {
                    sum = 0;

                    for (int p = 0; p <= k - 2; p++)
                        sum += L[k, p] * L[j, p];

                    A[j,k] = (A[k, j] - sum) / L[k, k];
                }
            }
           
            return L;
        }
    }
}
