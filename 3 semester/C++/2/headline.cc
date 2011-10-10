#include <headline.h>
#include <iostream>

using namespace std;

Headline::Headline(char* c) : Text(c) {
}

void Headline::print() {
	cout << "=== " << text << " ===\n";
}