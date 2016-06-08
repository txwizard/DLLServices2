/*
    ============================================================================

    Namespace:          WizardWrx.DLLServices2

    Class Name:         Util

    File Name:          Util.cs

    Synopsis:           This static class is a container for utility routines
                        intended for general consumption.

    Remarks:            Since TimeZoneInfo was added to Microsoft .NET Framework
                        version 3.5, any assembly that incorporates this class
                        must be built against that framework or higher.

                        This class was created around GetRegMultiStringValue, a
                        DllUtlity routine for gracefully handling REG_MULTI_SZ
                        values stored in Windows Registry keys. Putting them in
                        a separate class leaves me with the option of moving the
                        code into a class library.

    Reference:          "Json.net fails when trying to Deserialize a Class that Inherits from Exception,"
                        http://stackoverflow.com/questions/14186000/json-net-fails-when-trying-to-deserialize-a-class-that-inherits-from-exception#new-answer

    Author:             David A. Gray

    License:            Copyright (C) 2014-2016, David A. Gray.
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

    Created:            Friday, 29 August 2014 - Wednesday, 03 September 2014

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version Author Description
    ---------- ------- ------ --------------------------------------------------
    2014/09/03 1.0     DAG    Initial implementation.

    2014/09/12 1.0     DAG    Moved from DllUtl Class in EZXFER_Cloud_Manager and
                              merged FormatDateForShow from TimeZoneLab.

    2014/09/15 5.2     DAG    Permanently move to WizardWrx.ApplicationHelpers3
                              class library.

    2015/06/06 5.4     DAG    Correct the logic error in IsLastForIterationLE,
                              and add IsLastForIterationLT, which puts the bad
                              logic that I put into IsLastForIterationLE to some
                              productive use. Give MoreForIterationsToComeLE the
                              same treatment.

    2015/06/20 5.5     DAG    Relocate to WizardWrx.DLLServices2 namespace and
                              class library, move the functions that query the
                              Windows Registry into a new dedicated class, and
                              move the loop index tests into another dedicated
                              class. The new classes are named RegistryValues
                              and Logic, respectively. ByteArrayToHexDigitString
                              comes over from WizardWrx.SharedUtl4.dll, for use
                              by routines that must display a REG_BINARY value.

    2015/10/20 5.8     DAG    Extract the little loop that converts a byte array
                              of ANSI characters into a new System.string,
                              which I called StringFromANSICharacterArray.

    2016/04/03 6.0     DAG    Define a new member, SafeConsoleClear, for use as
                              a drop-in replacement for Console.Clear, which
                              throws a trappable exception if the standard
                              output stream is redirected.

    2016/04/26 6.0     DAG    Correct a spelling error found during a code scan.

    2016/06/07 6.3     DAG    Adjust the internal documentation to correct a few
                              inconsistencies uncovered while preparing this
							  library for publication on GetHub.
    ============================================================================
*/


using System;

using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using WizardWrx;


namespace WizardWrx.DLLServices2
{
    /// <summary>
    /// This static class exposes utility constants and methods that run the
    /// gamut from syntactic sugar to functions that hide useful, but somewhat
    /// obscure capabilities of the Microsoft Base Class Library, and don't fit
	/// neatly into another class.
    ///
    /// Since static classes are implicitly sealed, this class cannot be inherited.
    /// </summary>
    public static class Util
    {
        #region Public Constants
        /// <summary>
        /// Use this to set ByteArrayToHexDigitString argument puintGroupSize to
        /// insert a space between every 4th byte.
        /// </summary>
        public const uint BYTES_TO_STRING_BLOCK_OF_4 = 4;


        /// <summary>
        /// Use this to set ByteArrayToHexDigitString argument puintGroupSize to
        /// insert a space between every 8th byte.
        /// </summary>
        public const uint BYTES_TO_STRING_BLOCK_OF_8 = 8;


        /// <summary>
        /// Use this to set ByteArrayToHexDigitString argument puintGroupSize to
        /// format the string without any spaces.
        /// </summary>
        /// <remarks>
        /// This constant is intended primarily for internal use by the first
        /// overload, which omits the second argument, to call the second
        /// overload, which does the work.
        /// </remarks>
        public const uint BYTES_TO_STRING_NO_SPACING = 0;
        #endregion  // Public Constants


        #region Public Methods
        /// <summary>
        /// Convert a byte array into a printable hexadecimal representation.
        /// </summary>
        /// <param name="pbytInputData">
        /// Specify the byte array to be formatted. Any byte array will do.
        /// </param>
        /// <returns>
        /// The return value is a string that contains two characters for each
        /// byte in the array.
        /// </returns>
        public static string ByteArrayToHexDigitString ( byte [ ] pbytInputData )
        {
            return ByteArrayToHexDigitString (
                pbytInputData ,
                BYTES_TO_STRING_NO_SPACING );
        }   // ByteArrayToHexDigitString (1 of 2)


