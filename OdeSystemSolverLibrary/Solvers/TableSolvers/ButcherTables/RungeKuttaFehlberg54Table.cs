namespace OdeSystemSolverLibrary.Solvers.TableSolvers.ButcherTables
{
    public class RungeKuttaFehlberg54Table : IAdaptiveButcherTable
    {
        public int StagesCount => 6;

        public double[][] GetA() =>
            [
            [],
            [0.25],
            [3.0 / 32,          9.0 / 32],
            [1932.0 / 2197,     -7200.0 / 2197,     7296.0 / 2197],
            [439.0 / 216,       -8,                 3680.0 / 513,       -845.0 / 4104],
            [-8.0 / 27,         2,                  -3544.0 / 2565,     1859.0 / 4104,      -11.0 / 40]
            ];

        public double[] GetB() =>
            [16.0 / 135, 0, 6656.0 / 12825, 28561.0 / 56430, -9.0 / 50, 2.0 / 55];

        public double[] GetBStar() =>
            [25.0 / 216, 0, 1408.0 / 2565, 2197.0 / 4104, -1.0 / 5, 0];

        public double[] GetC() =>
            [0, 0.25, 3.0 / 8, 12.0 / 13, 1, 0.5];

        public IButcherSolver GetButcherSolver() => new ExplicitButcherSolver();
    }
}
