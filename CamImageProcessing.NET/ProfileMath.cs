using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Math
using MathNet.Numerics;

namespace CamImageProcessing.NET
{
    class ProfileMath
    {
        // Coefficients of fit polynom
        public List<double> FitPolyCoeff
        { get; set; }

        // *** Properties ***
        public int Npoints
        { get; set; }

        // X,Y points of data array
        public List<double> Xlist
        { get; set; }
        public List<double> Ylist
        { get; set; }

        public string ProfileName
        { get; set; }

        // ctor of the data array
        public ProfileMath(List<double> xx, List<double> yy, string name)
        {
            if (xx.Count == 0 || yy.Count == 0 || xx.Count != yy.Count)
                return;
            if (name != "")
                ProfileName = name;
            else
                ProfileName = "Profile";

            Xlist = new List<double>(xx);
            Ylist = new List<double>(yy);
            Npoints = Xlist.Count;
            FitPolyCoeff = new List<double>();
        }

        // Fits polynom pN to the data, returns polynom coefficient array[N+1]
        public double[] FitPoly(int N)
        {
            double[] p = { };
            // Limit the order of polinom to 100
            if (N > 100)
            {
                Console.WriteLine(" Error: order of polynom {0} must be <= 100. ", N );
                return Ylist.ToArray();
            }
            try
            {
                p = Fit.Polynomial(Xlist.ToArray(), Ylist.ToArray(), N);
                Console.WriteLine(" FitPoly: number of fit polinom coefficients: {0}. ", p.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error: Could not make fit of order {0} of {1}. Original error: " + ex.Message, N, ProfileName);
            }
            // Put result into list
            FitPolyCoeff.Clear();
            foreach (double v in p)
                FitPolyCoeff.Add(v);

            return Poly(Xlist.ToArray(), p);
        }

        // Function Poly(double x) or Poly (double[] x)
        public double Poly(double x, double[] p)
        {
            double v = 0;
            for (int i = 0; i < p.Length; i++)
                v += p[i] * Math.Pow(x, i);
            return v;
        }
        public double[] Poly(double[] x, double[] p)
        {
            List<double> vl = new List<double>();
            for (int ix = 0; ix < x.Length; ix++)
            {
                double v = 0;
                for (int i = 0; i < p.Length; i++)
                    v += p[i] * Math.Pow(x[ix], i);
                vl.Add(v);
            }
            return vl.ToArray();
        }

        /// <summary>
        /// Gets LEFT point of "zero intersection". The polyN is continued by f = ax^2 + bx + c
        /// Boundary conditions: (1) PolyN(x_b) = f(x_b); (2) d/dx(PolyN(x_b)) = df/dx(x_b)
        /// </summary>
        public double ZeroLeft()
        {
            // If there are no polyN coeff., return 0
            if (FitPolyCoeff.Count == 0)
                return 0;

            double x0 = Xlist.ElementAt(0);
            double dx = Xlist.ElementAt(1) - Xlist.ElementAt(0);
            double a = Poly(x0, PolyDerivative(FitPolyCoeff.ToArray())) / (2 * x0);
            double c = Poly(x0, FitPolyCoeff.ToArray()) - a * x0 * x0;
            Console.WriteLine("ZeroLeft: fit approximation f = ax^2+c : a = {0}, c = {1}, d/dx(polyN)(x0) = {2}", a, c, Poly(x0, PolyDerivative(FitPolyCoeff.ToArray())));
            return -dx * Math.Floor(Math.Sqrt(Math.Abs(c / a)) / dx);
        }

        /// <summary>
        /// Gets RIGHT point of "zero intersection". The polyN is continued by f = a*(x-xc)^2
        /// Boundary conditions: (1) PolyN(x_b) = f(x_b); (2) d/dx(PolyN(x_b)) = df/dx(x_b)
        /// </summary>
        public double ZeroRight()
        {
            // If there are no polyN coeff., return 0
            if (FitPolyCoeff.Count == 0)
                return 0;

            double x0 = Xlist.Last();
            double dx = Xlist.ElementAt(1) - Xlist.ElementAt(0);
            double a = Poly(x0, PolyDerivative(FitPolyCoeff.ToArray())) / (2 * x0);
            double c = Poly(x0, FitPolyCoeff.ToArray()) - a * x0 * x0;
            Console.WriteLine("ZeroRight: fit approximation f = ax^2+c : a = {0}, c = {1}, d/dx(polyN)(x0) = {2}", a, c, Poly(x0, PolyDerivative(FitPolyCoeff.ToArray())));
            return dx * Math.Floor(Math.Sqrt(Math.Abs(c / a)) / dx);
        }

        /// <summary>
        /// Makes extended X array where the fit polynom >= 0
        /// </summary>
        public double[] XarrayExt(double Xleft, double Xright)
        {
            List<double> XX = new List<double>(Xlist);
            double x0 = Xlist.ElementAt(0);
            double dx = Xlist.ElementAt(1) - x0;
            double x = Xlist.Last();
            while (x <= Xright)
            {
                x += dx;
                XX.Add(x);
            }
            x = x0;
            while (x >= Xleft)
            {
                x -= dx;
                XX.Insert(0, x);
            }
            return XX.ToArray();
        }

        /// <summary>
        /// Calculates coefficients of the derivative of the fit polynom.
        /// Coeff. with x^(N-1): N*P_N
        /// </summary>
       public double[] PolyDerivative(double[] p)
        {
            List<double> pd = new List<double>();
            for (int i = 0; i < FitPolyCoeff.Count - 1; i++)
                pd.Add(i * FitPolyCoeff.ElementAt(i));
            return pd.ToArray();
        }

        /// <summary>
        /// Calculates integral for an array Y[]: Int = Sum_N0-to-N1 (Y_i * dx)
        /// </summary>
        public double DefiniteIntegral(double[] Y, int N0, int N1, double dx)
        {
            double v = 0;
            // Check integration limits
            if (N1 <= N0 || N1 >= Y.Length)
                return 0;

            for (int i = N0; i < N1; i++)
                v += Y[i];
            return v * dx;
        }

        /// <summary>
        /// Abel inversion procedure:
        /// 1. Gets x[] and y[] = polyN[]
        /// 2. Calculates extended xExt[] from left zero crossing to right
        /// 3. Calculates the solution of the integral equation
        /// SEPARATELY FOR LEFT AND RIGHT PARTS of xExt[] (split point - middle of x[], which is the center of original data array)
        /// Assumes axial "half-symmetry" for both parts of the target function.
        /// 4. Returns target function g[] in the extended range xExt[].
        /// WARNING: call FitPoly() method before this method !
        /// </summary>
        public double[] AbelInversionPoly()
        {
            double XleftLimit = ZeroLeft();
            double XrightLimit = ZeroRight();
            double dx = Xlist.ElementAt(1) - Xlist.ElementAt(0);
            // "Axis" of both parts: x = 0
            // Prepare extended x-array from left zero crossing to right
            List<double> Xext = new List<double>(XarrayExt(XleftLimit, XrightLimit));
            // Prepare two part x-arrays
            List<double> XextLeft = new List<double>();
            List<double> XextRight = new List<double>();
            int ix = 0;
            while (Xext.ElementAt(ix) < 0)
            {
                XextLeft.Add(Xext.ElementAt(ix++));
            }
            int separatorIndex = ix;
            XextRight = XextRight.GetRange(separatorIndex, Xext.Count - separatorIndex);    // +1 ??
            // Prepare arrays Y[] and dY/dX[] (polyN and its derivative) for left and right parts of the extended x-array
            List<double> dYextLeft = new List<double>(Poly(XextLeft.ToArray(), PolyDerivative(FitPolyCoeff.ToArray())));
            List<double> dYextRight = new List<double>(Poly(XextRight.ToArray(), PolyDerivative(FitPolyCoeff.ToArray())));
            // Lists of calculated semi-profiles (excited atom density in case of measurements of emission intensity
            List<double> nLeft = new List<double>();
            List<double> nRight = new List<double>();
            // Function under Abel integral
            List<double> fInt = new List<double>();

            // ## Calculation of the left part ##
            // Mirror left side to the positive half-plane
            // That means index = MaxCount - i
            for (double Rleft = 0; Rleft > XleftLimit; Rleft -= dx)
            {
                int Index = XextLeft.Count - (int)Math.Abs(Rleft / dx);
                // I'm not sure about element-wise operations with arrays, so lets do it by hands
                for (double rl = Rleft; rl > XleftLimit; rl -= dx)
                {
                    int index = XextLeft.Count - (int)Math.Abs((rl / dx));
                    if (rl == Rleft)
                        fInt.Add(0);
                    else
                        fInt.Add(dYextLeft.ElementAt(index) / Math.Sqrt(rl * rl - Rleft * Rleft));
                }
                nLeft.Add( DefiniteIntegral(fInt.ToArray(), Index, XextLeft.Count, dx) );
            }
            // ## Calculation of the right part ##
            fInt.Clear();
            for (double Rright = 0; Rright < XrightLimit; Rright += dx)
            {
                int Index = (int)(Rright / dx);
                for (double rr = Rright; rr < XrightLimit; rr += dx)
                {
                    int index = (int)(rr / dx);
                    if (rr == Rright)
                        fInt.Add(0);
                    else
                        fInt.Add(dYextRight.ElementAt(index) / Math.Sqrt(rr * rr - Rright * Rright));
                    nRight.Add(DefiniteIntegral(fInt.ToArray(), Index, XextRight.Count, dx));
                }
            }
            // Merge both parts into a single list
            nLeft.Reverse();
            List<double> nExt = new List<double>(nLeft.Count + nRight.Count);
            nExt.AddRange(nLeft);
            nExt.AddRange(nRight);
            // Scale output to the maximum of input function
            double coeff = (nExt.Max() != 0) ? Ylist.Max() / nExt.Max() : 1;
            for (int i = 0; i < nExt.Count; i++)
                nExt[i] *= coeff;
            return nExt.ToArray();
        }


    }
}
