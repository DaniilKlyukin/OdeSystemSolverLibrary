using OdeSystemSolverLibrary.Solvers.TableSolvers;

namespace OdeSystemSolverLibrary.Solvers
{
    public class AdamsBashforth4StepSolver : AdamsStepSolver
    {
        public override int StagesCount => 5;

        public override int InitialSteps => 5;

        public AdamsBashforth4StepSolver(
            double dt, int equationsCount)
            : base(dt, equationsCount)
        {

        }

        protected override double[] getCoefs()
        {
            return
                [251.0 / 720, -1274.0 / 720, 2616.0 / 720, -2774.0 / 720, 1901.0 / 720];
        }

        protected override void solveStep()
        {
            for (int i = 0; i < x.Length; i++)
            {
                var sum = 0.0;
                for (int j = 0; j < a.Length; j++)
                    sum += a[j] * fArr[j][i];

                x[i] += dt * sum;
            }

            for (int i = 1; i < fArr.Length; i++)
            {
                for (int j = 0; j < fArr[i].Length; j++)
                {
                    fArr[i - 1][j] = fArr[i][j];
                }
            }

            Function.Invoke(t + dt, x, fArr[fArr.Length - 1]);
        }

        public override void Reset()
        {
            base.Reset();

            _solveStep = solveInitialStep;

            boosterSolver = new DormandPrince87StepSolver(dt, x.Length)
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
