/*
    ============================================================================

    Namespace:          WizardWrx.DLLServices2

    Class Name:         DisplayFormats

    File Name:          DisplayFormats.cs

    Synopsis:           Pass these constants to the ToString method on an object
                        of the appropriate type to render the object for display
                        or printing.

                        Use these service methods to facilitate using the
                        Date/Time formatting constants, which cannot be fed to
                        ToString, except, perhaps, with a custom formatting
                        engine.

    Remarks:            The comment associated with each constant identifies the
                        types for which it is appropriate.

                        Use these service methods, or call the ReformatSysDate
                        function, which also belongs to this library, directly.

                        The time formatting strings and routines in this class
                        are time zone agnostic. If you want or need the time
                        zone, use the companion method, GetDisplayTimeZone,
                        defined in sibling class Util (This API is available 
						only in libraries that target .NET Framework 3.5 or
						later.

						As of version 6.2, the numeric formatting strings are
						referred to a dedicated sibling class, NumericFormats.
						The date and time formatting strings already refer to
						another dedicated sibling class, SysDateFormatters. This
						change (at version 6.2) means that formatting string
						definition maintenance happens in the dedicated classes.
						However, rather than deprecate these, I am leaving them,
						because they transform this class into a single point of
						access to all of the standard formatting strings defined
						in this library.

    Author:             David A. Gray

    License:            Copyright (C) 2014-2015, David A. Gray. 
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
    2014/09/14 5.2     DAG    Initial implementation.

	2015/06/20 5.5     DAG    Relocate to WizardWrx.DLLServices2 namespace and
                              class library.

	2016/06/04 6.2     DAG    To lighten the maintenance burden, forward the
                              numeric format strings to sibling NumericFormats.
    ============================================================================
*/


using System;

namespace WizardWrx.DLLServices2
{
    /// <summary>
    /// Pass these constants to the ToString method on an object of the
    /// appropriate type to render the object for printing.
    /// 
    /// The comment associated with each constant identifies the types for
    /// which it is appropriate.
    /// 
    /// There are service methods to facilitate using the Date/Time formatting
    /// constants, which cannot be fed to ToString, except, perhaps, with a
    /// custom formatting engine. Use these service methods, or call the
    /// ReformatSysDate function, which also belongs to this library, directly.
    /// 
    /// NOTE: The time formatting strings and routines in this class are time
    /// zone agnostic. If you want or need the time zone, use the companion
    /// method, GetDisplayTimeZone, defined in sibling class Util.
    /// </summary>
	/// <seealso cref="SysDateFormatters"/>
	/// <seealso cref="TimeDisplayFormatter"/>
#if NET35
    /// <see cref="Util.GetDisplayTimeZone"/>
#endif	// #if NET35
	public static class DisplayFormats
    {
        #region Convenience Constants
		/// <summary>
		/// Pass this constant to the ToString method on any integral type to
		/// format it as an arbitrary string of hexadecimal digits, using lower
		/// case glyphs..
		/// </summary>
		public const string HEXADECIMAL_LC = NumericFormats.HEXADECIMAL_LC;

        /// <summary>
		/// Pass this constant to the ToString method on any integral type to
		/// format it as an arbitrary string of hexadecimal digits, using upper
		/// case glyphs..
		/// </summary>
		public const string HEXADECIMAL_UC = NumericFormats.HEXADECIMAL_UC;

        /// <summary>
        /// Pass this constant to the ToString method on any integral type to
        /// format it as a string of 2 hexadecimal digits.
        /// </summary>
		public const string HEXADECIMAL_2 = NumericFormats.HEXADECIMAL_2;

        /// <summary>
        /// Pass this constant to the ToString method on any integral type to
        /// format it as a string of 4 hexadecimal digits.
        /// </summary>
		public const string HEXADECIMAL_4 = NumericFormats.HEXADECIMAL_4;

        /// <summary>
        /// Pass this constant to the ToString method on any integral type to
        /// format it as a string of 8 hexadecimal digits.
        /// </summary>
		public const string HEXADECIMAL_8 = NumericFormats.HEXADECIMAL_8;

        /// <summary>
        /// Substitute this into a format string as a prefix to a hexadecimal
        /// number display. This string renders exactly as shown, 0h.
        /// </summary>
		public const string HEXADECIMAL_PREFIX_0H_LC = NumericFormats.HEXADECIMAL_PREFIX_0H_LC;

