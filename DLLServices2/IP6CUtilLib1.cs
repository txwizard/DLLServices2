/*
    ============================================================================

    Namespace:          WizardWrx.DLLServices2

    Class Name:         IP6CUtilLib1

    Dependencies:       P6CUtilLib1.dll, version 2.6.34.1 or later.

    File Name:          IP6CUtilLib1.cs

    Synopsis:           Expose methods that determine the subsystem in which the
                        current process is running by examining flags in the PE
                        (Portable Executable) file that was first loaded into
                        its address space.

    Remarks:            1)  Although it is theoretically possible to use managed
                            code to accomplish this task, since I have a working
                            Win32 DLL that does the hardest part of the work,
                            I decided that it wasn't worth the effort, when a
                            simple P/Invoke can leverage that DLL.

                        2)  The underlying Win32 DLL exposes a third function,
                            XlateProcessSubsystem_WW, that translates the codes
                            into meaningful statements, each of which is a 
                            complete sentence. Although I could probably have
                            imported that, too, I chose to duplicate it in
                            managed C# code, for two reasons.

                            a)  Importing the Win32 function would have required
                                marshaling another Unicode string.

                            b)  Of greater significance, since the returned
                                string is allocated from the process heap, and I
                                didn't want to expend the effort to figure out
                                how to do that, the native version prevents a
                                small memory leak.

    References:			CA1060: Move P/Invokes to NativeMethods Class
                        https://msdn.microsoft.com/en-us/library/ms182161.aspx

    Author:             David A. Gray.

    License:            Copyright (C) 2014-2016, David A. Gray. 
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

    Created:            Saturday, 17 May 2014

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version Author Description
    ---------- ------- ------ --------------------------------------------------
    2014/05/17 4.1     DAG    Initial implementation.

    2014/05/19 4.1     DAG    CLASS RELOCATION NOTICE

                              It finally occurred to me that I already have a
                              class that is an ideal candidate to house these
                              classes that were the source of the tight coupling
                              between the WizardWrx.ApplicationHelpers and
                              WizardWrx.ConsoleAppAids2. Moving the tightly
                              coupled classes into this class, which was created
                              to house a single abstract base class decouples
                              both libraries from each other, in exchange fro a
                              common coupling to this class library, yielding a
                              much more robust architecture that will be easier
                              to maintain.

    2014/06/06 4.2     DAG    Relocate this class to a new DLLServices2 
                              assembly, which is not signed with a strong name.
                              Like the other classes that I moved here, this one
                              keeps its place in the namespace hierarchy.

    2014/06/22 5.1     DAG    Correct an oversight that left this class in the
                              old WizardWrx.ApplicationHelpers namespace. Since
                              this change affects only two other DLLs, and, at
                              most one user program, I took advantage of the
                              opportunity to promote the DLLServices2 namespace
                              to the first rank under the overall WizardWrx
                              namespace.

    2015/06/05 5.4     DAG    Move the two Platform Invoke methods into internal
                              static class SafeNativeMethods, to resolve Code
                              Analysis warning CA1060, Move P/Invokes to
                              NativeMethods class.

    2015/07/10 5.5    DAG     Add SHS_StandardHandleState, which is exposed 
                              through StandardHandleState, a static method on
                              the StateManager class.

                              Incorporate my three-clause BSD license.

    2015/10/28 5.8    DAG	  1) Implement GetRedirectedStreamFileName as a
                                 wrapper around SHS_GetStdHandleFNW, a native
                                 function exported by WWConAid.dll. 

                              1) Implement GetExeSubsystemID, which casts the 
                                 value returned by method GetExeSubsystem to
                                 a member of the PESubsystemID enumeration.

                              2) Implement an overload of XlateProcessSubsystem
                                 that takes in a PESubsystemID, which it casts
                                 to unsigned integer, and hands off to the
                                 original method for processing.

                              3) Replace the switch block in the original
                                 implementation of XlateProcessSubsystem with a
                                 lookup table, which should be much more
                                 efficient.

    2016/03/29 6.0    DAG	  Resolve the ambiguous references in the XML help.
    ============================================================================
*/


using System;
using System.Runtime.InteropServices;
using System.Security;


