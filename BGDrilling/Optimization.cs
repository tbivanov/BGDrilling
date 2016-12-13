using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGDrilling
{
    

    static class Optimization
    {
        public static decimal[] GaussNewton(Func<decimal[], decimal[,]> J, Func<decimal[], decimal[]> r, decimal[] p0)
        {
            decimal[] p = p0;
            decimal[,] p1;
            decimal a = 1;
            int iter = 0;
            while (iter < 4) //TODO: while error is large
            {
                a = 1;


                decimal[] pAdd = LinearLeastSquares(J(p), MathDecimal.Negative(r(p)));

                while (MathDecimal.SquaredNorm2(r(p)) - MathDecimal.SquaredNorm2(r(MathDecimal.Sum(p, MathDecimal.Prod(a, pAdd)))) <
                        1M / 2M * a * MathDecimal.SquaredNorm2(MathDecimal.Prod(J(p), p)) && a>=0.000000000000000000000000001M)
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

        public static decimal[] LinearLeastSquares(decimal[,] A, decimal[] r)
        {
            decimal[,] B = MathDecimal.Prod(MathDecimal.Transpose(A), A);
            decimal[] y = MathDecimal.Prod(MathDecimal.Transpose(A), r);
            decimal[] res = LinearAlgebra.Gauss(B, y);
            return res;
        }

    }
}
