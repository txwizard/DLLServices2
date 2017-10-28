/*
    ============================================================================

    Namespace Name:     WizardWrx

    Class Name:         StringExtensions

    File Name:          StringExtensions.cs

    Synopsis:           This sealed class exposes functions that add features to
                        manipulate strings in ways that were omitted from the
                        base class libraries of the Microsoft .NET Framework.

    Remarks:            This class was created by cloning the StringTricks class
                        and converting everything in it to an extension method.
 
                        The like named methods in StringTricks remain, marked as
                        legal, though obsolete.

						LengthOfLongestString didn't make the cut, since its
						input is an array.
 
						ReplaceToken didn't make the cut, because it is 
						redundant, which I didn't realize when I wrote it.

	Author:             David A. Gray

	License:            Copyright (C) 2017, David A. Gray. All rights reserved.

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

    Date Written:       Saturday, 16 July 2017

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version By  Synopsis
    ---------- ------- --- --------------------------------------------------
    2017/07/17 7.0     DAG This class makes its first appearance.

    2017/08/26 7.0     DAG Define CountCharacterOccurrences, analogous to
                           CountSubstrings, to eliminate the need for a ToString
                           to create a string from SpecialCharacters.ENV_STR_DLM
                           in CountUnresolvedEnvironmentStrings.

	2017/08/29 7.0     DAG Define two more extension methods, EnsureFirstCharIs
                           and EnsureLastCharIs discovered missing while
                           correcting an oversight in the test program.
    ============================================================================
*/

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using WizardWrx.Common;


namespace WizardWrx
{
    /// <summary>
    /// This is a class of extension methods for performing common tasks not
    /// provided by the System.String class. All but the four Pad methods are
	/// derived from long established routines in companion class StringTricks.
	/// 
	/// Just as importing the System.Linq namespace makes its generic extension
	/// methods visible, importing the root WizardWrx namespace, accompanied by
	/// a reference to WizardWrx.Core, makes these methods visible on every
	/// instance of System.string.
    /// </summary>
	public static class StringExtensions
    {
        #region Public Constants
        /// <summary>
        /// Default token terminator string used by the version of public static
        /// method, MakeToken, which takes one argument.
        /// </summary>
        public const string DEFAULT_TOKEN_DELM = @"$$";
		#endregion	// Public Constants


		#region AppendFullStopIfMissing Method
		/// <summary>
		/// Unless the last character of the input string is a period (full
		/// stop), append one to the returned string.
		/// </summary>
		/// <param name="pstrInput">
		/// Specify the input string to evaluate and edit as needed.
		/// </param>
		/// <returns>
		/// The input string is returned with a period appended to it. If it already
		/// has one, the input string is returned unchanged.
		/// </returns>
		public static string AppendFullStopIfMissing ( this string pstrInput )
		{
			const string FULL_STOP_AS_STRING = @".";

			if ( pstrInput.EndsWith ( FULL_STOP_AS_STRING ) )
				return pstrInput;
			else
				return string.Concat ( pstrInput , FULL_STOP_AS_STRING );
		}	// AppendFullStopIfMissing
		#endregion	// AppendFullStopIfMissing Method


		#region ArrayOfOne Methods
		/// <summary>
		/// Return a one-element array containing the input character, for use
		/// as input to the Split method of the System.string class.
		/// </summary>
		/// <param name="pchrTheCharacter">
		/// Specify the character to be copied into an array of one.
		/// </param>
		/// <returns>
		/// The return value is an array of one element, ready to feed to the
		/// string.split method, or anything else that needs an array of one
		/// character.
		/// </returns>
		public static char [ ] ArrayOfOne ( this char pchrTheCharacter )
		{
			return new char [ ] { pchrTheCharacter };
		}   // ArrayOfOne (1 of 2)

		/// <summary>
		/// Return a one-element array containing the input string, for use
		/// as input to the Split method of the System.string class.
		/// </summary>
		/// <param name="pstrTheString">
		/// Specify the string to be copied into an array of one.
		/// </param>
		/// <returns>
		/// The return value is an array of one element, ready to feed to the
		/// string.split method, or anything else that needs an array of one
		/// string.
		/// </returns>
		public static string [ ] ArrayOfOne ( this string pstrTheString )
		{
			return new string [ ] { pstrTheString };
		}   // ArrayOfOne (2 of 2)
		#endregion	// ArrayOfOne Methods


		#region Chop Methods
		/// <summary>
		/// Return a new string with the terminal newline, if present, removed.
		/// </summary>
		/// <param name="pstrIn">
		/// Specify the string to be chopped.
		/// </param>
		/// <returns>
		/// The chopped string is returned, minus its newline if it contained
		/// one. This method treats all newlines equally, meaning that any of
		/// the following items is treated as a newline.
		/// 
		/// 1) Environment.Newline
		/// 2) SpecialStrings.STRING_SPLIT_CARRIAGE_RETURN, a bare carriage return (0x0D)
		/// 3) SpecialStrings.STRING_SPLIT_LINEFEED, a bare line feed (0x0A)
		/// </returns>
		/// <see cref="SpecialStrings.STRING_SPLIT_CARRIAGE_RETURN"/>
		/// <see cref="SpecialStrings.STRING_SPLIT_LINEFEED "/>
		/// <see cref="System.Environment.NewLine"/>
		public static string Chop ( this string pstrIn )
		{
			if ( string.IsNullOrEmpty ( pstrIn ) )
			{	// Since they get identical treatment, a null reference and the empty string collapse into a single degenerate case 1, of 2,
				return pstrIn;
			}
			else if ( pstrIn.EndsWith ( Environment.NewLine ) )
			{	// The string ends with a Windows Newline pair (CR/LF), case 1 of 3.
				return pstrIn.Substring ( 
					WizardWrx.ArrayInfo.ARRAY_FIRST_ELEMENT ,
					pstrIn.Length - Environment.NewLine.Length );
			}
			else if ( pstrIn.EndsWith ( SpecialStrings.STRING_SPLIT_CARRIAGE_RETURN ) )
			{	// The string ends with a Macintosh newline character (0x0d), case 2 of 3.
				return pstrIn.Substring (
					WizardWrx.ArrayInfo.ARRAY_FIRST_ELEMENT ,
					pstrIn.Length - SpecialStrings.STRING_SPLIT_CARRIAGE_RETURN.Length );
			}
			else if ( pstrIn.EndsWith ( SpecialStrings.STRING_SPLIT_LINEFEED ) )
			{	// The string ends with a Unix newline character (0x0a), case 3 of 3.
				return pstrIn.Substring (
					WizardWrx.ArrayInfo.ARRAY_FIRST_ELEMENT ,
					pstrIn.Length - SpecialStrings.STRING_SPLIT_LINEFEED.Length );
			}
			else
			{	// The last character is something else; return the string as is, degenerate case 2 of 2.
				return pstrIn;
			}
		}	// Chop
		#endregion	// Chop Methods


		#region CountCharacterOccurrences Method
		/// <summary>
		/// Strangely, the String class is missing an important static method to
		/// count occurrences of a specified character within a string. This is
		/// that missing method.
		/// </summary>
		/// <param name="pstrSource">
		/// Specify the string in which to count occurrences of substring 
		/// pstrToCount.
		///
		/// If pstrSource is null or empty, the method returns zero.
		/// 
		/// Since this is an extension method, pstrIn is supplied by the BCL
		/// when it binds this method to an instance of System.string.
		/// </param>
		/// <param name="pchrToCount">
		/// Specify the substring to count in string pstrSource. An empty string
		/// causes the method to return MagicNumbers.STRING_INDEXOF_NOT_FOUND,
		/// or -1.
		/// </param>
		/// <returns>
		/// The return value is the number of times, if any, that string
		/// pstrToCount occurs in string pstrSource, or
		/// MagicNumbers.STRING_INDEXOF_NOT_FOUND (-1) if pstrToCount is either
		/// null reference or empty the empty string.
		/// </returns>
		/// <remarks>
		/// This method implements the only overload of the string.IndexOf
		/// method that takes a character as its second argument for which I
		/// have yet to make use. There is currently no implementation of the
		/// overload that stops looking after scanning count characters, nor do
		/// I have immediate plans to implement one, though it wouldn't be hard.
		/// 
		/// This method uses the same algorithm as CountSubstrings, except that
		/// its second argument is a single character, which CANNOT be the NULL
		/// character.
		/// </remarks>
		public static int CountCharacterOccurrences (
			this string pstrSource ,
			char pchrToCount )
		{
			if ( string.IsNullOrEmpty ( pstrSource ) )
			{   // Treat null strings as empty, and treat both as a valid, but degenerate, case.
				return MagicNumbers.ZERO;
			}	// if ( string.IsNullOrEmpty ( pstrSource ) )

			if ( pchrToCount == SpecialCharacters.NULL_CHAR )
			{   // This is an error. String pstrToCount should never be null or empty.
				return MagicNumbers.STRING_INDEXOF_NOT_FOUND;
			}	// if ( string.IsNullOrEmpty ( pstrToCount ) )

			int rintCount = MagicNumbers.ZERO;

			//	----------------------------------------------------------------
			//	Unless pstrSource contains at least one instance of pstrToCount,
			//	this first IndexOf is the only one that executes.
			//
			//	If there are no matches, intPos is STRING_INDEXOF_NOT_FOUND (-1)
			//	and the WHILE loop is skipped. Hence, if control falls into the
			//	loop, at least one item was found, and must be counted, and the
			//	loop continues until intPos becomes STRING_INDEXOF_NOT_FOUND.
			//	----------------------------------------------------------------

			int intPos = pstrSource.IndexOf ( pchrToCount );					// Look for first instance.

			while ( intPos != MagicNumbers.STRING_INDEXOF_NOT_FOUND )
			{																	// Found at least one.
				rintCount++;													// Count it.
				intPos = pstrSource.IndexOf (
					pchrToCount ,
					( intPos + ArrayInfo.NEXT_INDEX ) );						// Search for more.
			}	// while ( intPos != MagicNumbers.STRING_INDEXOF_NOT_FOUND )

			return rintCount;													// Report.
		}   // CountCharacterOccurrences
		#endregion	// CountCharacterOccurrences Method


