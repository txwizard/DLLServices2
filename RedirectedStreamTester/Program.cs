/*
    ============================================================================

    Namespace:			RedirectedStreamTester

    Class Name:			Program

	File Name:			Program.cs

    Synopsis:			Test native (to the BCL) methods of detecting a
                        redirected standard stream.

    Remarks:			I have a sneaking suspicion that the new "IsRedirected"
						properties added to the Console class in version 4 of
						the Base Class Library won't cut it.

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
    2015/08/26 5.5     DAG    This class, and its project, make their first 
                              appearance.

    2016/04/03 6.0     DAG    Substitute a call into System.Reflection.Assembly
                              for an obsolete Assembly property on StateManager,
                              and adjust for StandardHandleState becoming an
                              instance method on a StateManager object.

	2016/06/06 6.3     DAG    Insert my three-clause BSD license.
    ============================================================================
*/


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


//	============================================================================
//	NOTICE: The root namespace is required, because the MagicNumbers and other
//			key classes are defined in the namespace.
//	============================================================================

using WizardWrx;
using WizardWrx.DLLServices2;

namespace RedirectedStreamTester
{
	class Program
	{
		static readonly string [ ] s_astrErrorMessagres =
        {
            Properties.Resources.ERRMSG_SUCCESS,
            Properties.Resources.ERRMSG_RUNTIME,
        };  // s_astrErrorMessagres

		static StateManager s_smTheApp = StateManager.GetTheSingleInstance ( );

		static void Main ( string [ ] args )
		{
            s_smTheApp.LoadErrorMessageTable ( s_astrErrorMessagres );
			s_smTheApp.AppExceptionLogger.OptionFlags = s_smTheApp.AppExceptionLogger.OptionFlags
														| ExceptionLogger.OutputOptions.EventLog
														| ExceptionLogger.OutputOptions.StandardError;
            TimeDisplayFormatter dtmfApp = s_smTheApp.ConsoleMessageTimeFormat;

			string strBOJMessage = string.Format (
				Properties.Resources.BOJ_MSG_TPL ,
				new string [ ]
                    {
                        s_smTheApp.AppRootAssemblyFileBaseName ,
                        System.Diagnostics.FileVersionInfo.GetVersionInfo ( System.Reflection.Assembly.GetEntryAssembly ( ).Location ).FileVersion ,
                        dtmfApp.FormatThisTime ( s_smTheApp.AppStartupTimeLocal ) ,
                        dtmfApp.FormatThisTime ( s_smTheApp.AppStartupTimeUtc ) ,
                        Environment.NewLine
                    } );
			Console.WriteLine ( strBOJMessage );
			Console.WriteLine (
				Properties.Resources.LOGMSG_ASSEMBLY_RUNTIME_VER ,
				Environment.NewLine ,
				System.Reflection.Assembly.GetEntryAssembly ( ).ImageRuntimeVersion ,
				SpecialStrings.EMPTY_STRING );

			try
			{
				Dictionary<StreamID , StreamStateInfo> dctStreamStates = GetStreamStateInfo ( );

				//	------------------------------------------------------------
				//	Label the columns.
				//	------------------------------------------------------------

				Console.WriteLine (
					SpecialStrings.SPACING_TEMPLATE ,							// Use this special template to lead or follow with a newline.
					Environment.NewLine ,										// Lead with a newline.
					Properties.Resources.MSG_STREAM_STATE_RULE2 );				// Lay out the line.
				Console.WriteLine ( Properties.Resources.MSG_STREAM_STATE_LABELS );
				Console.WriteLine ( Properties.Resources.MSG_STREAM_STATE_RULE1 );

				foreach ( StreamID enmThisStream in dctStreamStates.Keys )
				{
					StreamStateInfo ssiThisStream = dctStreamStates [ enmThisStream ];
					Console.WriteLine (
						Properties.Resources.MSG_STREAM_STATE_VALUES ,
						enmThisStream ,
						ssiThisStream.DisplayState ( ) );
				}	// foreach ( StreamID enmThisStream in dctStreamStates.Keys )
				
				//	------------------------------------------------------------
				//	Draw a line across the screen at the bottom of the report.
				//	------------------------------------------------------------

				Console.WriteLine (
					SpecialStrings.SPACING_TEMPLATE ,							// Use this special template to lead or follow with a newline.
					Properties.Resources.MSG_STREAM_STATE_RULE2 ,				// Lay out the line.
					Environment.NewLine );										// Follow with a newline.

				int intFoo = 12345;
				Console.WriteLine ( "Before calling FooBarFoo, intFoo = {0}" , intFoo );
				FooBarFoo ( intFoo );
				Console.WriteLine ( "After calling FooBarFoo, intFoo  = {0}" , intFoo );

				//	------------------------------------------------------------
				//	Everything else goes into the Windows Application Event Log.
				//	------------------------------------------------------------

				RecordInitialStateInLog (
					dctStreamStates ,
					s_smTheApp ,
					strBOJMessage );

				Console.Error.WriteLine (
					Properties.Resources.MSG_PROMPT_TO_RESUME ,					// Message Template (Format String)
					s_smTheApp.AppRootAssemblyFileBaseName ,					// Format Item 0 = Program Name
					Environment.NewLine );										// Format Item 1 = Newline, My Way

				EventLog.WriteEntry (											// and leave a copy in the event log.
					s_smTheApp.DefaultEventSourceID ,							// Event Source ID String
					string.Format (												// Message
						Properties.Resources.LOGMSG_WAIT_FOR_OPERATOR ,			// Template (Formatting String)
						s_smTheApp.AppRootAssemblyFileBaseName ) );				// Format Item 0 = Program Name

				Console.Beep ( );												// WaitForCarbonUnit emits a beep, and so do we, since this is a cheap imintation.
				Console.ReadLine ( );
				EventLog.WriteEntry (											// and leave a copy in the event log.
					s_smTheApp.DefaultEventSourceID ,							// Event Source ID String
					string.Format (												// Message
						Properties.Resources.LOGMSG_RESUMING ,					// Template (Formatting String)
						s_smTheApp.AppRootAssemblyFileBaseName ) );				// Format Item 0 = Program Name
				LogStreamTargetFileInfo (
					dctStreamStates ,
					Properties.Resources.LOGMSG_TASK_STAGE_EOJ );
			}
			catch ( Exception errAllKinds)
			{
				s_smTheApp.AppExceptionLogger.ReportException ( errAllKinds );
				s_smTheApp.AppReturnCode = MagicNumbers.ERROR_RUNTIME;
			}

			TimeSpan dtsToReport = s_smTheApp.AppUptime;						// See to it that the event log and the console report exactly the same run time.

			Console.Error.WriteLine (
				Properties.Resources.EOJ_MSG_TPL ,								// Message Template (Format String)
				s_smTheApp.AppRootAssemblyFileBaseName ,						// Format Item 0 = Program Name
				dtsToReport ,													// Format Item 1 = Program Running Time
				Environment.NewLine );											// Format Item 2 = Newline, My Way

			EventLog.WriteEntry (
				s_smTheApp.DefaultEventSourceID ,								// Event Source ID String
				string.Format (													// Message
					Properties.Resources.LOGMSG_EOJ ,							// Message Template (Format String)
					s_smTheApp.AppRootAssemblyFileBaseName ,					// Format Item 0 = Program Name
					dtsToReport ,   											// Format Item 1 = Newline, My Way
					Environment.NewLine ) );									// Format Item 2 = Newline, My Way

			Console.Beep ( );													// WaitForCarbonUnit emits a beep, and so do we, since this is a cheap imitation.
			Console.ReadLine ( );
			Environment.Exit (
				s_smTheApp.AppReturnCode > MagicNumbers.ERROR_SUCCESS
					? s_smTheApp.AppReturnCode
					: MagicNumbers.ERROR_SUCCESS );
		}	// static void Main




