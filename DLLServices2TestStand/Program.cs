/*
    ============================================================================

    Namespace:			DLLServices2TestStand

    Class Name:			Program

	File Name:			Program.cs

    Synopsis:			Exercise the classes in WizardWrx.DLLServices2.dll,
						which houses classes that once belonged to another
						library, WizardWrx.ApplicationHelpers.dll, but were
						moved out of it, to break a circular dependency between
						it and yet another library, WizardWrx.ConsoleAppAids,
						and their respective test stand programs.

    Remarks:			To prevent the circular dependencies that led to
						creation of WizardWrx.DLLServices2.dll, this test stand
						program deviates from my usual practice, and sticks to
						classes available from either WizardWrx.DLLServices2.dll
						or from one of my strong name signed base libraries.

    Author:				David A. Gray

	License:            Copyright (C) 2011-2016, David A. Gray.
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
    2013/02/22 1.0     DAG    This class, and its project, make their first 
                              appearance.

    2014/05/31 4.1     DAG    Bind against upgraded WizardWrx.DLLServices,
                              WizardWrx.ApplicationHelpers, and
                              WizardWrx.ConsoleAppAids2 libraries, and eliminate
                              references to unused system name spaces and 
                              libraries, to clarify what is actually required,
                              and to slightly streamline the build process.

    2014/06/07 5.0     DAG    Major namespace reorganization. This test program
                              confines itself to classes exposed by the class
                              library under test.

    2014/07/20 5.1     DAG    1) Correct an oversight that left this class in
                                 the old WizardWrx.ApplicationHelpers namespace.
                                 Since this change affects only two other DLLs,
                                 and, at most one user program, I took advantage
                                 of the opportunity to promote the DLLServices2
                                 namespace to the first rank under the overall
                                 WizardWrx namespace.

                              2) Swap the method names, so that the instance
                                 methods whose signatures exactly mirror those 
                                 of the Console.Write* methods have the same
                                 base method names, while the static methods,
                                 which require two additional arguments to name
                                 the foreground and background colors have names
                                 that remind you that you must specify colors in
                                 any call.

                                 The following table lists the substitutions.

                                 ------------------------------------------------------
                                 FindStr                    ReplStr
                                 ------------------------   ---------------------------
                                 public static void Write   public static void RGBWrite
                                 public void ColorWrite     public void Write
                                 ------------------------------------------------------

                                 This change means that any method that requires
                                 additional arguments has a name that differs
                                 slightly from that of the corresponding Console
                                 method, and the difference is a prefix, to hint
                                 that the colors go in front of the arguments to
                                 the analogous console method. Likewise, methods
                                 that have identical signatures have identical
                                 base names to the corresponding Console method.

    2014/10/14 5.3     DAG    As was true of the last point upgrade, the test 
                              stand is unchanged.
 
    2015/07/10 5.5     DAG    Incorporate tests for the classes that just moved
                              into WizardWrx.DLLServices2.dll, along with the
                              new native routine that reports the state of any
							  standard console handle for assemblies running in
                              ANY version of the Microsoft .NET Framework.

    2015/09/01 5.6     DAG    Add a couple of overlooked special characters and
                              strings, along with a new method, ChopChop, to
                              test the new Chop method.

	2015/09/01 5.7     DAG    Add a very simple product name and version format.

	2015/10/28 5.8     DAG    Add the console redirection file displays, which I
                              decided to keep in this library, so that the calls
                              into WWConAid.dll are all together.

    2016/04/12 6.0     DAG    Exercise the redirection state testers, including
                              timed tests of both approaches.

							  Investigate a remedy that would permit me to allow
							  the Visual Studio Hosting Process to execute
							  without interfering with my execution subsystem
							  tests.

    2016/05/11 6.0     DAG    Eliminate an unused constant.

	2016/05/12 6.1     DAG    Exercise the new routines to retrieve the special
                              error message strings.

	2016/05/20 6.1     DAG    Incorporate FileNameTricks_Exerciser.Drill to test
                              its namesake class.
                              error message strings.

	2016/06/04 6.2     DAG    Move the last two tests above the EOJ message.

	2016/06/06 6.3     DAG    Add a small routine to test the improved exception
                              reporting logic that suppresses printing a message
							  twice when reporting to both standard output 
							  streams is enabled, and neither is redirected.
    ============================================================================
*/


using System;

/* Added by DAG */

using System.ComponentModel;

using WizardWrx;
using WizardWrx.DLLServices2;


namespace DLLServices2TestStand
{
    class Program
    {
		internal const string ERRMSG_BADARG_PARAMINFO_TPL = @"Argument Name = {0} - Argument Value = {1}";
		internal const string ERRMSG_INTERNAL_ERROR_TPL = @"INTERNAL ERROR: {0}{1}";
		internal const string ERRMSG_INTERNAL_INNER_MSG_TPL = @"Inner Exception: Message = {0}";
		internal const string ERRMSG_INTERNAL_OBJNAME_TPL = @"Name of disposed object = {0}";
		internal const string ERRMSG_INTERNAL_SOURCE_TPL = @"Source = {0}{1}";
		internal const string ERRMSG_INTERNAL_STACKTRACE_TPL = @"{1}Stack Trace:{1}{0}{1}{1}";
		internal const string ERRMSG_INTERNAL_TARGETSITE_TPL = @"TargetSite = {0}{1}";
		internal const string ERRMSG_GENERIC_TPL = @"Error writing to log file {0}. Message = {1}{5}Source = {2}{5}TargetSite = {3}{5}StackTrace Begin:{5}{4}{5}StackTrace End";

		internal const bool OMIT_LINEFEED = false;
		internal const bool APPEND_LINEFEED = true;


		static readonly int [ ] s_aenmAssemblyVersionRequests =
		{
			( int ) StateManager.AssemblyVersionRequest.MajorOnly ,
			( int ) StateManager.AssemblyVersionRequest.MajorAndMinor ,
			( int ) StateManager.AssemblyVersionRequest.MajroMinorBuild ,
			( int ) StateManager.AssemblyVersionRequest.MajorMinorExceptRevision ,
			( int ) StateManager.AssemblyVersionRequest.MajorMinroBuildRevision ,
			( int ) StateManager.AssemblyVersionRequest.Complete ,
			WizardWrx.MagicNumbers.ZERO
		};	// s_aenmAssemblyVersionRequests