namespace WizardWrx.DLLServices2
{
    /// <summary>
    /// This class exposes static methods imported from its namesake library of
    /// unmanaged native routines for getting information not otherwise easily
    /// retrievable by managed code.
    /// 
    /// Since static classes are implicitly sealed, this class cannot be inherited.
    /// </summary>
    public static class IP6CUtilLib1
    {
        /// <summary>
        /// GetProcessSubsystem and GetExeSubsystem return this when they fail.
        /// </summary>
        /// <see cref="GetExeSubsystem"/>
        /// <see cref="GetExeSubsystemID"/>
        public const uint SUBSYSTEM_UNKNOWN = ( uint ) IP6CUtilLib1.PESubsystemID.IMAGE_SUBSYSTEM_UNKNOWN;
    
        /// <summary>
        /// Map the unsigned integer returned by GetExeSubsystem onto an
        /// enumerated type that conveys its correct interpretation.
        /// </summary>
        /// <see cref="GetExeSubsystem"/>
        /// <see cref="GetExeSubsystemID"/>
		/// <see cref="WizardWrx.DLLServices2.IP6CUtilLib1.XlateProcessSubsystem(WizardWrx.DLLServices2.IP6CUtilLib1.PESubsystemID)"/>
        public enum PESubsystemID : uint
        {
            /// <summary>
            /// Unknown subsystem.
            /// </summary>
            IMAGE_SUBSYSTEM_UNKNOWN ,

            /// <summary>
            /// Image doesn't require a subsystem.
            /// </summary>
            IMAGE_SUBSYSTEM_NATIVE ,

            /// <summary>
            /// Image runs in the Windows GUI subsystem.
            /// </summary>
            IMAGE_SUBSYSTEM_WINDOWS_GUI ,

            /// <summary>
            /// Image runs in the Windows character subsystem.
            /// </summary>
            IMAGE_SUBSYSTEM_WINDOWS_CUI ,

            /// <summary>
            /// Image runs in the OS/2 character subsystem.
            /// </summary>
            IMAGE_SUBSYSTEM_OS2_CUI ,

            /// <summary>
            /// Image runs in the Posix character subsystem.
            /// </summary>
            IMAGE_SUBSYSTEM_POSIX_CUI ,

            /// <summary>
            /// Image is a native Win9x driver.
            /// </summary>
            IMAGE_SUBSYSTEM_NATIVE_WINDOWS ,

            /// <summary>
            /// Image runs in the Windows CE subsystem.
            /// </summary>
            IMAGE_SUBSYSTEM_WINDOWS_CE_GUI ,

            /// <summary>
            /// Image is an EFI Application.
            /// </summary>
            IMAGE_SUBSYSTEM_EFI_APPLICATION ,

            /// <summary>
            /// Image is a EFI Boot Service Driver.
            /// </summary>
            IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER ,

            /// <summary>
            /// Image is a EFI Runtime Driver.
            /// </summary>
            IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER ,

            /// <summary>
            /// Image runs from a EFI ROM.
            /// </summary>
            IMAGE_SUBSYSTEM_EFI_ROM ,

            /// <summary>
            /// Image runs on XBOX.
            /// </summary>
            IMAGE_SUBSYSTEM_XBOX ,
        };  // PESubsystemID


        /// <summary>
        /// XlateProcessSubsystem uses this array of strings to map the ID
        /// returned by GetExeSubsystem or GetExeSubsystemID to a descriptive
        /// string suitable for display on a report.
        /// </summary>
        /// <see cref="GetExeSubsystem"/>
        /// <see cref="GetExeSubsystemID"/>
		/// <see cref="WizardWrx.DLLServices2.IP6CUtilLib1.XlateProcessSubsystem(WizardWrx.DLLServices2.IP6CUtilLib1.PESubsystemID)"/>
        private static readonly string [ ] s_astrPESubsystemTypeStrings =
        {
            WizardWrx.DLLServices2.Properties.Resources.IMAGE_SUBSYSTEM_UNKNOWN ,
            WizardWrx.DLLServices2.Properties.Resources.IMAGE_SUBSYSTEM_NATIVE ,
            WizardWrx.DLLServices2.Properties.Resources.IMAGE_SUBSYSTEM_WINDOWS_GUI ,
            WizardWrx.DLLServices2.Properties.Resources.IMAGE_SUBSYSTEM_WINDOWS_CUI ,
            WizardWrx.DLLServices2.Properties.Resources.IMAGE_SUBSYSTEM_OS2_CUI ,
            WizardWrx.DLLServices2.Properties.Resources.IMAGE_SUBSYSTEM_POSIX_CUI ,
            WizardWrx.DLLServices2.Properties.Resources.IMAGE_SUBSYSTEM_NATIVE_WINDOWS ,
            WizardWrx.DLLServices2.Properties.Resources.IMAGE_SUBSYSTEM_WINDOWS_CE_GUI ,
            WizardWrx.DLLServices2.Properties.Resources.IMAGE_SUBSYSTEM_EFI_APPLICATION ,
            WizardWrx.DLLServices2.Properties.Resources.IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER ,
            WizardWrx.DLLServices2.Properties.Resources.IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER ,
            WizardWrx.DLLServices2.Properties.Resources.IMAGE_SUBSYSTEM_EFI_ROM ,
            WizardWrx.DLLServices2.Properties.Resources.IMAGE_SUBSYSTEM_XBOX ,
            WizardWrx.DLLServices2.Properties.Resources.IMAGE_SUBSYSTEM_UNDEFINED_VALUE
        };	// s_astrPESubsystemTypeStrings

