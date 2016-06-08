/*
    ============================================================================

    Namespace Name:     WizardWrx.DLLServices2

    Class Name:         SysDateFormatters

    File Name:          SysDateFormatters.cs

    Synopsis:           This class implements my stalwart ReformatSysDate_P6C
                        date formatting algorithm as 100% managed code.

    Remarks:            This class is implicitly sealed. Instances of it cannot
                        be created, and the class cannot be inherited.

                        If you insist on saving your format strings, store them
                        in its sibling class, TimeDisplayFormatter, and call 
                        these methods through it.

    References:         1)  DayOfWeek Enumeration
                            http://msdn.microsoft.com/en-us/library/system.dayofweek.aspx

                        2)  DateTimeFormatInfo.AbbreviatedDayNames Property 
                            http://msdn.microsoft.com/en-us/library/system.globalization.datetimeformatinfo.abbreviateddaynames.aspx

                        3)  DateTimeFormatInfo Class
                            http://msdn.microsoft.com/en-us/library/system.globalization.datetimeformatinfo.aspx

    Author:             David A. Gray

    License:            Copyright (C) 2012-2016, David A. Gray. 
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
    2012/02/26 1.7     DAG    This class makes its first appearance.

    2012/03/03 1.8     DAG    Comment the public constants.

    2012/09/03 2.6     DAG    Complete the constellation by adding ReformatNow
                              and ReformatUtcNow.

    2013/02/17 2.96    DAG    Add RFD_MM_DD_YY, which defines the archaic short
                              (eight character) date format. The archaic format
                              is required by ZZRdInts, and probably others,
                              sooner or later.

                              For unrelated reasons, this version of the 
                              assembly gets a strong name.
 
    2013/11/24 3.1     DAG    1) Add the following overlooked, but often required
                                 compact date formats.

                                    ----------------------------------
                                    Mask            Symbol
                                    --------------- ------------------
                                    YYYYMMDD_hhmmss RFDYYYYMMDD_HHMMSS
                                    YYYYMMDD        RFDYYYYMMDD
                                    HHMMSS          RFDHHMMSS
                                    ----------------------------------

                              2) Correct a couple of typographical errors in the
                                 internal documentation that I discovered while
                                 scanning for the above-cited formatting masks.

                              The executable code is unaffected.

    2014/06/07 5.0     DAG    Major namespace reorganization.

    2014/06/23 5.1     DAG    Documentation corrections.

    2014/09/14 5.2     DAG    Copy into to WizardWrx.DLLServices2 class
                              library, which is built against the Microsoft .NET
                              Framework, version 3.5, to take advantage of its
                              new TimeZoneInfo class, required by the new Util
                              class, which brings together assorted constants
                              and methods developed for various applications.

	2015/06/06 5.4     DAG    Break completely free from WizardWrx.SharedUtl2.

	2015/06/20 5.5     DAG    Relocate to WizardWrx.DLLServices2 namespace and
                              class library.
 
    2016/04/10 6.0     DAG    Scan for typographical errors flagged by the
							  spelling checker add-in, and correct what I find,
                              and update the formatting and marking of blocks.
    ============================================================================
*/


using System;
using System.Collections.Generic;
using System.Text;

/*  Added by DAG */

using System.Globalization;
using WizardWrx;


namespace WizardWrx.DLLServices2
{
    /// <summary>
    /// This class implements my stalwart date formatter, ReformatSysDateP6C,
    /// which I created initially as a Windows Interface Language (WIL, 
    /// a. k. a. WinBatch) library function, Reformat_Date_YmdHms_P6C, in
    /// October 2001, although its roots go back much further in my WIL script
    /// development.
	/// 
	/// Since static classes are implicitly sealed, this class cannot be inherited.
	/// </summary>
	/// <seealso cref="DisplayFormats"/>
	/// <seealso cref="TimeDisplayFormatter"/>
    public static class SysDateFormatters
    {
        #region Public constants define the substitution tokens and a selection of useful format strings.
        /// <summary>
        /// The strings in this array are the substitution tokens supported by
        /// the date formatters in this class.
        /// </summary>
        public static readonly string [ ] RSD_TOKENS =
        {
            @"^MMMM" ,
            @"^MMM" ,
            @"^MM" , 
            @"^DD" ,
            @"^D" ,
            @"^YYYY" ,
            @"^YY" ,
            @"^hh" ,
            @"^h" ,
            @"^mm" ,
            @"^ss" ,
            @"^WWWW" ,
            @"^WWW" ,
            @"^WW" ,
            @"^ttt"
        };  // public static readonly string [ ] RSD_TOKENS


