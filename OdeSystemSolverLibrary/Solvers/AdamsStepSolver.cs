namespace OdeSystemSolverLibrary.Solvers
{
    public abstract class AdamsStepSolver : OdeStepSolver
    {
        public abstract int StagesCount { get; }

        public AdamsStepSolver(
            double dt, int equationsCount)
            : base(dt, equationsCount)
        {
            f = new double[StagesCount][];

            for (int i = 0; i < f.Length; i++)
            {
                f[i] = new double[equationsCount];
            }

            a = getCoefs();
        }

        protected readonly double[][] f;
        protected readonly double[][] a;
        protected int iteration;

        protected abstract double[][] getCoefs();

        public override void SolveStep()
        {
            var fRow = iteration;

            if (iteration > StagesCount - 1)
            {
                fRow = StagesCount - 1;

                for (int i = 1; i <= fRow; i++)
                {
                    for (int j = 0; j < f[i].Length; j++)
                    {
                        f[i - 1][j] = f[i][j];
                    }
                }
            }

            x = solveAdamsInnerStep(fRow);

            t += dt;
            iteration++;
        }

        protected abstract double[] solveAdamsInnerStep(int fRow);

        public virtual void Reset()
        {
            iteration = 0;
        }
    }
}