        /// <summary>
        /// Convert a byte array into a printable hexadecimal representation.
        /// </summary>
        /// <param name="pbytInputData">
        /// Specify the byte array to be formatted. Any byte array will do.
        /// </param>
        /// <param name="puintGroupSize">
        /// Specify the number of bytes to display as a group.
        /// </param>
        /// <returns>
        /// The return value is a string that contains two characters for each
        /// byte in the array, plus one space between every puintGroupSizeth
        /// byte.
        /// </returns>
        public static string ByteArrayToHexDigitString (
            byte [ ] pbytInputData ,
            uint puintGroupSize )
        {
            StringBuilder sbOutput = new StringBuilder ( pbytInputData.Length );

            //  ----------------------------------------------------------------
            //  Loop through each byte of the hashed data, and format each one
            //  as a hexadecimal string. Although this For loop will never
            //  contain more than one statement, I left the braces to separate
            //  that statement from the third line of the For statement, which I
            //  spread across three lines because of its length.
            //  ----------------------------------------------------------------

            for ( int intOffset = ArrayInfo.ARRAY_FIRST_ELEMENT ;
                  intOffset < pbytInputData.Length ;
                  intOffset++ )
            {
                if ( puintGroupSize > 0 && intOffset > 0 && intOffset % puintGroupSize == 0 )
                    sbOutput.Append ( SpecialCharacters.SPACE_CHAR );

                sbOutput.Append ( pbytInputData [ intOffset ].ToString ( DisplayFormats.HEXADECIMAL_2 ).ToLowerInvariant ( ) );
            }   //  for ( int intOffset = MagicNumbers.ARRAY_FIRST_ELEMENT ; ...

            return sbOutput.ToString ( );       //  Return the hexadecimal string.
        }   // ByteArrayToHexDigitString (2 of 2)


        /// <summary>
        /// Match file names against a true regular expression, as opposed to
        /// the anemic masks supported by DOS and Windows. Though occasionally
        /// referred to as regular expressions, file specifications that use
        /// DOS wild cards are a far cry from true regular expressions.
        /// </summary>
        /// <param name="pstrPathString">
        /// Specify the path string to match against PCRE pstrRegExpToMatch.
        /// </param>
        /// <param name="pstrRegExpToMatch">
        /// Specify the Perl Compatible Regular Expression against which to
        /// evaluate pstrFileName.
        /// </param>
        /// <returns>
        /// The function returns TRUE if neither string is null or empty AND
        /// pstrRegExpToMatch matches PCRE pstrFileName.
        /// </returns>
        /// <remarks>
        /// This method could have been coded inline. However, since I have at
        /// least one other project in the works that requires it, I segregated
        /// it in this routine in this small, easily navigable class.
        /// </remarks>
        public static bool FileMatchesRegExpMask (
            string pstrPathString ,
            string pstrRegExpToMatch )
        {
            if ( string.IsNullOrEmpty ( pstrPathString ) )
                return false;

            if ( string.IsNullOrEmpty ( pstrRegExpToMatch ) )
                return false;

            return Regex.IsMatch (
                pstrPathString ,
                pstrRegExpToMatch ,
                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase );
        }   // FileMatchesRegExpMask


