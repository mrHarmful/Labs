#include <iostream>
#include <document.h>

Document::Document() {
	elc = 0;
	els = new Element*[256];
}

void Document::print() {
	for (int i = 0; i < elc; i++)
		els[i]->print();
}

void Document::push(Element* e) {
	els[elc++] = e;
}

Document::~Document() {
	for (int i = 0; i < elc; i++)
		delete els[i];	
}