/*
    ============================================================================

    Namespace:          ConsoleStreamsLab

    Class Name:			Program

	File Name:			Program.cs

    Synopsis:			This class defines the entry point routine and most of
						its subroutines.

    Remarks:			After some debate, I chose to import TraceLogger.cs,
						rather than import WizardWrx.Core.dll, to keep this code
						as lightweight as possible.

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

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version Author Synopsis
    ---------- ------- ------ --------------------------------------------------
	2017/04/04 1.0	   DAG    Work began on this program.
	2017/04/07 1.0	   DAG    Program was completed and marked as final.
	2017/07/06 1.1	   DAG    Program was completed and marked as final.
	2017/07/09 1.2	   DAG    Program used to develop and test a 100% managed
							  successor to the existing console handle queries.

                              They are 100% managed, that is, if platform invoke
                              is allowed.
    ============================================================================
*/


using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;

using Microsoft.Win32.SafeHandles;

using WizardWrx;
using WizardWrx.Core;

namespace ConsoleStreamsLab
{
	class Program
	{
		enum OptionalStreamFeatures
		{
			Length ,
			Position ,
			ReadTimeout ,
			WriteTimeout
		}	// OptionalFeatures

		enum StandardOutputStream
		{
			Output ,
			Error
		}	// StandardOutputStream

		static readonly string [ ] s_astrStandardHandleNames =
		{
			"SHS_UNDEFINED (0) - Undefined" ,									// Value = 0, which is reserved for indicating that the value is uninitialized.
			"SHS_INPUT (1) - STDIN (a. k. a. Standard Input)" ,					// Value = 1, which corresponds to STD_INPUT_HANDLE
			"SHS_OUTPUT (2) - STDOUT (a. k. a. Standard Output)" ,				// Value = 2, which corresponds to STD_OUTPUT_HANDLE
			"SHS_ERROR (3) - STDERR (a. k. a. Standard Error)" 					// Value = 3, which corresponds to STD_ERROR_HANDLE
		};	// SHS_STANDARD_HANDLE

		static readonly string [ ] s_astrStdHandleState =
		{
			"SHS_UNDETERMINABLE (0) - cannot be determined" ,					// Value = 0, indicating that SHS_STANDARD_HANDLE is either SHS_UNDEFINED or out of range
			"SHS_ATTACHED (1) - attached to its console" ,						// Value = 1, indicating that the handle corresponding to the value of SHS_STANDARD_HANDLE is attached to its console
			"SHS_REDIRECTED (2) - redirected to a file",						// Value = 2, indicating that the handle corresponding to the value of SHS_STANDARD_HANDLE is redirected
			"SHS_SYSTEM_ERROR (3) - cannot be determined due to a system error"	// Value = 3, indicating that an internal error occurred. Call GetLastError to learn why.
		};	// SHS_HANDLE_STATE

		static readonly DateTime s_dtmStart = DateTime.UtcNow;

		static void Main ( string [ ] pastrArguments )
		{
			string strBanner = CreateStartupBanner ( );

			Console.WriteLine ( strBanner );
			TraceLogger.WriteWithBothTimesUnlabeledUTCFirst ( strBanner );

			Process thisProcess = Process.GetCurrentProcess ( );

			if ( pastrArguments.Length > ListInfo.LIST_IS_EMPTY && pastrArguments [ ArrayInfo.ARRAY_FIRST_ELEMENT ].ToLower ( ) == Properties.Resources.ENUMERATE_PROCESS_MODULES )
			{
				EnumerateProcessModules ( thisProcess );
			}	// if ( pastrArguments.Length > ListInfo.LIST_IS_EMPTY && pastrArguments [ ArrayInfo.ARRAY_FIRST_ELEMENT ].ToLower ( ) == Properties.Resources.ENUMERATE_PROCESS_MODULES )

			if ( Debugger.IsAttached )
			{
				Console.WriteLine (
					Properties.Resources.MSG_STARTED_IN_DEBUGGER ,
					Environment.NewLine );
			}	// TRUE (The application started in the Visual Studio debugger.) block, if ( Debugger.IsAttached )
			else
			{
				Console.WriteLine (
					Properties.Resources.MSG_STARTED_IN_CONSOLE , 
					Environment.NewLine );
				Debugger.Launch ( );
			}	// FALSE (The application started in a console window, and must launch and attach a debugger.) block, if ( Debugger.IsAttached )

			try
			{
				Console.WriteLine (
					"{3}The Process class reports as follows:{3}    Standard Input  = {0}{3}    Standard Output = {1}{3}    Standard Error  = {2}{3}{3}" ,
					new object [ ]
					{
						thisProcess.StartInfo.RedirectStandardInput ,
						thisProcess.StartInfo.RedirectStandardOutput ,
						thisProcess.StartInfo.RedirectStandardError ,
						Environment.NewLine } );

#if NET47
				Console.WriteLine ( "The Console singleton reports as follows: Standard Input Redirected  = {0}" , Console.IsInputRedirected );
				Console.WriteLine ( "                                          Standard Output Redirected = {0}" , Console.IsOutputRedirected );
				Console.WriteLine ( "                                          Standard Error Redirected  = {0}" , Console.IsErrorRedirected  )l
#endif	// #if NET47

				ShowHandleInfo ( StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_INPUT );
				ShowHandleInfo ( StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_OUTPUT );
				ShowHandleInfo ( StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_ERROR );
			}
			catch ( Exception exAll )
			{
				string strMsg = exAll.ToString ( );
				Console.WriteLine ( strMsg );
				TraceLogger.WriteWithBothTimesUnlabeledUTCFirst ( strMsg );
				Environment.ExitCode = WizardWrx.MagicNumbers.ERROR_RUNTIME;
			}

			strBanner = CreateShutdownBanner ( );
			Console.WriteLine ( strBanner );
			TraceLogger.WriteWithBothTimesUnlabeledUTCFirst ( strBanner );
			AwaitCarbonUnit ( );
		}	// 	// static void Main


