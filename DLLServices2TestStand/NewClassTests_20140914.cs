﻿// #define STRINGS_TO_CONSOLE				// Defined the classic C/C++ way, their effect is confined to one module.

/*
    ============================================================================

    Namespace:          DLLServices2TestStand

    Class Name:         NewClassTests_20140914

    File Name:          NewClassTests_20140914.cs

    Synopsis:           This static class exercises the new classes by listing
                        the constants defined by each, and exercising each of
                        their service methods.

    Remarks:            Each class has a corresponding static method, which
                        returns an integer status code of zero unless there is a
                        reason to shut down the program.

    Author:             David A. Gray

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

    Created:            Sunday, 14 September 2014 and Monday, 15 September 2014

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version Author Description
    ---------- ------- ------ --------------------------------------------------
    2014/09/15 5.2     DAG    Initial implementation.

    2015/06/21 5.5     DAG    Move to WizardWrx.DLLServices2TestStand.exe, to 
                              which the associated classes moved, extend to test
                              the loop index state evaluators and the
                              implementation in C# of the Unless idiom that I
                              borrowed from the Perl scripting language, and put
                              under my 3-clause BSD license.

    2015/08/31 5.6     DAG    Add a couple of overlooked special characters and
                              strings, along with a new method, ChopChop, to
                              test the new Chop method.

    2016/04/05 6.0     DAG    Add overlooked special characters to the listing.

	2016/06/04 6.2     DAG    Add the two new stream I/O flags and the routine
                              to put the new hex digit formatter through its
                              paces. IntegerToHexStrExercises is the name of the
                              new test routine.
    ============================================================================
*/


using System;
using System.Collections.Generic;

using WizardWrx;
using WizardWrx.DLLServices2;

namespace DLLServices2TestStand
{
    internal static class NewClassTests_20140914
	{
		#region Private Test Data Arrays
		const string TEST_STRING_1 = "This test line has a Windows line terminator.\r\n";
		const string TEST_STRING_2 = "This test line has a Unix line terminator.\n";
		const string TEST_STRING_3 = "This test line is terminated by a carriage return.\r";
		const string TEST_STRING_4 = "This test line is unterminated.";

		static readonly string [ ] s_astrTestStrings =
		{
			TEST_STRING_1 ,
			TEST_STRING_2 ,
			TEST_STRING_3 ,
			TEST_STRING_4
		};	// const string [ ] s_astrTestStrings

		//	--------------------------------------------------------------------
		//	This list comprises a full set of IntegerToHexStrExercises test 
		//	cases, including a couple of illogical combinations.
		//	--------------------------------------------------------------------

		static readonly NumericFormats.HexFormatDecoration [ ] s_enmFormatFlags =
		{
			NumericFormats.HexFormatDecoration.None ,
			NumericFormats.HexFormatDecoration.Glyphs_LC ,
			NumericFormats.HexFormatDecoration.Glyphs_UC ,
			NumericFormats.HexFormatDecoration.Glyphs_LC | NumericFormats.HexFormatDecoration.Prefix_Oh_LC ,
			NumericFormats.HexFormatDecoration.Glyphs_LC | NumericFormats.HexFormatDecoration.Prefix_Oh_UC ,
			NumericFormats.HexFormatDecoration.Glyphs_LC | NumericFormats.HexFormatDecoration.Prefix_Ox_LC ,
			NumericFormats.HexFormatDecoration.Glyphs_LC | NumericFormats.HexFormatDecoration.Prefix_Ox_UC ,
			NumericFormats.HexFormatDecoration.Glyphs_LC | NumericFormats.HexFormatDecoration.Suffix_h_LC ,
			NumericFormats.HexFormatDecoration.Glyphs_LC | NumericFormats.HexFormatDecoration.Suffix_h_UC ,
			NumericFormats.HexFormatDecoration.Glyphs_UC | NumericFormats.HexFormatDecoration.Prefix_Ox_LC | NumericFormats.HexFormatDecoration.Suffix_h_LC ,
			NumericFormats.HexFormatDecoration.Glyphs_UC | NumericFormats.HexFormatDecoration.Prefix_Oh_LC ,
			NumericFormats.HexFormatDecoration.Glyphs_UC | NumericFormats.HexFormatDecoration.Prefix_Oh_UC ,
			NumericFormats.HexFormatDecoration.Glyphs_UC | NumericFormats.HexFormatDecoration.Prefix_Ox_LC ,
			NumericFormats.HexFormatDecoration.Glyphs_UC | NumericFormats.HexFormatDecoration.Prefix_Ox_UC ,
			NumericFormats.HexFormatDecoration.Glyphs_UC | NumericFormats.HexFormatDecoration.Suffix_h_LC ,
			NumericFormats.HexFormatDecoration.Glyphs_UC | NumericFormats.HexFormatDecoration.Suffix_h_UC
		};	// s_enmFormatFlags
		#endregion	// Private Test Data Arrays


		#region Public Methods
		internal static int ArrayInfoExercises ( ref int pintTestNumber )
        {
            const int TEST_INDEX = 10;

            BeginTest (
                System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                ref pintTestNumber );

            Console.WriteLine ( "    Public  Constant ArrayInfo.ARRAY_FIRST_ELEMENT  = {0}" , ArrayInfo.ARRAY_FIRST_ELEMENT );
            Console.WriteLine ( "    Public  Constant ArrayInfo.ARRAY_IS_EMPTY       = {0}" , ArrayInfo.ARRAY_IS_EMPTY );
            Console.WriteLine ( "    Public  Constant ArrayInfo.ARRAY_INVALID_INDEX  = {0}" , ArrayInfo.ARRAY_INVALID_INDEX );
            Console.WriteLine ( "    Public  Constant ArrayInfo.INDEX_FROM_ORDINAL   = {0}" , ArrayInfo.INDEX_FROM_ORDINAL );
            Console.WriteLine ( "    Public  Constant ArrayInfo.ARRAY_SECOND_ELEMENT = {0}" , ArrayInfo.ARRAY_SECOND_ELEMENT );
            Console.WriteLine ( "    Public  Constant ArrayInfo.NEXT_INDEX           = {0}" , ArrayInfo.NEXT_INDEX );
            Console.WriteLine ( "    Public  Constant ArrayInfo.ORDINAL_FROM_INDEX   = {0}{1}" , ArrayInfo.ORDINAL_FROM_INDEX , Environment.NewLine );

            Console.WriteLine ( "    Public method ArrayInfo.IndexFromOrdinal       = {0,2:N0} , for pintOrdinal = {1,2:N0}" , ArrayInfo.IndexFromOrdinal ( TEST_INDEX ) , TEST_INDEX );
            Console.WriteLine ( "    Public method ArrayInfo.OrdinalFromIndex       = {0,2:N0} , for pintIndex   = {1,2:N0}" , ArrayInfo.OrdinalFromIndex ( TEST_INDEX ) , TEST_INDEX );

            return TestDone (
                WizardWrx.MagicNumbers.ERROR_SUCCESS ,
                pintTestNumber );
        }   // internal static int ArrayInfoExercises


		internal static int ChopChop ( ref int pintTestNumber )
		{
			BeginTest (
				System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
				ref pintTestNumber );

			int intCaseNumber = MagicNumbers.ZERO;

			foreach ( string strTestCase in s_astrTestStrings )
			{
				string strChopped = StringTricks.Chop ( strTestCase );
				Console.WriteLine (
					Properties.Resources.CHOP_TEST_REPORT ,						// Format Control String (message template)
					new object [ ]
					{
						++intCaseNumber ,										// Format Item 0 = Case Number
						strTestCase.Length ,									// Format Item 1 = Input String Length
						strTestCase ,											// Format Item 2 = Input String
						strChopped.Length ,										// Format Item 3 = Output String Length
						strChopped ,											// Format Item 4 = Output String
						Environment.NewLine } );								// Format Item 5 = Newline, My Way
			}	// foreach ( string strTestCase in s_astrTestStrings )

			{	// Test the empty string degenerate case.
				string strChopped = StringTricks.Chop ( WizardWrx.SpecialStrings.EMPTY_STRING );
				Console.WriteLine (
					Properties.Resources.CHOP_TEST_REPORT ,						// Format Control String (message template)
					new object [ ]
					{
						++intCaseNumber ,										// Format Item 0 = Case Number
						SpecialStrings.EMPTY_STRING.Length ,					// Format Item 1 = Input String Length
						Properties.Resources.MESSAGE_EMPTY_STRING ,	 			// Format Item 2 = Input String
						strChopped.Length ,										// Format Item 3 = Output String Length
						string.IsNullOrEmpty ( strChopped )
							? Properties.Resources.MESSAGE_EMPTY_STRING :
							strChopped ,										// Format Item 4 = Output String
						Environment.NewLine } );								// Format Item 5 = Newline, My Way
			}	// Empty string degenerate case.

			{	// Test the null string degenerate case.
				string strChopped = StringTricks.Chop ( null );
				Console.WriteLine (
					Properties.Resources.CHOP_TEST_REPORT ,						// Format Control String (message template)
					new object [ ]
					{
						++intCaseNumber ,										// Format Item 0 = Case Number
						SpecialStrings.EMPTY_STRING.Length ,					// Format Item 1 = Input String Length
						Properties.Resources.MESSAGE_NULL_STRING ,				// Format Item 2 = Input String
						strChopped == null
							? ListInfo.EMPTY_STRING_LENGTH
							: strChopped.Length ,								// Format Item 3 = Output String Length
						strChopped == null
							? Properties.Resources.MESSAGE_NULL_STRING
							: strChopped ,										// Format Item 4 = Output String
						Environment.NewLine } );								// Format Item 5 = Newline, My Way
			}	// Null string degenerate case.

			return TestDone (
				WizardWrx.MagicNumbers.ERROR_SUCCESS ,
				pintTestNumber );
		}	// internal static int ChopChop


