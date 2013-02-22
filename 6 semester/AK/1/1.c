#include <stdio.h>

#define SIZE 16
#define REGISTER(x) char x[SIZE]
#define DOPAUSE pause();

#define TRACESUM
#define TRACEMUL
//#define PAUSE

typedef char* reg_p;

void pause() {
    #ifdef PAUSE
    char buf[1024];
    printf("Continue?\n");
    gets(buf);
    #endif
}

void store(reg_p r, int d) {
    r[SIZE-1] = (d < 0) ? 1 : 0;
    int neg = d < 0;
    if (neg) {
        d = -d - 1;
    }

    for (int i = 0; i < SIZE-1; i++)
        r[i] = 0;

    int idx = 0;
    while (d > 0) {
        r[idx++] = d % 2;
        d /= 2;
    }

    if (neg)
        for (int i = 0; i < SIZE-1; i++)
            r[i] = 1 - r[i];
}

int extract(reg_p r) {
    int v = 0;
    int neg = r[SIZE-1] == 1;

    for (int i = SIZE-2; i >= 0; i--)
        v = v * 2 + (neg ? 1 - r[i] : r[i]);

    if (neg)
        v = -v - 1;
    return v;
}

void dump(reg_p r) {
    for (int i = SIZE-1; i >= 0; i--) {
        printf("%i", r[i]);
    }
    printf(" == %i\n", extract(r));
}

void add(reg_p RA, reg_p RB, reg_p RC) {
    int idx = 0, carry = 0;
    store(RC, 0);
    for (int i = 0; i < SIZE; i++) {
        RC[i] = RA[i] + RB[i] + carry;
        carry = 0;
        if (RC[i] > 1) {
            carry = 1;
            RC[i] %= 2;
        }
        #ifdef TRACE
        printf("Step %i:\n", i);
        dump(RC);
        #endif
    }
}

void sub(reg_p RA, reg_p RB, reg_p RC) {
    REGISTER(RT);
    int v = extract(RB);
    store(RT, -v);
    add(RA, RT, RC);
}

void shl(reg_p R) {
    for (int i = SIZE-1; i >= 0; i--)
        R[i] = R[i-1];
    R[0] = 0;
}

void copy(reg_p RA, reg_p RB) {
    store(RB, extract(RA));
}

void mul(reg_p RA, reg_p RB, reg_p RC) {
    REGISTER(RT);
    REGISTER(RT2);
    store(RT, 0);
    for (int i = SIZE-1; i >= 0; i--) {
        shl(RT);
        if (RA[i] == 1) {
            add(RT, RB, RT2);
            copy(RT2, RT);
        }
        #ifdef TRACEMUL
        printf("Step %i:\n", SIZE - i);
        dump(RT);
        DOPAUSE
        #endif
    }
    copy(RT, RC);

    #ifdef TRACEMUL
        store(RT2, 0);

        for (int i = 0; i < SIZE; i++) printf(" ");
        dump(RA);
        for (int i = 0; i < SIZE; i++) printf(" ");
        dump(RB);
        printf("-----------------------------------\n");

        for (int j = 0; j < SIZE; j++) {
            for (int i = 0; i < SIZE - j; i++) printf(" ");
            if (RA[j] == 1)
                dump(RB);
            else
                dump(RT2);
        }

        printf("-----------------------------------\n");
        for (int i = 0; i < SIZE; i++) printf(" ");
        dump(RC);

    #endif    
}

int main() {
    REGISTER(RA);
    REGISTER(RB);
    REGISTER(RC);

    store(RA, -5);
    store(RB, 35);

    mul(RA, RB, RC);

    printf("\n\n");
    dump(RA);
    dump(RB);
    printf("--------------------\n");
    dump(RC);

    return 0;   
}
