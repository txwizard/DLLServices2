// stdafx.h : include file for standard system include files or project specific
//			  include files that are used frequently, but are changed infrequently

#pragma once

#include "targetver.h"

#include <stdio.h>
#include <tchar.h>

#include <Windows.h>												// WinBase.h pulls FileAPI.h into the compilation stream, and Windows.h pulls wincon.h.
#include <FixedStringBuffers.H>

#include "StandardHandleState.h"

#include <MathMacros_WW.H>
#include <StandardMacros_DAG.H>

#include ".\resource.h"


#define FB_BUFFER_INDEX_1				( FB_FIRST_BUFFER_INDEX + ARRAY_NEXT_ELEMENT_P6C )
#define FB_BUFFER_INDEX_2				( FB_BUFFER_INDEX_1     + ARRAY_NEXT_ELEMENT_P6C )
#define FB_BUFFER_INDEX_3				( FB_BUFFER_INDEX_2     + ARRAY_NEXT_ELEMENT_P6C )
#define FB_BUFFER_INDEX_4				( FB_BUFFER_INDEX_3     + ARRAY_NEXT_ELEMENT_P6C )


#if defined ( __cplusplus )
extern "C"
{
#endif  /* #if defined ( __cplusplus ) */
	TCHAR * __stdcall ProgramIDFromArgV
		(
			const TCHAR * ppgmptr
		) ;

	LONG WINAPI SHL_CrashHandler
		(
			EXCEPTION_POINTERS *	plpExceptionPtrs				// ExceptionInfo
		) ;

	BOOL __stdcall SHL_GetOsVersion
		(
		RTL_OSVERSIONINFOEXW* pk_OsVer
		) ;

	int __stdcall SHL_PerformTests ( );

	LPTSTR __stdcall SHL_GetRedirectionTarget
		(
			HANDLE phStdHandle
		) ;
#if defined ( __cplusplus )
}
#endif  /* #if defined ( __cplusplus ) */