        /// <summary>
        /// Get the GUID string (Registry format) attached to an assembly.
        /// </summary>
        /// <param name="pasm">
        /// Assembly from which to return the GUID string.
        /// </param>
        /// <returns>
        /// If the method succeeds, the return value is the GUID attached to it
        /// and intended to be associated with its type library if the assembly
        /// is exposed to COM.
        /// </returns>
        public static string GetAssemblyGuidString ( Assembly pasm )
        {
            object [ ] objAttribs = pasm.GetCustomAttributes (
                typeof ( System.Runtime.InteropServices.GuidAttribute ) ,
                false );

            if ( objAttribs.Length > ListInfo.EMPTY_STRING_LENGTH )
            {
                System.Runtime.InteropServices.GuidAttribute oMyGuid = ( System.Runtime.InteropServices.GuidAttribute ) objAttribs [ ArrayInfo.ARRAY_FIRST_ELEMENT ];
                return oMyGuid.Value.ToString ( );
            }   // TRUE (anticipated outcome) block, if ( objAttribs.Length > ListInfo.EMPTY_STRING_LENGTH )
            else
            {
                return string.Empty;
            }   // FALSE (UNanticipated outcome) block, if ( objAttribs.Length > ListInfo.EMPTY_STRING_LENGTH )
        }   // GetAssemblyGuidString


#if NET35
        /// <summary>
        /// Given a DateTime and a system time zone ID string, return the
        /// appropriate text to display, depending on whether the specified time
        /// is standard or Daylight Saving Time.
        /// </summary>
        /// <param name="pdtmTestDate">
        /// Specify the Syatem.DateTime for which the appropriate time zone
        /// string is required. Both DateTime.MinValue and DateTime.MaxValue are
        /// invalid; specifying either elicits the empty string.
        /// </param>
        /// <param name="pstrTimeZoneID">
        /// Specify a valid time zone ID string. Please see the Remarks.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is the appropriate string
        /// to display for the given time. Otherwise, the empty string is
        /// returned or one of several exceptions is thrown, the most likely of
        /// which is a TimeZoneNotFoundException, which is thrown when the
        /// specified time zone ID string is invalid.
        /// </returns>
        /// <remarks>
        /// if in doubt, use TimeZoneInfo.GetSystemTimeZones to enumerate the
        /// time zones installed on the local machine. Invalid time zone strings
        /// always give rise to one of a number of exceptions, all of which are
        /// fully described in the documentation of a companion function,
        /// GetSystemTimeZoneInfo which this routine uses to get the time zone
        /// information that it needs.
        /// </remarks>
        /// <see cref="GetSystemTimeZoneInfo"/>
        public static string GetDisplayTimeZone (
            DateTime pdtmTestDate ,
            string pstrTimeZoneID )
        {
            if ( pdtmTestDate == DateTime.MinValue || pdtmTestDate == DateTime.MaxValue )
            {   // Insufficient data available
                return string.Empty;
            }   // TRUE (degenerate case) block, if ( pdtmTestDate == DateTime.MinValue || pdtmTestDate == DateTime.MaxValue || string.IsNullOrEmpty(pstrTimeZoneID) )
            else
            {
                TimeZoneInfo tzinfo = GetSystemTimeZoneInfo ( pstrTimeZoneID );
                return tzinfo.IsDaylightSavingTime ( pdtmTestDate ) ?
                    tzinfo.DaylightName :
                    tzinfo.StandardName;
            }   // FALSE (desired outcome) block, if ( pdtmTestDate == DateTime.MinValue || pdtmTestDate == DateTime.MaxValue || string.IsNullOrEmpty(pstrTimeZoneID) )
        }   // public static string GetDisplayTimeZone


