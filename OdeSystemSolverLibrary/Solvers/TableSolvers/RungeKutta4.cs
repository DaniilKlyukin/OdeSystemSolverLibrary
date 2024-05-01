using OdeSystemSolverLibrary.Solvers.TableSolvers.ButcherTables;

namespace OdeSystemSolverLibrary.Solvers.TableSolvers
{
    public class RungeKutta4 : OdeTableStepSolver
    {
        public RungeKutta4(double dt, int equationsCount) : base(new RungeKutta4Table(), dt, equationsCount)
        {

        }
    }
}
