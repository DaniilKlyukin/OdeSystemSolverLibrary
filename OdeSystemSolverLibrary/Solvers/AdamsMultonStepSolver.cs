namespace OdeSystemSolverLibrary.Solvers
{

    public class AdamsMultonStepSolver : AdamsStepSolver
    {
        public override int StagesCount => 5;

        public AdamsMultonStepSolver(
            double dt, int equationsCount, double tolerance)
            : base(dt, equationsCount)
        {
            this.tolerance = tolerance;
        }

        private readonly double tolerance;

        protected override double[][] getCoefs()
        {
            return
                [
                    [1.0],
                    [0.5, 0.5],
                    [-1.0 / 12, 8.0 / 12, 5.0 / 12],
                    [1.0 / 24, -5.0 / 24, 19.0 / 24, 9.0 / 24],
                    [-19.0 / 720, 106.0 / 720, -264.0 / 720, 646.0 / 720, 251.0 / 720]
                ];
        }

        protected override double[] solveAdamsInnerStep(int fRow)
        {
            var tNew = t + dt;
            var xNew = new double[x.Length];
            Array.Copy(x, xNew, x.Length);

            double epsAbsMax;
            do
            {
                epsAbsMax = 0;

                Function.Invoke(tNew, xNew, f[fRow]);

                for (int i = 0; i < x.Length; i++)
                {
                    var sum = 0.0;
                    for (int j = 0; j < a[fRow].Length; j++)
                        sum += a[fRow][j] * f[j][i];

                    var tempEps = xNew[i];

                    xNew[i] = x[i] + dt * sum;

                    tempEps -= xNew[i];
                    epsAbsMax = Math.Max(Math.Abs(tempEps), epsAbsMax); // Находим ошибку
                }
            } while (epsAbsMax > tolerance);

            return xNew;
        }
    }
}
