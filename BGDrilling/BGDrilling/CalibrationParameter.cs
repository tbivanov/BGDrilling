using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGDrilling
{
    class CalibrationParameter
    {
        private bool isTempModel;
        private bool? isLinear = null;
        private decimal[] mxx, mxy, mxz, myx, myy, myz, mzx, mzy, mzz, bx, by, bz;
        public CalibrationParameter(decimal mxx, decimal mxy, decimal mxz, decimal myx, decimal myy, 
            decimal myz, decimal mzx, decimal mzy, decimal mzz, decimal bx, decimal by, decimal bz)
        {
            isTempModel = false;
            this.mxx = new decimal[] { mxx };
            this.mxy = new decimal[] { mxy };
            this.mxz = new decimal[] { mxz };
            this.myx = new decimal[] { myx };
            this.myy = new decimal[] { myy };
            this.myz = new decimal[] { myz };
            this.mzx = new decimal[] { mzx };
            this.mzy = new decimal[] { mzy };
            this.mzz = new decimal[] { mzz };
            this.bx = new decimal[] { bx };
            this.by = new decimal[] { by };
            this.bz = new decimal[] { bz };
        }
        public CalibrationParameter(decimal[] mxx, decimal[] mxy, decimal[] mxz, decimal[] myx, decimal[] myy,
            decimal[] myz, decimal[] mzx, decimal[] mzy, decimal[] mzz, decimal[] bx, decimal[] by, decimal[] bz)
        {
            isTempModel = true;
            if (mxx.Length == 2)
                isLinear = true;
            else
                isLinear = false;
            this.mxx = mxx;
            this.mxy = mxy;
            this.mxz = mxz;
            this.myx = myx;
            this.myy = myy;
            this.myz = myz;
            this.mzx = mzx;
            this.mzy = mzy;
            this.mzz = mzz;
            this.bx = bx;
            this.by = by;
            this.bz = bz;
        }


        public decimal[] pars (decimal temp = 0) //TODO: decimal? temp = null
        {
            decimal[] result;
            if (isTempModel == false)
                result = new decimal[] { mxx[0], mxy[0], mxz[0], myx[0], myy[0], myz[0], mzx[0], mzy[0], mzz[0], bx[0], by[0], bz[0]};
            else if (isLinear == true)
                result = new decimal[] {mxx[0]*temp+mxx[1], mxy[0]*temp+mxy[1], mxz[0]*temp+mxz[1],
                    myx[0]*temp+myx[1], myy[0]*temp+myy[1], myz[0]*temp+myz[1],
                    mzx[0]*temp+mzx[1], mzy[0]*temp+mzy[1], mzz[0]*temp+mzz[1],
                    bx[0]*temp+bx[1], by[0]*temp+by[1], bz[0]*temp+bz[1]};
            else
                result = new decimal[] {mxx[0]*temp*temp+mxx[1]*temp+mxx[2], mxy[0]*temp*temp+mxy[1]*temp+mxy[2], mxz[0]*temp*temp+mxz[1]*temp+mxz[2],
                    myx[0]*temp*temp+myx[1]*temp+myx[2], myy[0]*temp*temp+myy[1]*temp+myy[2], myz[0]*temp*temp+myz[1]*temp+myz[2],
                    mzx[0]*temp*temp+mzx[1]*temp+mzx[2], mzy[0]*temp*temp+mzy[1]*temp+mzy[2], mzz[0]*temp*temp+mzz[1]*temp+mzz[2],
                    bx[0]*temp*temp+bx[1]*temp+bx[2], by[0]*temp*temp+by[1]*temp+by[2], bz[0]*temp*temp+bz[1]*temp+bz[2] };
            return result;

        }
    }
}
