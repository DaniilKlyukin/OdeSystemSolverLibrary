namespace OdeSystemSolverLibrary.Solvers
{
    public class AdamsBashforthStepSolver : AdamsStepSolver
    {
        public override int StagesCount => 5;

        public AdamsBashforthStepSolver(
            double dt, int equationsCount)
            : base(dt, equationsCount)
        {

        }

        protected override double[][] getCoefs()
        {
            return
                [
                    [1.0],
                    [-0.5, 1.5],
                    [5.0 / 12, -16.0 / 12, 23.0 / 12],
                    [-9.0 / 24, 37.0 / 24, -59.0 / 24, 55.0 / 24],
                    [251.0 / 720, -1274.0 / 720, 2616.0 / 720, -2774.0 / 720, 1901.0 / 720]
                ];
        }

        protected override double[] solveAdamsInnerStep(int fRow)
        {
            Function.Invoke(t, x, f[fRow]);

            for (int i = 0; i < x.Length; i++)
            {
                var sum = 0.0;
                for (int j = 0; j < a[fRow].Length; j++)
                    sum += a[fRow][j] * f[j][i];

                x[i] += dt * sum;
            }

            return x;
        }
    }
}