        /// <summary>
        /// Apply the following format to a date: YYYY/MM/DD
        /// 
        /// With respect to the date only, this format confirms to the ISO 8601
        /// standard for time representation.
        /// 
        /// Only the date is returned, all four digits of the year are included,
        /// and the month and day have leading zeros if either is less than 10.
        /// </summary>
        public const string RFD_YYYY_MM_DD = @"^YYYY/^MM/^DD";


        /// <summary>
        /// Apply the following format to a date: MM/DD/YY
        /// 
        /// This is the standard short format used in the USA.
        /// 
        /// Only the date is returned, including only the year of century, and
        /// the month and day have leading zeros if either is less than 10.
        /// </summary>
        public const string RFD_MM_DD_YY = @"^MM/^DD/^YY";


        /// <summary>
        /// Apply the following format to a date: MM/DD/YYYY
        /// 
        /// This is the standard format used in the USA.
        /// 
        /// Only the date is returned, all four digits of the year are included,
        /// and the month and day have leading zeros if either is less than 10.
        /// </summary>
        public const string RFD_MM_DD_YYYY = @"^MM/^DD/^YYYY";


        /// <summary>
        /// Apply the following format to a date: DD/MM/YYYY
        /// 
        /// This is the standard format used in most of the English speaking
        /// world, by all military organizations of which I am aware, Europeans,
        /// and others who take their lead from any of the above groups.
        /// 
        /// Only the date is returned, all four digits of the year are included,
        /// and the month and day have leading zeros if either is less than 10.
        /// </summary>
        public const string RFD_DD_MM_YYYY = @"^DD/^MM/^YYYY";


        /// <summary>
        /// Apply the following format to a time: hh:mm
        /// 
        /// The returned string represents the hours on a 24 hour clock.
        /// 
        /// At present, 12 hour (AM/PM) representation is unsupported.
        /// 
        /// This is a standard format used in most of the English speaking
        /// world, by all military organizations of which I am aware, Europeans,
        /// and others who take their lead from any of the above groups.
        /// 
        /// Only the time is returned, and the hour and minute have leading
        /// zeros if either is less than 10.
        /// </summary>
        public const string RFD_HH_MM = @"^hh:^mm";


        /// <summary>
        /// Apply the following format to a time: hh:mm:ss
        /// 
        /// The returned string represents the hours on a 24 hour clock.
        /// 
        /// At present, 12 hour (AM/PM) representation is unsupported.
        /// 
        /// This is a standard format used in most of the English speaking
        /// world, by all military organizations of which I am aware, Europeans,
        /// and others who take their lead from any of the above groups.
        /// 
        /// Only the time is returned, and the hour, minute, and second have 
        /// leading zeros if any of them is less than 10.
        /// </summary>
        public const string RFD_HH_MM_SS = @"^hh:^mm:^ss";


        /// <summary>
        /// Apply the following format to a time: hh:mm:ss.ttt
        /// 
        /// The returned string represents the hours on a 24 hour clock.
        /// 
        /// At present, 12 hour (AM/PM) representation is unsupported.
        /// 
        /// The final token, ttt, is the milliseconds portion of the time,
        /// which is reported with leading zeros.
        /// 
        /// This is an extension of a standard format used in most of the
        /// English speaking world, by all military organizations of which I am
        /// aware, Europeans, and others who take their lead from any of the
        /// above groups.
        /// 
        /// Only the time is returned, and the hour, minute, and second have 
        /// leading zeros if any of them is less than 10.
        /// </summary>
        public const string RFD_HH_MM_SS_TTT = @"^hh:^mm:^ss.^ttt";