        /// <summary>
        /// Given a system time zone ID string, return the corresponding
        /// TimeZoneInfo object if the specified time zone is defined on the
        /// local system.
        /// </summary>
        /// <param name="pstrTimeZoneID">
        /// Specify a valid time zone ID string. There are two special IDs,
        /// LocalTime and UTC, both of which are accessible through static
        /// properties on the TimeZoneInfo class. Although you could use the ID
        /// properties with this method, the most efficient way to handle these
        /// special cases is by reference to the Local property for LocalTime
        /// and the UTC property for UTC time. (This method could take the same
        /// shortcut, but I decided that it wasn't worth the extra code and
        /// testing.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is a TimeZoneInfo object,
        /// populated from the Windows Registry. Otherwise, one of the
        /// exceptions listed and described below is thrown.
        /// </returns>
        /// <exception cref="OutOfMemoryException">
        /// You should restart Windows if this happens.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Contact the author of the program. This is something that he or she
        /// must address.
        /// </exception>
        /// <exception cref="TimeZoneNotFoundException">
        /// Contact the author of the program. This is something that he or she
        /// must address.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// Contact your system administrator to inquire about why your program
        /// is forbidden to read the regional settings from the Windows
        /// Registry.
        /// </exception>
        /// <exception cref="InvalidTimeZoneException">
        /// Contact your system support group. A corrupted Windows Registry is a
        /// rare, but serious matter.
        /// </exception>
        /// <exception cref="Exception">
        /// Start with your system support group, who may need to request the
        /// assistance of the author of the program.
        /// </exception>
        /// <remarks>
        /// if in doubt, use TimeZoneInfo.GetSystemTimeZones to enumerate the
        /// time zones installed on the local machine.
        /// </remarks>
        public static TimeZoneInfo GetSystemTimeZoneInfo ( string pstrTimeZoneID )
        {
            const string ARGNAME = @"pstrTimeZoneID";

            const string ERRMSG_NO_MEMORY = @"SYSTEM RESOURCE FAMINE: The GetDisplayTimeZone method ran out of memory.";
            const string ERRMSG_NULL_TZ_ID = @"INTERNAL ERROR: The GetDisplayTimeZone method let a null pstrTimeZoneID through to TimeZoneInfo.FindSystemTimeZoneById.";
            const string ERRMSG_TZ_NOT_FOUND = @"INTERNAL or DATA ERROR: The GetDisplayTimeZone method let a pstrTimeZoneID that isn't registered on this computer through to TimeZoneInfo.FindSystemTimeZoneById.{1}                        Specified ID = {0}";
            const string ERRMSG_SECURITY = @"ACCESS VIOLATION: The GetDisplayTimeZone method cannot read the Registry keys where the time zone information is kept. The process has insufficient access permissions on that key.";
            const string ERRMSG_INV_TZINFO = @"CORRUPTED SYSTEM REGISTRY: The GetDisplayTimeZone method found the specified key, but the corresponding Registry key is corrupted.{1}                           Specified ID = {0}";
            const string ERRMSG_RUNTIME = @"RUNTIME EXCEPTION: The GetDisplayTimeZone method found the specified key, but the corresponding Registry key is corrupted.{1}                    Specified ID = {0}";

            if ( string.IsNullOrEmpty ( pstrTimeZoneID ) )
            {   // Insufficient data available
                throw new ArgumentNullException ( ARGNAME );
            }   // TRUE (undesired outcome) block, if ( pdtmTestDate == DateTime.MinValue || pdtmTestDate == DateTime.MaxValue || string.IsNullOrEmpty(pstrTimeZoneID) )
            else
            {
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById ( pstrTimeZoneID );
                }
                catch ( OutOfMemoryException exNoMem )
                {
                    throw new Exception (
                        ERRMSG_NO_MEMORY ,
                        exNoMem );
                }
                catch ( ArgumentNullException exNullID )
                {
                    throw new Exception (
                        ERRMSG_NULL_TZ_ID ,
                        exNullID );
                }
                catch ( TimeZoneNotFoundException exTZNotFound )
                {
                    throw new Exception (
                        string.Format (
                            ERRMSG_TZ_NOT_FOUND ,
                            pstrTimeZoneID ,
                            Environment.NewLine ) ,
                        exTZNotFound );
                }
                catch ( System.Security.SecurityException exSecurity )
                {
                    throw new Exception (
                        ERRMSG_SECURITY ,
                        exSecurity );
                }
                catch ( InvalidTimeZoneException exInvTZInfo )
                {
                    throw new Exception (
                        string.Format (
                            ERRMSG_INV_TZINFO ,
                            pstrTimeZoneID ,
                            Environment.NewLine ) ,
                        exInvTZInfo );
                }
                catch ( Exception exMisc )
                {
                    throw new Exception (
                        string.Format (
                            ERRMSG_RUNTIME ,
                            pstrTimeZoneID ,
                            Environment.NewLine ) ,
                        exMisc );
                }
            }   // FALSE (desired outcome) block, if ( pdtmTestDate == DateTime.MinValue || pdtmTestDate == DateTime.MaxValue || string.IsNullOrEmpty(pstrTimeZoneID) )
        }   // public static string GetSystemTimeZoneInfo


#endif  // #if NET35
        /// <summary>
        /// Load the lines of a plain ASCII text file that has been stored with
        /// the assembly as a embedded resource into an array of native strings.
        /// </summary>
        /// <param name="pstrResourceName">
        /// Specify the fully qualified resource name, which is its source file
        /// name appended to the default application namespace.
        /// </param>
        /// <returns>
        /// The return value is an array of Unicode strings, each of which is
        /// the text of a line from the original text file, sans terminator.
        /// </returns>
        /// <see cref="LoadTextFileFromAnyAssembly"/>
        /// <seealso cref="LoadTextFileFromEntryAssembly"/>
        public static string [ ] LoadTextFileFromCallingAssembly (
            string pstrResourceName )
        {
            return LoadTextFileFromAnyAssembly (
                pstrResourceName ,
                Assembly.GetCallingAssembly ( ) );
        }   // LoadTextFileFromCallingAssembly


        /// <summary>
        /// Load the lines of a plain ASCII text file that has been stored with
        /// the assembly as a embedded resource into an array of native strings.
        /// </summary>
        /// <param name="pstrResourceName">
        /// Specify the fully qualified resource name, which is its source file
        /// name appended to the default application namespace.
        /// </param>
        /// <returns>
        /// The return value is an array of Unicode strings, each of which is
        /// the text of a line from the original text file, sans terminator.
        /// </returns>
        /// <see cref="LoadTextFileFromAnyAssembly"/>
        /// <seealso cref="LoadTextFileFromCallingAssembly"/>
        public static string [ ] LoadTextFileFromEntryAssembly (
            string pstrResourceName )
        {
            return LoadTextFileFromAnyAssembly (
                pstrResourceName ,
                Assembly.GetEntryAssembly ( ) );
        }   // LoadTextFileFromEntryAssembly


