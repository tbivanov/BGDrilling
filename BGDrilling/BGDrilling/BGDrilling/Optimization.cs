using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace BGDrilling
{
    public enum LSMMethod
    {
        Gauss,
        LUDecomposition
    }

    static class Optimization
    {
       
        public static decimal[] GaussNewton(Func<decimal[], decimal[,]> J, Func<decimal[], decimal[]> r, decimal[] p0)
        {
            decimal[] p = p0;
            decimal a;
            int iter = 0;
            while (iter < 100) //TODO: while error is large
            {
                a = 1;

                decimal[] pAdd = LinearLeastSquares(J(p), MathDecimal.Negative(r(p)), "SVD");

                while (MathDecimal.SquaredNorm2(r(p)) - MathDecimal.SquaredNorm2(r(MathDecimal.Sum(p, MathDecimal.Prod(a, pAdd)))) <
                        1M / 2M * a * MathDecimal.SquaredNorm2(MathDecimal.Prod(J(p), pAdd)) && a>=0.0000000000001M)
                {
                    a /= 2;
                }
                
                p = MathDecimal.Sum(p, MathDecimal.Prod(a, pAdd));
                iter++;
            }
            

            return p; 
        }

        public static decimal[] LevenbergMarquardt(Func<decimal[], decimal[]> r, Func<decimal[], decimal[,]> J, decimal[] p0)
        {
            throw new NotImplementedException();
        }

        public static decimal[] LinearLeastSquares(decimal[,] A, decimal[] b, String method = "SVD")
        {
            decimal[] res=new decimal[A.GetLength(1)];

            if (method=="Gauss")
            {
                decimal[,] B = MathDecimal.Prod(MathDecimal.Transpose(A), A);
                decimal[] y = MathDecimal.Prod(MathDecimal.Transpose(A), b);
                res = LinearAlgebra.Gauss(B, y);
            }
            else if (method == "LUDecomposition")
            {
                List <decimal[,]> LU =LinearAlgebra.LUDecomposition(A);// L, U, PI1, Pi2;
                //Solve system Ly=b1, where b1=PI1*b
                //L^T*L*y=L^T*b1
                // M=L^T * L , N=L^T*PI1*b
                decimal[,] M = MathDecimal.Prod(MathDecimal.Transpose(LU[0]), LU[0]);
                decimal[] N  = MathDecimal.Prod(MathDecimal.Transpose(LU[0]), MathDecimal.Prod(LU[2], b));
                decimal[] y = LinearAlgebra.Gauss(M, N);
                
                //Solve system y=U*x1=U*PI2^T*x
                res = MathDecimal.Prod(LU[3],LinearAlgebra.BackwardSubstitutionUpp(LU[1], y));
            }
            else if(method=="SVD")
            {
                double[,] ADouble = new double[A.GetLength(0), A.GetLength(1)];
                for (int i = 0; i < A.GetLength(0); i++)
                    for (int j = 0; j < A.GetLength(1); j++)
                        ADouble[i, j] = (double)A[i, j];

                Matrix<double> JMatrix = DenseMatrix.OfArray(ADouble);
                MathNet.Numerics.LinearAlgebra.Factorization.Svd<double> Jsvd = JMatrix.Svd();

                double[,] LDouble = Jsvd.W.ToArray();
                double[,] UDouble = Jsvd.U.ToArray();
                double[,] VtDouble = Jsvd.VT.ToArray();

                decimal[,] L = new decimal[A.GetLength(0), A.GetLength(1)]; 
                decimal[,] U = new decimal[A.GetLength(0), A.GetLength(0)];
                decimal[,] Vt = new decimal[A.GetLength(1), A.GetLength(1)];

                for (int i = 0; i < A.GetLength(0); i++)
                    for (int j = 0; j < A.GetLength(1); j++)
                        L[i, j] = (decimal)LDouble[i, j];

                for (int i = 0; i < A.GetLength(0); i++)
                    for (int j = 0; j < A.GetLength(0); j++)
                        U[i, j] = (decimal)UDouble[i, j];

                for (int i = 0; i < A.GetLength(1); i++)
                    for (int j = 0; j < A.GetLength(1); j++)
                        Vt[i, j] = (decimal)VtDouble[i, j];

                for (int i = 0; i < L.GetLength(1); i++)
                    if (L[i, i] < 0.0000000000001M && L[i, i] > -0.0000000000001M)
                        L[i, i] = 0;
                    else
                        L[i, i] = 1 / L[i, i];

                res = MathDecimal.Prod(MathDecimal.Transpose(Vt), MathDecimal.Prod(MathDecimal.Transpose(L), MathDecimal.Prod(MathDecimal.Transpose(U),b)));
            }

            return res;
        }

    }
}
