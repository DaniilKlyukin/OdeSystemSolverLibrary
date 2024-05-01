namespace OdeSystemSolverLibrary.Solvers.TableSolvers.ButcherTables
{
    public class RungeKutta4Table : IButcherTable
    {
        public int StagesCount => 4;

        public double[][] GetA() => [[0], [0.5], [0, 0.5], [0, 0, 1]];

        public double[] GetB() => [1.0 / 6, 1.0 / 3, 1.0 / 3, 1.0 / 6];

        public double[] GetC() => [0, 0.5, 0.5, 1];

        public IButcherSolver GetButcherSolver() => new ExplicitButcher();
    }
}
