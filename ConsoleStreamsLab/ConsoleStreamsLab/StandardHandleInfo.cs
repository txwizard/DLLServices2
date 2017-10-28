/*
    ============================================================================

    Namespace:          ConsoleStreamsLab

    Class Name:			StandardHandleInfo

	File Name:			StandardHandleInfo.cs

    Synopsis:			This class is the managed implementation of the two
                        DllServices2 methods that still rely on unmanaged Win32
                        API wrappers.

    Remarks:			These are 100% managed, if you count a handful of calls
						into the Windows API via Platform Invoke as managed.

    Author:				David A. Gray

    License:            Copyright (C) 2017, David A. Gray. 
						All rights reserved.

                        Redistribution and use in source and binary forms, with
                        or without modification, are permitted provided that the
                        following conditions are met:

                        *   Redistributions of source code must retain the above
                            copyright notice, this list of conditions and the
                            following disclaimer.

                        *   Redistributions in binary form must reproduce the
                            above copyright notice, this list of conditions and
                            the following disclaimer in the documentation and/or
                            other materials provided with the distribution.

                        *   Neither the name of David A. Gray, nor the names of
                            his contributors may be used to endorse or promote
                            products derived from this software without specific
                            prior written permission.

                        THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
                        CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
                        WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
                        WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
                        PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL
                        David A. Gray BE LIABLE FOR ANY DIRECT, INDIRECT,
                        INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
                        (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
                        SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
                        PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
                        ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
                        LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
                        ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN
                        IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

	Date Begun:			Friday, 07 July 2017

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version Author Synopsis
    ---------- ------- ------ --------------------------------------------------
	2017/07/09 1.0	   DAG    This class makes its debut.
    ============================================================================
*/


using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

using WizardWrx;

namespace ConsoleStreamsLab
{
	[SuppressUnmanagedCodeSecurityAttribute]
	public static class StandardHandleInfo
	{
		#region Public Constants and Enumerations
		public const int SHS_UNDEFINED = MagicNumbers.ZERO;						// Define this in a way that enables this module to be distributed independently.

		//	--------------------------------------------------------------------
		//  The Input Mode flags returned by GetConsoleMode are transformed into
		//	a bit mapped enumeration.
		//	--------------------------------------------------------------------

		[Flags]
		public enum ConsoleModes
		{
			ENABLE_PROCESSED_INPUT = 0x0001 ,									// #define ENABLE_PROCESSED_INPUT  0x0001, per wincon.h
			ENABLE_LINE_INPUT = 0x0002 ,										// #define ENABLE_LINE_INPUT       0x0002, per wincon.h
			ENABLE_ECHO_INPUT = 0x0004 ,										// #define ENABLE_ECHO_INPUT       0x0004, per wincon.h
			ENABLE_WINDOW_INPUT = 0x0008 ,										// #define ENABLE_WINDOW_INPUT     0x0008, per wincon.h
			ENABLE_MOUSE_INPUT = 0x0010 ,										// #define ENABLE_MOUSE_INPUT      0x0010, per wincon.h
			ENABLE_INSERT_MODE = 0x0020 ,										// #define ENABLE_INSERT_MODE      0x0020, per wincon.h
			ENABLE_QUICK_EDIT_MODE = 0x0040 ,									// #define ENABLE_QUICK_EDIT_MODE  0x0040, per wincon.h
			ENABLE_EXTENDED_FLAGS = 0x0080 ,									// #define ENABLE_EXTENDED_FLAGS   0x0080, per wincon.h
			ENABLE_AUTO_POSITION = 0x0100 ,										// #define ENABLE_AUTO_POSITION    0x0100, per wincon.h
		}	// ConsoleModes

		//	--------------------------------------------------------------------
		//	The Output Mode flags used with GetConsoleMode are transformed into
		//	a bit mapped enumeration.
		//	--------------------------------------------------------------------

