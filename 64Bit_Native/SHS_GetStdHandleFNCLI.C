//	============================================================================
//	File Name:			SHS_GetStdHandleFNCLI.C
//
//	Abstract:			This routine is a wrapper for SHS_GetStdHandleFNW, which
//						is decorated with __declspec ( DllExport ), but is
//						devoid of a calling convention designation and, hence,
//						adheres to the old __cdecl calling convention. Though I
//						could live with that, since the default calling
//						convention of a Platform Invoke thunk is __stdcall, I
//						wrote this wrapper, so that I wouldn't have to disturb
//						the existing calls to SHS_GetStdHandleFNW.
//
//	Remarks:            Since it was created specifically to support a managed
//						caller, SHS_GetStdHandleFNCLI saves LastError into a
//						module variable, and provides SHS_GetStdHandleFNError,
//						in lieu of the managed wrapper, for retrieving it.
//
//						The other reason for leaving the existing function
//						prototypes is that it made a nice test case to prove and
//						demonstrate my better DLL header.
//	============================================================================

//	============================================================================
//	2015/10/15:	The objective of the following comment is to strip the
//              decorations from the function name when the linkage editor
//              generates the table of exports. Initially, this edit was
//              applied to GetOsVersionInfo, while SHS_GetStdHandleFNCLI seemed
//              to link without it. But, alas, no more!
//
//				IMPORTANT:	The spacing of this comment is very particular. The
//							white space that I prefer to put around parentheses
//							and commas breaks it. As straightforward as this one
//							is, I can live with the limitation.
//	============================================================================

#pragma comment(linker, "/EXPORT:SHS_GetStdHandleFNCLI=_SHS_GetStdHandleFNCLI@12")
#pragma comment(linker, "/EXPORT:SHS_GetStdHandleFNError=_SHS_GetStdHandleFNError@0")

#define UNICODE

#if !defined ( __WWCONAIDLIB_PVT_P6C__ )
    #define __WWCONAIDLIB_PVT_P6C__
#endif /* #if !defined ( __WWCONAIDLIB_PVT_P6C__) */

#define __WWCONAID_BUILDING_STDIN_WRAPPERS__
#define __WWCONAID_BUILDING_GETSTDHANDLEFN_4CLR__


#include <StandardHandleState.H>
#include ".\WWConAid_Pvt.H"

#include ".\resource.h"

#define WWCA_TS_FORCE_COTASKMALLOC	( FB_STRING_BUFFER_INDEX_5_OF_5 + ARRAY_NEXT_ELEMENT_P6C )

static DWORD m_dwLastError = ERROR_SUCCESS ;

INT32 SHS_STANDARDHANDLESTATE_API	__stdcall	SHS_GetStdHandleFNCLI
(
	CSHS_STANDARD_HANDLE penmStdHandleID ,	// Specify the desired standard stream.
	LPBYTE				 plpOutputBuffer ,	// Pointer to buffer of at least pintOutBufSize bytes where SHS_GetStdHandleFNCLI can deliver its output
	INT32				 pintOutBufSize		// Size, in BYTES, of the buffer to which plpOutputBuffer points
)
{
    LPWSTR lpStdHandleFNC = NULL;

	if ( WWCA_AcquireProcessMutex ( ) )
	{	// Begin synchronized use of module level resources.
		if ( plpOutputBuffer )
		{	// Buffer pointer passes the sniff test.
			if ( pintOutBufSize )
			{	// Buffer length passes, too.
				m_lpOutputBuffer	= plpOutputBuffer ;
				m_intOutBufSize		= pintOutBufSize  ;

				if ( lpStdHandleFNC = SHS_GetStdHandleFNW ( penmStdHandleID , WWCA_TS_FORCE_COTASKMALLOC ) )
				{
					if ( m_intFNLen <= TCharsMaxForBufSizeP6C ( pintOutBufSize ) )
					{	// Synchronize the local status code with the system status code.
						m_dwLastError = GetLastError ( ) ;
					}	// TRUE (anticipated outcome) block, if ( m_intFNLen <= TCharsMaxForBufSizeP6C ( pintOutBufSize ) )
					else
					{	// Set the internal and system status codes to the same value.
						SHS_SetLastError ( SHS_ERROR_NAME_TOO_LONG_FOR_FB ) ;
					}	// FALSE (unanticipated outcome) block, if ( m_intFNLen <= TCharsMaxForBufSizeP6C ( pintOutBufSize ) )
				}	// TRUE (anticipated outcome) block, if ( lpStdHandleFNC = SHS_GetStdHandleFNW ( penmStdHandleID , WWCA_TS_FORCE_COTASKMALLOC ) )
				else
				{	// Save LastError.
					m_dwLastError	= GetLastError ( ) ;
				}	// FALSE (unanticipated outcome) block, if ( lpStdHandleFNC = SHS_GetStdHandleFNW ( penmStdHandleID , WWCA_TS_FORCE_COTASKMALLOC ) )

				if ( WWCA_ReleaseAndDestroyProcessMutex ( ) )
				{	// A problem that prevents relinquishing and destroying the critical section object trumps whatever else was being reported.
					m_dwLastError	= GetLastError ( ) ;
				}	// if ( WWCA_ReleaseAndDestroyProcessMutex ( ) )
			}	// TRUE (anticipated outcome) block, if ( pintOutBufSize )
			else
			{
				SHS_SetLastError ( SHS_ERROR_INVALID_OUTBUF_SIZE ) ;
			}	// FALSE (unanticipated outcome) block, if ( pintOutBufSize )
		}	// TRUE (anticipated outcome) block, if ( plpOutputBuffer )
		else
		{
			SHS_SetLastError ( SHS_ERROR_INVALID_OUTPUT_BUFFER ) ;
		}	// FALSE (unanticipated outcome) block, if ( plpOutputBuffer )

		return m_dwLastError ? STRLEN_EMPTY_P6C : m_intFNLen ;
	}	// TRUE (anticipated outcome) block, if ( WWCA_AcquireProcessMutex ( ) )
	else
	{
		return STRLEN_EMPTY_P6C ;
	}	// FALSE (unanticipated outcome) block, if ( WWCA_AcquireProcessMutex ( ) )
}	// SHS_GetStdHandleFNCLI


DWORD 	SHS_STANDARDHANDLESTATE_API	 __stdcall SHS_GetStdHandleFNError ( void )
{
    return m_dwLastError ;
}	// SHS_GetStdHandleFNError