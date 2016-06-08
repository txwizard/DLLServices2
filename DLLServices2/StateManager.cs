﻿/*
    ============================================================================

    Namespace:          WizardWrx.DLLServices2

	File Name:			StateManager.cs

    Class Name:			StateManager

    Synopsis:			Maintain run-time information about the executing
						instance of the console application in a Singleton, so
						that it is readily accessible from any of its classes
						and methods.

    Remarks:			This class is a simplification of ApplicationInstance,
						which it supersedes. Methods and properties that are
						applicable only to a console mode applications were
						removed, and will resurface in a derived class, to be
						exported by the ConsoleAppAids2 class library, where
						they belong. This change is motivated by my desire to
						decouple this class library from ConsoleAppAids2.

						The Code Analysis Module raised two warnings about code
						in this module that I suspect would be truly difficult
						for a bad actor to leverage to cause harm. Regardless,
						I did my best to address both.

						1)	CA1404: Call GetLastError immediately after
								    P/Invoke.

							Having seen the disassembled native code generated
							to implement this method, it appears that this is
							unwarranted, and my code safely meets its design
							objective of skipping the call to GetLastError when
							its return value is meaningless,  and would be
                            ignored.

						2)	CA2122: Do not indirectly expose methods with link
								    demands.

							The method that calls Marshal.GetLastWin32Error
							raises this warning due to its link demand. The
							specified mitigation is to add an attribute,
							assembly: EnvironmentPermissionAttribute, with
							SecurityAction.RequestRefuse, Unrestricted = true.
						`	Since this prevented binding the library to other
							full trust assemblies, I did away with that, and
							settled for another attribute to suppress the
							warning.

						At long last, as of version 5.5, this assembly is
						completely independent of strong named assembly
						WizardWrx.SharedUtl2.dll.

	References:			1)	CA1404: Call GetLastError immediately after P/Invoke
							https://msdn.microsoft.com/en-us/library/ms182199.aspx?f=255&MSPPError=-2147217396

						2)	CA2122: Do not indirectly expose methods with link demands
							https://msdn.microsoft.com/en-us/library/ms182303.aspx?f=255&MSPPError=-2147217396

						3)	Be Careful When Using GetCallingAssembly() and Always Use the Release Build for Testing
							http://www.ticklishtechs.net/2010/03/04/be-careful-when-using-getcallingassembly-and-always-use-the-release-build-for-testing/

    Author:				David A. Gray

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

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version Author Synopsis
    ---------- ------- ------ --------------------------------------------------
    2014/03/28 4.0     DAG    Create this class by removing the methods and
                              properties that are meaningful only in a console
                              application from the ApplicationInstance class,
                              which remains, but is marked obsolete.

                              The following methods are moving to the derived
                              class.

                                  DisplayBOJMessage
                                  DisplayEOJMessage
                                  ErrorExit
                                  GetBOJMessage
                                  GetEOJMessage
                                  NormalExit

    2014/05/20 4.1     DAG    Since I finally have a working interface to the
                              GetExeSubsystem routine, use it to expose the
                              subsystem ID and string as a pair of properties.

                                  AppSubsystemID
                                  AppSubsystem

    2014/06/06 4.2     DAG    Class moved to library WizardWrx.DLLServices2, but
                              kept its position in the namespace hierarchy.

    2014/06/21 5.1     DAG    Correct an oversight that left this class in the
                              old WizardWrx.ApplicationHelpers namespace. Since
                              this change affects only two other DLLs, and, at
                              most one user program, I took advantage of the
                              opportunity to hoist the DLLServices2 namespace to
                              the first rank under the overall WizardWrx
                              namespace.

    2015/06/05 5.4     DAG    Allow the Code Analysis module to add attribute
                              System.Diagnostics.CodeAnalysis.SuppressMessage to
                              the InitializeOnFirstUse method, to suppress a
                              warning that the method doesn't call GetLastError
							  immediately upon returning from the indirect
                              P/Invoke call, P6CUtilLib1.GetExeSubsystem. The
                              method is designed so that GetLastError is skipped
                              when the value returned by GetExeSubsystem meets
                              the test in the IF statement that processes the
                              value returned by IP6CUtilLib1.GetExeSubsystem and
                              GetLastError would always return a meaningless
                              ERROR_SUCCESS.

    2015/07/10 5.5     DAG    Update this internal documentation, adding details
                              such as the namespace that were omitted, and make
                              the format consistent with that of the flower
                              boxes in the other modules of this library, and
                              make the registered event source stored in the
                              exception logger accessible through a read-only
                              property.

	2015/09/01 5.6     DAG    Correct syntax errors and other issues in the XML
                              documentation.

	2015/09/01 5.7     DAG    Add a very simple product name and version format.

	2015/10/18 5.8     DAG    1) GetStdHandleFQFN is a method that returns the
                                 absolute (fully qualified) file name to which a
                                 standard console stream is redirected, if any.

                              2) FormatSystemErrorMessage is a convenience
                                 method that constructs a Win32Exception
                                 exception, from which it returns the Message
                                 property, which is equivalent to calling the
                                 FormatMessage Windows API routine on the system
                                 status code.

                              3) StandardHandleState tests the subsystem type,
                                 and reports an error unless the code is running
                                 in the character mode subsystem. Adding this
                                 test makes it safe to leave this routine in
                                 in this DLL, although, strictly speaking, it
                                 should go into WizardWrx.ConsoleAppAids2, but
                                 keeping it here slightly decreases the overhead
                                 of the required platform invoke calls and
                                 marshaling of unmanaged resources.

                              4) GetAppSubsystemID is a method that returns the
                                 AppSubsystemID as a IP6CUtilLib1.PESubsystemID.

	2016/04/06 6.0     DAG    System.Reference.Assembly.GetCallingAssembly takes
                              the place of System.Reflection.GetEntryAssembly to
                              make the behavior of this class consistent whether
                              or not it runs under the Visual Studio Hosting
                              process.

    2016/04/08 6.0     DAG    Replace the generic (C++ oriented) implementation
						      with an implementation optimized for the .NET
							  Framework, which eliminates the need for double-
							  checked locking in the constructor.

    2016/06/03 6.2     DAG    Minor documentation cleanup; code is unchanged.

	2016/06/06 6.3     DAG    Private ExceptionLogger method ReportAsDirected 
							  was printing twice on standard output unless that
                              stream was redirected. To prevent a disastrous
                              recursive calling loop, a new instance method,
                              GetStandardHandleStates, gets the standard handle
                              states from the StateManager, which calls this
                              method once both objects have been constructed.

							  Wile working on the ExceptionLogger improvements,
                              I noticed several optimizations that could improve
							  not only its constructor, but this one, too. These
                              improvements are especially pertinent in both of
                              these cases, because they eliminate unnecessary
                              expensive trips across the managed/unmanaged code
                              boundary to gather information inaccessible via
                              managed code, and fairly complex to obtain, even
                              via the Windows API.
    ============================================================================
*/


