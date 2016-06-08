/*
	============================================================================

	File Name:			SHS_StandardHandleState.C

	Function Name:		SHS_StandardHandleState

	Declaring Header:	StandardHandleState.H

	Synopsis:			Use the SHS_HANDLE_STATE enumeration to report the state
						of the standard handle specified by penmStdHandleID.

	Arguments:			penmStdHandleID	= Use a SHS_STANDARD_HANDLE enumeration
										  member to identify a handle to query.

	Returns:			If the function succeeds, the returned SHS_HANDLE_STATE
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

	Remarks:			To determine whether a handle is attached to its console
						or redirected, this routine calls GetConsoleMode on the
						handle returned by GetStdHandle. If that call succeeds,
						the handle is attached to a console. Otherwise, 
						GetLastError should return ERROR_INVALID_HANDLE,
						confirming that the handle is redirected, since a handle
						loses its console mode when that happens. 
						
						If GetLastError returns ERROR_INVALID_HANDLE, the status
						code is cleared by calling SetLastError, and the handle is
						reported as redirected. Otherwise, this function returns
						SHS_SYSTEM_ERROR to signal the caller that it should
						call GetLastError, and recover from the system error.

						The argument is marked as const, meaning that the compiler
						insists that this routine must treat it as read only.

	License:			Copyright (C) 2015, David A. Gray. 	All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:

	*   Redistributions of source code must retain the above copyright notice,
		this list of conditions and the following disclaimer.

	*   Redistributions in binary form must reproduce the above copyright
		notice, this list of conditions and the following disclaimer in the
		documentation and/or other materials provided with the distribution.

	*   Neither the name of David A. Gray nor the names of his contributors may
	    be used to endorse or promote products derived from this software
		without specific prior written permission.

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

	============================================================================
*/

#include <stdio.h>
#include <tchar.h>

#include "StandardHandleState.h"

static HANDLE dwStdConsoleHandleIDs [ ] =
{
	INVALID_HANDLE_VALUE ,										// This value is intentionally invalid, and corresponds to SHS_UNDEFINED.
	STD_INPUT_HANDLE ,											// SHL_STDIN
	STD_OUTPUT_HANDLE ,											// SHL_STDOUT
	STD_ERROR_HANDLE ,											// SHL_STERR
	INVALID_HANDLE_VALUE 										// This value is intentionally invalid.
};	// static DWORD dwStdConsoleHandleIDs [ ]

SHS_HANDLE_STATE SHS_STANDARDHANDLESTATE_API SHS_StandardHandleState
(
	CSHS_STANDARD_HANDLE penmStdHandleID
)
{
	HANDLE hThis ;												// The first use of this variable initializes it.
	DWORD dwModde ;												// The first use of this variable initializes it, AND it's a throwaway.

	switch ( penmStdHandleID )
	{
		case SHS_UNDEFINED :									// Argument penmStdHandleID is uninitialized.
			return SHS_UNDETERMINABLE;

		case SHS_INPUT  :
		case SHS_OUTPUT :
		case SHS_ERROR  :
			if ( ( hThis = GetStdHandle ( dwStdConsoleHandleIDs [ ( int ) penmStdHandleID ] ) ) != INVALID_HANDLE_VALUE )
			{
				if ( GetConsoleMode ( hThis , &dwModde ) )
				{
					return SHS_ATTACHED;
				}	// TRUE (Handle is attached to its console.) block, if ( GetConsoleMode ( hThis , &dwModde ) )
				else
				{
					if ( GetLastError ( ) == ERROR_INVALID_HANDLE )
					{
						SetLastError ( ERROR_SUCCESS );
						return SHS_REDIRECTED;
					}	// TRUE (anticipated outcome) block, if ( GetLastError ( ) == ERROR_INVALID_HANDLE )
					else
					{
						return SHS_SYSTEM_ERROR;
					}	// FALSE (UNanticipated outcome) block, if ( GetLastError ( ) == ERROR_INVALID_HANDLE )
				}	// FALSE (Handle is redirected from its console into a file or pipe.) block, if ( GetConsoleMode ( hThis , &dwModde ) )
			}	// TRUE (anticipated outcome) block, if ( ( hThis = GetStdHandle ( dwStdConsoleHandleIDs [ ( int ) penmStdHandleID ] ) ) != INVALID_HANDLE_VALUE )
			else
			{
				return SHS_SYSTEM_ERROR;
			}	// FALSE (UNanticipated outcome) block, if ( ( hThis = GetStdHandle ( dwStdConsoleHandleIDs [ ( int ) penmStdHandleID ] ) ) != INVALID_HANDLE_VALUE )

		default:												// Argument penmStdHandleID is out of range.
			return SHS_SYSTEM_ERROR;
	}	// switch ( penmStdHandleID )
}	// SHS_HANDLE_STATE SHS_STANDARDHANDLESTATE_API SHS_StandardHandleState