		[Flags]
		public enum ConsoleOutputModes
		{
			ENABLE_PROCESSED_OUTPUT = 0x0001 ,									// #define ENABLE_PROCESSED_OUTPUT    0x0001, per wincon.h
			ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002 ,								// #define ENABLE_WRAP_AT_EOL_OUTPUT  0x0002, per wincon.h
		}	// ConsoleOutputModes

		//	--------------------------------------------------------------------
		//	This enumeration was ported from StandardHandleState.H.
		//	--------------------------------------------------------------------

		public enum SHS_STANDARD_HANDLE
		{
			SHS_UNDEFINED ,														// Value = 0, which is reserved for indicating that the value is uninitialized.
			SHS_INPUT ,															// Value = 1, which corresponds to STD_INPUT_HANDLE
			SHS_OUTPUT ,														// Value = 2, which corresponds to STD_OUTPUT_HANDLE
			SHS_ERROR 															// Value = 3, which corresponds to STD_ERROR_HANDLE
		}	// SHS_STANDARD_HANDLE

		//	--------------------------------------------------------------------
		//	This enumeration was ported from StandardHandleState.H.
		//	--------------------------------------------------------------------

		public enum SHS_HANDLE_STATE
		{
			SHS_UNDETERMINABLE ,												// Value = 0, indicating that SHS_STANDARD_HANDLE is either SHS_UNDEFINED or out of range
			SHS_ATTACHED ,														// Value = 1, indicating that the handle corresponding to the value of SHS_STANDARD_HANDLE is attached to its console
			SHS_REDIRECTED ,													// Value = 2, indicating that the handle corresponding to the value of SHS_STANDARD_HANDLE is redirected
			SHS_SYSTEM_ERROR													// Value = 3, indicating that an internal error occurred. Call GetLastError to learn why.
		}	// SHS_HANDLE_STATE
		#endregion	// Public Constants and Enumerations


		#region Private Constants and Enumerations
		//	--------------------------------------------------------------------
		//	The following constants were ported from WinBase.h. They could have
		//	been built up into a bit-mapped enumeration, but for this one-off
		//	application, I decided that it wasn't worth the effort.
		//	--------------------------------------------------------------------

		const UInt32 VOLUME_NAME_DOS = 0x0;										// #define VOLUME_NAME_DOS 0x0, per WinBase.h (default)
		const UInt32 VOLUME_NAME_GUID = 0x1;									// #define VOLUME_NAME_GUID 0x1, per WinBase.h
		const UInt32 VOLUME_NAME_NT = 0x2;										// #define VOLUME_NAME_NT   0x2, per WinBase.h
		const UInt32 VOLUME_NAME_NONE = 0x4;									// #define VOLUME_NAME_NONE 0x4, per WinBase.h

		const UInt32 FILE_NAME_NORMALIZED = 0x0;								// #define FILE_NAME_NORMALIZED 0x0, per WinBase.h (default)
		const UInt32 FILE_NAME_OPENED = 0x8;									// #define FILE_NAME_OPENED     0x8, per WinBase.h
		
		//	--------------------------------------------------------------------
		//	This enumeration was ported from StandardHandleState.H.
		//	--------------------------------------------------------------------

		enum SHS_HANDLE_LABELS
		{
			SHS_HANDLE_SHORT_LABEL ,											// Short Label
			SHS_HANDLE_LONG_LABEL ,												// Long Label
			SHS_HANDLE_CONSTANT_NAME 											// Standard Handle Symbolic Constant Name
		}	// SHS_HANDLE_LABELS

		//	--------------------------------------------------------------------
		//	This array was ported from SHS_StandardHandleState.C.
		//
		//  The standard handle values accepted by GetStdHandle are values that
		//	would be invalid if returned as Windows handles of any kind.
		//	--------------------------------------------------------------------