        internal static int CSVFileInfoExercises ( ref int pintTestNumber )
        {
            BeginTest (
                System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                ref pintTestNumber );

            Console.WriteLine ( "    Public  Constant CSVFileInfo.EMPTY_FILE     = {0}" , CSVFileInfo.EMPTY_FILE );
            Console.WriteLine ( "    Public  Constant CSVFileInfo.LABEL_ROW      = {0}" , CSVFileInfo.LABEL_ROW );
            Console.WriteLine ( "    Public  Constant CSVFileInfo.FIRST_RECORD   = {0}{1}" , CSVFileInfo.FIRST_RECORD , Environment.NewLine );

            string [ ] astrDummyRecords = new string [ ]
			{
				"LabelRow" ,
				"DataRRow1" ,
				"DataRow2" ,
				"DataRow3" ,
				"DataRow4" ,
				"DataRow5"
			};	// string [ ] astrDummyRecords

            Console.WriteLine (
				"    Public method CSVFileInfo.IndexFromOrdinal = {0} , for pastrWholeFile = {1}" ,
				CSVFileInfo.RecordCount ( astrDummyRecords ) ,
				astrDummyRecords.Length );

            return TestDone (
                WizardWrx.MagicNumbers.ERROR_SUCCESS ,
                pintTestNumber );
        }   // internal static int CSVFileInfoExercises


        internal static int DisplayFormatsExercises ( ref int pintTestNumber )
        {
            const int SAMPLE_INTEGER = 65533;               // 65,535 = (2 ^^ 16) - 3
            const double SAMPLE_FLOATING_POINT = 0.997;     // 99.7%

            BeginTest (
                System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                ref pintTestNumber );
            Console.WriteLine (
				"Integral format strings - sample = {0:n0}{1}" ,
				SAMPLE_INTEGER ,
				Environment.NewLine );

			Console.WriteLine ( "    IMPORTANT: As of version 6.2, the definitions of these formatting strings{0}               are in the NumericFormats class, but the indirect definitions{0}               in DisplayFormats are retained for convenient access to all{0}               display formats through a single class.{0}{0}               This report shows both sets.{0}" , Environment.NewLine );

			//	----------------------------------------------------------------
			//	Each group is enough to fill a screen, and the prologue gets one
			//	of its own.
			//	----------------------------------------------------------------

			Program.PauseForPictures ( Program.OMIT_LINEFEED );

			Console.WriteLine ( "{0}    --------------{0}    DisplayFormats{0}    --------------{0}" , Environment.NewLine );

            Console.WriteLine ( "    Public Constant DisplayFormats.HEXADECIMAL                       = {0} (Sample = {1})" , DisplayFormats.HEXADECIMAL_UC , SAMPLE_INTEGER.ToString ( DisplayFormats.HEXADECIMAL_UC ) );
            Console.WriteLine ( "    Public Constant DisplayFormats.HEXADECIMAL_2                     = {0} (Sample = {1})" , DisplayFormats.HEXADECIMAL_2 , SAMPLE_INTEGER.ToString ( DisplayFormats.HEXADECIMAL_2 ) );
            Console.WriteLine ( "    Public Constant DisplayFormats.HEXADECIMAL_4                     = {0} (Sample = {1})" , DisplayFormats.HEXADECIMAL_4 , SAMPLE_INTEGER.ToString ( DisplayFormats.HEXADECIMAL_4 ) );
            Console.WriteLine ( "    Public Constant DisplayFormats.HEXADECIMAL_8                     = {0} (Sample = {1}){2}" , DisplayFormats.HEXADECIMAL_8 , SAMPLE_INTEGER.ToString ( DisplayFormats.HEXADECIMAL_8 ) , Environment.NewLine );

            Console.WriteLine ( "    Public Constant DisplayFormats.HEXADECIMAL_PREFIX_0H_LC          = {0} (Sample = {0}{1})" , DisplayFormats.HEXADECIMAL_PREFIX_0H_LC , SAMPLE_INTEGER.ToString ( DisplayFormats.HEXADECIMAL_UC ) );
            Console.WriteLine ( "    Public Constant DisplayFormats.HEXADECIMAL_PREFIX_0H_UC          = {0} (Sample = {0}{1})" , DisplayFormats.HEXADECIMAL_PREFIX_0H_UC , SAMPLE_INTEGER.ToString ( DisplayFormats.HEXADECIMAL_UC ) );
            Console.WriteLine ( "    Public Constant DisplayFormats.HEXADECIMAL_PREFIX_0X_LC          = {0} (Sample = {0}{1})" , DisplayFormats.HEXADECIMAL_PREFIX_0X_LC , SAMPLE_INTEGER.ToString ( DisplayFormats.HEXADECIMAL_UC ) );
            Console.WriteLine ( "    Public Constant DisplayFormats.HEXADECIMAL_PREFIX_0X_UC          = {0} (Sample = {0}{1}){2}" , DisplayFormats.HEXADECIMAL_PREFIX_0X_UC , SAMPLE_INTEGER.ToString ( DisplayFormats.HEXADECIMAL_UC ) , Environment.NewLine );

			Console.WriteLine ( "    Public Constant DisplayFormats.HEXADECIMAL_PREFIX_0X_LC          = {0} (Sample = {0}{1})" , SAMPLE_INTEGER.ToString ( DisplayFormats.HEXADECIMAL_UC ) , NumericFormats.HEXADECIMAL_SUFFIX_H_LC );
			Console.WriteLine ( "    Public Constant DisplayFormats.HEXADECIMAL_PREFIX_0X_UC          = {0} (Sample = {0}{1}){2}" , SAMPLE_INTEGER.ToString ( DisplayFormats.HEXADECIMAL_UC ) , NumericFormats.HEXADECIMAL_SUFFIX_H_UC , Environment.NewLine );

			//	----------------------------------------------------------------
			//	Each group is enough to fill a screen.
			//	----------------------------------------------------------------

			Program.PauseForPictures ( Program.OMIT_LINEFEED );

			Console.WriteLine ( "{0}    --------------{0}    NumericFormats{0}    --------------{0}" , Environment.NewLine );

			Console.WriteLine ( "    Public Constant NumericFormats.HEXADECIMAL                       = {0} (Sample = {1})" , NumericFormats.HEXADECIMAL_UC , SAMPLE_INTEGER.ToString ( NumericFormats.HEXADECIMAL_UC ) );
			Console.WriteLine ( "    Public Constant NumericFormats.HEXADECIMAL_2                     = {0} (Sample = {1})" , NumericFormats.HEXADECIMAL_2 , SAMPLE_INTEGER.ToString ( NumericFormats.HEXADECIMAL_2 ) );
			Console.WriteLine ( "    Public Constant NumericFormats.HEXADECIMAL_4                     = {0} (Sample = {1})" , NumericFormats.HEXADECIMAL_4 , SAMPLE_INTEGER.ToString ( NumericFormats.HEXADECIMAL_4 ) );
			Console.WriteLine ( "    Public Constant NumericFormats.HEXADECIMAL_8                     = {0} (Sample = {1}){2}" , NumericFormats.HEXADECIMAL_8 , SAMPLE_INTEGER.ToString ( NumericFormats.HEXADECIMAL_8 ) , Environment.NewLine );

			Console.WriteLine ( "    Public Constant NumericFormats.HEXADECIMAL_PREFIX_0H_LC          = {0} (Sample = {0}{1})" , NumericFormats.HEXADECIMAL_PREFIX_0H_LC , SAMPLE_INTEGER.ToString ( NumericFormats.HEXADECIMAL_UC ) );
			Console.WriteLine ( "    Public Constant NumericFormats.HEXADECIMAL_PREFIX_0H_UC          = {0} (Sample = {0}{1})" , NumericFormats.HEXADECIMAL_PREFIX_0H_UC , SAMPLE_INTEGER.ToString ( NumericFormats.HEXADECIMAL_UC ) );
			Console.WriteLine ( "    Public Constant NumericFormats.HEXADECIMAL_PREFIX_0X_LC          = {0} (Sample = {0}{1})" , NumericFormats.HEXADECIMAL_PREFIX_0X_LC , SAMPLE_INTEGER.ToString ( NumericFormats.HEXADECIMAL_UC ) );
			Console.WriteLine ( "    Public Constant NumericFormats.HEXADECIMAL_PREFIX_0X_UC          = {0} (Sample = {0}{1}){2}" , NumericFormats.HEXADECIMAL_PREFIX_0X_UC , SAMPLE_INTEGER.ToString ( NumericFormats.HEXADECIMAL_UC ) , Environment.NewLine );

			Console.WriteLine ( "    Public Constant NumericFormats.HEXADECIMAL_PREFIX_0X_LC          = {0} (Sample = {0}{1})" , SAMPLE_INTEGER.ToString ( NumericFormats.HEXADECIMAL_UC ) , NumericFormats.HEXADECIMAL_SUFFIX_H_LC );
			Console.WriteLine ( "    Public Constant NumericFormats.HEXADECIMAL_PREFIX_0X_UC          = {0} (Sample = {0}{1}){2}" , SAMPLE_INTEGER.ToString ( NumericFormats.HEXADECIMAL_UC ) , NumericFormats.HEXADECIMAL_SUFFIX_H_UC , Environment.NewLine );

			Program.PauseForPictures ( Program.OMIT_LINEFEED );

			IntegerToHexStrExercises ( );			
			
			Program.PauseForPictures ( Program.OMIT_LINEFEED );

			Console.WriteLine (
				"{1}More Integral format strings - sample = {0}{1}" ,
				SAMPLE_INTEGER ,
				Environment.NewLine );

            Console.WriteLine ( "    Public Constant DisplayFormats.NUMBER_PER_REG_SETTINGS           = {0} (Sample = {1})" , DisplayFormats.NUMBER_PER_REG_SETTINGS , SAMPLE_INTEGER.ToString ( DisplayFormats.NUMBER_PER_REG_SETTINGS ) );
            Console.WriteLine ( "    Public Constant DisplayFormats.NUMBER_PER_REG_SETTINGS_0D        = {0} (Sample = {1})" , DisplayFormats.NUMBER_PER_REG_SETTINGS_0D , SAMPLE_INTEGER.ToString ( DisplayFormats.NUMBER_PER_REG_SETTINGS_0D ) );
            Console.WriteLine ( "    Public Constant DisplayFormats.NUMBER_PER_REG_SETTINGS_2D        = {0} (Sample = {1})" , DisplayFormats.NUMBER_PER_REG_SETTINGS_2D , SAMPLE_INTEGER.ToString ( DisplayFormats.NUMBER_PER_REG_SETTINGS_2D ) );
            Console.WriteLine ( "    Public Constant DisplayFormats.NUMBER_PER_REG_SETTINGS_3D        = {0} (Sample = {1})" , DisplayFormats.NUMBER_PER_REG_SETTINGS_3D , SAMPLE_INTEGER.ToString ( DisplayFormats.NUMBER_PER_REG_SETTINGS_3D ) );

            Console.WriteLine ( "{1}Floating point format strings - sample = {0}{1}" , SAMPLE_FLOATING_POINT , Environment.NewLine );

            Console.WriteLine ( "    Public Constant DisplayFormats.PERCENT                           = {0} (Sample = {1})" , DisplayFormats.PERCENT , SAMPLE_FLOATING_POINT.ToString ( DisplayFormats.PERCENT ) );
            Console.WriteLine ( "    Public Constant DisplayFormats.PERCENT_DIGITS_2                  = {0} (Sample = {1})" , DisplayFormats.PERCENT_DIGITS_2 , SAMPLE_FLOATING_POINT.ToString ( DisplayFormats.PERCENT_DIGITS_2 ) );

			Program.PauseForPictures ( Program.APPEND_LINEFEED );

            DateTime dtmSample = DateTime.Now;

			Console.WriteLine ( "{1}Date and Time format strings - sample = {0}{1}" , dtmSample , Environment.NewLine );

            Console.WriteLine ( "    Public Constant DisplayFormats.STANDARD_DISPLAY_DATE_FORMAT      = {0} (Sample = {1})" , DisplayFormats.STANDARD_DISPLAY_DATE_FORMAT , SysDateFormatters.ReformatSysDate ( dtmSample , DisplayFormats.STANDARD_DISPLAY_DATE_FORMAT ) );
            Console.WriteLine ( "    Public Constant DisplayFormats.STANDARD_DISPLAY_DATE_TIME_FORMAT = {0} (Sample = {1})" , DisplayFormats.STANDARD_DISPLAY_DATE_TIME_FORMAT , SysDateFormatters.ReformatSysDate ( dtmSample , DisplayFormats.STANDARD_DISPLAY_DATE_TIME_FORMAT ) );
            Console.WriteLine ( "    Public Constant DisplayFormats.STANDARD_DISPLAY_TIME_FORMAT      = {0} (Sample = {1})" , DisplayFormats.STANDARD_DISPLAY_TIME_FORMAT , SysDateFormatters.ReformatSysDate ( dtmSample , DisplayFormats.STANDARD_DISPLAY_TIME_FORMAT ) );

            Console.WriteLine ( "{1}    Public Method DisplayFormats.FormatDateForShow                   = {0}" , DisplayFormats.FormatDateForShow ( dtmSample ) , Environment.NewLine );
			Console.WriteLine ( "    Public Method DisplayFormats.FormatDateTimeForShow               = {0}" , DisplayFormats.FormatDateTimeForShow ( dtmSample ) );
			Console.WriteLine ( "    Public Method DisplayFormats.FormatTimeForShow                   = {0}" , DisplayFormats.FormatTimeForShow ( dtmSample ) );

            return TestDone (
                WizardWrx.MagicNumbers.ERROR_SUCCESS ,
                pintTestNumber );
        }   // internal static int DisplayFormatsExercises


