/*
	============================================================================

	File Name:			ProgramIDFromArgV.C

	Header:				At present, this function has no corresponding header to
						call "home." At least for now, it is anticipated that it
						will be declared in the main application header (e. g.,
						stdafx.h).

	File Synopsis:		This file defines function ProgramIDFromArgV.

	Function Synopsis:	ProgramIDFromArgV provides a portable mechanism to 
						identify the name of a program, for display on its
						console. Hence, it depends exclusively on CRT routines.

	Author:				David A. Gray

	----------------------------------------------------------------------------
	Revision History
	----------------------------------------------------------------------------

	Date       By  Synopsis
	---------- --- -------------------------------------------------------------
	2015/01/04 DAG Function created and tested to meet an immediate need.

	2015/05/06 DAG Replace all char entities with TCHAR entities, so that this
	               routine works in either ANSI or Unicode.

	2015/05/21 DAG 1) Revert from C++ to C.

	               2) Make this source file freestanding with respect to any
				      project that includes it.

				   3) Truncate the extension, too.

	2015/05/25 DAG Switch from the default calling convention (currently _cdecl)
	               to the ever so slightly more efficient (more so in a debug
				   build than a release build) __pascal.

	2015/05/27 DAG Correct errors in the degenerate cases that dropped the first
	               character of the program when the name is unqualified, and
				   causes it to be completely truncated when the extension is
				   omitted.
	============================================================================
*/

#include <ctype.h>
#include <stdlib.h>

#include "targetver.h"

//#include <stdio.h>
#include <tchar.h>

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

//	----------------------------------------------------------------------------
//	Since neither of these messages is likely ever to be needed in production, I
//	deviated from my usual practice, and hard coded them.
//	----------------------------------------------------------------------------

TCHAR chrArg0IsNull  [ ]	= TEXT ( "ERROR: The first string in the argument list passed into routine ProgramIDFromArgV is a null reference.\n" ) ;
TCHAR chrArg0IsBlank [ ]	= TEXT ( "ERROR: The first string in the argument list passed into routine ProgramIDFromArgV is the empty string.\n" ) ;

TCHAR chrPathDlm     [ ]	= TEXT ( "\\" ) ;
TCHAR chrExtnDlm     [ ]	= TEXT ( "." ) ;

TCHAR * lpchrArg0IsNull		= ( TCHAR * ) &chrArg0IsNull ;
TCHAR * lpchrArg0IsBlank	= ( TCHAR * ) &chrArg0IsBlank ;
TCHAR * lpchrPathDlm		= ( TCHAR * ) &chrPathDlm ;