using System;
using System.Text;

/*  Added by DAG */

using System.ComponentModel;
using System.Runtime.InteropServices;
using System.IO;

using WizardWrx;
using WizardWrx.DLLServices2;


namespace WizardWrx.DLLServices2
{
    /// <summary>
    /// This class maintains run-time information about the executing assembly,
    /// assumed to be a desktop application, that calls it into being. Since it
    /// implements the Singleton design pattern, there is never more than one
    /// instance.
    /// </summary>
    /// <remarks>
    /// This class has several read-write properties, all of which are protected
    /// by thread-safe accessors. Both the Get and Set methods are protected, so
    /// that a get request blocks until a set request executing in another thread
    /// completes.
    /// </remarks>
    public class StateManager : GenericSingletonBase<StateManager>
    {
        #region Public Constants and Enumerations
		/// <summary>
		/// Specify this constant to the ToString method of a System.Version
		/// object (e. g., of an Assembly) to cause it to return the entire
		/// version string.
		/// </summary>
		public const int ASSEMBLYVERSION_COMPLETE = 4;


		/// <summary>
		/// Specify this constant to the ToString method of a System.Version
		/// object (e. g., of an Assembly) to cause it to return all but the
		/// build number.
		/// </summary>
		public const int ASSEMBLYVERSION_EXCEPT_REVISION = 3;


		/// <summary>
		/// Specify this constant to the ToString method of a System.Version
		/// object (e. g., of an Assembly) to cause it to return only the
		/// major and minor version numbers.
		/// </summary>
		public const int ASSEMBLYVERSION_MAJOR_AND_MINOR = 2;


		/// <summary>
		/// Specify this constant to the ToString method of a System.Version
		/// object (e. g., of an Assembly) to cause it to return only the
		/// major version number.
		/// </summary>
		public const int ASSEMBLYVERSION_MAJOR_ONLY = 1;


		/// <summary>
		/// Use this enumeration as input to the overloaded StateManager class's
		/// GetAssemblyProductAndVersion method, to specify how many parts of
		/// the version string to return.
		/// </summary>
		public enum AssemblyVersionRequest
		{
			/// <summary>
			/// This is functionally equivalent to specifying a constant value of
			/// ASSEMBLYVERSION_MAJOR_ONLY to the ToString method of a
			/// System.Version object, and is intended to do so through the
			/// GetAssemblyProductAndVersion instance method of this class.
			/// </summary>
			MajorOnly = 1,

			/// <summary>
			/// This is functionally equivalent to specifying a constant value of
			/// ASSEMBLYVERSION_MAJOR_AND_MINOR to the ToString method of a
			/// System.Version object, and is intended to do so through the
			/// GetAssemblyProductAndVersion instance method of this class.
			/// </summary>
			MajorAndMinor = 2,

			/// <summary>
			/// This is functionally equivalent to specifying a constant value of
			/// ASSEMBLYVERSION_EXCEPT_REVISION to the ToString method of a
			/// System.Version object, and is intended to do so through the
			/// GetAssemblyProductAndVersion instance method of this class.
			/// 
			/// MajroMinorBuild and MajorMinorExceptRevision are synonyms.
			/// </summary>
			MajroMinorBuild = 3 ,

			/// <summary>
			/// This is functionally equivalent to specifying a constant value of
			/// ASSEMBLYVERSION_EXCEPT_REVISION to the ToString method of a
			/// System.Version object, and is intended to do so through the
			/// GetAssemblyProductAndVersion instance method of this class.
			/// 
			/// MajroMinorBuild and MajorMinorExceptRevision are synonyms.
			/// </summary>
			MajorMinorExceptRevision = 3 ,


			/// <summary>
			/// This is functionally equivalent to specifying a constant value of
			/// ASSEMBLYVERSION_COMPLETE to the ToString method of a
			/// System.Version object, and is intended to do so through the
			/// GetAssemblyProductAndVersion instance method of this class.
			/// 
			/// MajorMinroBuildRevision and Complete are synonyms.
			/// </summary>


			MajorMinroBuildRevision = 4 ,
			/// <summary>
			/// This is functionally equivalent to specifying a constant value of
			/// ASSEMBLYVERSION_COMPLETE to the ToString method of a
			/// System.Version object, and is intended to do so through the
			/// GetAssemblyProductAndVersion instance method of this class.
			/// 
			/// MajorMinroBuildRevision and Complete are synonyms.
			/// </summary>
			Complete = 4
		}	// AssemblyVersionRequest