		static readonly Type [ ] s_atypCommonExceptionTypes = new Type [ ] 
		{
			typeof ( System.Exception ) ,
			typeof ( System.ArgumentException ) ,
			typeof ( System.ArgumentOutOfRangeException ) ,
			typeof ( System.ArgumentNullException ) ,
			typeof ( System.ObjectDisposedException ) ,
			typeof ( System.IO.IOException ) ,
			typeof ( System.FormatException ) ,

			typeof ( System.IO.FileNotFoundException ) ,
			typeof ( System.IO.DirectoryNotFoundException ) ,
			typeof ( System.IO.DriveNotFoundException ),
			typeof ( System.IO.EndOfStreamException ) ,
			typeof ( System.IO.InternalBufferOverflowException ) ,
			typeof ( System.IO.InvalidDataException ) ,
			typeof ( System.IO.PathTooLongException ) ,

			typeof ( System.InvalidCastException) ,
			typeof ( System.InvalidOperationException) ,
			typeof ( System.NullReferenceException) ,
			typeof ( System.OutOfMemoryException) ,
			typeof ( System.OverflowException ) ,
			typeof ( System.PlatformNotSupportedException ) ,
			typeof ( System.RankException ) ,
			typeof ( System.StackOverflowException ) ,
			typeof ( System.TimeoutException ) ,
			typeof ( System.TypeInitializationException ) ,
			typeof ( System.TypeLoadException ) ,
			typeof ( System.TypeUnloadedException ) ,
			typeof ( System.UnauthorizedAccessException ) ,
			typeof ( System.UriFormatException ) ,

			typeof ( System.Text.DecoderFallbackException ) ,
			typeof ( System.Text.EncoderFallbackException ) ,
			typeof ( System.Security.SecurityException ) ,
			typeof ( System.Security.VerificationException ) ,
			typeof ( System.Security.XmlSyntaxException ) ,

			typeof ( System.Xml.XmlException ) ,

			typeof ( System.Configuration.SettingsPropertyIsReadOnlyException ) ,
			typeof ( System.Configuration.SettingsPropertyNotFoundException ) ,
			typeof ( System.Configuration.SettingsPropertyWrongTypeException ) ,

			typeof ( System.Reflection.AmbiguousMatchException ) ,
			typeof ( System.Reflection.CustomAttributeFormatException ) ,
			typeof ( System.Reflection.InvalidFilterCriteriaException ) ,
			typeof ( System.Reflection.TargetParameterCountException ) ,

			typeof ( System.Text.ASCIIEncoding ) ,
			typeof ( System.Text.UnicodeEncoding) ,
			typeof ( System.Text.UTF7Encoding ) ,
			typeof ( System.Text.UTF8Encoding ) ,
			typeof ( System.Text.UTF32Encoding )
		};	// s_atypCommonExceptionTypes


		static readonly Guid [ ] s_agidSupportedExceptionTypes = new Guid [ ]
		{
			typeof ( System.Exception ).GUID ,
			typeof ( System.ArgumentException ).GUID ,
			typeof ( System.ArgumentOutOfRangeException ).GUID ,
			typeof ( System.ArgumentNullException ).GUID ,
			typeof ( System.ObjectDisposedException ).GUID ,
			typeof ( System.IO.IOException ).GUID ,
			typeof ( System.FormatException ).GUID ,
		};	// s_agidSupportedExceptionTypes


		static readonly string [ ] s_astrErrorMessagres =
        {
            Properties.Resources.ERRMSG_SUCCESS,
            Properties.Resources.ERRMSG_RUNTIME,
        };  // static read only string [ ] s_astrErrorMessagres

        static StateManager s_smTheApp = StateManager.GetTheSingleInstance ( );