        /// <summary>
        /// Return the subsystem ID of the executing Portable Executable.
        /// </summary>
        /// <returns>
        /// The subsystem ID is returned as an unsigned integer.
        /// 
        /// COMPATIBLITY NOTE: If you are running the Visual Studio debugger and
        /// the Visual Studio Hosting Process (which is TRUE by default), this
        /// routine always returns 2 (Windows GUI Subsyste) because the hosting
        /// process, which is a Windows GUI program, is the first program loaded
        /// into the process. In most cases, you can get away without the Visual
        /// Studio hosting process.
        /// 
        /// For additional information, please see "Debugging and the Hosting
        /// Process (Visual Studio 2010)," in the MSDN library, at
        /// http://msdn.microsoft.com/en-us/library/ms242202(v=vs.100).aspx.
        /// </returns>
        /// <remarks>
        /// The return value maps to a member of the PESubsystemID enumeration,
        /// which is used internally by XlateProcessSubsystem.
        /// 
        /// Although this routine imports native code, since the code cleans up
        /// after itself before it returns control to its caller, there are no
        /// unmanaged resources to clean up.
        /// </remarks>
		/// <seealso cref="WizardWrx.DLLServices2.IP6CUtilLib1.XlateProcessSubsystem(uint)"/>
        public static uint GetProcessSubsystem ( )
        {
            return SafeNativeMethods.GetProcessSubsystem_WW ( );
        }   // public static uint GetProcessSubsystem


        /// <summary>
        /// Return the subsystem ID of the specified Portable Executable.
        /// </summary>
        /// <param name="plpExeName">
        /// Specify the name of the Portable Executable to be tested. The file
        /// name must be specified in such a way that the Win32 CreateFile
        /// routine can open a read only handle on it.
        /// </param>
        /// <returns>
        /// The subsystem ID is returned as an unsigned integer.
        /// </returns>
        /// <remarks>
        /// The return value maps to a member of the PESubsystemID enumeration,
        /// which is used internally by XlateProcessSubsystem.
        /// 
        /// Since this routine operates on the Portable Executable file named by
        /// its caller, it is immune to the compatibility issues that affect
        /// GetProcessSubsystem.
        /// 
        /// If you infer from the reference to CreateFile in the requirements 
        /// for argument plpMMainModuleFQFN that this routine imports native
        /// code, you are correct. Rather than develop a managed implementation,
        /// I imported a working native Win32 routine, GetExeSubsystemW_WW,
        /// from a DLL that contains dozens of routines that I have refined over
        /// the last nine years or so.
        /// 
        /// Although this routine imports native code, since the code cleans up
        /// after itself before it returns control to its caller, there are no
        /// unmanaged resources to clean up.
        /// </remarks>
        /// <seealso cref="GetExeSubsystemID"/>
		/// <seealso cref="WizardWrx.DLLServices2.IP6CUtilLib1.XlateProcessSubsystem(uint)"/>
        public static uint GetExeSubsystem ( string plpExeName )
        {
            return SafeNativeMethods.GetExeSubsystemW_WW ( plpExeName );
        }   // public static uint GetExeSubsystem