		#region CountSubstrings Methods
		/// <summary>
        /// Strangely, the String class is missing an important static method to
		/// count substrings within a string. This is that missing method.
        /// </summary>
        /// <param name="pstrSource">
        /// Specify the string in which to count occurrences of substring 
		/// pstrToCount.
        ///
        /// If pstrSource is null or empty, the method returns zero.
		/// 
		/// Since this is an extension method, pstrIn is supplied by the BCL
		/// when it binds this method to an instance of System.string.
		/// </param>
        /// <param name="pstrToCount">
        /// Specify the substring to count in string pstrSource. An empty string
		/// causes the method to return MagicNumbers.STRING_INDEXOF_NOT_FOUND,
		/// or -1.
        /// </param>
        /// <returns>
        /// The return value is the number of times, if any, that string
		/// pstrToCount occurs in string pstrSource, or
		/// MagicNumbers.STRING_INDEXOF_NOT_FOUND (-1) if pstrToCount is either
		/// null reference or empty the empty string.
        /// </returns>
		/// <remarks>
		/// This method implements the only overloads of the string.IndexOf
		/// method that takes a string as its second argument for which I
		/// have yet to make use. There is currently no implementation of the
		/// overloads that stops looking after scanning count characters, nor do
		/// I have immediate plans to implement one, though it wouldn't be hard.
		/// 
		/// This method is implemented by calling the second overload, which has
		/// an additional argument through which to specify a string comparison
		/// algorithm, since the algorithms required to implement both are
		/// otherwise identical.
		/// </remarks>
		public static int CountSubstrings (
            this string pstrSource ,
            string pstrToCount )
        {
			return pstrSource.CountSubstrings (
				pstrToCount ,
				StringComparison.CurrentCulture );
        }   // CountSubstrings (1 of 2)


        /// <summary>
        /// Strangely, the String class is missing an important static method to
		/// count substrings within a string. This is that missing method.
        /// </summary>
        /// <param name="pstrSource">
        /// Specify the string in which to count occurrences of substring
		/// pstrToCount.
        ///
        /// If pstrSource is null or empty, the method returns zero.
		/// 
		/// Since this is an extension method, pstrIn is supplied by the BCL
		/// when it binds this method to an instance of System.string.
		/// </param>
        /// <param name="pstrToCount">
        /// Specify the substring to count in string pstrSource.
		/// 
		/// The empty string causes the method to return 
		/// MagicNumbers.STRING_INDEXOF_NOT_FOUND, or -1.
        /// </param>
        /// <param name="penmComparisonType">
        /// Specify a member of the System.StringComparison enumeration, which
		/// defines the rules for performing the comparison.
        /// </param>
        /// <returns>
        /// The return value is the number of times, if any, that string
		/// pstrToCount occurs in string pstrSource, zero if pstrSource is a
		/// null reference or the empty string, or 
		/// MagicNumbers.STRING_INDEXOF_NOT_FOUND (-1) if pstrToCount is either
		/// a null reference or the empty string.
        /// </returns>
		/// <remarks>
		/// This method implements the only overloads of the string.IndexOf
		/// method that takes a string as its second argument for which I
		/// have yet to make use. There is currently no implementation of the
		/// overloads that stops looking after scanning count characters, nor do
		/// I have immediate plans to implement one, though it wouldn't be hard.
		/// </remarks>
		public static int CountSubstrings (
            this string pstrSource ,
            string pstrToCount ,
            StringComparison penmComparisonType )
        {
			//  ----------------------------------------------------------------
			//  Function to count no. of occurrences of Substring in Main string
			//
			//  Remarks:        This isn't anything I couldn't have easily
			//  				written myself, but why, when it's both trivial
			//  				and freely available?
			//
			//  				The code posted at the below reference called
			//  				this function CharCount, and all the variables
			//  				had different names.
			//
			//  				The example posted on the Web recycles input
			//  				string pstrSource, which is bad form, for two
			//  				reasons.
			//
			//  				1)  Inputs to functions should never be altered
			//  					by the function code.
			//
			//  				2)  The input string could be very long, causing
			//  					the code to consume gobs of extra memory to
			//  					make a new copy of part of the string with
			//  					each iteration. These unnecessary copies put
			//  					an unnecessarily heavy load on the garbage
			//  					collector.
			//
			//  				I rewrote it to use the fourth overload of the
			//  				String.IndexOf method, which takes a reference
			//  				to the original String and an Int32, which is
			//  				the index of a point just past where the last
			//  				substring was found.
			//
			//  				After testing, this function became a
			//  				public static method of a core library.
			//
			//  References:     "String Jargon in CSharp," at
			//  				http://www.csharphelp.com/archives/archive12.html
			//
			//  				"String.IndexOf Method (String, Int32)," at
			//  				http://msdn.microsoft.com/en-us/library/7cct0x33(VS.80).aspx
			//
			//	References:     "String.IndexOf Method (String, Int32, StringComparison)," at
			//					http://msdn.microsoft.com/en-us/library/ms224424(VS.80).aspx
			//
			//					"StringComparison Enumeration," at
			//					http://msdn.microsoft.com/en-us/library/system.stringcomparison(VS.80).aspx
			//	----------------------------------------------------------------

			if ( string.IsNullOrEmpty ( pstrSource ) )
			{   // Treat null strings as empty, and treat both as a valid, but degenerate, case.
				return MagicNumbers.ZERO;
			}	// if ( string.IsNullOrEmpty ( pstrSource ) )

			if ( string.IsNullOrEmpty ( pstrToCount ) )
			{   // This is an error. String pstrToCount should never be null or empty.
				return MagicNumbers.STRING_INDEXOF_NOT_FOUND;
			}	// if ( string.IsNullOrEmpty ( pstrToCount ) )

			int rintCount = MagicNumbers.ZERO;

			//	----------------------------------------------------------------
			//	Unless pstrSource contains at least one instance of pstrToCount,
			//	this first IndexOf is the only one that executes.
			//
			//	If there are no matches, intPos is STRING_INDEXOF_NOT_FOUND (-1)
			//	and the WHILE loop is skipped. Hence, if control falls into the
			//	loop, at least one item was found, and must be counted, and the
			//	loop continues until intPos becomes STRING_INDEXOF_NOT_FOUND.
			//	----------------------------------------------------------------

			int intPos = pstrSource.IndexOf (
				pstrToCount ,
				penmComparisonType );											// Look for first instance.

			while ( intPos != MagicNumbers.STRING_INDEXOF_NOT_FOUND )
            {																	// Found at least one.
				rintCount++;													// Count it.
				intPos = pstrSource.IndexOf (
					pstrToCount ,
					( intPos + ArrayInfo.NEXT_INDEX ) ,
					penmComparisonType );										// Search for more.
			}	// while ( intPos != MagicNumbers.STRING_INDEXOF_NOT_FOUND )

            return rintCount;													// Report.
        }   // CountSubstrings (2 of 2)
		#endregion	// CountSubstrings Methods