        /// <summary>
        /// Load the lines of a plain ASCII text file that has been stored with
        /// a specified assembly as a embedded resource into an array of native
        /// strings.
        /// </summary>
        /// <param name="pstrResourceName">
        /// Specify the fully qualified resource name, which is its source file
        /// name appended to the default application namespace.
        /// </param>
        /// <param name="pasmSource">
        /// Pass in a reference to the Assembly from which you expect to load
        /// the text file. Use any means at your disposal to obtain a reference
        /// from the System.Reflection namespace.
        /// </param>
        /// <returns></returns>
        /// <seealso cref="LoadTextFileFromCallingAssembly"/>
        /// <seealso cref="LoadTextFileFromEntryAssembly"/>
        public static string [ ] LoadTextFileFromAnyAssembly (
            string pstrResourceName ,
            Assembly pasmSource )
        {
            byte [ ] abytWholeFile = LoadBinaryResourceFromAnyAssembly (
                pstrResourceName ,
                pasmSource );

            //  ----------------------------------------------------------------
            //  The file is stored in single-byte ASCII characters. The native
            //  character set of the Common Language Runtime is Unicode. A new
            //  array of Unicode characters serves as a translation buffer which
            //  is filled a character at a time from the byte array.
            //  ----------------------------------------------------------------

            //  ----------------------------------------------------------------
            //  The character array converts to a Unicode string in one fell
            //  swoop. Since the new string vanishes when StringOfLinesToArray
            //  returns, the transformation is nested in the call to the
            //  consumer function, StringOfLinesToArray, which splits the lines
            //  of text, with their DOS line terminators, into the required
            //  array of strings.
            //
            //  Ideally, the blank line should be removed. However, since the
            //  RemoveEmptyEntries member of the StringSplitOptions enumeration
            //  does it for me, I may as well use it, and save myself the future
            //  aggravation, when I will have probably forgotten why it happens.
            //  ----------------------------------------------------------------

            return Util.StringOfLinesToArray (
                StringFromANSICharacterArray ( abytWholeFile ) ,
                StringSplitOptions.RemoveEmptyEntries );
        }   // LoadTextFileFromAnyAssembly