        internal static int FileIOFlagsExercises ( ref int pintTestNumber )
        {
            BeginTest (
                System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                ref pintTestNumber );

			Console.WriteLine ( "    Public Constant FileIOFlags.FILE_COPY_OVERWRITE_FORBIDDEN    = {0}" , FileIOFlags.FILE_COPY_OVERWRITE_FORBIDDEN );
			Console.WriteLine ( "    Public Constant FileIOFlags.FILE_COPY_OVERWRITE_PERMITTED    = {0}{1}" , FileIOFlags.FILE_COPY_OVERWRITE_PERMITTED , Environment.NewLine );

			Console.WriteLine ( "    Public Constant FileIOFlags.FILE_OUT_APPEND                  = {0}" , FileIOFlags.FILE_OUT_APPEND );
			Console.WriteLine ( "    Public Constant FileIOFlags.FILE_OUT_CREATE                  = {0}{1}" , FileIOFlags.FILE_OUT_CREATE , Environment.NewLine );

			Console.WriteLine ( "    Public Constant FileIOFlags.FILE_READ_ENCODING_CHECK_FOR_BOM = {0}" , FileIOFlags.FILE_READ_ENCODING_CHECK_FOR_BOM );
			Console.WriteLine ( "    Public Constant FileIOFlags.FILE_READ_ENCODING_IGNORE_BOM    = {0}{1}" , FileIOFlags.FILE_READ_ENCODING_IGNORE_BOM , Environment.NewLine );

			Console.WriteLine ( "    Public Constant FileIOFlags.MAKE_STREAM_IO_ASYNCHRONOUS      = {0}" , FileIOFlags.MAKE_STREAM_IO_ASYNCHRONOUS );
			Console.WriteLine ( "    Public Constant FileIOFlags.MAKE_STREAM_IO_SYNCHRONOUS       = {0}" , FileIOFlags.MAKE_STREAM_IO_SYNCHRONOUS );

            return TestDone (
                WizardWrx.MagicNumbers.ERROR_SUCCESS ,
                pintTestNumber );
        }   // internal static int FileIOFlagsExercises


		private static void IntegerToHexStrExercises ( )
		{
			//	----------------------------------------------------------------
			//	All but the last test use the same sample integer.
			//	----------------------------------------------------------------

			const int TEST_INTEGER = 123456789;
			const double TEST_NON_INTEGER = 3.15159;
			const int HEX_GLYPHS_DEFAULT_MINIMUM = 8;

			int intTotalCases = s_enmFormatFlags.Length;

			Console.WriteLine (
				"{2}Thoroughly exercise the IntegerToHexStr method in class NumericFormats.{2}{2}    All {0} test cases use the same integer, {1:n0}.{2}" ,
				intTotalCases ,			// Format Item 0 = Total test cases, read from Length property of the array of test cases
				TEST_INTEGER ,			// Format Item 1 = Integer used for all tests
				Environment.NewLine );	// Format Item 2 = Embedded newline

			for ( int intCurrentCase = ArrayInfo.ARRAY_FIRST_ELEMENT ;
					  intCurrentCase < intTotalCases ;
					  intCurrentCase++ )
			{
				int intScenarioOrdinal = ArrayInfo.OrdinalFromIndex ( intCurrentCase );
				Console.WriteLine (
					"    Scenario {0} of {1}: HexFormatDecoration = {2} (as integer = {3}):{4}" ,	// Format control string
					new object [ ]
					{
						intScenarioOrdinal ,														// Format Item 0 = Scenario number
						intTotalCases ,																// Format Item 1 = Total number of scenarios
						s_enmFormatFlags [ intCurrentCase ] ,										// Format Item 2 = Enumeration value, as member name
						( int ) s_enmFormatFlags [ intCurrentCase ] ,								// Format Item 3 = Enumeration value, as integer
						Environment.NewLine															// Format Item 4 = Embedded Newline
					} );

				for ( int intMinDigits = MagicNumbers.PLUS_ONE ;
						  intMinDigits <= HEX_GLYPHS_DEFAULT_MINIMUM ;
						  intMinDigits++ )
				{
					Console.WriteLine (
						"    Test case {0}: HexFormatDecoration = {1,-12} Minimum Digits = {2} Result = {3}" ,
						new object [ ]
							{
								intScenarioOrdinal ,												// Format Item 0 = Ordinal test case number derived from array subscript
								s_enmFormatFlags [ intCurrentCase ] ,								// Format Item 1 = HexFormatDecoration value
								intMinDigits ,														// Format Item 2 = Minimum digits to display in this test.
								NumericFormats.IntegerToHexStr (									// Format Item 3 = Formatted integer
									TEST_INTEGER ,													//		T pintegralValue
									intMinDigits ,													//		int pintTotalDigits
									s_enmFormatFlags [ intCurrentCase ] )							//		HexFormatDecoration penmHexDecoration
							} );
				}	// for ( int intMinDigits = MagicNumbers.PLUS_ONE ; intMinDigits <= HEX_GLYPHS_DEFAULT_MINIMUM ; intMinDigits++ )

				Console.WriteLine ( );																// PauseForPictures adds its line feed after the message. I want one before.
				Program.PauseForPictures ( Program.OMIT_LINEFEED );
			}	// for ( int intCurrentCase = ArrayInfo.ARRAY_FIRST_ELEMENT ; intCurrentCase < intTotalCases ; intCurrentCase++ )

			try
			{
				Console.WriteLine ( "    Scenario {0} deliberately throws, because the input is non-integral." , intTotalCases++ );
				Console.WriteLine (
					"    Test case {0}: HexFormatDecoration = {1,-12} Minimum Digits = {2} Result = {3}" ,
					new object [ ]
							{
								intTotalCases ,														// Format Item 0 = Ordinal test case number derived from array subscript
								s_enmFormatFlags [ ArrayInfo.ARRAY_SECOND_ELEMENT ] ,				// Format Item 1 = HexFormatDecoration value
								HEX_GLYPHS_DEFAULT_MINIMUM ,										// Format Item 2 = Minimum digits to display in this test.
								NumericFormats.IntegerToHexStr (									// Format Item 3 = Formatted integer
									TEST_NON_INTEGER ,												//		T pintegralValue
									HEX_GLYPHS_DEFAULT_MINIMUM ,									//		int pintTotalDigits
									s_enmFormatFlags [ ArrayInfo.ARRAY_SECOND_ELEMENT ] )			//		HexFormatDecoration penmHexDecoration
							} );
			}
			catch ( Exception exBadArg )
			{
				StateManager.GetTheSingleInstance ( ).AppExceptionLogger.ReportException ( exBadArg );
			}
			finally
			{
				Console.WriteLine (
					"{0}IntegerToHexStr method exercised.{0}" ,
					Environment.NewLine );
			}		// IntegerToHexStrExercises
		}


