using OdeSystemSolverLibrary.Solvers;

namespace OdeSystemSolverLibrary.ButcherSolvers
{
    public class ExplicitButcher : IButcherSolver
    {
        public void Solve(
            OdeFunction function,
            double dt,
            double t,
            double[] x,
            double[][] a, double[] b, double[] c, double[][] k)
        {
            var tempX = new double[x.Length];

            function.Invoke(t, x, k[0]);

            for (int i = 1; i < k.Length; ++i)
            {
                var tempSumX = new double[k[i].Length];

                for (int j = 0; j < i; ++j)
                {
                    for (int z = 0; z < k[i].Length; ++z)
                    {
                        tempSumX[z] += a[i][j] * k[j][z];
                    }
                }

                for (int z = 0; z < tempX.Length; ++z)
                {
                    tempX[z] = x[z] + dt * tempSumX[z];
                }

                function.Invoke(t + c[i] * dt, tempX, k[i]);
            }

            for (int z = 0; z < x.Length; ++z)
            {
                for (int i = 0; i < k.Length; ++i)
                {
                    x[z] += dt * b[i] * k[i][z];
                }
            }
        }
    }
}
