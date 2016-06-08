/*
    ============================================================================

    Namespace:          WizardWrx

    Class Name:         SpecialCharacters

    File Name:          SpecialCharacters.cs

    Synopsis:           Define a handful of frequently used characters that can
                        be difficult to correctly differentiate in a source code
                        listing, either in print or in a text editor window.

	Remarks:            This class is one of a constellation of static classes
						that define a wide variety of symbolic constants that I
						use to make my code easier to understand when I need a
						refresher or am about to change it.

						This class implements a subset of the characters defined
                        in WizardWrx.MagicNumbers. Some of those constants,
                        especially those intended mainly for use with arrays and
                        lists, have moved into sibling classes in this library.

    Author:             David A. Gray

    License:            Copyright (C) 2014-2016, David A. Gray 
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

    Created:            Sunday, 14 September 2014

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version Author Description
    ---------- ------- ------ --------------------------------------------------
    2014/09/14 1.0     DAG    Initial implementation.

	2015/06/06 5.4     DAG    Break completely free from WizardWrx.SharedUtl2.

	2015/06/20 5.5     DAG    Relocate to WizardWrx class library, 
                              promote to the root WizardWrx namespace, add
                              special characters that I originally defined in
                              class WizardWrx.SharedUt2.MagicNumbers, and
                              cross reference related constants defined in other
                              classes.

    2015/08/31 5.6     DAG    Add a couple of overlooked special characters.

    2016/04/06 6.0     DAG    Add a full-stop character that I discovered was
                              overlooked in the last review.

    2016/06/07 6.3     DAG    Adjust the internal documentation to correct a few
                              inconsistencies uncovered while preparing this
							  library for publication on GetHub.
    ============================================================================
*/


namespace WizardWrx
{
    /// <summary>
    /// Use these constants when you want or need your listings to be crystal
    /// clear about certain ambiguous literals.
	/// 
	/// Since static classes are implicitly sealed, this class cannot be inherited.
	/// </summary>
	/// <remarks>
	/// For ease of access, I promoted the classes that expose only constants to
	/// the root of the WizardWrx namespace.
	/// </remarks>
	/// <seealso cref="ArrayInfo"/>
	/// <seealso cref="MagicNumbers"/>
	/// <seealso cref="SpecialStrings"/>
	public static class SpecialCharacters
    {
		/// <summary>
		/// Use this character anywhere in your code that requires a bare
		/// carriage return character.
		/// </summary>
		/// <seealso cref="LINEFEED"/>
		/// <seealso cref="SpecialStrings.STRING_SPLIT_CARRIAGE_RETURN"/>
		/// <seealso cref="SpecialStrings.STRING_SPLIT_LINEFEED"/>
		/// <seealso cref="SpecialStrings.STRING_SPLIT_NEWLINE"/>
		public const char CARRIAGE_RETURN = '\r';

		/// <summary>
		/// Use this character to unambiguously denote a period character, for
		/// example, when specifying a delimiter character or appending a full
		/// stop character to a string.
		/// </summary>
		public const char FULL_STOP = '.';

		/// <summary>
		/// Lower case I, for use in code, where it might be easily mistaken for
		/// a number 1 or a letter L.
		/// </summary>
		public const char CHAR_LC_I = 'i';

		/// <summary>
		/// Upper case I, for use in code, where it might be easily mistaken for
		/// a number 1 or a letter L.
		/// </summary>
		/// <seealso cref="CHAR_LC_L"/>
		/// <seealso cref="CHAR_NUMERAL_1"/>
		public const char CHAR_UC_I = 'I';

		/// <summary>
		/// Lower case L, for use in code, where it might be easily mistaken for
		/// a number 1 or a letter I.
		/// </summary>
		/// <seealso cref="CHAR_UC_I"/>
		/// <seealso cref="CHAR_NUMERAL_1"/>
		public const char CHAR_LC_L = 'l';

		/// <summary>
		/// Upper case L, for use in code, where it might be easily mistaken for
		/// a number 1 or a letter I.
		/// </summary>
		/// <seealso cref="CHAR_LC_L"/>
		/// <seealso cref="CHAR_NUMERAL_1"/>
		public const char CHAR_UC_L = 'L';

		/// <summary>
		/// Lower case O, for use in code where it might be easily mistaken for
		/// a number zero. 
		/// </summary>
		/// <seealso cref="CHAR_LC_O"/>
		/// <seealso cref="CHAR_UC_O"/>
		/// <seealso cref="CHAR_NUMERAL_0"/>
		public const char CHAR_LC_O = 'o';

