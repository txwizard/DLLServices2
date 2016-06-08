// StandardHandlesLab.cpp : Defines the entry point for the console application.
//

#include ".\StandardHandlesLab.H"

bool g_showCrashDialog = false;

int _tmain ( int argc , _TCHAR* argv [ ] )
{
	int intRC		= ERROR_SUCCESS;
	ForceIntoDEebugger;

	LPTSTR lpPgmID	= ProgramIDFromArgV ( argv [ 0 ] ) ;

	_tprintf ( 
		FB_LoadString (
			FB_LOOK_IN_THIS_EXE ,
			IDS_BOJ ,
			FB_FIRST_BUFFER_INDEX ,
			FB_HIDE_LENGTH ) ,
		lpPgmID ) ;

	UINT uintInitialErrorMode = SetErrorMode ( SEM_NOGPFAULTERRORBOX | SEM_FAILCRITICALERRORS ) ;
	_tprintf ( 
		FB_LoadString (
			FB_LOOK_IN_THIS_EXE ,
			IDS_NEW_CRT_ERRORMODE ,
			FB_FIRST_BUFFER_INDEX ,
			FB_HIDE_LENGTH ) ,										// Format Control String (template), fed directly into _tprintf.
		uintInitialErrorMode ,										// Original:  Hexadecimal
		uintInitialErrorMode ,										//            Decimal
		SEM_NOGPFAULTERRORBOX | SEM_FAILCRITICALERRORS ,			// New Value: Hexadecimal
		SEM_NOGPFAULTERRORBOX | SEM_FAILCRITICALERRORS );			//            Decimal
	SetUnhandledExceptionFilter ( SHL_CrashHandler );				// From now on, SHL_CrashHandler gets all unhandled exceptions except buffer overruns and such.

	if ( intRC = SHL_PerformTests ( ) )
	{
		_tprintf (
			FB_FormatMessage ( 
				FB_LoadString (
					FB_LOOK_IN_THIS_EXE , 
					IDS_ERRMSG_GETSTDHANDLE ,
					FB_FIRST_BUFFER_INDEX ,
					FB_HIDE_LENGTH ) ,
				intRC ,
				SCF2_HEXADECIMAL ) );
	}	// if ( intRC = SHL_PerformTests ( ) )

	_tprintf (
		FB_LoadString (
			FB_LOOK_IN_THIS_EXE ,
			IDS_EOJ ,
			FB_FIRST_BUFFER_INDEX ,
			FB_HIDE_LENGTH ) ,
		lpPgmID ) ;
	return intRC;
}	// int _tmain(int argc, _TCHAR* argv[])


LONG WINAPI SHL_CrashHandler
(
	EXCEPTION_POINTERS *	plpExceptionPtrs						// ExceptionInfo
)
{
	_tprintf (
		FB_LoadString (
			FB_LOOK_IN_THIS_EXE ,
			IDS_ERRMSG_UNHANDLED_EXCEPTION ,
			FB_FIRST_BUFFER_INDEX ,
			FB_HIDE_LENGTH ) ,										// Format Control String (template)
		plpExceptionPtrs->ExceptionRecord->ExceptionCode ,			// Exception Code:		Hexadecimal
		plpExceptionPtrs->ExceptionRecord->ExceptionCode ,			//						Decimal
		plpExceptionPtrs->ExceptionRecord->ExceptionAddress ,		// Exception Address:	Hexadecimal
		plpExceptionPtrs->ExceptionRecord->ExceptionAddress ) ;		//						Decimal
	return g_showCrashDialog ? EXCEPTION_CONTINUE_SEARCH : EXCEPTION_EXECUTE_HANDLER ;
}	// SHL_CrashHandler


