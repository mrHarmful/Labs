#ifndef TEXT_H
#define TEXT_H

#include <iostream>
#include <element.h>

using namespace std;

class Text : public Element {
	public:
		Text(char*);
		virtual void print();
	protected:
		char* text;
};

#endif