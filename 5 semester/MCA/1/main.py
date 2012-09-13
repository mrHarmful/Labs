import math
import matplotlib
import matplotlib.pyplot as plt
import matplotlib.lines as lines
import matplotlib.transforms as mtransforms
import matplotlib.text as mtext
import sympy


a = 0.5
m = 1.0

df = lambda x,y : (a*(1-y*y)) / ((1+m)*x*x + y*y + 1)
x0 = 0
y0 = 0

h = 1
minx = 0.0
maxx = 1.0

def step_euler_normal(x,y):
	return y + h * df(x,y)

def step_euler_ext(x,y):
	return y + h * df(x+h/2,y + h/2 * df(x, y))

def step_rounge_coutte(x,y):
	k1 = h * df(x,y)
	k2 = h * df(x + h/2, y + k1/2)
	k3 = h * df(x + h/2, y + k2/2)
	k4 = h * df(x + h, y + k3)
	return y + (k1 + 2*k2 + 2*k3 + k4) / 6

def plot_euler(stepfx):
	x = x0
	y = y0
	xs = [x] if x >= minx else []
	ys = [y] if x >= minx else []
	for i in range(0, int(maxx/h)):
		y = stepfx(x, y)
		x += h
		if x >= minx:
			xs += [x]
			ys += [y]
	return xs, ys


def converge_epsilon(stepfx):
	global h
	h = h0 * 2
	yl = plot_euler(stepfx)[1][-1]
	yk = -111
	while abs(yk-yl) > e:
		h /= 2
		yk = yl
		xs,ys = plot_euler(stepfx)
		yl = ys[-1]
	print 'step size for %s : %f' % (str(stepfx), h)
	return xs, ys



e = 0.001
h0 = 0.5

fig = plt.figure()

# Euler normal
ga = fig.add_subplot(111)
h = 0.01
ga.plot(*converge_epsilon(step_euler_normal), color='red')
ga.grid()

# Euler extended
ga.plot(*converge_epsilon(step_euler_ext), color='blue')
ga.grid()

# R-K
ga.plot(*converge_epsilon(step_rounge_coutte), color='orange')
ga.grid()

# Euler precise
h = 0.0001
ga.plot(*converge_epsilon(step_euler_normal), color='green')
ga.grid()


fig.show()
while True:
	fig.waitforbuttonpress()
