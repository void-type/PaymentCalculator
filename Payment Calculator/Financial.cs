using System;

namespace VoidType.Financial
{

    // This is a class with some of the functionality of the Excel and VB.Net finanacial functions. 
    // This can be used when you need these functions and don't have access to the legacy .Net libraries.


    public static class Financial
    {
        #region PMT
        public static decimal PMT(decimal r, int nper, decimal pv, decimal fv = 0, int type = 0)
        {
            var pmt = 0.0m;

            if (nper == 0)
            {
                pmt = 0;
            } else if (r == 0)
            {
                pmt = (fv - pv) / nper;
            } else
            {
                pmt = r / (decimal)(Math.Pow(1 + (double)r, nper) - 1) * -(pv * (decimal)Math.Pow(1 + (double)r, nper) + fv);
            }

            if (type == 1)
            {
                pmt /= (1 + r);
            }

            return pmt;
        }

        public static double PMT(double r, int nper, double pv, double fv = 0, int type = 0)
        {
            return Convert.ToDouble(PMT(Convert.ToDecimal(r), nper, Convert.ToDecimal(pv), Convert.ToDecimal(fv), type));
        }
        #endregion

        #region IPMT
        public static decimal IPMT(decimal r, int per, int nper, decimal pv, decimal fv = 0, int type = 0)
        {
            var pmt = PMT(r, nper, pv, fv, type);
            var ipmt = FV(r, per - 1, pmt, pv, type) * r;

            if (type == 1)
            {
                ipmt /= (1 + r);
            }

            return ipmt;
        }

        public static double IPMT(double r, int per, int nper, double pv, double fv = 0, int type = 0)
        {
            return Convert.ToDouble(IPMT(Convert.ToDecimal(r), per, nper, Convert.ToDecimal(pv), Convert.ToDecimal(fv), type));
        }
        #endregion

        #region PPMT
        public static decimal PPMT(decimal r, int per, int nper, decimal pv, decimal fv = 0, int type = 0)
        {
            var pmt = PMT(r, nper, pv, fv, type);
            var ipmt = FV(r, per - 1, pmt, pv, type) * r;

            if (type == 1)
            {
                ipmt /= (1 + r);
            }

            return pmt - ipmt;
        }

        public static double PPMT(double r, int per, int nper, double pv, double fv = 0, int type = 0)
        {
            return Convert.ToDouble(PPMT(Convert.ToDecimal(r), per, nper, Convert.ToDecimal(pv), Convert.ToDecimal(fv), type));
        }
        #endregion

        #region FV
        public static decimal FV(decimal r, int nper, decimal pmt, decimal pv = 0, int type = 0)
        {
            var fv = 0.0m;
            var pow = (decimal)Math.Pow(1 + (double)r, nper);

            if (type == 1)
            {
                pmt = pmt * (1 + r);
            }

            if (nper == 0)
            {
                fv = -pv;
            } else if (r == 0)
            {
                fv = -(pv + (nper * pmt));
            } else
            {
                fv = -(pmt * (pow - 1) / r + pv * pow);
            }

            return fv;
        }

        public static double FV(double r, int nper, double pmt, double pv = 0, int type = 0)
        {
            return Convert.ToDouble(FV(Convert.ToDecimal(r), nper, Convert.ToDecimal(pmt), Convert.ToDecimal(pv), type));
        }
        #endregion

        #region PV
        public static decimal PV(decimal r, int nper, decimal pmt, decimal fv = 0, int type = 0)
        {
            var num = 1.0m;
            var pow = (decimal)Math.Pow((1 + (double)r), nper);

            if (r == 0)
            {
                return (-fv - (pmt * nper));
            } else if (type == 1)
            {
                num = (1 + r);
            }
            return (-(fv + ((pmt * num) * ((pow - 1) / r))) / pow);
        }

        public static double PV(double r, int nper, double pmt, double pv = 0, int type = 0)
        {
            return Convert.ToDouble(PV(Convert.ToDecimal(r), nper, Convert.ToDecimal(pmt), Convert.ToDecimal(pv), type));
        }
        #endregion

        #region NPV
        public static decimal NPV(decimal r, params decimal[] cfs)
        {
            var ans = 0.0m;

            for (int i = 0; i < cfs.Length; i++)
            {
                ans += cfs[i] / (decimal)Math.Pow((1 + (double)r), (i + 1));
            }

            return ans;
        }

        public static double NPV(double r, params double[] cfs)
        {
            var ans = 0.0;

            for (int i = 0; i < cfs.Length; i++)
            {
                ans += cfs[i] / Math.Pow((1 + r), (i + 1));
            }

            return ans;
        }
        #endregion

    }
}

