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

        /// <summary>
        /// Switch of direction to approximate fit function outside the input x-range.
        /// </summary>
        public enum ApproximationDirection : byte { LEFT=0, RIGHT=1 };

        /// <summary>
        /// Switch of method to continue the fit function outside the input x-range.
        /// Using poly2 = p0 + p1*x + p2*x^2
        /// 0: LINEAR: p2 = 0
        /// 1,2: parabola branches up or down
        /// </summary>
        public enum ApproximationMethodOusideXrange : byte { LINEAR=0, PARABOLIC_POSITIVE=1, PARABOLIC_NEGATIVE=2 };

        // *** Methods ***
        /// <summary>
        /// // ctor allocates x- and y-lists.
        /// </summary>
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

        /// <summary>
        /// Fits polynom poly_N to the Xlist data, returns polynom coefficient array[N+1]. Fills ProfileMath.FitPolyCoeff list with these coefficients.
        /// </summary>
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
                Console.WriteLine(" FitPoly: number of fit polynom coefficients: {0}. ", p.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error: Could not make fit of order {0} of {1}. Original error: " + ex.Message, N, ProfileName);
            }
            // Put result into list
            FitPolyCoeff.Clear();
            foreach (double v in p)
                FitPolyCoeff.Add(v);

            return p;
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
        /// This method uses poly2 = p0 + p1*x + p2*x^2 used to lock an extended x-range between left/right zero crossing and input x-range.
        /// Boundary conditions: equality of functions and 1st derivatives.
        /// Method returns the array of poly2 coefficients for a right side if arg=RIGHT and for a left side if arg=LEFT
        /// </summary>
        public double[] GetLockFunction(ApproximationDirection dir, ApproximationMethodOusideXrange method)
        {
            // If there are no polyN coeff., return 0
            if (FitPolyCoeff.Count == 0)
                return null;

            double x0 = (dir == ApproximationDirection.LEFT) ? Xlist.ElementAt(0) : Xlist.Last();
            double[] p = { 0, 0, 0 };
            //Console.WriteLine("ZeroLeft: fit approximation - {0}", method);
            switch (method)
            {
                case ApproximationMethodOusideXrange.LINEAR:
                    p[2] = 0;
                    p[1] = Poly(x0, PolyDerivative(FitPolyCoeff.ToArray()));
                    p[0] = Poly(x0, FitPolyCoeff.ToArray()) - p[1] * x0;
                    break;
                case ApproximationMethodOusideXrange.PARABOLIC_NEGATIVE:
                    p[1] = 0;
                    p[2] = Poly(x0, PolyDerivative(FitPolyCoeff.ToArray())) / (2 * x0);
                    p[0] = Poly(x0, FitPolyCoeff.ToArray()) - p[2] * x0 * x0;
                    break;
                case ApproximationMethodOusideXrange.PARABOLIC_POSITIVE:
                    double xc = x0 - 2 * Poly(x0, FitPolyCoeff.ToArray()) / Poly(x0, PolyDerivative(FitPolyCoeff.ToArray()));
                    double a = 0.25 * Math.Pow(Poly(x0, PolyDerivative(FitPolyCoeff.ToArray())), 2) / Poly(x0, FitPolyCoeff.ToArray());
                    p[0] = a * xc * xc;
                    p[1] = -2 * a * xc;
                    p[2] = a;
                    break;
                default:    // PARABOLIC_POSITIVE
                    double xc1 = x0 - 2 * Poly(x0, FitPolyCoeff.ToArray()) / Poly(x0, PolyDerivative(FitPolyCoeff.ToArray()));
                    double a1 = 0.25 * Math.Pow(Poly(x0, PolyDerivative(FitPolyCoeff.ToArray())), 2) / Poly(x0, FitPolyCoeff.ToArray());
                    p[0] = a1 * xc1 * xc1;
                    p[1] = -2 * a1 * xc1;
                    p[2] = a1;
                    break;
            }
            return p;
        }
        /// <summary>
        /// Calculates point of zero intersection by solving quadratic equation (Math.NET findRoots).
        /// </summary>
        public double ZeroCrossing(double[] p, ApproximationDirection dir)
        {
            double zc = 0;
            try
            {
                Tuple<System.Numerics.Complex, System.Numerics.Complex> C_roots = FindRoots.Quadratic(p[0], p[1], p[2]);
                if (C_roots.Item1.Imaginary != 0 || C_roots.Item2.Imaginary != 0)   // Error
                {
                    Console.WriteLine("FindRoots: roots are complex, check polynom coefficients.");
                    zc = 0;
                }
                else if (dir == ApproximationDirection.LEFT && C_roots.Item1.Real > 0 && C_roots.Item2.Real > 0)    // Error
                {
                    Console.WriteLine("FindRoots: solving for left (negative) zero crossing, but both roots are positive: {0}, {1}.", C_roots.Item1.Real, C_roots.Item2.Real);
                    zc = 0;
                }
                else if (dir == ApproximationDirection.RIGHT && C_roots.Item1.Real < 0 && C_roots.Item2.Real < 0)   // Error
                {
                    Console.WriteLine("FindRoots: solving for right (positive) zero crossing, but both roots are negative: {0}, {1}.", C_roots.Item1.Real, C_roots.Item2.Real);
                    zc = 0;
                }
                else if (dir == ApproximationDirection.LEFT)    // Ok
                    zc = (C_roots.Item1.Real < 0) ? C_roots.Item1.Real : C_roots.Item2.Real;
                else if (dir == ApproximationDirection.RIGHT)
                    zc = (C_roots.Item1.Real > 0) ? C_roots.Item1.Real : C_roots.Item2.Real;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FindRoots: Error: could not calculate." + ex.Message);
            }


            return zc;
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
            pd.Add(0);
            for (int i = 1; i < FitPolyCoeff.Count - 1; i++)
                pd.Add((i + 1) * FitPolyCoeff.ElementAt(i + 1));
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
        /// 1. Uses data provided by the class
        ///     - Xlist (input x array)
        ///     - Coefficients of the fit polyN (not y-array itself)
        /// 2. Calculates extended xExt[] from left zero crossing to right
        /// 3. Calculates the solution of the integral equation with the _polyN_ fit function
        /// SEPARATELY FOR LEFT AND RIGHT PARTS of xExt[] (split point - middle of x[], which is the center of original data array)
        /// Assumes axial "half-symmetry" for both parts of the target function.
        /// 4. Returns target function g[] in the extended range xExt[].
        /// WARNING: call FitPoly() method before this method !
        /// </summary>
        public double[] AbelInversionPoly(string meth)
        {
            ApproximationMethodOusideXrange method;
            switch (meth)
            {
                case "Linear":
                    method = ApproximationMethodOusideXrange.LINEAR;
                    break;
                case "Quad. positive":
                    method = ApproximationMethodOusideXrange.PARABOLIC_POSITIVE;
                    break;
                case "Quad negative":
                    method = ApproximationMethodOusideXrange.PARABOLIC_NEGATIVE;
                    break;
                default:
                    Console.WriteLine("AbelInversion: Error: could not recognize lock method: " + meth + ", will use quad. positive");
                    method = ApproximationMethodOusideXrange.PARABOLIC_POSITIVE;
                    break;
            }
            // Lock function (poly2) for left side
            double[] p2left = { 0, 0, 0 };
            // Lock function (poly2) for right side
            double[] p2right = { 0, 0, 0 };
            p2left = GetLockFunction(ApproximationDirection.LEFT, method);
            p2right = GetLockFunction(ApproximationDirection.RIGHT, method);
            double XleftLimit = ZeroCrossing(p2left, ApproximationDirection.LEFT);
            double XrightLimit = ZeroCrossing(p2right, ApproximationDirection.RIGHT);
            double dx = Xlist.ElementAt(1) - Xlist.ElementAt(0);
            // "Axis" of both parts: x = 0
            // Prepare extended x-array from left zero crossing to right
            List<double> Xext = new List<double>(XarrayExt(XleftLimit, XrightLimit));
            // Round limits
            XleftLimit = Xext.ElementAt(0);
            XrightLimit = Xext.Last();
            // Count points in all three parts
            int Nleft = (int)((Xlist.ElementAt(0) - Xext.ElementAt(0)) / dx);
            int Nmain = Xlist.Count;
            int Nright = (int)((Xext.Last() - Xlist.Last()) / dx);
            int separatorIndex = Xext.IndexOf(0);
            // Prepare two part x-arrays
            List<double> XextLeft = Xext.GetRange(0, separatorIndex);
            List<double> XextRight = Xext.GetRange(separatorIndex, Xext.Count - separatorIndex);
            // Prepare arrays dY/dX[] (Y - polyN and lock functions on wings) for left and right parts of the extended x-array
            // Derivative of left lock function
            List<double> dYextLeft = new List<double>(Poly(Xext.GetRange(0, Nleft).ToArray(), PolyDerivative(p2left)));
            // + 1/2 central part
            dYextLeft.AddRange(Poly(Xext.GetRange(Nleft, separatorIndex - Nleft).ToArray(), PolyDerivative(FitPolyCoeff.ToArray())));
            // 1/2 central part
            List<double> dYextRight = new List<double>(Poly(Xext.GetRange(separatorIndex, Nmain - separatorIndex).ToArray(), PolyDerivative(FitPolyCoeff.ToArray())));
            // + derivative of right lock function
            dYextRight.AddRange(Poly(Xext.GetRange(Nleft + Nmain, Nright).ToArray(), PolyDerivative(p2right)));
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
