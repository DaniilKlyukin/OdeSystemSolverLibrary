using OdeSystemSolverLibrary.Solvers.TableSolvers.ButcherTables;

namespace OdeSystemSolverLibrary.Solvers.TableSolvers
{
    public class DormandPrince87StepSolver : OdeAdaptiveTableStepSolver
    {
        public DormandPrince87StepSolver(
            double dt,
            int equationsCount)
            : base(new DormandPrince87Table(), dt, equationsCount, 8)
        {
        }
    }
}