		const int STD_INPUT_HANDLE = -10;										// #define STD_INPUT_HANDLE    ((DWORD)-10), per winbase.h
		const int STD_OUTPUT_HANDLE = -11;										// #define STD_OUTPUT_HANDLE   ((DWORD)-11), per winbase.h
		const int STD_ERROR_HANDLE = -12;										// #define STD_ERROR_HANDLE    ((DWORD)-12), per winbase.h

		const int ERROR_INVALID_HANDLE = 0x00000006;							// #define ERROR_INVALID_HANDLE          6L, per winerror.h

		//	--------------------------------------------------------------------
		//	The following constants are ported from WWKernelLibWrapper.H.
		//	--------------------------------------------------------------------

		const int WWKV_GETOSVERSIONINFO_WIN2K = 0x00000005;						// #define WWKV_GETOSVERSIONINFO_WIN2K         0x00000005L
		const int WWKW_GETOSVERSIONINFO_VISTA = 0x00000006;						// #define WWKW_GETOSVERSIONINFO_VISTA         0x00000006L
		const int WWKW_GETOSVERSIONINFO_WIN8 = 0x00000008;						// #define WWKW_GETOSVERSIONINFO_WIN8          0x00000008L
		const int WWKW_GETOSVERSIONINFO_WIN10 = 0x0000000A;						// #define WWKW_GETOSVERSIONINFO_WIN10         0x0000000AL
		const int WWKW_GETOSVERSIONINFO_BASE = 0x00000000;						// #define WWKW_GETOSVERSIONINFO_BASE          0x00000000L     // Windows 2000, with WWKV_GETOSVERSIONINFO_WIN2K, or Windows Vista, with WWKW_GETOSVERSIONINFO_VISTA
		const int WWKW_GETOSVERSIONINFO_PONT_1 = 0x00000001;					// #define WWKW_GETOSVERSIONINFO_PONT_1        0x00000001L     // Windows XP, with WWKV_GETOSVERSIONINFO_WIN2K, or Windows 7, with WWKW_GETOSVERSIONINFO_VISTA
		#endregion	// Private Constants and Enumerations


		#region Private static Data
		static readonly IntPtr INVALID_HANDLE_VALUE = ( IntPtr ) ( -1 );		// INVALID_HANDLE_VALUE is defined in the Windows Platform SDK, but I've forgotten in which header. Since its type is IntPtr, a structure type, it cannot be a constant.
		static readonly IntPtr INVALID_MODULE_HANDLE = IntPtr.Zero;				// INVALID_MODULE_HANDLE must also be defined as a static read-only variable for the same reason as does INVALID_HANDLE_VALUE.

		static readonly Int32 [ ] dwStdConsoleHandleIDs =
		{
			SHS_UNDEFINED ,														// This value is intentionally invalid, and corresponds to SHS_UNDEFINED.
			STD_INPUT_HANDLE ,													// SHS_STDIN
			STD_OUTPUT_HANDLE ,													// SHS_STDOUT
			STD_ERROR_HANDLE 													// SHS_STERR
		};	// dwStdConsoleHandleIDs
		#endregion	// Private static Data


		#region Platform Invoke Declarations for Windows Platform SDK Routines
		[DllImport ( "kernel32.dll" ,
					 SetLastError = true )]
		static extern bool GetConsoleMode
			(
				IntPtr hConsoleHandle ,
				out uint lpMode
			);
		[DllImport ( "kernel32.dll" ,
			         SetLastError = true )]
		static extern IntPtr GetStdHandle ( int nStdHandle );

		// typedef DWORD ( WINAPI* tGetFinalPathNameByHandle )( HANDLE , LPTSTR , DWORD , DWORD );
		delegate Int32 GetFinalPathNameByHandle ( IntPtr hFile , IntPtr lpszFilePath , UInt32 cchFilePath , UInt32 dwFlags );
		#endregion	// Platform Invoke Declarations for Windows Platform SDK Routines


