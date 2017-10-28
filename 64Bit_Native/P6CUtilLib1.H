#ifndef _P6CUTILLIB1_INCLUDED
#define _P6CUTILLIB1_INCLUDED

#if defined (_MSC_VER) && (_MSC_VER >= 1020)
    #pragma once
#endif  /*  #if defined (_MSC_VER) && (_MSC_VER >= 1020) */

/*
@dllname:   P6CUtilLib1.dll
*/

/*
    ============================================================================

    Header File Name:   P6CUtilLib1.H

    Library Name:       P6CUtilLib1.lib

    Required DLL:       P6CUtilLib1.dll

    Synopsis:           This header defines the public structures and functions
                        exposed by library P6CUtilLib1.DLL.

    Dependencies:       This library requires shell32.dll version 3.51 or later.

    Remarks:            This library is the result of a merger of several
                        library modules that I developed in the course of
                        creating several other complete programs and libraries.

                        Its initial version number is one higher than that of
                        the highest version number assigned to any module from
                        which code was drawn.

                        Use the following binary to debug from VB.

                           C:\Program Files\Microsoft Visual Studio\VB98\vb6.exe

                        Use the following binary to debug from the VC=+ test
                        harness.

                           ANSI Entry Points    P6CUtilLib1\ANSI_Test_Harness\Debug\Test_Harness.exe
                           Unicode Entry Points P6CUtilLib1\Unicode_Test_Harness\Debug\Test_Harness.exe

                        Due to the origins of most of the functions declared in
                        this header, and the volume of associated documentation,
                        which I keep with the declarations, most functions are
                        declared in other header files, which are included right
                        behind CommonConstants_P6C.H, which pulls in Windows and
                        CRT headers, then adds numerous commonly used constants
                        not declared in the CRT or the Windows Platform SDK, and
                        an assortment of general purpose preprocessor macros.

    Author:             David A. Gray, of Simple Soft Services, Inc.,
                        d/b/a WizardWrx. http://www.wizardwrx.com/

    Copyright:          Copyright 2005-2014, David A. Gray, Irving, Texas, USA.
                        All rights reserved world wide.

                        This code may be freely reproduced, so long as it
                        remains intact, including this copyright notice and
                        license.

                        However, since it is in the public domain, it is
                        unsupported in any way and carries no warranty. Use it
                        at your own risk.

    Date Created:       Tuesday, 16 August 2005

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Author Synopsis
    ---------- ------ ----------------------------------------------------------
    2005/09/18 DAG/WW Initial working version of a collection of modules
                      gathered from working code in several projects.

    2005/10/14 DAG/WW Add GetVolumeInformation_P6C.H.

    2006/09/26 DAG/WW 1) Replace calls to SetLastError_P6Lib1 with calls to
                         Windows API function SetLastError.

                      2) Replace calls to GetLocalHeap_P6LIB1 with calls to
                         GetProcessHeap, using a local variable.

                      2) Add HINSTANCE to GetOpenFileName_VB_P6C and
                         GetOpenFileName_VB_P6C function prototypes.

                      3) Move prototype for GetSaveFileName_P6C to
                         FileOpenSaveDialog_P6C.h.

    2006/10/27 DAG/WW Add strEscape_P6C.H, containing prototypes for ANSI and
                      Unicode versions of function strEscape, and prototype for
                      strEscape_VB_P6C, the companion VB/VBA wrapper function,
                      and new function strCopyCStrToBstr_P6C, by way of
                      strCopyCStrToBstr_P6C.H.

    2006/11/03 DAG/WW Remove the unused HINSTANCE argument, which is supplied
                      internally from this DLL, through GetOpenFileName_VB_P6C
                      and GetSaveFileName_VB_P6C.

    2006/11/03 DAG/WW Add CreateGUID_WW.H header for new function CreateGUID_WW
                      and its constants.

    2006/11/04 DAG/WW Remove CreateGUID_WW.H header for function CreateGUID_WW
                      and its constants, because the code proved to be unstable
                      in a VB project.

    2010/03/08 DAG/WW Eliminate include directives that confilct with those in
                      CommonConstants_P6C.H, which MUST be included therein, to
                      ensure that they are intrepreted in the expected way by
                      the C preprocessor.

                      Add #pragma once immediately belos directive #define
                      _P6CUTILLIB1_INCLUDED, as further insurance against this
                      header being processed more than once.

                      Fix FOS_SZFILTERS_NULL_ERR_P6C and the other error codes,
                      all of which are missing an OR operator.

    2010/04/25 DAG/WW Reinstate direct inclusion of windows.h, since, as of this
                      date, CommonConstants_P6C.H works just fine without it.

    2010/06/04 DAG/WW Move the #pragma once preprocessor directive outside the
                      scope of the preprocessor variable which performs the same
                      function for other C compilers.

    2010/11/05 DAG/WW Add the following functions, all long overdue.

                        DirWindowsSystem_WW_VB
                        DirWindowsSystem_WW
                        DirWindows_WW_VB
                        DirWindows_WW
                        LeakTest_WW

    2011/06/04 DAG/WW Increment version to 2,3,24,0, to cover the following
                      additions to the list of exported functions, all of which
                      are already bound into the library, but were left off the
                      list.

                            ---------------------------------------
                            Name                            Ordinal
                            ---------------------           -------
                            GetWinVersionInfo_P6C           @52
                            GetWINCSDStringA_P6C            @53
                            GetWINCSDStringW_P6C            @54
                            ---------------------------------------

    2011/06/08 DAG/WW Increment version to 2,3,25,0 to cover an overhaul of the
                      organization of my dynamic link libraries, such that every
                      function in static link library P6CLIB1 is exported by at
                      least one, and preferably only one, dynamic link library.

    2011/09/28 DAG/WW Increment version to 2,3,26,0 to cover improved versions
                      of functions linked from static link library P6CLIB1 and
                      imported from other dynamic link libraries that provide
                      assorted primitives used by the routines in this library,
                      and the addition of Unicode implementations of functions
                      ShellExecute_P6C and GetVolumeInformation_P6C.

    2011/10/26 DAG/WW Increment version to 2,3,27,0 to cover removal of headers
                      included in P6CStringLib1.H, which declare functions that
                      I already moved into that library, where they logically
                      belong.

                      Eliminating this redundancy avoids confusion about which
                      library includes them.

                      There being no revisions to the executable code, the test
                      harnesses were rebuilt and run, and had their version
                      numbers synchronized, but their executable code is also
                      unchanged.

    2012/05/18 DAG/WW 1) Increment version to 2,3,28,0, to cover incorporating
                         new versions of headers which define functions imported
                         into this library from most of my other libraries.

                      2) Relocate the StrEscape routines to their logical home,
                         P6CStringLib1.

                      3) Move strCopyCStrToBstr_P6C to P6CStringLib1, from
                         P6CUtilLib1.

    2012/07/13 DAG/WW Move declaration of LeakTest_WW to WWKernelLibWrapper.H,
                      and move the implementation to WWKernelLibWrapper.dll.

                      While I have it open in an editor, make three cosmetic
                      changes that I am applying as I vist each header.

                      1) Move the #pragma once directive inside the guard code
                         block.

                      2) Substitute APPLICATION_ERROR_MASK for CUSTOMER_FLAG.

                      3) Label the arguments in the file open common dialog
                         wrapper routines.

    2012/07/16 DAG/WW Version 2,4,29,2 is a tiger-proofing build.
                      I am also adding this header to the Visual Studio project
                      from which the library is built.

    2013/05/25 DAG/WW Version 2,4,30,3 is a future-proofing build, which appends
                      a _WW suffix to the name of symbolic constant SE_SUCCESS.

    2013/07/01 DAG/WW In addition to adding a pair of related functions, Version
                      2, 5, 31,0 hides some constants that contribute nothing to
                      its public interface.

                      1) Eliminate CommonConstants_P6C.H in favor of the much
                         more focused Const_Typedefs_WW.H.

                      3) Move GetTempPath_WW.H and the functions declared by it
                         to P6CUtilLib1.H and P6CUtilLib1.dll, respectively.

                      4) Delete obsolete constant FOS_SZFILTERS_INVALID_ERR_P6C.

                      5) Make status codes unique throughout the library.

                      6) Change the calling convention of every routine that
                         returns a Basic String (BSTR) from APIENTRY to
                         __stdcall, and add a const modifier to the inputs that
                         don't already have them.

    2013/07/01 DAG/WW Remove references to pNArgs from the documentation of
                      GetOpenFileName_VB_P6C and GetSaveFileName_VB_P6C, since
                      it isn't in the actual prototype of either.
                      This is only a documentation change, so only the build
                      number is incremented, giving a new version number of
                      2, 5, 32,0.

    2013/08/04 DAG/WW Version 2, 5, 33,0:
                      Incorporate the two routines declared in FileExists_WW.H.

    2014/04/08 DAG/WW Version 2, 6, 34,0:
                      Declare the following new functions:

                      1) GetProcessSubsystem_WW
                      2) GetExeSubsystem_WW                 (ANSI and Unicode)
                      3) GetProcessSubsystem_Error_WW       (ANSI and Unicode)
                      4) XlateExeSubsystem_WW               (ANSI and Unicode)
                      5) XlateExeSubsystem_VB_WW
                      6) GetProcessSubsystem_Error_VB_WW

    2014/05/16 DAG/WW Version 2, 6, 34,1: This change is confined to the header
                                          and the test stand programs.

                      Changes to header:  Add tags for use by DllImportDoc.exe
                                          to generate P/Invoke mapping for
                                          functions GetProcessSubsystem_WW and
                                          GetExeSubsystemW_WW.

                      Changes to Tests:   The ANSI test stand was missing the
                                          tests of GetExeSubsystemA_WW, and the
                                          Unicode test stand was missing a test
                                          of GetProcessSubsystem_WW.

    ============================================================================
*/