		/// <summary>
		/// Scan a string for environment string delimiter characters left
		/// behind by an environment string expansion.
		/// </summary>
		/// <param name="pstrInput">
		/// Specify a string that has had its environment strings expanded.
		/// </param>
		/// <returns>
		/// The return value is the count of remaining environment string
		/// delimiters. Please see the remarks for additional information.
		/// </returns>
		/// <remarks>
		/// There are two reasons that such delimiters might be left behind.
		/// 
		/// 1) The input string contains environment strings that have no like
		/// named strings in the environment block that belongs to the process.
		/// 
		/// 2) The input string contains a malformed string that is missing one
		/// of its delimiting tokens.
		/// 
		/// This routine wraps CountSubstrings, supplying the required token,
		/// which is defined as public character constant in SpecialCharacters,
		/// a sibling class. Since you could as well call that routine directly,
		/// this routine is syntactic sugar.
		/// </remarks>
		/// <seealso cref="ReportUnresolvedEnvironmentStrings(string,uint,uint)"/>
		public static int CountUnresolvedEnvironmentStrings ( this string pstrInput )
		{
			return pstrInput.CountCharacterOccurrences ( SpecialCharacters.ENV_STR_DLM );
		}   // CountUnresolvedEnvironmentStrings


		#region EncloseInChar Methods
		/// <summary>
		/// Append a specified character to both ends of a string, unless it is
		/// already present.
		/// </summary>
		/// <param name="pstrIn">
		/// Specify the string to evaluate, which may, or may not, end with the
		/// character specified in pchrEnd.
		/// 
		/// Since this is an extension method, pstrIn is supplied by the BCL
		/// when it binds this method to an instance of System.string.
		/// </param>
		/// <param name="pchrEnd">
		/// Specify the character to append, if absent.
		/// </param>
		/// <returns>
		/// The return value is a new string that is guaranteed to have exactly
		/// one of the character specified in pchrEnd at each end.
		/// </returns>
		public static string EncloseInChar (
			this string pstrIn ,
			char pchrEnd )
		{
			string strEndChar = pchrEnd.ToString ( );

			if ( string.IsNullOrEmpty ( pstrIn ) )
				return strEndChar + strEndChar;

			if ( pstrIn.StartsWith ( strEndChar ) && pstrIn.EndsWith ( strEndChar ) )
				return pstrIn;
			else
				if ( pstrIn.StartsWith ( strEndChar ) )
					return pstrIn + strEndChar;
				else
					if ( pstrIn.EndsWith ( strEndChar ) )
						return strEndChar + pstrIn;
					else
						return strEndChar + pstrIn + strEndChar;
		}   // EncloseInChar
		#endregion	// EncloseInChar Methods


		#region EnsureFirstCharIs Methods
		/// <summary>
		/// Return a string that is guaranteed to start with a specified character.
		/// </summary>
		/// <param name="pstrInput"></param>
		/// Evaluate this string, and return it as is if it already starts with
		/// the specified character. Otherwise, return the specified character,
		/// followed by the input string.
		/// <param name="pchrMustBeFirst">
		/// Ensure that the first character in the string is this one.
		/// </param>
		/// <returns>
		/// The returned string is guaranteed to start with the specified character.
		/// </returns>
		public static string EnsureFirstCharIs ( this string pstrInput , char pchrMustBeFirst )
		{
			if ( pchrMustBeFirst != SpecialCharacters.NULL_CHAR )
			{
				if ( pstrInput.StartsWith ( pchrMustBeFirst.ToString ( ) ) )
				{
					return pstrInput;
				}	// TRUE (degenerate case) block, if ( pstrInput.EndsWith ( pchrMustBeFirst.ToString ( ) ) )
				else
				{
					return string.Concat ( pchrMustBeFirst , pstrInput );
				}	// FALSE (standard case) block, if ( pstrInput.EndsWith ( pchrMustBeFirst.ToString ( ) ) )
			}	// TRUE (anticipated outcome) block, if ( pchrMustBeFirst != SpecialCharacters.NULL_CHAR )
			else
			{
				throw new ArgumentNullException ( "pchrMustBeFirst" );
			}	// FALSE (unanticipated outcome) block, if ( pchrMustBeFirst != SpecialCharacters.NULL_CHAR )
		}	// EnsureFirstCharIs
		#endregion	// EnsureFirstCharIs Methods


		#region EnsureLastCharIs Methods
		/// <summary>
		/// Return a string that is guaranteed to end with a specified character.
		/// </summary>
		/// <param name="pstrInput"></param>
		/// Evaluate this string, and return it as is if it already ends with
		/// the specified character. Otherwise, return a copy with the specified
		/// character appended.
		/// <param name="pchrMustBeLast">
		/// Ensure that the last character in the string is this one.
		/// </param>
		/// <returns>
		/// The returned string is guaranteed to end with the specified character.
		/// </returns>
		public static string EnsureLastCharIs (
			this string pstrInput ,
			char pchrMustBeLast )
		{
			if ( pchrMustBeLast != SpecialCharacters.NULL_CHAR )
			{
				if ( pstrInput.EndsWith ( pchrMustBeLast.ToString ( ) ) )
				{
					return pstrInput;
				}	// TRUE (degenerate case) block, if ( pstrInput.EndsWith ( pchrMustBeFirst.ToString ( ) ) )
				else
				{
					return string.Concat (
						pstrInput ,
						pchrMustBeLast );
				}	// FALSE (standard case) block, if ( pstrInput.EndsWith ( pchrMustBeFirst.ToString ( ) ) )
			}	// TRUE (anticipated outcome) block, if ( pchrMustBeFirst != SpecialCharacters.NULL_CHAR )
			else
			{
				throw new ArgumentNullException ( "pchrMustBeLast" );
			}	// FALSE (unanticipated outcome) block, if ( pchrMustBeFirst != SpecialCharacters.NULL_CHAR )
		}	// EnsureLastCharIs
		#endregion	// EnsureLastCharIs Methods


		#region ExtractBetweenIndexOfs Methods
		/// <summary>
		/// Extract the substring bounded by the characters at either end of it.
		/// </summary>
		/// <param name="pstrWholeString">
		/// Specify the string from which to extract the substring.
		/// 
		/// Since this is an extension method, the CLR supplies this argument
		/// when it binds the method to an instance of the System.string class.
		/// </param>
		/// <param name="pintPosBegin">
		/// Specify the position, as it would be reported by IndexOf, of the 
		/// character that bounds the left end of the desired substring.
		/// </param>
		/// <param name="pintPosEnd">
		/// Specify the position, as it would be reported by IndexOf, of the 
		/// character that bounds the right end of the desired substring.
		/// </param>
		/// <returns>
		/// The returned substring begins with the character immediately to the
		/// right of the left hand bounding character, and ending with the last
		/// character before the right hand bounding character.
		/// </returns>
		public static string ExtractBetweenIndexOfs (
		    this string pstrWholeString ,
            int pintPosBegin ,
            int pintPosEnd )
        {
			const int DISCARD = MagicNumbers.PLUS_ONE;

            if ( string.IsNullOrEmpty ( pstrWholeString ) )
				return SpecialStrings.EMPTY_STRING;

			if ( pintPosBegin > MagicNumbers.STRING_INDEXOF_NOT_FOUND )
                if ( pintPosEnd > pintPosBegin )
                    if ( pintPosEnd < pstrWholeString.Length )
                        return pstrWholeString.Substring (
                            pintPosBegin + DISCARD ,
                            ( pintPosEnd - pintPosBegin ) - DISCARD );

            return SpecialStrings.EMPTY_STRING;
        }   // static string ExtractBetweenIndexOfs (1 of 2)


		/// <summary>
		/// Extract the substring bounded by the characters at either end of it.
		/// </summary>
		/// <param name="pstrWholeString">
		/// Specify the string from which to extract the substring.
		/// 
		/// Since this is an extension method, the CLR supplies this argument
		/// when it binds the method to an instance of the System.string class.
		/// </param>
		/// <param name="pstrLeftMarker">
		/// This overload handles the case where the left boundary is bounded by
		/// a string. The method needs a copy of the string in order to find the
		/// true beginning of the substring to extract, and to compute its
		/// length.
		/// </param>
		/// <param name="pintPosBegin">
		/// This integer is the position, given by IndexOf, of the character
		/// that bounds the left end of the desired substring.
		/// </param>
		/// <param name="pintPosEnd">
		/// This integer is the position, given by IndexOf, of the character
		/// that bounds the right end of the desired substring.
		/// </param>
		/// <returns>
		/// The returned substring begins with the character immediately to the
		/// right of the left hand bounding character, and ending with the last
		/// character before the right hand bounding character.
		///
		/// Inputs and computed values are thoroughly sanity checked to prevent
		/// run-time exceptions. If anything is out of order, an empty string is
		/// returned.
		/// </returns>
		public static string ExtractBetweenIndexOfs (
			this string pstrWholeString ,
            string pstrLeftMarker ,
            int pintPosBegin ,
            int pintPosEnd )
        {
            if ( string.IsNullOrEmpty ( pstrWholeString ) )
                return SpecialStrings.EMPTY_STRING;

            if ( string.IsNullOrEmpty ( pstrLeftMarker ) )
                return SpecialStrings.EMPTY_STRING;

            int intToDiscard = pstrLeftMarker.Length;

            if ( pintPosBegin > MagicNumbers.STRING_INDEXOF_NOT_FOUND )
                if ( pintPosEnd > pintPosBegin )
                    if ( pintPosEnd < pstrWholeString.Length - intToDiscard )
                        return pstrWholeString.Substring (
                            pintPosBegin + intToDiscard ,
                            ( pintPosEnd - pintPosBegin ) - intToDiscard );

            return SpecialStrings.EMPTY_STRING;
        }   // static string ExtractBetweenIndexOfs (2 of 2)
		#endregion	// ExtractBetweenIndexOfs Methods