		private static void FooBarFoo ( int pintFoo )
		{
			Console.WriteLine ( "     Entering FooBarFoo, pintFoo = {0}" , pintFoo );
			pintFoo = pintFoo * pintFoo;
			Console.WriteLine ( "     Leaving FooBarFoo, pintFoo  = {0}" , pintFoo );
		}	// private static void FooBarFoo


		private static Dictionary<StreamID , StreamStateInfo> GetStreamStateInfo ( )
		{
			CmdLneArgsBasic argsParsed = new CmdLneArgsBasic (
				new string [ ]
				{
					Properties.Resources.ARGNAME_STDERR ,
					Properties.Resources.ARGNAME_STDIN ,
					Properties.Resources.ARGNAME_STDOUT
				} ,
				CmdLneArgsBasic.ArgMatching.CaseInsensitive );

			Dictionary<StreamID , StreamStateInfo> rStreamStates = new Dictionary<StreamID , StreamStateInfo> ( argsParsed.Count );

			foreach ( string strInternalArgName in argsParsed.Keys )
			{
				string strExternalArgName = CmdLneArgsBasic.ArgNameFromKeyValue ( strInternalArgName );

				StreamID enmState = ( StreamID ) Enum.Parse (
						typeof ( StreamID ) ,
						strExternalArgName ,
						MagicBooleans.ENUM_PARSE_CASE_INSENSITIVE );

				StreamStateInfo enmStateInfo = new StreamStateInfo (
						strExternalArgName ,
						argsParsed.GetArgByName (
							strExternalArgName ,
							Properties.Resources.ARGVALUE_DEFAULT ) );
				rStreamStates.Add (
					enmState ,
					enmStateInfo );
			}	// foreach ( string strArgName in argsParsed.Keys )

			return rStreamStates;
		}	// private static Dictionary<StreamState , StreamStateInfo> GetStreamStateInfo


