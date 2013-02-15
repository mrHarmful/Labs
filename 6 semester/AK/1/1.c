#include <stdio.h>

int main() {
    unsigned int a, b, c;

    scanf("%i %i", &a, &b);

    unsigned int bit, idx, carry;

    carry = 0;
    bit = 1;
    idx = 0;
    c = 0;

    while (idx < 31) {
        bit = 1 << idx;
        if ((a & bit) ^ (b & bit) ^ (carry << idx))
            c |= bit;
            
        carry = (a & b & bit) | ((carry << idx) & bit & (a | b));
        
        idx++;
    }

    int res;
    res = c;
    //if (c & (1 << 30))
        //res = c & !(1 << 30);

    printf("%u\n", c);

    return 0;
}