        /// <summary>
        /// Load the named embedded binary resource into a byte array.
        /// </summary>
        /// <param name="pstrResourceName">
        /// Specify the external name of the file as it appears in the source
        /// file tree and the Solution Explorer.
        /// </param>
        /// <param name="pasmSource">
        /// Supply a System.Reflection.Assembly reference to the assembly that
        /// contains the embedded resource.
        /// </param>
        /// <returns>
        /// If the function succeeds, it returns a byte array containing the raw
        /// bytes that comprise the embedded resource. Hence, this method can
        /// load ANY embedded resource.
        /// </returns>
        /// <remarks>
        /// Since all other resource types ultimately come out as byte arrays,
        /// the text file loaders call upon this routine to extract their data.
        ///
        /// The notes in the cited reference refreshed my memory of observations
        /// that I made and documented a couple of weeks ago. However, it was a
        /// lot easier to let Google find a reference document, which was
        /// probably intended for students in the Computer Science department at
        /// Columbia University, at http://www1.cs.columbia.edu/~lok/csharp/refdocs/System.IO/types/Stream.html"/>,
        /// than find my own source.
        /// </remarks>
        public static byte [ ] LoadBinaryResourceFromAnyAssembly (
            string pstrResourceName ,
            Assembly pasmSource )
        {
            string strInternalName = GetInternalResourceName (
                pstrResourceName ,
                pasmSource );

            if ( strInternalName == null )
            {   // This exception is thrown clear.
                throw new Exception (
                    string.Format (
                        Properties.Resources.ERRMSG_EMBEDDED_RESOURCE_NOT_FOUND ,               // Message Template
                        pstrResourceName ,                                                      // Format Item 0 = Name of resource, as specified in the call
                        pasmSource.FullName ) );                                                // Format Item 1 = Assembly full name (Base, Version, PKT, Culture)
            }   // if ( strInternalName == null )

            Stream stgTheFile = null;

            try
            {   // Subsequent exceptions are caught, packaged, and re-thrown.
                stgTheFile = pasmSource.GetManifestResourceStream (
                    strInternalName );

                //  ------------------------------------------------------------
                //  The character count is used several times, always as an
                //  integer. Cast it once, and keep it, since implicit casts
                //  create new, short lived local variables that quickly land in
                //  the recycle bin (a. k. a., the Garbage Collector).
                //
                //  The integer is immediately put to use, to allocate a byte
                //  array, which must have room for every character in the input
                //  file. Since this value is given by the logical size of the
                //  embedded resource, it makes sense to allocate all of it at
                //  once, and to use the least expensive data structure, the old
                //  fashioned array.
                //  ------------------------------------------------------------

                byte [ ] abytWholeFile;
                int intTotalBytesAsInt = ( int ) stgTheFile.Length;
                abytWholeFile = new Byte [ intTotalBytesAsInt ];
                int intBytesRead = stgTheFile.Read (
                    abytWholeFile ,                                                             // Buffer sufficient to hold it.
                    ListInfo.BEGINNING_OF_BUFFER ,                                              // Read from the beginning of the file.
                    intTotalBytesAsInt );                                                       // Swallow the file whole.

                //  ------------------------------------------------------------
                //  Though its backing store is a resource embedded in the
                //  assembly, the stream must be treated like any other.
                //  Investigating in the Visual Studio Debugger showed me that
                //  it is implemented as an UnmanagedMemoryStream. That
                //  "unmanaged" prefix is a clarion call that the stream must be
                //  closed, disposed, and destroyed. To that end, this method
                //  handles its own exceptions, although it eventually throws an
                //  exception of its own, in a way that ensures that a Finally
                //  block cleans up the stream.
                //  ------------------------------------------------------------

                stgTheFile.Close ( );
                stgTheFile.Dispose ( );
                stgTheFile = null;      // Signal the Finally block that the stream closed normally.

                //  ------------------------------------------------------------
                //  In the unlikely event that the byte count is short or long,
                //  the program must croak. Since the three items that we want
                //  to include in the report are stored in local variables,
                //  including the reported file length, we go ahead and close
                //  the stream before the count of bytes read is evaluated.
                //  ------------------------------------------------------------

                if ( intBytesRead == intTotalBytesAsInt )
                {   // Byte count matches count stored in metadata.
                    return abytWholeFile;
                }   // TRUE (expected outcome) block, if ( intBytesRead == intTotalBytesAsInt )
                else
                {   //  Organize everything we know into an exception report,
                    //  which is caught, packaged, and re-thrown in this
                    //  routine.

                    throw new InvalidDataException (
                        string.Format (
                            Properties.Resources.ERRMSG_EMBEDDED_RESOURCE_READ_TRUNCATED ,      // Message Template
                            new object [ ]
                        {
                            strInternalName ,                                                   // Format Item 0 = Internal name derived from name passed into call
                            intTotalBytesAsInt ,                                                // Format Item 1 = Expected byte count
                            intBytesRead ,                                                      // Format Item 2 = Actual byte count
                            Environment.NewLine                                                 // Format Item 3 = Newline
                        } ) );
                }   // FALSE (UNexpected outcome) block, if ( intBytesRead == intTotalBytesAsInt )
            }
            catch ( Exception exAll )
            {
                throw new Exception (
                    string.Format (
                        Properties.Resources.ERRMSG_EMBEDDED_RESOURCE_READ_ERROR ,              // Message Template
                        new string [ ]
                        {
                            pasmSource.FullName ,                                               // Format Item 0 = Assembly full name (Base, Version, PKT, Culture)
                            strInternalName ,                                                   // Format Item 1 = Internal name derived from name passed into call
                            exAll.Message ,                                                     // Format Item 2 = Message taken from Inner Exception
                            Environment.NewLine                                                 // Format Item 3 = Newline
                        } ) ,
                    exAll );
            }   // The stream remains open if there was a mishap.
            finally
            {   // Whatever happens, clean up the unmanaged memory.
                if ( stgTheFile != null )
                {   // The stream read succeeded, closing the stream normally.
                    if ( stgTheFile.CanRead )
                    {   // The stream is open if CanRead is TRUE.
                        stgTheFile.Close ( );
                    }   // if ( stgTheFile.CanRead )

                    //  --------------------------------------------------------
                    //  Presumably, Close calls Dispose. However, since it is
                    //  marked public, I prefer to call it anyway. For similar
                    //  reasons, I set the stream to NULL, even though hough it
                    //  is about to go out of scope.
                    //  --------------------------------------------------------

                    stgTheFile.Dispose ( );
                    stgTheFile = null;
                }   // if ( stgTheFile != null )
            }   // Try/Catch/Finally block
        }   // LoadBinaryResourceFromAnyAssembly


