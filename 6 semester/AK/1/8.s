%define DATA_SELECTOR (1 << 3)
%define CODE_SELECTOR (2 << 3)
%define RM_DATA_SELECTOR (3 << 3)
%define RM_CODE_SELECTOR (4 << 3)

%define OFFSET_RM   (80 * 2 * 2)
%define OFFSET_PM   (80 * 2 * 3)
%define OFFSET_RM2  (80 * 2 * 4)


bits 16
org 0x7c00


start:
    cli

    ; Clear screen
    mov     cx, 80 * 25 * 2
    mov     ax, 0xb800
    mov     es, ax
    mov     di, 0
    _loop_clear_screen:
        mov     ax, 0x00
        stosb
        mov     ax, 0x0f
        stosb
        loop    _loop_clear_screen

    ; Entering PM
    lgdt    [cs:GDTR]
    
    mov     eax, cr0
    bts     eax, 0
    mov     cr0, eax
    
    jmp     CODE_SELECTOR:pm_start


bits 32

pm_start:
    ; PROTECTED MODE --------------------
    mov     ax, DATA_SELECTOR
    mov     ds, ax
    mov     es, ax
    mov     ss, ax


    ; Leaving PM
    jmp     RM_CODE_SELECTOR:pm_exiting

pm_exiting:
    mov     ax, RM_DATA_SELECTOR
    mov     ds, ax
    
    mov     eax, cr0
    btc     eax, 0
    mov     cr0, eax

    jmp     0:pm_exit

bits 16

pm_exit:
    ; REAL MODE --------------------
    mov     ax, 0
    mov     ds, ax
    sti
    jmp $


message:
    db "Hello from tasks", 0


TSS0: times 64 db 0
TSS1: times 64 db 0
TSS2: times 64 db 0

GDTR:
    dw 5 * 8 - 1
    dq GDT


%macro TSSDESC 1
    db 0xff, 0xff
    dw (%1 & 0xffff)
    db ((%1 << 16) & 0xff), 0x92, 0x9f, ((%1 << 24) & 0xff) 
%endmacro


GDT:
    dq 0                            
    db 0xff, 0xff, 0, 0, 0, 0x92, 0x8f, 0 ; PM DATA
    db 0xff, 0xff, 0, 0, 0, 0x9a, 0xcf, 0 ; PM CODE
    db 0xff, 0xff, 0, 0, 0, 0x92, 0x0f, 0 ; RM DATA
    db 0xff, 0xff, 0, 0, 0, 0x9a, 0x0f, 0 ; RM CODE

    ; TSS 0
    TSSDESC(TSS0-$$)

times 510-($-$$) db 0
    db 0x55
    db 0xAA
