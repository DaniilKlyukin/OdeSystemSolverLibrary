# Решатель систем обыкновенных дифференциальных уравнений / Solver of systems of ordinary differential equations

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

```math
\begin{cases}
    \dfrac{dx}{dt}=\sigma\cdot(y-x) \\ \\
    \dfrac{dy}{dt}=x\cdot(\rho-z)-y \\ \\
    \dfrac{dz}{dt}=xy-\beta z
\end{cases}
```

```math
\begin{bmatrix}
t_0=0, & x_0=10, & y_0=1, & z_0=1
\end{bmatrix}
```
### Пошаговое решение СОДУ / Step-by-step solution of a system of ordinary differential equations
Рассмотрим пример пошагового решения с помощью метода Рунге-Кутта 4-го порядка.

Let's consider an example of a step-by-step solution using the 4th order Runge-Kutta method.

``` cs
using OdeSystemSolverLibrary;
using OdeSystemSolverLibrary.Solvers.TableSolvers;

var x0 = new List<double>();
var x2 = new List<double>();

var rk4Step = new RungeKutta4(0.001, 3)
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

![image](https://github.com/DaniilKlyukin/OdeSystemSolverLibrary/assets/32903150/64beca23-3e7c-4b36-a3f9-3356361bfbe0)

Пошаговый решатель также удобен при решении задач, где необходимо менять функцию правых частей и перерешивать пройденный шаг с новыми условиями.

The step-by-step solver is also convenient when solving problems where it is necessary to change the function of the right parts and solve again the completed step with new conditions.

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

### Решение СОДУ до некоторого условия завершения / Solving system of ordinary differential equations before some completion condition

Любой пошаговый решатель можно передать в класс OdeSolver для решения СОДУ до условия окончания расчета.
Пример с адаптивным методом Рунге-Кутта-Фельберга 5-го порядка.

Any step-by-step solver can be passed to the OdeSolver class to solve a system of ordinary differential equations before the calculation is completed.
An example with the adaptive Runge-Kutta-Felberg method of the 5th order.

``` cs
var rkf54 = new RungeKuttaFehlberg54(0.001, 3)
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
    EpsilonVectorNorm = (eps) => eps.Sum(e => Math.Abs(e))
};

var x0 = new List<double>();
var x2 = new List<double>();

