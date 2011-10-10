#ifndef DOCUMENT_H
#define DOCUMENT_H

#include <iostream>
#include <element.h>


using namespace std;

class Document {
	public:
		Document();
		void 	  print();
		void      push(Element*);
		~Document();
	private:
		Element** els;
		int		  elc; 	
};

#endif