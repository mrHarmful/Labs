from sympy import *
from sympy.core.mod import *

EPS = 0.000001

a = -14.4621
b = 60.6959
c = -70.9238

x = Symbol('x')
fx = x**3 + a*x**2 + b*x + c
fx = x

def sturm_seq(f):
	r = []
	r += [f, diff(f)]
	i = 1

	while not r[i].is_Number:
		_, n = div(r[i-1], r[i])
		r += [-n]
		i += 1

	return r

def count_sign_changes(ss, x):
	l = None
	c = 0

	for s in ss:
		su = s.subs('x', x)
		if l is not None:
			if abs(l)/l != abs(su)/su:
				c += 1
		l = su

	return c


ss = sturm_seq(fx)
print count_sign_changes(ss, -100)
print count_sign_changes(ss, 100)
