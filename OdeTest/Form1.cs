using OdeSystemSolverLibrary;
using OdeSystemSolverLibrary.Solvers;
using OdeSystemSolverLibrary.Solvers.TableSolvers;
using ScottPlot.WinForms;
using System.Diagnostics;
using System.Security.Cryptography;
using OdeSystemSolverLibrary.EndInterpolators;

namespace OdeTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var dt = 1e-4;

            var rk4Step = new RungeKutta4(dt, 3)
            {
                Function = (t, x, dxdt) =>
                {
                    const double sigma = 10.0;
                    const double r = 28.0;
                    const double b = 8.0 / 3.0;
                    dxdt[0] = sigma * (x[1] - x[0]);
                    dxdt[1] = r * x[0] - x[1] - x[0] * x[2];
                    dxdt[2] = -b * x[2] + x[0] * x[1];
                },
                t = 0,
                x = [10, 1, 1]
            };

            SolveExample(rk4Step, "Runge-Kutta 4");

            var rkf54Step = new RungeKuttaFehlberg54(dt, 3)
            {
                Function = (t, x, dxdt) =>
                {
                    const double sigma = 10.0;
                    const double r = 28.0;
                    const double b = 8.0 / 3.0;
                    dxdt[0] = sigma * (x[1] - x[0]);
                    dxdt[1] = r * x[0] - x[1] - x[0] * x[2];
                    dxdt[2] = -b * x[2] + x[0] * x[1];
                },
                t = 0,
                x = [10, 1, 1],
                dtMaxMultiplier = 4,
                Tolerance = 1e-12,
                EpsilonVectorNorm = (eps) => eps.Sum(e => Math.Abs(e)),
            };

            SolveExample(rkf54Step, "Runge-Kutta-Fehlberg 54");

            var dp87Step = new DormandPrince87(dt, 3)
            {
                Function = (t, x, dxdt) =>
                {
                    const double sigma = 10.0;
                    const double r = 28.0;
                    const double b = 8.0 / 3.0;
                    dxdt[0] = sigma * (x[1] - x[0]);
                    dxdt[1] = r * x[0] - x[1] - x[0] * x[2];
                    dxdt[2] = -b * x[2] + x[0] * x[1];
                },
                t = 0,
                x = [10, 1, 1],
                dtMaxMultiplier = 4,
                Tolerance = 1e-12,
                EpsilonVectorNorm = (eps) => eps.Sum(e => Math.Abs(e)),
            };

            SolveExample(dp87Step, "Dormand-Prince 87");

            var ab4Step = new AdamsBashforth4(dt, 3)
            {
                Function = (t, x, dxdt) =>
                {
                    const double sigma = 10.0;
                    const double r = 28.0;
                    const double b = 8.0 / 3.0;
                    dxdt[0] = sigma * (x[1] - x[0]);
                    dxdt[1] = r * x[0] - x[1] - x[0] * x[2];
                    dxdt[2] = -b * x[2] + x[0] * x[1];
                },
                t = 0,
                x = [10, 1, 1],
            };
            ab4Step.Reset();
            SolveExample(ab4Step, "Adams-Bashfort 4");

            var gl6Step = new GaussLegendre3(dt, 3, 1e-12)
            {
                Function = (t, x, dxdt) =>
                {
                    const double sigma = 10.0;
                    const double r = 28.0;
                    const double b = 8.0 / 3.0;
                    dxdt[0] = sigma * (x[1] - x[0]);
                    dxdt[1] = r * x[0] - x[1] - x[0] * x[2];
                    dxdt[2] = -b * x[2] + x[0] * x[1];
                },
                t = 0,
                x = [10, 1, 1],
                dtMaxMultiplier = 4,
                Tolerance = 1e-12,
                EpsilonVectorNorm = (eps) => eps.Sum(e => Math.Abs(e)),
            };

            SolveExample(gl6Step, "Gauss-Legendre 6");

            var am4Step = new AdamsMulton4(dt, 3, 1e-12)
            {
                Function = (t, x, dxdt) =>
                {
                    const double sigma = 10.0;
                    const double r = 28.0;
                    const double b = 8.0 / 3.0;
                    dxdt[0] = sigma * (x[1] - x[0]);
                    dxdt[1] = r * x[0] - x[1] - x[0] * x[2];
                    dxdt[2] = -b * x[2] + x[0] * x[1];
                },
                t = 0,
                x = [10, 1, 1],
            };
            am4Step.Reset();
            SolveExample(am4Step, "Adams-Multon 4");

            formsPlot.Plot.ShowLegend(ScottPlot.Alignment.UpperRight);
        }

        private void SolveExample(OdeStepSolver stepSolver, string label)
        {
            var x0 = new List<double>();
            var x2 = new List<double>();

            var solver = new OdeSolver
            {
                StepSolver = stepSolver,
                Stop = (i, t, x) => t >= 35,
                EndInterpolator = new ChordInterpolator(1e-12)
                {
                    OdeDistanceToStop = (t, x) => t - 35
                },
                Observer = (i, t, x) =>
                {
                    x0.Add(x[0]);
                    x2.Add(x[2]);
                }
            };

            solver.Solve();

            var scatter = formsPlot.Plot.Add.Scatter(x2, x0);
            scatter.MarkerShape = ScottPlot.MarkerShape.None;
            scatter.Label = label;
            formsPlot.Plot.XLabel("Z");
            formsPlot.Plot.YLabel("X");
        }
    }
}