        internal static int ListInfoExercises ( ref int pintTestNumber )
        {
            const string SAMPLE_1 = @"The quick brown fox jumped over the lazy dog.";
            const string SAMPLE_2 = @"A";

            const int NTH_1 = 5;
            const int NTH_2 = 100;
            const string NULL_CHAR = @"NUL";

            BeginTest (
                System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                ref pintTestNumber );

			Console.WriteLine ( "    Public Constant ListInfo.BEGINNING_OF_BUFFER       = {0}" , ListInfo.BEGINNING_OF_BUFFER );
			Console.WriteLine ( "    Public Constant ListInfo.BINARY_SEARCH_NOT_FOUND   = {0}" , ListInfo.BINARY_SEARCH_NOT_FOUND );
			Console.WriteLine ( "    Public Constant ListInfo.EMPTY_STRING_LENGTH       = {0}" , ListInfo.EMPTY_STRING_LENGTH );
            Console.WriteLine ( "    Public Constant ListInfo.INDEXOF_NOT_FOUND         = {0}" , ListInfo.INDEXOF_NOT_FOUND );
            Console.WriteLine ( "    Public Constant ListInfo.LIST_IS_EMPTY             = {0}" , ListInfo.LIST_IS_EMPTY );
            Console.WriteLine ( "    Public Constant ListInfo.SUBSTR_BEGINNING          = {0}" , ListInfo.SUBSTR_BEGINNING );
            Console.WriteLine ( "    Public Constant ListInfo.SUBSTR_SECOND_CHAR        = {0}" , ListInfo.SUBSTR_SECOND_CHAR );

            Console.WriteLine ( "{1}Input Sample for the following Public Method tests = {0}{1}" , SAMPLE_1 , Environment.NewLine );
            Console.WriteLine ( "    Public Method ListInfo.FirstCharOfString           = {0}" , ListInfo.FirstCharOfString ( SAMPLE_1 ) );
            Console.WriteLine ( "    Public Method ListInfo.LastCharacterOfString       = {0}" , ListInfo.LastCharacterOfString ( SAMPLE_1 ) );

            Console.WriteLine ( "    Public Method ListInfo.NthCharacterOfString        = {0}, where N = {1}" , ListInfo.NthCharacterOfString ( SAMPLE_1 , NTH_1 ) , NTH_1 );
            Console.WriteLine ( "    Public Method ListInfo.NthCharacterOfString        = {0}, where N = {1}" , ListInfo.NthCharacterOfString ( SAMPLE_1 , NTH_2 ) , NTH_2 );

            //  ----------------------------------------------------------------
            //  Since the null character returned for the error doesn't print,
            //  and I want to display a string when that happens, this test is a
            //  tad artificial, because it masks the fact that both methods
            //  (PenultimateCharactrOfString and SecondCharacterOfString) return
            //  a character (char).
            //  ----------------------------------------------------------------

            Console.WriteLine ( "    Public Method ListInfo.PenultimateCharactrOfString = {0}" , ListInfo.PenultimateCharactrOfString ( SAMPLE_1 ) != SpecialCharacters.NULL_CHAR ? ListInfo.PenultimateCharactrOfString ( SAMPLE_1 ).ToString ( ) : NULL_CHAR );
            Console.WriteLine ( "    Public Method ListInfo.PenultimateCharactrOfString = {0}" , ListInfo.PenultimateCharactrOfString ( SAMPLE_2 ) != SpecialCharacters.NULL_CHAR ? ListInfo.PenultimateCharactrOfString ( SAMPLE_2 ).ToString ( ) : NULL_CHAR );

            Console.WriteLine ( "    Public Method ListInfo.SecondCharacterOfString     = {0}" , ListInfo.SecondCharacterOfString ( SAMPLE_1 ) != SpecialCharacters.NULL_CHAR ? ListInfo.SecondCharacterOfString ( SAMPLE_1 ).ToString ( ) : NULL_CHAR );
            Console.WriteLine ( "    Public Method ListInfo.SecondCharacterOfString     = {0}" , ListInfo.SecondCharacterOfString ( SAMPLE_2 ) != SpecialCharacters.NULL_CHAR ? ListInfo.SecondCharacterOfString ( SAMPLE_2 ).ToString ( ) : NULL_CHAR );

            return TestDone (
                WizardWrx.MagicNumbers.ERROR_SUCCESS ,
                pintTestNumber );
        }   // internal static int ListInfoExercises


        internal static int PathPositionsExercises ( ref int pintTestNumber )
        {
            BeginTest (
                System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                ref pintTestNumber );

            Console.WriteLine ( "    Public Constant PathPositions.FQFN_PREFIX_START_POS   = {0}" , PathPositions.FQFN_PREFIX_START_POS );
            Console.WriteLine ( "    Public Constant PathPositions.FQFN_PREFIX_START_LEN   = {0}" , PathPositions.FQFN_PREFIX_START_LEN );
            Console.WriteLine ( "    Public Constant PathPositions.MAX_PATH                = {0}" , PathPositions.MAX_PATH );
            Console.WriteLine ( "    Public Constant PathPositions.UNC_HOSTNAME_PREFIX_POS = {0}" , PathPositions.UNC_HOSTNAME_PREFIX_POS );
            Console.WriteLine ( "    Public Constant PathPositions.UNC_HOSTNAME_START_POS  = {0}" , PathPositions.UNC_HOSTNAME_START_POS );

            return TestDone (
                WizardWrx.MagicNumbers.ERROR_SUCCESS ,
                pintTestNumber );
        }   // internal static int PathPositionsExercises


