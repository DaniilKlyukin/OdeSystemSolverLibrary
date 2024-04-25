namespace OdeSystemSolverLibrary.Solvers.TableSolvers.ButcherTables
{
    public interface IAdaptiveButcherTable : IButcherTable
    {
        public double[] GetBStar();
    }
}