		/// <summary>
		/// Upper case O, for use in code where it might be easily mistaken for
		/// a number zero.
		/// </summary>
		/// <seealso cref="CHAR_LC_O"/>
		/// <seealso cref="CHAR_UC_O"/>
		/// <seealso cref="CHAR_NUMERAL_0"/>
		public const char CHAR_UC_O = 'O';

		/// <summary>
		/// Lower case Z, for use in code where it might be easily mistaken for
		/// a numeric character 2 or 7.
		/// </summary>
		/// <seealso cref="CHAR_UC_Z"/>
		/// <seealso cref="CHAR_NUMERAL_2"/>
		/// <seealso cref="CHAR_NUMERAL_7"/>
		public const char CHAR_LC_Z = 'z';

		/// <summary>
		/// Upper case Z, for use in code where it might be easily mistaken for
		/// a numeric character 2 or 7.
		/// </summary>
		/// <seealso cref="CHAR_LC_Z"/>
		/// <seealso cref="CHAR_NUMERAL_2"/>
		/// <seealso cref="CHAR_NUMERAL_7"/>
		public const char CHAR_UC_Z = 'Z';

		/// <summary>
		/// Numeric character 0, for use in code where it might be mistaken for
		/// a letter O.
		/// </summary>
		/// <seealso cref="CHAR_LC_O"/>
		/// <seealso cref="CHAR_UC_O"/>
		public const char CHAR_NUMERAL_0 = '0';

		/// <summary>
		/// Numeric character 1, for use in code where it might be mistaken for
		/// a lower case l or an upper case I.
		/// </summary>
		/// <seealso cref="CHAR_UC_I"/>
		/// <seealso cref="CHAR_LC_L"/>
		public const char CHAR_NUMERAL_1 = '1';

		/// <summary>
		/// Numeric character 2, for use in code where it might be mistaken for
		/// a letter Z or a numeral 7.
		/// </summary>
		/// <seealso cref="CHAR_LC_Z"/>
		/// <seealso cref="CHAR_UC_Z"/>
		/// <seealso cref="CHAR_NUMERAL_7"/>
		public const char CHAR_NUMERAL_2 = '2';

		/// <summary>
		/// Numeric character 2, for use in code where it might be mistaken for
		/// a letter Z or a numeral 7.
		/// </summary>
		/// <seealso cref="CHAR_LC_Z"/>
		/// <seealso cref="CHAR_UC_Z"/>
		/// <seealso cref="CHAR_NUMERAL_2"/>
		public const char CHAR_NUMERAL_7 = '7';

		/// <summary>
		/// Use this when your code calls for a ampersand literal, when you want the
		/// listing to be crystal clear about what it is.
		/// </summary>
		/// <seealso cref="SpecialStrings.AMPERSAND"/>
		/// <seealso cref="COMMA"/>
		/// <seealso cref="PERCENT_SIGN"/>
		/// <seealso cref="PIPE_CHAR"/>
		/// <seealso cref="SEMICOLON"/>
		/// <seealso cref="TAB_CHAR"/>
		public const char AMPERSAND = '&';

		/// <summary>
        /// Use this when your code calls for a colon literal, when you want the
        /// listing to be crystal clear about what it is.
        /// </summary>
		/// <seealso cref="AMPERSAND"/>
		/// <seealso cref="COMMA"/>
		/// <seealso cref="PERCENT_SIGN"/>
		/// <seealso cref="PIPE_CHAR"/>
		/// <seealso cref="SEMICOLON"/>
		/// <seealso cref="TAB_CHAR"/>
		public const char COLON = ':';

		/// <summary>
        /// Use this when your code calls for a comma literal, when you want the
        /// listing to be crystal clear about what it is.
        /// </summary>
		/// <seealso cref="AMPERSAND"/>
		/// <seealso cref="COLON"/>
		/// <seealso cref="PERCENT_SIGN"/>
		/// <seealso cref="PIPE_CHAR"/>
		/// <seealso cref="SEMICOLON"/>
		/// <seealso cref="TAB_CHAR"/>
        public const char COMMA = ',';

		/// <summary>
        /// Use this when your code calls for a double quotation mark literal, 
        /// when you want the listing to be crystal clear about what it is.
        /// </summary>
		/// <seealso cref="SINGLE_QUOTE"/>
		/// <seealso cref="SPACE_CHAR"/>
		public const char DOUBLE_QUOTE = '"';

		/// <summary>
		/// Use this constant whey your code calls for a literal equals sign.
		/// </summary>
		/// <seealso cref="HYPHEN"/>
		/// <seealso cref="UNDERSCORE_CHAR"/>
		public const char EQUALS_SIGN = '=';