		#region ExtractBoundedSubstrings Methods
		/// <summary>
        /// Extract a substring that is bounded by a character. See Remarks.
        /// </summary>
        /// <param name="pstrWholeString">
        /// The substring is extracted from this string.
        /// </param>
        /// <param name="pchrBoundingCharacter">
        /// Specify the bounding character. Please see Remarks.
        /// </param>
        /// <returns>
        /// The return value is the desired substring, without its bounding 
		/// characters. See Remarks.
        /// </returns>
        /// <remarks>
        /// The left and right ends must be bounded by the same character. To
        /// extract a string bounded on each end by a different character, use
        /// the next overload.
        ///
        /// Inputs and computed values are thoroughly sanity checked to prevent
        /// run-time exceptions. If anything is out of order, an empty string is
        /// returned.
        /// </remarks>
        public static string ExtractBoundedSubstrings (
			this string pstrWholeString ,
            char pchrBoundingCharacter )
        {
            if ( string.IsNullOrEmpty ( pstrWholeString ) )
                return SpecialStrings.EMPTY_STRING;

            if ( pchrBoundingCharacter == MagicNumbers.ZERO )
                return SpecialStrings.EMPTY_STRING;

            int intPosLeftEnd = pstrWholeString.IndexOf ( pchrBoundingCharacter );

            if ( intPosLeftEnd == MagicNumbers.STRING_INDEXOF_NOT_FOUND )
                return SpecialStrings.EMPTY_STRING;

            int intPosRightEnd = pstrWholeString.IndexOf (
                pchrBoundingCharacter ,
                intPosLeftEnd + MagicNumbers.PLUS_ONE );

            if ( intPosRightEnd == MagicNumbers.STRING_INDEXOF_NOT_FOUND )
                return SpecialStrings.EMPTY_STRING;

			return pstrWholeString.ExtractBetweenIndexOfs (
                intPosLeftEnd ,
                intPosRightEnd );
        }   // ExtractBoundedSubstrings (1 of 3)


        /// <summary>
        /// Extract a substring that is bounded by a character. See Remarks.
        /// </summary>
        /// <param name="pstrWholeString">
        /// The substring is extracted from this string.
        /// </param>
        /// <param name="pchrLeftBound">
        /// Specify the character that marks the left end of the string. See
        /// Remarks.
        /// </param>
        /// <param name="pchrRightBound">
        /// Specify the character that marks the right end of the string. See
        /// Remarks.
        /// </param>
        /// <returns>
		/// The returned substring begins with the character immediately to the
		/// right of the left hand bounding substring, and ending with the last
		/// character before the right hand bounding substring.
		/// </returns>
        /// <remarks>
        /// The left and right ends are expected to be bounded by different
        /// characters. To  extract a string bounded on each end by the same
        /// character, use the previous overload.
        ///
        /// Inputs and computed values are thoroughly sanity checked to prevent
        /// run-time exceptions. If anything is out of order, the empty string
		/// is returned.
        /// </remarks>
        public static string ExtractBoundedSubstrings (
			this string pstrWholeString ,
            char pchrLeftBound ,
            char pchrRightBound )
        {
            if ( string.IsNullOrEmpty ( pstrWholeString ) )
                return SpecialStrings.EMPTY_STRING;

            if ( pchrLeftBound == MagicNumbers.ZERO )
                return SpecialStrings.EMPTY_STRING;

            if ( pchrRightBound == MagicNumbers.ZERO )
                return SpecialStrings.EMPTY_STRING;

            int intPosLeftEnd = pstrWholeString.IndexOf ( pchrLeftBound );

            if ( intPosLeftEnd == MagicNumbers.STRING_INDEXOF_NOT_FOUND )
                return SpecialStrings.EMPTY_STRING;

            int intPosRightEnd = pstrWholeString.IndexOf (
                pchrRightBound ,
                intPosLeftEnd + MagicNumbers.PLUS_ONE );

            if ( intPosRightEnd == MagicNumbers.STRING_INDEXOF_NOT_FOUND )
                return SpecialStrings.EMPTY_STRING;

			return pstrWholeString.ExtractBetweenIndexOfs (
                intPosLeftEnd ,
                intPosRightEnd );
        }   // ExtractBoundedSubstrings (2 of 3)


        /// <summary>
        /// Extract a substring that is bounded by a pair of substrings. See
        /// Remarks.
        /// </summary>
        /// <param name="pstrWholeString">
        /// Specify the string from which to extract the bounded substring.
        /// </param>
        /// <param name="pstrLeftBound">
        /// Specify the substring that marks the left end of the string. See
        /// Remarks.
        /// </param>
        /// <param name="pstrRightBound">
		/// Specify the substring that marks the right end of the string. See
        /// Remarks.
        /// </param>
        /// <returns>
		/// The returned substring begins with the character immediately to the
		/// right of the left hand bounding substring, and ending with the last
		/// character before the right hand bounding substring.
		/// </returns>
        /// <remarks>
        /// The left and right ends are expected to be bounded by different
        /// substrings. To  extract a string bounded on each end by the same
        /// substring, use the same value for the second and third arguments.
        ///
        /// Inputs and computed values are thoroughly sanity checked to prevent
        /// run-time exceptions. If anything is out of order, an empty string is
        /// returned.
        /// </remarks>
        public static string ExtractBoundedSubstrings (
			this string pstrWholeString ,
            string pstrLeftBound ,
            string pstrRightBound )
        {
            if ( string.IsNullOrEmpty ( pstrWholeString ) )
                return SpecialStrings.EMPTY_STRING;

            if ( string.IsNullOrEmpty ( pstrLeftBound ) )
                return SpecialStrings.EMPTY_STRING;

            if ( string.IsNullOrEmpty ( pstrRightBound ) )
                return SpecialStrings.EMPTY_STRING;

            int intPosLeftEnd = pstrWholeString.IndexOf ( pstrLeftBound );

            if ( intPosLeftEnd == MagicNumbers.STRING_INDEXOF_NOT_FOUND )
                return SpecialStrings.EMPTY_STRING;

            int intPosRightEnd = pstrWholeString.IndexOf (
                pstrRightBound ,
                intPosLeftEnd + WizardWrx.MagicNumbers.PLUS_ONE );

            if ( intPosRightEnd == MagicNumbers.STRING_INDEXOF_NOT_FOUND )
                return SpecialStrings.EMPTY_STRING;

			//	----------------------------------------------------------------
			//	Eat your own dog food!
			//	----------------------------------------------------------------

			return pstrWholeString.ExtractBetweenIndexOfs (
                pstrLeftBound ,
                intPosLeftEnd ,
                intPosRightEnd );
        }   // ExtractBoundedSubstrings (3 of 3)
		#endregion	// ExtractBoundedSubstrings Methods


		#region LeftPadNChars Extension Methods
		/// <summary>
		/// Left pad the string with a specified number of spaces.
		/// </summary>
		/// <param name="pstrPadThisString">
		/// This argument is supplied by the framework when it binds the method
		/// to an instance of the System.String class.
		/// </param>
		/// <param name="paddingCharacterCount">
		/// Specify the number of space characters to add on the left end of the
		/// string. Please see the Remarks for important details.
		/// </param>
		/// <returns>
		/// The input string is padded on the left with the specified number of
		/// space characters.
		/// 
		/// Please see the Remarks for important details.
		/// </returns>
		/// <remarks>
		/// These methods compensate for the completely logical, if unexpected,
		/// behavior of the native PadLeft and PadRight methods on the
		/// System.string class. Their objective is to guarantee that the new
		/// string is truly padded with a specific number of characters.
		/// 
		/// The names of the visible arguments differ from my usual Hungarian
		/// naming convention so that they conform to the naming convention of
		/// the Base Class Library methods that they wrap.
		/// </remarks>
		public static string LeftPadNChars (
			this string pstrPadThisString ,
			int paddingCharacterCount )
		{
			return pstrPadThisString.PadLeft (
				PaddedStringLength (
					pstrPadThisString ,
					paddingCharacterCount ) );
		}	// public static string LeftPadNChars (1 of 2)