		/// <summary>
		/// Since there is no hard and fast rule about the numbering of standard
		/// handles, whether attached or redirected, the Windows API routines
		/// abstract away the association. SHS_StandardHandleState adds another
		/// abstraction, in the form of an enumeration, which identifies the
		/// handle to query.
		/// 
		/// Rather than jump through hoops to figure out which of the three
		/// handles you want to query, I decided that asking you to identify it
		/// by specifying a member of this enumeration was not excessively
		/// onerous, since the answer reflects a design time decision.
		/// </summary>
		public enum ShsStandardHandle
		{
			/// <summary>
			/// // Value 0 is reserved for indicating that the value is uninitialized.
			/// </summary>
			SHSUndefined ,						

			/// <summary>
			/// Standard Input, also called STDIn, corresponds to Console.In.
			/// </summary>
			ShSStdIn ,

			/// <summary>
			/// Standard Output, also called STDOUT, corresponds to Console.Out.
			/// </summary>
			ShsStdOut ,

			/// <summary>
			/// Standard Error, also called STDERR, corresponds to Console.Error.
			/// </summary>
			ShsStdEror
		}	// ShsStandardHandle


		/// <summary>
		/// The DWLastDllError property returns this value when 
		/// StandardHandleState is called by code running in anything except the
		/// Windows character mode subsystem.
		/// </summary>
		public const int UNSUPPORTED_EXE_SUBSYSTEM = ( int ) MagicNumbers.APPLICATION_ERROR_MASK | 0x00000001;


		/// <summary>
		/// This enumeration maps the value returned by SHS_StandardHandleState
		/// to a value that ideally indicates whether a handle is attached to a
		/// console or redirected, while the remaining codes indicate error
		/// conditions.
		/// </summary>
		public enum ShsHandleState
		{
			/// <summary>
			/// Value = 0 indicates that ShsStandardHandle is either undefined
			/// or out of range.
			/// </summary>
			ShsUndeterminable ,

			/// <summary>
			/// Value = 1 indicates that the handle corresponding to the value
			/// of ShsStandardHandle is attached a console.
			/// </summary>
			ShsAttached ,

			/// <summary>
			/// Value = 2 indicates that the handle corresponding to the value
			/// of ShsStandardHandle is redirected to a file or pipe.
			/// </summary>
			ShsRedirected ,

			/// <summary>
			/// Value = 3 indicates that an internal error occurred. Call
			/// Marshal.GetLastWin32Error to learn why.
			/// </summary>
			ShsSystemError
		} // ShsHandleState 
		#endregion  // Public Constants and Enumerations


        #region Private Constants and Storage
        private bool _fShowUTCTime = true;

        private int _intAppReturnCode;											// This is implicitly initialized to MagicNumbers.ERROR_SUCCESS.
        private uint _uintAppSubsystemID;
		private static readonly string s_strInitialWorkingDirectoryName;
		private static string s_strMainAssemblyFQFN;
		private static SyncRoot s_srCriticalSection = new SyncRoot ( typeof ( StateManager ).ToString ( ) );

		private ExceptionLogger _pvtExceptionLogger;
		private static TimeDisplayFormatter s_fltConsoleMessageTimeFormat;

        private string [ ] _astrErrorMessages;
		private static DateTime s_dtmStartup;

		private static int s_dwLastError;										// This is implicitly initialized to MagicNumbers.ERROR_SUCCESS.
        #endregion  // Private Constants and Storage


        #region Lazy Singleton Constructors
		/// <summary>
		/// This method initializes all static members except the class, itself.
		/// </summary>
		static StateManager ( )
		{
			System.Diagnostics.Process sdpThisProcess = System.Diagnostics.Process.GetCurrentProcess ( );

			//	----------------------------------------------------------------
			//	The WorkingDirectory property of the StartInfo member of a
			//	System.Diagnostics.Process is empty unless the process starts
			//	via something like ShellExecute or a desktop shortcut that has a
			//	working directory. However, Environment.CurrentDirectory always
			//	has a value.
			//	----------------------------------------------------------------

			s_strInitialWorkingDirectoryName = String.IsNullOrEmpty ( sdpThisProcess.StartInfo.WorkingDirectory )
				? Environment.CurrentDirectory
				: sdpThisProcess.StartInfo.WorkingDirectory;
			s_fltConsoleMessageTimeFormat = new TimeDisplayFormatter ( );
			s_dtmStartup = sdpThisProcess.StartTime.ToUniversalTime ( );
		}	// private static StateManage constructor


		/// <summary>
		/// To prevent the framework from generating a default constructor and
		/// marking it public, which would break the singleton design pattern,
		/// we provide our own do-nothing constructor.
		/// </summary>
        private StateManager ( ) { }	// Prevent the framework from generating a public default constructor.


        /// <summary>
        /// Call this static method from anywhere in your code to get a
        /// reference to the StateManager singleton.
        /// </summary>
        /// <returns>
        /// The return value is a reference to the singleton, which is created
        /// the first time the method is called. Subsequent calls return a
        /// reference to the singleton.
        /// </returns>
		/// <remarks>
		/// Version 6.0.381.32883 of this class called static method
		/// System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName
		/// to obtain the name of the main module. However, the first test of
		/// this library as a dependent exposed a moderately surprising, but not
		/// completely unexpected, side effect of having the Visual Studio
		/// Hosting Process enabled; it becomes the MainModule of the process.
		/// Worse yet, enumerating the ModulesCollection lists all the DLLs, but
		/// the expected MainModule, the main assembly under test, is nowhere to
		/// be found!
		/// 
		/// Hence, beginning with version 6.0.389, this class reverts to a more
		/// reliable method, System.Reflection.Assembly.GetCallingAssembly(). To
		/// ensure that the optimizer doesn't inline it, although I find such an
		/// idea unlikely, the method is marked with a MethodImplAttribute,
		/// MethodImplOptions.Nogenerating the code inline. Since I don't have a
		/// Using directive, both must be fully qualified by System.Runtime.CompilerServices.
		///</remarks>
		[System.Runtime.CompilerServices.MethodImplAttribute ( System.Runtime.CompilerServices.MethodImplOptions.NoInlining )]
		public static StateManager GetTheSingleInstance ( )
        {	// This call MUST be made by a public entry point method, so that the calling assembly is set correctly.
			if ( string.IsNullOrEmpty ( s_strMainAssemblyFQFN ) )
			{	// If s_strMainAssemblyFQFN is null, so is everything else, and vice versa.
				s_strMainAssemblyFQFN = System.Reflection.Assembly.GetCallingAssembly ( ).Location;
				InitializeOnFirstUse (
					s_srCriticalSection ,
					s_genTheOnlyInstance );
			}	// if ( string.IsNullOrEmpty ( s_strMainAssemblyFQFN ) )

			return s_genTheOnlyInstance;
        }   // public static StateManager GetTheSingleInstance


