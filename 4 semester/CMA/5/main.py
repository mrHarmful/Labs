from sympy import *
from sympy.core.mod import *

EPS = 0.000001

a = -14.4621
b = 60.6959
c = -70.9238

x = Symbol('x')
fx = x**3 + a*x**2 + b*x + c
#fx = x

_sturm_cache = {}
def sturm_seq(f):
	if f in _sturm_cache:
		return _sturm_cache[f]

	r = []
	r += [f, diff(f)]
	i = 1

	while not r[i].is_Number:
		_, n = div(r[i-1], r[i])
		r += [-n]
		i += 1
	
	_sturm_cache[f] = r
	return r


def count_sign_changes(ss, x):
	l = None
	c = 0

	for s in ss:
		su = s.subs('x', x)
		if l is not None:
			if abs(abs(l)/l - abs(su)/su) > EPS:
				c += 1
		l = su

	return c


def count_roots_between(fx, l, r):
	ss = sturm_seq(fx)
	return count_sign_changes(ss, l) - count_sign_changes(ss, r)


def isolate_next_root(fx, l, r):
	ll = l
	while count_roots_between(fx, ll, r) > 1:
		if count_roots_between(fx, l, (l+r)/2) > 1:
			r = (l+r)/2
		elif count_roots_between(fx, l, (l+r)/2) == 0:
			l = (l+r)/2
		else:
			return (l+r)/2
	return r


def split_roots(fx, l, r):
	res = []
	i = l
	while i < r:
		l = i
		i = isolate_next_root(fx, i, r)
		res += [[l,i]]
	return res



print solve(fx)
print split_roots(fx, -100, 100)