        /// <summary>
        /// Substitute this into a format string as a prefix to a hexadecimal
        /// number display. This string renders exactly as shown, 0H.
        /// </summary>
		public const string HEXADECIMAL_PREFIX_0H_UC = NumericFormats.HEXADECIMAL_PREFIX_0H_UC;

        /// <summary>
        /// Substitute this into a format string as a prefix to a hexadecimal
        /// number display. This string renders exactly as shown, 0x.
        /// </summary>
		public const string HEXADECIMAL_PREFIX_0X_LC = NumericFormats.HEXADECIMAL_PREFIX_0X_LC;

        /// <summary>
        /// Substitute this into a format string as a prefix to a hexadecimal
        /// number display. This string renders exactly as shown, 0X.
        /// </summary>
		public const string HEXADECIMAL_PREFIX_0X_UC = NumericFormats.HEXADECIMAL_PREFIX_0X_UC;

        /// <summary>
        /// Pass this constant to the ToString method on any integral type to
        /// format it according to the current settings in the Regional Settings
        /// part of the Windows Control Panel.
        /// 
        /// This format string causes the formatting engine to obey ALL of the
        /// settings, including the number of digits to display after the
        /// decimal point for a whole number.
        /// </summary>
		public const string NUMBER_PER_REG_SETTINGS = NumericFormats.NUMBER_PER_REG_SETTINGS;

        /// <summary>
        /// Pass this constant to the ToString method on any integral type to
        /// format it according to the current settings in the Regional Settings
        /// part of the Windows Control Panel.
        /// 
        /// This format string causes the formatting engine to obey all of the
        /// settings, EXCEPT the number of digits to display after the decimal
        /// point for a whole number.
        /// 
        /// This format string overrides the digits after decimal value
        /// specified by the iDigits value of Windows Registry key
        /// HKCU\Control Panel\International, causing it to behave as if it had
        /// been set to 0.
        /// 
        /// The override applies only to the instance ToString method being
        /// called; the Registry is unchanged.
        /// 
        /// See http://technet.microsoft.com/en-us/library/cc978638.aspx.
        /// </summary>
		public const string NUMBER_PER_REG_SETTINGS_0D = NumericFormats.NUMBER_PER_REG_SETTINGS_0D;

        /// <summary>
        /// Pass this constant to the ToString method on any integral type to
        /// format it according to the current settings in the Regional Settings
        /// part of the Windows Control Panel.
        /// 
        /// This format string causes the formatting engine to obey all of the
        /// settings, EXCEPT the number of digits to display after the decimal
        /// point for a whole number.
        /// 
        /// This format string overrides the digits after decimal value
        /// specified by the iDigits value of Windows Registry key
        /// HKCU\Control Panel\International, causing it to behave as if it had
        /// been set to 2, which happens to be the default for a US
        /// installation. Nevertheless, uses this value if changes made by the
        /// user would mess up your work.
        /// 
        /// The override applies only to the instance ToString method being
        /// called; the Registry is unchanged.
        /// 
        /// See http://technet.microsoft.com/en-us/library/cc978638.aspx.
        /// </summary>
		public const string NUMBER_PER_REG_SETTINGS_2D = NumericFormats.NUMBER_PER_REG_SETTINGS_2D;

        /// <summary>
        /// Pass this constant to the ToString method on any integral type to
        /// format it according to the current settings in the Regional Settings
        /// part of the Windows Control Panel.
        /// 
        /// This format string causes the formatting engine to obey all of the
        /// settings, EXCEPT the number of digits to display after the decimal
        /// point for a whole number.
        /// 
        /// This format string overrides the digits after decimal value
        /// specified by the iDigits value of Windows Registry key
        /// HKCU\Control Panel\International, causing it to behave as if it had
        /// been set to 3.
        /// 
        /// The override applies only to the instance ToString method being
        /// called; the Registry is unchanged.
        /// 
        /// See http://technet.microsoft.com/en-us/library/cc978638.aspx.
        /// </summary>
		public const string NUMBER_PER_REG_SETTINGS_3D = NumericFormats.NUMBER_PER_REG_SETTINGS_3D;

        /// <summary>
        /// Pass this constant to the ToString method on a single or double
        /// precision floating point number to be displayed as an integral
        /// percentage.
        /// </summary>
		public const string PERCENT = NumericFormats.PERCENT;