		/// <summary>
		/// Call this method from any class that exposes a reference to the
		/// singleton as a read only property. Please see the Remarks section.
		/// </summary>
		/// <param name="pasmCallingAssembly">
		/// Pass in a reference to the assembly that is calling this method on
		/// behalf of an object that exposes a reference to this object as a
		/// read only property. Please see the Remarks section.
		/// </param>
		/// <returns>
		/// The return value is a reference to the singleton, which is created
		/// the first time the method is called. Subsequent calls return a
		/// reference to the singleton.
		/// </returns>
		/// <remarks>
		/// Since making a protected constructor safe for use by a class that
		/// implements the Singleton design pattern, and a class that calls this
		/// method causes System.Reflection.Assembly.GetCalllingAssembly to
		/// report the assembly in which the call executes as the calling
		/// assembly, any such method must explicitly identify the assembly that
		/// contains the process entry point.
		/// 
		/// When the Visual Studio Hosting Process is enabled, it becomes the
		/// entry assembly, hiding the "real" entry assembly (the assembly under
		/// test), System.Reflection.Assembly.GetEntryAssembly cannot be used
		/// safely. Moreover, since the Visual Studio Hosting Process runs in
		/// the graphical (Windows) subsystem, the entry assembly cannot be used
		/// to correctly determine whether the entry assembly runs in the
		/// character mode subsystem.
		/// 
		/// Although the class needs only the Location property off the Assembly
		/// instance, I elected to call for the entire Assembly to be passed in,
		/// to make the needs of this method unambiguous. Since the calling
		/// method must call one of two static methods on the Assembly class,
		/// both of which return instances of Assembly, this doesn't impose any
		/// extra burden on the calling assembly.
		/// </remarks>
		public static StateManager GetTheSingleInstance ( System.Reflection.Assembly pasmCallingAssembly )
		{
			if ( string.IsNullOrEmpty ( s_strMainAssemblyFQFN ) )
			{	// If s_strMainAssemblyFQFN is null, so is everything else, and vice versa.
				s_strMainAssemblyFQFN = pasmCallingAssembly.Location;
				InitializeOnFirstUse (
					s_srCriticalSection ,
					s_genTheOnlyInstance );
			}	// if ( string.IsNullOrEmpty ( s_strMainAssemblyFQFN ) )

			return s_genTheOnlyInstance;
		}   // public static StateManager GetTheSingleInstance
		#endregion  // Lazy Singleton Constructor


		#region Public Instance Methods
		/// <summary>
        /// Return the fully qualified name of the assembly that started the
        /// current process.
        /// </summary>
        /// <returns>
        /// The return value is a string that contains the fully qualified name
        /// of the assembly that started the current process.
        /// </returns>
        ///<remarks>
        /// I made this a method so that it is computed as needed, even in a
        /// debugging session when the Visual Studio Hosting process runs all
		/// the property getters when an object is expanded in the locals and
		/// watch windows.
		/// </remarks>
        public string GetAssemblyFQFN ( )
        {
			return s_strMainAssemblyFQFN;
        }   // public string GetAssemblyFQFN


		/// <summary>
		/// Get the assembly product name and version number is a string that is
		/// suitable for use as a window caption.
		/// </summary>
		/// <returns>
		/// The return value is a string, suitable for use as the caption of a
		/// window, such as a form or message box. The returned string contains
		/// the product name, followed by the entire 4-part version string.
		/// </returns>
		public string GetAssemblyProductAndVersion ( )
		{
			return GetAssemblyProductAndVersion ( AssemblyVersionRequest.Complete );
		}	// GetAssemblyProductAndVersion (1 of 2)


