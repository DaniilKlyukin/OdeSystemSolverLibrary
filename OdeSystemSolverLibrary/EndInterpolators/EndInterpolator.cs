namespace OdeSystemSolverLibrary
{
    public delegate double OdeDistanceToStop(double t, double[] x);
    public abstract class EndInterpolator
    {
        public required OdeDistanceToStop OdeDistanceToStop { get; set; }
        public abstract (double, double[]) Interpolate(OdeStepSolver solver, double t0, double[] x0, double t1, double[] x1);
    }
}
