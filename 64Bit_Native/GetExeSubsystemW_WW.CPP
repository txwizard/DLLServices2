/*
    ============================================================================

    File Name:          GetExeSubsystemA_WW.CPP

    Function Name:      GetExeSubsystemA_WW (P6CUtilLib1.dll)
						PROC_GetExeSubsystemW (ProcessInfo.dll)

    Synopsis:           Determine the subsystem in which a Portable Executable
                        file runs.

    Arguments:          plpMMainModuleFQFN  = A pointer to a null terminated
                                              string that contains the name of a
                                              Portable Executable file to test.

    Returns:            If the function succeeds, its return value is a DWORD,
                        cast up from the WORD value retrieved from the first EXE
                        file to be loaded into the process.

                        If it fails, it returns IMAGE_SUBSYSTEM_UNKNOWN. To get
                        extended error information, call GetLastError. The
                        returned status code may be a system status code, coming
                        from a Windows system routines, or a custom status code,
                        defined by this library, to cover cases for which there
                        is no system status code.

    Remarks:            IMAGE_SUBSYSTEM_UNKNOWN, the value returned to indicate
                        an error, is defined in Microsoft Windows Platform SDK
                        header WinNT.H, and has a value of zero. Though failure
                        is pretty unlikely, I have covered every case that I can
                        imagine.

                        A handful of custom status codes cover cases for which
                        the Windows system libraries don't provide status codes.
                        All such codes are represented by constants with names
                        that begin with "GETSUBSYST_" and have values in which
                        the APPLICATION_ERROR_MASK bit is set.

                        In the event of an error, GetProcessSubsystem_Error_WW
                        returns a translation of the error code, along with any
                        information it has bout the circumstances surrounding
                        the error.

    References:         None.

    Date Completed:     Tuesday, 08 April 2014

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Author Synopsis
    ---------- ------ ----------------------------------------------------------
    2014/04/08 DAG/WW First appearance of this function.
	2016/05/08 DAG    Function moved to ProcessInfo.dll, and renamed to conform
					  to its function naming convention.
    ============================================================================
*/

#define UNICODE
#define _BUILDING_P6CUTILLIB1LIB1

#include ".\P6CUtilLib1_Pvt.H"

#if defined ( PROCESSINFO_EXPORTS )
	#include <ProcessInfo.H>
#else
	#include <P6CUtilLib1.H>
#endif	/* #ifdef PROCESSINFO_EXPORTS */


//  ----------------------------------------------------------------------------
//  Link to external variables defined in the module that contains DLLMain. 
//  Since DLLMain has "C" linkage, so must these.
//  ----------------------------------------------------------------------------

#if defined ( __cplusplus )
extern "C"
{
#endif

	extern HANDLE m_hDfltHeap ;
	extern LPTSTR m_lpErrMsgBuf ;

#if defined ( __cplusplus )
}
#endif  /* __cplusplus */

#if defined ( PROCESSINFO_EXPORTS )
	DWORD PROCESSINFO_API             PROC_GetExeSubsystemW
	#pragma comment(linker, "/EXPORT:PROC_GetExeSubsystemW@4=_PROC_GetExeSubsystemW@4")
#else
	DWORD LIBSPEC_P6CUTILLIB1LIB1_API GetExeSubsystemW_WW
#endif	/* #if defined ( PROCESSINFO_EXPORTS ) */
(
    LPCTSTR plpMMainModuleFQFN
)
{
    #include ".\GetExeSubsystem_WW.INL"
}   // GetExeSubsystemA_WW (P6CUtilLib1.dll) and PROC_GetExeSubsystemW (ProcessInfo.dll)