        static void Main ( string [ ] pastrArgs )
        {
			TimeDisplayFormatter dtmfApp = s_smTheApp.ConsoleMessageTimeFormat;

            try
            {
				Console.WriteLine (
					Properties.Resources.BOJ_MSG_TPL ,
					new string [ ]
                    {
                        s_smTheApp.AppRootAssemblyFileBaseName ,
                        System.Diagnostics.FileVersionInfo.GetVersionInfo ( s_smTheApp.GetAssemblyFQFN ( ) ).FileVersion ,
                        dtmfApp.FormatThisTime ( s_smTheApp.AppStartupTimeLocal ) ,
                        dtmfApp.FormatThisTime ( s_smTheApp.AppStartupTimeUtc ) ,
                        Environment.NewLine
                    } );

				ExceptionLogger.TimeStampedTraceWrite ( string.Format ( 
					"BOJ {0}." ,
					s_smTheApp.AppRootAssemblyFileBaseName ) );

				s_smTheApp.LoadErrorMessageTable ( s_astrErrorMessagres );

				Console.WriteLine ( s_smTheApp.AppExceptionLogger.OutputOptionsDisplay ( "Initial                 " ) );
				s_smTheApp.AppExceptionLogger.OutputOptionTurnOn ( ExceptionLogger.OutputOptions.EventLog );
				Console.WriteLine ( s_smTheApp.AppExceptionLogger.OutputOptionsDisplay ( "Event Logging Enabled   " ) );

				if ( pastrArgs.Length > CmdLneArgsBasic.NONE && pastrArgs [ ArrayInfo.ARRAY_FIRST_ELEMENT ] == Properties.Resources.CMDARG_REDIRECTION_DETECTION )
				{	// Focus on this single test, without any other stuff to interfere.
					System.Diagnostics.Debugger.Launch ( );
					EvaluateConsoleHandleStates ( );
					ExploreProcessModulesCollection ( );
				}	// TRUE block, if ( pastrArgs.Length > CmdLneArgsBasic.NONE && pastrArgs [ ArrayInfo.ARRAY_FIRST_ELEMENT ] == Properties.Resources.CMDARG_REDIRECTION_DETECTION )
				else if ( pastrArgs.Length > CmdLneArgsBasic.NONE && pastrArgs [ ArrayInfo.ARRAY_FIRST_ELEMENT ] == Properties.Resources.CMDARG_REDIRECTION_STATE_TESTS )
				{
					CompareConsoleStateTestingMethods ( GetIterationCount ( pastrArgs ) );
				}	// TRUE block, else if ( pastrArgs.Length > CmdLneArgsBasic.NONE && pastrArgs [ ArrayInfo.ARRAY_FIRST_ELEMENT ] == Properties.Resources.CMDARG_REDIRECTION_STATE_TESTS )
				else if ( pastrArgs.Length > CmdLneArgsBasic.NONE && pastrArgs [ ArrayInfo.ARRAY_FIRST_ELEMENT ] == Properties.Resources.CMDARG_ENUMERATE_EXCEPTION_GUIDS )
				{	// This test gathers research data to go into the table of exception message formats.
					EnumExcpetionGUIDs ( );
				}	// TRUE block, else if ( pastrArgs.Length > CmdLneArgsBasic.NONE && pastrArgs [ ArrayInfo.ARRAY_FIRST_ELEMENT ] == Properties.Resources.CMDARG_ENUMERATE_EXCEPTION_GUIDS )
				else if ( pastrArgs.Length > CmdLneArgsBasic.NONE && pastrArgs [ ArrayInfo.ARRAY_FIRST_ELEMENT ] == Properties.Resources.CMDARG_GENERATE_EXCEPTION_MESSAGE_FORMAT_TABLE )
				{	// This task generates the configuration file.
					GenerateExceptionMessageFormatTable ( );
				}	// TRUE block, else if ( pastrArgs.Length > CmdLneArgsBasic.NONE && pastrArgs [ ArrayInfo.ARRAY_FIRST_ELEMENT ] == Properties.Resources.CMDARG_ENUMERATE_EXCEPTION_GUIDS )
				else
				{	// Run the whole set.

					//  --------------------------------------------------------
					//  Put the new GetAssemblyProductAndVersion method through 
					//	its paces.
					//  --------------------------------------------------------

					Console.WriteLine (
						Properties.Resources.MSG_VERSIONIFNO_TESTS_BEGIN ,
						Environment.NewLine );

					try
					{	// The last iteration is an invalid enumeration value, and it always throws.
						foreach ( StateManager.AssemblyVersionRequest enmAssemblyVersionRequest in s_aenmAssemblyVersionRequests )
						{
							Console.WriteLine (
								Properties.Resources.MSG_VERSIONINFO ,
								enmAssemblyVersionRequest ,
								( int ) enmAssemblyVersionRequest ,
								s_smTheApp.GetAssemblyProductAndVersion ( enmAssemblyVersionRequest ) );
						}	// foreach ( StateManager.AssemblyVersionRequest enmAssemblyVersionRequest in s_aenmAssemblyVersionRequests )
					}
					catch ( Exception exAll )
					{
						s_smTheApp.AppExceptionLogger.ReportException ( exAll );
					}

					Console.WriteLine (
						Properties.Resources.MSG_VERSIONINFO_TESTS_DONE ,
						s_smTheApp.GetAssemblyProductAndVersion ( ) ,
						Environment.NewLine );
					PauseForPictures ( OMIT_LINEFEED );

					//  --------------------------------------------------------
					//  Display information about the library under test.
					//  --------------------------------------------------------

					Console.WriteLine (
						Properties.Resources.MSG_ASBSOLUTE_ASSEMBLYNAME ,
						s_smTheApp.GetAssemblyFQFN ( ) ,
						Environment.NewLine );
					Console.WriteLine ( Properties.Resources.MSG_ASSEMBLY_SUBSYSTEM ,
						s_smTheApp.AppSubsystemID ,
						s_smTheApp.GetAppSubsystemString ( ) ,
						Environment.NewLine );

					//  --------------------------------------------------------
					//  Display information about the standard stream handles.
					//  --------------------------------------------------------

					EvaluateConsoleHandleStates ( );

					MessageInColor mic = new MessageInColor (
						ConsoleColor.White ,
						ConsoleColor.DarkGray );
					mic.WriteLine (
						Properties.Resources.COLOR_MESSAGE_TEXT ,
						mic.MessageForegroundColor ,
						mic.MessageBackgroundColor ,
						Environment.NewLine );

					Console.WriteLine (
						Properties.Resources.MSG_SHOWING_LIBRARY_INFO ,		// This is the message template.
						Environment.NewLine );								// This token adds newlines my way.

					PauseForPictures ( OMIT_LINEFEED );

					WizardWrx.DLLServices2.Util.ShowKeyAssemblyProperties ( System.Reflection.Assembly.GetAssembly ( s_smTheApp.GetType ( ) ) );

					PauseForPictures ( APPEND_LINEFEED );

					ShowCurrentDefaultErrorMessageColors ( Properties.Resources.MSG_SHOWING_CONFIGURED_COLORS );

					PauseForPictures ( APPEND_LINEFEED );

					ErrorMessagesInColor emcOld = s_smTheApp.AppExceptionLogger.SaveCurrentColors ( );

					Console.WriteLine ( s_smTheApp.AppExceptionLogger.OutputOptionsDisplay ( "Before Adding STDERR    " ) );
					s_smTheApp.AppExceptionLogger.OutputOptionTurnOn ( ExceptionLogger.OutputOptions.StandardError );
					Console.WriteLine ( s_smTheApp.AppExceptionLogger.OutputOptionsDisplay ( "Output to Standard Error" ) );

					int intTestCase = ArrayInfo.ARRAY_FIRST_ELEMENT;

					try
					{
						Console.WriteLine (
							Properties.Resources.TEST_EXCEPTION_LABEL ,
							++intTestCase ,
							Environment.NewLine );
						string strMsg = string.Format (
							Properties.Resources.TEST_EXCEPTION_MESSAGE ,
							intTestCase );
						throw new Exception ( strMsg );
					}
					catch ( Exception exAll )
					{
						s_smTheApp.AppExceptionLogger.ReportException ( exAll );
					}

					PauseForPictures ( APPEND_LINEFEED );

					ChangeDefaultErrorMessageColors ( );
					ShowCurrentDefaultErrorMessageColors ( Properties.Resources.MSG_SHOWING_PROGRAMMATIC_COLORS );

					PauseForPictures ( APPEND_LINEFEED );

					s_smTheApp.AppExceptionLogger.RestoreSavedColors ( );

					try
					{
						Console.WriteLine (
							Properties.Resources.TEST_EXCEPTION_LABEL ,
							++intTestCase ,
							Environment.NewLine );
						string strMsg = string.Format (
							Properties.Resources.TEST_EXCEPTION_MESSAGE ,
							intTestCase );
						throw new Exception ( strMsg );
					}
					catch ( Exception exAll )
					{
						s_smTheApp.AppExceptionLogger.ReportException ( exAll );
					}
				}	// FALSE block, if ( pastrArgs.Length > CmdLneArgsBasic.NONE && pastrArgs [ ArrayInfo.ARRAY_FIRST_ELEMENT ] == Properties.Resources.CMDARG_REDIRECTION_DETECTION )
			}
            catch ( Exception exAll )
            {
				s_smTheApp.AppExceptionLogger.ReportException ( exAll );
				s_smTheApp.AppReturnCode = MagicNumbers.ERROR_RUNTIME;
            }

			//	----------------------------------------------------------------
			//	Unless restricted to one test by a command line argument, run
			//	the remainder of the tests.
			//	----------------------------------------------------------------

			if ( pastrArgs.Length == CmdLneArgsBasic.NONE )
			{
				if ( Logic.Unless ( pastrArgs.Length > CmdLneArgsBasic.NONE && pastrArgs [ ArrayInfo.ARRAY_FIRST_ELEMENT ] == Properties.Resources.CMDARG_REDIRECTION_DETECTION ) )
				{
					PauseForPictures ( APPEND_LINEFEED );

					try
					{
						//	----------------------------------------------------
						//  Exercise the new utility classes.
						//
						//	To simplify the back-port, these tests get a
						//	dedicated class, NewClassTests_20140914.
						//	----------------------------------------------------

						int intTestNumber = ArrayInfo.ARRAY_FIRST_ELEMENT;

						int intRetCode = NewClassTests_20140914.ArrayInfoExercises ( ref intTestNumber );

						if ( intRetCode != MagicNumbers.ERROR_SUCCESS )
							throw new Exception ( string.Format (
								Properties.Resources.ERRMSG_NEW_CLASS_TESTS_20140914 ,
								"ArrayInfoExercises" ,
								intRetCode ) );

						intRetCode = NewClassTests_20140914.CSVFileInfoExercises ( ref intTestNumber );

						if ( intRetCode == MagicNumbers.ERROR_SUCCESS )
							PauseForPictures ( OMIT_LINEFEED );
						else
							throw new Exception ( string.Format (
								Properties.Resources.ERRMSG_NEW_CLASS_TESTS_20140914 ,
								"CSVFileInfoExercises" ,
								intRetCode ) );

						intRetCode = NewClassTests_20140914.DisplayFormatsExercises ( ref intTestNumber );

						if ( intRetCode == MagicNumbers.ERROR_SUCCESS )
							PauseForPictures ( OMIT_LINEFEED );
						else
							throw new Exception ( string.Format (
								Properties.Resources.ERRMSG_NEW_CLASS_TESTS_20140914 ,
								"DisplayFormats" ,
								intRetCode ) );

						intRetCode = NewClassTests_20140914.FileIOFlagsExercises ( ref intTestNumber );

						if ( intRetCode == MagicNumbers.ERROR_SUCCESS )
							PauseForPictures ( OMIT_LINEFEED );
						else
							throw new Exception ( string.Format (
								Properties.Resources.ERRMSG_NEW_CLASS_TESTS_20140914 ,
								"FileIOFlags" ,
								intRetCode ) );

						intRetCode = NewClassTests_20140914.ListInfoExercises ( ref intTestNumber );

						if ( intRetCode == MagicNumbers.ERROR_SUCCESS )
							PauseForPictures ( OMIT_LINEFEED );
						else
							throw new Exception ( string.Format (
								Properties.Resources.ERRMSG_NEW_CLASS_TESTS_20140914 ,
								"ListInfo" ,
								intRetCode ) );

						//	----------------------------------------------------
						//	These two tests fit comfortably on one screen.
						//	----------------------------------------------------

						intRetCode = NewClassTests_20140914.PathPositionsExercises ( ref intTestNumber );

						if ( intRetCode != MagicNumbers.ERROR_SUCCESS )
							throw new Exception ( string.Format (
								Properties.Resources.ERRMSG_NEW_CLASS_TESTS_20140914 ,
								"PathPositions" ,
								intRetCode ) );

						intRetCode = NewClassTests_20140914.SpecialCharactersExercises ( ref intTestNumber );

						if ( intRetCode == MagicNumbers.ERROR_SUCCESS )
							PauseForPictures ( OMIT_LINEFEED );
						else
							throw new Exception ( string.Format (
								Properties.Resources.ERRMSG_NEW_CLASS_TESTS_20140914 ,
								"SpecialCharacters" ,
								intRetCode ) );
						intRetCode = NewClassTests_20140914.ChopChop ( ref intTestNumber );

						if ( intRetCode == MagicNumbers.ERROR_SUCCESS )
							PauseForPictures ( OMIT_LINEFEED );
						else
							throw new Exception ( string.Format (
								Properties.Resources.ERRMSG_NEW_CLASS_TESTS_20140914 ,
								"Chop" ,
								intRetCode ) );

						//	----------------------------------------------------
						//	The last stop doesn't merit a photo op, because the
						//	program is ending, and there is ample room
						//	remaining on the screen to display the end of job
						//	message.
						//	----------------------------------------------------

						intRetCode = NewClassTests_20140914.UtilsExercises ( ref intTestNumber );

						if ( intRetCode != MagicNumbers.ERROR_SUCCESS )
							throw new Exception ( string.Format (
								Properties.Resources.ERRMSG_NEW_CLASS_TESTS_20140914 ,
								"Util" ,
								intRetCode ) );
					}
					catch ( Exception errAnyKind )
					{
						s_smTheApp.AppExceptionLogger.ReportException ( errAnyKind );
					}
				}	// if ( Logic.Unless ( pastrArgs.Length > CmdLneArgsBasic.NONE && pastrArgs [ ArrayInfo.ARRAY_FIRST_ELEMENT ] == Properties.Resources.CMDARG_REDIRECTION_DETECTION ) )
			}	// if ( pastrArgs.Length == CmdLneArgsBasic.NONE )

			//	----------------------------------------------------------------
			//	Exercise the FileNameTricks and special messages classes.
			//	----------------------------------------------------------------

			FileNameTricks_Exerciser.Drill ( );
			ExerciseSpecialMessageGenerators ( );
			Console.Beep ( );		// WaitForCarbonUnit emits a beep.
			Console.ReadLine ( );

			//	----------------------------------------------------------------
			//	Write the final report, clean up, and shut down.
			//	----------------------------------------------------------------

			ExceptionLogger.TimeStampedTraceWrite ( string.Format ( "EOJ {0}." , s_smTheApp.AppRootAssemblyFileBaseName ) );
			System.Diagnostics.Trace.Close ( );

			//	----------------------------------------------------------------
			//	Exercise the new duplicate output suppression feature of the
			//	ExceptionLogger ReportException methods.
			//	----------------------------------------------------------------

			DeduplicateExceptionLogs ( );

			Console.WriteLine (
				Properties.Resources.EOJ_MSG_TPL ,
				new string [ ]
                {
                    s_smTheApp.AppRootAssemblyFileBaseName ,
                    s_smTheApp.AppUptime.ToString ( ) ,
                    Environment.NewLine
                } );
			s_smTheApp.AppReturnCode = MagicNumbers.ERROR_SUCCESS;
			Environment.Exit ( MagicNumbers.ERROR_SUCCESS );
        }   // static void Main


