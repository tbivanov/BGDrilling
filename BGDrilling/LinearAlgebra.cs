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
                //Find the indices of the maximal element
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

        public static void  JacobiMethod(decimal[,] A, decimal accuracy)
        {
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
                       if (i != j && MathDecimal.Abs(A[i_max, j_max]) < MathDecimal.Abs(A[i, j]))
                       {
                           i_max = i;
                           j_max = j;
                       }
           }
        }

        public static List<decimal[,]> LUDecomposition (decimal[,] A)
        {
            int rows = A.GetLength(0);
            int cols = A.GetLength(1);
            decimal l, temp;
            int maxRowIndex, maxColIndex;
            List<decimal[,]> result=new List<decimal[,]>();
            decimal[,] L = new decimal[rows, cols];
            decimal[,] Alocal = new decimal[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    Alocal[i, j] = A[i, j];
            }
            for (int i=0; i<cols; i++)
            {
                L[i, i] = 1;
            }
            decimal[,] Pi1 = new decimal[rows, rows];
            for (int i = 0; i < rows; i++)
            {
                Pi1[i, i] = 1;
            }
            decimal[,] Pi2 = new decimal[cols, cols];
            for (int i = 0; i < cols; i++)
            {
                Pi2[i, i] = 1;
            }
            for (int i=0; i<cols; i++)
            {
                //Find the largest element in the i-th column
                maxRowIndex = i;
                maxColIndex = i;
                for (int j = i + 1; j < rows; j++)
                    for (int k = i; k < cols; k++)
                        if (MathDecimal.Abs(Alocal[maxRowIndex, maxColIndex]) < MathDecimal.Abs(Alocal[j, k]))
                        {
                            maxRowIndex = j;
                            maxColIndex = k;
                        }
                //Exchange rows
                for(int j=i;j<cols;j++)
                {
                    temp = Alocal[i, j];
                    Alocal[i, j] = Alocal[maxRowIndex, j];
                    Alocal[maxRowIndex,j] = temp;
                }
                for (int j = 0; j < i; j++)
                {
                    temp = L[i, j];
                    L[i, j] = L[maxRowIndex, j];
                    L[maxRowIndex,j] = temp;
                }
                for (int j = 0; j < rows; j++)
                {
                    temp = Pi1[i, j];
                    Pi1[i, j] = Pi1[maxRowIndex, j];
                    Pi1[maxRowIndex, j] = temp;
                }
                //Exchange columns
                for (int j = 0; j < rows; j++)
                {
                    temp = Alocal[j, i];
                    Alocal[j, i] = Alocal[j, maxColIndex];
                    Alocal[j, maxColIndex] = temp;
                }
                for (int j = 0; j < cols; j++)
                {
                    temp = Pi2[j, i];
                    Pi2[j, i] = Pi2[j, maxColIndex];
                    Pi2[j, maxColIndex] = temp;
                }
                //Proceed with elimination
                for (int j=i+1; j<rows; j++)
                {
                    l = Alocal[j, i] / Alocal[i, i];
                    L[j, i] = l;
                    Alocal[j, i] = 0;
                    for(int k=i+1; k<cols; k++)
                    {
                        Alocal[j, k] = Alocal[j, k] - l * Alocal[i, k];
                    }
                }
            }
            decimal[,] U = new decimal[cols, cols];
            for (int i =0; i<cols; i++)
            {
                for (int j = 0; j < cols; j++)
                    U[i, j] = Alocal[i, j];
            }
            result.Add(L);
            result.Add(U);
            result.Add(Pi1);
            result.Add(Pi2);
           return result;
        }

    }
}
