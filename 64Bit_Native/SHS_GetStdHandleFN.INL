//	LPTSTR SHS_STANDARDHANDLESTATE_API SHS_GetStdHandleFNA
//	(
//		CSHS_STANDARD_HANDLE penmStdHandleID ,								// Specify the desired standard stream.
//		CUINT                puintBufferID                   				// Specify the zero based index of the buffer whose address you need.
//	)

    if ( HANDLE hStdHandle = StandardeConsoleHandleFromEnum ( penmStdHandleID ) )
    {
        RTL_OSVERSIONINFOEXW osvStruct;

        if ( GetOsVersionInfo ( &osvStruct ) )
        {
			if ( WWKW_OSIsVistaOrNewer ( osvStruct ) )
            {	// Get parameters used two or more times that must be retrieved from other libraries.
				bool fUseFixedBuffer	= ( puintBufferID < ( unsigned int ) FB_GetResourceBufferCount ( ) ) ;
				int  intFixedBufSize	= fUseFixedBuffer ? FB_GetResourceBufferTChars ( ) : m_intOutBufSize ;

				//	------------------------------------------------------------
				//	Macro TcharsMinBufSizeP6C takes into account both the number
				//	of bytes occupied by one TCHAR and the room required for the
				//	terminal null character.
				//	------------------------------------------------------------

                #pragma warning ( suppress: 4018 )
				if ( LPSFSBUF rlpTargetFileName = fUseFixedBuffer ? FB_GetlpResourceBuffer ( puintBufferID ) : ( LPSFSBUF ) m_lpOutputBuffer )
                {	// Unless the unthinkable happens, and FB_GetlpResourceBuffer fails... 
                    if ( HMODULE hKernel32 = GetModuleHandle ( _T ( "Kernel32.dll" ) ) )
                    {
						if ( tGetFinalPathNameByHandle faddrGetFinalPathNameByHandle = ( tGetFinalPathNameByHandle ) GetProcAddress ( hKernel32 , "GetFinalPathNameByHandleW" ) )
						{
							if ( DWORD dwFnLen = faddrGetFinalPathNameByHandle ( hStdHandle ,						// _In_  HANDLE hFile
              					                                                 ( LPTSTR ) rlpTargetFileName ,		// _Out_ LPTSTR lpszFilePath
								                                                 intFixedBufSize ,					// _In_  DWORD  cchFilePath
								                                                 FILE_NAME_NORMALIZED ) )			// _In_  DWORD  dwFlags
							{
								if ( dwFnLen <= ( DWORD ) intFixedBufSize )
								{	// The entire file name string fits into the static buffer.
									m_intFNLen = dwFnLen ;
									
									//	----------------------------------------
									//	There are situations in which system 
									//	status code ERROR_INVALID_PARAMETER is
									//	set, but the program recovers and
									//	succeeds, but it never clears the status
									//	code. To avoid confusing routines higher
									//	in the call stack, clear it now.
									//	----------------------------------------

									if ( GetLastError ( ) == ERROR_INVALID_PARAMETER )
									{
										SetLastError ( ERROR_SUCCESS ) ;
									}	// if ( GetLastError ( ) == ERROR_INVALID_PARAMETER )

									return ( LPTSTR ) rlpTargetFileName ;
								}	// TRUE (anticipated outcome) block, if ( dwFnLen <= ( DWORD ) intFixedBufSize )
								else
								{	// The string is too long to fit in the 4097 character static buffer. 
									return FB_FormatMessage3 ( m_hinstMe ,
										                       IDS_ERRMSG_REDIRECTED_FN_TOO_LONG ,
										                       FB_REVSB_CALL_GETLASTERROR ,							// Get it yourself, by calling GetLastError.
										                       FB_SCF2_HEXADECIMAL );								// Display the hexadecimal representation of the status code.
								}	// FALSE (unanticipated outcome) block, if ( dwFnLen <= ( DWORD ) intFixedBufSize )
							}	// if ( DWORD dwFnLen = faddrGetFinalPathNameByHandle ( hStdHandle , ( LPTSTR ) rlpTargetFileName , intFixedBufSize , FILE_NAME_NORMALIZED )
							else
							{
								return FB_FormatMessage3 ( m_hinstMe ,
				              					           IDS_ERRMSG_GETFILENAMEBYHANDLE ,
									                       FB_REVSB_CALL_GETLASTERROR ,								// Get it yourself, by calling GetLastError.
									                       FB_SCF2_HEXADECIMAL );									// Display the hexadecimal representation of the status code.
							}	// FALSE (UNanticipated outcome) block, if ( DWORD dwFnLen = faddrGetFinalPathNameByHandle ( phStdHandle , ( LPWSTR ) rlpTargetFileName , intFixedBufSize , FILE_NAME_NORMALIZED ) )
						}	// TRUE (anticipated outcome) block, if ( tGetFinalPathNameByHandle faddrGetFinalPathNameByHandle = ( tGetFinalPathNameByHandle ) GetProcAddress ( hKernel32 , "GetFinalPathNameByHandleW" ) )
                        else
                        {
							return FB_FormatMessage3 ( m_hinstMe ,
                                                       IDS_ERRMSG_GETPROCADDRESS ,
                                                       FB_REVSB_CALL_GETLASTERROR ,									// Get it yourself, by calling GetLastError.
                                                       FB_SCF2_HEXADECIMAL );										// Display the hexadecimal representation of the status code.
                        }	// FALSE (anticipated outcome) block, if ( tGetFinalPathNameByHandle faddrGetFinalPathNameByHandle = ( tGetFinalPathNameByHandle ) GetProcAddress ( hKernel32 , "GetFinalPathNameByHandleW" ) )
                    }	// TRUE (anticipated outcome) block, if ( HMODULE hKernel32 = GetModuleHandle ( L"Kernel32.dll" ) )
                    else
                    {
						return FB_FormatMessage3 ( m_hinstMe ,
							                       IDS_ERRMSG_GETMODULEHANDLE ,
							                       FB_REVSB_CALL_GETLASTERROR ,										// Get it yourself, by calling GetLastError.
							                       FB_SCF2_HEXADECIMAL );											// Display the hexadecimal representation of the status code.
                    }	// FALSE (UNanticipated outcome) block, if ( HMODULE hKernel32 = GetModuleHandle ( L"Kernel32.dll" ) )
                }	// TRUE (anticipated outcome) block, if ( LPSFSBUF rlpTargetFileName = fUseFixedBuffer ? FB_GetlpResourceBuffer ( puintBufferID ) : ( LPSFSBUF ) m_lpOutputBuffer )
                else
                {
					return FB_FormatMessage3 ( m_hinstMe ,
					   						   IDS_ERRMSG_GETLPRESOURCEBUFFER ,
											   FB_REVSB_CALL_GETLASTERROR ,											// Get it yourself, by calling GetLastError.
											   FB_SCF2_HEXADECIMAL );												// Display the hexadecimal representation of the status code.
                }	// FALSE (UNanticipated outcome) block, if ( LPSFSBUF rlpTargetFileName = fUseFixedBuffer ? FB_GetlpResourceBuffer ( puintBufferID ) : ( LPSFSBUF ) m_lpOutputBuffer )
            }	// TRUE (anticipated outcome) block, if ( WWKW_OSIsVistaOrNewer ( osvStruct ) )
            else
            {
				return FB_FormatMessage3 ( m_hinstMe ,
						                   IDS_ERRMSG_UNSUPPORTED_FEATURE ,
					                       FB_REVSB_CALL_GETLASTERROR ,												// Get it yourself, by calling GetLastError.
					                       FB_SCF2_HEXADECIMAL );													// Display the hexadecimal representation of the status code.
            }	// FALSE (UNanticipated outcome) block, if ( WWKW_OSIsVistaOrNewer ( osvStruct ) )
        }	// TRUE (anticipated outcome) block, if ( GetVersionEx ( &osvStruct ) )
        else
        {
			return FB_FormatMessage3 ( m_hinstMe ,
				                       IDS_ERRMSG_GETVERSIONINFOEX ,
				                       FB_REVSB_CALL_GETLASTERROR ,													// Get it yourself, by calling GetLastError.
				                       FB_SCF2_HEXADECIMAL );														// Display the hexadecimal representation of the status code.
        }	// FALSE (UNanticipated outcome) block, if ( GetVersionEx ( &osvStruct ) )
    }	// TRUE (anticipated outcome) block, if ( ( HANDLE hStdHandle = StandardeConsoleHandleFromEnum ( penmStdHandleID ) ) )
    else
    {
		return FB_FormatMessage3 ( m_hinstMe ,
			                       IDS_ERRMSG_HANDLE_IS_NULL ,
			                       FB_REVSB_CALL_GETLASTERROR ,														// Get it yourself, by calling GetLastError.
			                       FB_SCF2_HEXADECIMAL );															// Display the hexadecimal representation of the status code.
    }	// FALSE (UNanticipated outcome) block, if ( ( HANDLE hStdHandle = StandardeConsoleHandleFromEnum ( penmStdHandleID ) ) )
//	SHS_GetStdHandleFNA AND SHS_GetStdHandleFNW effectively end here.