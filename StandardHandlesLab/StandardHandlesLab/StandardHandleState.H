#if !defined ( STANDARDHANDLESTATE_INCLUDED )
#define STANDARDHANDLESTATE_INCLUDED

#if defined ( _MSC_VER ) && ( _MSC_VER >= 1020 )
	#pragma once
#endif  /*  #if defined (_MSC_VER) && (_MSC_VER >= 1020) */

/*
	============================================================================

	Name:               StandardHandleState.H
	Library Header      WWConAid.H
	Library:            WWConAid.dll
	Link Library:       WWConAid.lib

	Synopsis:           Declare the functions for evaluating the state of one or
						more of the three standard handles attached to character
						mode programs when they start.

	Dependencies:       None, other than a couple of standard system libraries

	Remarks:            Since the local storage used by these functions is
	
	License:			Copyright (C) 2015, David A. Gray. 	All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:

	*   Redistributions of source code must retain the above copyright notice,
		this list of conditions and the following disclaimer.

	*   Redistributions in binary form must reproduce the above copyright
		notice, this list of conditions and the following disclaimer in the
		documentation and/or other materials provided with the distribution.

	*   Neither the name of David A. Gray nor the names of his contributors may
	    be used to endorse or promote products derived from this software without
		specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
	AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
	IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
	ARE DISCLAIMED. IN NO EVENT SHALL David A. Gray BE LIABLE FOR ANY
	DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
	THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

	Date Created:       Wednesday, 08 July 2015

	----------------------------------------------------------------------------
	Revision History
	----------------------------------------------------------------------------

	Date       Version By  Synopsis
	---------- ------- --- -----------------------------------------------------
	2015/09/21 1.0.0.1 DAG First appearance of this header and its module.
	============================================================================
*/

#include <Windows.h>

typedef enum _SHS_STANDARD_HANDLE
{
	SHS_UNDEFINED ,						// Value = 0, which is reserved for indicating that the value is uninitialized.
	SHS_INPUT ,							// Value = 1, which corresponds to STD_INPUT_HANDLE
	SHS_OUTPUT ,						// Value = 2, which corresponds to STD_OUTPUT_HANDLE
	SHS_ERROR 							// Value = 3, which corresponds to STD_ERROR_HANDLE
} SHS_STANDARD_HANDLE ;

typedef enum _SHS_HANDLE_STATE
{
	SHS_UNDETERMINABLE ,				// Value = 0, indicating that SHS_STANDARD_HANDLE is either SHS_UNDEFINED or out of range
	SHS_ATTACHED ,						// Value = 1, indicating that the handle corresponding to the value of SHS_STANDARD_HANDLE is attached to its console
	SHS_REDIRECTED ,					// Value = 2, indicating that the handle corresponding to the value of SHS_STANDARD_HANDLE is redirected
	SHS_SYSTEM_ERROR					// Value = 3, indicating that an internal error occurred. Call GetLastError to learn why.
} SHS_HANDLE_STATE ;

typedef const SHS_STANDARD_HANDLE		CSHS_STANDARD_HANDLE ;

#define SHS_ERROR_ID_IS_UNINITIALIZED	( APPLICATION_ERROR_MASK        | 0x00000001 )
#define SHS_ERROR_ID_IS_OUT_OF_RANGE	( SHS_ERROR_ID_IS_UNINITIALIZED + 0x00000001 )

#if defined ( STANDARD_HANDLES_LAB )
		#define SHS_STANDARDHANDLESTATE_API __stdcall
#else	/* #if defined ( STANDARD_HANDLES_LAB ) */
	#if defined ( __DEFINING_STANDARDHANDLESTATE__ ) || defined ( __WWCONAIDLIB_PVT_P6C__ )
		#define SHS_STANDARDHANDLESTATE_API __declspec(dllexport)
	#else   /* #if defined ( __DEFINING_STANDARDHANDLESTATE__ ) || defined ( __WWCONAIDLIB_PVT_P6C__ ) */
		#define SHS_STANDARDHANDLESTATE_API __declspec(dllimport)
	#endif  /* #if defined ( __DEFINING_STANDARDHANDLESTATE__ ) || defined ( __WWCONAIDLIB_PVT_P6C__ ) */
#endif	/* #if defined ( STANDARD_HANDLES_LAB ) */

#if defined ( __cplusplus )
extern "C" {
#endif /* #if defined ( __cplusplus ) */
	/*
		========================================================================

		Function Name:  SHS_StandardHandleState

		Definition:		StandardHandleState.C

		Synopsis:       Use the SHS_HANDLE_STATE enumeration to report the state
						of the standard handle specified by penmStdHandleID.

		Arguments:      penmStdHandleID	= Use a SHS_STANDARD_HANDLE enumeration
										  member to identify a handle to query.

		Returns:        If the function succeeds, the returned SHS_HANDLE_STATE
						indicates that the specified handle is either attached
						(SHS_ATTACHED) or redirected (SHS_REDIRECTED).

						The remaining two return values indicate errors.

						SHS_UNDETERMINABLE	= The value of penmStdHandleID is
											  either SHS_UNDEFINED (zero,
											  meaning that it is uninitialized)
											  or out of range. Call GetLastError
											  to determine which.

						SHS_SYSTEM_ERROR	= An internal error occurred, which
											  should be reported by GetLastError
											  as a system status code.

		  Remarks:      To determine whether a handle is attached to its 
						console or redirected, this routine calls
						GetConsoleMode on the handle returned by GetStdHandle. If
						that call succeeds, the handle is attached to a console.
						Otherwise, GetLastError should return
						ERROR_INVALID_HANDLE, confirming that the handle is
						redirected, since a handle loses its console mode when
						that happens. If GetLastError returns 
						ERROR_INVALID_HANDLE, the status code is cleared by
						calling SetLastError, and the handle is reported as
						redirected. Otherwise, this function returns
						SHS_SYSTEM_ERROR to signal the caller that it should
						call GetLastError, and recover from the system error.

						The argument is marked as const, meaning that the C
						compiler insists that this routine must treat it as
						read only. The implementation is straight C, as is
						everything else in the library that will become its
						permanent home.

		=======================================================================
	*/

	SHS_HANDLE_STATE SHS_STANDARDHANDLESTATE_API SHS_StandardHandleState
		(
			CSHS_STANDARD_HANDLE penmStdHandleID
		) ;
#if defined ( __cplusplus )
}
#endif  /* #if defined ( __cplusplus ) */
#endif	/* #if !defined ( STANDARDHANDLESTATE_INCLUDED ) */