#include <windows.h>

#ifndef LIBSPEC_P6CUTILLIB1_API
    #ifdef  _BUILDING_P6CUTILLIB1LIB1
        #define LIBSPEC_P6CUTILLIB1LIB1_API __declspec(dllexport)
    #else
        #define LIBSPEC_P6CUTILLIB1LIB1_API __declspec(dllimport)
    #endif  /* _BUILDING_P6CUTILLIB1LIB1 */
#endif  /* #ifndef  LIBSPEC_P6CUTILLIB1LIB1_API */


//  ----------------------------------------------------------------------------
//  Define standard symbolic constants and macros. CommonConstants_P6C.H also
//  imports windows.h and the core CRT library headers.
//  ----------------------------------------------------------------------------

#include <Const_Typedefs_WW.H>


//  ----------------------------------------------------------------------------
//  incorporate external headers, each of which has an associated module. The
//  routines declared in these headers belong to this library.
//  ----------------------------------------------------------------------------

#include <CharSubst_P6C.H>
#include <ClipLib_P6C.h>
#include <FileExists_WW.H>
#include <FileOpenSaveDialog_P6C.H>
#include <GetComputerName_P6C.H>
#include <GetTempPath_WW.H>
#include <GetVolumeInformation_P6C.H>
#include <GetWinVersionInfo_P6C.H>
#include <ProfileStrings_P6C.H>

//  ----------------------------------------------------------------------------
//  Define result codes returned by the routines declared below.
//  ----------------------------------------------------------------------------

/*  Constants for the File Open and File Save Common Dialog wrapper functions */
#ifndef FOS_SZFILTERS_NULL_ERR_P6C
    #define FOS_SZFILTERS_NULL_ERR_P6C      ( 0x00000011 | APPLICATION_ERROR_MASK )
#endif  /* #ifndef FOS_SZFILTERS_NULL_ERR_P6C */

/*  Constants for the NetShareOps_P6C functions */
#ifndef NS_BUFFER_TO_SMALL_P6C
    #define NS_BUFFER_TO_SMALL_P6C          ( 0x00000031 | APPLICATION_ERROR_MASK )
#endif  /* #ifndef NS_BUFFER_TO_SMALL_P6C */

#ifndef CHAR_SUBST_NULL_PTR_P6C
    #define CHAR_SUBST_NULL_PTR_P6C         ( 0x00000041 | APPLICATION_ERROR_MASK )
#endif  /* #ifndef CHAR_SUBST_NULL_PTR_P6C */

#ifndef CHAR_SUBST_BUFFSIZE_LT_1_P6C
    #define CHAR_SUBST_BUFFSIZE_LT_1_P6C    ( 0x00000042 | APPLICATION_ERROR_MASK )
#endif  /* #ifndef CHAR_SUBST_BUFFSIZE_LT_1_P6C */

#ifndef CHAR_SUBST_CHAR_NOT_FOUND_P6C
    #define CHAR_SUBST_CHAR_NOT_FOUND_P6C   ERROR_SUCCESS
#endif  /* #ifndef CHAR_SUBST_CHAR_NOT_FOUND_P6C */

#ifndef PROF_STR_NO_SECTIONS_P6C
    #define PROF_STR_NO_SECTIONS_P6C        ( 0x00000051 | APPLICATION_ERROR_MASK )
#endif  /* #ifndef PROF_STR_NO_SECTIONS_P6C */

#ifndef GETD_GET_SPECIAL_DIR_WW
    #define GETD_GET_SPECIAL_DIR_WW         ( 0x00000061 | APPLICATION_ERROR_MASK )
#endif  /* #ifndef GETD_GET_SPECIAL_DIR_WW */

