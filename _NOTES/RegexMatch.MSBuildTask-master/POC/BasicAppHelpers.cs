using System;
using System.Reflection;
using System.Text;

using WizardWrx;

namespace POC
{
	static class BasicAppHelpers
	{
		static readonly DateTime s_dtmStart = System.Diagnostics.Process.GetCurrentProcess ( ).StartTime;

		internal static void AwaitCarbonUnit ( )
		{
			Console.Error.WriteLine ( Properties.Resources.MSG_AWAIT_CARBON_UNIT );
			Console.ReadLine ( );
		}	// internal static void AwaitCarbonUnit ( )


		internal static string CreateShutdownBanner ( )
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
		}	// internal static string CreateShutdownBanner ( )


		internal static string CreateStartupBanner ( )
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
		}	// internal static string CreateStartupBanner


		internal static void ReportException ( Exception pexAllKinds )
		{
			Console.WriteLine (
				Properties.Resources.MSG_EXCEPTION_REPORT_PROLOGUE ,		// Format Control String
				pexAllKinds.GetType ( ).FullName ,							// Format Item 0 = Absolute (Fully Qualified) Exception Type
				pexAllKinds.Message ,										// Format Item 1 = Exception Message
				Environment.NewLine );										// Format Item 2 = Embedded Newline
			Console.WriteLine (
				Properties.Resources.MSG_EXCEPTION_REPORT_METHOD ,
				pexAllKinds.TargetSite );
			Console.WriteLine (
				Properties.Resources.MSG_EXCEPTION_REPORT_SOURCE ,
				pexAllKinds.Source );
			Console.WriteLine (
				Properties.Resources.MSG_EXCEPTION_REPORT_STACKTRACE ,
				AlignStackTrace (
					Properties.Resources.MSG_EXCEPTION_REPORT_STACKTRACE ,
					pexAllKinds.StackTrace ) );

			if ( pexAllKinds.InnerException == null )
			{	// There is usually just one exception, without nested exceptions attached to it.
				Console.WriteLine (
					Properties.Resources.MSG_EXCEPTION_REPORT_EPOLOGUE_END ,
					Environment.NewLine );
			}	// TRUE (anticipated outcome) block, if ( pexAllKinds.InnerException == null )
			else
			{	// There is at least one inner exception; take a reference to it, since we need two of its properties.
				Exception exInner = pexAllKinds.InnerException;
				Console.WriteLine (
					Properties.Resources.MSG_EXCEPTION_REPORT_EPOLOGUE_MORE ,	// Format Control String
					exInner.GetType ( ).FullName ,								// Format Item 0 = Absolute (fully qualified) inner exception type
					pexAllKinds.Message ,										// Format Item 1 = Message property on the inner exception
					Environment.NewLine );										// Format Item 2 = Embedded Newline to add white space below.
			}	// FALSE (unanticipated outcome) block, if ( pexAllKinds.InnerException == null )
			Environment.ExitCode = WizardWrx.MagicNumbers.ERROR_RUNTIME;
		}	// ReportException


		private static string AlignStackTrace (
			string pstrFormatControlString ,
			string pstrStackTrace )
		{
			const string ARG_NAME_FORMAT_CONTROL_STRING = @"pstrFormatControlString";

			//	----------------------------------------------------------------
			//	I wish that C# supported declarations inside the condition block
			//	of its IF statements, as do C and C++.
			//	----------------------------------------------------------------

			int intFormatItemStartPosition = pstrFormatControlString.IndexOf ( SpecialCharacters.DLM_FORMAT_ITEM_BEGIN );

			if ( intFormatItemStartPosition > ListInfo.INDEXOF_NOT_FOUND )
			{	// First, replace all Environment.Newline strings with bare carriage returns, then split the string at every carriage return.
				string [ ] astrStackFrames = pstrStackTrace.Replace (
					Environment.NewLine ,
					SpecialStrings.STRING_SPLIT_CARRIAGE_RETURN ).Split (
						SpecialCharacters.CARRIAGE_RETURN );

				if ( astrStackFrames.Length > ArrayInfo.ARRAY_SECOND_ELEMENT )
				{
					//	--------------------------------------------------------
					//	Create a StringBuilder that has enough room to hold the
					//	input StackTrace string plus the number of characters 
					//	to be occupied by the additional left padding. Since the
					//	first string goes in without padding, while subsequent
					//	strings get left padding, the constructor uses it as its
					//	initial value.
					//
					//	The remaining strings are added by a classic FOR loop
					//	that starts with the second element. The loop performs
					//	the following transformations.
					//
					//	1)	Trim away leading and trailing white space.
					//
					//	2)	Left pad so that it aligns with the frame above it.
					//
					//	Sine the first transformation affects the first string,
					//	too, the StringBuilder constructor handles it inline.
					//
					//	IMPORTANT:	Since PadLeft takes int account the overall
					//				length of the string, to truly left pad by N
					//				characters, the integer passed into PadLeft
					//				must account for the length of the input
					//				string AND the desired padding.
					//
					//				A new string that is equivalent to this
					//              instance, but left-aligned and padded on the
					//              right with as many spaces as needed to
					//              create a length of totalWidth. However, if
					//              totalWidth is less than the length of this
					//              instance, the method returns a reference to
					//              the existing instance. If totalWidth is
					//              equal to the length of this instance, the
					//              method returns a new string that is
					//              identical to this instance.
					//
					//				Reference: String.PadRight Method (Int32)
					//				           https://msdn.microsoft.com/en-us/library/34d75d7s(v=vs.110).aspx
					//	--------------------------------------------------------

					StringBuilder rsbAlgned = new StringBuilder (
						astrStackFrames [ ArrayInfo.ARRAY_FIRST_ELEMENT ].Trim ( ) ,
						pstrStackTrace.Length + ( astrStackFrames.Length * intFormatItemStartPosition ) );

					for ( int intStackFrame = ArrayInfo.ARRAY_SECOND_ELEMENT ;
						      intStackFrame < astrStackFrames.Length ;
							  intStackFrame++ )
					{	// Use the native PadLeft method on System.String that I tend to forget about until I need it again.
						rsbAlgned.Append ( Environment.NewLine );
						rsbAlgned.Append ( astrStackFrames [ intStackFrame ].Trim ( ).LeftPadNChars ( intFormatItemStartPosition ) );
					}	// for ( int intStackFrame = ArrayInfo.ARRAY_SECOND_ELEMENT ; intStackFrame < astrStackFrames.Length ; intStackFrame++ )

					return rsbAlgned.ToString ( );
				}	// TRUE (typical case) block, if ( astrStackFrames.Length > ArrayInfo.ARRAY_SECOND_ELEMENT )
				else
				{	// The degenerate case is a crash that happens in the entry point routine, which has a single-item stack trace.
					return pstrFormatControlString;
				}	// FALSE (degenerate case) block, if ( astrStackFrames.Length > ArrayInfo.ARRAY_SECOND_ELEMENT )
			}	// TRUE (anticipated outcome) block, if ( intFormatItemStartPosition > WizardWrx.ListInfo.INDEXOF_NOT_FOUND )
			else
			{	// Though the exception at hand doesn't strictly meet the definition of an ArgumentOutOfRangeException exception, that overload facilitates reporting both the name and value of the invalid argument.
				throw new ArgumentOutOfRangeException (
					ARG_NAME_FORMAT_CONTROL_STRING ,
					pstrFormatControlString ,
					Properties.Resources.ERRMSG_MISSING_FORMAT_ITEM );
			}	// FALSE (unanticipated outcome) block, if ( intFormatItemStartPosition > WizardWrx.ListInfo.INDEXOF_NOT_FOUND )
		}	// AlignStackTrace


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
	}	// static class BasicAppHelpers
}	// partial namespace POC