        /// <summary>
        /// Apply the following format to a date and time: YYYY/MM/DD hh:mm:ss
        /// 
        /// The returned string represents the hours on a 24 hour clock.
        /// 
        /// At present, 12 hour (AM/PM) representation is unsupported.
        /// 
        /// This format conforms fully to the ISO 8601 standard for time
        /// representation.
        /// 
        /// The month, day, hour, minute, and second have leading zeros if any
        /// of them is less than 10.
        /// </summary>
        public const string RFD_YYYY_MM_DD_HH_MM_SS = @"^YYYY/^MM/^DD ^hh:^mm:^ss";


        /// <summary>
        /// Apply the following format to a date and time: YYYY/MM/DD hh:mm:ss.ttt
        /// 
        /// The returned string represents the hours on a 24 hour clock.
        /// 
        /// At present, 12 hour (AM/PM) representation is unsupported.
        /// 
        /// This format conforms fully to the ISO 8601 standard for time
        /// representation.
        /// 
        /// The final token, ttt, is the milliseconds portion of the time,
        /// which is reported with leading zeros.
        /// 
        /// The month, day, hour, minute, and second have leading zeros if any
        /// of them is less than 10.
        /// </summary>
        public const string RFD_YYYY_MM_DD_HH_MM_SS_TTT = @"^YYYY/^MM/^DD ^hh:^mm:^ss.^ttt";


        /// <summary>
        /// Apply the following format to a date: WWW DD/MM/YYYY
        /// 
        /// The first token, WWW, represents the three letter abbreviation of
        /// the weekday name, which is derived from the regional settings in the
        /// Windows Control Panel. The returned string conforms to the settings
        /// in the UICulture of the calling thread.
        /// 
        /// This is the standard format used in most of the English speaking
        /// world, by all military organizations of which I am aware, Europeans,
        /// and others who take their lead from any of the above groups.
        /// 
        /// Only the date is returned, all four digits of the year are included,
        /// and the month and day have leading zeros if either is less than 10.
        /// </summary>
        public const string RFD_WWW_DD_MM_YYYY = @"^WWW ^DD/^MM/^YYYY";

        
        /// <summary>
        /// Apply the following format to a date: WWW DD/MM/YYYY
        /// 
        /// The first token, WWW, represents the three letter abbreviation of
        /// the weekday name, which is derived from the regional settings in the
        /// Windows Control Panel. The returned string conforms to the settings
        /// in the UICulture of the calling thread.
        /// 
        /// This is the standard format used in the USA.
        /// 
        /// Only the date is returned, all four digits of the year are included,
        /// and the month and day have leading zeros if either is less than 10.
        /// </summary>
        public const string RFD_WWW_MM_DD_YYYY = @"^WWW ^MM/^DD/^YYYY";

        /// <summary>
        /// Apply the following format to a date: WW DD/MM/YYYY
        /// 
        /// The first token, WW, represents enough of the three letter weekday
        /// name abbreviation, which is derived from the regional settings in
        /// the Windows Control Panel, to uniquely identify the weekday. The
        /// returned string conforms to the settings in the UICulture of the
        /// calling thread.
        /// 
        /// This is the standard format used in most of the English speaking
        /// world, by all military organizations of which I am aware, Europeans,
        /// and others who take their lead from any of the above groups.
        /// 
        /// Only the date is returned, all four digits of the year are included,
        /// and the month and day have leading zeros if either is less than 10.
        /// </summary>
        public const string RFD_WW_DD_MM_YYYY = @"^WW ^DD/^MM/^YYYY";


        /// <summary>
        /// Apply the following format to a date: WW DD/MM/YYYY
        /// 
        /// The first token, WW, represents enough of the three letter weekday
        /// name abbreviation, which is derived from the regional settings in
        /// the Windows Control Panel, to uniquely identify the weekday. The
        /// returned string conforms to the settings in the UICulture of the
        /// calling thread.
        /// 
        /// This is the standard format used in the USA.
        /// 
        /// Only the date is returned, all four digits of the year are included,
        /// and the month and day have leading zeros if either is less than 10.
        /// </summary>
        public const string RFD_WW_MM_DD_YYYY = @"^WW ^MM/^DD/^YYYY";