#define GETSUBSYST_MALLOC_ERROR_WW          ( 0x00001001 | APPLICATION_ERROR_MASK )
#define GETSUBSYST_NO_MAGIC_WW              ( 0x00001002 | APPLICATION_ERROR_MASK )
#define GETSUBSYST_NO_NT_SIGNATURE_WW       ( 0x00001003 | APPLICATION_ERROR_MASK )
#define GETSUBSYST_NULL_FILENAME_POINTER_WW ( 0x00001004 | APPLICATION_ERROR_MASK )


typedef enum _SUBSYST_STRING_GROUP_WW
{
    SUBSYST_STRING_SHORT ,   // 0
    SUBSYST_STRING_LONG ,    // 1
} SUBSYST_STRING_GROUP_WW ;

typedef const SUBSYST_STRING_GROUP_WW       CSUBSYST_STRING_GROUP_WW ;


//  ----------------------------------------------------------------------------
//  Declare exported functions, all with "C" linkage.
//  ----------------------------------------------------------------------------

#if defined(__cplusplus)
extern "C"
{
#endif

/*
    ============================================================================

    Function Name:      GetOpenFileName_VB_P6C

    Synopsis:           Given a string containing pairs of filter strings
                        suitable for use with the common dialog functions, open
                        or create the specified file.

    Note:               The caller is responsible for supplying a correctly
                        formatted set of file name masks. Please see notes about
                        private function FilterStringFixup_P6C for details.

    Arguments:          phwnd           = Handle to the module that calls the
                                          function.

                        phInstance      = Instance handle, used to retrieve
                                          resources, such as dialog templates.
                                          Although this function usse no such
                                          resources, the Common Dialog library
                                          expects one, so we oblige it.

                        pszFilters      = An array of null terminated strings,
                                          terminated by a double null OR a pipe
                                          delimited string, with an optional
                                          terminal pipe character.

                                          Odd numbered strings are text to
                                          display in the file mask combo box and
                                          even numbered strings are the
                                          corresponding file name masks.

                        pstrInitialDir  = Path of initial directory to display.
                                          If null, platform dependent default
                                          rules apply.

                        pstrDefExt      = String containing a valid extension of
                                          no more than three characters. Periods
                                          are disallowed and will be stripped.
                                          This argument may be NULL.

                        pstrTitle       = Caption to display in dialog box.
                                          If null, platform dependent default
                                          rules apply.

                        pdwFlags        = DWORD bit mask of flags that govern
                                          behavior of the control. By default,
                                          the flags value is zero.

                                          Per Microsoft Platform SDK C header
                                          file CommDlg.h, the following flags
                                          are defined.

                                          OFN_READONLY             0x00000001
                                          OFN_OVERWRITEPROMPT      0x00000002
                                          OFN_HIDEREADONLY         0x00000004
                                          OFN_NOCHANGEDIR          0x00000008
                                          OFN_SHOWHELP             0x00000010
                                          OFN_ENABLEHOOK           0x00000020
                                          OFN_ENABLETEMPLATE       0x00000040
                                          OFN_ENABLETEMPLATEHANDLE 0x00000080
                                          OFN_NOVALIDATE           0x00000100
                                          OFN_ALLOWMULTISELECT     0x00000200
                                          OFN_EXTENSIONDIFFERENT   0x00000400
                                          OFN_PATHMUSTEXIST        0x00000800
                                          OFN_FILEMUSTEXIST        0x00001000
                                          OFN_CREATEPROMPT         0x00002000
                                          OFN_SHAREAWARE           0x00004000
                                          OFN_NOREADONLYRETURN     0x00008000
                                          OFN_NOTESTFILECREATE     0x00010000
                                          OFN_NONETWORKBUTTON      0x00020000
                                          OFN_NOLONGNAMES          0x00040000
                                                                   // force no
                                                                   long names
                                                                   for 4.x
                                                                   modules
                                          #if(WINVER >= 0x0400)
                                          OFN_EXPLORER             0x00080000
                                                                   // new look
                                                                   commdlg
                                          OFN_NODEREFERENCELINKS   0x00100000
                                          OFN_LONGNAMES            0x00200000
                                                                   // force long
                                                                   names for 3.x
                                                                   modules
                                          OFN_ENABLEINCLUDENOTIFY  0x00400000
                                                                   // send in-
                                                                   clude message
                                                                   to callback
                                          OFN_ENABLESIZING         0x00800000

                        pdwFlagsEx      = Long integer (DWORD) bit mask of
                                          extended flags that is copied to the
                                          FlagsEx member of the OPENFILENAME
                                          structure. The olny documented flag is
                                          OFN_EX_NOPLACESBAR, which causes the
                                          Places Bar to be suppressed.

    Returns:            strGetOFN_P6c   = String containing fully qualified name
                                          of selected file or files.

                                          If an error occurs, a null string is
                                          returned and library variable
                                          m_dwLastError_P6LIB1, defined in
                                          module Main.C, is set to the value
                                          returned by CommDlgExtendedError.

                                          Call GetLastError to get detailed
                                          error information.

    Remarks:            1) Function GetOpenFileName_VB_P6C is a wrapper for
                           companion function GetOpenFileName_P6C. It differs in
                           that it returns a BSTR, which is allocated from the
                           global address space and becomes the property of the
                           caller.

                        2) See http://msdn.microsoft.com/library/en-us/winui/
                           winui/windowsuserinterface/userinput/
                           commondialogboxlibrary/commondialogboxreference/
                           commondialogboxfunctions/GetOpenFileName.asp (VERY
                           LONG URL) for the official definiation of the
                           GetOpenFileName Windows API function for which this
                           function is a VB/VBA wrapper.

                        3) This is a port of function strGetOFN_P6c,
                           from VB/VBA library module mCommonDialogWrappers_P6C,
                           Copyright 2004, David A. Gray, Fort Worth, Texas.

    References:         http://msdn.microsoft.com/library/en-us/winui/winui/
                        windowsuserinterface/userinput/commondialogboxlibrary/
                        commondialogboxreference/commondialogboxfunctions/
                        GetOpenFileName.asp defines the GetOpenFileName Windows
                        API function for which this is a C wrapper.

    Calls:              GetOpenFileName from library comdlg32.dll.

    Date Written:       Sunday, 12 June 2005.

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version      Author Synopsis
    ---------- ------------ ------ ----------------------------------------------
    2005/08/19 1, 0,  0, 4  DAG/WW First working version.

    2005/10/12 1, 0,  0, 8  DAG/WW Add argument pdwFlagsEx.

    2006/11/03 1, 0,  0, 11 DAG/WW Remove the unused HINSTANCE argument, which
                                   is supplied internally from this DLL.

    ============================================================================
*/

BSTR __stdcall GetOpenFileName_VB_P6C
(
    CHWND   phwnd ,
    LPCTSTR pszFilters ,
    LPCTSTR pstrInitialDir ,
    LPCTSTR pstrDefExt ,
    LPCTSTR pstrTitle ,
    CDWORD  pdwFlags ,
    CDWORD  pdwFlagsEx
) ;


/*
    ============================================================================

    Function Name:      GetSaveFileName_VB_P6C

    Synopsis:           Given a string containing pairs of filter strings
                        suitable for use with the common dialog functions, open
                        or create the specified file.

    IMPORTANT:          The caller is responsible for supplying a correctly
                        formatted set of file name masks. Please see notes about
                        private function FilterStringFixup_P6C for details.

    Arguments:          phwnd           = Handle to the module that calls the
                                          function.

                        pszFilters      = An array of null terminated strings,
                                          terminated by a double null OR a pipe
                                          delimited string, with an optional
                                          terminal pipe character.

                                          Odd numbered strings are text to
                                          display in the file mask combo box and
                                          even numbered strings are the
                                          corresponding file name masks.

                        pstrInitialDir  = Path of initial directory to display.
                                          If null, platform dependent default
                                          rules apply.

                        pstrDefExt      = String containing a valid extension of
                                          no more than three characters. Periods
                                          are disallowed and will be stripped.
                                          This argument may be NULL.

                        pstrTitle       = Caption to display in dialog box.
                                          If null, platform dependent default
                                          rules apply.

                        pdwFlags        = Long integer bit mask of flags that
                                          govern behavior of the control.
                                          By default, the flags value is zero.

                        pdwFlags        = Long integer bit mask of flags that
                                          govern behavior of the control.
                                          By default, the flags value is zero.

    Returns:            strGetOFN_P6c   = String containing fully qualified name
                                          of selected file or files.

                                          If an error occurs, a null string is
                                          returned and library variable
                                          m_dwLastError_P6LIB1, defined in
                                          module Main.C, is set to the value
                                          returned by CommDlgExtendedError.

                                          Call function GetLastError to get
                                          detailed error information.

    Remarks:            1) Function GetSaveFileName_VB_P6C is a wrapper for
                           companion function GetSaveFileName_P6C. It differs in
                           that it returns a BSTR, which is allocated from the
                           global address space and becomes the property of the
                           caller.

                        2) See http://msdn.microsoft.com/library/en-us/winui/
                           winui/windowsuserinterface/userinput/
                           commondialogboxlibrary/commondialogboxreference/
                           commondialogboxfunctions/GetSaveFileName.asp (VERY
                           LONG URL) for the official definiation of the
                           GetOpenFileName Windows API function for which this
                           function is a VB/VBA wrapper.

                        3) This function was created by adapting my existing C
                           function GetOpenFileName_P6C, which is defined above
                           in this same source file.

    References:         http://msdn.microsoft.com/library/en-us/winui/winui/
                        windowsuserinterface/userinput/commondialogboxlibrary/
                        commondialogboxreference/commondialogboxfunctions/
                        GetOpenFileName.asp defines the GetOpenFileName Windows
                        API function for which this is a C wrapper.

    Calls:              GetOpenFileName from library comdlg32.dll.

    Date Written:       Tuesday, 16 August 2005.

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version     Author Synopsis
    ---------- ----------- ------ -----------------------------------------------
    2005/08/19 1, 0, 0,  4 DAG/WW First working version.

    2005/10/12 1, 0, 0,  8 DAG/WW Add argument pdwFlagsEx.

    2006/11/03 1, 0, 0, 11 DAG/WW Remove the unused HINSTANCE argument, which is
                                  supplied internally from this DLL.

    ============================================================================
*/

BSTR __stdcall GetSaveFileName_VB_P6C
(
    CHWND   phwnd ,
    LPCTSTR pszFilters ,
    LPCTSTR pstrInitialDir ,
    LPCTSTR pstrDefExt ,
    LPCTSTR pstrTitle ,
    CDWORD  pdwFlags ,
    CDWORD  pdwFlagsEx
) ;


/*
    ============================================================================

    Function Name:      GetTempPath_VB_WW

    Synopsis:           Get the name of the user scratch (temp) directory.

    Arguments:          None.

    Returns:            String containing the name of the user's scratch
                        (temporary) directory. The return value points to a BSTR
                        (Basic String) which becomes the property of the caller.

                        If the calling program is implemented in C, C++, or
                        Assembly, the caller must string discard it by calling
                        SysFreeString.

                        If the calling program is implemented in some version of
                        Visual Basic, the garbage collector disposes of it.

    Remarks:            WARNING: You should terminate immediately if this
                                 function returns NULL, although you may be able
                                 to coax an error message from Windows, by
                                 calling GetLastError, EM_ShowMsgOnConsoleW_WW,
                                 for console applications, or
                                 EM_FormatMsgForMsgbox_WW, for Windows
                                 applications.

    References:         GetTempPath Function, in the MSDN library, at
                        http://msdn.microsoft.com/en-us/library/aa364992(VS.85).aspx
    ============================================================================
*/

BSTR __stdcall GetTempPath_VB_WW ( void ) ;


/*
    ============================================================================

    Function Name:      CB_PasteByID_VB_P6C

    Synopsis:           Return a long pointer to a string that contains a local
                        copy of the test on the Windows clipboard.

    Arguments:          pMyHwnd         = Handle to the module that calls the
                                          function.

                        plngCBFormatID  = Long integer containing a registered
                                          Clipboard format ID.

    Returns:            rlpszText       = Long pointer to string containing text
                                          copied from the Windows Clipboard. If
                                          the clipboard is empty, an empty
                                          string is returned. If an error
                                          occurs, a NULL pointer is returned. If
                                          so, call GetLastError to learn why.

    Remarks:            1) Function CB_PasteByID_VB_P6C is a wrapper for
                           companion function CB_PasteText_P6C. It differs in
                           that it returns a BSTR, which is allocated from the
                           global address space and becomes the property of the
                           caller.

                        2) This function handles only ASCIIZ strings.

                        3) If the clipboard is empty, a pointer to an empty
                           string is returned.

    References:         Please see the module flower box above.

    Date Written:       Sunday, 12 June 2005.

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version    Author Synopsis
    ---------- ---------- ------ -----------------------------------------------
    2005/10/13 1, 0, 0, 8 DAG/WW First working version.
    ============================================================================
*/

BSTR __stdcall CB_PasteByID_VB_P6C
(
    CHWND phwnd ,
    CLONG plngCBFormatID
) ;


/*
    ============================================================================

    Function Name:      CB_PasteByName_VB_P6C

    Synopsis:           Return a long pointer to a string that contains a local
                        copy of the test on the Windows clipboard.

    Arguments:          pMyHwnd          = Handle to the module that calls the
                                           function.

                        pstrCBFormatName = String containing the expected
                                           Clipboard Format Name.

    Returns:            rlpszText        = Long pointer to string containing
                                           text copied from the Windows
                                           Clipboard. Ifthe clipboard is empty,
                                           an empty string is returned. If an
                                           error occurs, a NULL pointer is
                                           returned. If so, call GetLastError to
                                           learn why.

    Remarks:            1) Function CB_PasteByName_VB_P6C is a wrapper for
                           companion function CB_PasteByID_P6C. It differs in
                           that it returns a BSTR, which is allocated from the
                           global address space and becomes the property of the
                           caller.

                        2) This function handles only ASCIIZ strings.

                        3) If the clipboard is empty, a pointer to an empty
                           string is returned.

    References:         Please see the module flower box above.

    Calls:              The following Windows API functions are called in the
                        order named.

                             Function         Library      Header
                             ---------------- ------------ ---------
                             strchr           C Run-Time   string.h
                             strlen           C Run-Time   string.h
                             ----------------------------------------

    Date Written:       Tuesday, 16 August 2005 - Wednesday, 17 August 2005.

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version    Author Synopsis
    ---------- ---------- ------ -----------------------------------------------
    2005/08/28 1, 0, 0, 4 DAG/WW First working version.
    ============================================================================
*/

BSTR __stdcall CB_PasteByName_VB_P6C
(
    CHWND   pMyHwnd ,
    LPCTSTR pstrCBFormatName
) ;


/*
    ============================================================================

    Function Name:      CB_PasteText_VB_P6C

    Synopsis:           Return a long pointer to a string that contains a local
                        copy of the text on the Windows clipboard.

    Arguments:          pMyHwnd         = Handle to the module that calls the
                                          function.

    Returns:            rlpszText       = Long pointer to string containing text
                                          copied from the Windows Clipboard. If
                                          the clipboard is empty, an empty
                                          string is returned. If an error
                                          occurs, a NULL pointer is returned. If
                                          so, call GetLastError to learn why.

    Remarks:            1) Function CB_PasteText_VB_P6C is a wrapper for
                           companion function CB_PasteText_P6C. It differs in
                           that it returns a BSTR, which is allocated from the
                           global address space and becomes the property of the
                           caller.

                        2) This function handles only ASCIIZ strings.

                        3) If the clipboard is empty, a pointer to an empty
                           string is returned.

    References:         Please see the module flower box above.


    Date Written:       Sunday, 12 June 2005.

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version    Author Synopsis
    ---------- ---------- ------ -----------------------------------------------
    2005/08/19 1, 0, 0, 4 DAG/WW First working version.
    ============================================================================
*/

BSTR __stdcall CB_PasteText_VB_P6C
(
    CHWND phwnd
) ;


/*
    ============================================================================

    Function Name:      DirWindows_WW_VB

    Synopsis:           Provide a faster, more efficient means of getting the
                        name of the Windows directory, for use by Visual Basic
                        and Visual Basic for Applications code than the
                        roundabout way, through COM, that seems to be the only
                        other practical option.

    Arguments:          None.

    Returns:            The return value is a BSTR (Basic String) that contains
                        the name of the Windows directory.

    Remarks:            This is a VB/VBA wrapper for the DirWindows_WW function,
                        which returns a buffer, allocated from the process heap,
                        which is just big enough to hold the string.

    References:         None.

    Calls:              DirWindows_WW, declared elsewhere in this header
                        SysAllocString, declared in the Windows Platform SDK

    See Also:           DirWindowsSystem_WW_VB
                        DirWindowsSystem_WW

    Date Completed:     Sunday, 31 October 2010.

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Author Synopsis
    ---------- ------ ----------------------------------------------------------
    2010/11/01 DAG/WW First appearance of this function.
    ============================================================================
*/

BSTR    __stdcall                      DirWindows_WW_VB ( void ) ;

LPTSTR  LIBSPEC_P6CUTILLIB1LIB1_API    DirWindowsA_WW ( void ) ;
LPTSTR  LIBSPEC_P6CUTILLIB1LIB1_API    DirWindowsW_WW ( void ) ;

#ifdef  UNICODE
    #define DirWindows_WW   DirWindowsW_WW
#else
    #define DirWindows_WW   DirWindowsA_WW
#endif  /*  #ifdef  UNICODE */


/*
    ============================================================================

    Function Name:      DirWindowsSystem_WW_VB

    Synopsis:           Provide a faster, more efficient means of getting the
                        name of the Windows System directory, for use by Visual
                        Basic and Visual Basic for Applications code than the
                        roundabout way, through COM, that seems to be the only
                        other practical option.

    Arguments:          None.

    Returns:            The return value is a BSTR (Basic String) that contains
                        the name of the Windows System directory.

    Remarks:            This is a VB/VBA wrapper for the DirWindowsSystem_WW
                        function, which returns a buffer, allocated from the
                        process heap, which is just big enough to hold the
                        string.

    References:         None.

    Calls:              DirWindowsSystem_WW, declared elsewhere in this header
                        SysAllocString, declared in the Windows Platform SDK

    See Also:           DirWindows_WW_VB
                        DirWindows_WW

    Date Completed:     Sunday, 31 October 2010.

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Author Synopsis
    ---------- ------ ----------------------------------------------------------
    2010/11/01 DAG/WW First appearance of this function.
    ============================================================================
*/

BSTR    __stdcall                      DirWindowsSystem_WW_VB ( void ) ;

LPTSTR  LIBSPEC_P6CUTILLIB1LIB1_API    DirWindowsSystemA_WW ( void ) ;
LPTSTR  LIBSPEC_P6CUTILLIB1LIB1_API    DirWindowsSystemW_WW ( void ) ;

#ifdef  UNICODE
    #define DirWindowsSystem_WW DirWindowsSystemW_WW
#else
    #define DirWindowsSystem_WW DirWindowsSystemA_WW
#endif  /*  #ifdef  UNICODE */


/*
    ============================================================================

    Function Name:      NetShareEnum_P6C

    Synopsis:           Wrapper for the NetShareEnum Windows Lan Manager API
                        function used to enumerate all shares.

    Arguments:          LPCTSTR plpServerName   = [in] Long pointer to string
                                                  containing name of server on
                                                  which the function should run.
                                                  If this is NULL, use the local
                                                  machine.

    Returns:            lpShareInfo             = Long pointer to TAB delimited
                                                  block of textual information
                                                  about the shares.

                                                  The following three items are
                                                  reported for each share.

                                                  Share Name    = Untruncated
                                                                  share name.
                                                  Type          = Numeric code,
                                                                  as a 32 bit
                                                                  hexadecimal
                                                                  value.
                                                  Remark        = Untruncated
                                                                  text.

    Remarks:            The API documentation says the following about the final
                        bufptr argument.

                            [out] Pointer to the buffer that receives the data.
                            The format of this data depends on the value of the
                            level parameter.

                            Windows NT/2000/XP: This buffer is allocated by the
                            system and must be freed using the NetApiBufferFree
                            function. Note that you must free the buffer even if
                            the function fails with ERROR_MORE_DATA.

                        The above information is provided primarily for internal
                        use, as the library looks after this buffer and disposes
                        of it before it returns.

    References:         http://msdn.microsoft.com/library/library/en-us/stgmgmt/fs/netshareenum.asp

    Date Written:       Thursday, 18 August through Friday, 19 August 2005.

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version  Author Synopsis
    ---------- -------- ------ -------------------------------------------------
    2005/08/19 1,0,0,2  DAG/WW First working version.
    ============================================================================
*/

LPTSTR LIBSPEC_P6CUTILLIB1LIB1_API NetShareEnum_P6C
(
    LPCTSTR plpServerName
) ;


/*
    ============================================================================

    Function Name:      NetShareEnum_VB_P6C

    Synopsis:           VB/VBA wrapper for the NetShareEnum_P6C function, which
                        is, itself, a wrapper for the NetShareEnum Windows Lan
                        Manager API function, used to enumerate all shares.

    Arguments:          LPCTSTR plpServerName   = [in] Long pointer to string
                                                  containing name of server on
                                                  which the function should run.
                                                  If this is NULL, use the local
                                                  machine.

    Returns:            bstrShareInfo           = A BSTR containing the TAB
                                                  delimited block of textual
                                                  information about the shares.

                                                  The following three items are
                                                  reported for each share.

                                                  Share Name    = Untruncated
                                                                  share name.

                                                  Type          = Numeric code,
                                                                  as a 32 bit
                                                                  hexadecimal
                                                                  value.

                                                  Remark        = Untruncated
                                                                  text.

    Remarks:            Please see the documentation for NetShareGetInfo_P6C for
                        additional details about the inner workings of this
                        function - if you are insatiably curious. Everything you
                        really need to know is here.

    Date Written:       Friday, 19 August 2005.

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version    Author Synopsis
    ---------- ---------- ------ -----------------------------------------------
    2005/08/19 1, 0, 0, 2 DAG/WW First working version.
    ============================================================================
*/

BSTR __stdcall NetShareEnum_VB_P6C
(
    LPCTSTR plpServerName
) ;


/*
    ============================================================================

    Function Name:      ShellExecute_P6C

    Synopsis:           Wrapper for the ShellExecute Windows API function.

    Arguments:          NOTE: For the benefit of our readers who are working
                              without access to the Microsoft Platform SDK, I
                              have reproduced the complete definitions of the
                              arguments, all of which are passed, unlatered, to
                              the ShellExecute function.

                              All references to "lpFile" should be interpreted
                              as applying to "plpFile."

                        HWND phwnd              = [in] Handle to the owner
                                                  window used for displaying a
                                                  user interface (UI) or error
                                                  messages. This value can be
                                                  NULL if the operation is not
                                                  associated with a window.

                                                  NOTE: If phwnd is NULL (zero)
                                                        and an error occurs, the
                                                        error is returned but
                                                        the message box is sup-
                                                        pressed.

                        LPCTSTR plpOperation    = [in] Pointer to a null-termi-
                                                  nated string, referred to in
                                                  this case as a verb, that
                                                  specifies the action to be
                                                  performed. The set of
                                                  available verbs depends on the
                                                  particular file or folder.
                                                  Generally, the actions
                                                  available from an object s
                                                  shortcut menu are available
                                                  verbs. For more information
                                                  about verbs and their avail-
                                                  bility, see Object Verbs. See
                                                  Extending Shortcut Menus for
                                                  further discussion of shortcut
                                                  menus. The following verbs are
                                                  commonly used.

                                                  edit      = Launches an editor
                                                              and opens the
                                                              document for
                                                              editing. If lpFile
                                                              is not a document
                                                              file, the function
                                                              will fail.

                                                  explore   = Explores the
                                                              folder specified
                                                              by lpFile.

                                                  find      = Initiates a search
                                                              starting from the
                                                              specified direc-
                                                              tory.

                                                  open      = Opens the file
                                                              specified by the
                                                              lpFile parameter.
                                                              The file can be an
                                                              executable file, a
                                                              document file, or
                                                              a folder.

                                                  print     = Prints the file
                                                              specified by
                                                              lpFile. If lpFile
                                                              is not a document
                                                              file, the function
                                                              will fail.

                                                  NULL      = For systems prior
                                                              to Microsoft Win-
                                                              dows 2000, the
                                                              default verb is
                                                              used if it is va-
                                                              lid and available
                                                              in the registry.
                                                              If not, the "open"
                                                              verb is used.

                                                              For Windows 2000
                                                              and later systems,
                                                              the default verb
                                                              is used if avail-
                                                              able. If not, the
                                                              "open" verb is
                                                              used. If neither
                                                              verb is available,
                                                              the system uses
                                                              the first verb
                                                              listed in the
                                                              registry.

                                                  For the benefit of our readers
                                                  who are working without access
                                                  to the Microsoft Platform SDK,
                                                  here are definitions of the
                                                  above constants, taken from C
                                                  header file winuser.h. All
                                                  numbers are decimal.

                                                  #define SW_HIDE              0
                                                  #define SW_SHOWNORMAL        1
                                                  #define SW_NORMAL            1
                                                  #define SW_SHOWMINIMIZED     2
                                                  #define SW_SHOWMAXIMIZED     3
                                                  #define SW_MAXIMIZE          3
                                                  #define SW_SHOWNOACTIVATE    4
                                                  #define SW_SHOW              5
                                                  #define SW_MINIMIZE          6
                                                  #define SW_SHOWMINNOACTIVE   7
                                                  #define SW_SHOWNA            8
                                                  #define SW_RESTORE           9
                                                  #define SW_SHOWDEFAULT      10
                                                  #define SW_FORCEMINIMIZE    11
                                                  #define SW_MAX              11

                       LPCTSTR plpFile          = [in] Pointer to a null-termi-
                                                  nated string that specifies
                                                  the file or object on which to
                                                  execute the specified verb. To
                                                  specify a Shell namespace
                                                  object, pass the fully quali-
                                                  fied parse name. Note that not
                                                  all verbs are supported on all
                                                  objects. For example, not all
                                                  document types support the
                                                  "print" verb.

                       LPCTSTR plpParameters    = [in] If the lpFile parameter
                                                  specifies an executable file,
                                                  lpParameters is a pointer to a
                                                  null-terminated string that
                                                  specifies the parameters to be
                                                  passed to the application. The
                                                  format of this string is
                                                  determined by the verb that is
                                                  to be invoked. If lpFile
                                                  specifies a document file,
                                                  lpParameters should be NULL.

                       LPCTSTR plpDirectory = [in] Pointer to a null-termi-
                                                  nated string that specifies
                                                  the default directory.

                       INT     pnShowCmd        = [in] Flags that specify how an
                                                  application is to be displayed
                                                  when it is opened. If lpFile
                                                  specifies a document file, the
                                                  flag is simply passed to the
                                                  associated application. It is
                                                  up to the application to
                                                  decide how to handle it.

                                                  SW_HIDE            = Hides the
                                                                       window
                                                                       and acti-
                                                                       vates
                                                                       another
                                                                       window.

                                                  SW_MAXIMIZE        = Maximizes
                                                                       the spe-
                                                                       cified
                                                                       window.

                                                  SW_MINIMIZE        = Minimizes
                                                                       the spe-
                                                                       cified
                                                                       window
                                                                       and acti-
                                                                       vates the
                                                                       next top-
                                                                       level
                                                                       window in
                                                                       the z-or-
                                                                       der.

                                                  SW_RESTORE         = Activates
                                                                       and dis-
                                                                       plays the
                                                                       window.
                                                                       If the
                                                                       window is
                                                                       minimized
                                                                       or maxi-
                                                                       mized,
                                                                       Windows
                                                                       restores
                                                                       it to its
                                                                       original
                                                                       size and
                                                                       position.
                                                                       An appli-
                                                                       cation
                                                                       should
                                                                       specify
                                                                       this flag
                                                                       when re-
                                                                       storing a
                                                                       minimized
                                                                       window.

                                                  SW_SHOW            = Activates
                                                                       the win-
                                                                       dow and
                                                                       displays
                                                                       it in its
                                                                       current
                                                                       size and
                                                                       position.

                                                  SW_SHOWDEFAULT     = Sets the
                                                                       show
                                                                       state
                                                                       based on
                                                                       the SW_
                                                                       flag spe-
                                                                       cified in
                                                                       the
                                                                       STARTUP-
                                                                       INFO
                                                                       structure
                                                                       passed to
                                                                       the
                                                                       Create-
                                                                       Process
                                                                       function
                                                                       by the
                                                                       program
                                                                       that
                                                                       started
                                                                       the
                                                                       appli-
                                                                       cation.
                                                                       An appli-
                                                                       cation
                                                                       should
                                                                       call
                                                                       Show-
                                                                       Window
                                                                       with this
                                                                       flag to
                                                                       set the
                                                                       initial
                                                                       show
                                                                       state of
                                                                       its main
                                                                       window.

                                                  SW_SHOWMAXIMIZED   = Activates
                                                                       the win-
                                                                       dow and
                                                                       displays
                                                                       it as a
                                                                       maximized
                                                                       window.

                                                  SW_SHOWMINIMIZED   = Activates
                                                                       the win-
                                                                       dow and
                                                                       displays
                                                                       it as a
                                                                       minimized
                                                                       window.

                                                  SW_SHOWMINNOACTIVE = Displays
                                                                       the win-
                                                                       dow as a
                                                                       minimized
                                                                       window.
                                                                       The ac-
                                                                       tive win-
                                                                       dow re-
                                                                       mains
                                                                       active.

                                                  SW_SHOWNA          = Displays
                                                                       the win-
                                                                       dow in
                                                                       its cur-
                                                                       rent
                                                                       state.
                                                                       The ac-
                                                                       tive win-
                                                                       dow re-
                                                                       mains ac-
                                                                       tive.

                                                  SW_SHOWNOACTIVATE  = Displays
                                                                       a window
                                                                       in its
                                                                       most re-
                                                                       cent size
                                                                       and po-
                                                                       sition.
                                                                       The ac-
                                                                       tive win-
                                                                       dow re-
                                                                       mains ac-
                                                                       tive.

                                                  SW_SHOWNORMAL      = Activates
                                                                       and dis-
                                                                       plays a
                                                                       window.
                                                                       If the
                                                                       window is
                                                                       minimized
                                                                       or maxi-
                                                                       mized,
                                                                       Windows
                                                                       restores
                                                                       it to its
                                                                       original
                                                                       size and
                                                                       position.
                                                                       An appli-
                                                                       cation
                                                                       should
                                                                       specify
                                                                       this flag
                                                                       when dis-
                                                                       playing
                                                                       the win-
                                                                       dow for
                                                                       the first
                                                                       time.

    Returns:            rhInstance     = If the function succeeds, the return
                                         value is the instance handle
                                         (HINSTANCE) of the new process.

                                         If the function fails, the return
                                         value is less than 32, and it maps to
                                         a reason code.

    Remarks:            None.

    References:         http://msdn.microsoft.com/library/en-us/shellcc/platform/shell/reference/functions/shellexecute.asp

    Date Written:       Tuesday, 16 August 2005.

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version  Author Synopsis
    ---------- -------- ------ -------------------------------------------------
    2005/08/19 1,0,0,2  DAG/WW First working version.
    2011/09/24 2,3,26,0 DAG/WW Add a Unicode implementation.
    ============================================================================
*/

HINSTANCE  LIBSPEC_P6CUTILLIB1LIB1_API ShellExecuteA_P6C
(
    HWND    phwnd ,
    LPCTSTR plpOperation ,
    LPCTSTR plpFile ,
    LPCTSTR plpParameters ,
    LPCTSTR plpDirectory ,
    CINT    pnShowCmd
) ;

HINSTANCE  LIBSPEC_P6CUTILLIB1LIB1_API ShellExecuteW_P6C
(
    HWND    phwnd ,
    LPCTSTR plpOperation ,
    LPCTSTR plpFile ,
    LPCTSTR plpParameters ,
    LPCTSTR plpDirectory ,
    CINT    pnShowCmd
) ;

#ifdef UNICODE
    #define ShellExecute_P6C    ShellExecuteW_P6C
#else
    #define ShellExecute_P6C    ShellExecuteA_P6C
#endif  /*  #ifdef UNICODE */


/*
    ============================================================================

    Function Name:      GetExeSubsystem_WW

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

    ============================================================================
*/

/*************************************************************************************************************
@func:  GetExeSubsystemW_WW | Get subsystem ID of the specifed Portable Executable.
@param: WCHAR* plpMMainModuleFQFN | Null terminated string containing name of Portable Executable file to test
@return:    DWORD | Windows subsystem in which the calling process is running
@note:  The return value is typlically 2 or 3.
@see:   GetProcessSubsystem_Error_WW XlateExeSubsystem_WW
@area:  Operating Environment
*************************************************************************************************************/


DWORD LIBSPEC_P6CUTILLIB1LIB1_API GetExeSubsystemA_WW ( LPCTSTR plpMMainModuleFQFN ) ;
DWORD LIBSPEC_P6CUTILLIB1LIB1_API GetExeSubsystemW_WW ( LPCTSTR plpMMainModuleFQFN ) ;

#ifdef  UNICODE
    #define GetExeSubsystem_WW GetExeSubsystemW_WW
#else
    #define GetExeSubsystem_WW GetExeSubsystemA_WW
#endif  /*  #ifdef  UNICODE */


/*
    ============================================================================

    Function Name:      GetProcessSubsystem_WW

    Synopsis:           Determine whether a process is running in the character
                        mode or graphical subsystem.

    Arguments:          None.

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

    Date Completed:     Monday, 07 April 2014

    ============================================================================
*/

/***********************************************************************************
@func:  GetProcessSubsystem_WW | Get subsystem ID of the current process.
@return:    DWORD | Windows subsystem in which the calling process is running
@note:  The return value is typlically 2 or 3.
@see:   GetProcessSubsystem_Error_WW XlateExeSubsystem_WW
@area:  Operating Environment
**********************************************************************************/

DWORD LIBSPEC_P6CUTILLIB1LIB1_API GetProcessSubsystem_WW ( ) ;


/*
    ============================================================================

    Function Name:      GetProcessSubsystem_Error_WW

    Synopsis:           Use this routine to get a nice human-readable message to
                        display on the console or write in the Windows event log
                        in the unlikely event that GetProcessSubsystem_WW or
                        GetExeSubsystem_WW returns IMAGE_SUBSYSTEM_UNKNOWN.

    Arguments:          None.

    Returns:            If the function succeeds, its return value is a LPTSTR
                        (Long Pointer To STRing), suitable for display in a
                        message box or the console, or recording in a log file
                        or a Windows event log.

    Remarks:            You must call this routine right away, because any of a
                        number of routines that are beyond our control can reset
                        the status code.

                        IMAGE_SUBSYSTEM_UNKNOWN, the value returned to indicate
                        an error, is defined in Microsoft Windows Platform SDK
                        header WinNT.H, and has a value of zero. Though failure
                        is pretty unlikely, I have covered every case that I can
                        imagine.

    References:         None.

    Date Completed:     Monday, 07 April 2014

    ============================================================================
*/

LPTSTR LIBSPEC_P6CUTILLIB1LIB1_API GetProcessSubsystem_ErrorA_WW ( ) ;
LPTSTR LIBSPEC_P6CUTILLIB1LIB1_API GetProcessSubsystem_ErrorW_WW ( ) ;

#ifdef  UNICODE
    #define GetProcessSubsystem_Error_WW GetProcessSubsystem_ErrorW_WW
#else
    #define GetProcessSubsystem_Error_WW GetProcessSubsystem_ErrorA_WW
#endif  /*  #ifdef  UNICODE */


/*
    ============================================================================

    Function Name:      GetProcessSubsystem_Error_VB_WW

    Synopsis:           Use this routine to get a nice human-readable message to
                        display on the console or write in the Windows event log
                        in the unlikely event that GetProcessSubsystem_WW or
                        GetExeSubsystem_WW returns IMAGE_SUBSYSTEM_UNKNOWN.

    Arguments:          None.

    Returns:            If the function succeeds, its return value is a BSTR
                        (Basic STRing), suitable for display in a message box or
                        recording in a log file or a Windows event log.

    Remarks:            You must call this routine right away, because any of a
                        number of routines that are beyond our control can reset
                        the status code.

                        IMAGE_SUBSYSTEM_UNKNOWN, the value returned to indicate
                        an error, is defined in Microsoft Windows Platform SDK
                        header WinNT.H, and has a value of zero. Though failure
                        is pretty unlikely, I have covered every case that I can
                        imagine. The following Visual Basic statement defines
                        its value as a constant.

                            Const IMAGE_SUBSYSTEM_UNKNOWN as Integer = 0

    References:         None.

    Date Completed:     Monday, 07 April 2014

    ============================================================================
*/

BSTR   __stdcall                   GetProcessSubsystem_Error_VB_WW ( ) ;


/*
    ============================================================================

    Function Name:      XlateExeSubsystem_WW

    Synopsis:           Use this routine to get a nice human-readable string to
                        display on the console or write in the Windows event log
                        to go with the subsystem ID that GetExeSubsystem_WW or
                        GetProcessSubsystem_WW returns.

    Arguments:          pdwSubsystem        = Return value of GetExeSubsystem_WW
                                              or GetProcessSubsystem_WW.

                        penmStringGroup     = Member of SUBSYST_STRING_GROUP_WW
                                              enumeration, specifies whether to
                                              return the abbreviated subsystem
                                              name or the longer descriptive
                                              message.

    Returns:            If the function succeeds, its return value is a LPTSTR
                        (Long Pointer To STRing), suitable for display in a
                        message box or the console, or recording in a log file
                        or a Windows event log.

    Remarks:            IMAGE_SUBSYSTEM_UNKNOWN, the value returned to indicate
                        an error, is defined in Microsoft Windows Platform SDK
                        header WinNT.H, and has a value of zero.

                        Invalid pdwSubsystem values fed into this routine are
                        treated as IMAGE_SUBSYSTEM_UNKNOWN.

                        Invalid SUBSYST_STRING_GROUP_WW values are treated as
                        SUBSYST_STRING_SHORT.

    References:         None.

    Date Completed:     Tuesday, 08 April 2014

    ============================================================================
*/


LPTSTR LIBSPEC_P6CUTILLIB1LIB1_API XlateExeSubsystemA_WW
(
    CDWORD                   pdwSubsystem ,
    CSUBSYST_STRING_GROUP_WW penmStringGroup
) ;

LPTSTR LIBSPEC_P6CUTILLIB1LIB1_API XlateExeSubsystemW_WW
(
    CDWORD                   pdwSubsystem ,
    CSUBSYST_STRING_GROUP_WW penmStringGroup
) ;

#ifdef  UNICODE
    #define XlateExeSubsystem_WW XlateExeSubsystemW_WW
#else
    #define XlateExeSubsystem_WW XlateExeSubsystemA_WW
#endif  /*  #ifdef  UNICODE */


/*
    ============================================================================

    Function Name:      XlateExeSubsystem_VB_WW

    Synopsis:           Use this routine to get a nice human-readable string to
                        display on the console or write in the Windows event log
                        to go with the subsystem ID that GetExeSubsystem_WW or
                        GetProcessSubsystem_WW returns.

    Arguments:          pdwSubsystem        = Return value of GetExeSubsystem_WW
                                              or GetProcessSubsystem_WW.

                        penmStringGroup     = Member of SUBSYST_STRING_GROUP_WW
                                              enumeration, specifies whether to
                                              return the abbreviated subsystem
                                              name or the longer descriptive
                                              message.

    Returns:            If the function succeeds, its return value is a LPTSTR
                        (Long Pointer To STRing), suitable for display in a
                        message box or the console, or recording in a log file
                        or a Windows event log.

    Remarks:            IMAGE_SUBSYSTEM_UNKNOWN, the value returned to indicate
                        an error, is defined in Microsoft Windows Platform SDK
                        header WinNT.H, and has a value of zero.

                        Invalid pdwSubsystem values fed into this routine are
                        treated as IMAGE_SUBSYSTEM_UNKNOWN.

                        Invalid SUBSYST_STRING_GROUP_WW values are treated as
                        SUBSYST_STRING_SHORT.

    References:         None.

    Date Completed:     Tuesday, 08 April 2014

    ============================================================================
*/

BSTR   __stdcall                   XlateExeSubsystem_VB_WW
(
    CDWORD                   pdwSubsystem ,
    CSUBSYST_STRING_GROUP_WW penmStringGroup
) ;


#if defined(__cplusplus)
}
#endif  /* __cplusplus */
#endif  /*  _P6CUTILLIB1_INCLUDED */