		/// <summary>
		/// Get the assembly product name and version number is a string that is
		/// suitable for use as a window caption.
		/// </summary>
		/// <param name="penmAssemblyVersionRequest">
		/// Use a member of the AssemblyVersionRequest enumeration to indicate
		/// how many parts of the four-part version number to include.
		/// </param>
		/// <returns>
		/// The return value is a string, suitable for use as the caption of a
		/// window, such as a form or message box. The returned string contains
		/// the product name, followed by the specified number of parts of the
		/// product version.
		/// </returns>
		public string GetAssemblyProductAndVersion ( AssemblyVersionRequest penmAssemblyVersionRequest )
		{
			const string ARGNAME_ASM_VER_REQ = @"penmAssemblyVersionRequest";

			//	---------------------------------------------------------------
			//	Though the ProcessModule class has no Version property, one can
			//	be constructed from its FileVersionInfo.FileVersion property.
			//
			//	Since both tokens are derived from properties of an instance of
			//	ProcessModule, one is constructed from the Process object
			//	returned by its static GetCurrentProcess method.
			//
			//	This slightly convoluted approach easily avoids breaking the
			//	GetAssemblyProductAndVersion interface.
			//	---------------------------------------------------------------

			System.Diagnostics.FileVersionInfo fviModuleVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo ( s_strMainAssemblyFQFN );
			StringBuilder sbVersion = new StringBuilder ( );

			switch ( penmAssemblyVersionRequest )
			{
				case AssemblyVersionRequest.MajorMinroBuildRevision:
					sbVersion.Append ( fviModuleVersion.ProductMajorPart );
					sbVersion.Append ( SpecialCharacters.FULL_STOP );
					sbVersion.Append ( fviModuleVersion.ProductMinorPart );
					sbVersion.Append ( SpecialCharacters.FULL_STOP );
					sbVersion.Append ( fviModuleVersion.ProductBuildPart );
					sbVersion.Append ( SpecialCharacters.FULL_STOP );
					sbVersion.Append ( fviModuleVersion.ProductPrivatePart );
					break;	// AssemblyVersionRequest.MajorMinroBuildRevision

				case AssemblyVersionRequest.MajorAndMinor:
					sbVersion.Append ( fviModuleVersion.ProductMajorPart );
					sbVersion.Append ( SpecialCharacters.FULL_STOP );
					sbVersion.Append ( fviModuleVersion.ProductMinorPart );
					break;	// AssemblyVersionRequest.MajorAndMinor

				case AssemblyVersionRequest.MajroMinorBuild:
					sbVersion.Append ( fviModuleVersion.ProductMajorPart );
					sbVersion.Append ( SpecialCharacters.FULL_STOP );
					sbVersion.Append ( fviModuleVersion.ProductMinorPart );
					sbVersion.Append ( SpecialCharacters.FULL_STOP );
					sbVersion.Append ( fviModuleVersion.ProductBuildPart );
					break;	// AssemblyVersionRequest.MajroMinorBuild

				case AssemblyVersionRequest.MajorOnly:
					sbVersion.Append ( fviModuleVersion.ProductMajorPart );
					break;	// AssemblyVersionRequest.MajorOnly

				default:
					throw new InvalidEnumArgumentException (
						ARGNAME_ASM_VER_REQ ,
						( int ) penmAssemblyVersionRequest ,
						penmAssemblyVersionRequest.GetType ( ) );
			}	// switch ( penmAssemblyVersionRequest )

			return string.Format (
				Properties.Resources.PRODUCT_NAME_AND_VERSION_TPL ,
				System.IO.Path.GetFileNameWithoutExtension ( s_strMainAssemblyFQFN ) ,
				sbVersion.ToString ( ) );
		}	// GetAssemblyProductAndVersion (2 of 2)


		/// <summary>
		/// Return the AppSubsystemID property, explicitly cast to a PESubsystemID.
		/// </summary>
		/// <returns>
		/// The return value is the AppSubsystemID property, cast to a member of
		/// the IP6CUtilLib1.PESubsystemID enumeration.
		/// </returns>
		/// <remarks>
		/// This method provides a more meaningful interpretation of the AppSubsystemID value.
		/// 
		/// This routine is implemented as an instance method, so that its code runs only as needed.
		/// </remarks>
		/// <seealso cref="AppSubsystemID"/>
		/// <seealso cref="GetAppSubsystemString"/>
		public IP6CUtilLib1.PESubsystemID GetAppSubsystemID ( )
		{
			return ( IP6CUtilLib1.PESubsystemID ) _uintAppSubsystemID;
		}	// GetAppSubsystemID


        /// <summary>
        /// Return a sentence that identifies the subsystem in which the 
        /// assembly that started the current process runs.
        /// </summary>
        /// <returns>
        /// The return value is a short sentence that describes the subsystem in
        /// which the assembly that started the current process runs.
        /// </returns>
        ///<remarks>
        /// I made this a method so that it is computed strictly as needed, even
        /// in a debugging session when the Visual Studio Hosting process runs
        /// all the property getters when an object is expanded in the locals or
        /// watch window.
        ///
        /// This method is much more expensive than GetAssemblyFQFN, because it
        /// gets the returned string from a string resource.
        ///</remarks>
        public string GetAppSubsystemString ( )
        {
            return IP6CUtilLib1.XlateProcessSubsystem ( _uintAppSubsystemID );
        }   // public string GetAppSubsystemString




        /// <summary>
        /// Load the table of error messages for use with the ErrorExit method.
        /// </summary>
        /// <param name="pastrErrorMessages">
        /// Array of strings, for use as error messages. See Remarks.
        /// </param>
        /// <returns>
        /// Count of error messages loaded into the object. Hence, the Ubound of
        /// the array is one less than the return value, and its LBound is zero.
        /// </returns>
        /// <remarks>
        /// Error message numbers are array subscripts. This imposes two
        /// requirements on the list.
        ///
        /// 1) You must supply a message for the default exit code of zero,
        /// even if you don't use it.
        ///
        /// 2) Status codes must be sequentially numbered.
        ///
        /// The second requirement can be relaxed, at the cost of inserting one
        /// or more dummy error messages, which may be empty strings, to account
        /// for the skipped numbers. The obvious disadvantage is some bloat in
        /// the message table.
        ///
        /// You must call this method before your program can get a useful error
        /// message from ErrorExit. Although the call to ErrorExit succeeds, the
        /// message is "Internal error: Unknown status code," followed by the
        /// supplied raw exit code.
        /// </remarks>
        public int LoadErrorMessageTable ( string [ ] pastrErrorMessages )
        {
            _astrErrorMessages = pastrErrorMessages;
            return _astrErrorMessages.Length;
        }   // public int LoadErrorMessageTable
        #endregion  // Public Instance Methods