		private static void DeduplicateExceptionLogs ( )
		{
			Console.WriteLine (
				"{1}ExceptionLogger flags before changes for test: {0}" ,
				s_smTheApp.AppExceptionLogger.OptionFlags ,
				Environment.NewLine );
			s_smTheApp.AppExceptionLogger.OutputOptionTurnOn (
				ExceptionLogger.OutputOptions.ActivePropsToStdOut );
			Console.WriteLine (
				"ExceptionLogger flags after changes for test: {0}{1}" ,
				s_smTheApp.AppExceptionLogger.OptionFlags ,
				Environment.NewLine );

#if DOUBLE_TROUBLE_TEST
			System.Diagnostics.Debugger.Launch ( );
#endif	// #if DOUBLE_TROUBLE_TEST
			try
			{
				throw new Exception ( "Double Trouble: Up to version 6.2, this message would have displayed twice when neither standard stream is redirected." );
			}
			catch ( Exception exDoubleTrouble )
			{
				s_smTheApp.AppExceptionLogger.ReportException ( exDoubleTrouble );
			}
		}	// DeduplicateExceptionLogs


		private static void EnumExcpetionGUIDs ( )
		{
			using ( System.IO.StreamWriter swOutputStream = new System.IO.StreamWriter (
				Properties.Settings.Default.ExceptionGUIDsListingFile ,
				FileIOFlags.FILE_OUT_CREATE ,
				System.Text.Encoding.ASCII ,
				MagicNumbers.CAPACITY_08KB ) )
			{
				swOutputStream.WriteLine ( "FullName\tName\tGUID\tTypeHandle\tGUIDBytes" );			// Label the columns.

				foreach ( Type typCurrent in s_atypCommonExceptionTypes )
				{
					swOutputStream.WriteLine (
						"{0}\t{1}\t{2}\t{3}\t{4}" ,
						new object [ ]
						{
							typCurrent.FullName ,													// Token 0
							typCurrent.Name ,														// Token 1
							typCurrent.GUID ,														// Token 2
							typCurrent.TypeHandle.Value.ToString ( NumericFormats.HEXADECIMAL_8 ) ,	// Token 3
							SerializeByteArray ( typCurrent.GUID.ToByteArray ( ) )					// Token 4
						} );
					
				}	// foreach ( Type typCurrent in s_atypCommonExceptionTypes )

				System.IO.FileInfo fiOutputFile = new System.IO.FileInfo ( Properties.Settings.Default.ExceptionGUIDsListingFile );
				Console.WriteLine (
					"{1}Properties of selected System Object Types written onto file {0}{1}" ,
					fiOutputFile.FullName ,
					Environment.NewLine );
			}	// using ( System.IO.StreamWriter swOutputStream = new System.IO.StreamWriter ( ... )
		}	// private static void EnumExcpetionGUIDs


