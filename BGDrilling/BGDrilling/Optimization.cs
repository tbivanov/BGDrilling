using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGDrilling
{
    class lqlql { }

    static class Optimization
    {
        public static decimal[] GaussNewton (Func<decimal[], decimal[][]> J, Func<decimal[], decimal[]> r, decimal[] p0)
        {
            decimal[] p = p0;
            decimal a = 1;
            while(true)
            {
                decimal[] pAdd = LinearLeastSquares(J, r);
                while ( MathDecimal.Pow2(MathDecimal.Norm2(r(p))) - MathDecimal.Pow2(MathDecimal.Norm2(MathDecimal.Sum(p, MathDecimal.Prod(a, pAdd)))) <
                    1M/2M*a*MathDecimal.Pow2(MathDecimal.Prod(J(p),p)))
                {
                    a /= 2;

                }
                p = MathDecimal.Sum(p, MathDecimal.Prod(a, pAdd));
                
            }
            throw new NotImplementedException();    
        }

        public static decimal[] LevenbergMarquardt(Func<decimal[], decimal[]> r, Func<decimal[], decimal[][]> J, decimal[] p0)
        {
            throw new NotImplementedException();
        }

        public static decimal[] LinearLeastSquares(Func<decimal[], decimal[][]> J, Func<decimal[], decimal[]> r)
        {
            throw new NotImplementedException();
        }

    }
}
