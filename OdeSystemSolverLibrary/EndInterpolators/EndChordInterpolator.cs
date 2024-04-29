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
            solver.t = t1;
            Array.Copy(x1, solver.x, x1.Length);

            double f_0 = OdeDistanceToStop.Invoke(t0, x0);
            double f_1 = OdeDistanceToStop.Invoke(t1, x1);

            while (true)
            {
                var tTemp = t0 - f_0 * (t1 - t0) / (f_1 - f_0);
                double dt = tTemp - t1;

                solver.dt = dt;
                solver.SolveStep();

                var fTemp = OdeDistanceToStop?.Invoke(solver.t, solver.x) ?? 0;

                t0 = t1;
                t1 = tTemp;

                f_0 = f_1;
                f_1 = fTemp;

                if (Math.Abs(fTemp) <= tolerance || Math.Abs(dt) <= tolerance)
                    return (solver.t, solver.x);
            }
        }
    }
}
