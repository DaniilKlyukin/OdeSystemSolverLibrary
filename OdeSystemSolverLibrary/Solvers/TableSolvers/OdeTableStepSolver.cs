using OdeSystemSolverLibrary.Solvers.TableSolvers.ButcherTables;

namespace OdeSystemSolverLibrary.Solvers.TableSolvers
{
    public abstract class OdeTableStepSolver : OdeStepSolver
    {
        protected readonly IButcherTable butcherTable;
        protected readonly IButcherSolver butcherSolver;
        protected readonly double[][] a;
        protected readonly double[] b;
        protected readonly double[] c;
        protected double[][] k { get; init; }

        protected OdeTableStepSolver(
            IButcherTable butcherTable, double dt, int equationsCount) : base(dt, equationsCount)
        {
            this.butcherTable = butcherTable;

            a = butcherTable.GetA();
            b = butcherTable.GetB();
            c = butcherTable.GetC();
            butcherSolver = butcherTable.GetButcherSolver();

            k = new double[butcherTable.StagesCount][];

            for (int i = 0; i < k.Length; i++)
                k[i] = new double[equationsCount];
        }

        public override void SolveStep()
        {
            butcherSolver.Solve(Function, dt, t, x, a, b, c, k);
            t = t + dt;
        }
    }
}
