using OdeSystemSolverLibrary.Solvers;

namespace OdeSystemSolverLibrary.ButcherSolvers
{
    public interface IButcherSolver
    {
        public void Solve(OdeFunction function, double dt, double t, double[] x, double[][] a, double[] b, double[] c, double[][] k);
    }
}
