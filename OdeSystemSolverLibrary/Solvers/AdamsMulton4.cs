using OdeSystemSolverLibrary.Solvers.TableSolvers;

namespace OdeSystemSolverLibrary.Solvers
{

    public class AdamsMulton4 : AdamsStepSolver
    {
        public override int StagesCount => 5;

        public override int InitialSteps => 4;

        private readonly double tolerance;

        public AdamsMulton4(
            double dt, int equationsCount, double tolerance)
            : base(dt, equationsCount)
        {
            this.tolerance = tolerance;

            fArr = new double[StagesCount][];

            for (int i = 0; i < fArr.Length; i++)
            {
                fArr[i] = new double[equationsCount];
            }
        }

        protected override double[] getCoefs()
        {
            return
                [-19.0 / 720, 106.0 / 720, -264.0 / 720, 646.0 / 720, 251.0 / 720];
        }

        protected override void solveStep()
        {
            var fRow = Math.Min(iteration + 1, fArr.Length - 1);

            var xNew = new double[x.Length];

            boosterSolver.SolveStep();
            var tNew = boosterSolver.t;

            Function.Invoke(tNew, boosterSolver.x, fArr[fRow]);

            double epsAbsMax;
            do
            {
                epsAbsMax = 0;
                for (int i = 0; i < x.Length; i++)
                {
                    var sum = 0.0;
                    for (int j = 0; j < a.Length; j++)
                        sum += a[j] * fArr[j][i];

                    var tempEps = xNew[i];

                    xNew[i] = x[i] + dt * sum;

                    tempEps -= xNew[i];

                    epsAbsMax = Math.Max(Math.Abs(tempEps), epsAbsMax); // Находим ошибку
                }

                Function.Invoke(tNew, xNew, fArr[fRow]);

            } while (epsAbsMax > tolerance);

            for (int i = 1; i < fArr.Length; i++)
            {
                for (int j = 0; j < fArr[i].Length; j++)
                {
                    fArr[i - 1][j] = fArr[i][j];
                }
            }

            x = xNew;
            Function.Invoke(tNew, x, fArr[fArr.Length - 1]);
        }

        public override void Reset()
        {
            base.Reset();

            _solveStep = solveInitialStep;

            boosterSolver = new DormandPrince87(dt, x.Length)
            {
                Function = Function,
                t = t,
                x = new double[x.Length],
                dtMaxMultiplier = 1,
                Tolerance = 1e-10,
                EpsilonVectorNorm = (eps) => eps.Sum(e => Math.Abs(e)),
            };
            Array.Copy(x, boosterSolver.x, x.Length);
            Function.Invoke(t, x, fArr[0]);
        }
    }
}