		private static void EnumerateProcessModules ( Process pthisProcess )
		{
			ProcessModuleCollection theseModules = pthisProcess.Modules;

			Console.WriteLine ( "Process Details: Id                 = {0}" , pthisProcess.Id );
			Console.WriteLine ( "                 StartTime          = {0}" , pthisProcess.StartTime );
			Console.WriteLine ( "                 TotalProcessorTime = {0}" , pthisProcess.TotalProcessorTime );
			Console.WriteLine ( "                 UserProcessorTime  = {0}" , pthisProcess.UserProcessorTime );
			Console.WriteLine ( "                 Modules Count      = {0}" , theseModules.Count );

			int intModuleOrdinal = ListInfo.LIST_IS_EMPTY;

			foreach ( ProcessModule thisModule in theseModules )
			{
				Console.WriteLine ( "      Module {0,2}: ModuleName = {1}" , ++intModuleOrdinal , thisModule.ModuleName );
				Console.WriteLine ( "                 FileName     = {0}" , thisModule.FileName );
				Console.WriteLine ( "                 Base Address = 0x{0}{1}" , thisModule.BaseAddress.ToInt64 ( ).ToString ( NumericFormats.HEXADECIMAL_16 ) , Environment.NewLine );

				thisModule.Dispose ( );											// ProcessModule is a Disposable resource.
			}	// foreach ( Module thisModule in theseModules )
		}	// private static void EnumerateProcessModules


		private static void AwaitCarbonUnit ( )
		{
			Console.Error.WriteLine ( Properties.Resources.MSG_AWAIT_CARBON_UNIT );
			Console.ReadLine ( );
		}	// private static void AwaitCarbonUnit ( )


		private static string CreateShutdownBanner ( )
		{
			AssemblyName anTheApp = GetAppAssemblyName ( );
			DateTime dtmStopping = DateTime.UtcNow;
			TimeSpan tsRunning = dtmStopping - s_dtmStart;

			return string.Format (
				Properties.Resources.MSG_STOP ,				// The format control string contains five substitution tokens.
				new object [ ]								// Since there are more than three, the format items go into a parameter array.
				{
					anTheApp.Name ,							// Format Item 0 = Program Name
					dtmStopping.ToLocalTime ( ) ,			// Format Item 1 = Local Program Ending Time
					dtmStopping ,							// Format Item 2 = UTC Program Ending Time
					tsRunning ,								// Format Item 3 = Running time
					Environment.NewLine } );				// Format Item 4 = Embedded Newline
		}	// private static string CreateShutdownBanner ( )


		private static string CreateStartupBanner ( )
		{
			AssemblyName anTheApp = GetAppAssemblyName ( );

			return string.Format (
				Properties.Resources.MSG_START ,			// The format control string contains six substitution tokens.
				new object [ ]								// Since there are more than three, the format items go into a parameter array.
				{
					anTheApp.Name ,							// Format Item 0 = Program Name
					anTheApp.Version.Major ,				// Format Item 1 = Major Version Number
					anTheApp.Version.Minor ,				// Format Item 2 = Minor Version Number
					s_dtmStart.ToLocalTime ( ) ,			// Format Item 3 = Local Startup Time
					s_dtmStart ,							// Format Item 4 = UTC Startup Time
					Environment.NewLine						// Format Item 5 = Embedded Newline
				} );
		}	// private static string CreateStartupBanner


