//	References:			1)	"How To: Implementing Custom Tasks - Part I"
//							https://blogs.msdn.microsoft.com/msbuild/2006/01/21/how-to-implementing-custom-tasks-part-i/

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;


namespace MSBuildTask
{
	/// <summary>
	/// Use this class to perform operations that use regular expressions to
	/// test strings against patterns or to transform them by replacing part of
	/// the input string with new text.
	/// </summary>
	public class RegexMatch : Task
	{
		#region Constructors
		/// <summary>
		/// Give this class a first class public default constructor that
		/// initializes all of its private members, even though it has already a
		/// protected constructor that it inherits from its base class, and I am
		/// virtually certain that the base constructor gets called anyway.
		/// </summary>
		public RegexMatch ( )
		{
			_strPattern = string.Empty;
			_strInput = string.Empty;
			_strReplaceWith = string.Empty;
			_strReplacement = string.Empty;
			_fIsMatch = false;
			_strMatch = string.Empty;

			_astrGroups = new string [ 9 ];

			ResetGroups ( );
		}	// RegexMatch (the one and only) is in charge of initializing everything.
		#endregion	// Constructors


		#region Public Input Properties (Read/Write)
		/// <summary>
		/// Specify the input to evaluate against Pattern.
		/// </summary>
		[Required]
		public string Input
		{
			get { return _strInput; }
			set { _strInput = value; }
		}	// Input Property (Read/Write)

		/// <summary>
		/// Specify the pattern to evaluate against Input or drive a 
		/// transformation that uses ReplaceWith.
		/// </summary>
		[Required]
		public string Pattern
		{
			get { return _strPattern; }
			set { _strPattern = value; }
		}	// Pattern Property (Read/Write)

		/// <summary>
		/// Specify text to replace the text that matches part or all of the
		/// matched text. See Remarks.
		/// </summary>
		/// <remarks>
		/// To replace a portion of the text, combine capturing groups and back
		/// references.
		/// </remarks>
		public string ReplaceWith
		{
			get { return _strReplaceWith; }
			set { _strReplaceWith = value; }
		}	// ReplaceWith Property (Read/Write)
		#endregion	// Public Input Properties (Read/Write)


		#region Output Match Properties (ReadOnly)
		/// <summary>
		/// After Execute is called, this property returns TRUE if the Pattern
		/// matched the Input, otherwise, it returns FALSE. Until Execute is
		/// called on the instance, this property returns FALSE, and is
		/// meaningless.
		/// </summary>
		[Output]
		public bool IsMatch
		{
			get { return _fIsMatch; }
		}	// IsMatch Property (ReadOnly)

		/// <summary>
		/// After Execute is called, this property returns the value that
		/// matched the pattern if the match succeeded, indicated by the
		/// IsMatch property being TRUE. Otherwise, it returns the empty
		/// string. Until Execute happens, its value is meaningless.
		/// </summary>
		[Output]
		public string Match
		{
			get { return _strMatch; }
		}	// Match Property (ReadOnly)


		/// <summary>
		/// Match zero corresponds to the whole match; this property and the
		/// Match property usually have the same value.
		/// </summary>
		[Output]
		public string Match1
		{
			get { return _astrGroups [ 0 ]; }
		}	// Match1 Property (ReadOnly)


		/// <summary>
		/// Match 2 corresponds to the first parenthetical expression.
		/// </summary>
		[Output]
		public string Match2
		{
			get { return _astrGroups [ 1 ]; }
		}	// Match2 Property (ReadOnly)


		/// <summary>
		/// Match 3 corresponds to the second parenthetical expression.
		/// </summary>
		[Output]
		public string Match3
		{
			get { return _astrGroups [ 2 ]; }
		}	// Match3 Property (ReadOnly)


		/// <summary>
		/// Match 4 corresponds to the third parenthetical expression.
		/// </summary>
		[Output]
		public string Match4
		{
			get { return _astrGroups [ 3 ]; }
		}	// Match4 Property (ReadOnly)


		/// <summary>
		/// Match 5 corresponds to the fourth parenthetical expression.
		/// </summary>
		[Output]
		public string Match5
		{
			get { return _astrGroups [ 4 ]; }
		}	// Match5 Property (ReadOnly)


		/// <summary>
		/// Match 6 corresponds to the fifth parenthetical expression.
		/// </summary>
		[Output]
		public string Match6
		{
			get { return _astrGroups [ 5 ]; }
		}	// Match6 Property (ReadOnly)


		/// <summary>
		/// Match 7 corresponds to the sixth parenthetical expression.
		/// </summary>
		[Output]
		public string Match7
		{
			get { return _astrGroups [ 6 ]; }
		}	// Match7 Property (ReadOnly)


		/// <summary>
		/// Match 8 corresponds to the seventh parenthetical expression.
		/// </summary>
		[Output]
		public string Match8
		{
			get { return _astrGroups [ 7 ]; }
		}	// Match8 Property (ReadOnly)


		/// <summary>
		/// Match 9 corresponds to the eighth parenthetical expression.
		/// </summary>
		[Output]
		public string Match9
		{
			get { return _astrGroups [ 8 ]; }
		}	// Match9 Property (ReadOnly)
		#endregion	// Output Match Properties (ReadOnly)


