#ifndef HEADLINE_H
#define HEADLINE_H

#include <text.h>

class Headline : public Text {
	public:
		Headline(char*);
		virtual void print();
};

#endif