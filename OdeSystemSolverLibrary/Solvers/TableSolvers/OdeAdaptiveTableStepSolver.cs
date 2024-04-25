using OdeSystemSolverLibrary.Solvers.TableSolvers.ButcherTables;

namespace OdeSystemSolverLibrary.Solvers.TableSolvers
{
    public delegate double EpsilonVectorNorm(double[] errors);
    public abstract class OdeAdaptiveTableStepSolver : OdeTableStepSolver
    {
        public required EpsilonVectorNorm EpsilonVectorNorm { get; set; }
        public required double dtMaxMultiplier { get; set; }
        public required double Tolerance { get; set; }

        public double dt0 { get; protected init; }

        protected readonly double[] bStar;
        private readonly int methodDegree;

        protected OdeAdaptiveTableStepSolver(
            IAdaptiveButcherTable butcherTable,
            double dt,
            int equationsCount,
            int methodDegree)
            : base(butcherTable, dt, equationsCount)
        {
            dt0 = dt;
            bStar = butcherTable.GetBStar();
            this.methodDegree = methodDegree;
        }

        public override void SolveStep()
        {
            butcherSolver.Solve(Function, dt, t, x, a, b, c, k);
            t = t + dt;

            dt = getNewTimeStep();
        }

        protected double getNewTimeStep()
        {
            var epsilon = new double[x.Length]; // Массив ошибок по каждой переменной системы

            for (int z = 0; z < x.Length; ++z)
            {
                for (int i = 0; i < k.Length; ++i)
                {
                    epsilon[z] += dt * (b[i] - bStar[i]) * k[i][z];
                }
            }

            double err = EpsilonVectorNorm.Invoke(epsilon); // Ошибка по координатам по которой выбирается новый шаг

            double newDt = dt * Math.Pow(Tolerance / err, 1.0 / methodDegree);

            if (newDt > dtMaxMultiplier * dt0) // Ограничение на выбор шага, чтобы он не был слишком большой или слишком маленький
                return dtMaxMultiplier * dt0;
            else if (newDt < dt0 / dtMaxMultiplier)
                return dt0 / dtMaxMultiplier;

            return newDt;
        }
    }
}