        internal static int SpecialCharactersExercises ( ref int pintTestNumber )
        {
            BeginTest (
                System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                ref pintTestNumber );

			Console.WriteLine ( "    Public Constant SpecialCharacters.AMPERSAND     = {0} (ASCII code = {1,2:N0} (0x{2})" , SpecialCharacters.AMPERSAND , ( int ) SpecialCharacters.AMPERSAND , ( ( int ) SpecialCharacters.AMPERSAND ).ToString ( DisplayFormats.HEXADECIMAL_2 ) );
            Console.WriteLine ( "    Public Constant SpecialCharacters.COLON         = {0} (ASCII code = {1,2:N0} (0x{2})" , SpecialCharacters.COLON , ( int ) SpecialCharacters.COLON , ( ( int ) SpecialCharacters.COLON ).ToString ( DisplayFormats.HEXADECIMAL_2 ) );
            Console.WriteLine ( "    Public Constant SpecialCharacters.COMMA         = {0} (ASCII code = {1,2:N0} (0x{2})" , SpecialCharacters.COMMA , ( int ) SpecialCharacters.COMMA , ( ( int ) SpecialCharacters.COMMA ).ToString ( DisplayFormats.HEXADECIMAL_2 ) );
            Console.WriteLine ( "    Public Constant SpecialCharacters.DOUBLE_QUOTE  = {0} (ASCII code = {1,2:N0} (0x{2})" , SpecialCharacters.DOUBLE_QUOTE , ( int ) SpecialCharacters.DOUBLE_QUOTE , ( ( int ) SpecialCharacters.DOUBLE_QUOTE ).ToString ( DisplayFormats.HEXADECIMAL_2 ) );
			Console.WriteLine ( "    Public Constant SpecialCharacters.FULL_STOP     = {0} (ASCII code = {1,2:N0} (0x{2})" , SpecialCharacters.FULL_STOP , ( int ) SpecialCharacters.FULL_STOP , ( ( int ) SpecialCharacters.FULL_STOP ).ToString ( DisplayFormats.HEXADECIMAL_2 ) );
            Console.WriteLine ( "    Public Constant SpecialCharacters.NUL           = {0} (ASCII code = {1,2:N0} (0x{2})" , SpecialCharacters.NULL_CHAR , ( int ) SpecialCharacters.NULL_CHAR , ( ( int ) SpecialCharacters.NULL_CHAR ).ToString ( DisplayFormats.HEXADECIMAL_2 ) );
			Console.WriteLine ( "    Public Constant SpecialCharacters.PERCENT_SIGN  = {0} (ASCII code = {1,2:N0} (0x{2})" , SpecialCharacters.PERCENT_SIGN , ( int ) SpecialCharacters.PERCENT_SIGN , ( ( int ) SpecialCharacters.PERCENT_SIGN ).ToString ( DisplayFormats.HEXADECIMAL_2 ) );
			Console.WriteLine ( "    Public Constant SpecialCharacters.SEMICOLON     = {0} (ASCII code = {1,2:N0} (0x{2})" , SpecialCharacters.SEMICOLON , ( int ) SpecialCharacters.SEMICOLON , ( ( int ) SpecialCharacters.SEMICOLON ).ToString ( DisplayFormats.HEXADECIMAL_2 ) );
            Console.WriteLine ( "    Public Constant SpecialCharacters.SPACE         = {0} (ASCII code = {1,2:N0} (0x{2})" , SpecialCharacters.SPACE_CHAR , ( int ) SpecialCharacters.SPACE_CHAR , ( ( int ) SpecialCharacters.SPACE_CHAR ).ToString ( DisplayFormats.HEXADECIMAL_2 ) );
            Console.WriteLine ( "    Public Constant SpecialCharacters.TAB           = {0} (ASCII code = {1,2:N0} (0x{2})" , SpecialCharacters.TAB_CHAR , ( int ) SpecialCharacters.TAB_CHAR , ( ( int ) SpecialCharacters.TAB_CHAR ).ToString ( DisplayFormats.HEXADECIMAL_2 ) );

			return TestDone (
                WizardWrx.MagicNumbers.ERROR_SUCCESS ,
                pintTestNumber );
        }   // internal static int SpecialCharactersExercises


        internal static int UtilsExercises ( ref int pintTestNumber )
		{
			const string TEST_FILENAME_1 = @"WEBPMTS.TXT";
			const string TEST_FILENAME_2 = @"WEBPMTS.PSV";

			const string TEST_MASK_1 = @"WEBPMTS\.*";
			const string TEST_MASK_2 = @"WEBPMTS.TXT";

			const int    REG_DWORD_DEFAULT	  = 0;

			const long   REG_QWORD_DEFAULT	  = 0;

			const bool   REGISTRY_WRITING_OFF = false;

			const string REGISTRY_KEY_1 = @"Control Panel\Desktop\WindowMetrics";												// in HKEY_CURRENT_USER
			const string REGISTRY_KEY_2 = @"Control Panel\International";														// in HKEY_CURRENT_USER
			const string REGISTRY_KEY_3 = @"SYSTEM\CurrentControlSet\Control\Session Manager";									// in HKEY_LOCAL_MACHINE
			//	To keep the key and value string names synced, strings REGISTRY_KEY_4 and REGISTRY_5 are reserved, but unused.
			const string REGISTRY_KEY_6 = @"Software\Microsoft\Office\14.0\Excel\Security\Trusted Locations\Location2";			// HKEY_CURRENT_USER
			const string REGISTRY_KEY_7 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\StructuredQuery";							// in HKEY_LOCAL_MACHINE
			const string REGISTRY_KEY_8 = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones\Central Standard Time";		// in HKEY_LOCAL_MACHINE

			const string REGISTRY_VALUE_1 = @"AppliedDPI";																		// Valuse exists, and its type is REG_DWORD.
			const string REGISTRY_VALUE_2 = @"iDigits";																			// Value exists, but its type is REG_SZ, rather than the expected REG_DWORD.
			const string REGISTRY_VALUE_3 = @"ObjectDirectories";																// This REG_MULTI_SZ value, which lives in Registry key HKLM\SYSTEM\CurrentControlSet\Control\Session Manager, contains two substrings.
			const string REGISTRY_VALUE_4 = @"SetupExecute";																	// This REG_MULTI_SZ value, which lives in Registry key HKLM\SYSTEM\CurrentControlSet\Control\Session Manager, contains ZERO substrings.
			const string REGISTRY_VALUE_5 = @"SetupExecute2";																	// This REG_MULTI_SZ value doesn't exist in Registry key HKLM\SYSTEM\CurrentControlSet\Control\Session Manager.
			const string REGISTRY_VALUE_6 = @"Path";																			// This REG_EXPAND_SZ value exists, and is of the expected type.
			const string REGISTRY_VALUE_7 = @"SchemaChangedLast";
			const string REGISTRY_VALUE_8 = @"TZI";

			const string REGISTRY_VALUE_TYPE_BINARY = @"REG_BINARY";
			const string REGISTRY_VALUE_TYPE_DWORD	= @"REG_DWORD";
			const string REGISTRY_VALUE_TYPE_EXPAND = @"REG_EXPAND_SZ";
			const string REGISTRY_VALUE_TYPE_MULTI  = @"REG_MULTI_SZ";
			const string REGISTRY_VALUE_TYPE_QWORD	= @"REG_QWORD";

			BeginTest (
				System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
				ref pintTestNumber );

			Console.WriteLine (
				Properties.Resources.REGEXP_TEST_ALL_BUT_LAST ,					// Format Control String
				Util.FileMatchesRegExpMask (
					TEST_FILENAME_1 ,
					TEST_MASK_1 ) ,												// Format Item 0 = Files matching mask
				TEST_FILENAME_1 ,												// Format Item 1 = File name from which mask was constructed
				TEST_MASK_1 );													// Format Item 2 = Regular Expression match expression generated from filename
			Console.WriteLine (
				Properties.Resources.REGEXP_TEST_ALL_BUT_LAST ,					// Format Control String
				Util.FileMatchesRegExpMask (
					TEST_FILENAME_1 ,
					TEST_MASK_2 ) ,												// Format Item 0 = Files matching mask
				TEST_FILENAME_1 ,												// Format Item 1 = File name from which mask was constructed
				TEST_MASK_2 );													// Format Item 2 = Regular Expression match expression generated from filename
			Console.WriteLine (
				Properties.Resources.REGEXP_TEST_ALL_BUT_LAST ,					// Format Control String
				Util.FileMatchesRegExpMask (
					TEST_FILENAME_2 ,
					TEST_MASK_1 ) ,												// Format Item 0 = Files matching mask
				TEST_FILENAME_2 ,												// Format Item 1 = File name from which mask was constructed
				TEST_MASK_1 );													// Format Item 2 = Regular Expression match expression generated from filename

			Console.WriteLine (
				Properties.Resources.REGEXP_TEST_LAST ,							// Format Control String
				new object [ ]
				{
					Util.FileMatchesRegExpMask (
						TEST_FILENAME_2 ,
						TEST_MASK_2 ) ,											// Format Item 0 = Files matching mask
					TEST_FILENAME_2 ,											// Format Item 1 = File name from which mask was constructed
					TEST_MASK_2 ,												// Format Item 2 = Regular Expression match expression generated from filename
					Environment.NewLine											// Format Item 3 = Newline, my way
				} );

			Program.PauseForPictures ( Program.APPEND_LINEFEED );

			//  ----------------------------------------------------------------
			//  Since GetDisplayTimeZone uses GetSystemTimeZoneInfo, this test
			//  case covers both.
			//  ----------------------------------------------------------------

#if NET35
            Console.WriteLine ( "{2}Current Local Time Zone for Machine {0} = {1}{2}" ,     // Message Template
                Environment.MachineName ,                                                   // Format Item 0 = Machine Name
                Util.GetDisplayTimeZone (                                                   // Format Item 1 = Time Zone
                    DateTime.Now ,                                                              // DateTime pdtmTestDate = Current time from clock
                    TimeZoneInfo.Local.Id ) ,                                                   // string pstrTimeZoneID = ID of Local Machine time zone, per TimeZoneInfo
                Environment.NewLine );                                                      // Format Item 2 = Newline, my way
#endif	// #if NET35

			{   // Constrain the scope of Microsoft.Win32.RegistryKey hK and string strTpl.
				string strTpl					= Properties.Resources.MSG_REG_KEY_VALUE;
				Microsoft.Win32.RegistryKey hK	= Microsoft.Win32.Registry.CurrentUser.OpenSubKey (
					REGISTRY_KEY_1 ,
					REGISTRY_WRITING_OFF );

				Console.WriteLine (
					strTpl ,
					new string [ ]
					{
						hK.Name ,												// Format Item 0 = Key Name
						REGISTRY_VALUE_1 ,										// Format Item 1 = Value Name
						REGISTRY_VALUE_TYPE_DWORD ,								// Format Item 2 = Value Type
						RegistryValues.RegQueryValue (
							hK ,
							REGISTRY_VALUE_1 ,
							REG_DWORD_DEFAULT ).ToString ( ) ,					// Format Item 3 = Value Data
						Environment.NewLine										// Format Item 4 = Newline my way
					} );

				hK.Close ( );
				hK = null;

				hK = Microsoft.Win32.Registry.CurrentUser.OpenSubKey (
					REGISTRY_KEY_2 ,
					REGISTRY_WRITING_OFF );

				Console.WriteLine (
					strTpl ,
					new string [ ]
					{
						hK.Name ,												// Format Item 0 = Key Name
						REGISTRY_VALUE_2 ,										// Format Item 1 = Value Name
						REGISTRY_VALUE_TYPE_DWORD ,								// Format Item 2 = Value Type
						RegistryValues.RegQueryValue (
							hK ,
							REGISTRY_VALUE_2 ,
							REG_DWORD_DEFAULT ).ToString ( ) ,					// Format Item 3 = Value Data
						Environment.NewLine										// Format Item 4 = Newline my way
					} );

				hK.Close ( );
				hK = null;

				hK = Microsoft.Win32.Registry.CurrentUser.OpenSubKey (
					REGISTRY_KEY_6 ,
					REGISTRY_WRITING_OFF );

				Console.WriteLine (
					strTpl ,
					new object [ ]
					{
						hK.Name ,												// Format Item 0 = Key Name
						REGISTRY_VALUE_6 ,										// Format Item 1 = Value Name
						REGISTRY_VALUE_TYPE_EXPAND ,							// Format Item 2 = Value Type
						RegistryValues.RegQueryValue (
							hK ,
							REGISTRY_VALUE_6 ,
							string.Empty ) ,									// Format Item 3 = Value Data
						Environment.NewLine										// Format Item 4 = Newline my way
					} );

				hK.Close ( );
				hK = null;

				Program.PauseForPictures ( Program.APPEND_LINEFEED );

				string [ ] astrValueNames = new string [ ]
					{
	                    REGISTRY_VALUE_3 ,
		                REGISTRY_VALUE_4 ,
			            REGISTRY_VALUE_5
				    };  // string [ ] astrValueNames = new string [ ]

				hK = Microsoft.Win32.Registry.LocalMachine.OpenSubKey (
					REGISTRY_KEY_3 ,
					REGISTRY_WRITING_OFF );

				foreach ( string strValue in astrValueNames )
				{
					string [ ] astrIndividualValues = RegistryValues.RegQueryValue (
						hK ,
						strValue );
					int intNValues = astrIndividualValues.Length;

					Console.WriteLine (
						strTpl ,
						new string [ ]
					{
						hK.Name ,												// Format Item 0 = Key Name
						strValue ,												// Format Item 1 = Value Name
						REGISTRY_VALUE_TYPE_MULTI ,								// Format Item 2 = Value Type
						string.Concat (
							intNValues ,
						    Properties.Resources.MSG_SUBSTRING_SUMMARY_1 ,
							intNValues > ArrayInfo.ARRAY_IS_EMPTY
							? Properties.Resources.MSG_SUBSTRING_SUMMARY_2
							: string.Empty ) ,									// Format Item 3 = Value Data
						Environment.NewLine										// Format Item 4 = Newline my way, but only if substrings follow.
					} );

					//  --------------------------------------------------------
					//  This loop exercises methods IsFirstForIteration and
					//  IsLastForIterationLE.
					//  --------------------------------------------------------

					for ( int intIndex = ArrayInfo.ARRAY_FIRST_ELEMENT ;
							  intIndex < intNValues ;
							  intIndex++ )
					{
						Console.WriteLine (
							Properties.Resources.MSG_SUBSTRING_VALUE ,
							new object [ ]
                            {	// These ternary expressions use some of the routines under test.
                                ArrayInfo.OrdinalFromIndex ( intIndex ) ,		// Format Item 0 = Ordinal Position of Substring
                                astrIndividualValues [ intIndex ] ,				// Format Item 1 = Substring Value
                                Logic.IsLastForIterationLT ( intIndex ,
									                         intNValues )
                                    ? Environment.NewLine
                                    : string.Empty								// Format Item 2 = Newline on last item, otherwise, nothing
                            } );
					}   // for ( int intIndex = ArrayInfo.ARRAY_FIRST_ELEMENT ; intIndex < intNValues ; intNValues++ )
				}   // foreach ( string strValue in astrValueNames )

				hK.Close ( );
				hK = null;

				Program.PauseForPictures ( Program.APPEND_LINEFEED );

				hK = Microsoft.Win32.Registry.LocalMachine.OpenSubKey (
					REGISTRY_KEY_7 ,
					REGISTRY_WRITING_OFF );
				Console.WriteLine (
					strTpl ,
					new object [ ]
					{
						hK.Name ,												// Format Item 0 = Key Name
						REGISTRY_VALUE_7 ,										// Format Item 1 = Value Name
						REGISTRY_VALUE_TYPE_QWORD ,								// Format Item 2 = Value Type
					    ( long ) RegistryValues.RegQueryValue (
							hK ,
							REGISTRY_VALUE_7 ,
							REG_QWORD_DEFAULT ) ,								// Format Item 3 = Value Data
						Environment.NewLine										// Format Item 4 = Newline my way
					} );

				hK.Close ( );
				hK = null;

				hK = Microsoft.Win32.Registry.LocalMachine.OpenSubKey (
					REGISTRY_KEY_8 ,
					REGISTRY_WRITING_OFF );
				Console.WriteLine (
					strTpl ,
					new string [ ]
					{
						hK.Name ,												// Format Item 0 = Key Name
						REGISTRY_VALUE_8 ,										// Format Item 1 = Value Name
						REGISTRY_VALUE_TYPE_BINARY ,							// Format Item 2 = Value Type
						Util.ByteArrayToHexDigitString (
							RegistryValues.RegQueryValue (
								hK ,
								REGISTRY_VALUE_8 ,
								RegistryValues.REG_BINARY_NULL_FOR_ABSENT ) ,
							Util.BYTES_TO_STRING_BLOCK_OF_4 ) ,					// Format Item 3 = Value Data
						Environment.NewLine										// Format Item 4 = Newline my way
					} );

				hK.Close ( );
				hK = null;
			}   // Microsoft.Win32.RegistryKey hK and string strTpl go out of scope.

			//	----------------------------------------------------------------
			//	Test the Unless idiom.
			//	----------------------------------------------------------------

			UnlessWhat ( );

			Program.PauseForPictures ( Program.OMIT_LINEFEED );

			//	----------------------------------------------------------------
			//	Test the loop state evaluators.
			//	----------------------------------------------------------------

			EvaluateLoopState ( );

			return TestDone (
				WizardWrx.MagicNumbers.ERROR_SUCCESS ,
				pintTestNumber );
		}   // internal static int UtilsExercises
        #endregion  // Public Methods