        /// <summary>
        /// Use this method as a non-throwing replacement for Console.Clear,
        /// which throws an System.IO.IOException exception if the standard
        /// output stream is redirected. This catches that exception, and
        /// casts it to an instance of EnhancedIOException, which is derived from
        /// System.IO.IOException.
        /// </summary>
        /// <remarks>
        /// This feat is accomplished by a round trip through SerializeObject,
        /// followed immediately by DeserializeObject, both static methods on
        /// the JsonConvert object. These tactics give me read only access to
        /// the HResult, which is hidden in version 2.0 of the .NET Framework.
        ///
        /// Comparing the HResult to a local constant, E_HANDLE, means that the
        /// error test works correctly in any locale.
        ///
        /// Thankfully, Microsoft came to their senses, and made the HResult
        /// visible in later frameworks.
        /// </remarks>
        public static void SafeConsoleClear ( )
        {
            const int E_HANDLE = -2147024890;   // 0x80070006, per WinError.h.

            try
            {
#if DEBUG
                System.Diagnostics.Debugger.Launch ( );
#endif  // #if DEBUG
                Console.Clear ( );
            }
            catch ( IOException exIO )
            {
                try
                {   // Serializing the IOException is the most straightforward way to cast it to a EnhancedIOException object.
                    EnhancedIOException exRichIO = EnhancedIOException.SubclassIOException ( exIO );

                    if ( exRichIO != null )
                    {
                        if ( exRichIO.HRESULT != E_HANDLE )
                        {
                            StateManager.GetTheSingleInstance ( ).AppExceptionLogger.ReportException ( exIO );
                        }   // if ( exRichIO.HRESULT != E_HANDLE )
                    }   // TRUE (anticipated outcome) block, if ( exRichIO != null )
                    else
                    {
                        throw new NullReferenceException ( Properties.Resources.ERRMSG_NULL_EnhancedIOException );
                    }   // FALSE (UNanticipated outcome) block, if ( exRichIO != null )
                }
                catch ( Exception exOtherNested )
                {   // Report the unexpected generic exception, then resume without clearing the console.
                    StateManager.GetTheSingleInstance ( ).AppExceptionLogger.ReportException ( exOtherNested );
                }   // catch ( System.IO.IOException exIO )
            }   // catch ( System.IO.IOException exIO )
            catch ( Exception exOthers)
            {   // Report the unexpected generic exception, then resume without clearing the console.
                {
                    StateManager.GetTheSingleInstance ( ).AppExceptionLogger.ReportException ( exOthers );
                }
            }   // catch ( System.IO.IOException exIO )
        }   // public static void SafeConsoleClear


