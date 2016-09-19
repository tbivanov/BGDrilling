using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGDrilling
{
    abstract class Sensor
    {
        private decimal[,] M = new decimal[3, 3], b = new decimal [3,1];
        public Sensor()
        {

        }
        public abstract void calibrate();
        public abstract decimal tf();
        public abstract decimal incl();
    }
}
