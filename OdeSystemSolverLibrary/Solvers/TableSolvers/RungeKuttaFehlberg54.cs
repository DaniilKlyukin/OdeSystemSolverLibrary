using OdeSystemSolverLibrary.Solvers.TableSolvers.ButcherTables;

namespace OdeSystemSolverLibrary.Solvers.TableSolvers
{
    public class RungeKuttaFehlberg54 : OdeAdaptiveTableStepSolver
    {
        public RungeKuttaFehlberg54(
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
