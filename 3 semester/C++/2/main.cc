#include <iostream>
#include <element.h>
#include <headline.h>
#include <document.h>
#include <text.h>


int main() {
	Document* d = new Document();
	d->push(new Headline("Header"));
	d->push(new Text("Test"));
	d->push(new Text("Test"));
	d->print();
	return 0;
}