		/// <summary>
		/// Left pad the string with a specified number of some arbitrary
		/// character.
		/// </summary>
		/// <param name="pstrPadThisString">
		/// This argument is supplied by the framework when it binds the method
		/// to an instance of the System.String class.
		/// </param>
		/// <param name="paddingCharacterCount">
		/// Specify the number of arbitrary characters to add on the left end of
		/// the string. Please see the Remarks for important details.
		/// </param>
		/// <param name="paddingChar">
		/// Specify the arbitrary character with which the string is to be padded.
		/// </param>
		/// <returns>
		/// The input string is padded on the left with the specified number of
		/// the specified arbitrary character.
		/// 
		/// Please see the Remarks for important details.
		/// </returns>
		/// <remarks>
		/// These methods compensate for the completely logical, if unexpected,
		/// behavior of the native PadLeft and PadRight methods on the
		/// System.string class. Their objective is to guarantee that the new
		/// string is truly padded with a specific number of characters.
		/// 
		/// The names of the visible arguments differ from my usual Hungarian
		/// naming convention so that they conform to the naming convention of
		/// the Base Class Library methods that they wrap.
		/// </remarks>
		public static string LeftPadNChars (
			this string pstrPadThisString ,
			int paddingCharacterCount ,
			char paddingChar )
		{
			return pstrPadThisString.PadLeft (
				PaddedStringLength (
					pstrPadThisString ,
					paddingCharacterCount ) ,
				paddingChar );
		}	// public static string LeftPadNChars (2 of 2)
		#endregion // LeftPadNChars


		#region MakeToken Methods
		/// <summary>
        /// Given a string containing the name of a form control (field) or
        /// other token, create its place holder token.
        /// </summary>
        /// <param name="pstrFieldName">
        /// String containing the name of the token.
        /// </param>
        /// <returns>
        /// String containing the text of the corresponding template text place
        /// holder. See Remarks.
        /// </returns>
        /// <remarks>
        /// The string is constructed by appending a standard token delimiter,
        /// which is a pair of dollar signs, to each end of the string.
        ///
        /// The token is exposed as a static property, DEFAULT_TOKEN_DELM.
        /// </remarks>
		public static string MakeToken ( this string pstrFieldName )
		{
			if ( string.IsNullOrEmpty ( pstrFieldName ) )
				return SpecialStrings.EMPTY_STRING;

			return string.Concat (
				DEFAULT_TOKEN_DELM ,
				pstrFieldName ,
				DEFAULT_TOKEN_DELM );
		}   // public static MakeToken method (1 of 2)


        /// <summary>
        /// Given a string containing the name of a form control (field) or
        /// other token, and another string containing a static token, create
        /// its place holder token.
        /// </summary>
        /// <param name="pstrFieldName">
        /// Specify the string containing the name of the token.
        /// </param>
        /// <param name="pstrTokenEnds">
		/// Specify the string to attach to both ends of the string specified by
		/// pstrFieldName to generate the token.
		/// </param>
        /// <returns>
        /// The string is constructed by appending the token delimiter specified
        /// in argument pstrTokenEnds to both ends of the string specified in
        /// argument pstrFieldName.
        /// </returns>
		public static string MakeToken (
			this string pstrFieldName ,
			string pstrTokenEnds )
		{
			if ( string.IsNullOrEmpty ( pstrFieldName ) )
				return SpecialStrings.EMPTY_STRING;

			if ( string.IsNullOrEmpty ( pstrTokenEnds ) )
				return SpecialStrings.EMPTY_STRING;

			return string.Concat (
				pstrTokenEnds ,
				pstrFieldName ,
				pstrTokenEnds );
		}   // public static MakeToken method (2 of 2)
		#endregion	// MakeToken Methods


		#region ParseCommentInHTMLComment Method
		/// <summary>
		/// Extract parameters, expressed as key-value pairs, from a standard
		/// HTML comment.
		/// </summary>
		/// <param name="pstrInput">
		/// String containing a well formed HTML comment, surrounding the
		/// key-value pairs. If the string is not a well formed HTML comment,
		/// with a single space between the comment delimiters and the body,
		/// or the string is null or empty, the returned collection is empty.
		/// </param>
		/// <returns>
		/// A NameValueCollection of parameter names and values, which may be
		/// empty, but is guaranteed to be returned, empty or not.
		/// </returns>
		/// <example>
		/// Parse this: <!-- ForPage=default;UseTable=False -->
		/// 
		/// Return this:
		/// 
		///			=======================
		///			Name		Value
		///			-----------	-----------
		///			ForPage		default
		///			UseTable	False
		///			=======================
		///			
		/// The returned NameValueCollection contains two members.
		/// 
		/// Since this method guarantees to return an initialized
		/// NameValueCollection, the empty collection is allocated by the first
		/// statement, and is unconditionally returned by the last statement.
		/// </example>
		public static NameValueCollection ParseCommentInHTMLComment ( this string pstrInput )
		{
			// Parse this: <!-- ForPage=default;UseTable=False -->

			const string LEFT_DELIMITER = @"<!-- ";
			const string RIGHT_DELIMITER = @" -->";

			const char ARG_DELIM = ';';
			const char VALUE_DELIM = '=';

			const int VALUE_FROM_NAME = 2;
			const int NAME_INDEX = 0;
			const int VALUE_INDEX = 1;

			NameValueCollection rnvcArgs = new NameValueCollection ( );

			if ( !string.IsNullOrEmpty ( pstrInput ) )
			{
				if ( pstrInput.StartsWith ( LEFT_DELIMITER ) && pstrInput.EndsWith ( RIGHT_DELIMITER ) )
				{
					int intTotalLen = pstrInput.Length;
					int intLeftLen = LEFT_DELIMITER.Length;
					int intRightLen = RIGHT_DELIMITER.Length;
					int intMiddleLen = intTotalLen - ( intLeftLen + intRightLen );

					string strMeat = pstrInput.Substring ( intLeftLen , intMiddleLen );
					string [ ] astrParams = strMeat.Split ( ARG_DELIM );

					foreach ( string strParam in astrParams )
					{
						string [ ] astrPVPair = strParam.Split (
							VALUE_DELIM.ArrayOfOne ( ) ,
							VALUE_FROM_NAME );
						rnvcArgs.Add (
							astrPVPair [ NAME_INDEX ] ,
							astrPVPair [ VALUE_INDEX ] );
					}   // foreach ( string strParam in astrParams )
				}   // if ( pstrInput.StartsWith ( LEFT_DELIMITER ) && pstrInput.EndsWith ( RIGHT_DELIMITER ) )
			}   // if ( !string.IsNullOrEmpty ( pstrInput ) )

			return rnvcArgs;
		}   // ParseCommentInHTMLComment
		#endregion	// ParseCommentInHTMLComment Methods


		#region QuoteString Method
		/// <summary>
        /// Append a quote character to both ends of a string, unless it is
        /// already present.
        /// </summary>
        /// <param name="pstrIn">
        /// String to evaluate, which may, or may not, end with the characterr
        /// specified in pchrEnd.
        /// </param>
        /// <returns>
        /// String with quote character at both ends.
        /// </returns>
		public static string QuoteString ( this string pstrIn )
		{
			return EncloseInChar (
				pstrIn ,
				SpecialCharacters.DOUBLE_QUOTE );
		}   // function QuoteString
		#endregion	// QuoteString Method


		#region RemoveEndChars Method
		/// <summary>
        /// Remove ending character, such as brackets, from a string, if present.
        /// </summary>
        /// <param name="pstrIn">
        /// String to evaluate, which may, or may not, end with the characterr
        /// specified in pchrEnd.
        /// </param>
        /// <param name="pchrEnd">
        /// Character to remove, if present.
        /// </param>
        /// <returns>
        /// String with character pchrEnd removed from both ends.
        /// </returns>
		public static string RemoveEndChars (
			this string pstrIn ,
			char pchrEnd )
		{
			if ( pstrIn == null )
				return null;

			string strEndChar = pchrEnd.ToString ( );

			switch ( pstrIn.Length )
			{
				case MagicNumbers.ZERO:
					return SpecialStrings.EMPTY_STRING;
				case MagicNumbers.PLUS_ONE:
					if ( pstrIn == strEndChar )
						return SpecialStrings.EMPTY_STRING;
					else
						return pstrIn;
			}   // switch (pstrIn.Length)

			int intCharsToKeep = MagicNumbers.ZERO;
			int intKeepFromPos = MagicNumbers.ZERO;

			if ( pstrIn.StartsWith ( strEndChar ) && pstrIn.EndsWith ( strEndChar ) )
			{   // Trim both ends.
				intKeepFromPos = MagicNumbers.PLUS_ONE;
				intCharsToKeep = pstrIn.Length - MagicNumbers.PLUS_TWO;
				return pstrIn.Substring ( intKeepFromPos , intCharsToKeep );
			}	// True block, if ( pstrIn.StartsWith ( strEndChar ) && pstrIn.EndsWith ( strEndChar ) )
			else
			{   // At most, one end needs trimming.
				if ( pstrIn.StartsWith ( strEndChar ) )
				{   // Trim the left end.
					intKeepFromPos = MagicNumbers.PLUS_ONE;
					intCharsToKeep = pstrIn.Length - MagicNumbers.PLUS_ONE;
					return pstrIn.Substring ( intKeepFromPos , intCharsToKeep );
				}	// True block, if ( pstrIn.StartsWith ( strEndChar ) )
				else
				{   // Check the right end.
					intKeepFromPos = ArrayInfo.ARRAY_FIRST_ELEMENT;

					if ( pstrIn.EndsWith ( strEndChar ) )
					{
						intCharsToKeep = pstrIn.Length - MagicNumbers.PLUS_ONE;
						return pstrIn.Substring ( intKeepFromPos , intCharsToKeep );
					}	// True block, if ( pstrIn.EndsWith ( strEndChar ) )
					else
					{   // The string is already fit for use.
						return pstrIn;
					}	// False block, if ( pstrIn.EndsWith ( strEndChar ) )
				}   // False block, if ( pstrIn.StartsWith ( strEndChar ) )
			}	// False block, if ( pstrIn.StartsWith ( strEndChar ) && pstrIn.EndsWith ( strEndChar ) )
		}   // RemoveEndChars
		#endregion	// RemoveEndChars Methods


