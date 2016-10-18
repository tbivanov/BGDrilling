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
            decimal l, sum, temp;
            decimal[] res = new decimal[y.Length];
            int n = A.GetLength(0);
            for (int i = 0; i < n-1; i++)
            {
                int maxIndex = i;
                //Find the index of the row with maximal element
                for (int j = i + 1; j < n; j++)
                    if (MathDecimal.Abs(A[j, i]) > MathDecimal.Abs(A[maxIndex, i]))
                        maxIndex = j;
                //Change the rows with indices i and maxIndex
                for(int j=i; j<n; j++)
                { 
                    temp = A[i, j];
                    A[i, j] = A[maxIndex, j];
                    A[maxIndex, j] = temp;
                }
                temp = y[i];
                y[i] = y[maxIndex];
                y[maxIndex] = temp;
                //Eliminate the elements under the main diagonal in the i-th column
                for (int j = i + 1; j < n; j++)
                {
                    l = A[j, i] / A[i, i];
                    A[j, i] = 0;
                    for (int k = i + 1; k < n; k++)
                        A[j, k] -= l * A[i, k];
                    y[j] -= l * y[i];
                }
            }
            //Backward substitution
            for (int i = n - 1; i >= 0; i--)
            {
                sum = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sum += A[i, j] * res[j];
                }
                res[i] = (y[i] - sum) / A[i, i];
            }
            return res;

        }

        public static decimal[,] Cholesky (decimal[,] A)
        {
            int rows = A.GetLength(0);
            int cols = A.GetLength(1);
            decimal sum;
            decimal[,] L = new decimal[rows, cols];/*lower triangular*/

            /*if (rows==cols & MathDecimal.Transpose(B)=B)*/
            for (int k = 0; k < cols; k++)
            {
                sum = 0;

                for (int p=0; p<k; p++)
                    sum += L[k, p] * L[k, p];

                L[k, k] = MathDecimal.Sqrt(A[k, k] - sum);

                for( int j=k; j<cols; j++)
                {
                    sum = 0;

                    for (int p = 0; p < k; p++)
                        sum += L[k, p] * L[j, p];

                    L[j,k] = (A[k, j] - sum) / L[k, k];
                }
            }
           
            return L;
        }

        public static decimal[] BackwardSubstitutionUpp(decimal[,] A, decimal[] y)
        {
            decimal sum;
            decimal[] res = y;
            int n = A.GetLength(0);

            //if A is upper triangular
            // * * *
            // 0 * *
            // 0 0 *

            for (int i = n - 1; i >= 0; i--)
            {
                sum = 0;
                for (int j = i + 1; j < n; j++)
                    sum += A[i, j] * res[j];
                
                res[i] = (y[i] - sum) / A[i, i];
            }

            return res;
        }

        public static decimal[] BackwardSubstitutionLow(decimal[,] A, decimal[] y)
        {
            decimal sum;
            decimal[] res = y;
            int n = A.GetLength(0);

            //if A is upper triangular
            // * 0 0
            // * * 0
            // * * *

            for (int i = 0; i < n; i++)
            {
                sum = 0;
                for (int j = 0; j < i; j++)
                    sum += A[i, j] * res[j];

                res[i] = (y[i] - sum) / A[i, i];
            }

            return res;
        }

        public static decimal[] JacobiMethod(decimal[,] A, decimal accuracy)
        {
           throw new NotImplementedException();
           int rows=A.GetLength(0);
           int cols=A.GetLength(1);

           decimal sigma = 0;
           int i_max = 0, j_max = 1;

           /*m==n and A symmetric*/ 
           while (sigma>accuracy)
           {
               /*find max off-diagonal element */
               for(int i=0;i<rows;i++ )
                   for(int j=i+1;j<cols;j++ )
                       if (i != j && MathDecimal.Abs(A[i_max,j_max]) < MathDecimal.Abs(A[i,j]))
                       {
                           i_max = i;
                           j_max = j;
                       }
           }
        }

    }
}