		private static void ExerciseSpecialMessageGenerators ( )
		{
			Console.WriteLine ( "{0}Exercising the new special message generators.{0}" , Environment.NewLine );
			Console.WriteLine ( "    Test 1: Get the ERROR_SUCCESS placeholder from ExceptionLogger.GetSpecifiedReservedErrorMessage         : {0}" , ExceptionLogger.GetSpecifiedReservedErrorMessage ( ExceptionLogger.ErrorExitOptions.Succeeded ) );
			Console.WriteLine ( "    Test 2: Get the runtime error message for EvtLog from ExceptionLogger.GetSpecifiedReservedErrorMessage  : {0}" , ExceptionLogger.GetSpecifiedReservedErrorMessage ( ExceptionLogger.ErrorExitOptions.RecordedInEventLog ) );
			Console.WriteLine ( "    Test 3: Get the runtime error message for STDERR from ExceptionLogger.GetSpecifiedReservedErrorMessage  : {0}" , ExceptionLogger.GetSpecifiedReservedErrorMessage ( ExceptionLogger.ErrorExitOptions.RecordedInStandardError ) );
			Console.WriteLine ( "    Test 4: Get the runtime error message for STDOUT from ExceptionLogger.GetSpecifiedReservedErrorMessage  : {0}" , ExceptionLogger.GetSpecifiedReservedErrorMessage ( ExceptionLogger.ErrorExitOptions.RecordedInStandardOutput ) );
			Console.WriteLine ( "    Test 5: Get the runtime error message for Program from ExceptionLogger.GetSpecifiedReservedErrorMessage : {0}" , s_smTheApp.AppExceptionLogger.GetReservedErrorMessage ( ) );
			Console.WriteLine ( "{0}Special message generators have been exercised.{0}" , Environment.NewLine );
		}	// ExerciseSpecialMessageGenerators


		private static string SerializeByteArray ( byte [ ] pbytArbitraryBytes )
		{
			int intByteCount = pbytArbitraryBytes.Length;
			System.Text.StringBuilder rsbSerializedBytes = new System.Text.StringBuilder ( intByteCount * MagicNumbers.PLUS_TWO );

			for ( int intCurrentByte = ArrayInfo.ARRAY_FIRST_ELEMENT ; intCurrentByte < intByteCount ; intCurrentByte++ )
				rsbSerializedBytes.Append ( pbytArbitraryBytes [ intCurrentByte ].ToString ( NumericFormats.HEXADECIMAL_2 ) );

			return rsbSerializedBytes.ToString ( );
		}	// EnumExcpetionGUIDs


