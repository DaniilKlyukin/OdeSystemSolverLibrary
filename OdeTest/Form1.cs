using OdeSystemSolverLibrary;
using OdeSystemSolverLibrary.Solvers;
using OdeSystemSolverLibrary.Solvers.TableSolvers;

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
            /*
            var a4Step = new AdamsBashforthStepSolver(0.001, 3)
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
                //dtMaxMultiplier = 4,
                //EpsilonVectorNorm = (eps) => eps.Sum(e => Math.Abs(e)),
                //Tolerance = 1e-9,
                t = 0,
                x = [10, 1, 1]
            };*/

            var rk4Step = new RungeKutta4StepSolver(0.001, 3)
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

            var rkf54Step = new RungeKuttaFehlberg54StepSolver(0.001, 3)
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
                Tolerance = 1e-6,
                EpsilonVectorNorm = (eps) => eps.Sum(e => Math.Abs(e)),
            };

            SolveExample(rkf54Step, "Runge-Kutta-Fehlberg 54");

            var dp87Step = new DormandPrince87StepSolver(0.001, 3)
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
                Tolerance = 1e-6,
                EpsilonVectorNorm = (eps) => eps.Sum(e => Math.Abs(e)),
            };

            SolveExample(dp87Step, "Dormand-Prince 87");

            formsPlot1.Plot.ShowLegend();
        }

        private void SolveExample(OdeStepSolver stepSolver, string label)
        {
            var x0 = new List<double>();
            var x2 = new List<double>();

            var solver = new OdeSolver
            {
                StepSolver = stepSolver,
                Stop = (t, x) => t >= 100,
                EndInterpolator = new EndChordInterpolator(1e-6)
                {
                    OdeDistanceToStop = (t, x) => t - 100
                },
                Observer = (t, x) =>
                {
                    x0.Add(x[0]);
                    x2.Add(x[2]);
                }
            };

            solver.Solve();

            var sc1 = formsPlot1.Plot.Add.Scatter(x2, x0);
            sc1.MarkerShape = ScottPlot.MarkerShape.None;
            sc1.Label = label;
        }
    }
}