        #region Properties
        /// <summary>
        /// Make the array of error messages available, for reading only, under
        /// guard of a lock, to ensure that another thread isns't adding to the
        /// array while it is being returned.
        /// </summary>
        public string [ ] AppErrorMessages
        { 
            get
            {
				lock ( s_srCriticalSection )
                    return _astrErrorMessages;
			}   // public string [ ] AppErrorMessages property Get method
        }   // public string [ ] AppErrorMessages property


        /// <summary>
        /// Use this read/write property to set the application return code from
        /// anywhere, so that you don't have to use another, potentially awkward,
        /// method to get the word back to the main routine.
        /// </summary>
        /// <remarks>
        /// Access to this property is synchronized by an internally managed
        /// object lock.
        /// </remarks>
        public int AppReturnCode
        {
            get
            {
				lock ( s_srCriticalSection )
                    return _intAppReturnCode;
            }   // public int AppReturnCode Get method

            set
            {
				lock ( s_srCriticalSection )
                    _intAppReturnCode = value;
            }   // public int AppReturnCode Set method
        }   // public int AppReturnCode property


        /// <summary>
        /// This property has a static initializer, and is never changed. Use it
        /// to get a reference to a reflection-only copy of the application
        /// startup assembly.
        /// </summary>
        /// <remarks>
        /// The AppRootAssemblyFileBaseName, AppRootAssemblyFileDirName,
        /// AppRootAssemblyFileName, AppStartupTimeLocal, and
        /// AppStartupTimeLocal properties are shortcuts to frequently used
        /// properties of the WizardWrx.My object returned by this property.
        /// </remarks>
		[Obsolete ( "Call System.Reflection.Assembly.GetEntryAssembly directly." )]
		public System.Reflection.Assembly AppRootAssembly
		{
			get
			{
				return System.Reflection.Assembly.GetEntryAssembly ( );
			}	// public System.Reflection.Assembly AppRootAssembly property Get method
		}	// public System.Reflection.Assembly AppRootAssembly property


        /// <summary>
        /// This property is a shortcut to the AssemblyFileBaseName property of
        /// the AppRootAssembly property.
        /// </summary>
        /// <remarks>
        /// Since the underlying property is read only, and has a static
        /// initializer, it is thread-safe without the overhead of a lock.
        /// </remarks>
		public string AppRootAssemblyFileBaseName
		{
			get
			{	// The computational burden should be fairly light.
				return Path.GetFileNameWithoutExtension ( s_strMainAssemblyFQFN );
			}	// public string AppRootAssemblyFileBaseName property Get method
		}	// public string AppRootAssemblyFileBaseName property


        /// <summary>
        /// This property is a shortcut to the AssembyDirName property of
        /// the AppRootAssembly property.
        /// </summary>
        /// <remarks>
        /// Since the underlying property is read only, and has a static
        /// initializer, it is thread-safe without the overhead of a lock.
        /// </remarks>
		public string AppRootAssemblyFileDirName
		{
			get
			{
				return Path.GetDirectoryName ( s_strMainAssemblyFQFN );
			}	// public string AppRootAssemblyFileDirName Get method
		}	// public string AppRootAssemblyFileDirName property


        /// <summary>
        /// This property is a shortcut to the AssemblyFileName property of
        /// the AppRootAssembly property.
        /// </summary>
        /// <remarks>
        /// Since the underlying property is read only, and has a static
        /// initializer, it is thread-safe without the overhead of a lock.
        /// </remarks>
		public string AppRootAssemblyFileName
		{
			get
			{
				return Path.GetFileNameWithoutExtension ( s_strMainAssemblyFQFN );
			}	// public string AppRootAssemblyFileName property Get method
		}	// public string AppRootAssemblyFileName property


        /// <summary>
        /// This property is a shortcut to the StartupTime property of
        /// the AppRootAssembly property.
        /// </summary>
        /// <remarks>
        /// Since the underlying property is read only, and has a static
        /// initializer, it is thread-safe without the overhead of a lock.
        /// </remarks>
		public DateTime AppStartupTimeLocal
		{
			get
			{
				return StateManager.s_dtmStartup.ToLocalTime ( );
			}	// public DateTime AppStartupTimeLocal property Get method
		}	// public DateTime AppStartupTimeLocal property


        /// <summary>
        /// This property is a shortcut to the StartupTimeUTC property of
        /// the AppRootAssembly property.
        /// </summary>
        /// <remarks>
        /// Since the underlying property is read only, and has a static
        /// initializer, it is thread-safe without the overhead of a lock.
        /// </remarks>
		public DateTime AppStartupTimeUtc
		{
			get
			{
				return s_dtmStartup;
			}	// public DateTime AppStartupTimeUtc property Get method
		}	// public DateTime AppStartupTimeUtc property

		
		/// <summary>
		/// Gets the elapsed time since the application started.
		/// </summary>
		public TimeSpan AppUptime
		{
			get
			{
				return DateTime.UtcNow - s_dtmStartup;
			}	// public TimeSpan AppUptime property Get method
		}	// public TimeSpan AppUptime property


        /// <summary>
        /// Return the ID of the subsystem in which the assembly runs. Call the
        /// GetAppSubsystemString method to translate the ID into a human
        /// readable message. The translation is way too expensive to expose as
        /// a property, and too infrequently needed to justify having the
        /// constructor store it in a string.
        /// </summary>
		/// <seealso cref="GetAppSubsystemID"/>
		/// <seealso cref="GetAppSubsystemString"/>
		public uint AppSubsystemID
		{
			get
			{
				return _uintAppSubsystemID;
			}	// public AppSubsystemID property Get method
		}	// public AppSubsystemID property


        /// <summary>
        /// Use this property to get a reference to the FormattedLocalTime
        /// object that governs the formatting of the times displayed in the BOJ
        /// and EOJ messages.
        /// </summary>
        public TimeDisplayFormatter ConsoleMessageTimeFormat
        {
            get { return s_fltConsoleMessageTimeFormat; }
        }   // public FormattedLocalTime ConsoleMessageTimeFormat ReadOnly property