        /// <summary>
        /// List selected properties of any assembly on a console.
        /// </summary>
        /// <param name="pmyLib">
        /// Pass in a reference to the desired assembly, which may be the
        /// assembly that exports a specified type, the executing assembly, the
        /// calling assembly, the entry assembly, or any other assembly for
        /// which you can obtain a reference.
        /// </param>
        public static void ShowKeyAssemblyProperties ( System.Reflection.Assembly pmyLib )
        {
            System.Reflection.AssemblyName MyNameIs = System.Reflection.AssemblyName.GetAssemblyName ( pmyLib.Location );
            System.Diagnostics.FileVersionInfo myVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo ( pmyLib.Location );

            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_SELECTED_DLL_PROPS_BEGIN,  Environment.NewLine );

            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_ASSEMBLYFILEBASENAME , System.IO.Path.GetFileNameWithoutExtension ( pmyLib.Location ) );
            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_VERSIONSTRING , myVersionInfo.FileVersion );
            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_CULTURE , MyNameIs.CultureInfo.DisplayName );
            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_PUBLICKEYTOKEN , Util.ByteArrayToHexDigitString ( MyNameIs.GetPublicKeyToken ( ) ) );

            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_RUNTIME_VERSION , pmyLib.ImageRuntimeVersion );
            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_ASSEMBLYGUIDSTRING , WizardWrx.DLLServices2.Util.GetAssemblyGuidString ( pmyLib ) );

            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_PRODUCTNAME , myVersionInfo.ProductName );
            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_LEGALCOPYRIGHT , myVersionInfo.LegalCopyright );
            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_LEGALTRADEMARKS , myVersionInfo.LegalTrademarks );
            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_COMPANYNAME , myVersionInfo.CompanyName );

            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_DESCRIPTION , myVersionInfo.FileDescription );
            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_COMMENTS , myVersionInfo.Comments , Environment.NewLine );

            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_ASSEMBYDIRNAME , System.IO.Path.GetDirectoryName ( pmyLib.Location ) );
            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_ASSEMBLYFILENAME , System.IO.Path.GetFileName ( pmyLib.Location ) , Environment.NewLine );

            string strAssemblyFileFQFN = pmyLib.Location;
            System.IO.FileInfo fiLibraryFile = new System.IO.FileInfo ( strAssemblyFileFQFN );

            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_FILE_CREATION_DATE , fiLibraryFile.CreationTime , fiLibraryFile.CreationTimeUtc );
            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_FILE_MODIFIED_DATE , fiLibraryFile.LastWriteTime , fiLibraryFile.LastWriteTimeUtc );

            Console.WriteLine ( Properties.Resources.MSG_ASM_PROPS_SELECTED_DLL_PROPS_END , Environment.NewLine );
        }   // private static void ShowKeAssemblyProperties method


        /// <summary>
        /// Transform an array of bytes, each representing one ANSI character, into a string.
        /// </summary>
        /// <param name="pabytWholeFile">
        /// Specify the array to transform.
        /// </param>
        /// <returns>
        /// The specified array is returned as a string.
        /// </returns>
        /// <remarks>
        /// I did this refactoring, thinking that I had a new use for the code,
        /// only to realize as I finished cleaning it up that I can't use it,
        /// because it deals in ANSI characters, and my present need involves
        /// Unicode characters. Nevertheless, the exercise is not a total loss,
        /// because it reminded me of the trick that I needed to transform the
        /// array of Unicode characters into a string.
        /// </remarks>
        public static string StringFromANSICharacterArray ( byte [ ] pabytWholeFile )
        {
            int intNCharacters = pabytWholeFile.Length;
            char [ ] achrWholeFile = new char [ intNCharacters ];

            for ( int intCurrentByte = ListInfo.BEGINNING_OF_BUFFER ;
                      intCurrentByte < intNCharacters ;
                      intCurrentByte++ )
                achrWholeFile [ intCurrentByte ] = ( char ) pabytWholeFile [ intCurrentByte ];

            //  ----------------------------------------------------------------
            //  The character array converts to a Unicode string in one fell
            //  swoop.
            //  ----------------------------------------------------------------

            return new string ( achrWholeFile );
        }   // public static string StringFromANSICharacterArray


        /// <summary>
        /// Split a string containing lines of text into an array of strings.
        /// </summary>
        /// <param name="pstrLines">
        /// String containing lines of text, terminated by CR/LF pairs.
        /// </param>
        /// <returns>
        /// Array of strings, one line per string. Blank lines are preserved as
        /// empty strings.
        /// </returns>
        public static string [ ] StringOfLinesToArray ( string pstrLines )
        {
            if ( pstrLines == null )
                return new string [ ] { };     // Return an empty array.

            if ( pstrLines.Length == ArrayInfo.ARRAY_IS_EMPTY )
                return new string [ ] { };     // Return an empty array.

            return pstrLines.Split (
                StringToArray ( SpecialStrings.STRING_SPLIT_NEWLINE ) ,
                StringSplitOptions.None );
        }   // public static string [ ] StringOfLinesToArray {1 of 2)


        /// <summary>
        /// Split a string containing lines of text into an array of strings,
        /// as modified by the StringSplitOptions flag.
        /// </summary>
        /// <param name="pstrLines">
        /// String containing lines of text, terminated by CR/LF pairs.
        /// </param>
        /// <param name="penmStringSplitOptions">
        /// A member of the StringSplitOptions enumeration, presumably other
        /// than StringSplitOptions.None, which is assumed by the first
        /// overload. The only option supported by version 2 of the Microsoft
        /// .NET CLR is RemoveEmptyEntries.
        /// </param>
        /// <returns>
        /// Array of strings, one line per string. Blank lines are preserved as
        /// empty strings unless penmStringSplitOptions is RemoveEmptyEntries,
        /// as is most likely to be the case.
        /// </returns>
        /// <remarks>
        /// Use this overload to convert a string, discarding blank lines.
        /// </remarks>
        public static string [ ] StringOfLinesToArray (
            string pstrLines ,
            StringSplitOptions penmStringSplitOptions )
        {
            if ( string.IsNullOrEmpty ( pstrLines ) )
                return new string [ ] { };     // Return an empty array.

            return pstrLines.Split (
                StringToArray ( SpecialStrings.STRING_SPLIT_NEWLINE ) ,
                penmStringSplitOptions );
        }   // public static string [ ] StringOfLinesToArray {2 of 2)


        /// <summary>
        /// Return a one-element array containing the input string.
        /// </summary>
        /// <param name="pstr">
        /// String to place into the returned array.
        /// </param>
        /// <returns>
        /// Array of strings, containing exactly one element, which contains
        /// the single input string.
        /// </returns>
        public static string [ ] StringToArray ( string pstr )
        {
            return new string [ ] { pstr };
        }   // public static string [ ] StringToArray
        #endregion  // Public Methods


        #region Private Static Methods
        /// <summary>
        /// Use the list of Manifest Resource Names returned by method
        /// GetManifestResourceNames on a specified assembly. Each of several
        /// methods employs a different mechanism to identify the assembly of
        /// interest.
        /// </summary>
        /// <param name="pstrResourceName">
        /// Specify the name of the file from which the embedded resource was
        /// created. Typically, this will be the local name of the file in the
        /// source code tree.
        /// </param>
        /// <param name="pasmSource">
        /// Pass a reference to the Assembly that is supposed to contain the
        /// desired resource.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is the internal name of
        /// the requested resource, which is fed to GetManifestResourceStream on
        /// the same assembly, which returns a read-only Stream backed by the
        /// embedded resource. If the specified resource is not found, it
        /// returns null.
        /// </returns>
        /// <remarks>
        /// Since I cannot imagine any use for this method beyond its
        /// infrastructure role in this class, I marked it private.
        /// </remarks>
        private static string GetInternalResourceName (
            string pstrResourceName ,
            Assembly pasmSource )
        {
            foreach ( string strManifestResourceName in pasmSource.GetManifestResourceNames ( ) )
                if ( strManifestResourceName.EndsWith ( pstrResourceName ) )
                    return strManifestResourceName;

            return null;
        }   // private static string GetInternalResourceName
        #endregion  //  Private Static Methods
    }   // public static class Util
}   // partial namespace WizardWrx.DLLServices2