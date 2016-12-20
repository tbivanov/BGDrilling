using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGDrilling
{
    class Accelerometer : Sensor
    {

        public Accelerometer(List<Measurement> data, CalibrationParameter pars = null) : base(data,pars) { }
        public Accelerometer() : base() { }
        public override decimal[] calibrate()
        {
            Func<decimal[], decimal[,]> J = new Func<decimal[], decimal[,]>(computeJ);
            Func<decimal[], decimal[]> r = new Func<decimal[], decimal[]>(computeR);
            decimal[] p = { 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 };
            decimal[,] M, Mfinal=new decimal[3, 3] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            decimal[] b, bFinal=new decimal[3] { 0, 0, 0 };

            for (int j=0; j<10; j++)
            {

                p=Optimization.GaussNewton(J, r, p);
                M = new decimal[,] { { p[0], p[1], p[2] }, { p[3], p[4], p[5] }, { p[6], p[7], p[8] } };
                b = new decimal[] { p[9], p[10], p[11] };

                Mfinal = MathDecimal.Prod(Mfinal, M);
                bFinal = MathDecimal.Sum(MathDecimal.Prod(M, bFinal), b);


                for (int i = 0; i < data.Count; i++)
                    data[i].data = MathDecimal.Sum(b,MathDecimal.Prod(M, data[i].data));


            //scale with respect to the gravitational acceleration
            /*decimal[] rGravity = new decimal[data.Count];
             for (int i = 0; i < rGravity.GetLength(0); i++)
                 rGravity[i] = Preferences.G;
             decimal[,] M = new decimal[,] { { p[0], p[1], p[2] }, { p[3], p[4], p[5] }, { p[6], p[7], p[8] } };
             decimal[] b = new decimal[] { p[9], p[10], p[11] };
             decimal[,] JGravity = new decimal[data.Count,1];
             for (int i = 0; i < rGravity.GetLength(0); i++)
                 JGravity[i,1] = MathDecimal.Norm2(MathDecimal.Sum(MathDecimal.Prod(M, data[i].data),b));
             decimal Q = Optimization.LinearLeastSquares(JGravity, rGravity)[1];
             p = MathDecimal.Prod(Q, p);*/
            }

            pars = new CalibrationParameter(Mfinal[0,0], Mfinal[0, 1], Mfinal[0, 2], Mfinal[1, 0], Mfinal[1, 1], Mfinal[1, 2], Mfinal[2, 0], Mfinal[2, 1], Mfinal[2, 2], bFinal[0], bFinal[1], bFinal[2]);
            return p;
        }
        public override decimal[] computeR(decimal[] p)
        {
            //TODO: Rewrite computeR!!!
            decimal[,] M = { { p[0], p[1], p[2] }, { p[3], p[4], p[5] }, { p[6], p[7], p[8] } };
            decimal[] b = { p[9], p[10], p[11] };
            decimal[] res = new decimal[data.Count*2];/*rewrite length ...*/

            //res=this.data[0].data;
            for(int i = 0; i<res.GetLength(0)/2; i++)
            {
                res[2 * i] =  tf(MathDecimal.Sum(MathDecimal.Prod(M, data[i].data), b))-(decimal)data[i].tf;
                res[2 * i + 1] = incl(MathDecimal.Sum(MathDecimal.Prod(M, data[i].data), b))-(decimal)data[i].incl;
            }
            return res; 
        }

        public override decimal[,] computeJ(decimal[] p)
        {
            //TODO: Rewrite computeJ!!!
           // decimal[,] M = { { p[0], p[1], p[2] }, { p[3], p[4], p[5] }, { p[6], p[7], p[8] } };
            //decimal[] b = { p[9], p[10], p[11] };
            decimal[,] res = new decimal[data.Count*2,12];/*rewrite length ...*/
            decimal B1, B2, B3, B, A;

            for (int i = 0; i < data.Count; i++)
            {
                B1 = p[0] * data[i].data[0]+p[1]* data[i].data[1]+p[2]* data[i].data[2] + p[9];
                B2 = p[3] * data[i].data[0] + p[4] * data[i].data[1] + p[5] * data[i].data[2] + p[10];
                B3 = p[6] * data[i].data[0] + p[7] * data[i].data[1] + p[8] * data[i].data[2] + p[11];
                B = B1 * B1 + B2 * B2 + B3 * B3;
                A = B1 * B1 + B2 * B2;

                res[2 * i, 0] = (180 * data[i].data[0] * B2) / (MathDecimal.PI * A);
                res[2 * i, 1] = (180 * data[i].data[1] * B2) / (MathDecimal.PI * A);
                res[2 * i, 2] = (180 * data[i].data[2] * B2) / (MathDecimal.PI * A);
                res[2 * i, 3] = -(180 * data[i].data[0] * B1) / (MathDecimal.PI * A);
                res[2 * i, 4] = -(180 * data[i].data[1] * B1) / (MathDecimal.PI * A);
                res[2 * i, 5] = -(180 * data[i].data[2] * B1) / (MathDecimal.PI * A);
                res[2 * i, 6] = 0;
                res[2 * i, 7] = 0;
                res[2 * i, 8] = 0;
                res[2 * i, 9] = (180 * B2) / (MathDecimal.PI * A);
                res[2 * i, 10] = -(180 * B1) / (MathDecimal.PI * A);
                res[2 * i, 11] = 0;

                res[2 * i + 1, 0] = -(180 * data[i].data[0] * B1 * B3) / (MathDecimal.PI * B * MathDecimal.Sqrt(A));
                res[2 * i + 1, 1] = -(180 * data[i].data[1] * B1 * B3) / (MathDecimal.PI * B * MathDecimal.Sqrt(A));
                res[2 * i + 1, 2] = -(180 * data[i].data[2] * B1 * B3) / (MathDecimal.PI * B * MathDecimal.Sqrt(A));
                res[2 * i + 1, 3] = -(180 * data[i].data[0] * B2 * B3) / (MathDecimal.PI * B * MathDecimal.Sqrt(A));
                res[2 * i + 1, 4] = -(180 * data[i].data[1] * B2 * B3) / (MathDecimal.PI * B * MathDecimal.Sqrt(A));
                res[2 * i + 1, 5] = -(180 * data[i].data[2] * B2 * B3) / (MathDecimal.PI * B * MathDecimal.Sqrt(A));
                res[2 * i + 1, 6] = (180 * data[i].data[0] * MathDecimal.Sqrt(A)) / (MathDecimal.PI * B);
                res[2 * i + 1, 7] = (180 * data[i].data[1] * MathDecimal.Sqrt(A)) / (MathDecimal.PI * B);
                res[2 * i + 1, 8] = (180 * data[i].data[2] * MathDecimal.Sqrt(A)) / (MathDecimal.PI * B);
                res[2 * i + 1, 9]  = -(180 * B1 * B3) / (MathDecimal.PI * B * MathDecimal.Sqrt(A));
                res[2 * i + 1, 10] = -(180 * B2 * B3) / (MathDecimal.PI * B * MathDecimal.Sqrt(A)); ;
                res[2 * i + 1, 11] = (180 * MathDecimal.Sqrt(A)) / (MathDecimal.PI * B);
            }

            return res;
        }

        public static decimal incl(decimal[] p)
        {
            decimal incl;
            /*if (90 - 360 / (2 * MathDecimal.PI) *
                   MathDecimal.ACos(p[2]/ MathDecimal.Norm2(p)) > 60)
            {
                incl = 90 - 360 / (2 * MathDecimal.PI) *
                    MathDecimal.ASin(MathDecimal.Sqrt((p[0] * p[0] + p[1] * p[1])
                    / MathDecimal.Norm2(p)));
            }
            else
            {
                incl = 90 - 360 / (2 * MathDecimal.PI) *
                   MathDecimal.ACos(p[2]
                   / MathDecimal.Norm2(p));
            }*/
            if (p[2] == 0)
                incl = 0;
            else
                incl = 90 - 360 / (2 * MathDecimal.PI) *
                   MathDecimal.ACos(p[2]
                   / MathDecimal.Norm2(p));

            return incl;
        }
        
        public static decimal tf(decimal[] p)
        {
            decimal tf;
            if (MathDecimal.ATan2(p[1], p[0]) > -0.1M)
                tf = 360 / (2 * MathDecimal.PI) * MathDecimal.ATan2(p[1], p[0]);
            else
                tf = 360 + 360 / (2 * MathDecimal.PI) * MathDecimal.ATan2(p[1], p[0]);
            return tf;
        }
    }
}