        /// <summary>
        /// Apply the following format to a date: WWWW DD/MM/YYYY
        /// 
        /// The first token, WWWW, represents full name of the weekday, which is
        /// derived from the regional settings in the Windows Control Panel. The
        /// returned string conforms to the settings in the UICulture of the
        /// calling thread.
        /// 
        /// This is the standard format used in most of the English speaking
        /// world, by all military organizations of which I am aware, Europeans,
        /// and others who take their lead from any of the above groups.
        /// 
        /// Only the date is returned, all four digits of the year are included,
        /// and the month and day have leading zeros if either is less than 10.
        /// </summary>
        public const string RFD_WWWW_DD_MM_YYYY = @"^WWWW, ^DD/^MM/^YYYY";


        /// <summary>
        /// Apply the following format to a date: WWWW DD/MM/YYYY
        /// 
        /// The first token, WWWW, represents full name of the weekday, which is
        /// derived from the regional settings in the Windows Control Panel. The
        /// returned string conforms to the settings in the UICulture of the
        /// calling thread.
        /// 
        /// This is the standard format used in the USA.
        /// 
        /// Only the date is returned, all four digits of the year are included,
        /// and the month and day have leading zeros if either is less than 10.
        /// </summary>
        public const string RFD_WWWW_MM_DD_YYYY = @"^WWWW, ^MM/^DD/^YYYY";


        /// <summary>
        /// Apply the following format to a date and time: YYYYMMDD_hhmmss
        /// 
        /// The returned string represents the hours on a 24 hour clock.
        /// 
        /// At present, 12 hour (AM/PM) representation is unsupported.
        /// 
        /// This format conforms fully to the ISO 8601 standard for time
        /// representation.
        /// 
        /// The month, day, hour, minute, and second have leading zeros if any
        /// of them is less than 10.
        /// </summary>
        public const string RFDYYYYMMDD_HHMMSS = @"^YYYY^MM^DD_^hh^mm^ss";


        /// <summary>
        /// Apply the following format to a date and time: YYYYMMDD
        /// 
        /// The returned string represents the hours on a 24 hour clock.
        /// 
        /// At present, 12 hour (AM/PM) representation is unsupported.
        /// 
        /// This format conforms fully to the ISO 8601 standard for time
        /// representation.
        /// 
        /// The month and day have leading zeros if either is less than 10.
        /// </summary>
        public const string RFDYYYYMMDD = @"^YYYY^MM^DD";


        /// <summary>
        /// Apply the following format to a date and time: hhmmss
        /// 
        /// The returned string represents the hours on a 24 hour clock.
        /// 
        /// At present, 12 hour (AM/PM) representation is unsupported.
        /// 
        /// This format conforms fully to the ISO 8601 standard for time
        /// representation.
        /// 
        /// The hour, minute, and second have leading zeros if any of them is
        /// less than 10.
        /// </summary>
        public const string RFDHHMMSS = @"^hh^mm^ss";
		#endregion	// Public constants define the substitution tokens and a selection of useful format strings.


		#region Although static, this class requires several tables that are initialized at compile time.
		enum FormattingAlgorithm
        {
            ApplyFormatToDateTime ,
            TwoLetterWeekday
        }   // FormattingAlgorithm

        static readonly FormattingAlgorithm [ ] RSD_ALGORITHM =
        {
            FormattingAlgorithm.ApplyFormatToDateTime ,
            FormattingAlgorithm.ApplyFormatToDateTime ,
            FormattingAlgorithm.ApplyFormatToDateTime ,
            FormattingAlgorithm.ApplyFormatToDateTime ,
            FormattingAlgorithm.ApplyFormatToDateTime ,
            FormattingAlgorithm.ApplyFormatToDateTime ,
            FormattingAlgorithm.ApplyFormatToDateTime ,
            FormattingAlgorithm.ApplyFormatToDateTime ,
            FormattingAlgorithm.ApplyFormatToDateTime ,
            FormattingAlgorithm.ApplyFormatToDateTime ,
            FormattingAlgorithm.ApplyFormatToDateTime ,
            FormattingAlgorithm.ApplyFormatToDateTime ,
            FormattingAlgorithm.ApplyFormatToDateTime ,
            FormattingAlgorithm.TwoLetterWeekday ,
            FormattingAlgorithm.ApplyFormatToDateTime
        };  // FormattingAlgorithm [ ] RSD_ALGORITHM =


