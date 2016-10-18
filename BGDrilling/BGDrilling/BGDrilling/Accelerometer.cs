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
            return Optimization.GaussNewton(J, r, new decimal[3] { 0, 0, 0 });
        }
        private decimal[] computeR(decimal[] p)
        {
            //TODO: Rewrite computeR!!!
            decimal[] res = new decimal[11];
            for(int i = 0; i<res.Length; i++)
            {
                res[i]=(decimal)(Math.Exp(((double)p[0]) * i * 0.1) + ((double)p[2]) * Math.Exp(((double)p[1]) * i * 0.1 - Math.Exp(i * 0.1) - Math.Sin(i * 0.1)));
            }
            return res; 
        }
        private decimal[,] computeJ(decimal[] p)
        {
            //TODO: Rewrite computeR!!!
            decimal[,] res = new decimal[11,3];
            for (int i = 0; i < res.Length; i++)
            {
                res[i,0] = (decimal)((double)p[0]*Math.Exp(((double)p[0]) * i * 0.1));
                res[i,1] = (decimal)((double)(p[1]*p[2])*Math.Exp(((double)p[2]) * i * 0.1) );
                res[i,2] = (decimal)(Math.Exp(((double)p[1]) * i * 0.1));
            }
            return res;
        }
        public static decimal incl(Measurement meas)
        {
            decimal incl;
            if (90 - 360 / (2 * MathDecimal.PI) *
                   MathDecimal.ACos(meas.data[2]
                   / MathDecimal.Sqrt((meas.data[0] * meas.data[0] + meas.data[1] * meas.data[1] + meas.data[2] * meas.data[2]))) > 60)
            {
                incl = 90 - 360 / (2 * MathDecimal.PI) *
                    MathDecimal.ASin(MathDecimal.Sqrt((meas.data[0] * meas.data[0] + meas.data[1] * meas.data[1])
                    / (meas.data[0] * meas.data[0] + meas.data[1] * meas.data[1] + meas.data[2] * meas.data[2])));
            }
            else
            {
                incl = 90 - 360 / (2 * MathDecimal.PI) *
                   MathDecimal.ACos(meas.data[2]
                   / MathDecimal.Sqrt((meas.data[0] * meas.data[0] + meas.data[1] * meas.data[1] + meas.data[2] * meas.data[2])));
            }
            return incl;
        }
        
        public static decimal tf(Measurement meas)
        {
            decimal tf;
            if (MathDecimal.ATan2(meas.data[1], meas.data[0]) > -0.1M)
                tf = 360 / (2 * MathDecimal.PI) * MathDecimal.ATan2(meas.data[1], meas.data[0]);
            else
                tf = 360 + 360 / (2 * MathDecimal.PI) * MathDecimal.ATan2(meas.data[1], meas.data[0]);
            return tf;
        }
    }
}