		private static void GenerateExceptionMessageFormatTable ( )
		{
			const string RESERVED_GUID = @"{733C6022-332D-4D3A-B9AE-41600AAE349F}";
			Guid gidReserved = new Guid ( RESERVED_GUID );
			byte [ ] abytReservedGuid = gidReserved.ToByteArray ( );
			Console.WriteLine (
				"My Reserved GUID = {0}{2}     Byte Array  = {1}" ,
				RESERVED_GUID ,
				SerializeByteArray ( abytReservedGuid )
				, Environment.NewLine );
			Guid gidReconstituted = new Guid ( abytReservedGuid );

			if ( Guid.Equals ( gidReconstituted , gidReserved ) )
				Console.WriteLine ( "{0}The GUID reconstituted correctly from the byte array.{0}" , Environment.NewLine );
			else
				Console.WriteLine ( "{0}The GUID reconstituted INcorrectly from the byte array.{0}" , Environment.NewLine );
		}	// GenerateExceptionMessageFormatTable


		//	====================================================================
		//	Subroutines, one of which is marked Internal, so that routines in
		//	related classes can call it.
		//	====================================================================

		/// <summary>
		/// Since StateManager.GetStdHandleFQFN returns a well formed filename
		/// string suitable for use anywhere that I valid filename is required,
		/// the filename needs a leading space to make it look right in the test
		/// program's message. Otherwise, the returned string is acceptable as
		/// is.
		/// </summary>
		/// <param name="penmStdHandelID">
		/// The StateManager.ShsStandardHandle enumeration simplifies gathering
		/// information about any of the three streams, each represented by a
		/// property on the static Console object by mapping them to an
		/// enumeration.
		/// </param>
		/// <returns>
		/// If the standard stream that corresponds to argument penmStdHandelID
		/// is redirected, the return value is a single space character followed
		/// by the name of the file to which it is redirected. Otherwise, the
		/// string returned by StateManager.GetStdHandleFQFN is returned as is.
		/// </returns>
		/// <remarks>
		/// Implementing this transformation as a method saves the second trip
		/// through StateManager.GetStdHandleFQFN that would be required if it
		/// were implemented inline as a ternary expression.
		/// 
		/// DLLServices2.Properties.Resources.MSG_CONSOLE_HAS_STD_HANDLE is read
		/// from the managed resources of the DLL, which must be able to return
		/// it through StateManager.GetStdHandleFQFN.
		/// </remarks>
		private static string BeautifyStandardHandleFQFN ( StateManager.ShsStandardHandle penmStdHandelID )
		{
			string strTempStandardHandleFQFN = s_smTheApp.GetStdHandleFQFN ( penmStdHandelID );

			if ( strTempStandardHandleFQFN == WizardWrx.DLLServices2.Properties.Resources.MSG_CONSOLE_HAS_STD_HANDLE )
				return strTempStandardHandleFQFN;
			else
				return string.Concat ( SpecialCharacters.SPACE_CHAR , strTempStandardHandleFQFN );
		}	// private static string BeautifyStandardHandleFQFN


        private static void ChangeDefaultErrorMessageColors ( )
        {
            ErrorMessagesInColor.FatalExceptionTextColor = ConsoleColor.Red;
            ErrorMessagesInColor.FatalExceptionBackgroundColor = ConsoleColor.White;

            ErrorMessagesInColor.RecoverableExceptionTextColor = ConsoleColor.Yellow;
            ErrorMessagesInColor.RecoverableExceptionBackgroundColor = ConsoleColor.DarkMagenta;
        }   // private static void ChangeDefaultErrorMessageColors


		private static void CompareConsoleStateTestingMethods ( int pintNIterations )
		{
			string strMsgTpl = Properties.Resources.MSG_REDIRECTION_STATE_TEST_TIMING;

			bool fIsRedirected = false;

			//	----------------------------------------------------------------
			//	Test the 100% managed method that tests for a null stream base.
			//	----------------------------------------------------------------

			DateTime dtmBegin = DateTime.UtcNow;
			DateTime dtmDone = DateTime.MinValue;

			for ( int intIteration = ArrayInfo.ARRAY_FIRST_ELEMENT ; intIteration < pintNIterations ; intIteration++ )
			{
				fIsRedirected = ( s_smTheApp.StandardHandleState ( WizardWrx.DLLServices2.StateManager.ShsStandardHandle.ShsStdOut ) == StateManager.ShsHandleState.ShsRedirected );
			}	// for ( int intIteration = ArrayInfo.ARRAY_FIRST_ELEMENT ; intIteration < pintNIterations ; intIteration++ )

			//	----------------------------------------------------------------
			//	Keep the expensive I/O bound tasks outside the test loop.
			//	----------------------------------------------------------------

			dtmDone = DateTime.UtcNow;

			Console.Error.WriteLine (
				strMsgTpl ,																		// Format control string
				new string [ ]																	// Array of format items, as follows:
				{
					Properties.Resources.MSG_HANDLEBASE_NULL ,									// Format Item 0 = Algorithm
					pintNIterations.ToString ( NumericFormats.NUMBER_PER_REG_SETTINGS_0D ) ,	// Format Item 1 = pintNIterations
					DisplayFormats.FormatDateTimeForShow ( dtmBegin.ToLocalTime ( ) ) ,			// Format Item 2 = Start Time
					DisplayFormats.FormatDateTimeForShow ( dtmDone.ToLocalTime ( ) ) ,			// Format Item 3 = Finish Time
					TimeSpan.FromTicks ( dtmDone.Ticks - dtmBegin.Ticks ).ToString ( ) ,		// Format Item 4 = Time Span
					Environment.NewLine } );													// Format Item 5 = Newline, My Way

			//	----------------------------------------------------------------
			//	Test the unmanaged method that invokes a Win32 API function.
			//	----------------------------------------------------------------

			dtmBegin = DateTime.UtcNow;

			for ( int intIteration = ArrayInfo.ARRAY_FIRST_ELEMENT ; intIteration < pintNIterations ; intIteration++ )
			{
				fIsRedirected = ( s_smTheApp.StandardHandleState ( StateManager.ShsStandardHandle.ShsStdOut ) == StateManager.ShsHandleState.ShsRedirected );
			}	// for ( int intIteration = ArrayInfo.ARRAY_FIRST_ELEMENT ; intIteration < pintNIterations ; intIteration++ )

			//	----------------------------------------------------------------
			//	Keep the expensive I/O bound tasks outside the test loop.
			//	----------------------------------------------------------------

			dtmDone = DateTime.UtcNow;

			Console.Error.WriteLine (
				strMsgTpl ,																		// Format control string
				new string [ ]																	// Array of format items, as follows:
				{
					Properties.Resources.MSG_PINVOKE_WIN32 ,										// Format Item 0 = Algorithm
					pintNIterations.ToString ( NumericFormats.NUMBER_PER_REG_SETTINGS_0D ) ,		// Format Item 1 = pintNIterations
					DisplayFormats.FormatDateTimeForShow ( dtmBegin.ToLocalTime ( ) ) ,				// Format Item 2 = Start Time
					DisplayFormats.FormatDateTimeForShow ( dtmDone.ToLocalTime ( ) ) ,				// Format Item 3 = Finish Time
					TimeSpan.FromTicks ( dtmDone.Ticks - dtmBegin.Ticks ).ToString ( ) ,			// Format Item 4 = Time Span
					Environment.NewLine } );														// Format Item 5 = Newline, My Way
		}	// private static void CompareConsoleStateTestingMethods ( )


