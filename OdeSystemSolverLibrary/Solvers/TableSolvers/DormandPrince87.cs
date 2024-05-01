using OdeSystemSolverLibrary.Solvers.TableSolvers.ButcherTables;

namespace OdeSystemSolverLibrary.Solvers.TableSolvers
{
    public class DormandPrince87 : OdeAdaptiveTableStepSolver
    {
        public DormandPrince87(
            double dt,
            int equationsCount)
            : base(new DormandPrince87Table(), dt, equationsCount, 8)
        {
        }
    }
}