		#region RemoveEndQuotes Method
		/// <summary>
		/// Remove ending quotation marks from a string, if present.
		/// </summary>
		/// <param name="pstrIn">
		/// Specify the string to evaluate, which may, or may not, end with
		/// quotes.
		/// </param>
		/// <returns>
		/// The return value is a new string with ending quotes, if present,
		/// removed. Otherwise, a copy of the original string is returned.
		/// </returns>
		public static string RemoveEndQuotes ( this string pstrIn )
		{
			return RemoveEndChars (
				pstrIn ,
				SpecialCharacters.DOUBLE_QUOTE );
		}   // method RemoveEndQuotes
		#endregion	// RemoveEndQuotes Method


		#region ReplaceTokensFromList Methods
		/// <summary>
        /// Given a string containing tokens of the form "^^ListKeyValue^^"
        /// where ListKeyValue is the value of a key in the pnvcList collection,
        /// which may or may not exist in the collection, replace all such
        /// tokens with the contents of the like named value in the collection.
        /// </summary>
        /// <param name="pstrMsg">
        /// String containing the message containing the substitution tokens.
        /// </param>
        /// <param name="pnvcList">
        /// A NameValueCollection, in which each key represents a token, and its
        /// value represents the value to be substituted for it.
        /// </param>
        /// <returns>
        /// String with tokens replaced, and tokens that have no corresponding
        /// object in the pnvcList collection preserved.
        /// </returns>
		public static string ReplaceTokensFromList (
			this string pstrMsg ,
			NameValueCollection pnvcList )
		{
			if ( string.IsNullOrEmpty ( pstrMsg ) )
				return SpecialStrings.EMPTY_STRING;

			if ( pnvcList == null )
				return pstrMsg;

			if ( pnvcList.Count == WizardWrx.MagicNumbers.ZERO )
				return pstrMsg;

			string strNewMsg = pstrMsg;

			foreach ( string strVarName in pnvcList.Keys )
				strNewMsg = strNewMsg.Replace (
					strVarName.MakeToken ( DEFAULT_TOKEN_DELM ) ,
					pnvcList [ strVarName ] );

			return strNewMsg;
		}   // static method ReplaceTokensFromList (1 of 6)


        /// <summary>
        /// Replace place holders (markers) with the value of the corresponding
        /// form control (field), or a default value, if the field is empty and
        /// a default is designated.
        /// </summary>
        /// <param name="pstrTemplate">
        /// String containing a template containing the text and place markers.
        /// </param>
        /// <param name="pnvcFields">
        /// NameValueCollection containing the form control (field) values.
        /// </param>
        /// <param name="pnvcDefaults">
        /// NameValueCollection containing the form control (field) or token
        /// default values.
        ///
        /// Default values are optional. If omitted, the method substitutes an
        /// empty string.
        /// </param>
        /// <returns>
        /// String containing the text in the template, with all tokens
        /// formatted with default endings replaced by the contents of the
        /// corresponding, and like named, control (field) on the input form.
        ///
        /// Tokens bounded by "##" are replaced by strings from the Session
        /// variables collection. The same defaults collection is used for both.
        /// </returns>
		public static string ReplaceTokensFromList (
			this string pstrTemplate ,
			NameValueCollection pnvcFields ,
			NameValueCollection pnvcDefaults )
		{
			if ( string.IsNullOrEmpty ( pstrTemplate ) )
				return SpecialStrings.EMPTY_STRING;

			if ( pnvcFields == null )
				return SpecialStrings.EMPTY_STRING;

			if ( pnvcFields.Count == MagicNumbers.ZERO )
				return pstrTemplate;        // Since there are no tokens, return the template as is.

			string rstrMsg = pstrTemplate;  // Make a copy of the template.

			try
			{
				//	------------------------------------------------------------
				//	Replace tokens with default ($$) endings with data from the
				//	form fields collection.
				//	-------------------------------------------------------------

				foreach ( string strName in pnvcFields )
				{	// Since they are used more than once, both of these strings earn their keep.
					string strToken = strName.MakeToken ( );
					string strValue = pnvcFields [ strName ];

					if ( strValue.Length == MagicNumbers.ZERO )
						if ( pnvcDefaults [ strName ] != null )
							if ( pnvcDefaults [ strName ].Length > MagicNumbers.ZERO )
								strValue = pnvcDefaults [ strName ];

					if ( strToken.Length > MagicNumbers.ZERO )
					{
						rstrMsg = rstrMsg.Replace (
							strToken ,
							strValue );
					}   // if (strToken.Length > MagicNumbers.ZERO)
				}   // foreach (string strName in pnvcFields)
			}
			catch
			{
				throw;
			}

			return rstrMsg;
		}  // private static function ReplaceTokensFromList (2 of 6)


        /// <summary>
        /// Given a string containing tokens of the form "^^ListKeyValue^^"
        /// where ListKeyValue is the value of a key in the pnvcList collection,
        /// which may or may not exist in the collection, replace all such
        /// tokens with the contents of the like named value in the collection.
        /// </summary>
        /// <param name="pstrMsg">
        /// String containing the message containing the substitution tokens.
        /// </param>
        /// <param name="pdctList">
        /// A Dictionary, in which each key represents a token, and its value
        /// represents the value to be substituted for it.
        ///
        /// The Dictionary may contain anything, as it accepts any Object. The
        /// required substitution string is obtained by calling the default
        /// ToString method on each Object. To supply a format string, which
        /// will be applied to each Object, in turn, use the next overload.
        /// </param>
        /// <returns>
        /// String with tokens replaced, and tokens that have no corresponding
        /// object in the pnvcList collection preserved.
        /// </returns>
		public static string ReplaceTokensFromList (
			this string pstrMsg ,
			Dictionary<string , object> pdctList )
		{
			if ( string.IsNullOrEmpty ( pstrMsg ) )
				return SpecialStrings.EMPTY_STRING;

			if ( pdctList == null )
				return pstrMsg;

			if ( pdctList.Count == WizardWrx.MagicNumbers.ZERO )
				return pstrMsg;

			string strNewMsg = pstrMsg;

			foreach ( string strVarName in pdctList.Keys )
				strNewMsg = strNewMsg.Replace (
					strVarName.MakeToken ( DEFAULT_TOKEN_DELM ) ,
					pdctList [ strVarName ].ToString ( ) );

			return strNewMsg;
		}   // static method ReplaceTokensFromList (3 of 6)


