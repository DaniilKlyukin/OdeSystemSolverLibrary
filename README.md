# Решатель систем дифференциальных уравнений / Solver of systems of differential equations

## Реализованные методы / Implemented methods
1. Явный Рунге-Кутта 4-го порядка / Explicit Runge-Kutta of the 4th order
2. Явный адаптивный Рунге-Кутта-Фельберга 5-го порядка / Explicit adaptive Runge-Kutta-Felberg 5th order
3. Явный адаптивный Дорманда-Принса 8-го порядка / Explicit 8th-order adaptive Dormand-Prince
4. Явный Адамса-Башфорта 4-го порядка / Explicit Adams-Bashfort of the 4th order
5. Неявный адаптивный Гаусса-Лежандра 6-го порядка / Implicit adaptive Gauss-Legendre of the 6th order
6. Неявный Адамса-Мультона 4-го порядка / The implicit Adams-Multon of the 4th order

## Примеры / Examples
Для визуализации используется библиотека ScottPlot. Тестирование проводится на Аттракторе Лоренца:
https://en.wikipedia.org/wiki/Lorenz_system

### Пошаговое решение СОДУ
Рассмотрим пример пошагового решения с помощью метода Рунге-Кутта 4-го порядка.

``` cs
using OdeSystemSolverLibrary;
using OdeSystemSolverLibrary.Solvers.TableSolvers;

var x0 = new List<double>();
var x2 = new List<double>();

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

var x0 = new List<double>();
var x2 = new List<double>();

while (rk4Step.t <= 100)
{
    rk4Step.SolveStep();
    x0.Add(rk4Step.x[0]);
    x2.Add(rk4Step.x[2]);
}

var scatter = formsPlot.Plot.Add.Scatter(x2, x0);
scatter.MarkerShape = ScottPlot.MarkerShape.None;
scatter.Label = "Runge-Kutta 4";

formsPlot.Plot.ShowLegend();
```
![image](https://github.com/DaniilKlyukin/OdeSystemSolverLibrary/assets/32903150/40411194-f6cc-4d56-b9cd-1b9bb5dbb5a6)

Пошаговый решатель также удобен при решении задач, где необходимо менять функцию правых частей и перерешивать пройденный шаг.

``` cs
rk4Step.t = t; // Задаём t
rk4Step.x = new double[3]; // Задаём x
rk4Step.Function = (t, x, dxdt) => // Задаём новую функцию правых частей
{
    // ...
};
// ...
rk4Step.SolveStep();
```

### Решение СОДУ до некоторого условия завершения

Любой пошаговый решатель можно передать в класс OdeSolver для решения СОДУ до условия окончания расчета.
Пример с адаптивным методом Рунге-Кутта-Фельберга 5-го порядка.

``` cs
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

var sc1 = formsPlot.Plot.Add.Scatter(x2, x0);
sc1.MarkerShape = ScottPlot.MarkerShape.None;
sc1.Label = label;
```
