# Решатель систем дифференциальных уравнений / Solver of systems of differential equations

## Реализованные методы / Implemented methods
1. Явный Рунге-Кутта 4-го порядка / Explicit Runge-Kutta of the 4th order
2. Явный адаптивный Рунге-Кутта-Фельберг 5-го порядка / Explicit adaptive Runge-Kutta-Felberg 5th order
3. Явный адаптивный Дорманда-Принса 8-го порядка / Explicit 8th-order adaptive Dormand-Prince
4. Явный Адамса-Башфорта 4-го порядка / Explicit Adams-Bashfort of the 4th order
5. Неявный адаптивный Гаусса-Лежандра 6-го порядка / Implicit adaptive Gauss-Legendre of the 6th order
6. Неявный Адамса-Мультона 4-го порядка / The implicit Adams-Multon of the 4th order

## Примеры / Examples
Для визуализации используется библиотека ScottPlot. Тестирование проводится на Аттрактор Лоренца
### Пошаговое решение СОДУ
Каждый из решателей используется пошагово

``` cs
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
```
