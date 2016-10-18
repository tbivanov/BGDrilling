using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGDrilling
{
    class Measurement
    {
        public decimal[] data { get; set; }
        public decimal temp { get; set; }
        public decimal? tf { get; set; }
        public decimal? incl { get; set; }
        public CalibrationStatus status { get; set; }
        public Measurement (decimal[] data, decimal temp, decimal? tf = null, decimal? incl = null, CalibrationStatus status = CalibrationStatus.ToCalibrate)
        {
            this.data = data;
            this.temp = temp;
            this.tf = tf;
            this.incl = incl;
            this.status = status;
        }
    }
}