		/// <summary>
		/// Literal hyphens are also easily confused in code, especially with
		/// minus signs.
		/// </summary>
		/// <seealso cref="EQUALS_SIGN"/>
		/// <seealso cref="UNDERSCORE_CHAR"/>
		public const char HYPHEN = '-';

		/// <summary>
		/// Use this character anywhere in your code that requires a bare
		/// linefeed character.
		/// </summary>
		/// <seealso cref="SpecialStrings.STRING_SPLIT_NEWLINE"/>
		/// <seealso cref="SpecialStrings.STRING_SPLIT_CARRIAGE_RETURN"/>
		/// <seealso cref="SpecialStrings.STRING_SPLIT_LINEFEED"/>
		public const char LINEFEED ='\n';

		/// <summary>
        /// Use this when your code calls for a literal null character, and you
        /// want the listing to be crystal clear. This can be especially useful
		/// to distinguish a null character from a null reference.
        /// </summary>
        public const char NULL_CHAR = '\0';

		/// <summary>
		/// Use this when your code calls for a ampersand literal, when you want the
		/// listing to be crystal clear about what it is.
		/// </summary>
		/// <seealso cref="AMPERSAND"/>
		/// <seealso cref="COLON"/>
		/// <seealso cref="COMMA"/>
		/// <seealso cref="SEMICOLON"/>
		/// <seealso cref="TAB_CHAR"/>
		/// <seealso cref="SpecialStrings.PERCENT_SIGN"/>
		public const char PERCENT_SIGN = '%';

		/// <summary>
		/// How have I got on this long without my faithful field separator?
		/// </summary>
		/// <seealso cref="AMPERSAND"/>
		/// <seealso cref="COLON"/>
		/// <seealso cref="COMMA"/>
		/// <seealso cref="PERCENT_SIGN"/>
		/// <seealso cref="SEMICOLON"/>
		/// <seealso cref="TAB_CHAR"/>
		public const char PIPE_CHAR = '|';

        /// <summary>
        /// Use this when your code calls for a semicolon literal, when you want
        /// the listing to be crystal clear about what it is.
        /// </summary>
		/// <seealso cref="AMPERSAND"/>
		/// <seealso cref="COMMA"/>
		/// <seealso cref="COLON"/>
		/// <seealso cref="PERCENT_SIGN"/>
		/// <seealso cref="PIPE_CHAR"/>
		/// <seealso cref="TAB_CHAR"/>
		public const char SEMICOLON = ';';

        /// <summary>
        /// Use this when your code calls for a single quotation mark literal, 
        /// when you want the listing to be crystal clear about what it is.
        /// </summary>
		/// <seealso cref="DOUBLE_QUOTE"/>
		/// <seealso cref="SPACE_CHAR"/>
        public const char SINGLE_QUOTE = '\x0027';

        /// <summary>
        /// Use this when your code calls for a single space when you want the
        /// listing to be crystal clear about what it is.
        /// </summary>
		/// <seealso cref="DOUBLE_QUOTE"/>
		/// <seealso cref="SINGLE_QUOTE"/>
		/// <seealso cref="SPACE_CHAR"/>
		/// <seealso cref="TAB_CHAR"/>
		/// <seealso cref="EQUALS_SIGN"/>
		/// <seealso cref="UNDERSCORE_CHAR"/>
		public const char SPACE_CHAR = ' ';

        /// <summary>
        /// Use this when your code calls for a tab literal, when you want the
        /// listing to be crystal clear about what it is.
        /// </summary>
		/// <seealso cref="AMPERSAND"/>
		/// <seealso cref="COLON"/>
		/// <seealso cref="COMMA"/>
		/// <seealso cref="PERCENT_SIGN"/>
		/// <seealso cref="PIPE_CHAR"/>
		/// <seealso cref="SEMICOLON"/>
		public const char TAB_CHAR = '\t';

		/// <summary>
		/// Underscores can be really hard to see in code, both on paper and
		/// on screen.
		/// </summary>
		/// <seealso cref="DOUBLE_QUOTE"/>
		/// <seealso cref="SINGLE_QUOTE"/>
		/// <seealso cref="SPACE_CHAR"/>
		/// <seealso cref="EQUALS_SIGN"/>
		/// <seealso cref="HYPHEN"/>
		/// <seealso cref="TAB_CHAR"/>
		public const char UNDERSCORE_CHAR = '_';
	}   // public static class SpecialCharacters
}   // partial namespace WizardWrx