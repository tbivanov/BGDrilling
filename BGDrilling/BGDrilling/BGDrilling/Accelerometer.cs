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
            return Optimization.GaussNewton(J, r, new decimal[12] {  1M, 0, 0 ,0,1M,0, 0,0,1M,0,0,0 });
        }
        private decimal[] computeR(decimal[] p)
        {
            int dataCount = data.Count;
            decimal[] res = new decimal[2*dataCount];
            for(int i = 0; i<dataCount; i++)
            {
                res[2*i] = tf( new Measurement(computeCalibrated(p,i)) ) - (decimal)data[i].tf;
                res[2 * i + 1] = incl(new Measurement(computeCalibrated(p, i))) - (decimal)data[i].incl;
            }
            return res; 
        }
        private decimal[,] computeJ(decimal[] p)
        {
            int dataCount = data.Count;
            decimal eps = 0.000001M;
            decimal[,] res = new decimal[2*dataCount, 12];
            for (int i = 0; i < dataCount; i++)
            {
                decimal[] perturb = new decimal[12];
                for (int j = 0; j < 12; j++)
                {
                    perturb = new decimal[12];
                    perturb[j] = eps;
                    res[2*i, j] = 1 / eps * (tf(new Measurement(computeCalibrated(MathDecimal.Sum(p, perturb), i))) - tf(new Measurement(computeCalibrated(p, i))));
                    res[2*i+1, j] = 1 / eps * (incl(new Measurement(computeCalibrated(MathDecimal.Sum(p, perturb), i))) - incl(new Measurement(computeCalibrated(p, i))));
                }
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

        private decimal[] computeCalibrated(decimal[] p, int index)
        {
            decimal[] res = new decimal[3];
            res[0] = p[0] * data[index].data[0] + p[1] * data[index].data[1] + p[2] * data[index].data[2] + p[9];
            res[1] = p[3] * data[index].data[0] + p[4] * data[index].data[1] + p[5] * data[index].data[2] + p[10];
            res[2] = p[6] * data[index].data[0] + p[7] * data[index].data[1] + p[8] * data[index].data[2] + p[11];
            return res;
        }

    }
}