		private static void RecordInitialStateInLog (
			Dictionary<StreamID , StreamStateInfo> pdctStreamStates ,
			StateManager psmTheApp ,
			string pstrBOJMessage )
		{
			string strRegisteredEventSourceIDString = psmTheApp.DefaultEventSourceID;

			EventLog.WriteEntry (
				strRegisteredEventSourceIDString ,
				pstrBOJMessage );

			EventLog.WriteEntry (
				strRegisteredEventSourceIDString ,
				string.Format (
					Properties.Resources.LOGMSG_ASSEMBLY_RUNTIME_VER ,
					SpecialStrings.EMPTY_STRING ,
					System.Reflection.Assembly.GetEntryAssembly ( ).ImageRuntimeVersion ,
					Environment.NewLine ) );

			//	----------------------------------------------------------------
			//	Organize the information about the state of the console handles
			//	into a single message.
			//	----------------------------------------------------------------

			StringBuilder sbMsgForLog = new StringBuilder ( MagicNumbers.CAPACITY_08KB );

			sbMsgForLog.AppendFormat (
				Properties.Resources.LOGMSG_RUN_PARAMETERS ,
				Environment.NewLine );

			sbMsgForLog.AppendFormat (
				Properties.Resources.LOGMSG_CWD ,
				psmTheApp.InitialWorkingDirectoryName ,
				Environment.NewLine );

			sbMsgForLog.AppendFormat (
				Properties.Resources.LOGMSG_ACTUAL_STDOUT_STATE ,
				pdctStreamStates [ StreamID.STDOUT ].DisplayState ( ) ,
				Environment.NewLine );
			sbMsgForLog.AppendFormat (
				Properties.Resources.LOGMSG_ACTUAL_STDIN_STATE ,
				pdctStreamStates [ StreamID.STDIN ].DisplayState ( ) ,
				Environment.NewLine );
			sbMsgForLog.AppendFormat (
				Properties.Resources.LOGMSG_ACTUAL_STDERR_STATE ,
				pdctStreamStates [ StreamID.STDERR ].DisplayState ( ) ,
				Environment.NewLine );

			sbMsgForLog.AppendFormat (
				Properties.Resources.LOGMSG_REPORTED_STDOUT_STATE ,
//				Properties.Resources.LOGMSG_STREAM_STATE_UNKNOWN ,
				s_smTheApp.StandardHandleState ( StateManager.ShsStandardHandle.ShSStdIn ) == StateManager.ShsHandleState.ShsRedirected
					? Properties.Resources.LOGMSG_STREAM_REDIRECTED
					: Properties.Resources.LOGMSG_STREAM_DEFAULT ,
				//				Console.IsOutputRedirected ?
				//					Properties.Resources.LOGMSG_STREAM_REDIRECTED :
				//					Properties.Resources.LOGMSG_STREAM_DEFAULT ,
				Environment.NewLine );
			sbMsgForLog.AppendFormat (
				Properties.Resources.LOGMSG_REPORTED_STDIN_STATE ,
//				Properties.Resources.LOGMSG_STREAM_STATE_UNKNOWN ,
				s_smTheApp.StandardHandleState ( StateManager.ShsStandardHandle.ShsStdOut ) == StateManager.ShsHandleState.ShsRedirected
					? Properties.Resources.LOGMSG_STREAM_REDIRECTED
					: Properties.Resources.LOGMSG_STREAM_DEFAULT ,
				//				Console.IsOutputRedirected ?
				//					Properties.Resources.LOGMSG_STREAM_REDIRECTED :
				//					Properties.Resources.LOGMSG_STREAM_DEFAULT ,
				Environment.NewLine );
			sbMsgForLog.AppendFormat (
				Properties.Resources.LOGMSG_REPORTED_STDERR_STATE ,
//				Properties.Resources.LOGMSG_STREAM_STATE_UNKNOWN ,
				s_smTheApp.StandardHandleState ( StateManager.ShsStandardHandle.ShsStdEror ) == StateManager.ShsHandleState.ShsRedirected
					? Properties.Resources.LOGMSG_STREAM_REDIRECTED
					: Properties.Resources.LOGMSG_STREAM_DEFAULT ,
				//				Console.IsOutputRedirected ?
				//					Properties.Resources.LOGMSG_STREAM_REDIRECTED :
				//					Properties.Resources.LOGMSG_STREAM_DEFAULT ,
				Environment.NewLine );

			EventLog.WriteEntry (
				strRegisteredEventSourceIDString ,
				sbMsgForLog.ToString ( ) );

			//	----------------------------------------------------------------
			//	Roll through the collection, reporting as needed on any stream
			//	that has an associated file.
			//	----------------------------------------------------------------

			LogStreamTargetFileInfo (
				pdctStreamStates ,
				Properties.Resources.LOGMSG_TASK_STAGE_BOJ );
		}	// private static void RecordInitialStateInLog