int __stdcall SHL_PerformTests ( )
{
#define SHL_STDIN	ARRAY_FIRST_ELEMENT_P6C
#define SHL_STDOUT	( SHL_STDIN  + ARRAY_NEXT_ELEMENT_P6C )
#define SHL_STERR	( SHL_STDOUT + ARRAY_NEXT_ELEMENT_P6C )
#define SHL_1ST_LBL	IDS_HANDLE_STDIN

	static HANDLE hStdConsoleHandles [ ] =
	{
		NULL ,														// SHL_STDIN
		NULL ,														// SHL_STDOUT
		NULL														// SHL_STERR
	};	// static HFILE hStdConsoleHandles [ ]

	static DWORD dwStdConsoleHandleIDs [ ] =
	{
		STD_INPUT_HANDLE ,											// SHL_STDIN
		STD_OUTPUT_HANDLE ,											// SHL_STDOUT
		STD_ERROR_HANDLE											// SHL_STERR
	};	// static DWORD dwStdConsoleHandleIDs [ ]

	static SHS_STANDARD_HANDLE ashsStandardHandleIDs [ ]
	{
		SHS_INPUT ,													// Value = 1, which corresponds to STD_INPUT_HANDLE
		SHS_OUTPUT ,												// Value = 2, which corresponds to STD_OUTPUT_HANDLE
		SHS_ERROR 													// Value = 3, which corresponds to STD_ERROR_HANDLE
	};	// static SHS_STANDARD_HANDLE ashsStandardHandleIDs [ ]

	DWORD dwModde = ZERO_P6C;

	LPTSTR lpHandleLabel = NULL;
	LPTSTR lpHandleMessage = NULL;
	LPTSTR lpTargetFileName = NULL;

	for ( UINT uintIndex = ARRAY_FIRST_ELEMENT_P6C;
		       uintIndex < sizeof ( hStdConsoleHandles ) / sizeof ( HFILE );
		       uintIndex++ )
	{
		lpHandleLabel = FB_LoadString (
			FB_LOOK_IN_THIS_EXE ,
			SHL_1ST_LBL + uintIndex ,
			FB_FIRST_BUFFER_INDEX ,
			FB_HIDE_LENGTH ) ;

		if ( ( hStdConsoleHandles [ uintIndex ] = GetStdHandle ( dwStdConsoleHandleIDs [ uintIndex ] ) ) != INVALID_HANDLE_VALUE )
		{
			if ( GetConsoleMode ( hStdConsoleHandles [ uintIndex ] , &dwModde ) )
			{
				lpHandleMessage = FB_LoadString (
					FB_LOOK_IN_THIS_EXE ,
					IDS_MSG_HANDLE_IS_DEFAULT ,
					FB_BUFFER_INDEX_1 ,
					FB_HIDE_LENGTH );
				lpTargetFileName = NULL;
			}	// TRUE (The handle is attached to the console.) block, if ( GetConsoleMode ( hStdConsoleHandles [ uintIndex ] , &dwModde ) )
			else
			{
				lpHandleMessage = FB_LoadString (
					FB_LOOK_IN_THIS_EXE ,
					IDS_MSG_HANDLE_IS_REDIRECTED ,
					FB_BUFFER_INDEX_1 ,
					FB_HIDE_LENGTH );
				lpTargetFileName = SHL_GetRedirectionTarget ( hStdConsoleHandles [ uintIndex ] ) ;
			}	// FALSE (The handle is redirected.) block, if ( GetConsoleMode ( hStdConsoleHandles [ uintIndex ] , &dwModde ) )

			_tprintf (
				lpHandleMessage ,									// Format Control String (template)
				lpHandleLabel );									// Descriptive label.

			SHS_HANDLE_STATE shsHandleState = SHS_StandardHandleState ( ashsStandardHandleIDs [ uintIndex ] );
			
			switch ( shsHandleState )
			{
				case SHS_ATTACHED:
					_tprintf (
						FB_LoadString (
							FB_LOOK_IN_THIS_EXE ,
							IDS_ATTACHED_PER_SHS_STANDARDHANDLESTATE ,
							FB_BUFFER_INDEX_1 ,
							FB_HIDE_LENGTH ) );
					break;											// case SHS_ATTACHED

				case SHS_REDIRECTED:
					_tprintf (
						FB_LoadString (
							FB_LOOK_IN_THIS_EXE ,
							IDS_REDIRECTED_PER_SHS_STANDARDHANDLESTATE ,
							FB_BUFFER_INDEX_1 ,
							FB_HIDE_LENGTH ) );
					break;											// case SHS_REDIRECTED:

				default:
					DWORD dwStatusCode = GetLastError ( );
					_tprintf (
						FB_LoadString (
							FB_LOOK_IN_THIS_EXE ,
							IDS_ERRMSG_STD_HANDLE_STATE ,
							FB_BUFFER_INDEX_1 ,
							FB_HIDE_LENGTH ) ,
						dwStatusCode ,
						dwStatusCode );
					break;											// SHS_StandardHandleState reported an error.
			}	// switch ( shsHandleState )

			if ( lpTargetFileName )
			{	// Unless the handle is redirected, lpTargetFileName is NULL.
				_tprintf (
					FB_LoadString (
						FB_LOOK_IN_THIS_EXE ,
						IDS_MSG_REDIRECTION_TARGET ,
						FB_BUFFER_INDEX_2 ,
						FB_HIDE_LENGTH ) ,
					lpTargetFileName );
			}	// if ( lpTargetFileName )
		}	// TRUE (anticipated outcome) block, if ( ( hStdConsoleHandles [ uintIndex ] = GetStdHandle ( dwStdConsoleHandleIDs [ uintIndex ] ) ) != INVALID_HANDLE_VALUE )
		else
		{
			return GetLastError ( );
		}	// FALSE (UNanticipated outcome) block, if ( ( hStdConsoleHandles [ uintIndex ] = GetStdHandle ( dwStdConsoleHandleIDs [ uintIndex ] ) ) != INVALID_HANDLE_VALUE )
	}	// for ( UINT uintIndex = ARRAY_FIRST_ELEMENT_P6C; uintIndex < sizeof ( hStdConsoleHandle ) / sizeof ( HFILE ); uintIndex++ )

	return ERROR_SUCCESS ;
}	// int __stdcall SHL_PerformTests


