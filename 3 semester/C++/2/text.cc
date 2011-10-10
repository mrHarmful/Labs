#include <text.h>
#include <iostream>

using namespace std;


Text::Text(char* t) {
	text = t;	
}

void Text::print() {
	cout << text << endl;
}