		private static AssemblyName GetAppAssemblyName ( )
		{
			return GetAppEntryAssembly ( ).GetName ( );
		}	// private static AssemblyName GetAppAssemblyName


		private static Assembly GetAppEntryAssembly ( )
		{
			if ( AppDomain.CurrentDomain.DomainManager == null )
			{	// Using the EntryAssembly returned by the static Reflection method is safe.
				return Assembly.GetEntryAssembly ( );
			}	// TRUE (most probable outcome in production) block, if ( AppDomain.CurrentDomain.DomainManager == null )
			else
			{	// The static EntryAssembly method returns the method that owns the domain manager, but the domain manager knows about the correct assembly.
				return AppDomain.CurrentDomain.DomainManager.EntryAssembly;
			}	// FALSE (probable outcome in debugging) block, if ( AppDomain.CurrentDomain.DomainManager == null )
		}	// private static Assembly GetAppEntryAssembl


		private static void ShowHandleInfo ( StandardHandleInfo.SHS_STANDARD_HANDLE penmStandardHandle )
		{
			StandardHandleInfo.SHS_HANDLE_STATE enmStandardHandleState = StandardHandleInfo.SHS_StandardHandleState ( penmStandardHandle );

			switch ( enmStandardHandleState )
			{
				case StandardHandleInfo.SHS_HANDLE_STATE.SHS_ATTACHED:
					Console.WriteLine (
						"The {0} console handle is {1}." ,
						s_astrStandardHandleNames [ ( int ) penmStandardHandle ] ,
						s_astrStdHandleState [ ( int ) enmStandardHandleState ] );
					break;	// case SHS_ATTACHED

				case StandardHandleInfo.SHS_HANDLE_STATE.SHS_REDIRECTED:
					Console.WriteLine (
						"The {0} console handle is {1}." ,
						s_astrStandardHandleNames [ ( int ) penmStandardHandle ] ,
						s_astrStdHandleState [ ( int ) enmStandardHandleState ] );
					Console.WriteLine (
						"    File Name = {0}" ,
						StandardHandleInfo.GetRedirectedFileName ( penmStandardHandle ) );
					break;	// case SHS_REDIRECTED

				case StandardHandleInfo.SHS_HANDLE_STATE.SHS_SYSTEM_ERROR:
				case StandardHandleInfo.SHS_HANDLE_STATE.SHS_UNDETERMINABLE:
					Console.WriteLine (
						"The {0} console handle is {1}." ,
						s_astrStandardHandleNames [ ( int ) penmStandardHandle ] ,
						s_astrStdHandleState [ ( int ) enmStandardHandleState ] );
					break;	// cases SHS_SYSTEM_ERROR and SHS_UNDETERMINABLE

				default:
					throw new System.ComponentModel.InvalidEnumArgumentException (
						"enmStandardHandleInfo" ,
						( int ) enmStandardHandleState ,
						enmStandardHandleState.GetType ( ) );
			}	// switch ( enmStandardHandleInfo )
		}	// ShowHandleInfo