		#region Private Enumerations and Instance Storage
		private const string ARGNAME_COND = @"LimitCondition";

		private enum LimitCondition
		{
			EqualTo ,
			GreaterThan ,
			GreaterThanOrEqualTo ,
			LessThanOrEqualTo ,
			LessThan
		}	// LimitCondition
		
		private enum LimitValue
		{
			Lower ,
			Upper
		}	// LimitValue

		static readonly LimitCondition [ ] s_aenmLimitConditions =
		{
			LimitCondition.LessThanOrEqualTo ,
			LimitCondition.LessThan ,
			LimitCondition.GreaterThan ,
			LimitCondition.GreaterThanOrEqualTo ,
			LimitCondition.EqualTo
		};	// static const LimitCondition [ ] s_aenmLimitConditions

		static int s_intStopWhenEquals;
		static bool s_fEqualsFound = false;

		static readonly string [ ] s_astrLimitConditionMessages =
		{
			Properties.Resources.MSG_LOOP_STATE_EQ ,
			Properties.Resources.MSG_LOOP_STATE_GT ,
			Properties.Resources.MSG_LOOP_STATE_GE ,
			Properties.Resources.MSG_LOOP_STATE_LE ,
			Properties.Resources.MSG_LOOP_STATE_LT ,
		};	// static const string [ ] s_astrLimitConditionMessages

		//  --------------------------------------------------------------------
		//  Once populated by the static constructor, the BeginTest method uses
		//  this dictionary to look up the name of the class processed by the
		//  calling method.
		//  --------------------------------------------------------------------

		static Dictionary<string , string> s_dctClassTestMap;
		#endregion	// Private Enumerations and Instance Storage


		#region Private Methods
		private static bool AreWeDoneYet (
			LimitCondition penmLimitCondition ,
			int pintCurrent ,
			int pintLoopLimit )
		{
			switch ( penmLimitCondition )
			{
				case LimitCondition.EqualTo:
					if ( pintCurrent == pintLoopLimit )
					{
						if ( s_fEqualsFound )
						{	// When the last iteration found the match, we are done.
							s_fEqualsFound = false;		// Clear the flag, since it's static, and, threfore, survives the loop.
							return false;
						}	// TRUE block, if ( s_fEqualsFound )
						else
						{	// Since the condition test is at the top of a FOR loop, the code inside the block hasn't yet executed.
							s_fEqualsFound = true;
							return true;
						}	// FALSE block, if ( s_fEqualsFound )
					}	// TRUE (Match found) block, if ( pintCurrent == pintLoopLimit )
					else
					{
						return true;
					}	// FALSE (no match yet) block, if ( pintCurrent == pintLoopLimit )
				case LimitCondition.GreaterThan:
					return pintCurrent > pintLoopLimit;
				case LimitCondition.GreaterThanOrEqualTo:
					return pintCurrent >= pintLoopLimit;
				case LimitCondition.LessThan:
					return pintCurrent < pintLoopLimit;
				case LimitCondition.LessThanOrEqualTo:
					return pintCurrent <= pintLoopLimit;
				default:
					throw new ArgumentOutOfRangeException (
						ARGNAME_COND ,
						penmLimitCondition ,
						Properties.Resources.ERRMSG_LIMIT_CONDITON );
			}	// switch ( penmLimitCondition )
		}	// private static bool AreeWeDoneYet


