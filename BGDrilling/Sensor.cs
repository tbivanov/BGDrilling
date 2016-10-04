﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGDrilling
{
    abstract class Sensor
    {
        public List<Measurement> data { get; set; }
        public CalibrationParameter pars { get; set; }
        public Sensor():this(new List<Measurement>())
        {
          
        }
        public Sensor(List<Measurement> data, CalibrationParameter pars = null)
        {
            this.data = data;
            this.pars = pars;
        }
        public abstract void calibrate();
        public virtual void compute()
        {
            throw new NotImplementedException();
        }
    }
}