        /// <summary>
        /// Given a string containing tokens of the form "^^ListKeyValue^^"
        /// where ListKeyValue is the value of a key in the pnvcList collection,
        /// which may or may not exist in the collection, replace all such
        /// tokens with the contents of the like named value in the collection.
        /// </summary>
        /// <param name="pstrMsg">
        /// String containing the message containing the substitution tokens.
        /// </param>
        /// <param name="pdctList">
        /// A Dictionary, in which each key represents a token, and its value
        /// represents the value to be substituted for it.
        ///
        /// The Dictionary may contain anything, as it accepts any Object. The
        /// required substitution string is obtained by calling the default
        /// ToString method on each Object. To supply a format string, which
        /// will be applied to each Object, in turn, use the next overload.
        /// </param>
        /// <param name="pdctDefaults">
        /// A Dictionary, in which each key represents a token, and its value
        /// represents the default value to be substituted for it, if there is
        /// no corresponding key in dictionary pdctList.
        ///
        /// The Dictionary may contain anything, as it accepts any Object. The
        /// required substitution string is obtained by calling the default
        /// ToString method on each Object. To supply a format string, which
        /// will be applied to each Object, in turn, use the next overload.
        /// </param>
        /// <returns>
        /// String with tokens replaced, and tokens that have no corresponding
        /// object in the pnvcList OR the pdctDefaults collection preserved.
        /// </returns>
		public static string ReplaceTokensFromList (
			this string pstrMsg ,
			Dictionary<string , object> pdctList ,
			Dictionary<string , object> pdctDefaults )
		{
			if ( string.IsNullOrEmpty ( pstrMsg ) )
				return SpecialStrings.EMPTY_STRING;

			if ( pdctList == null )
				return pstrMsg;

			if ( pdctDefaults == null )
				return pstrMsg;

			if ( pdctList.Count == WizardWrx.MagicNumbers.ZERO )
				return pstrMsg;

			string strNewMsg = pstrMsg;

			foreach ( string strVarName in pdctList.Keys )
				strNewMsg = strNewMsg.Replace (
					strVarName.MakeToken ( DEFAULT_TOKEN_DELM ) ,
					pdctList [ strVarName ].ToString ( ) );

			//  ----------------------------------------------------------------
			//  Repeat for defaults, which should cover all remaining tokens.
			//  ----------------------------------------------------------------

			foreach ( string strVarName in pdctDefaults.Keys )
				strNewMsg = strNewMsg.Replace (
					strVarName.MakeToken ( DEFAULT_TOKEN_DELM ) ,
					pdctDefaults [ strVarName ].ToString ( ) );

			return strNewMsg;
		}   // static method ReplaceTokensFromList (4 of 6)


        /// <summary>
        /// Given a string containing tokens of the form "^^ListKeyValue^^"
        /// where ListKeyValue is the value of a key in the pnvcList collection,
        /// which may or may not exist in the collection, replace all such
        /// tokens with the contents o object.
        /// </summary>
        /// <param name="pstrMsg">
        /// String containing the message containing the substitution tokens.
        /// </param>
        /// <param name="pdctList">
        /// A Dictionary, in which each key represents a token, and its value
        /// represents the value to be substituted for it.
        ///
        /// The Dictionary may contain anything, as it accepts any Object. The
        /// required substitution string is obtained by calling the default
        /// ToString method on each Object. To supply a format string, which
        /// will be applied to each Object, in turn, use the next overload.
        /// </param>
        /// <param name="pstrFormat">
        /// Format string, acceptable to the static String.Format method, which
        /// is used to format the string representation of each object.
        ///
        /// The string must contain a "[0}" placeholder, which may occur one or
        /// more times in the format string, which is replaced by the string
        /// returned by the ToString method of each object.
        /// </param>
        /// <returns>
        /// String with tokens replaced, and tokens that have no corresponding
        /// object in the pnvcList collection preserved.
        /// </returns>
		public static string ReplaceTokensFromList (
			this string pstrMsg ,
			Dictionary<string , object> pdctList ,
			string pstrFormat )
		{
			if ( string.IsNullOrEmpty ( pstrMsg ) )
				return SpecialStrings.EMPTY_STRING;

			if ( string.IsNullOrEmpty ( pstrFormat ) )
				return SpecialStrings.EMPTY_STRING;

			if ( pdctList == null )
				return pstrMsg;

			if ( pdctList.Count == WizardWrx.MagicNumbers.ZERO )
				return pstrMsg;

			string strNewMsg = pstrMsg;

			foreach ( string strVarName in pdctList.Keys )
				strNewMsg = strNewMsg.Replace (
					strVarName.MakeToken ( DEFAULT_TOKEN_DELM ) ,
					string.Format (
						pstrFormat ,
						pdctList [ strVarName ].ToString ( ) ) );

			return strNewMsg;
		}   // static method ReplaceTokensFromList (5 of 6)


        /// <summary>
        /// Given a string containing tokens of the form "^^ListKeyValue^^"
        /// where ListKeyValue is the value of a key in the pnvcList collection,
        /// which may or may not exist in the collection, replace all such
        /// tokens with the contents of the like named session object.
        /// </summary>
        /// <param name="pstrMsg">
        /// String containing the message containing the substitution tokens.
        /// </param>
        /// <param name="pdctList">
        /// A Dictionary, in which each key represents a token, and its value
        /// represents the value to be substituted for it.
        ///
        /// The Dictionary may contain anything, as it accepts any Object. The
        /// required substitution string is obtained by calling the default
        /// ToString method on each Object. To supply a format string, which
        /// will be applied to each Object, in turn, use the next overload.
        /// </param>
        /// <param name="pdctDefaults">
        /// A Dictionary, in which each key represents a token, and its value
        /// represents the default value to be substituted for it, if there is
        /// no corresponding key in dictionary pdctList.
        ///
        /// The Dictionary may contain anything, as it accepts any Object. The
        /// required substitution string is obtained by calling the default
        /// ToString method on each Object. To supply a format string, which
        /// will be applied to each Object, in turn, use the next overload.
        /// </param>
        /// <param name="pstrFormat">
        /// Format string, acceptable to the static String.Format method, which
        /// is used to format the string representation of each object.
        ///
        /// The string must contain a "[0}" placeholder, which may occur one or
        /// more times in the format string, which is replaced by the string
        /// returned by the ToString method of each object.
        /// </param>
        /// <returns>
        /// String with tokens replaced, and tokens that have no corresponding
        /// object in the pnvcList OR the pdctDefaults collection preserved.
        /// </returns>
		public static string ReplaceTokensFromList (
			this string pstrMsg ,
			Dictionary<string , object> pdctList ,
			Dictionary<string , object> pdctDefaults ,
			string pstrFormat )
		{
			if ( string.IsNullOrEmpty ( pstrMsg ) )
				return SpecialStrings.EMPTY_STRING;

			if ( string.IsNullOrEmpty ( pstrFormat ) )
				return SpecialStrings.EMPTY_STRING;

			if ( pdctList == null )
				return pstrMsg;

			if ( pdctDefaults == null )
				return pstrMsg;

			if ( pdctList.Count == WizardWrx.MagicNumbers.ZERO )
				return pstrMsg;

			string strNewMsg = pstrMsg;

			foreach ( string strVarName in pdctList.Keys )
				strNewMsg = strNewMsg.Replace (
					strVarName.MakeToken ( DEFAULT_TOKEN_DELM ) ,
					string.Format (
						pstrFormat ,
						pdctList [ strVarName ].ToString ( ) ) );

			//	----------------------------------------------------------------
			//	Repeat for defaults, which should cover all remaining tokens.
			//	----------------------------------------------------------------

			foreach ( string strVarName in pdctDefaults.Keys )
				strNewMsg = strNewMsg.Replace (
					strVarName.MakeToken ( DEFAULT_TOKEN_DELM ) ,
					string.Format (
						pstrFormat ,
						pdctDefaults [ strVarName ].ToString ( ) ) );

			return strNewMsg;
		}   // static method ReplaceTokensFromList (6 of 6)
		#endregion	// ReplaceTokensFromList Methods


		#region ReportUnresolvedEnvironmentStrings Method
		/// <summary>
		/// Display a string that contains unmatched environment strings or
		/// unmatched environment string delimiters, followed by details about
		/// the locations of the errors.
		/// </summary>
		/// <param name="pstrInput">
		/// Specify a string that has had its environment strings expanded.
		/// </param>
		/// <param name="puintNEnvStrDlms">
		/// Specify the count of unmatched delimiters. A companion routine,
		/// UnresolvedEnvironmentStrings, can deliver the count, although the
		/// call cannot be nested. Please see the remarks.
		/// </param>
		/// <param name="puintExitCode">
		/// This routine is intended to report the error and exit the calling
		/// console application, returning the specified value as its exit code.
		/// </param>
		/// <returns>
		/// The exit code is passed through, so that the control need not return
		/// to the caller, but may exit directly or indirectly through 
		/// Environment.Exit.
		/// </returns>
		/// <remarks>
		/// After the count is written onto the standard error stream, control
		/// returns to its caller, which may take subsequent actions based upon
		/// the return value.
		/// 
		/// Most of the 
		/// </remarks>
		/// <see cref="CountUnresolvedEnvironmentStrings"/>
		public static uint ReportUnresolvedEnvironmentStrings (
			this string pstrInput ,
			uint puintNEnvStrDlms ,
			uint puintExitCode )
		{
			Console.Error.WriteLine ( ReportUnresolvedEnvironmentStrings (
				pstrInput ,
				puintNEnvStrDlms ) );
			return puintExitCode;
		}   // ReportUnresolvedEnvironmentStrings (1 of 2)


