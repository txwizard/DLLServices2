extern	_printf
SECTION .data		; Data section, initialized variables


	newmsg:  db "We are now from data section into an external function ;) ",10,0 

 global _extfun		; the standard gcc entry point
_extfun:				; the program label for the entry point
    push    ebp		; set up stack frame
    mov     ebp,esp
	push    dword newmsg	; address of ctrl string
	call    _printf		; Call C function
	add     esp, 4		; pop stack 3 push times 4 bytes
	mov     esp, ebp	; takedown stack frame
	pop     ebp		; same as "leave" op	
	xor	eax,eax		;  normal, no error, return value
	ret		