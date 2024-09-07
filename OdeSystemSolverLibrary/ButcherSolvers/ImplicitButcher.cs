using OdeSystemSolverLibrary.Solvers;

namespace OdeSystemSolverLibrary.ButcherSolvers
{
    public class ImplicitButcher : IButcherSolver
    {
        private readonly double tolerance;

        public ImplicitButcher(double tolerance)
        {
            this.tolerance = tolerance;
        }

        public void Solve(OdeFunction function, double dt, double t, double[] x, double[][] a, double[] b, double[] c, double[][] k)
        {
            var xTemp = new double[x.Length];
            var sums = new double[x.Length];

            var kiErr = new double[x.Length]; // Временный массив k_i для расчета ошибки
            double epsAbsMax;

            do
            {
                epsAbsMax = 0;

                for (int i = 0; i < k.Length; ++i)
                {
                    for (int j = 0; j < x.Length; ++j)
                        sums[j] = 0;

                    for (int j = 0; j < k.Length; ++j)
                    {
                        for (int z = 0; z < k[j].Length; ++z)
                        {
                            sums[z] += a[i][j] * k[j][z];
                        }
                    }

                    for (int z = 0; z < x.Length; ++z)
                    {
                        xTemp[z] = x[z] + dt * sums[z];

                        kiErr[z] = k[i][z]; // Сохраняем текущие k
                    }

                    function(t + c[i] * dt, xTemp, k[i]);

                    for (int z = 0; z < x.Length; ++z)
                        epsAbsMax = Math.Max(Math.Abs(kiErr[z] - k[i][z]), epsAbsMax); // Находим ошибку по k
                }

            } while (epsAbsMax >= tolerance);

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
