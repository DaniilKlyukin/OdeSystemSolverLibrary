namespace OdeSystemSolverLibrary.Solvers
{
    public delegate void AdamsSolveStep();
    public abstract class AdamsStepSolver : OdeStepSolver
    {
        public abstract int StagesCount { get; }
        public abstract int InitialSteps { get; }

        protected AdamsSolveStep _solveStep;
        protected double[][] fArr;
        protected OdeStepSolver? boosterSolver;

        public AdamsStepSolver(
            double dt, int equationsCount)
            : base(dt, equationsCount)
        {
            _solveStep = solveInitialStep;
            a = getCoefs();
            fArr = new double[StagesCount][];

            for (int i = 0; i < fArr.Length; i++)
            {
                fArr[i] = new double[equationsCount];
            }
        }

        protected readonly double[] a;
        protected int iteration;

        public override void SolveStep()
        {
            _solveStep.Invoke();

            t += dt;
            iteration++;
        }

        protected abstract double[] getCoefs();

        protected virtual void solveInitialStep()
        {
            if (boosterSolver == null)
                throw new ArgumentNullException("Need to call Reset before start solving");

            boosterSolver.SolveStep();
            Array.Copy(boosterSolver.x, x, x.Length);

            Function.Invoke(boosterSolver.t, boosterSolver.x, fArr[iteration + 1]);

            if (iteration + 1 == InitialSteps - 1)
                _solveStep = solveStep;
        }

        protected abstract void solveStep();        

        public virtual void Reset()
        {
            iteration = 0;
        }
    }
}