		private static void LogStreamTargetFileInfo (
			Dictionary<StreamID , StreamStateInfo> pdctStreamStates ,
			string pstrReportStage )
		{
			StringBuilder sbMsgForLog = new StringBuilder ( MagicNumbers.CAPACITY_08KB );

			foreach ( StreamID enmThisStream in pdctStreamStates.Keys )
			{
				StreamStateInfo ssiThisStream = pdctStreamStates [ enmThisStream ];

				if ( !string.IsNullOrEmpty ( ssiThisStream.TargetFileName ) )
				{	// If the command line names a file to which the stream is redirected, gather and report statistics on it.
					if ( sbMsgForLog.Length == ListInfo.EMPTY_STRING_LENGTH )
					{
						sbMsgForLog.AppendFormat (
							Properties.Resources.LOGMSG_STATE_OF_AFFAIRS ,		// Message template
							pstrReportStage ,									// Format Item 0 = Reporting Stage
							Environment.NewLine );								// Format Item 1 = Newline, My Way
					}	// if ( sbMsgForLog.Length == ListInfo.EMPTY_STRING_LENGTH )

					System.IO.FileInfo fiAssociatedFile = new System.IO.FileInfo ( ssiThisStream.TargetFileName );

					sbMsgForLog.AppendFormat (
						Properties.Resources.LOGMSG_STREAM_FILE_STATE ,			// Message template
						enmThisStream ,											// Format Item 0 = Associated stream
						fiAssociatedFile.FullName ,								// Format Item 1 = Fully qualified name.
						Environment.NewLine );									// Format Item 2 = Newline, My Way
					sbMsgForLog.AppendFormat (
						Properties.Resources.LOGMSG_STREAM_FILE_PRESENT ,		// Message template
						fiAssociatedFile.Exists ,								// Format item 0 = File exists (True or False)
						Environment.NewLine );									// Format Item 1 = Newline, My Way

					if ( fiAssociatedFile.Exists )
					{	// If the file exists, record selected statistics from the file system.
						sbMsgForLog.AppendFormat (								// Message template
							Properties.Resources.LOGMSG_STREAM_FILE_SIZE ,
							fiAssociatedFile.Length.ToString (
								DisplayFormats.NUMBER_PER_REG_SETTINGS_0D ) ,	// Format Item 0 = Length (size) in bytes
								Environment.NewLine );							// Format Item 1 = Newline, My Way
						sbMsgForLog.AppendFormat (								// Message template
							Properties.Resources.LOGMSG_STREAM_FILE_CREATED ,
							DisplayFormats.FormatDateTimeForShow (
								fiAssociatedFile.CreationTimeUtc ) ,			// Format Item 1 = Create time (UTC)
								Environment.NewLine );							// Format Item 1 = Newline, My Way
						sbMsgForLog.AppendFormat (								// Message template
							Properties.Resources.LOGMSG_STREAM_FILE_MODIFIED ,	// Message template
							DisplayFormats.FormatDateTimeForShow (
								fiAssociatedFile.LastAccessTimeUtc ) ,			// Format Item 1 = Create time (UTC)
								Environment.NewLine );							// Format Item 1 = Newline, My Way
						sbMsgForLog.AppendFormat (
							Properties.Resources.LOGMSG_STREAM_FILE_ACCESSED ,	// Message template
							DisplayFormats.FormatDateTimeForShow (
								fiAssociatedFile.LastAccessTimeUtc ) ,			// Format Item 1 = Create time (UTC)
								Environment.NewLine );							// Format Item 1 = Newline, My Way
					}	// if ( fiAssociatedFile.Exists )
				}	// if ( !string.IsNullOrEmpty ( ssiThisStream.TargetFileName ) )
			}	// foreach ( StreamID enmThisStream in pdctStreamStates.Keys )

			if ( sbMsgForLog.Length > ListInfo.EMPTY_STRING_LENGTH )
			{	// There is something to write. Otherwise, be quiet.
				EventLog.WriteEntry (
					s_smTheApp.DefaultEventSourceID ,
					sbMsgForLog.ToString ( ) );
			}	// if ( sbMsgForLog.Length > ListInfo.EMPTY_STRING_LENGTH )
		}	// private static void LogStreamTargetFileInfo
	}	// class Program
}	// partial namespace RedirectedStreamTester