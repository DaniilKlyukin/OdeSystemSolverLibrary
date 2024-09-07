using OdeSystemSolverLibrary.ButcherSolvers;

namespace OdeSystemSolverLibrary.Solvers.TableSolvers.ButcherTables
{
    public class GaussLegendre3Table : IAdaptiveTable
    {
        private readonly double tolerance;
        private double SQRT15 = Math.Sqrt(15);

        public GaussLegendre3Table(double tolerance)
        {
            this.tolerance = tolerance;
        }

        public int StagesCount => 3;

        public double[][] GetA()
        {
            return [
                [5.0 / 36,                  2.0 / 9 - 1.0 / SQRT15,     5.0 / 36 - 0.5 / SQRT15],
                [5.0 / 36 + SQRT15 / 24,    2.0 / 9,                    5.0 / 36 - SQRT15 / 24],
                [5.0 / 36 + 0.5 / SQRT15,   2.0 / 9 + 1.0 / SQRT15,     5.0 / 36]];
        }

        public double[] GetB() => [5.0 / 18, 4.0 / 9, 5.0 / 18];

        public double[] GetBStar() => [-5.0 / 6, 8.0 / 3, -5.0 / 6];

        public double[] GetC() => [0.5 - SQRT15 / 10, 0.5, 0.5 + SQRT15 / 10];

        public IButcherSolver GetButcherSolver() => new ImplicitButcher(tolerance);

    }
}
