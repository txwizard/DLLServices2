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

    Remarks:            Since it was created specifically to support a managed
                        caller, SHS_GetStdHandleFNCLI saves LastError into a
                        module variable, and provides SHS_GetStdHandleFNError,
                        in lieu of the managed wrapper, for retrieving it.

                        Since local storage used by the functions declared in
                        this header is allocated on the stack (all 8 bytes!),
                        and the code is free of side effects, they are
                        manifestly thread safe.

    License:			Copyright (C) 2015, David A. Gray. 	All rights reserved.

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions are met:

    *   Redistributions of source code must retain the above copyright notice,
        this list of conditions and the following disclaimer.

    *   Redistributions in binary form must reproduce the above copyright
        notice, this list of conditions and the following disclaimer in the
        documentation and/or other materials provided with the distribution.

    *   Neither the name of Simple Soft Services, Inc. nor the names of its
        contributors may be used to endorse or promote products derived from
        this software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
    AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
    IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
    ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
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
    2015/07/09 2.2.0.6 DAG This header and the associated routine make their
                           first appearance in WWConAid.dll.

    2015/07/14 2.3.0.7 DAG Drop a declaration for SHS_GetRedirectionTarget into
                           this header, and define ANSI and Unicode versions in
                           a pair of C++ source files.

    2015/10/07 2.4.0.9 DAG Implement SHS_GetStdHandleFN, which I never brought
                           over from the Standard Handles Laboratory.

    2015/10/28 2.5.0.1 DAG Implement SHS_GetStdHandleFNCLI as a wrapper around
                           SHS_GetStdHandleFNW that takes responsibility for the
                           stack, AND writes its output directly into a buffer
                           supplied by its caller, even if that happens to be a
                           managed routine.
    ============================================================================
*/

#include <Windows.h>
#include <Const_Typedefs_WW.H>


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

typedef enum _SHS_HANDLE_LABELS
{
    SHS_HANDLE_SHORT_LABEL ,			// Short Label
    SHS_HANDLE_LONG_LABEL ,				// Long Label
    SHS_HANDLE_CONSTANT_NAME 			// Standard Handle Constant Name
} SHS_HANDLE_LABELS ;

typedef const SHS_STANDARD_HANDLE			CSHS_STANDARD_HANDLE ;
typedef const SHS_HANDLE_LABELS				CSHS_HANDLE_LABELS ;

