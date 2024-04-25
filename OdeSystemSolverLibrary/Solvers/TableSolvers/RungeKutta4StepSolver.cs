using OdeSystemSolverLibrary.Solvers.TableSolvers.ButcherTables;

namespace OdeSystemSolverLibrary.Solvers.TableSolvers
{
    public class RungeKutta4StepSolver : OdeTableStepSolver
    {
        public RungeKutta4StepSolver(double dt, int equationsCount) : base(new RungeKutta4Table(), dt, equationsCount)
        {

        }
    }
}
