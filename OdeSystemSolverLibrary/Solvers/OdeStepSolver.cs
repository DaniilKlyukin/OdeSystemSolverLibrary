namespace OdeSystemSolverLibrary
{
    public delegate void OdeFunction(double t, double[] x, double[] dxdt);
    public abstract class OdeStepSolver
    {
        public required OdeFunction Function { get; set; }

        public required double t { get; set; }

        public required double[] x { get; set; }

        public double dt { get; set; }

        public abstract void SolveStep();

        protected OdeStepSolver(double dt, int equationsCount)
        {
            this.dt = dt;
            x = new double[equationsCount];
        }
    }
}