#define SHS_ERROR_ID_IS_UNINITIALIZED		( APPLICATION_ERROR_MASK          | 0x00006000 )
#define SHS_ERROR_INVALID_OUTPUT_BUFFER		( SHS_ERROR_ID_IS_UNINITIALIZED   + 0x00000001 )
#define SHS_ERROR_INVALID_OUTBUF_SIZE		( SHS_ERROR_INVALID_OUTPUT_BUFFER + 0x00000001 )
#define SHS_ERROR_ID_IS_OUT_OF_RANGE		( SHS_ERROR_INVALID_OUTBUF_SIZE   + 0x00000001 )
#define SHS_ERROR_INVALID_LABEL_GROUP		( SHS_ERROR_ID_IS_OUT_OF_RANGE    + 0x00000001 )
#define SHS_ERROR_INVALID_LABEL_ID			( SHS_ERROR_INVALID_LABEL_GROUP   + 0x00000001 )
#define SHS_ERROR_NAME_TOO_LONG_FOR_FB		( SHS_ERROR_INVALID_LABEL_ID      + 0x00000001 )
#define SHS_ERROR_REDIRECTED_FN_TOO_LONG	( SHS_ERROR_NAME_TOO_LONG_FOR_FB  + 0x00000001 )

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

        Function Name:  StandardeConsoleHandleFromEnum

        Definition:		StandardeConsoleHandleFromEnum.CPP

        Synopsis:       Map a member of the SHS_STANDARD_HANDLE enumeration to
                        a standard console stream, and return its handle.

        Arguments:      penmStdHandleID	= Use a SHS_STANDARD_HANDLE enumeration
                                          member to identify a handle to return.

        Returns:        If the function succeeds, the returned HANDLE is an open
                        handle of the desired type.

        Remarks:		LEAVE THE RETURNED HANDLE OPEN! Bad Things can happen
                        if you close it; Windows will do that as part of its
                        process cleanup.
        =======================================================================
    */

    HANDLE SHS_STANDARDHANDLESTATE_API StandardeConsoleHandleFromEnum ( CSHS_STANDARD_HANDLE penmStdHandleID );


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

          Remarks:      Unless this function returns SHS_SYSTEM_ERROR, calling
                        GetLastError is pointless, since the return value is
                        meaningless under other circumstances.

                        To determine whether a handle is attached to its 
                        console or redirected, this routine calls GetConsoleMode
                        on the handle returned by GetStdHandle. If that call 
                        succeeds, the handle is attached to a console.  
                        Otherwise, GetLastError returns ERROR_INVALID_HANDLE,
                        confirming that the handle is redirected, since a handle
                        loses its console mode when that happens. When 
                        GetLastError returns ERROR_INVALID_HANDLE, the status
                        code is cleared, by calling SetLastError, and the handle
                        is reported as redirected. Otherwise, this function 
                        returns SHS_SYSTEM_ERROR to signal the calling routine
                        to call GetLastError, and recover from the system error.

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

    /*
        ========================================================================

        Function Name:  SHS_GetLabelString

        Definition:		SHS_ResolveLabelStringIDA.C and 
                        SHS_ResolveLabelStringIDW.C

        Synopsis:       Use the SHS_HANDLE_STATE enumeration to report the fully
                        qualified name of the file to which the standard handle
                        specified by penmStdHandleID is redirected.

        Arguments:      penmStdHandleID	= Use a SHS_STANDARD_HANDLE enumeration
                                          member to identify a handle to query.

                        penmStdHandleID	= Use a CSHS_HANDLE_LABELS enumeration
                                          member to identify the set from which
                                          to select the label.

                        puintBufferID	= Specify which of the (currently 5)
                                          static buffers available through
                                          FixedStringBuffers.dll to use. The
                                          function returns the address of the
                                          selected buffer.

        Returns:        If the function succeeds, it returns a static pointer to
                        the string that contains the name of the file to which
                        specified handle is redirected. The string is designated
                        as static because it points to a static buffer that
                        belongs to FixedStringBuffers.dll.

                        Please see FixedStringBuffers.H for additional details.

        Remarks:		Since this routine returns a string, there are ANSI and
                        Unicode (wide character) implementations, called through
                        the same alias.

                        Unless this function returns NULL, the value returned by
                        GetLastError is meaningless.
        ========================================================================
    */

    LPTSTR SHS_STANDARDHANDLESTATE_API SHS_GetLabelStringA
        (
            CSHS_STANDARD_HANDLE penmStdHandleID ,								// Specify the desired standard stream.
            CSHS_HANDLE_LABELS   penmLabelType ,								// Specify the set from which to select the label.
            CUINT                puintBufferID                   				// Specify the zero based index of the buffer whose address you need.
        ) ;

    LPTSTR SHS_STANDARDHANDLESTATE_API SHS_GetLabelStringW
        (
            CSHS_STANDARD_HANDLE penmStdHandleID ,								// Specify the desired standard stream.
            CSHS_HANDLE_LABELS   penmLabelType ,								// Specify the set from which to select the label.
            CUINT                puintBufferID                   				// Specify the zero based index of the buffer whose address you need.
        ) ;

    /*
        ========================================================================

        Function Name:  SHS_GetStdHandleFN

        Definition:		SHS_GetStdHandleFNA.CPP and SHS_GetStdHandleFNW.CPP

        Synopsis:		Combine the SHS_HANDLE_STATE and HS_HANDLE_LABELS 
                        enumerations to identify and return one of several
                        labels to use in reports to describe one of the three
                        standard streams associated with most console programs.

        Arguments:      penmStdHandleID	= Use a SHS_STANDARD_HANDLE enumeration
                                          member to identify a handle to query.
                        
                        puintBufferID	= Specify which of the (currently 5)
                                          static buffers available through
                                          FixedStringBuffers.dll to use, or
                                          pass an out of range index to request
                                          a buffer allocated by CoTaskMemAlloc.
                                          The function returns the address of 
                                          the selected buffer. Please see the
                                          Remarks for important information.

        Returns:        If the function succeeds, it returns a pointer to the
                        desired string.

                        There are two conditions when this routine returns the
                        empty string (and it returns a TRUE empty string, rather
                        than a null pointer!).

                        1)	The handle is attached to its console. Therefore,
                            there is no associated file or pipe.

                        2)	A system error prevented it from completing its
                            assigned task.

        Remarks:		Since it is intended for use with managed console mode
                        programs, SHS_GetStdHandleFNCLI must use 

                        Since it is intended for use in character mode programs
                        that usually either have but one thread, or delegate all
                        interaction with the console to a single thread, you may
                        trade thread safety for elimination of memory management
                        chores for the calling routine. To take advantage of the
                        simplified memory management, specify a valid buffer
                        index for argument puintBufferID. The current version of
                        FixedStringBuffers.dll exposes 5 resource string buffers
                        (numbered 0 through 4), each having a capacity of 4097
                        characters.

                        Utility function FB_GetSprintFBufferCount returns the
                        number of available buffers, from which the maximum
                        valid index is derived by subtracting 1 from it.

                        If puintBufferID has a value between 0 and the value
                        returned by FB_GetSprintFBufferCount inclusive, the
                        correspondingly numbered string resource buffer is used,
                        at the expense of thread safety.

                        If puintBufferID is greater than or equal to the value
                        returned by FB_GetSprintFBufferCount, a buffer with a
                        capacity of FB_GetResourceBufferTChars() characters is
                        allocated via CoTaskMemAlloc. Both methods yield buffers
                        of the same size (4097 TCHARs), which should be more
                        than enough to handle your redirected standard handle.

                        Since this routine returns a string, there are ANSI and
                        Unicode (wide character) implementations, called through
                        the same alias. If puintBufferID specifies a valid fixed
                        buffer index, the returned static buffer has a capacity
                        of 4097 characters.

                        Unless this function returns the empty string, the value
                        returned by GetLastError is meaningless, and should not
                        be queried, because it may contain a bogus 057h (invalid
                        parameter) that has already been handled deep inside the
                        routine that assembles the path string.
        ========================================================================
    */

    LPTSTR SHS_STANDARDHANDLESTATE_API	SHS_GetStdHandleFNA
        (
            CSHS_STANDARD_HANDLE penmStdHandleID ,								// Specify the desired standard stream.
            CUINT                puintBufferID                   				// Specify the zero based index of the buffer whose address you need.
        ) ;
    LPTSTR SHS_STANDARDHANDLESTATE_API	SHS_GetStdHandleFNW
        (
            CSHS_STANDARD_HANDLE penmStdHandleID ,								// Specify the desired standard stream.
            CUINT                puintBufferID                   				// Specify the zero based index of the buffer whose address you need.
        ) ;
    INT32 SHS_STANDARDHANDLESTATE_API	 __stdcall SHS_GetStdHandleFNCLI
        (
            CSHS_STANDARD_HANDLE penmStdHandleID ,								// Specify the desired standard stream.
            LPBYTE				 plpOutputBuffer ,								// Pointer to buffer of at least pintOutBufSize bytes where SHS_GetStdHandleFNCLI can deliver its output
            INT32				 pintOutBufSize									// Size, in BYTES, of the buffer to which plpOutputBuffer points
        ) ;

    DWORD 	SHS_STANDARDHANDLESTATE_API	 __stdcall SHS_GetStdHandleFNError ( void ) ;

    #if defined ( UNICODE )
        #define SHS_GetLabelString		SHS_GetLabelStringW
        #define SHS_GetStdHandleFN		SHS_GetStdHandleFNW
    #else
        #define SHS_GetLabelString		SHS_GetLabelStringA
        #define SHS_GetStdHandleFN		SHS_GetStdHandleFNA
    #endif	/* #if defined ( UNICODE ) */
#if defined ( __cplusplus )
}
#endif  /* #if defined ( __cplusplus ) */
#endif	/* #if !defined ( STANDARDHANDLESTATE_INCLUDED ) */