		#region Public Static Methods
		//	--------------------------------------------------------------------
		//	This method was ported from SHS_StandardHandleState.C. Portions of
		//	it were subsequently re-factored into GetStandardHandle, to vastly
		//	simplify porting SHS_GetStdHandleFNCLI to GetRedirectedFileName.
		//	--------------------------------------------------------------------

		public static SHS_HANDLE_STATE SHS_StandardHandleState ( SHS_STANDARD_HANDLE penmStdHandleID )
		{
			IntPtr hThis = INVALID_MODULE_HANDLE;								// In the ANSI C implementation, the first use of this variable initializes it. As a concession to the C# compiler, this version employs a static initializer.
			UInt32 dwModde = ( uint ) INVALID_MODULE_HANDLE;					// The first use of this variable initializes it, AND it's a throwaway. As a concession to the C# compiler, this version employs a static initializer.

			if ( ( hThis = GetStandardHandle ( penmStdHandleID ) ) != INVALID_HANDLE_VALUE )
			{	// Handle acquired. Since GetStdHandle leaves the reference count unchanged, hThis needn't, and shouldn't, be closed.
				if ( GetConsoleMode ( hThis , out dwModde ) )
				{	// Handles that are attached to a console have a ConsoleMode.
					return SHS_HANDLE_STATE.SHS_ATTACHED;
				}	// TRUE (Handle is attached to its console.) block, if ( GetConsoleMode ( hThis , &dwModde ) )
				else
				{	// When a console stream is redirected, its handle loses its ConsoleMode.
					if ( Marshal.GetLastWin32Error ( ) == ERROR_INVALID_HANDLE )
					{	// Since LastError is ERROR_INVALID_HANDLE, the cause of the failure is that the handle is redirected.
						return SHS_HANDLE_STATE.SHS_REDIRECTED;
					}	// TRUE (anticipated outcome) block, if ( GetLastError ( ) == ERROR_INVALID_HANDLE )
					else
					{	// Something besides redirection caused GetConsoleMode to fail.
						return SHS_HANDLE_STATE.SHS_SYSTEM_ERROR;
					}	// FALSE (unanticipated outcome) block, if ( GetLastError ( ) == ERROR_INVALID_HANDLE )
				}	// FALSE (Handle is redirected from its console into a file or pipe.) block, if ( GetConsoleMode ( hThis , &dwModde ) )
			}	// TRUE (anticipated outcome) block, if ( ( hThis = GetStdHandle ( dwStdConsoleHandleIDs [ ( int ) penmStdHandleID ] ) ) != ( IntPtr ) INVALID_HANDLE_VALUE )
			else
			{	// Windows declined the request. Report that GetStdHandle failed.
				return penmStdHandleID == SHS_STANDARD_HANDLE.SHS_UNDEFINED
					? SHS_HANDLE_STATE.SHS_UNDETERMINABLE
					: SHS_HANDLE_STATE.SHS_SYSTEM_ERROR;
			}	// FALSE (unanticipated outcome) block, if ( ( hThis = GetStdHandle ( dwStdConsoleHandleIDs [ ( int ) penmStdHandleID ] ) ) != ( IntPtr ) INVALID_HANDLE_VALUE )
		}	// SHS_StandardHandleState

		//	--------------------------------------------------------------------
		//	This method is a consolidated port of  SHS_GetStdHandleFNCLI and 
		//	SHS_GetStdHandleFNW, which are rolled into one method.
		//	--------------------------------------------------------------------

