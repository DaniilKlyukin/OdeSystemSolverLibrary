namespace OdeSystemSolverLibrary.Solvers.TableSolvers.ButcherTables
{
    public interface IAdaptiveTable : IButcherTable
    {
        public double[] GetBStar();
    }
}
