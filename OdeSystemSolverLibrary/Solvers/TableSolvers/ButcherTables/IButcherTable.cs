namespace OdeSystemSolverLibrary.Solvers.TableSolvers.ButcherTables
{
    public interface IButcherTable
    {
        public int StagesCount { get; }
        public double[][] GetA();
        public double[] GetB();
        public double[] GetC();
        public IButcherSolver GetButcherSolver();
    }
}
