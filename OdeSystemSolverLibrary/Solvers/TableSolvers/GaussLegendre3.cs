using OdeSystemSolverLibrary.Solvers.TableSolvers.ButcherTables;

namespace OdeSystemSolverLibrary.Solvers.TableSolvers
{
    public class GaussLegendre3 : OdeAdaptiveTableStepSolver
    {
        public GaussLegendre3(
            double dt, int equationsCount, double tableSolverTolerance)
            : base(new GaussLegendre3Table(tableSolverTolerance), dt, equationsCount, 6)
        {

        }
    }
}