		private static void ShowHandleInfo (
			TextReader ptrStandardInputHandle ,
			string pstrHandleLabel )
		{
			Console.WriteLine (
				Properties.Resources.MSG_STD_HANDLE_BEGIN ,	// Format Control String
				pstrHandleLabel ,							// Format Item 0 = Label
				Environment.NewLine );						// Format Item 1 = Embedded Newline

			//	----------------------------------------------------------------
			//	Display the properties of the input object.
			//	----------------------------------------------------------------

			Console.WriteLine ( "        Type: FullName                       = {0}" , ptrStandardInputHandle.GetType ( ).FullName );
			Console.WriteLine ( "        Type: AssemblyQualifiedName          = {0}" , ptrStandardInputHandle.GetType ( ).AssemblyQualifiedName );
			Console.WriteLine ( "        Type: Assembly.FullName              = {0}" , ptrStandardInputHandle.GetType ( ).Assembly.FullName );
			Console.WriteLine ( "        Type: Assembly.Location              = {0}" , ptrStandardInputHandle.GetType ( ).Assembly.Location );
			Console.WriteLine ( "        String Representation                = {0}{1}" , ptrStandardInputHandle.ToString ( ) , Environment.NewLine );

			//	---------------------------------------------------------------------------
			//	Downcast to StreamReader, and examine its properties.
			//
			//	Reference:	Console.OpenStandardInput Method (Visual Studio 2008)
			//				https://msdn.microsoft.com/en-us/library/tx55zca2(v=vs.90).aspx
			//	---------------------------------------------------------------------------

			Stream baseStream = Console.OpenStandardInput ( );

			Console.WriteLine ( "        StreamReader.BaseStream.CanRead      = {0}" , baseStream.CanRead );
			Console.WriteLine ( "        StreamReader.BaseStream.CanSeek      = {0}" , baseStream.CanSeek );
			Console.WriteLine ( "        StreamReader.BaseStream.CanTimeout   = {0}" , baseStream.CanTimeout );
			Console.WriteLine ( "        StreamReader.BaseStream.CanWrite     = {0}{1}" , baseStream.CanWrite , Environment.NewLine );

			Console.WriteLine ( "        StreamReader.BaseStream.Length       = {0}" , ShowIfSupported ( baseStream , OptionalStreamFeatures.Length ) );
			Console.WriteLine ( "        StreamReader.BaseStream.Position     = {0}" , ShowIfSupported ( baseStream , OptionalStreamFeatures.Position ) );
			Console.WriteLine ( "        StreamReader.BaseStream.ReadTimeout  = {0}" , ShowIfSupported ( baseStream , OptionalStreamFeatures.ReadTimeout ) );
			Console.WriteLine ( "        StreamReader.BaseStream.WriteTimeout = {0}" , ShowIfSupported ( baseStream , OptionalStreamFeatures.ReadTimeout ) );
			Console.WriteLine ( "        StreamReader.BaseStream.WriteTimeout = {0}" , ShowIfSupported ( baseStream , OptionalStreamFeatures.ReadTimeout ) );

			StandardHandleInfo.SHS_HANDLE_STATE enmStandardHandleInfo = StandardHandleInfo.SHS_StandardHandleState (
				StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_INPUT );

			switch ( enmStandardHandleInfo )
			{
				case StandardHandleInfo.SHS_HANDLE_STATE.SHS_ATTACHED:
					Console.WriteLine (
						"The {0} console handle is attached to its console." ,
						s_astrStandardHandleNames [ ( int ) StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_INPUT ] );
					break;
				case StandardHandleInfo.SHS_HANDLE_STATE.SHS_REDIRECTED:
					Console.WriteLine ( "The {0} console handle is redirected to a file." ,
						s_astrStandardHandleNames  [ ( int ) StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_INPUT ] );
					break;
				case StandardHandleInfo.SHS_HANDLE_STATE.SHS_SYSTEM_ERROR:
					Console.WriteLine ( "The {0} console handle state cannot be determined because the attempt provoked a system error." ,
						s_astrStandardHandleNames [ ( int ) StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_INPUT ] );
					break;
				case StandardHandleInfo.SHS_HANDLE_STATE.SHS_UNDETERMINABLE:
					Console.WriteLine ( "The {0} console handle state cannot be determined because a coding error interfered." ,
						s_astrStandardHandleNames [ ( int ) StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_INPUT ] );
					break;
				default:
					throw new System.ComponentModel.InvalidEnumArgumentException (
						"enmStandardHandleInfo" ,
						( int ) enmStandardHandleInfo ,
						enmStandardHandleInfo.GetType ( ) );
			}	// switch ( enmStandardHandleInfo )

			Console.WriteLine (
				Properties.Resources.MSG_STD_HANDLE_END ,	// Format Control String
				pstrHandleLabel ,							// Format Item 0 = Label
				Environment.NewLine );						// Format Item 1 = Embedded Newline
		}	// private static void ShowHandleInfo


