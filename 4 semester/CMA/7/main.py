import math
import numpy as np
import matplotlib.pyplot as plt
import matplotlib.lines as lines
import matplotlib.transforms as mtransforms
import matplotlib.text as mtext


def fx(x):
	return math.sin(x)/(x+2)

L = 1
R = 10

S = 1.
C = int((R-L)/S)

fig = plt.figure()
ga = fig.add_subplot(111)

# Approximation ---------------------
r = 0

aa = []
bb = []
cc = []
dd = []
y = [0] * int((R-L)/S+1)
x = [0] * int((R-L)/S+1)

for i in range(0, C+1):
	x[i] = L+S*i
	y[i] = fx(x[i])

mc = []
bc = []
for i in range(0, C-1):
	l =  [0]*(i)
	l += [S/3]
	l += [S*4/3]
	l += [S/3]
	l += [0]*(C-i-2)
	mc += [l]
	bc += [[(y[i+1]-y[i])/S - (y[i]-y[i-1])/S]]


l = [1]
l +=  [0]*(C)
mc += [l]

l =  [0]*(C)
l += [1]
mc += [l]

bc += [[0],[0]]

mc = np.matrix(mc)
bc = np.matrix(bc)

for l in np.linalg.solve(mc,bc):
	cc += [l.item(0,0)]

cc += [0]

for i in range(0,C):
	bb += [(y[i]-y[i-1])/S-cc[i]*S-(cc[i+1]-cc[i])*S/3]
bb[0] = (y[i]-fx(L-S))/S-cc[i]*S-(cc[i+1]-cc[i])*S/3

bb += [0]

aa += [fx(L-S)]
for i in range(1,C):
	aa += [y[i-1]]

for i in range(0,C):
	dd += [(cc[i+1]-cc[i])/S/3]


for i in range(1,C-1):
	def appr(xx):
		return aa[i] + bb[i]*(xx-x[i]) + cc[i]*(xx-x[i])**2 + dd[i]*(xx-x[i])**3
	#print (L+R)/2,x[i],x[i+1]
	if (L+R)/2 >= x[i] and (L+R)/2 < x[i+1]:
		print 'Spline: %.5f' % appr((L+R)/2+S)
		print 'Function: %.5f' % fx((L+R)/2)
	xs = np.arange(x[i], x[i+1]+S/8, S/8)
	ys = [appr(xx) for xx in xs]
	xs = [xx-S for xx in xs]
	ga.plot(xs,ys)

ga.grid()

xs = np.arange(L, R-S, S/8)
ys = [fx(xx) for xx in xs]
ga.plot(xs,ys)


fig.show()
fig.waitforbuttonpress()