        static readonly string [ ] RSD_BCL_FORMATS =
        {
            @"MMMM" ,
            @"MMM" ,
            @"MM" ,
            @"dd" ,
            @"%d" ,         // See http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx#UsingSingleSpecifiers.
            @"yyyy" ,
            @"yy" ,
            @"HH" ,
            @"%H" ,         // See http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx#UsingSingleSpecifiers.
            @"mm" ,
            @"ss" ,
            @"dddd" ,
            @"ddd" ,
            @"^WW" ,        // There is no custom format string that corresponds to ^WW.
            @"fff"
        };  // string [ ] RSD_BCL_FORMATS
		#endregion	// Although static, this class requires several tables that are initialized at compile time.


		#region In addition to the hard coded tables, this class uses another table, and a scalar which holds the size of the three hard coded tables, which are initialized at run time by the static constructor.
		static readonly string [ ] RSD_WEEKDAY_2LTRS;

        static readonly int _intNDefinedTokens;
		#endregion	// In addition to the hard coded tables, this class uses another table, and a scalar which holds the size of the three hard coded tables, which are initialized at run time by the static constructor.


		#region The static constructor initializes the two-letter weekday table based on the current UI culture.
		static SysDateFormatters ( )
        {
            const int DAYS_IN_WEEK = 7;
            const int ONE_LETTER = ArrayInfo.NEXT_INDEX;
            const int SECOND_LETTER_INDEX = ArrayInfo.NEXT_INDEX;
            
            const string TWO_LETTERS = @"{0}{1}";
            const string UNBALANCED_ERRMSG = @"The static SysDateFormatters constructor has detected unbalanced string arrays.{2}Length of array RSD_TOKENS = {0}{2}Length of array RSD_BCL_FORMATS = {1}";

            //  ----------------------------------------------------------------
            //  Sanity check the sizes of the three arrays that were initialized
            //  at compile time. All three must be the same size.
            //  ----------------------------------------------------------------

            int intRSDTokenArraySize = RSD_TOKENS.Length;
            int intBCLFormatArraySize = RSD_BCL_FORMATS.Length;
            int intAlgorithmArraySize = RSD_ALGORITHM.Length;

            if ( intRSDTokenArraySize == intBCLFormatArraySize )
                if ( intAlgorithmArraySize == intRSDTokenArraySize )
                    _intNDefinedTokens = intRSDTokenArraySize;
                else
                    throw new Exception ( string.Format (
                        UNBALANCED_ERRMSG ,
                        intRSDTokenArraySize ,
                        intBCLFormatArraySize ,
                        Environment.NewLine ) );

            //  ----------------------------------------------------------------
            //  The fourth array, RSD_WEEKDAY_2LTRS, is initialized by this
            //  constructor, because the required values are unknown until this
            //  constructor executes.
            //  ----------------------------------------------------------------

            RSD_WEEKDAY_2LTRS = new string [ DAYS_IN_WEEK ];

            string [ ] astrAbbrWdayNames = CultureInfo.CurrentUICulture.DateTimeFormat.AbbreviatedDayNames;
            char [ ] achrFirstLetter = new char [ DAYS_IN_WEEK ];
            Dictionary<char , int> dctTimesUsed = new Dictionary<char , int> ( DAYS_IN_WEEK );
            int intDayIndex = ArrayInfo.ARRAY_INVALID_INDEX;

            //  ----------------------------------------------------------------
            //  Isolate and count the first letter.
            //  ----------------------------------------------------------------

            foreach ( string strWeekdayName in astrAbbrWdayNames )
            {
                char [ ] achrFirstLtrOfWday = strWeekdayName.ToCharArray (
                    ArrayInfo.ARRAY_FIRST_ELEMENT ,
                    ONE_LETTER );

                achrFirstLetter [ ++intDayIndex ] = achrFirstLtrOfWday [ ArrayInfo.ARRAY_FIRST_ELEMENT ];

                if ( dctTimesUsed.ContainsKey ( achrFirstLetter [ intDayIndex ] ) )
                {   // Letter already used at least once.
                    ++dctTimesUsed [ achrFirstLetter [ intDayIndex ] ];
                }   // TRUE block, if ( dctTimesUsed.ContainsKey ( achrFirstLetter [ intDayIndex ] ) )
                else
                {   // First use of this letter.
                    dctTimesUsed [ achrFirstLetter [ intDayIndex ] ] = ONE_LETTER;
                }   // FALSE block, if ( dctTimesUsed.ContainsKey ( achrFirstLetter [ intDayIndex ] ) )
            }   //  foreach ( string strWeekdayName in astrAbbrWdayNames )

            //  ----------------------------------------------------------------
            //  Assemble the two-letter abbreviations, and load them into the
            //  string array. If one letter is enough to uniquely identify a day
            //  of the week, confine the abbreviation to the initial letter. If
            //  a second letter is required, append the second character of the
            //  weekday name.
            //  ----------------------------------------------------------------

            intDayIndex = ArrayInfo.ARRAY_INVALID_INDEX;    // Reset.

            foreach ( string strWeekdayName in astrAbbrWdayNames )
            {
                char chrFirstLetter = achrFirstLetter [ ++intDayIndex ];

                if ( dctTimesUsed [ chrFirstLetter ] == ONE_LETTER )
                {   // Letter is used once only, so it uniquely identifies the weekday.
                    RSD_WEEKDAY_2LTRS [ intDayIndex ] = chrFirstLetter.ToString ( );
                }   // TRUE block, if ( dctTimesUsed [ chrFirstLetter ] == ONE_LETTER )
                else
                {   // Letter is used more than once. Get the second letter.
                    char [ ] achrSecondLetter = strWeekdayName.ToCharArray (
                        SECOND_LETTER_INDEX ,
                        ONE_LETTER );
                    RSD_WEEKDAY_2LTRS [ intDayIndex ] = string.Format (
                        TWO_LETTERS ,
                        chrFirstLetter ,
                        achrSecondLetter [ ArrayInfo.ARRAY_FIRST_ELEMENT ] );
                }   // FALSE block, if ( dctTimesUsed [ chrFirstLetter ] == ONE_LETTER )
            }   // foreach ( string strWeekdayName in astrAbbrWdayNames )
        }   // static SysDateFormatters constructor
		#endregion	// The static constructor initializes the two-letter weekday table based on the current UI culture.


