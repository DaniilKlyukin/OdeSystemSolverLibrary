namespace OdeSystemSolverLibrary
{
    public class EndChordInterpolator : EndInterpolator
    {
        private readonly double tolerance;

        public EndChordInterpolator(double tolerance)
        {
            this.tolerance = tolerance;
        }

        public override (double, double[]) Interpolate(OdeStepSolver solver, double t0, double[] x0, double t1, double[] x1)
        {
            var t2 = t1;
            var x2 = new double[x1.Length];
            Array.Copy(x1, x2, x1.Length);

            double f_0 = OdeDistanceToStop.Invoke(t0, x0);
            double f_1 = OdeDistanceToStop.Invoke(t1, x1);

            while (true)
            {
                t2 = t0 - f_0 * (t1 - t0) / (f_1 - f_0);
                for (int j = 0; j < x1.Length; ++j)
                    x2[j] = x0[j] - f_0 * (x1[j] - x0[j]) / (f_1 - f_0);

                double dt = t2 - t1;

                solver.t = t1;
                solver.dt = dt;
                solver.x = x2;
                solver.SolveStep();

                t0 = t1;
                t1 = t2;

                f_0 = f_1;
                f_1 = OdeDistanceToStop?.Invoke(t2, x2) ?? 0;

                Array.Copy(x1, x0, x1.Length);
                Array.Copy(x2, x1, x1.Length);

                if (Math.Abs(f_0 - f_1) <= tolerance)
                    return (t2, x2);
            }
        }
    }
}
