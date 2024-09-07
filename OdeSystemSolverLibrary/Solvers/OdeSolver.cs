using OdeSystemSolverLibrary.EndInterpolators;

namespace OdeSystemSolverLibrary.Solvers
{
    public delegate void OdeObserver(long iteration, double t, double[] x);
    public delegate bool OdeStop(long iteration, double t, double[] x);
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
            var iteration = 0L;
            tPrev = StepSolver.t;
            xPrev = new double[StepSolver.x.Length];
            Array.Copy(StepSolver.x, xPrev, StepSolver.x.Length);

            Observer?.Invoke(iteration,StepSolver.t, StepSolver.x);
            iteration = 1;

            while (true)
            {
                StepSolver.SolveStep();

                var isStop = Stop.Invoke(iteration, StepSolver.t, StepSolver.x);

                if (isStop)
                {
                    if (EndInterpolator != null)
                        (StepSolver.t, StepSolver.x) = EndInterpolator.Interpolate(StepSolver, tPrev, xPrev, StepSolver.t, StepSolver.x);

                    Observer?.Invoke(iteration, StepSolver.t, StepSolver.x);
                    break;
                }

                Observer?.Invoke(iteration, StepSolver.t, StepSolver.x);

                tPrev = StepSolver.t;
                iteration++;
                Array.Copy(StepSolver.x, xPrev, StepSolver.x.Length);
            }
        }
    }
}