		#region Other Output Properties (ReadOnly)
		/// <summary>
		/// Replacement is the new text produced by the ReplaceWith property.
		/// 
		/// Please see the remarks under the ReplaceWith property for additional details.
		/// </summary>
		[Output]
		public string Replacement
		{
			get { return _strReplacement; }
		}	// Replacement Property (ReadOnly)
		#endregion	// Other Output Properties (ReadOnly)



		/// <summary>
		/// Execute the task.
		/// </summary>
		/// <returns>
		/// true if the task successfully executed; otherwise, false
		/// 
		/// The current implementation of this method always returns TRUE.
		/// </returns>
		public override bool Execute ( )
		{
			ResetGroups ( );								// The constructor does this, too.

			Match match = Regex.Match (						// Since it contributes nothing useful, this code bypasses the property getter methods.
				_strInput ,									// Input against which to apply Pattern, bypassing the Getter method
				_strPattern );								// Pattern to apply to Input, bypassing the Getter method

			if ( _fIsMatch = match.Success )
			{	// If the expression matched the pattern, update the main Match property and the nine capturing group properties.
				_strMatch = match.Value;

				foreach ( var group in match.Groups.Cast<Group> ( ).Take ( 9 ).Select ( ( g , i ) => new { Index = i , g.Value } ) )
				{
					_astrGroups [ group.Index ] = group.Value;
				}	// foreach ( var group in match.Groups.Cast<Group> ( ).Take ( 9 ).Select ( ( g , i ) => new { Index = i , g.Value } ) )

				if ( !string.IsNullOrEmpty ( _strReplaceWith ) )
				{	// If the input includes a replacement, make it so.
					_strReplacement = Regex.Replace (
						_strInput ,
						_strPattern ,
						_strReplaceWith );
				}	// if ( !string.IsNullOrEmpty ( _strReplaceWith ) )
			}	// if (_fIsMatch = match.Success )

			//	----------------------------------------------------------------
			//	Regardless of the outcome, one final message gets logged
			//	somewhere. Since both messages use the same template and
			//	parameter list, it's more efficient to write one block of code
			//	to build the array, since it occupies space on the managed heap,
			//	whether it is defined inline or in advance.
			//	----------------------------------------------------------------

			object [ ] objMsgParams = new object [ ]
			{
				this.GetType ( ).FullName ,					// Format Item  0: TaskFactory = {0}
				_strInput ,									// Format Item  1: Input       = {1}
				_strPattern ,								// Format Item  2: Pattern     = {2}
				_strReplaceWith ,							// Format Item  3: ReplaceWith = {3}
				_fIsMatch ,									// Format Item  4: IsMatch     = {4}
				_strMatch ,									// Format Item  5: Match       = {5}
				_astrGroups [ 0 ] ,							// Format Item  6: Match 1     = {6}
				_astrGroups [ 1 ] ,							// Format Item  7: Match 2     = {7}
				_astrGroups [ 2 ] ,							// Format Item  8: Match 3     = {8}
				_astrGroups [ 3 ] ,							// Format Item  9: Match 4     = {9}
				_astrGroups [ 4 ] ,							// Format Item 10: Match 5     = {10}
				_astrGroups [ 5 ] ,							// Format Item 11: Match 6     = {11}
				_astrGroups [ 6 ] ,							// Format Item 12: Match 7     = {12}
				_astrGroups [ 7 ] ,							// Format Item 13: Match 8     = {13}
				_astrGroups [ 8 ] ,							// Format Item 14: Match 9     = {14}
				_strReplacement ,							// Format Item 15: Replacement = {15}
				Environment.NewLine							// Format Item 16: Embedded newlines are much easier to slip into a resource string by this mechanism.
			};	// objMsgParams

			if ( base.BuildEngine == null )
			{	// Log the message on the standard output console stream.
				Console.WriteLine (
					Properties.Resources.MSG_TPL_SIMULATION_ALERT ,
					Environment.NewLine );
				Console.WriteLine (
					Properties.Resources.MSG_TPL_SUMMARY_FOR_LOG ,
					objMsgParams );
			}	// TRUE (The task is executing on behalf of a test stand assembly.) block, if ( base.BuildEngine == null )
			else
			{	// Send the message to the build engine logger.
				BuildMessageEventArgs msg = new BuildMessageEventArgs (
					string.Format ( Properties.Resources.MSG_TPL_SUMMARY_FOR_LOG , objMsgParams ) ,
					string.Empty ,							// HelpKeyword
					this.GetType ( ).Name ,					// Sender
					MessageImportance.Normal );				// MessgeImportance
				base.BuildEngine.LogMessageEvent ( msg );	// Send by raising an event.
			}	// FALSE (The task is executing on behalf of a build engine.) block, if ( base.BuildEngine == null )

			return true;
		}	// Execute Method


		#region Private Instance Methods
		private void ResetGroups ( )
		{
			for ( int i = 0 ; i < _astrGroups.Length ; i++ ) _astrGroups [ i ] = string.Empty;
		}	// ResetGroups Method
		#endregion	// Private Instance Methods


		#region Private Instance Storage
		private string _strPattern;
		private string _strInput;
		private string _strReplaceWith;
		private string _strReplacement;
		private bool _fIsMatch;
		private string _strMatch;
		private string [ ] _astrGroups;
		#endregion	// Private Instance Storage
	}	// public class RegexMatch
}	// namespace MSBuildTask