        /// <summary>
        /// Pass this constant to the ToString method on a single or double
        /// precision floating point number to be displayed as a fixed point
        /// percentage, accurate to two decimal places.
        /// </summary>
		public const string PERCENT_DIGITS_2 = NumericFormats.PERCENT_DIGITS_2;

        /// <summary>
        /// I use this with my SysDateFormatters class to format a date (sans
        /// time) so that it prints as YYYY/MM/DD.
        /// 
        /// IMPORTANT: This string specifically targets the methods in the
        /// SysDateFormatters class. SysDateFormatters strings are incompatible
        /// with ToString.
        /// </summary>
        /// <example>
        /// 2014/09/04
        /// </example>
        public const string STANDARD_DISPLAY_DATE_FORMAT = SysDateFormatters.RFD_YYYY_MM_DD;

        /// <summary>
        /// I use this with my SysDateFormatters class to format a date and time
		/// so that it prints as YYYY/MM/DD HH:MM:SS.
        /// 
        /// IMPORTANT: This string specifically targets the methods in the
        /// SysDateFormatters class. SysDateFormatters strings are incompatible
        /// with ToString.
        /// </summary>
        /// <example>
        /// 2014/09/04 16:17:30
        /// </example>
        public const string STANDARD_DISPLAY_DATE_TIME_FORMAT = SysDateFormatters.RFD_YYYY_MM_DD_HH_MM_SS;

        /// <summary>
        /// I use this with my SysDateFormatters class to format a time (sans
		/// date) so that it prints as HH:MM:SS.
        /// 
        /// IMPORTANT: This string specifically targets the methods in the
        /// SysDateFormatters class. SysDateFormatters strings are incompatible
        /// with ToString.
        /// </summary>
        /// <example>
        /// 16:17:30
        /// </example>
        public const string STANDARD_DISPLAY_TIME_FORMAT = SysDateFormatters.RFD_HH_MM_SS;
        #endregion  // Convenience Constants


        #region Service Methods
        /// <summary>
        /// Use my standard format string for displaying date stamps in
        /// reports, to format a DateTime structure.
        /// </summary>
        /// <param name="pdtmTestDate">
        /// Specify the populated DateTime to be formatted. Since only the date
        /// goes into the format, the time component MAY be uninitialized.
        /// </param>
        /// <returns>
        /// The return value is a string representation of the date and time,
        /// rendered according to constant STANDARD_DISPLAY_TIME_FORMAT.
        /// </returns>
        public static string FormatDateForShow ( DateTime pdtmTestDate )
        {
            return SysDateFormatters.ReformatSysDate (
                pdtmTestDate ,
                DisplayFormats.STANDARD_DISPLAY_DATE_FORMAT );
        }   // public static string FormatDateForShow

        /// <summary>
        /// Use my standard format string for displaying date/time stamps in
        /// reports, to format a DateTime structure.
        /// </summary>
        /// <param name="pdtmTestDate">
        /// Specify the populated DateTime to be formatted. Since the date and
        /// time go into the output string, the entire structure must be
        /// initialized.
        /// </param>
        /// <returns>
        /// The return value is a string representation of the date and time,
        /// rendered according to constant STANDARD_DISPLAY_DATE_TIME_FORMAT.
        /// </returns>
        public static string FormatDateTimeForShow ( DateTime pdtmTestDate )
        {
            return SysDateFormatters.ReformatSysDate (
                pdtmTestDate ,
                DisplayFormats.STANDARD_DISPLAY_DATE_TIME_FORMAT );
        }   // public static string FormatDateTimeForShow

        /// <summary>
        /// Use my standard format string for displaying time stamps in reports,
        /// to format a DateTime structure.
        /// </summary>
        /// <param name="pdtmTestDate">
        /// Specify the populated DateTime to be formatted. Since only the time
        /// goes into the format, the date component MAY be uninitialized.
        /// </param>
        /// <returns>
        /// The return value is a string representation of the date and time,
        /// rendered according to constant STANDARD_DISPLAY_TIME_FORMAT.
        /// </returns>
        public static string FormatTimeForShow ( DateTime pdtmTestDate )
        {
            return SysDateFormatters.ReformatSysDate (
                pdtmTestDate ,
                DisplayFormats.STANDARD_DISPLAY_TIME_FORMAT );
        }   // public static string FormatTimeForShow
        #endregion  // Service Methods
    }   // public static class DisplayFormats
}   // partial namespace WizardWrx.DLLServices2