		/// <summary>
		/// Gets the registered default event source ID.
		/// </summary>
		/// <remarks>
		/// If no event source is registered, this property returns the 
		/// WIZARDWRX_EVENT_SOURCE_ID string static ExceptionLogger string.
		/// </remarks>
		public string DefaultEventSourceID
		{
			get
			{
				return string.IsNullOrEmpty ( _pvtExceptionLogger.AppEventSourceID )
					? ExceptionLogger.WIZARDWRX_EVENT_SOURCE_ID :
					_pvtExceptionLogger.AppEventSourceID;
			}	// public string DefaultEventSourceID Property Get method
		}	// public string DefaultEventSourceID Property


        /// <summary>
        /// Gets the initial working directory, which reflects its value when
		/// the application started.
        /// </summary>
		/// <remarks>
		/// This property has a static initializer, and is never changed.
		/// </remarks>
        public string InitialWorkingDirectoryName
        {   // This property is read only.
            get { return s_strInitialWorkingDirectoryName; }
        }   // public string InitialWorkingDirectoryName property


		/// <summary>
		/// Query this property for the value returned by GetLastError.
		/// </summary>
		/// <remarks>
		/// Strictly speaking, the value is returned by Marshal.GetLastWin32Error.
		/// </remarks>
		public static int DWLastDllError
		{
			get
			{
				return s_dwLastError;
			}	// public static int DWLastDllError Get Method
		}	// public static int DWLastDllError Property


        /// <summary>
        /// This property returns a reference to the singleton ExceptionLogger,
        /// which is created immediately following creation of this instance.
        /// Hence, by the time the reference is returned, this reference is
        /// guaranteed to be valid and read only for work.
        /// </summary>
        public ExceptionLogger AppExceptionLogger { get { return _pvtExceptionLogger; } }


        /// <summary>
        /// Set this property to TRUE to cause UTC times to be displayed in all
        /// time stamps included in console BOJ and EOJ messages.
        ///
        /// Set it to FALSE to display only local times. The class default
        /// setting is TRUE.
        ///
        /// See the Remarks listed with the class definition.
        /// </summary>
        public bool ShowUTCTime
        {
            get
            {
				lock ( s_srCriticalSection )
                    return _fShowUTCTime;
            }   // ShowUTCTime reader method

            set
            {
				lock ( s_srCriticalSection )
                    _fShowUTCTime = value;
			}   // ShowUTCTime writer method
        }   // ShowUTCTime property
        #endregion  // Properties


		#region Public Instance Methods
		/// <summary>
		/// Report the file, if any, to which the specified standard handle is
		/// redirected.
		/// </summary>
		/// <param name="penmStdHandle">
		/// Specify the ShsStandardHandle member that corresponds to the handle
		/// for which you wish to know whether it is attached or redirected.
		/// </param>
		/// <returns>
		/// If the function succeeds and the handle is redirected, the return
		/// value is the name of the file to which the specified handle is
		/// redirected.
		/// 
		/// Otherwise, the return value is one of two strings that explain the
		/// error.
		/// 
		/// Properties.Resources.MSG_APP_CHARACTER_MODE_APPS_ONLY is the 
		/// complete text of the message displayed when the calling routine is
		/// running in the Windows graphical mode subsystem.
		/// 
		/// Properties.Resources.ERRMSG_STD_HANDLE_STATE is the beginning of a
		/// message, which is followed by the returned system status code, shown
		/// in both hexadecimal and decimal notation, followed by the associated
		/// system message.
		/// </returns>
		public string GetStdHandleFQFN ( ShsStandardHandle penmStdHandle )
		{
			switch ( StandardHandleState ( penmStdHandle ) )
			{
				case ShsHandleState.ShsRedirected:
					return IP6CUtilLib1.GetStdHandleFNCLI ( ( uint ) penmStdHandle );
				case ShsHandleState.ShsAttached:
					return Properties.Resources.MSG_CONSOLE_HAS_STD_HANDLE;
				default:
					if ( s_dwLastError == UNSUPPORTED_EXE_SUBSYSTEM )
					{	// Console mode callers only, please!
						return Properties.Resources.MSG_APP_CHARACTER_MODE_APPS_ONLY;
					}	// TRUE (most likely outcome) block, if ( _dwLastError ==  UNSUPPORTED_EXE_SUBSYSTEM)
					else
					{	// Something completely unexpected happened.
						return string.Format (
							Properties.Resources.ERRMSG_STD_HANDLE_STATE ,
							s_dwLastError.ToString ( DisplayFormats.HEXADECIMAL_8 ) ,
							s_dwLastError.ToString ( DisplayFormats.NUMBER_PER_REG_SETTINGS_0D ) ,
							FormatSystemErrorMessage ( s_dwLastError ) );
					}	// FALSE (least likely outcome) block, if ( _dwLastError ==  UNSUPPORTED_EXE_SUBSYSTEM)
			}	// switch ( StandardHandleState ( penmStdHandle ) )
		}	// GetStdHandleFQFN


