#include <iostream>

using namespace std;

template <typename T>
class Matrix {
	public:
		Matrix(int w, int h) {
			data = new T*[w];
			for (int i = 0; i < w; i++)
				data[i] = new T[h];
			width = w;
			height = h;
		}

		~Matrix() {
			for (int i = 0; i < width; i++)
				delete data[i];
			delete data;
		}

		T* operator [] (int x) { 
			return data[x]; 
		}

		Matrix<T>* operator + (Matrix<T>* b) {
			if (width != b->width || height != b->height)
				return NULL;

			Matrix<T>* m = new Matrix<T>(width, height);

			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
					(*m)[x][y] = data[x][y] + (*b)[x][y];

			return m;
		}

		Matrix<T>* operator - () {
			Matrix<T>* m = new Matrix<T>(width, height);

			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
					(*m)[x][y] = -data[x][y];

			return m;
		}

		Matrix<T>* operator - (Matrix<T>* b) {
			Matrix<T>* neg = -(*b);
			Matrix<T>* res = (*this) + neg;
			delete neg;
			return res;
		}

		void print() {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++)
					cout << data[x][y] << "\t";
				cout << endl;
			}
		}

		int width, height;
		
	private:
		T** data;
};