		private static string ConsoleHandleStateMessage ( StateManager.ShsHandleState shsHandleState )
		{
			switch ( shsHandleState )
			{
				case StateManager.ShsHandleState.ShsAttached:
					return Properties.Resources.MSG_HANDLE_IS_ATTACHED;
				case StateManager.ShsHandleState.ShsRedirected:
					return Properties.Resources.MSG_HANDLE_IS_REDIRECTED;
				default:
					return Properties.Resources.MSG_HANDLE_IS_UNDEFINED;
			}	// switch ( shsHandleState )
		}	// private static string ConsoleHandleStateMessage


		private static void ExploreProcessModulesCollection ( )
		{
			Console.WriteLine ( @"{0}ExploreProcessModulesCollection Begin:{0}" , Environment.NewLine );
			System.Diagnostics.Process cpInfo = System.Diagnostics.Process.GetCurrentProcess ( );
			System.Diagnostics.ProcessModuleCollection apmLoadedModules = cpInfo.Modules;

			Console.WriteLine ( @"{0}ExploreProcessModulesCollection Done{0}" , Environment.NewLine );
		}	// ExploreProcessModulesCollection


		private static void EvaluateConsoleHandleStates ( )
		{	// The function nesting is encoded in the stepwise indentations.
			Console.WriteLine ( StringTricks.AppendFullStopIfMissing (
				string.Format (
					Properties.Resources.MSG_STANDARD_HANDLE_STATE ,			// Format control string
					Properties.Resources.MSG_STDIN ,							// Format Item 0: Textual description of stream
					ConsoleHandleStateMessage (
						s_smTheApp.StandardHandleState (
							StateManager.ShsStandardHandle.ShSStdIn ) ) ,		// Format Item 1: Standard handle state
					BeautifyStandardHandleFQFN (
						StateManager.ShsStandardHandle.ShSStdIn ) ) ) );		// Format Item 2: File name to which redirected, if applicable.
			Console.WriteLine ( StringTricks.AppendFullStopIfMissing (
				string.Format (
					Properties.Resources.MSG_STANDARD_HANDLE_STATE ,			// Format control string
					Properties.Resources.MSG_STDOUT ,							// Format Item 0: Textual description of stream
					ConsoleHandleStateMessage (
						s_smTheApp.StandardHandleState (
							StateManager.ShsStandardHandle.ShsStdOut ) ) ,		// Format Item 1: Standard handle state
					BeautifyStandardHandleFQFN (
						StateManager.ShsStandardHandle.ShsStdOut ) ) ) );		// Format Item 2: File name to which redirected, if applicable.
			Console.WriteLine ( StringTricks.AppendFullStopIfMissing (
				string.Format (
					Properties.Resources.MSG_STANDARD_HANDLE_STATE ,			// Format control string
					Properties.Resources.MSG_STDERR ,							// Format Item 0: Textual description of stream
					ConsoleHandleStateMessage (
						s_smTheApp.StandardHandleState (
							StateManager.ShsStandardHandle.ShsStdEror ) ) ,		// Format Item 1: Standard handle state
					BeautifyStandardHandleFQFN (
						StateManager.ShsStandardHandle.ShsStdEror ) ) ) );		// Format Item 2: File name to which redirected, if applicable.
		}	// private static void EvaluateConsoleHandleStates


		private static int GetIterationCount ( string [ ] pastrArgs )
		{
			const int DEFAULT = 10;

			if ( pastrArgs.Length > CmdLneArgsBasic.FIRST_POSITIONAL_ARG )
			{
				int rintArgValue = MagicNumbers.ZERO;

				if ( int.TryParse ( pastrArgs [ CmdLneArgsBasic.FIRST_POSITIONAL_ARG ] , out rintArgValue ) )
				{	// A valid iteration count was specified. use it.
					return rintArgValue;
				}	// TRUE (anticipated outcome) block, if ( int.TryParse ( pastrArgs [ CmdLneArgsBasic.FIRST_POSITIONAL_ARG ] , out rintArgValue ) )
				else
				{	// The specified iteration count is invalid. Use the default.
					return DEFAULT;
				}	// FALSE (unanticipated outcome) block, if ( int.TryParse ( pastrArgs [ CmdLneArgsBasic.FIRST_POSITIONAL_ARG ] , out rintArgValue ) )
			}	// TRUE (anticipated outcome) block, if ( pastrArgs.Length > CmdLneArgsBasic.NONE )
			else
			{	// The iteration count is unspecified. Use the default.
				return DEFAULT;
			}	// FALSE (unanticipated outcome) block, if ( pastrArgs.Length > CmdLneArgsBasic.NONE )
		}	// GetIterationCount