		/// <summary>
		/// Evaluate whether a standard handle is attached to a console or 
		/// redirected.
		/// </summary>
		/// <param name="penmStdHandle">
		/// Specify the ShsStandardHandle member that corresponds to the handle
		/// for which you wish to know whether it is attached or redirected.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is either ShsAttached or
		/// ShsRedirected. Otherwise, there was an error, and you should call
		/// Marshal.GetLastWin32Error to determine exactly what happened. The 
		/// singleton has a private property where the return value is stored.
		/// </returns>
		public ShsHandleState StandardHandleState
			( ShsStandardHandle penmStdHandle )
		{
			if ( ( IP6CUtilLib1.PESubsystemID ) this._uintAppSubsystemID == IP6CUtilLib1.PESubsystemID.IMAGE_SUBSYSTEM_WINDOWS_CUI )
			{
				//	------------------------------------------------------------
				//	Since they are immune to the effects of the Visual Studio
				//	Hosting Process, the routines that call into my unmanaged
				//	DLLs carry the day.
				//	------------------------------------------------------------

				switch ( ( ShsHandleState ) SafeNativeMethods.SHS_StandardHandleState ( ( uint ) penmStdHandle ) )
				{
					case ShsHandleState.ShsAttached:
						return ShsHandleState.ShsAttached;
					case ShsHandleState.ShsRedirected:
						return ShsHandleState.ShsRedirected;
					case ShsHandleState.ShsUndeterminable:
						s_dwLastError = Marshal.GetLastWin32Error ( );
						return ShsHandleState.ShsUndeterminable;
					default:
						s_dwLastError = Marshal.GetLastWin32Error ( );
						return ShsHandleState.ShsSystemError;
				}	// switch ( ( ShsHandleState ) SafeNativeMethods.SHS_StandardHandleState ( ( uint ) penmStdHandle ) )
			}	// TRUE (anticipated outcome) block, if ( IP6CUtilLib1.GetExeSubsystemID ( System.Reflection.Assembly.GetEntryAssembly ( ).Location ) == IP6CUtilLib1.PESubsystemID.IMAGE_SUBSYSTEM_WINDOWS_CUI )
			else
			{
				s_dwLastError = StateManager.UNSUPPORTED_EXE_SUBSYSTEM;
				return ShsHandleState.ShsSystemError;
			}	// FALSE (unanticipated outcome) block, if ( IP6CUtilLib1.GetExeSubsystemID ( System.Reflection.Assembly.GetEntryAssembly ( ).Location ) == IP6CUtilLib1.PESubsystemID.IMAGE_SUBSYSTEM_WINDOWS_CUI )
		}	// public static ShsHandleState StandardHandleState
		#endregion	// Public Static Methods


		#region Public Static Methods
		/// <summary>
		/// Return the message associated with a system status code.
		/// </summary>
		/// <param name="pdwLastError">
		/// Specify the status code for which the system message is desired.
		/// </param>
		/// <returns>
		/// A new Win32Exception exception is constructed from the specified
		/// status code, and its Message property is returned.
		/// </returns>
		/// <remarks>
		/// This convenience method amounts to syntactic sugar. Since it is very
		/// unlikely to ever be called by the method that motivated me to create
		/// it, I'll let it go ahead and call through it, even though calling
		/// the constructor directly, passing in the static member that holds the
		/// status code would be a couple of orders of magnitude more efficient.
		/// </remarks>
		public static string FormatSystemErrorMessage ( int pdwLastError )
		{
			return new Win32Exception ( pdwLastError ).Message;
		}	// FormatSystemErrorMessage
		#endregion	// Public Static Methods


		#region Private Static Methods
		[System.Diagnostics.CodeAnalysis.SuppressMessage ( "Microsoft.Security" , "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands" ) ,
			System.Diagnostics.CodeAnalysis.SuppressMessage ( "Microsoft.Interoperability" , "CA1404:CallGetLastErrorImmediatelyAfterPInvoke" )]
		private static void InitializeOnFirstUse (
            SyncRoot plwlMaster ,
            StateManager ptsfThis )
        {
            //  ----------------------------------------------------------------
			//	Since the optimized implementation of the Singleton design
			//	pattern uses a static constructor to create the single instance,
			//	the StateManager is guaranteed to exist on entry.
			//
            //  Following construction, perform the expensive property
            //  initializations that require unmanaged code, but skip it on any
			//	subsequent calls.
            //  ----------------------------------------------------------------

			if ( ptsfThis._pvtExceptionLogger == null )
			{
				if ( ( ptsfThis._uintAppSubsystemID = WizardWrx.DLLServices2.IP6CUtilLib1.GetExeSubsystem ( s_strMainAssemblyFQFN ) ) > IP6CUtilLib1.SUBSYSTEM_UNKNOWN )
				{	// Create the ExceptionLogger and initialize its subsystem ID.
					ptsfThis._pvtExceptionLogger = ExceptionLogger.GetTheSingleInstance ( ( ExceptionLogger.ProcessSubsystem ) ptsfThis._uintAppSubsystemID );
					ptsfThis._pvtExceptionLogger.GetStandardHandleStates ( ptsfThis );
				}   // TRUE (expected outcome) block, if ( ptsfThis._uintAppSubsystemID > IP6CUtilLib1.SUBSYSTEM_UNKNOWN )
				else
				{	// Since GetExeSubsystem failed, raise a Win32Exception exception, effectively failing the constructor.
					int dwLastError = Marshal.GetLastWin32Error ( );	// Integer dwLastError earns its keep because it is used twice as a method argument.
					Win32Exception w32Exception = new Win32Exception ( dwLastError );
					string strMsg = string.Format (
						Properties.Resources.ERRMSG_ASM_SUBSYSTEM ,		// Message template
						ptsfThis.GetAssemblyFQFN ( ) ,					// Token 0
						Marshal.GetLastWin32Error ( ) ,					// Token 1
						w32Exception );									// Token 2
					throw new Exception (
						strMsg ,
						w32Exception );
				}   // FALSE (unexpected outcome) block, if ( ptsfThis._uintAppSubsystemID > IP6CUtilLib1.SUBSYSTEM_UNKNOWN )
			}	// if ( ptsfThis._pvtExceptionLogger == null )
		}   // private static void InitializeOnFirstUse
        #endregion  // Private Static Methods
    }   // public class StateManager
}   // partial namespace WizardWrx.DLLServices2