var solver = new OdeSolver
{
    StepSolver = rkf54,
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

var sc = formsPlot.Plot.Add.Scatter(x2, x0);
sc.MarkerShape = ScottPlot.MarkerShape.None;
sc.Label = "Runge-Kutta-Felberg 5";
```

![image](https://github.com/DaniilKlyukin/OdeSystemSolverLibrary/assets/32903150/1a2afe25-b7b5-4349-b612-71a41bf289fa)

### Дополнительные параметры методов / Additional method parameters

Для всех адаптивных методов задается блок:

A block is set for all adaptive methods:

``` cs
dtMaxMultiplier = 4,                                   // во сколько раз может уменьшиться или увеличиться шаг;
Tolerance = 1e-6,                                      // точность расчета на шаге в соответствии с заданной нормой;
EpsilonVectorNorm = (eps) => eps.Sum(e => Math.Abs(e)) // метод расчета нормы ошибки на шаге;
```

Условие окончания расчета задается с помощью делегата

The condition for the end of the calculation is set using a delegate

``` cs
Stop = (t, x) => ...,
```

Условие окончания расчета можно наложить не только на независимую переменную t, но и на фазовые переменные **x**.

Чтобы избежать перескакивания за последнюю точке, в которой происходит окончание расчета, необходимо использовать интерполяцию:

To avoid jumping over the last point at which the calculation ends, it is necessary to use interpolation:

``` cs
EndInterpolator = new ChordInterpolator(1e-6)
{
    OdeDistanceToStop = (t, x) => t - 100
},
```

В данном примере класс ```ChordInterpolator``` ищет решение задачи  t - 100 = 0.

In this example, the class and its value are t - 100 = 0.

### Сравнение методов / Comparison of methods

![image](https://github.com/DaniilKlyukin/OdeSystemSolverLibrary/assets/32903150/2b4ba92e-f77f-4133-bf12-7c3754e0ef00)
![image](https://github.com/DaniilKlyukin/OdeSystemSolverLibrary/assets/32903150/2912205f-60e5-4a1a-902e-641fa3bb8b04)
![image](https://github.com/DaniilKlyukin/OdeSystemSolverLibrary/assets/32903150/0d2fd60b-6db4-46a4-8d42-907c0f7957cd)
![image](https://github.com/DaniilKlyukin/OdeSystemSolverLibrary/assets/32903150/bd290400-0be8-48a1-ac57-baa3c34bfc65)
![image](https://github.com/DaniilKlyukin/OdeSystemSolverLibrary/assets/32903150/29bff06e-0779-47c9-a99d-1280311bc665)
![image](https://github.com/DaniilKlyukin/OdeSystemSolverLibrary/assets/32903150/83b09472-b79f-48bb-81ba-43d8e623f669)

### Скорость методов / Speed of methods

В таблице ниже представлено сравнение скорости методов для задачи Лоренца при шаге интегрирования &Delta; = 2<sup>-13</sup>, момент окончания расчета t=100, адаптивные методы решали задачу с точностью 10<sup>-8</sup>.

The table below shows a comparison of the speed of the methods for the Lorentz problem at the integration step &Delta; = 2<sup>-13</sup>,at the end of the calculation, t=100, adaptive methods solved the problem with an accuracy of 10<sup>-8</sup>.

|        Метод            |         Время, мс      |
|-------------------------|------------------------|
| Рунге-Кутта 4           | 247                    |
| Рунге-Кутта-Фельберга 5 | 261                    |
| Дорманда-Принса 8       | 509                    |
| Адамса-Башфорта 4       | 199                    |
| Гаусса-Лежандра 6       | 1000                   |
| Адамса-Мультона 4       | 740                    |

### Точность методов / Accuracy of methods

Исследуем точность на примере

Let's examine the accuracy using an example

```math
\dfrac{dx}{dt}=x
```

```math
x_0=1
```

Аналитическое решение

Analytical solution

```math
x(t)=e^{t}
```

В таблице ниже представлено сравнение точности методов при шаге интегрирования &Delta; = 1/8, момент окончания расчета t=1, x(1)=2,718281828459045235360287471352...

The table below shows a comparison of the accuracy of the methods at the integration step &Delta; = 1/8, the moment of completion of the calculation t=1, x(1)=2,718281828459045235360287471352...

|        Метод            |       Решение      |         Ошибка         |
|-------------------------|--------------------|------------------------|
| Рунге-Кутта 4           | 2.7182815003405842 | 3.2811846084612739E-07 |
| Рунге-Кутта-Фельберга 5 | 2.7182817601331042 | 6.8325940905111793E-08 |
| Дорманда-Принса 8       | 2.7182818284590424 | 2.6645352591003757E-15 |
| Адамса-Башфорта 4       | 2.7182687707408806 | 1.3057718164510845E-05 |
| Гаусса-Лежандра 6       | 2.7182818285619783 | 1.0293321750509676E-10 |
| Адамса-Мультона 4       | 2.7182807852248594 | 1.0432341857047334E-06 |

## Пример падение объекта / Example of an object falling

Рассмотрим пример падение объекта с некоторой точки y<sub>0</sub> = 10 м, начальная скорость v<sub>0</sub> = 0 м/с. Момент времени, когда объект достигнет земли неизвестен, мы знаем только то, что в момент падения y = 0 м. Тогда условие окончания расчета будет y <= 0 и также нам необходимо интерполировать решение в точке y = 0.

Consider an example of an object falling from some point y<sub>0</sub> = 10 m, initial velocity v<sub>0</sub> = 0 m/s. The moment of time when the object reaches the earth is unknown, we only know that at the moment of falling y = 0 m. Then the condition for the end of the calculation will be y <= 0 and we also need to interpolate the solution at the point y = 0.

```math
\begin{cases}
    \dfrac{dv}{dt}=-g \\ \\
    \dfrac{dy}{dt}=v
\end{cases}
```

```math
\begin{bmatrix}
t_0=0, & v_0=0, & y_0=10
\end{bmatrix}
```

``` cs
var stepSolver = new GaussLegendre3(0.001, 2, 1e-6)
{
    Function = (t, x, dxdt) =>
    {
        const double g = 9.81;

        dxdt[0] = -g;
        dxdt[1] = x[0];
    },
    t = 0,
    x = [0, 10],
    dtMaxMultiplier = 4,
    Tolerance = 1e-6,
    EpsilonVectorNorm = (eps) => eps.Sum(e => Math.Abs(e)),
};

var solver = new OdeSolver
{
    StepSolver = stepSolver,
    Stop = (t, x) => x[1] <= 0,
    EndInterpolator = new ChordInterpolator(1e-6)
    {
        OdeDistanceToStop = (t, x) => x[1]
    },
};

solver.Solve();
```

![image](https://github.com/DaniilKlyukin/OdeSystemSolverLibrary/assets/32903150/5fcab53a-c3eb-466f-9384-6d9e69dce3cf)