        /// <summary>
        /// Given the ID of a redirected standard console file handle, return
        /// the name of the file to which it is redirected.
        /// </summary>
        /// <param name="puintStdHandleID">
        /// A member of the ShsStandardHandle enumeration, cast to an unsigned integer
        /// </param>
        /// <returns>
        /// Absolute (fully qualified) name of file to which the specified handle is
        /// redirected.
        /// </returns>
        /// <remarks>
        /// This method wraps a StringBuilder around the private
        /// SafeNativeMethods.SHS_GetStdHandleFNCLI method.
        /// </remarks>
        public static string GetStdHandleFNCLI ( uint puintStdHandleID )
        {
			char [ ] achrManagedBuffer = new char [ MagicNumbers.CAPACITY_32KB ];	// Reserve 32K characters.
			GCHandle gchPinnedArray = GCHandle.Alloc (
				achrManagedBuffer ,													// Now that it is allocated, keep the garbage collector away from it.		
				GCHandleType.Pinned );
			IntPtr lpManagedBuffer = gchPinnedArray.AddrOfPinnedObject ( );			// Since IntPtr is a structure, this operation cannot be nested.
			Int32 intFQFNLength = SafeNativeMethods.SHS_GetStdHandleFNCLI (
				puintStdHandleID ,													// Desired standard handle
				lpManagedBuffer ,													// Address of managed buffer
				MagicNumbers.CAPACITY_64KB );										// Size of buffer, in BYTES.
			return new string (														// Array of characters in, string out.
				achrManagedBuffer ,													//		Array from which to copy characters
				ArrayInfo.ARRAY_FIRST_ELEMENT ,										//		Start copying at this index into the array.
				intFQFNLength );													//		Stop when this many characters have been copied.
        }	// GetStdHandleFNCLI


        /// <summary>
        /// Return the subsystem ID of the specified Portable Executable cast to
        /// a member of the PESubsystemID enumeration, thus clearly documenting
        /// its correct interpretation.
        /// </summary>
        /// Specify the name of the Portable Executable to be tested. The file
        /// name must be specified in such a way that the Win32 CreateFile
        /// routine can open a read only handle on it.
        /// <returns>
        /// The subsystem ID is returned as a member of the PESubsystemID
        /// enumeration, thus documenting its correct interpretation.
        /// </returns>
        /// <remarks>
        /// This method calls sibling method GetExeSubsystem. At the cost of a
        /// very short extra stack frame, this approach clearly documents the
        /// data flow.
        /// </remarks>
        /// <see cref="GetExeSubsystem"/>
		/// <seealso cref="WizardWrx.DLLServices2.IP6CUtilLib1.XlateProcessSubsystem(uint)"/>
        public static PESubsystemID GetExeSubsystemID ( string plpExeName )
        {
            return ( PESubsystemID ) GetExeSubsystem ( plpExeName );
        }	// public static PESubsystemID GetExeSubsystemID


        /// <summary>
        /// Translate a subsystem ID, such as the value returned by 
        /// GetProcessSubsystem, into a descriptive sentence.
        /// </summary>
        /// <param name="puintSubsystemID">
        /// Pass in the raw ID returned by GetProcessSubsystem To have it
        /// translated into English.
        /// </param>
        /// <returns>
        /// The return value is a string, suitable for display in a message.
        /// </returns>
        /// <remarks>
        /// The strings are read from resource strings that are stored in the
        /// DLL that exports this routine.
        /// 
        /// Unlike the other two routines defined by this class, this routine is
        /// 100% managed C# code.
        /// 
        /// The first version of this method was implemented as a switch block.
        /// This improved version uses a lookup table, which should be faster.
        /// </remarks>
        /// <see cref="GetExeSubsystem"/>
        /// <seealso cref="GetExeSubsystemID"/>
        public static string XlateProcessSubsystem ( uint puintSubsystemID )
        {
            if ( puintSubsystemID < s_astrPESubsystemTypeStrings.Length )
                return s_astrPESubsystemTypeStrings [ puintSubsystemID ];
            else
                return string.Format (
                    s_astrPESubsystemTypeStrings [ ArrayInfo.IndexFromOrdinal ( ( int ) puintSubsystemID ) ] ,
                    puintSubsystemID );
        }   // public static string XlateProcessSubsystem (1 of 2)


        /// <summary>
        /// Translate a subsystem ID, such as the value returned by 
        /// GetProcessSubsystem, into a descriptive sentence.
        /// </summary>
        /// <param name="penmPESubsystemID">
        /// Pass in the PESubsystemID returned by GetExeSubsystemID To have it
        /// translated into English.
        /// </param>
        /// <returns>
        /// The return value is a string, suitable for display in a message.
        /// </returns>
        /// <remarks>
        /// The strings are read from resource strings that are stored in the
        /// DLL that exports this routine.
        /// 
        /// Unlike the other two routines defined by this class, this routine is
        /// 100% managed C# code.
        /// 
        /// Internally, this routine casts its input to unsigned integer, then
        /// hands off to the original overload, which validates the argument and
        /// returns the string.
        /// </remarks>
        /// <seealso cref="GetExeSubsystem"/>
        /// <see cref="GetExeSubsystemID"/>
        public static string XlateProcessSubsystem ( PESubsystemID penmPESubsystemID )
        {
            return XlateProcessSubsystem ( ( uint ) penmPESubsystemID );
        }   // public static string XlateProcessSubsystem (2 of 2)
    }   // public static class IP6CUtilLib1


