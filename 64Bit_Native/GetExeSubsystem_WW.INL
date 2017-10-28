/*
    ============================================================================

    File Name:          GetExeSubsystemA_WW.CPP

    Function Name:      GetExeSubsystemA_WW (P6CUtilLib1.dll)
						PROC_GetExeSubsystem (ProcessInfo.dll)

    Synopsis:           Determine the subsystem in which a Portable Executable
                        file runs.

    Arguments:          plpMMainModuleFQFN  = A pointer to a null terminated
                                              string that contains the name of a
                                              Portable Executable file to test.

    Returns:            If the function succeeds, its return value is a DWORD,
                        upcast from the WORD value retrieved from the first EXE
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
    ============================================================================
*/

    if ( plpMMainModuleFQFN )
    {	// Caller gave us a good pointer.
        HANDLE  hStartupPgmFile             = CreateFile ( plpMMainModuleFQFN ,
                                                           GENERIC_READ ,
                                                           FILE_SHARE_READ ,
                                                           NULL ,
                                                           OPEN_EXISTING ,
                                                           FILE_ATTRIBUTE_NORMAL ,
                                                           0 ) ;

        if ( hStartupPgmFile )
        {	// File handle is valid.
            LPVOID lpExeMap                 = MapStartupExeIntoMemory ( hStartupPgmFile ) ;

            if ( lpExeMap )
            {	// Pointer to memory-mapped file is valid.
                PIMAGE_DOS_HEADER dosHeader = ( PIMAGE_DOS_HEADER ) lpExeMap ;

                //  ------------------------------------------------------------
                //  Since UnmapViewOfFile invalidates the memory to which 
                //  lpExeMap and dosHeader (which is lpExeMap explicitly
                //  cast to a new type), QueryExeHeader must be called, and its
                //  return value saved, before UnmapViewOfFile is called to free
                //  the memory.
                //
                //  I have verified by experiment that the void pointer returned
                //  by MapViewOfFile and the handle opened by CreateFileMapping
                //  and used by MapViewOfFile are independent of each other. 
                //  Once MapViewOfFile returns, the handle can be closed without
                //  losing the buffer or invalidating the pointer.
                //  ------------------------------------------------------------

                if ( dosHeader->e_magic == IMAGE_DOS_SIGNATURE )
                {   // File appears to be a valid PE file.
					DWORD rdwSubsystem		= QueryExeHeader ( dosHeader ) ;
                    
                    if ( UnmapViewOfFile ( dosHeader ) )
					{	// Return with the Subsystem ID. A successful outcome always ends here.
						return rdwSubsystem ;
					}   // TRUE (expected outcome) block, if ( UnmapViewOfFile ( dosHeader ) )
					else
                    {   // UnmapViewOfFile calls SetLastError when it shoots craps.
                        return IMAGE_SUBSYSTEM_UNKNOWN ;
                    }   // FALSE (UNexpected outcome) block, if ( UnmapViewOfFile ( dosHeader ) )
                }	// TRUE (expected outcome) block, if ( dosHeader->e_magic == IMAGE_DOS_SIGNATURE )
                else
                {
                    SetLastError ( GETSUBSYST_NO_MAGIC_WW ) ;
                    return IMAGE_SUBSYSTEM_UNKNOWN ;
                }   // FALSE (UNexpected outcome) block, if ( dosHeader->e_magic == IMAGE_DOS_SIGNATURE )
            }   // TRUE (expected outcome) block, if ( lpExeMap )
            else
            {   // MapStartupExeIntoMemory called SetLastError, either directly or indirectly.
                return IMAGE_SUBSYSTEM_UNKNOWN ;
            }   // FALSE (UNexpected outcome) block, if ( lpExeMap )
        }   // TRUE (expected outcome) block, if ( hStartupPgmFile )
		else
		{
			return IMAGE_SUBSYSTEM_UNKNOWN ;
		}   // FALSE (UNexpected outcome) block, if ( hStartupPgmFile )
    }   // TRUE (expected outcome) block, if ( plpMMainModuleFQFN )
    else
    {	// Report the bad pointer.
		Unless ( GetLastError ( ) )
		{	// If Windows didn't set LastError, we must do so.
			SetLastError ( GETSUBSYST_NULL_FILENAME_POINTER_WW ) ;
		}	// Unless ( GetLastError ( ) )

        return IMAGE_SUBSYSTEM_UNKNOWN ;
    }   // FALSE (UNexpected outcome) block, if ( plpMMainModuleFQFN )