LPTSTR __stdcall SHL_GetRedirectionTarget ( HANDLE phStdHandle )
{
	if ( phStdHandle )
	{
		/*
			--------------------------------------------------------------------
			DWORD WINAPI GetFinalPathNameByHandle(
				_In_  HANDLE hFile,
				_Out_ LPTSTR lpszFilePath,
				_In_  DWORD  cchFilePath,
				_In_  DWORD  dwFlags
			);

			This function has ANSI and Unicode (wide character) implementations,
			differentiated by the usual name decorations.
			--------------------------------------------------------------------
		*/

		typedef DWORD ( WINAPI* tGetFinalPathNameByHandle )( HANDLE , LPTSTR , DWORD , DWORD );

		RTL_OSVERSIONINFOEXW osvStruct;
		
		if ( SHL_GetOsVersion ( &osvStruct ) )
		{
			if ( osvStruct.dwMajorVersion >= 6 )
			{
				if ( LPSFSBUF rlpTargetFileName = FB_GetlpResourceBuffer ( FB_BUFFER_INDEX_3 ) )
				{	// Unless the unthinkable happens, and FB_GetlpResourceBuffer fails... 
					if ( HMODULE hKernel32 = GetModuleHandle ( L"Kernel32.dll" ) )
					{
						if ( tGetFinalPathNameByHandle faddrGetFinalPathNameByHandle = ( tGetFinalPathNameByHandle ) GetProcAddress ( hKernel32 , "GetFinalPathNameByHandleW" ) )
						{
							if ( DWORD dwFnLen = faddrGetFinalPathNameByHandle (
								phStdHandle ,									// _In_  HANDLE hFile
								( LPWSTR ) rlpTargetFileName ,					// _Out_ LPTSTR lpszFilePath
								FB_GetResourceBufferTChars ( ) ,				// _In_  DWORD  cchFilePath
								FILE_NAME_NORMALIZED ) )						// _In_  DWORD  dwFlags
							{
								return ( LPTSTR ) rlpTargetFileName;
							}	// TRUE (anticipated outcome) block, if ( DWORD dwFnLen = faddrGetFinalPathNameByHandle ( phStdHandle , ( LPWSTR ) rlpTargetFileName , FB_GetResourceBufferTChars ( ) , FILE_NAME_NORMALIZED ) )
							else
							{
								return FB_FormatMessage (
									FB_LoadString (
										FB_LOOK_IN_THIS_EXE ,
										IDS_ERRMSG_GETFILENAMEBYHANDLE ,
										FB_FIRST_BUFFER_INDEX ,
										FB_HIDE_LENGTH ) ,
									GetLastError ( ) ,
									SCF2_HEXADECIMAL );
							}	// FALSE (UNanticipated outcome) block, if ( DWORD dwFnLen = faddrGetFinalPathNameByHandle ( phStdHandle , ( LPWSTR ) rlpTargetFileName , FB_GetResourceBufferTChars ( ) , FILE_NAME_NORMALIZED ) )
						}	// TRUE (anticipated outcome) block, if ( tGetFinalPathNameByHandle faddrGetFinalPathNameByHandle = ( tGetFinalPathNameByHandle ) GetProcAddress ( hKernel32 , "GetFinalPathNameByHandleW" ) )
						else
						{
							return FB_FormatMessage (
								FB_LoadString (
									FB_LOOK_IN_THIS_EXE ,
									IDS_ERRMSG_GETPROCADDRESS ,
									FB_FIRST_BUFFER_INDEX ,
									FB_HIDE_LENGTH ) ,
								GetLastError ( ) ,
								SCF2_HEXADECIMAL );
						}	// FALSE (UNanticipated outcome) block, if ( tGetFinalPathNameByHandle faddrGetFinalPathNameByHandle = ( tGetFinalPathNameByHandle ) GetProcAddress ( hKernel32 , "GetFinalPathNameByHandleW" ) )
					}	// TRUE (anticipated outcome) block, if ( HMODULE hKernel32 = GetModuleHandle ( L"Kernel32.dll" ) )
					else
					{
						return FB_FormatMessage (
							FB_LoadString (
								FB_LOOK_IN_THIS_EXE ,
								IDS_ERRMSG_GETMODULEHANDLE ,
								FB_FIRST_BUFFER_INDEX ,
								FB_HIDE_LENGTH ) ,
							GetLastError ( ) ,
							SCF2_HEXADECIMAL );
					}	// FALSE (UNanticipated outcome) block, if ( HMODULE hKernel32 = GetModuleHandle ( L"Kernel32.dll" ) )
				}	// TRUE (anticipated outcome) block, if ( LPSFSBUF rlpTargetFileName = FB_GetlpResourceBuffer ( FB_BUFFER_INDEX_3 ) )
				else
				{
					return FB_FormatMessage (
						FB_LoadString (
							FB_LOOK_IN_THIS_EXE ,
							IDS_ERRMSG_GETLPRESOURCEBUFFER ,
							FB_FIRST_BUFFER_INDEX ,
							FB_HIDE_LENGTH ) ,
						GetLastError ( ) ,
						SCF2_HEXADECIMAL );
				}	// FALSE (UNanticipated outcome) block, if ( LPSFSBUF rlpTargetFileName = FB_GetlpResourceBuffer ( FB_BUFFER_INDEX_3 ) )
			}	// TRUE (anticipated outcome) block, if ( osvStruct.dwMajorVersion >= 6 )
			else
			{
				return FB_LoadString (
					FB_LOOK_IN_THIS_EXE ,
					IDS_ERRMSG_UNSUPPORTED_FEATURE ,
					FB_FIRST_BUFFER_INDEX ,
					FB_HIDE_LENGTH );
			}	// FALSE (UNanticipated outcome) block, if ( osvStruct.dwMajorVersion >= 6 )
		}	// TRUE (anticipated outcome) block, if ( GetVersionEx ( &osvStruct ) )
		else
		{
			return FB_FormatMessage (
				FB_LoadString (
					FB_LOOK_IN_THIS_EXE ,
					IDS_ERRMSG_GETVERSIONINFOEX ,
					FB_FIRST_BUFFER_INDEX ,
					FB_HIDE_LENGTH ) ,
				GetLastError ( ) ,
				SCF2_HEXADECIMAL );
		}	// FALSE (UNanticipated outcome) block, if ( GetVersionEx ( &osvStruct ) )
	}	// TRUE (anticipated outcome) block, if ( phStdHandle )
	else
	{
		return FB_LoadString (
			FB_LOOK_IN_THIS_EXE , 
			IDS_ERRMSG_HANDLE_IS_NULL , 
			FB_BUFFER_INDEX_4 , 
			FB_HIDE_LENGTH );
	}	// FALSE (UNanticipated outcome) block, if ( phStdHandle )
}	// LPTSTR __stdcall SHL_GetRedirectionTarget


BOOL __stdcall SHL_GetOsVersion ( RTL_OSVERSIONINFOEXW* pk_OsVer )
{
	// RTL_OSVERSIONINFOEXW is defined in winnt.h.
	typedef LONG ( WINAPI* tRtlGetVersion )( RTL_OSVERSIONINFOEXW* );

	memset ( pk_OsVer ,												// Starting address
		     0 ,													// Initialization constant
			 sizeof ( RTL_OSVERSIONINFOEXW ) );						// Bytes to initialize
	pk_OsVer->dwOSVersionInfoSize = sizeof ( RTL_OSVERSIONINFOEXW );

	HMODULE hNtDll = GetModuleHandleW ( L"ntdll.dll" );				// ntdll.dll is already loaded.

	if ( tRtlGetVersion fRtlGetVersion = ( tRtlGetVersion ) GetProcAddress ( hNtDll , "RtlGetVersion" ) )
		return ( fRtlGetVersion ( pk_OsVer ) ) == ERROR_SUCCESS;
	else
		return FALSE;												// This will never happen, since all processes load ntdll.dll.
}	// BOOL __stdcall SHL_GetOsVersion