        internal static void PauseForPictures ( bool pfAppendLineFeed )
        {
			bool fPauseing = true;

			ExceptionLogger.TimeStampedTraceWrite ( string.Format ( "Entering PauseForPictures method with pfAppendLineFeed = {0}." , pfAppendLineFeed ) );

			if ( ( s_smTheApp.StandardHandleState ( WizardWrx.DLLServices2.StateManager.ShsStandardHandle.ShsStdOut ) == StateManager.ShsHandleState.ShsRedirected ) )
			{	// Emit a line feed in lieu of the page break.
				ExceptionLogger.TimeStampedTraceWrite ( "     PauseForPictures: IsStdOutRedirected returned TRUE." );
				Console.WriteLine ( );
				fPauseing = false;
			}	// TRUE block, if ( MessageInColor.IsStdOutRedirected ( ) )
			else
			{	// Display a prompt, wait for input, clear the screen (and the I/O buffer), and emit a line feed if requested.
				ExceptionLogger.TimeStampedTraceWrite ( "     PauseForPictures: IsStdOutRedirected returned FALSE." );

				if ( fPauseing )
				{	// Skip the pauses if standard output is redirected.
					Console.Error.Write ( Properties.Resources.MSG_PHOTO_OP );
					Console.ReadLine ( );
				}	// if ( fPauseing )
					
				Util.SafeConsoleClear ( );	// Util.SafeConsoleClear is a non-throwing wrapper around Console.Clear ( );

				if ( pfAppendLineFeed )
				{	// Caller requested a lone feed.
					Console.WriteLine ( );
				}	// if ( pfAppendLineFeed )
			}	// FALSE block, if ( MessageInColor.IsStdOutRedirected ( ) )
        }   // private static void PauseForPictures




		private static void ShowCurrentDefaultErrorMessageColors ( string pstrHeadingMessage )
        {
			Console.WriteLine ( pstrHeadingMessage , Environment.NewLine );
            ShowDefaulErrorMessageColors (
                ErrorMessagesInColor.FatalExceptionTextColor ,
                ErrorMessagesInColor.FatalExceptionBackgroundColor ,
                ErrorMessagesInColor.ErrorSeverity.Fatal );
            ShowDefaulErrorMessageColors (
                ErrorMessagesInColor.RecoverableExceptionTextColor ,
                ErrorMessagesInColor.RecoverableExceptionBackgroundColor ,
                ErrorMessagesInColor.ErrorSeverity.Recoverable );
        }   // private static void ShowCurrentDefaultErrorMessageColors 


        private static void ShowDefaulErrorMessageColors (
            ConsoleColor pclrFG ,
            ConsoleColor pclrBG ,
            ErrorMessagesInColor.ErrorSeverity penmSeverity )
        {
            string strMsgType;  // The following switch block initializes this, or throws up.

            switch ( penmSeverity )
            {
                case ErrorMessagesInColor.ErrorSeverity.Fatal:
                    strMsgType = Properties.Resources.EXCEPTION_IS_FATAL;
                    break;
                case ErrorMessagesInColor.ErrorSeverity.Recoverable:
                    strMsgType = Properties.Resources.EXCEPTION_IS_RECOVERABLE;
                    break;
                default:
                    throw new InvalidEnumArgumentException (
                        "penmSeverity" ,
                        ( int ) penmSeverity ,
                        typeof ( ErrorMessagesInColor.ErrorSeverity ) );
            }   // switch ( penmSeverity )

            if ( pclrFG != pclrBG )
            {
                MessageInColor.RGBWriteLine (
                    pclrFG ,
                    pclrBG ,
                    Properties.Resources.DEFAULT_EXCEPTION_COLORS_MESSAGE ,
                    new string [ ]
                    {
                        strMsgType ,
                        Properties.Resources.DEFAULT_EXCEPTION_COLORS_PROPERTIES ,
                        pclrFG.ToString ( ) ,
                        pclrBG.ToString ( ) ,
                        Environment.NewLine
                    } );

                //  ------------------------------------------------------------
                //  An ErrorMessagesInColor object writes to the Standard Error
                //  (STDERR) stream.
                //  ------------------------------------------------------------

                {   // Severely constrain the scope of the new ErrorMessagesInColor. 
                    ErrorMessagesInColor emic = ErrorMessagesInColor.GetDefaultErrorMessageColors ( penmSeverity );
                    MessageInColor.RGBWriteLine (
                        pclrFG ,
                        pclrBG ,
                        Properties.Resources.DEFAULT_EXCEPTION_COLORS_MESSAGE ,
                        new string [ ]
                    {
                        strMsgType ,
                        Properties.Resources.DEFAULT_EXCEPTION_COLORS_METHOD_1 ,
                        emic.MessageForegroundColor.ToString ( ) ,
                        emic.MessageBackgroundColor.ToString ( ) ,
                        Environment.NewLine
                    } );
                }   // ErrorMessagesInColor object emic has done its job, and goes out of scope.

                //  ------------------------------------------------------------
                //  A MessageInColor object writes to the Standard Output
                //  (STDOUT) stream.
                //  ------------------------------------------------------------

                {   // Severely constrain the scope of the new MessageInColor. 
                    MessageInColor mic = ErrorMessagesInColor.GetDefaultMessageColors ( penmSeverity );
                    MessageInColor.RGBWriteLine (
                        pclrFG ,
                        pclrBG ,
                        Properties.Resources.DEFAULT_EXCEPTION_COLORS_MESSAGE ,
                        new string [ ]
                    {
                        strMsgType ,
                        Properties.Resources.DEFAULT_EXCEPTION_COLORS_METHOD_2 ,
                        pclrFG.ToString ( ) ,
                        pclrBG.ToString ( ) ,
                        Environment.NewLine
                    } );
                }   // MessageInColor object mic has done its job, and goes out of scope.
            }   // if ( pclrFG != pclrBG )
            else
            {
                Console.WriteLine (
                    Properties.Resources.DEFAULT_EXCEPTION_COLORS_MESSAGE ,
                    new string [ ]
                    {
                        strMsgType ,
                        Properties.Resources.DEFAULT_EXCEPTION_COLORS_PROPERTIES ,
                        pclrFG.ToString ( ) ,
                        pclrBG.ToString ( ) ,
                        Environment.NewLine
                    } );
            }   // if ( pclrFG != pclrBG )
        }   // private static void ShowDefaulErrorMessageColors
    }   // class Program
}   // partial namespace DLLServices2TestStand