		/// <summary>
		/// Display a string that contains unmatched environment strings or
		/// unmatched environment string delimiters, followed by details about
		/// the locations of the errors.
		/// </summary>
		/// <param name="pstrInput">
		/// Specify a string that has had its environment strings expanded.
		/// </param>
		/// <param name="puintNEnvStrDlms">
		/// Specify the count of unmatched delimiters. A companion routine,
		/// UnresolvedEnvironmentStrings, can deliver the count, although the
		/// call cannot be nested. Please see the remarks.
		/// </param>
		/// <returns>
		/// The return value is a detailed message that shows each unresolved
		/// string.
		/// </returns>
		public static string ReportUnresolvedEnvironmentStrings ( 
			this string pstrInput ,
			uint puintNEnvStrDlms )
		{
			const uint FIRST_ERROR = 1;
			const int POS_TO_ORD = 1;
			const int POS_START = 0;

			StringBuilder sbMessage = new StringBuilder ( MagicNumbers.CAPACITY_01KB );

			sbMessage.AppendFormat (
				StringTricks.AdjustNumberOfNoun (                                       // Format string (message template)
					puintNEnvStrDlms ,                                                  // uint puintNumber         = number of items
					Core.Properties.Resources.ERRMSG_VARIABLE_LITERAL ,                 // string pstrSingularForm  = singular form of noun
					null ,                                                              // string pstrPluralForm    = plural form of noun, or suffix
					Core.Properties.Resources.ERRMSG_UNRESLOVED_ENVIRONEMT_STRINGS ) ,  // string pstrPhrase        = phrase in which the noun appears
				pstrInput.QuoteString ( ) ,												// Format Item 0            = Input string
				puintNEnvStrDlms ,														// Format Item 1            = Count of unmatched environment strings
				Environment.NewLine );													// Format Item 2            = Embedded implementation-dependent newline.

			int intLastPos = POS_START;

			for ( uint uintOccurrence = FIRST_ERROR ;
					   uintOccurrence == puintNEnvStrDlms ;
					   uintOccurrence++ )
			{
				intLastPos = pstrInput.IndexOf (
					SpecialCharacters.ENV_STR_DLM ,
					intLastPos );

				if ( uintOccurrence < puintNEnvStrDlms )
				{   // All but the last error get a message that says that more are coming.
					sbMessage.AppendFormat (
						Core.Properties.Resources.ERRMSG_START_CHARACTER ,              // Format string (message template)
						intLastPos + POS_TO_ORD ,										// Format Item 0 = position of error
						Core.Properties.Resources.ERRMSG_COMMA_AND_LITERAL ,            // Format Item 1 = more to follow
						Environment.NewLine );											// Format Item 2 = newline
				}   // TRUE (all but last error) block, if ( uintOccurrence < puintNEnvStrDlms )
				else
				{   // This is the last (or only) error.
					sbMessage.AppendFormat (
						Core.Properties.Resources.ERRMSG_START_CHARACTER ,				// Format string (message template)
						intLastPos + POS_TO_ORD ,										// Format Item 0 = position of error
						string.Empty ,													// Format Item 1 = nothing (omit)
						Environment.NewLine );											// Format Item 2 = newline
				}   // FALSE (last error) block, if ( uintOccurrence < puintNEnvStrDlms )
			}   // for ( uint uintOccurrence = FIRST_ERROR ; uintOccurrence == puintNEnvStrDlms ; uintOccurrence++ )

			return sbMessage.ToString ( );
		}	// ReportUnresolvedEnvironmentStrings (2 of 2)
		#endregion	// ReportUnresolvedEnvironmentStrings Method



		#region RightPadNChars Extension Method
		/// <summary>
		/// Right pad the string with a specified number of spaces.
		/// </summary>
		/// <param name="pstrPadThisString">
		/// This argument is supplied by the framework when it binds the method
		/// to an instance of the System.String class.
		/// </param>
		/// <param name="paddingCharacterCount">
		/// Specify the number of space characters to add on the right end of
		/// the string.
		/// 
		/// Please see the Remarks for important details.
		/// </param>
		/// <returns>
		/// The input string is padded on the right with the specified number of
		/// space characters.
		/// 
		/// Please see the Remarks for important details.
		/// </returns>
		/// <remarks>
		/// These methods compensate for the completely logical, if unexpected,
		/// behavior of the native PadLeft and PadRight methods on the
		/// System.string class. Their objective is to guarantee that the new
		/// string is truly padded with a specific number of characters.
		/// 
		/// The names of the visible arguments differ from my usual Hungarian
		/// naming convention so that they conform to the naming convention of
		/// the Base Class Library methods that they wrap.
		/// </remarks>
		public static string RightPadNChars (
			this string pstrPadThisString ,
			int paddingCharacterCount )
		{
			return pstrPadThisString.PadRight (
				PaddedStringLength (
					pstrPadThisString ,
					paddingCharacterCount ) );
		}	// public static string RightPadNChars (1 of 2)

		/// <summary>
		/// Left pad the string with a specified number of some arbitrary
		/// character.
		/// </summary>
		/// <param name="pstrPadThisString">
		/// This argument is supplied by the framework when it binds the method
		/// to an instance of the System.String class.
		/// </param>
		/// <param name="paddingCharacterCount">
		/// Specify the number of arbitrary characters to add on the right end 
		/// of the string. Please see the Remarks for important details.
		/// </param>
		/// <param name="paddingChar">
		/// Specify the arbitrary character with which the string is to be padded.
		/// </param>
		/// <returns>
		/// The input string is padded on the right with the specified number of
		/// the specified arbitrary character.
		/// 
		/// Please see the Remarks for important details.
		/// </returns>
		/// <remarks>
		/// These methods compensate for the completely logical, if unexpected,
		/// behavior of the native PadLeft and PadRight methods on the
		/// System.string class. Their objective is to guarantee that the new
		/// string is truly padded with a specific number of characters.
		/// 
		/// The names of the visible arguments differ from my usual Hungarian
		/// naming convention so that they conform to the naming convention of
		/// the Base Class Library methods that they wrap.
		/// </remarks>
		public static string RightPadNChars (
			this string pstrPadThisString ,
			int paddingCharacterCount ,
			char paddingChar )
		{
			return pstrPadThisString.PadRight (
				PaddedStringLength (
					pstrPadThisString ,
					paddingCharacterCount ) ,
				paddingChar );
		}	// public static string RightPadNChars (2 of 2)
		#endregion	// RightPadNChars Extension Method


		#region Truncate Method
		/// <summary>
        /// Supply the missing Truncate function to members of the String class.
        /// </summary>
        /// <param name="pstrSource">
        /// String to truncate, if it is longer than pintMaxLength.
        ///
        /// A null string is treated as an empty string, and the return value
        /// for either is an empty string.
        /// </param>
        /// <param name="pintMaxLength">
        /// Desired maximum length of the returned string. If pstrSource is
        /// longer than pintMaxLength characters, the first pintMaxLength are
        /// returned. Otherwise, the whole string is returned.
        ///
        /// If pintMaxLength is less than or equal to zero, an empty string is
        /// returned.
        /// </param>
        /// <returns>
        /// If the string length is less than or equal to the specified maximum
        /// length, the whole string is returned.
        ///
        /// Otherwise, the first pintMaxLength characters are returned.
        ///
        /// Regardless, the return value is a new System.String object.
        /// </returns>
        public static string Truncate (
			this string pstrSource ,
            int pintMaxLength )
        {
            if ( string.IsNullOrEmpty ( pstrSource ) )
                return SpecialStrings.EMPTY_STRING;
            else
                if ( pintMaxLength <= MagicNumbers.EMPTY_STRING_LENGTH )
                    return SpecialStrings.EMPTY_STRING;
                else
                    if ( pstrSource.Length > pintMaxLength )
                        return pstrSource.Substring (
                            MagicNumbers.STRING_SUBSTR_BEGINNING ,
                            pintMaxLength );
                    else
                        return pstrSource;
        }   // Truncate method
		#endregion	// Truncate Methods


		#region Private Static Helper Methods
		/// <summary>
		/// All four LeftPadNChars and RightPadNChars extension methods use the
		/// same algorithm to compute the overall length of the new string,
		/// which is fed to the analogous PadLeft or PadRight method, which
		/// expects the overall string length.
		/// </summary>
		/// <param name="pstrPadThisString">
		/// Since these methods are used internally, a reference to the string
		/// must be explicitly passed into it.
		/// </param>
		/// <param name="paddingCharacterCount">
		/// The character count is passed through from the extension method 
		/// argument list.
		/// </param>
		/// <returns>
		/// The returned integer is the overall length that must be passed along
		/// to the native PadLeft or PadRight method to guarantee that the
		/// desired number of padding characters are appended to the new string;
		/// </returns>
		/// <remarks>
		/// This method is syntactic sugar that documents the algorithm by which
		/// the overall length of the new string must be computed, in order to
		/// coerce the underlying Pad methods to deliver the desired effect. Any
		/// decent optimizing compiler should render this inline, in registers.
		/// </remarks>
		private static int PaddedStringLength (
			string pstrPadThisString ,
			int paddingCharacterCount )
		{
			return pstrPadThisString.Length + paddingCharacterCount;
		}	// Private Static Helper Methods
		#endregion	// Private Static Helper Methods
	}   // class StringExtensions
}   // partial namespace WizardWrx