		private static void ShowHandleInfo (
			StandardOutputStream penmStandardOutputStream ,
			TextWriter ptrStandardOutputHandle ,
			string pstrHandleLabel )
		{
			Console.WriteLine (
				Properties.Resources.MSG_STD_HANDLE_BEGIN ,	// Format Control String
				pstrHandleLabel ,							// Format Item 0 = Label
				Environment.NewLine );						// Format Item 1 = Embedded Newline

			//	----------------------------------------------------------------
			//	Display the properties of the input object.
			//	----------------------------------------------------------------

			Console.WriteLine ( "        Type: FullName                       = {0}" , ptrStandardOutputHandle.GetType ( ).FullName );
			Console.WriteLine ( "        Type: AssemblyQualifiedName          = {0}" , ptrStandardOutputHandle.GetType ( ).AssemblyQualifiedName );
			Console.WriteLine ( "        Type: Assembly.FullName              = {0}" , ptrStandardOutputHandle.GetType ( ).Assembly.FullName );
			Console.WriteLine ( "        Type: Assembly.Location              = {0}" , ptrStandardOutputHandle.GetType ( ).Assembly.Location );
			Console.WriteLine ( "        String Representation                = {0}{1}" , ptrStandardOutputHandle.ToString ( ) , Environment.NewLine );

			//	---------------------------------------------------------------------------
			//	Downcast to StreamReader, and examine its properties.
			//
			//	Reference:	Console.OpenStandardInput Method (Visual Studio 2008)
			//				https://msdn.microsoft.com/en-us/library/tx55zca2(v=vs.90).aspx
			//
			//	SafeFileHandle sfhFileNandle = (SafeFileHandle)typeof(FileStream).GetField("_handle", BindingFlags.NonPublic | BindingFlags.Instance).GetValue((FileStream)YOUR_BINARY_READER.BaseStream)
			//
			//	Reference:	How to get the underlying file handle of a Stream?
			//				https://stackoverflow.com/questions/8640660/how-to-get-the-underlying-file-handle-of-a-stream
			//	---------------------------------------------------------------------------

			Stream baseStream = penmStandardOutputStream == StandardOutputStream.Output ? Console.OpenStandardOutput ( ) : Console.OpenStandardError ( );
			//	SafeFileHandle sfhFileNandle = ( SafeFileHandle ) typeof ( Stream ).GetField ( "_handle" , BindingFlags.NonPublic | BindingFlags.Instance ).GetValue ( ( System.IO.__ConsoleStream ) baseStream.__ConsoleStream );	// This is an invalid cast.
			//	StreamWriter swFromConsoleStream = ( StreamWriter ) ptrStandardOutputHandle;																																		// This fails at run time with message: Unable to cast object of type 'SyncTextWriter' to type 'System.IO.StreamWriter'.
			//	StreamWriter swFromConsoleStream = penmStandardOutputStream == StandardOutputStream.Output ? ( StreamWriter ) Console.OpenStandardOutput ( ) : ( StreamWriter) Console.OpenStandardError ( );						// This is an invalid cast.
			//	FileStream fsFromConsoleStream = ( FileStream ) baseStream;																																							// This fails at run time with message: Unable to cast object of type 'System.IO.__ConsoleStream' to type 'System.IO.FileStream'
			//	FileStream fsFromConsoleStream = penmStandardOutputStream == StandardOutputStream.Output ? ( FileStream ) Console.OpenStandardOutput ( ) : ( FileStream ) Console.OpenStandardError ( );							// This fails at run time with message: Unable to cast object of type 'System.IO.__ConsoleStream' to type 'System.IO.FileStream'.

			Console.WriteLine ( "        StreamReader.BaseStream.CanRead      = {0}" , baseStream.CanRead );
			Console.WriteLine ( "        StreamReader.BaseStream.CanSeek      = {0}" , baseStream.CanSeek );
			Console.WriteLine ( "        StreamReader.BaseStream.CanTimeout   = {0}" , baseStream.CanTimeout );
			Console.WriteLine ( "        StreamReader.BaseStream.CanWrite     = {0}{1}" , baseStream.CanWrite , Environment.NewLine );

			Console.WriteLine ( "        StreamReader.BaseStream.Length       = {0}" , ShowIfSupported ( baseStream , OptionalStreamFeatures.Length ) );
			Console.WriteLine ( "        StreamReader.BaseStream.Position     = {0}" , ShowIfSupported ( baseStream , OptionalStreamFeatures.Position ) );
			Console.WriteLine ( "        StreamReader.BaseStream.ReadTimeout  = {0}" , ShowIfSupported ( baseStream , OptionalStreamFeatures.ReadTimeout ) );
			Console.WriteLine ( "        StreamReader.BaseStream.WriteTimeout = {0}" , ShowIfSupported ( baseStream , OptionalStreamFeatures.ReadTimeout ) );
			Console.WriteLine ( "        StreamReader.BaseStream.WriteTimeout = {0}" , ShowIfSupported ( baseStream , OptionalStreamFeatures.ReadTimeout ) );

			StandardHandleInfo.SHS_HANDLE_STATE enmStandardHandleInfo = StandardHandleInfo.SHS_StandardHandleState (
				penmStandardOutputStream == StandardOutputStream.Output
				? StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_OUTPUT
				: StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_ERROR );

			switch ( enmStandardHandleInfo )
			{
				case StandardHandleInfo.SHS_HANDLE_STATE.SHS_ATTACHED:
					Console.WriteLine (
						"The {0} console handle is attached to its console." ,
						s_astrStandardHandleNames [ ( penmStandardOutputStream == StandardOutputStream.Output )
							? ( int ) StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_OUTPUT
							: ( int ) StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_ERROR ] );
					break;
				case StandardHandleInfo.SHS_HANDLE_STATE.SHS_REDIRECTED:
					Console.WriteLine ( "The {0} console handle is redirected to a file." ,
						s_astrStandardHandleNames [ ( penmStandardOutputStream == StandardOutputStream.Output )
							? ( int ) StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_OUTPUT
							: ( int ) StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_ERROR ] );
					break;
				case StandardHandleInfo.SHS_HANDLE_STATE.SHS_SYSTEM_ERROR:
					Console.WriteLine ( "The {0} console handle state cannot be determined because the attempt provoked a system error." ,
						s_astrStandardHandleNames [ ( penmStandardOutputStream == StandardOutputStream.Output )
							? ( int ) StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_OUTPUT
							: ( int ) StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_ERROR ] );
					break;
				case StandardHandleInfo.SHS_HANDLE_STATE.SHS_UNDETERMINABLE:
					Console.WriteLine ( "The {0} console handle state cannot be determined because a coding error interfered." ,
						s_astrStandardHandleNames [ ( penmStandardOutputStream == StandardOutputStream.Output )
							? ( int ) StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_OUTPUT
							: ( int ) StandardHandleInfo.SHS_STANDARD_HANDLE.SHS_ERROR ] );
					break;
				default:
					throw new System.ComponentModel.InvalidEnumArgumentException (
						"enmStandardHandleInfo" ,
						( int ) enmStandardHandleInfo ,
						enmStandardHandleInfo.GetType ( ) );
			}	// switch ( enmStandardHandleInfo )

			Console.WriteLine (
				Properties.Resources.MSG_STD_HANDLE_END ,	// Format Control String
				pstrHandleLabel ,							// Format Item 0 = Label
				Environment.NewLine );						// Format Item 1 = Embedded Newline
		}	// private static void ShowHandleInfo


