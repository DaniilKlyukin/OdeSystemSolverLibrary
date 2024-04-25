using OdeSystemSolverLibrary.Solvers.TableSolvers.ButcherTables;

namespace OdeSystemSolverLibrary.Solvers.TableSolvers
{
    public class RungeKuttaFehlberg54StepSolver : OdeAdaptiveTableStepSolver
    {
        public RungeKuttaFehlberg54StepSolver(
             double dt,
             int equationsCount)
            : base(
                  new RungeKuttaFehlberg54Table(),
                  dt,
                  equationsCount,
                  5)
        {

        }
    }
}
