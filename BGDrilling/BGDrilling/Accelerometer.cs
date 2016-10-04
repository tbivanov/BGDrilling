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
        public override void calibrate()
        {
            throw new NotImplementedException();
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