		#region Public static methods are the reason this class exists.
		/// <summary>
        /// This method has a nearly exact analogue in the constellations of WIL
        /// User Defined Functions that gave rise to its immediate predecessor,
        /// a like named function implemented in straight C, with a little help
        /// from the Windows National Language Subsystem, which underlies the
        /// CultureInfo class.
        /// </summary>
        /// <param name="pstrFormat">
        /// This System.String is a combination of tokens and literal text that
        /// governs the formatting of the date.
        /// </param>
        /// <returns>
        /// The return value is a string containing the current date and time,
		/// formatted according to the rules spelled out in format string
		/// pstrFormat.
        /// </returns>
        public static string ReformatNow ( string pstrFormat )
        { return ReformatSysDate ( DateTime.Now , pstrFormat ); }


        /// <summary>
        /// In the original constellation of WinBatch functions and their C
        /// descendants, this function took the form of an optional argument to
        /// ReformatNow. I think I prefer this way.
        /// </summary>
        /// <param name="pstrFormat">
        /// This System.String is a combination of tokens and literal text that
        /// governs the formatting of the date.
        /// </param>
        /// <returns>
        /// The return value is a string containing the current UTC time,
        /// formatted according to the rules spelled out in format string
		/// pstrFormat.
        /// </returns>
        public static string ReformatUtcNow ( string pstrFormat )
        { return ReformatSysDate ( DateTime.UtcNow , pstrFormat ); }