TCHAR * __stdcall ProgramIDFromArgV ( const TCHAR * ppgmptr )
{
	TCHAR * lpLastCharacterInString				= NULL ;
	TCHAR *	lpLastPathDelimiter					= NULL ;
	TCHAR *	lpLastDelimiterScan					= NULL ;
	TCHAR * lpLastExtnDelimiter					= NULL ;
	TCHAR * rlpBaseNameByItself					= NULL ;

	BOOL	fBaseNameIsolated					= FALSE ;
	BOOL	fIsFirstDoLoop						= TRUE ;

	int		intPgmIdLengthbBytes				= -1 ;
	int		intPgmPtrLength						= -1 ;

	if ( ppgmptr )
	{
		intPgmPtrLength							= _tcslen ( ppgmptr ) ;

		if ( intPgmPtrLength )
		{
			lpLastDelimiterScan					= ( TCHAR * ) ppgmptr ;
			lpLastPathDelimiter					= ( TCHAR * ) ppgmptr ;

			do
			{
				lpLastDelimiterScan				= _tcsstr ( lpLastDelimiterScan ,
															lpchrPathDlm ) ;

				if ( lpLastDelimiterScan )
				{
					if ( fIsFirstDoLoop )
					{
						fIsFirstDoLoop			= FALSE ;
					}	// TRUE (degenerate case, where the program name is unqualified) block, if ( fIsFirstDoLoop )

					lpLastPathDelimiter			= lpLastDelimiterScan ;
					lpLastDelimiterScan++ ;
				}	// TRUE block, if ( lpLastDelimiterScan )
				else
				{
					if ( fIsFirstDoLoop )
					{
						fIsFirstDoLoop			= FALSE ;
					}	// TRUE (degenerate case, where the program name is unqualified) block, if ( fIsFirstDoLoop )
					else
					{
						lpLastPathDelimiter++ ;
					}	// FALSE (standard case, where the programm name is at least partially qualified) block, if ( fIsFirstDoLoop )
				}	// FALSE block, if ( lpLastDelimiterScan )
			} while ( lpLastDelimiterScan ) ;

			//	----------------------------------------------------------------
			//	Sanity check the string pointer. Find the extension delimiter,
			//	unless it is NULL.
			//	----------------------------------------------------------------

			if ( lpLastPathDelimiter )
			{
				lpLastCharacterInString			= ( DWORD_PTR ) ppgmptr + ( ( intPgmPtrLength - 1 ) * sizeof ( TCHAR ) ) ;
				lpLastExtnDelimiter				= lpLastCharacterInString;

				do
				{
					if ( ( TCHAR ) lpLastExtnDelimiter [ 0 ] == chrExtnDlm [ 0 ] )
					{	// Found extension delimiter.
						fBaseNameIsolated		= TRUE;
					}	// TRUE (Extension delimiter found.) block, if ( ( TCHAR ) lpLastExtnDelimiter [ 0 ] == chrExtnDlm )
					else
					{
						lpLastExtnDelimiter		= _tcsdec ( ppgmptr ,
							                                lpLastExtnDelimiter ) ;

						if ( ( DWORD_PTR ) lpLastExtnDelimiter == ( DWORD_PTR ) lpLastPathDelimiter )
						{	// Reached the beginning of the base name.
							fBaseNameIsolated	= TRUE;
						}	// if ( ( DWORD_PTR ) lpLastExtnDelimiter == ( DWORD_PTR ) lpLastPathDelimiter )
					}	// TRUE (Extension delimiter not yet found.) block, if ( ( TCHAR ) lpLastExtnDelimiter [ 0 ] == chrExtnDlm )
				}
				while ( !fBaseNameIsolated ) ;

				intPgmIdLengthbBytes			=    ( ( DWORD_PTR ) lpLastExtnDelimiter     == ( DWORD_PTR ) lpLastPathDelimiter )
					                               ? ( ( DWORD_PTR ) lpLastCharacterInString -  ( DWORD_PTR ) lpLastPathDelimiter )
												   : ( ( DWORD_PTR ) lpLastExtnDelimiter     -  ( DWORD_PTR ) lpLastPathDelimiter ) ;

				rlpBaseNameByItself				= HeapAlloc ( GetProcessHeap ( ) ,
					                                          HEAP_ZERO_MEMORY ,
															  intPgmIdLengthbBytes + sizeof ( TCHAR ) ) ;

				memcpy ( rlpBaseNameByItself ,
					     lpLastPathDelimiter ,
						 intPgmIdLengthbBytes ) ;

				return rlpBaseNameByItself ;
			}	// TRUE (expected outcome) block, if ( lpLastPathDelimiter )
			else
			{
				return lpchrArg0IsBlank ;
			}	// FALSE (UNexpected outcome) block, if ( lpLastPathDelimiter )
		}	// TRUE (expected outcome) block, if ( strlen ( ppgmptr ) )
		else
		{
			return lpchrArg0IsBlank ;
		}	// FALSE (UNexpected outcome) block, if ( strlen ( ppgmptr ) )
	}	// TRUE (expected outcome) block, if ( ppgmptr )
	else
	{
		return lpchrArg0IsNull ;
	}	// FALSE (UNexpected outcome) if ( ppgmptr )
}	// LPTSTR ProgramIDFromArgV