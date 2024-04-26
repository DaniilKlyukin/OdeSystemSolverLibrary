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
            double f_0 = OdeDistanceToStop.Invoke(t0, x0);
            double f_1 = OdeDistanceToStop.Invoke(t1, x1);

            while (true)
            {
                var tTemp = t0 - f_0 * (t1 - t0) / (f_1 - f_0);

                var xTemp = new double[x1.Length];
                for (int j = 0; j < x1.Length; ++j)
                    xTemp[j] = x0[j] - f_0 * (x1[j] - x0[j]) / (f_1 - f_0);

                double dt = tTemp - t1;

                solver.t = t1;
                solver.dt = dt;
                Array.Copy(x1, solver.x, x1.Length);
                solver.SolveStep();

                var fTemp = OdeDistanceToStop?.Invoke(solver.t, solver.x) ?? 0;

                if (fTemp > 0)
                {
                    t1 = solver.t;
                    Array.Copy(solver.x, x1, x1.Length);
                }
                else if (fTemp < 0)
                {
                    t0 = solver.t;
                    Array.Copy(solver.x, x1, x1.Length);
                }
                else if (Math.Abs(fTemp) <= tolerance)
                    return (solver.t, solver.x);
            }
        }
    }
}