		static void BeginTest (
            string pstrMethodName ,
            ref int pintTestNumber )
        {
            const string ERRMSG_UNDEFINED_METHOD = @"Method {0} is undefined.";
            const string MSG_BEGIN = @"{2}Test # {0} - Exercising class {1}:{2}";

            if ( s_dctClassTestMap.ContainsKey ( pstrMethodName ) )
            {
                Console.WriteLine (
                    MSG_BEGIN ,                             // Message Template
                    ++pintTestNumber ,                      // Format Item 0 = Test Number - Increment, then print
                    s_dctClassTestMap [ pstrMethodName ] ,  // Format Item 1 = Method Name per System.Reflection
                    Environment.NewLine );                  // Format Item 2 = Newline
            }   // TRUE (expected outcome) block, if ( s_dctClassTestMap.ContainsKey ( pstrMethodName ) )
            else
            {
                throw new Exception ( string.Format (
                    ERRMSG_UNDEFINED_METHOD ,
                    pstrMethodName ) );
            }   // FALSE (UNexpected outcome) block, if ( s_dctClassTestMap.ContainsKey ( pstrMethodName ) )
        }   // static void BeginTest


		private static void EvaluateLoopState ( )
		{
			const int LOOP_START_1_OF_3	= 1;
			const int LOOP_START_2_OF_3 = 0;
			const int LOOP_START_3_OF_3 = -10;

			const int LOOP_STOP_1_OF_3	= 10;
			const int LOOP_STOP_2_OF_3	= 12;
			const int LOOP_STOP_3_OF_3	= 5;

			int intNLimitConditions = s_aenmLimitConditions.Length;
			int intLimitOrdinal = MagicNumbers.ZERO;

			foreach ( LimitCondition lc in s_aenmLimitConditions )
			{
				intLimitOrdinal++;
				ExerciseLoopStateEvaluators ( lc , SetStartStop ( lc , LimitValue.Lower , LOOP_START_1_OF_3 , LOOP_STOP_1_OF_3 ) , SetStartStop ( lc , LimitValue.Upper , LOOP_START_1_OF_3 , LOOP_STOP_1_OF_3 ) , true );
				ExerciseLoopStateEvaluators ( lc , SetStartStop ( lc , LimitValue.Lower , LOOP_START_2_OF_3 , LOOP_STOP_2_OF_3 ) , SetStartStop ( lc , LimitValue.Upper , LOOP_START_2_OF_3 , LOOP_STOP_2_OF_3 ) , true );
				ExerciseLoopStateEvaluators ( lc , SetStartStop ( lc , LimitValue.Lower , LOOP_START_3_OF_3 , LOOP_STOP_3_OF_3 ) , SetStartStop ( lc , LimitValue.Upper , LOOP_START_3_OF_3 , LOOP_STOP_3_OF_3 ) , intLimitOrdinal != intNLimitConditions );
			}	// foreach ( LimitCondition lc in s_aenmLimitConditions )
		}	// static void EvaluateLoopState


		private static void ExerciseLoopStateEvaluators (
			LimitCondition penmLimitCondition ,
			int pintLoopStart ,
			int pintLoopLimit ,
			bool pfPauseForPictures )
		{
			const string COLUMN_HEADER = @"-----";

			Console.WriteLine (
				s_astrLimitConditionMessages [ ( int ) penmLimitCondition ] ,
				Environment.NewLine );
			Console.WriteLine (
				Properties.Resources.MSG_LOOP_LIMIT_VALUES ,
				pintLoopStart ,
				pintLoopLimit , Environment.NewLine );

#if STRINGS_TO_CONSOLE
			string strDummy = string.Format (
				Properties.Resources.MSG_LOOP_STATE_TABLE_LABELS ,
				SpecialCharacters.TAB );
			Console.WriteLine ( strDummy ) ;
#else
			Console.WriteLine (
				Properties.Resources.MSG_LOOP_STATE_TABLE_LABELS ,
				SpecialCharacters.TAB_CHAR );
#endif
			Console.WriteLine (
				Properties.Resources.MSG_LOOP_STATE_TABLE_DATA ,										// Format control string
				new string [ ]																			// Array of format items (>3 items)
					{
						COLUMN_HEADER ,																	// Format Item 0 = Index
						COLUMN_HEADER ,																	// Format Item 1 = First
						COLUMN_HEADER ,																	// Format Item 2 = Next
						COLUMN_HEADER ,																	// Format Item 3 = More
						COLUMN_HEADER ,																	// Format Item 4 = Last
						SpecialCharacters.TAB_CHAR.ToString ( )												// Format Item 5 = TAB character
					} );

			for ( int intCurrent = pintLoopStart ;
					  AreWeDoneYet ( penmLimitCondition , intCurrent , pintLoopLimit ) ;
					  NextIteration ( penmLimitCondition , intCurrent , out intCurrent ) )
			{
				Console.WriteLine (
					Properties.Resources.MSG_LOOP_STATE_TABLE_DATA ,									// Format control string
					new string [ ]																		// Array of format items (>3 items)
					{
						intCurrent.ToString ( ) ,														// Format Item 0 = Index
						Logic.IsFirstForIteration ( intCurrent , pintLoopStart ) .ToString ( ) ,		// Format Item 1 = First
						Logic.IsNextForIteration ( intCurrent , pintLoopStart ).ToString ( ) ,			// Format Item 2 = Next
						MoreIterations ( penmLimitCondition , intCurrent , pintLoopLimit ) ,			// Format Item 3 = More
						LastIteration  ( penmLimitCondition , intCurrent , pintLoopLimit ) ,			// Format Item 4 = Last
						SpecialCharacters.TAB_CHAR.ToString ( )												// Format Item 5 = TAB character
					} );
			}	// for ( int intCurrent = pintLoopStart ; AreeWeDoneYet ( penmLimitCondition , intCurrent , pintLoopLimit ) ; NextIteration ( penmLimitCondition , intCurrent , out intCurrent ) )

			if ( pfPauseForPictures )
			{	// Stop all but the last time through.
				Program.PauseForPictures ( Program.OMIT_LINEFEED );
			}	// if ( pfPauseForPictures )
		}	// private static void ExerciseLoopStateEvaluators


		private static string LastIteration (
			LimitCondition penmLimitCondition ,
			int pintCurrent ,
			int pintLoopLimit )
		{
			switch ( penmLimitCondition )
			{
				case LimitCondition.LessThanOrEqualTo:
					return Logic.IsLastForIterationLE ( pintCurrent , pintLoopLimit ).ToString ( );
				case LimitCondition.LessThan:
					return Logic.IsLastForIterationLT ( pintCurrent , pintLoopLimit ).ToString ( );
				case LimitCondition.GreaterThanOrEqualTo:
					return Logic.IsLastForIterationGE ( pintCurrent , pintLoopLimit ).ToString ( );
				case LimitCondition.GreaterThan:
					return Logic.IsLastForIterationGT ( pintCurrent , pintLoopLimit ).ToString ( );
				case LimitCondition.EqualTo:
					return Logic.IsLastForIterationEQ ( pintCurrent , pintLoopLimit ).ToString ( );
				default:
					return string.Empty;
			}	// switch ( penmLimitCondition )
		}	// private static string LastIteration


		private static string MoreIterations (
			LimitCondition penmLimitCondition ,
			int pintCurrent ,
			int pintLoopLimit )
		{
			switch ( penmLimitCondition )
			{
				case LimitCondition.LessThanOrEqualTo:
					return Logic.MoreForIterationsToComeLE ( pintCurrent , pintLoopLimit ).ToString ( );
				case LimitCondition.LessThan:
					return Logic.MoreForIterationsToComeLT ( pintCurrent , pintLoopLimit ).ToString ( );
				case LimitCondition.GreaterThanOrEqualTo:
					return Logic.MoreForIterationsToComeGE ( pintCurrent , pintLoopLimit ).ToString ( );
				case LimitCondition.GreaterThan:
					return Logic.MoreForIterationsToComeGT ( pintCurrent , pintLoopLimit ).ToString ( );
				case LimitCondition.EqualTo:
					return Logic.MoreForIterationsToComeEQ ( pintCurrent , pintLoopLimit ).ToString ( );
				default:
					return string.Empty;
			}	// switch ( penmLimitCondition )
		}	// private static string MoreIterations


		private static void NextIteration (
			LimitCondition penmLimitCondition ,
			int pintOldCurrent ,
		    out	int pintNewCurrent )
		{
			switch ( penmLimitCondition )
			{
				case LimitCondition.LessThanOrEqualTo:
				case LimitCondition.LessThan:
					pintNewCurrent = pintOldCurrent + 1;
					break;
				case LimitCondition.GreaterThanOrEqualTo:
				case LimitCondition.GreaterThan:
					pintNewCurrent = pintOldCurrent - 1;
					break;
				case LimitCondition.EqualTo:
					pintNewCurrent = s_intStopWhenEquals;
					break;
				default:
					throw new ArgumentOutOfRangeException (
						ARGNAME_COND ,
						penmLimitCondition ,
						Properties.Resources.ERRMSG_LIMIT_CONDITON );
			}	// switch ( penmLimitCondition )
		}	// private static object NextIteration


