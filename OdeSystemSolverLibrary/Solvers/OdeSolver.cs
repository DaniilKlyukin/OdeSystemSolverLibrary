namespace OdeSystemSolverLibrary
{
    public delegate void OdeObserver(double t, double[] x);
    public delegate bool OdeStop(double t, double[] x);
    public class OdeSolver
    {
        public required OdeStepSolver StepSolver { get; set; }
        public required OdeStop Stop { get; set; }
        public OdeObserver? Observer { get; set; }
        public EndInterpolator? EndInterpolator { get; set; }

        private double tPrev;
        private double[] xPrev;

        public virtual void Solve()
        {
            tPrev = StepSolver.t;
            xPrev = new double[StepSolver.x.Length];
            Array.Copy(StepSolver.x, xPrev, StepSolver.x.Length);

            Observer?.Invoke(StepSolver.t, StepSolver.x);

            while (true)
            {
                StepSolver.SolveStep();

                var isStop = Stop.Invoke(StepSolver.t, StepSolver.x);

                if (isStop)
                {
                    if (EndInterpolator != null)
                        (StepSolver.t, StepSolver.x) = EndInterpolator.Interpolate(StepSolver, tPrev, xPrev, StepSolver.t, StepSolver.x);

                    Observer?.Invoke(StepSolver.t, StepSolver.x);
                    break;
                }

                Observer?.Invoke(StepSolver.t, StepSolver.x);

                tPrev = StepSolver.t;
                Array.Copy(StepSolver.x, xPrev, StepSolver.x.Length);
            }
        }
    }
}
