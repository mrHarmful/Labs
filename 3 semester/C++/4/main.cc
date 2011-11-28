#include <iostream>
#include <stdlib.h>
#include "matrix.cc"

using namespace std;


template <typename T>
Matrix<T>* randomize(Matrix<T>* d) {
	for (int x = 0; x < d->width; x++)
		for (int y = 0; y < d->height; y++)
			(*d)[x][y] = rand() % 10;
	return d;
}

int main() {
	Matrix<int>* a = randomize(new Matrix<int>(3,3));
	Matrix<int>* b = randomize(new Matrix<int>(3,3));
	Matrix<int>* c = (*a) - b;

	cout << endl << "A" << endl;
	a->print();

	cout << endl << "B" << endl;
	b->print();

	cout << endl << "A-B" << endl;
	c->print();

	return 0;
}