import matplotlib.pyplot as plt
import math

from sympy import diff
from sympy.abc import x
from sympy.core import Symbol
from sympy.solvers import solve

xmin = -1.0
xmax = 1.0

a = math.sin(2)
b = math.cos(2)
a=1;b=1


def colocations(steps):
    basis = [(x ** i * (1 - x ** 2)) for i in range(0, steps + 1)]
    basis[0] = 0

    xs = [xmin + (xmax - xmin) / steps * i for i in range(0, steps + 1)]
    ax = [1] + [Symbol('a%i' % i) for i in range(1, steps + 1)]

    yapprox = sum(ax[i] * basis[i] for i in range(0, steps + 1))
    error = a * diff(diff(yapprox, 'x'), 'x') + (1 + b * x ** 2) * yapprox + 1

    matrix = [error.subs(x, xs[i]) for i in range(0, steps )]

    sol = solve(matrix)
    y = yapprox.subs(sol)
    print sol
    return y


def plot_colocations(steps, ga, **kwargs):
    y = colocations(steps)
    gsteps = 100
    print y
    xs = [xmin + (xmax - xmin) / gsteps * i for i in range(0, gsteps + 1)]
    ys = [y.subs(x, xi) for xi in xs]
    ga.plot(xs, ys, **kwargs)


fig = plt.figure()

ga = fig.add_subplot(111)
ga.grid()


plot_colocations(3, ga, color='red')
plot_colocations(5, ga, color='green')
plot_colocations(20, ga, color='blue')

#ga.plot(*plot_euler(step_rounge_coutte), color='green')


fig.show()
while True:
    fig.waitforbuttonpress()