		public static string GetRedirectedFileName ( SHS_STANDARD_HANDLE penmStdHandleID )
		{
			if ( WWKW_OSIsVistaOrNewer ( ) )
			{
				// typedef DWORD ( WINAPI* tGetFinalPathNameByHandle )( HANDLE , LPTSTR , DWORD , DWORD );

				using ( UnmanagedLibrary lib = new UnmanagedLibrary ( "kernel32" ) ) // becomes call to LoadLibrary
				{
					GetFinalPathNameByHandle fnGetFinalPathNameByHandle = lib.GetUnmanagedFunction<GetFinalPathNameByHandle> ( "GetFinalPathNameByHandleW" );
					
					char [ ] achrManagedBuffer = new char [ MagicNumbers.CAPACITY_32KB ];	// Reserve 32K characters.
					GCHandle gchPinnedArray = GCHandle.Alloc (
						achrManagedBuffer ,													// Now that it is allocated, keep the garbage collector away from it.		
						GCHandleType.Pinned );
					IntPtr lpManagedBuffer = gchPinnedArray.AddrOfPinnedObject ( );			// Since IntPtr is a structure, this operation cannot be nested.

					//	--------------------------------------------------------
					//	Since the return value, intFQFNLength, is the last 
					//	parameter of the string constructor, which must be first
					//	onto the stack, this function call cannot be nested.
					//	--------------------------------------------------------

					Int32 intFQFNLength = fnGetFinalPathNameByHandle (
						GetStandardHandle ( penmStdHandleID ) ,								// IntPtr hFile
						lpManagedBuffer ,													// IntPtr lpszFilePath
						MagicNumbers.CAPACITY_32KB ,										// UInt32 cchFilePath
						FILE_NAME_NORMALIZED | VOLUME_NAME_DOS );							// UInt32 dwFlags

					return new string (														// Array of characters in, string out.
						achrManagedBuffer ,													//		Array from which to copy characters
						ArrayInfo.ARRAY_FIRST_ELEMENT ,										//		Start copying at this index into the array.
						intFQFNLength );													//		Stop when this many characters have been copied.
				}	// implicit call to lib.Dispose, which calls FreeLibrary.
			}	// TRUE (anticipated outcome) block, if ( WWKW_OSIsVistaOrNewer ( ) )
			else
			{
				throw new InvalidOperationException (
					string.Format (
						"SYSTEM ERROR: The required Windows system routines are unavailable on the installed version of Microsoft Windows, version {0}{1}.{4}The minimum supported version is {2}.{3}." ,
						new object [ ]
						{
							Environment.OSVersion.Version.Major ,				// Format Item 0 = installed version of Microsoft Windows, version {0}
							Environment.OSVersion.Version.Minor ,				// Format Item 1 = {1}.
							WWKW_GETOSVERSIONINFO_VISTA ,						// Format Item 2 = minimum supported version is {2}.
							WWKW_GETOSVERSIONINFO_BASE ,						// Format Item 3 = .{3}.
							Environment.NewLine									// Format Item 4 = Newline embedded: .{4}The minimum supported
						} ) );
			}	// FALSE (unanticipated outcome) block, if ( WWKW_OSIsVistaOrNewer ( ) )
		}	// GetRedirectedFileName
		#endregion	// Public Static Methods


		#region Private Static Methods
		/// <summary>
		/// Though perhaps a tad less efficient than calling the like named
		/// Windows API function, this method is a 100% managed approach to the
		/// task of getting the instance handle (base address) of a module.
		/// </summary>
		/// <param name="pstrModuleName">
		/// Specify the base name of the desired module. Omit the .DLL suffix.
		/// </param>
		/// <returns>
		/// If a like named module exists in the collection of loaded modules,
		/// its instance handle, which happens also to be its base address, is
		/// returned. Otherwise, the return value is INVALID_MODULE_HANDLE.
		/// </returns>
		/// <see cref="https://blogs.msdn.microsoft.com/oldnewthing/20041025-00/?p=37483"/>
		private static IntPtr GetModuleHandle ( string pstrModuleName )
		{
			string strModuleNameMatch = pstrModuleName.ToLower ( );

			foreach ( System.Diagnostics.ProcessModule thisModule in System.Diagnostics.Process.GetCurrentProcess ( ).Modules )
			{
				if ( thisModule.ModuleName.ToLower ( ) == strModuleNameMatch )
				{	// Save the base address, so that the module can be disposed.
					IntPtr rHINSTHandle =  thisModule.BaseAddress;
					thisModule.Dispose ( );
					return rHINSTHandle;
				}	// TRUE (The desired module is loaded, and has been found in the collection.) block, if ( thisModule.ModuleName.ToLower ( ) == strModuleNameMatch )
				else
				{	// Modules have unmanaged resources, and implement IDisposable. Do so.
					thisModule.Dispose ( );
				}	// FALSE (The current module isn't the one that was sought.) block, if ( thisModule.ModuleName.ToLower ( ) == strModuleNameMatch )
			}	// foreach ( System.Diagnostics.ProcessModule thisModule in System.Diagnostics.Process.GetCurrentProcess ( ).Modules )

			return INVALID_MODULE_HANDLE;
		}	// GetModuleHandle


