using System;

using WizardWrx;
using WizardWrx.Core;

namespace POC
{
	class Program
	{
		static void Main ( string [ ] args )
		{
			Console.WriteLine ( BasicAppHelpers.CreateStartupBanner ( ) );

			CmdLneArgsBasic argV = new CmdLneArgsBasic (
				new string [ ]
				{
					Properties.Resources.CMD_ARG_INPUT ,
					Properties.Resources.CMD_ARG_PATTERN ,
					Properties.Resources.CMD_ARG_REPLACEWITH
				} ,
				CmdLneArgsBasic.ArgMatching.CaseInsensitive );

			//	----------------------------------------------------------------
			//	Create the RegExMatch task, and set its 2 read/write properties,
			//	Input and Pattern, then list them on the output stream.
			//	----------------------------------------------------------------

			MSBuildTask.RegexMatch matchEngine = new MSBuildTask.RegexMatch ( );

			matchEngine.Input = argV.GetArgByName (
				Properties.Resources.CMD_ARG_INPUT ,
				null ,
				CmdLneArgsBasic.BLANK_AS_DEFAULT_FORBIDDEN );
			matchEngine.Pattern = argV.GetArgByName (
				Properties.Resources.CMD_ARG_PATTERN ,
				null ,
				CmdLneArgsBasic.BLANK_AS_DEFAULT_FORBIDDEN );
			matchEngine.ReplaceWith = argV.GetArgByName (
				Properties.Resources.CMD_ARG_REPLACEWITH ,
				CmdLneArgsBasic.VALUE_NOT_SET ,
				CmdLneArgsBasic.BLANK_AS_DEFAULT_ALLOWED );

			Console.WriteLine (
				Properties.Resources.MSG_INPUTS_INPUT ,
				matchEngine.Input );
			Console.WriteLine (
				Properties.Resources.MSG_INPUTS_PATTERN ,
				matchEngine.Pattern );
			Console.WriteLine (
				Properties.Resources.MSG_INPUTS_REPLACE_WITH ,
				matchEngine.ReplaceWith );

			//	----------------------------------------------------------------
			//	Verify that both required inputs are present, and report missing
			//	values, if any.
			//	----------------------------------------------------------------

			int intNMissingArgs = ListInfo.LIST_IS_EMPTY;

			if ( string.IsNullOrEmpty ( matchEngine.Input))
			{
				Console.WriteLine (
					Properties.Resources.MSG_MISSING_ARG ,
					Properties.Resources.CMD_ARG_INPUT );
				intNMissingArgs++;
			}	// if ( string.IsNullOrEmpty ( matchEngine.Input))

			if ( string.IsNullOrEmpty ( matchEngine.Pattern ) )
			{
				Console.WriteLine (
					Properties.Resources.MSG_MISSING_ARG ,
					Properties.Resources.CMD_ARG_PATTERN );
				intNMissingArgs++;
			}	// if ( string.IsNullOrEmpty ( matchEngine.Input))

			//	----------------------------------------------------------------
			//	If required parameters are accounted for, simulate the task, and
			//	report the outcome. Otherwise, report that execution cannot
			//	continue, and return with a status code.
			//	----------------------------------------------------------------

			if ( intNMissingArgs == ListInfo.LIST_IS_EMPTY )
			{
				try
				{
					//	--------------------------------------------------------
					//	Since the MSBuild task sends output of its own to the
					//	console, its own report must be stashed in a string, to
					//	be displayed after its Execute method returns.
					//
					//	The host banner, which is wrapped in lines of hyphens,
					//	is copied into a string, so that its length can be taken
					//	without making a second trip to copy the string into the
					//	output stream.
					//
					//	Finally, Simulate replaces Execute in this simulator,
					//	since ITask insists that its Execute method be hidden.
					//	Hence, Simulate is an analogue, and both methods call a
					//	private method that does their real work.
					//	--------------------------------------------------------

					string strHostBanner = Properties.Resources.MSG_TEST_HOST_REPORT;
					string strProgressReport = string.Format (
						Properties.Resources.MSG_PROGRESS_EXECUTE ,
						matchEngine.Execute ( ) ,
						Environment.NewLine );
					string strProgressBanner = CreateStringOfCharacter (
						SpecialCharacters.HYPHEN ,
						strHostBanner.Length );

					Console.WriteLine (
						string.Concat (
							Environment.NewLine ,
							strProgressBanner ) );
					Console.WriteLine ( strHostBanner );
					Console.WriteLine ( strProgressBanner );

					Console.WriteLine ( strProgressReport );

					ReportMatches ( matchEngine );
				}
				catch ( Exception exAllKinds )
				{
					BasicAppHelpers.ReportException ( exAllKinds );
				}
			}	// TRUE (anticipated outcome+) block, if ( intNMissingArgs == WizardWrx.ListInfo.LIST_IS_EMPTY )
			else
			{
				Console.WriteLine (
					Properties.Resources.MSG_MISSING_ARGS_COUNT ,
					intNMissingArgs ,
					Environment.NewLine );
				Environment.ExitCode = MagicNumbers.ERROR_INVALID_CMD_LNE_ARGS;
			}	// FALSE (unanticipated outcome+) block, if ( intNMissingArgs == WizardWrx.ListInfo.LIST_IS_EMPTY )

			Console.WriteLine ( BasicAppHelpers.CreateShutdownBanner ( ) );

			BasicAppHelpers.AwaitCarbonUnit ( );
			Environment.Exit ( Environment.ExitCode );
		}	// static void Main


