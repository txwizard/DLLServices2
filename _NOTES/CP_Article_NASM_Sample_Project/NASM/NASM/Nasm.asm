extern	_printf		; the C function, to be called
extern _getchar
extern _extfun

SECTION .data		; Data section, initialized variables

	a:	dd	5		; int a=5;
	fmt:    db "a=%d, eax=%d", 10, 0 ; The printf format, "\n",'0'
	msg:  db "We are now executing in data area :) With DEP disabled",10,0 
	Codev:
	push    ebp		; set up stack frame
    mov     ebp,esp
    push    dword msg	; address of ctrl string
    call    _printf		; Call C function
    add     esp, 4		; pop stack 3 push times 4 bytes
	call _extfun
	mov     esp, ebp	; takedown stack frame
    pop     ebp		; same as "leave" op
	mov	eax,0		;  normal, no error, return value
	ret

	
 SECTION .text                   ; Code section.

        global _main		; the standard gcc entry point
_main:				; the program label for the entry point
    push    ebp		; set up stack frame
    mov     ebp,esp
	mov	eax, [a]	; put a from memory and store into register
	add	eax, 2		; a+2
	push	eax		; value of a+2
	push    dword [a]	; value of variable a
	push    dword fmt	; address of ctrl string
	call    _printf		; Call C function
	add     esp, 12		; pop stack 3 push times 4 bytes
	mov     esp, ebp	; takedown stack frame
	pop     ebp		; same as "leave" op	
	mov	eax,0		;  normal, no error, return value
	call Codev		;Time to break into data section
	call _getchar	;let us C ;)
	
	
	ret			; return 