		private static IntPtr GetStandardHandle ( SHS_STANDARD_HANDLE penmStdHandleID )
		{
			IntPtr hThis = INVALID_HANDLE_VALUE;								// In the ANSI C implementation, the first use of this variable initializes it. As a concession to the C# compiler, this version employs a static initializer.

			switch ( penmStdHandleID )
			{
				case SHS_STANDARD_HANDLE.SHS_INPUT:
				case SHS_STANDARD_HANDLE.SHS_OUTPUT:
				case SHS_STANDARD_HANDLE.SHS_ERROR:
					return GetStdHandle (
						dwStdConsoleHandleIDs [ ( int ) penmStdHandleID ] );	// An invalid argument elicits a return value of INVALID_HANDLE_VALUE, for which the caller must test. 

				case SHS_UNDEFINED:												// Argument penmStdHandleID is uninitialized.
				default:														// Argument penmStdHandleID is out of range.
					return INVALID_HANDLE_VALUE ;
			}	// switch ( penmStdHandleID )
		}	// GetStandardHandle


		//	--------------------------------------------------------------------
		//	The following methods are implemented as preprocessor macros in
		//	WKernelLibWrapper.H. However, since C# doesn't support macros, they
		//	are implemented as one-line static methods that we fervently hope to
		//	see substituted with code that is generated inline. Otherwise, they
		//	are cheap enough as out-of-line functions.
		//
		//	Only the first, WWKW_OSIsVistaOrNewer, is actually required by this
		//	class, but I decided that I may as well port all four, and be done
		//	with it. Eventually, they'll probably find a home in another library
		//	module.
		//	--------------------------------------------------------------------

		private static bool WWKW_OSIsVistaOrNewer ( )
		{
			return Environment.OSVersion.Version.Major >= WWKW_GETOSVERSIONINFO_VISTA;
		}	// WWKW_OSIsVistaOrNewer

		private static bool WWKW_OSIsWindows7OrNewer ( )
		{
			return Environment.OSVersion.Version.Major >= WWKW_GETOSVERSIONINFO_VISTA && Environment.OSVersion.Version.Minor >= WWKW_GETOSVERSIONINFO_PONT_1;
		}	// WWKW_OSIsWindows7OrNewer

		private static bool WWKW_OSIsMinimumVersionMaj ( int pintMajorMinimum )
		{
			return Environment.OSVersion.Version.Major >= pintMajorMinimum;
		}	// WWKW_OSIsMinimumVersionMaj

		private static bool WWKW_OSIsMinimumVersionMin ( int pintMajorMinimum , int pintOSVersionMinorinimum )
		{
			return Environment.OSVersion.Version.Major >= pintMajorMinimum && Environment.OSVersion.Version.Minor >= pintOSVersionMinorinimum;
		}	// WWKW_OSIsMinimumVersionMin
		#endregion	// Private Static Methods
	}	// public static class StandardHandleInfo
}	// partial namespace ConsoleStreamsLab