		private static object ShowIfSupported ( Stream pStream , OptionalStreamFeatures penmFeature )
		{
			switch ( penmFeature )
			{
				case OptionalStreamFeatures.Length:
					if ( pStream.CanSeek )
					{
						return pStream.Length;
					}	// TRUE (The stream supports seeking.) block, if ( pStream.CanSeek )
					else
					{
						return Properties.Resources.MSG_UNSUPPORTED_FEATURE;
					}	// FALSE (The stream prohibits seeking.) block, if ( pStream.CanSeek )
				case OptionalStreamFeatures.Position:
					if ( pStream.CanSeek )
					{
						return pStream.Position;
					}	// TRUE (The stream supports seeking.) block, if ( pStream.CanSeek )
					else
					{
						return Properties.Resources.MSG_UNSUPPORTED_FEATURE;
					}	// FALSE (The stream prohibits seeking.) block, if ( pStream.CanSeek )
				case OptionalStreamFeatures.ReadTimeout:
					if ( pStream.CanTimeout )
					{
						return pStream.ReadTimeout;
					}	// TRUE (The stream supports seeking.) block, if ( pStream.CanSeek )
					else
					{
						return Properties.Resources.MSG_UNSUPPORTED_FEATURE;
					}	// FALSE (The stream prohibits seeking.) block, if ( pStream.CanSeek )
				case OptionalStreamFeatures.WriteTimeout:
					if ( pStream.CanTimeout )
					{
						return pStream.WriteTimeout;
					}	// TRUE (The stream supports seeking.) block, if ( pStream.CanSeek )
					else
					{
						return Properties.Resources.MSG_UNSUPPORTED_FEATURE;
					}	// FALSE (The stream prohibits seeking.) block, if ( pStream.CanSeek )
				default:
					throw new System.ComponentModel.InvalidEnumArgumentException (
						"penmFeature" ,
						( int ) penmFeature ,
						typeof ( OptionalStreamFeatures ) );
			}	// switch ( penmFeature )
		}	// private static object ShowIfSupported
	}	// class Program
}	// namespace ConsoleStreamsLab