		private static int SetStartStop (
			LimitCondition penmLimitCondition ,
			LimitValue penmLimitValue ,
			int pintLowerInteger ,
			int pintUpperInteger )
		{
			const string ARGNAME_COND = @"LimitCondition";
			const string ARGNAME_LIMIT = @"penmLimitValue";

			switch ( penmLimitCondition )
			{
				case LimitCondition.LessThanOrEqualTo:
				case LimitCondition.LessThan:
					switch ( penmLimitValue )
					{
						case LimitValue.Lower:
							return pintLowerInteger;
						case LimitValue.Upper:
							return pintUpperInteger;
						default:
							throw new ArgumentOutOfRangeException (
								ARGNAME_LIMIT ,
								penmLimitValue ,
								Properties.Resources.ERRMSG_LIMIT_VALUE );
					}	// switch ( penmLimitValue)
				case LimitCondition.GreaterThanOrEqualTo:
				case LimitCondition.GreaterThan:
					switch ( penmLimitValue )
					{
						case LimitValue.Lower:
							return pintUpperInteger;
						case LimitValue.Upper:
							return pintLowerInteger;
						default:
							throw new ArgumentOutOfRangeException (
								ARGNAME_LIMIT ,
								penmLimitValue ,
								Properties.Resources.ERRMSG_LIMIT_VALUE );
					}	// switch ( penmLimitValue)
				case LimitCondition.EqualTo:
					switch ( penmLimitValue )
					{
						case LimitValue.Lower:
							return pintLowerInteger;
						case LimitValue.Upper:
							s_intStopWhenEquals = pintUpperInteger;
							return s_intStopWhenEquals;
						default:
							throw new ArgumentOutOfRangeException (
								ARGNAME_LIMIT ,
								penmLimitValue ,
								Properties.Resources.ERRMSG_LIMIT_VALUE );
					}	// switch ( penmLimitValue)
				default:
					throw new ArgumentOutOfRangeException (
						ARGNAME_COND ,
						penmLimitCondition ,
						Properties.Resources.ERRMSG_LIMIT_CONDITON );
			}	// switch ( penmLimitCondition )
		}	// private static int SetStartStop


        static int TestDone (
            int pintFinalStatusCode ,
            int pintTestNumber )
        {
            const string MSG_DONE = @"{2}Test # {0} Done - Final Status Code = {1}";

            Console.WriteLine ( 
                MSG_DONE ,                  // Message Template
                pintTestNumber ,            // Format Item 0 = Test Number
                pintFinalStatusCode ,       // Format Item 1 = Final Status Code
                Environment.NewLine );      // Format Item 2 = Newline
            return pintFinalStatusCode;
        }   // private static int TestDone


		private static void UnlessWhat ( )
		{
			const bool UNLESS_CASE_T = true;
			const bool UNLESS_CASE_F = false;

			Console.WriteLine (
				Properties.Resources.MSG_UNLESS_BEGIN ,
				Environment.NewLine );

			Console.WriteLine (
				Properties.Resources.MSG_UNLESS_WHAT ,
				UNLESS_CASE_T ,
				Logic.Unless ( UNLESS_CASE_T ) );
			Console.WriteLine (
				Properties.Resources.MSG_UNLESS_WHAT ,
				UNLESS_CASE_F ,
				Logic.Unless ( UNLESS_CASE_F ) );

			Console.WriteLine (
				Properties.Resources.MSG_UNLESS_END ,
				Environment.NewLine );
		}	// private static void UnlessWhat


		static NewClassTests_20140914 ( )
        {
            //  ----------------------------------------------------------------
            //  The first executable statement in this routine exercises the
            //  whole LoadTextFileFromCallingAssembly chain.
            //  ----------------------------------------------------------------

            const string CLASS_MAP_TABLE_NAME = @"ClassTestMap.TXT";
            const string ERRMSG_MISSING_CLASS_MAP_TABLE = @"Class map table resource {0} is invalid.";
            const string ERRMSG_INVALID_CLASS_MAP_LABELS = @"Class map table label row is invalid.{2}Labels found = {0}{2}Labels expected = {1}";
            const string ERRMSG_INVALID_CLASS_MAP_RECORD = @"Class map {0} record {1} field count is invalid.{4}Field Count found = {2}{4}Expected field count = {3}";
            const string ERRMSG_DUPLICATE_CLASS_MAP_KEY = @"Class map {0} record {1} method name field is a duplicate.{4}Method Name = {2}{4}Class Name = {3}";

            const int FIELD_ROUTINE_NAME = ArrayInfo.ARRAY_FIRST_ELEMENT;
            const int FIELD_CLASS_NAME = FIELD_ROUTINE_NAME + ArrayInfo.NEXT_INDEX;
            const int FIELD_EXPECTED_COUNT = FIELD_CLASS_NAME + ArrayInfo.ORDINAL_FROM_INDEX;

            const string VALID_LABEL_ROW = @"Method Name	Class Tested";

            string [ ] astrClassMap = Util.LoadTextFileFromCallingAssembly ( CLASS_MAP_TABLE_NAME );

            int intNRecs = CSVFileInfo.RecordCount ( astrClassMap );

            if ( intNRecs > CSVFileInfo.EMPTY_FILE )
            {   // There is a label row and at least one data row.
                if ( astrClassMap [ CSVFileInfo.LABEL_ROW ] == VALID_LABEL_ROW )
                {   // The label row matches the design specification.
                    s_dctClassTestMap = new Dictionary<string , string> ( intNRecs );

                    for ( int intIndex = CSVFileInfo.FIRST_RECORD ;
                              intIndex <= intNRecs ;
                              intIndex++ )
                    {   // Process each record.
                        string [ ] astrFields = astrClassMap [ intIndex ].Split ( SpecialCharacters.TAB_CHAR );

                        if ( astrFields.Length == FIELD_EXPECTED_COUNT )
                        {   // Field count matches the design.
                            if ( s_dctClassTestMap.ContainsKey ( astrFields [ FIELD_ROUTINE_NAME ] ) )
                            {   // Croak on duplicate record.
                                throw new Exception ( string.Format (
                                    ERRMSG_DUPLICATE_CLASS_MAP_KEY ,   // Message template                    
                                    new object [ ]
                                {
                                    CLASS_MAP_TABLE_NAME ,              // Format Item 0 = Table File Name (per constant)
                                    intIndex ,                          // Format Item 1 = Record number
                                    astrFields [ FIELD_ROUTINE_NAME ] , // Format Item 2 = Reported routine name
                                    astrFields [ FIELD_CLASS_NAME ] ,   // Format Item 3 = Expected class name
                                    Environment.NewLine                 // Format Item 4 = Newline
                                } ) );
                            }   // TRUE (UNexpected outcome) block, if ( s_dctClassTestMap.ContainsKey ( astrFields [ FIELD_ROUTINE_NAME ] ) )
                            else
                            {   // Record is unique; add it to the list.
                                s_dctClassTestMap.Add (
                                    astrFields [ FIELD_ROUTINE_NAME ] ,
                                    astrFields [ FIELD_CLASS_NAME ] );
                            }   // FALSE (expected outcome) block, if ( s_dctClassTestMap.ContainsKey ( astrFields [ FIELD_ROUTINE_NAME ] ) )
                        }   // TRUE (expected outcome) block, if ( astrFields.Length == FIELD_EXPECTED_COUNT )
                        else
                        {   // Field count differs from specification.
                            throw new Exception ( string.Format (
                                ERRMSG_INVALID_CLASS_MAP_RECORD ,   // Message template                    
                                new object [ ]
                                {
                                    CLASS_MAP_TABLE_NAME ,          // Format Item 0 = Table File Name (per constant)
                                    intIndex ,                      // Format Item 1 = Record number
                                    astrFields.Length ,             // Format Item 2 = Reported field count
                                    FIELD_EXPECTED_COUNT ,          // Format Item 3 = Expected field count
                                    Environment.NewLine             // Format Item 4 = Newline
                                } ) );
                        }   // FALSE (UNexpected outcome) block, if ( astrFields.Length == FIELD_EXPECTED_COUNT )
                    }   // for ( int intIndex = CSVFileInfo.FIRST_RECORD ; intIndex <= intNRecs ; intIndex++ )
                }   // TRUE (expected outcome) block, if ( astrClassMap [ CSVFileInfo.LABEL_ROW ] == VALID_LABEL_ROW )
                else
                {   // Label row is invalid. File in file system and constants in this routine differ.
                    throw new Exception ( string.Format (
                        ERRMSG_INVALID_CLASS_MAP_LABELS ,
                        astrClassMap [ CSVFileInfo.LABEL_ROW ] ,
                        VALID_LABEL_ROW ,
                        Environment.NewLine ) );
                }   // FALSE (UNexpected outcome) block, if ( astrClassMap [ CSVFileInfo.LABEL_ROW ] == VALID_LABEL_ROW )
            }   // TRUE (expected outcome) block, if ( intNRecs > CSVFileInfo.EMPTY_FILE )
            else
            {   // The required resource is either absent or empty.
                throw new Exception ( string.Format (
                    ERRMSG_MISSING_CLASS_MAP_TABLE ,
                    CLASS_MAP_TABLE_NAME ) );
            }   // FALSE (UNexpected outcome) block, if ( intNRecs > CSVFileInfo.EMPTY_FILE )
        }   // static NewClassTests_20140914 ( )
        #endregion  // Private Methods
    }   // internal static class NewClassTests_20140914
}   // partial namespace DLLServices2TestStand