    /// <summary>
    /// This class exposes my two P/Invoke methods in a way that enables the CLR
    /// to treat them as safe for use in any application, including partial
    /// trust applications.
    /// </summary>
    [SuppressUnmanagedCodeSecurityAttribute]
    internal static class SafeNativeMethods
    {
        //  --------------------------------------------------------------------
        //  To prompt the CLR to call GetLastError immediately after the invoked
        //  routine returns, and save its value for Marshal.GetLastWin32Error,
        //  the SetLastError attribute is set to true.
        //  --------------------------------------------------------------------

        [DllImport ( "P6CUtilLib1.dll" ,
                     EntryPoint = "GetProcessSubsystem_WW" ,
                     SetLastError = true )]
        internal static extern uint GetProcessSubsystem_WW ( );

        //  --------------------------------------------------------------------
        //  Since my naming convention puts the "A" and "W" suffixes in a
        //  nonstandard place, my DLLImport attribute MUST set ExactSpelling to
        //  TRUE, and SHOULD explicitly specify Unicode string marshaling by
        //  setting the CharSet attribute to CharSet.Unicode.
        //
        //  To prompt the CLR to call GetLastError immediately after the invoked
        //  routine returns, and save its value for Marshal.GetLastWin32Error,
        //  the SetLastError attribute is set to true.
        //  --------------------------------------------------------------------

        [DllImport ( "P6CUtilLib1.dll" ,
                     EntryPoint = "GetExeSubsystemW_WW" ,
                     ExactSpelling = true ,
                     CharSet = CharSet.Unicode ,
                     SetLastError = true )]
        internal static extern uint GetExeSubsystemW_WW
            (
                string plpMMainModuleFQFN
            );

        [DllImport ( "WWConAid.dll" ,
                     EntryPoint = "SHS_GetStdHandleFNCLI" ,
                     CallingConvention = CallingConvention.StdCall ,	// Make it explicitly so, although this is the default.
                     CharSet = CharSet.Unicode ,
                     ExactSpelling = true ,
                     SetLastError = false ,
                     ThrowOnUnmappableChar = false ,					// We'll take our chances.
                     PreserveSig = true )]								// This method returns a retVal. Was this the issue the whole time?
        [return: MarshalAs ( UnmanagedType.I4 )]
        internal static extern Int32 SHS_GetStdHandleFNCLI
            (
				uint puintStdHandleID ,									// Specify the desired standard stream.
				IntPtr plpOutput ,										// Pointer to buffer of at least pintOutBufSize bytes where SHS_GetStdHandleFNCLI can deliver its output
				Int32 pintOutBufSize									// Size, in BYTES, of the buffer to which plpOutputBuffer points
            );

        [DllImport ( "WWConAid.dll" ,
                     EntryPoint = "SHS_GetStdHandleFNError" ,
                     CallingConvention = CallingConvention.Winapi ,
                     ExactSpelling = true ,
                     SetLastError = false ,
                     ThrowOnUnmappableChar = false ,
                     PreserveSig = true )]
        [return: MarshalAs ( UnmanagedType.U4 )]

        //	--------------------------------------------------------------------
        //	In the unlikely event that we need it following a call to 
        //	SHS_GetStdHandleFNCLI, we'll use our own mechanism in lieu of
        //	GetLastError.
        //	--------------------------------------------------------------------

        internal static extern UInt32 SHS_GetStdHandleFNError ( );

        //  --------------------------------------------------------------------
        //	Rather than confuse the marshaler with the fact that the inputs and
        //	outputs of SHS_StandardHandleState are enumerated types, I chose to
        //	treat them as unsigned integers at the interface, and cast them back
        //	in the wrapper. The enumerations and wrapper are defined in another
        //	local class, StateManager, but I put the methods into this class, to
        //	eliminate having to figure out how, or even whether, I can define 2
        //	SafeNativeMethods classes in the same assembly and namespace. The
        //	private call should cost about the same, since both live in the same
        //	assembly and namespace.
        //  --------------------------------------------------------------------

        [DllImport ( "WWConAid.dll" ,
                     EntryPoint = "SHS_StandardHandleState" ,
                     SetLastError = true )]
        internal static extern uint SHS_StandardHandleState
            (
                uint puintStdHandleID
            );
    }	// internal static class SafeNativeMethods
}   // partial namespace WizardWrx.DLLServices2