        /// <summary>
        /// ReformatSysDate is the core function of the constellation of
        /// routines that grew from the original WIL script. Substitution tokens
        /// drive construction of a formatted date string.
        /// </summary>
        /// <param name="pdtm">
        /// This System.DateTime is the time to be formatted.
        /// </param>
        /// <param name="pstrFormat">
        /// This System.String is a combination of tokens and literal text that
        /// governs the formatting of the date.
        /// </param>
        /// <returns>
        /// The return value is a string containing the date and/or time in
        /// argument pdtm, formatted according to the rules spelled out in
        /// format string pstrFormat.
        /// </returns>
        public static string ReformatSysDate (
            DateTime pdtm ,
            string pstrFormat )
        {
            const string ARG_PDTM = @"pdtm";
            const string ARG_PSTRFORMAT = @"pstrFormat";
            const string ERRMSG_INTERNAL_ERROR_UNSUPP_ALG = @"An internal error has been detected in routine ReformatSysDate.{2}An unexpected FormattingAlgorithm of {0} has been encountered while processing the following input string:{2}{1}";

            if ( pdtm == null )
                throw new ArgumentNullException (
                    ARG_PDTM ,
                    Properties.Resources.ERRMSG_ARG_IS_NULL );

            if ( string.IsNullOrEmpty ( pstrFormat ) )
                throw new ArgumentException (
                    Properties.Resources.ERRMSG_ARG_IS_NULL_OR_EMPTY ,
                    ARG_PSTRFORMAT );

            StringBuilder rsbFormattedDate = new StringBuilder (
                pstrFormat ,
                EstimateFinalLength ( ref pstrFormat ) );

            for ( int intCurrToken = ArrayInfo.ARRAY_FIRST_ELEMENT ; intCurrToken < _intNDefinedTokens ; intCurrToken++ )
            {   // Unless a token appears in the format string, everything inside the switch block, and the switch, itself, is wasted effort.
                if ( pstrFormat.IndexOf ( RSD_TOKENS [ intCurrToken ] ) > ArrayInfo.ARRAY_INVALID_INDEX )
                {   // This token is used.
                    switch ( RSD_ALGORITHM [ intCurrToken ] )
                    {
                        case FormattingAlgorithm.ApplyFormatToDateTime:
                            {   // In order to reuse the name, strValue must be confined in a closure.
                                string strValue = pdtm.ToString ( RSD_BCL_FORMATS [ intCurrToken ] );
                                rsbFormattedDate.Replace (
                                    RSD_TOKENS [ intCurrToken ] ,
                                    strValue );
                            }   // The string, strValue goes out of scope.
                            break;

                        case FormattingAlgorithm.TwoLetterWeekday:
                            {   // In order to reuse the name, strValue must be confined in a block.
                                string strValue = RSD_WEEKDAY_2LTRS [ ( int ) pdtm.DayOfWeek ];
                                rsbFormattedDate.Replace (
                                    RSD_TOKENS [ intCurrToken ] ,
                                    strValue );
                            }   // The string, strValue goes out of scope.
                            break;

                        default:
                            string strMsg = string.Format (
                                ERRMSG_INTERNAL_ERROR_UNSUPP_ALG ,          // Format string
                                ( int ) RSD_ALGORITHM [ intCurrToken ] ,    // Token 0
                                pstrFormat ,                                // Token 1
                                Environment.NewLine );                      // Token 2
                            throw new Exception ( strMsg );
                    }   // switch ( RSD_ALGORITHM [ intCurrToken ] )
                }   // if ( pstrFormat.IndexOf ( RSD_TOKENS [ intCurrToken ] ) > ArrayInfo.ARRAY_INVALID_INDEX )
            }   // for (int intCurrToken=ArrayInfo.ARRAY_FIRST_ELEMENT;intCurrToken<_intNDefinedTokens;intCurrToken++)

            return rsbFormattedDate.ToString ( );
        }   // ReformatSysDate
		#endregion	// Public static methods are the reason this class exists.


		#region Private methods simplify the public methods.
		private static int EstimateFinalLength ( ref string pstrFormat )
        {
			return pstrFormat.Length * 2;
        }   // private static int EstimateFinalLength
		#endregion	// Private methods simplify the public methods.
	}   // public class SysDateFormatters
}   // partial namespace WizardWrx.DLLServices2