		private static string CreateStringOfCharacter ( char pchrThisCharacter , int pintThisManyOfThem )
		{
			System.Text.StringBuilder rsbFillThis = new System.Text.StringBuilder ( pintThisManyOfThem );

			for ( int intI = ArrayInfo.ARRAY_FIRST_ELEMENT ; intI < pintThisManyOfThem ; intI++ )
				rsbFillThis.Append ( pchrThisCharacter );

			return rsbFillThis.ToString ( );
		}	// CreateStringOfCharacter


		private static void ReportMatches ( MSBuildTask.RegexMatch pmatchEngine )
		{
			string [ ] astrMatches =
			{
				pmatchEngine.Match ,
				pmatchEngine.Match1 ,
				pmatchEngine.Match2 ,
				pmatchEngine.Match3 ,
				pmatchEngine.Match4 ,
				pmatchEngine.Match5 ,
				pmatchEngine.Match6 ,
				pmatchEngine.Match7 ,
				pmatchEngine.Match8 ,
				pmatchEngine.Match9
			};	// astrMatches

			Console.WriteLine (
				Properties.Resources.MSG_OUTPUTS_ISMATCH ,
				pmatchEngine.IsMatch );

			int intMatchNumber = ArrayInfo.ARRAY_INVALID_INDEX;

			foreach ( string strThisMatch in astrMatches )
			{	// All ten of these use the same format control string.
				Console.WriteLine (
					Properties.Resources.MSG_OUTPUTS_MATCHES ,
					( ++intMatchNumber ) == ArrayInfo.ARRAY_FIRST_ELEMENT
						? SpecialCharacters.SPACE_CHAR.ToString ( )
						: intMatchNumber.ToString ( ) ,
					strThisMatch );
			}	// foreach ( string strThisMatch in astrMatches )

			if ( !string.IsNullOrEmpty ( pmatchEngine.Replacement ) )
			{
				Console.WriteLine (
					Properties.Resources.MSG_OUTPUTS_REPLACEMENT ,
					pmatchEngine.Replacement );
			}	// if ( !string.IsNullOrEmpty ( pmatchEngine.Replacement ) )
		}	// ReportMatches
	}	// class Program
}	// namespace POC