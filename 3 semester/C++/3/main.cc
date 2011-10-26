#include <iostream>
#include "list.h"

using namespace std;


class Item {
	public:
		Item(int v) {
			value = v;
		}
		int value;
};

void dump(LinkedList<Item*>* lst) {
	LinkedListIter<Item*>* iter = lst->iter();
	for (; !iter->end(); iter->next()) {
		cout << iter->get()->value << " ";
	}
	cout << endl;
}

int main() {
	LinkedList<Item*>* l = new LinkedList<Item*>();
	cout << "5 -> 0\n";
	l->insert(new Item(5), 0);
	cout << "7 -> 1\n";
	l->insert(new Item(7), 1);
	cout << "9 -> 0\n";
	l->insert(new Item(9), 0);
	dump(l);

	cout << "del #1\n";
	l->remove(1);
	dump(l);

	cout << "del #0\n";
	l->remove(0);
	dump(l);

	l->purge();
	delete l;
	return 0;
}