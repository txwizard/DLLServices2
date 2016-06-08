/*
    ============================================================================

    Namespace Name:     WizardWrx.DLLServices2

    Class Name:         ExceptionLogger

    File Name:          ExceptionLogger.cs

    Synopsis:           This module contains the entire definition of a utility
                        class, ExceptionLogger, originally called
                        ThreadSafeExceptionReporting, and is intended to provide
                        thread-safe exception reporting for all applications
                        (Console, Windows Forms, ASP.NET, etc).

                        Since each thread has its own stack frame, and all data
                        is either contained in an argument or allocated locally
                        (with its address kept in the stack frame), all methods
                        should be thread safe.

    Remarks:            To ensure that the entire exception report is kept
                        together, even if another thread throws the same kind of
                        exception, messages are sent to the console in a single
                        string, with one synchronized stream I/O operation.

                        Though it is almost certainly overkill, as extra
                        insurance, access to the AppEventSourceID property is
                        synchronized for both reading and writing.

                        Although I have confirmed that EventLog.WriteEntry can
                        include both event ID and event Category values, I shall
                        defer adding the necessary overloads to support it until
                        another day.

    References:         1)  "Statics & Thread Safety: Part II," K. Scott Allen
                            http://odetocode.com/Articles/314.aspx

                        2)  The original ThreadSafeExceptionReporter class is
                            defined in the following source file.

                            %USERPROFILE%\Documents\Visual Studio 2013\Projects\WizardWrx_Libs\TimeStampGenerator\TimeStampGenerator\ThreadSafeExceptionReporters.cs

    Author:             David A. Gray

    License:            Copyright (C) 2010-2016, David A. Gray. All rights reserved.

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

    Contact:            dgray@wizardwrx.com

    Date Written:       Tuesday, 23 March 2010 and Wednesday, 24 March 2010.

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version Author Synopsis
    ---------- ------- ------ --------------------------------------------------
    2010/03/24 1.0     DAG    Initial version.

    2010/10/22 2.52    DAG    1) Move this class from TimeStampGenerator, the
                                 project in which it was created and tested, so
                                 that other projects that write to the console
                                 can use it.

                              2) Remove the lock dependency by replacing each
                                 series of console writes with appends to a
                                 StringBuilder, followed by a synchronized
                                 console write.

                                 This changes takes advantage of the recently
                                 noticed AppendFormat method of the workhorse
                                 StringBuilder class.

    2011/12/02 2.65    DAG    Add reporting of inner exceptions by recursively
                              calling private method AddCommonElements.

    2011/11/12 2.8     DAG    Move ThreadSafeExceptionReporting from SharedUtl2,
                              rename it ExceptionLogger, and change methods from
                              void to string, so that the return values can be
                              recycled as event log records. To support CUI or
                              GUI applications from one code base, it becomes
                              a private singleton, which is exposed as a read
                              only property of the ApplicationInstance object.

    2014/02/05 3.2     DAG    Add support for the new MessageInColors class, for
                              use in character mode applications.

    2014/03/24 3.3     DAG    1) Most overloads of ReportException were ignoring
                                 the AppStackTraceDisposition property setting,
                                 and blindly including the stack trace in every
                                 message.

                              2) If event logging is enabled, see to it that the
                                 message that goes into the event log always has
                                 the stack trace attached to it.

                              3) Replace string constants that become part of a
                                 message with named resource strings.

    2014/03/28 4.0     DAG    Enable greater control over the content and format
                              of the string displayed on the console when an
                              instance is configured to run in Console mode, and
                              display its own messages.

    2014/05/19 4.1     DAG    Use my spiffy new IP6CUtilLib1.GetProcessSubsystem
                              method to identify the subsystem and set a default
                              value for the Subsystem property.

                              This class is moving! This morning, it occurred to
                              me that it belongs in this library, which is at a
                              lower architectural level that allows me to break
                              the tight bond between the ApplicationHelpers and
                              ConsoleAppAids2 namespace.

                              CLASS RELOCATION NOTICE

                              It finally occurred to me that I already have a
                              class that is an ideal candidate to house these
                              classes that were the source of the tight coupling
                              between the WizardWrx.ApplicationHelpers and
                              WizardWrx.ConsoleAppAids2. Moving the tightly
                              coupled classes into this class, which was created
                              to house a single abstract base class decouples
                              both libraries from each other, in exchange fro a
                              common coupling to this class library, yielding a
                              much more robust architecture that will be easier
                              to maintain.

    2014/06/06 5.0     DAG    1) Fix the code associated with saving and
                                 restoring ErrorMessageInColor settings in
                                 configuration files, and add synchronization of
                                 old and new flag values to all read/write
                                 properties.

                              2) Correct typographical errors in the
                                 IntelliSense documentation that I discovered
                                 while chasing down the source of a type
                                 conversion exception that was thrown at run
                                 time by a class that consumes this class and
                                 its converters.

                              3) Relocate this class to a new DLLServices2
                                 assembly, which is not signed with a strong
                                 name. Like the other classes that I moved here,
                                 this one keeps its place in the namespace
                                 hierarchy.

    2014/07/20 5.1     DAG    1) Correct an oversight that left this class in
                                 the old WizardWrx.ApplicationHelpers namespace.
                                 Since this change affects only two other DLLs
                                 and, at most one user program, I took advantage
                                 of the opportunity to promote the DLLServices2
                                 namespace to the first rank under the overall
                                 WizardWrx namespace.

                              2) Add methods for saving and restoring color
                                 settings, to permit the default settings stored
                                 in the configuration file to be overridden at
                                 run time.

                              3) Account for the renaming of the methods in the
                                 color message writers for STDOUT and STDERR.

    2015/06/05 5.4     DAG    Insert missing XML documentation for argument
                              penmOutputOptions of method AddCommonElements.

    2015/06/20 5.5     DAG    Protect the static arrays of constants against
                              accidental or malicious changes.

    2015/09/01 5.7     DAG    Correct spelling errors flagged by the spelling
                              checker add-in that I recently installed into the
                              Visual Studio 2013 IDE, along with a documentation
                              XML error that I overlooked.

    2016/04/06 6.0     DAG    ExceptionLogger BREAKING CHANGE ALERT:
                              --------------------------------------

                              Simplify the ReportException interface to get the
                              name of the failing method from the TargetSite
                              property.

                              The break eliminates the second argument from all
                              method signatures.

                              To prevent breaking existing code, the old methods
                              are left, marked as obsolete, to elicit a compiler
                              warning the next time a dependent program is
                              compiled.

    2016/04/12 6.0     DAG    Replace the generic (C++ oriented) implementation
						      with an implementation optimized for the .NET
							  Framework, which eliminates the need for double-
							  checked locking in the constructor.

    2016/05/10 6.0     DAG    Undefine preprocessor symbol SHOW_TYPE_HANDLES, and
                              reinstate the message type name, which isn't there,
                              since the new table is unfinished, though it works
                              for looking up the prefixes.

	2016/05/12 6.1     DAG    Define instance and static methods that return the
                              strings reserved for reporting a run-time error
                              through console mode exception reporting routine
                              ConsoleAppAids2.ConsoleAppStateManager.ErrorExit.

                              While the static method must rely on the caller to
                              specify which of the two messages to use to report
                              a run-time exception, the instance methods can use
                              properties of the instance to make that decision,
                              subject to overriding by the calling assembly.

	2016/06/06 6.3     DAG    1) SetDefaultOptions was harmlessly setting the
                                 Method bit twice.

                              2) ReportAsDirected was printing twice on standard
                                 output unless that stream was redirected. To
                                 prevent a disastrous recursive calling loop, a
                                 new instance method, GetStandardHandleStates,
								 gets the standard handle states from the
                                 StateManager, which calls this method once both
                                 objects have been constructed.
    ============================================================================
*/

using System;
using System.Collections.Generic;
using System.Text;

/* Added by DAG */

using System.IO;

namespace WizardWrx.DLLServices2
{
    /// <summary>
    /// This Singleton class exposes methods for formatting data from instances
    /// of the System.Exception class, and commonly used derived classes, and
    /// displaying the formatted data on a console (strictly speaking, on
    /// STDOUT, which can be redirected to a file), and recording them in a
    /// Windows event log.
    ///
    /// Unlike most of the classes defined in this library, this class may be
    /// inherited.
    ///
    /// All methods of this class are thread-safe. See Remarks.
    /// </summary>
    /// <remarks>
    /// This class was developed to report exceptions in a multi threaded console
    /// application. Its methods were designed to report messages in a manner
    /// that is thread-safe, yet keeps each message intact.
    ///
    /// Both objectives are achieved fairly easily.
    ///
    /// 1) All methods use only local variables. The only class level static
    /// data is a handful of constants, which are, by definition, read only, and
    /// the private SyncRoot object used to synchronize access to the object and
    /// its properties. This satisfies the first condition of thread safety,
    /// because all methods have exclusive access to all of their data.
    ///
    /// 2) Each message is built up, line by line, by appending to an instance
    /// of a StringBuilder class, using its AppendFormat method, which behaves
    /// like the static Format method of the System.String class. Although the
    /// AppendFormat method is an instance method, since the associated instance
    /// is local, thread safety remains intact. See Reference 1.
    ///
    /// 2) A single call to TextWriter.Synchronized ( Console.Out ).WriteLine
    /// or TextWriter.Synchronized ( Console.Error ).WriteLine writes the
    /// message, all at once, onto the console, preserving its integrity. Since
    /// the only event that uses a shared resource is called once only, and that
    /// call is synchronized, the message display is intact.
    ///
    /// Since each method uses the shared resource, access to the console
    /// Standard Error (STDERR) stream, once only, and does so in a synchronized
    /// (thread-safe) way, the entire method is thread-safe.
    ///
    /// As a reminder to include WizardWrx.DLLServices2.dll in your projects, I
    /// left this class in the WizardWrx.DLLServices2 namespace. Only symbolic
    /// constants got promoted to the root namespace.
    /// </remarks>
    /// <seealso cref="PropertyDefaults"/>
	public class ExceptionLogger : GenericSingletonBase<ExceptionLogger>
    {
        #region Public Enumerations and Constants
        /// <summary>
        /// // Use these flags to control the output. There is no flag for the
        /// Message property, which always becomes part of the message.
        /// </summary>
		/// <seealso cref="ErrorExitOptions"/>
		[Flags]
        public enum OutputOptions : byte
        {
            /// <summary>
            /// Show Method Name if TRUE.
            ///
            /// If the EventLog flag is also set, the method name is always
            /// written there.
            /// </summary>
            Method = 0x01 ,

            /// <summary>
            /// Show Source (Assembly) Name if TRUE.
            ///
            /// If the EventLog flag is also set, the originating assembly name
            /// is always written there.
            /// </summary>
            Source = 0x02 ,

            /// <summary>
            /// Show Stack Trace if TRUE.
            ///
            /// If the EventLog flag is also set, the stack trace is always
            /// written there.
            /// </summary>
            Stack = 0x04 ,

            /// <summary>
            /// Post to associated event log if TRUE.
            ///
            /// The value of the event source associated with the current
            /// ExceptionLogger instance determines which event log gets the
			/// message.
            ///
            /// WARNING: Unless your event source string is already registered,
            /// the application MUST run, one time only, with full administrator
            /// privileges AND use the event source to write a message into the
            /// Windows event log in order for it to be registered with Windows,
            /// which maps it to an event log.
            ///
            /// Once the event source string is registered, the application can
            /// use it to post messages to the event log in any Windows security
            /// context.
            /// </summary>
            EventLog = 0x08 ,

            /// <summary>
            /// Write message on STDOUT if TRUE and if the application has a
            /// working console.
            ///
            /// CAUTION: Although it is technically legal to set both
            /// StandardOutput and StandardError to TRUE, this can have the
            /// unwanted consequence of generating TWO copies of the message,
            /// unless STDOUT and/or STDERR is directed to a file or if both are
            /// redirected to the SAME file.
            /// </summary>
            StandardOutput = 0x10 ,

            /// <summary>
            /// Write message on STDERR if TRUE and if the application has a
            /// working console.
            ///
            /// CAUTION: Although it is technically legal to set both
            /// StandardOutput and StandardError to TRUE, this can have the
            /// unwanted consequence of generating TWO copies of the message,
            /// unless STDOUT and/or STDERR is directed to a file or if both are
            /// redirected to the SAME file.
            /// </summary>
            StandardError = 0x20 ,

            /// <summary>
            /// Undefined - reserved for future use
            /// </summary>
            Reserved1 = 0x40 ,

            /// <summary>
            /// Undefined - reserved for future use
            /// </summary>
            Reserved2 = 0x80 ,

			/// <summary>
			/// Use this bit mask to include all auxiliary properties of the
			/// Exception (StackTrace, TargetSite, and Source) in the report,
			/// or to strip them form a set of flags.
			/// </summary>
			ActiveProperties = 0x07 ,

			/// <summary>
			/// Use this bit mask to include all auxiliary properties of the
			/// Exception (StackTrace, TargetSite, and Source) in the report,
			/// and to send the report to a Windows event log.
			/// </summary>
			ActivePropsToEventLog = 0x0F ,

			/// <summary>
			/// Use this bit mask to include all auxiliary properties of the
			/// Exception (StackTrace, TargetSite, and Source) in the report,
			/// and to send the report to the standard error stream.
			/// </summary>
			ActivePropsToStdErr = 0x27 ,

			/// <summary>
			/// Use this bit mask to include all auxiliary properties of the
			/// Exception (StackTrace, TargetSite, and Source) in the report,
			/// and to send the report to the standard output stream.
			/// </summary>
			ActivePropsToStdOut = 0x17 ,

			/// <summary>
			/// Use this bit mask to include all auxiliary properties of the
			/// Exception (StackTrace, TargetSite, and Source) in the report,
			/// and to send the report to a Windows event log and both the
			/// standard output and standard error streams. This is useful when
			/// you know that the standard output is redirected, and you want to
			/// preserve the report in the output file, in addition to having a
			/// copy displayed on the console.
			/// </summary>
			ActivePropsEverywhere = 0x3F ,

			/// <summary>
			/// Use this flag to include all auxiliary properties of the 
			/// Exception (StackTrace, TargetSite, Source, Reserved1, and
			/// Reserved2) in the report, or to strip them form a set of flags.
			/// </summary>
			AllProperties = 0xC7 ,

			/// <summary>
			/// Use this flag to include all destinations for output, or as a
			/// mask to strip them from a set of flags.
			/// </summary>
			AllDestiations = 0x38 ,

			/// <summary>
			/// This flag turns on everything, and is of no practical use, but
			/// is defined for reference, to document that every bit in the byte
			/// is accounted for.
			/// </summary>
			AllFlags = 0xFF
        }  // OutputOptions


		/// <summary>
		/// Use with the static GetSpecifiedReservedErrorMessage method or with
		/// the overloaded instance GetReservedErrorMessage method that allows
		/// callers to override the behavior dictated by its properties.
		/// </summary>
		/// <remarks>
		/// The correspondence between the naming and numbering of the members
		/// of this enumeration is by design, since the two work hand in hand
		/// internally, and the consistency should simplify writing calls to the
		/// GetReservedErrorMessage methods.
		/// </remarks>
		/// <see cref="WizardWrx.DLLServices2.ExceptionLogger.GetSpecifiedReservedErrorMessage"/>
		/// <see cref="WizardWrx.DLLServices2.ExceptionLogger.GetSpecifiedReservedErrorMessage(ExceptionLogger.ErrorExitOptions)"/>
		/// <seealso cref="OutputOptions"/>
		[Flags]
		public enum ErrorExitOptions : byte
		{
			/// <summary>
			/// Execution succeeded; return the ERROR_SUCCESS placeholder for
			/// the table of error messages.
			/// </summary>
			Succeeded = 00 ,

			/// <summary>
			/// Details of the run-time exception were reported in a Windows
			/// Event Log.
			/// </summary>
			RecordedInEventLog = 0x08 ,

			/// <summary>
			/// Details of the run-time exception were listed on STDOUT if the
			/// application has a working console.
			/// </summary>
			RecordedInStandardOutput = 0x10 ,

			/// <summary>
			/// Details of the run-time exception were listed on STDERR if the
			/// application has a working console.
			/// </summary>
			RecordedInStandardError = 0x20 ,
		}	// ErrorExitOptions


        /// <summary>
        /// Use this enumeration to disable or enable event logging.
        /// </summary>
        /// <remarks>
        /// The penmEventLoggingState argument is defined and specified in terms
        /// of this enumeration.
        /// </remarks>
        [Obsolete("Use OutputOptions.EventLog.")]
        public enum RecordinEventLog
        {
            /// <summary>
            /// By default, events are not recorded in the event log.
            /// </summary>
            Disabled ,

            /// <summary>
            /// This value enables event logging.
            /// </summary>
            Enabled
        }   // RecordinEventLog


        /// <summary>
        /// Use this enumeration to specify whether to include or omit the
        /// StackTrace from the report. See Remarks.
        /// </summary>
        /// <remarks>
        /// The penmStackTraceDisposition is specified in terms of this
        /// enumeration.
        /// </remarks>
        [Obsolete ( "Use OutputOptions.Stack." )]
        public enum StackTraceDisposition
        {
            /// <summary>
            /// Include the StackTrace.
            /// </summary>
            Include ,
            /// <summary>
            /// Omit (suppress) the StackTrace.
            /// </summary>
            Omit
        }   // StackTraceDisposition


        /// <summary>
        /// Use this enumeration to specify the subsystem (console or graphical)
        /// targeted by the application. Although I have read that the console
        /// I/O methods silently fail if called from a GUI application, I chose
        /// to skip calls to them unless the application signals explicitly that
        /// it has a console.
        /// </summary>
        [Obsolete ( "Use OutputOptions.StandardOutput or OutputOptions.StandardError." )]
        public enum Subsystem
        {
            /// <summary>
            /// The subsystem type is unknown. The class must assume that it is
            /// running WITHOUT a console.
            /// </summary>
            Unknown = 0 ,

            /// <summary>
            /// The application is running in the character mode subsystem, and
            /// it has a console.
            /// </summary>
            ///
            /// CUI is a synonym.
            Console = 1 ,

            /// <summary>
            /// The application is running in the character mode subsystem, and
            /// it has a console.
            ///
            /// Console is a synonym.
            /// </summary>
            CUI = 1 ,

            /// <summary>
            /// The application is running in the graphical subsystem. The class
            /// must assume that it is running WITHOUT a console.
            ///
            /// GUI is a synonym.
            /// </summary>
            Graphical = 2 ,

            /// <summary>
            /// The application is running in the graphical subsystem. The class
            /// must assume that it is running WITHOUT a console.
            ///
            /// Graphical is a synonym.
            /// </summary>
            GUI = 2
        }   // Subsystem

        /// <summary>
        /// The new ProcessSubsystem enumeration maps exactly to the subsystem
        /// ID returned by GetExeSubsystem.
        /// </summary>
        public enum ProcessSubsystem : uint
        {
            /// <summary>
            /// Subsystem is unknown, either because GetExeSubsystem hasn't been
            /// called, or because it returned zero and called SetLastError.
            /// </summary>
            Unknown = 0 ,

            /// <summary>
            /// Application runs in the graphical subsystem; it is a true blue
            /// Windows application.
            /// </summary>
            Windows = 2 ,

            /// <summary>
            /// Application runs in the character mode subsystem; it is a
            /// console mode (a. k. a. Character Mode) application.
            /// </summary>
            Console = 3
        }   // ProcessSubsystem


        /// <summary>
        /// This string defines a default event source ID, WizardWrx, which I
        /// register on behalf of my own applications.
        /// </summary>
        public const string WIZARDWRX_EVENT_SOURCE_ID = @"WizardWrx";
        #endregion  // Public Enumerations and Constants


        #region Static Data
		static Dictionary<System.Guid , string> s_dctKnowExceptionTypes = new Dictionary<System.Guid , string> ( )
		{  // Exception Subtype                                       Prefix to Trim
			{ typeof ( System.Exception ).GUID                   , "\r\n"                 } ,
			{ typeof ( System.ArgumentException ).GUID           , "\r\nParameter name: " } ,
			{ typeof ( System.ArgumentOutOfRangeException ).GUID , "\r\nParameter name: " } ,
			{ typeof ( System.ArgumentNullException ).GUID       , "\r\nParameter name: " } ,
			{ typeof ( System.ObjectDisposedException ).GUID     , "\r\nObject name: "    } ,
			{ typeof ( System.IO.IOException ).GUID              , "\r\n"                 } ,
			{ typeof ( System.FormatException ).GUID             , "\r\n"                 }
		};	// s_dctKnowExceptionTypes

		#if SHOW_TYPE_HANDLES
		static Type [ ] s_atypKnowExceptionTypes = new Type [ ] 
		{
			typeof ( System.Exception ) ,
			typeof ( System.ArgumentException ) ,
			typeof ( System.ArgumentOutOfRangeException ) ,
			typeof ( System.ArgumentNullException ) ,
			typeof ( System.ObjectDisposedException ) ,
			typeof ( System.IO.IOException ) ,
			typeof ( System.FormatException )
		};	// s_atypKnowExceptionTypes
		#endif	// SHOW_TYPE_HANDLES


		//  --------------------------------------------------------------------
        //  Overloads turn this off before they call the default
        //  GetTheSingleInstance method.
        //
        //  s_strDefaultEventSource is initialized if, and when, private static
        //  method GetDefaultEventSourceID is called upon to retrieve a default
        //  event source ID string from the DLL's private configuration file.
        //  --------------------------------------------------------------------

        private static bool s_fSynchronizeFlagsNow = true;
        private static string s_strDefaultEventSource = null;
		private static SyncRoot s_srCriticalSection = new SyncRoot ( typeof ( ExceptionLogger ).ToString ( ) );
        #endregion  // Static Data


        #region Instance Data Storage
        private string _strEventSource;                                         // Leave uninitialized until it is set through its property accessor.
        private MessageInColor _emsgColorsStdOut;                               // Defer initialization until requested by the property setter.
        private ErrorMessagesInColor _emsgColorsStdErr;                         // Ditto, and they are created in tandem, using the same colors.
        private ErrorMessagesInColor _emsgSpecialColorsSave;
        private OutputOptions _enmOutputOptions = SetDefaultOptions ( );
        private OutputOptions _enmSavedOptions;
        private RecordinEventLog _enmEventLoggingState = RecordinEventLog.Disabled;
        private StackTraceDisposition _enmStackTraceDisposition = StackTraceDisposition.Include;
        private Subsystem _enmSubsystem;
        private ProcessSubsystem _enmProcessSubsystem;
		private bool _fStdOutIsRedirected = false;
		private bool _fStdErrIsRedirected = false;
        #endregion  // Instance Data Storage


        #region Public Properties
        /// <summary>
        /// Along with the EventLoggingState property, this property governs
        /// recording of events in the Windows Application Event Log or in a
        /// custom event log of your choice.
        ///
        /// The value of this property is the event source ID string to use. To
        /// simplify applications, this property has a default value, defined by
        /// WIZARDWRX_EVENT_SOURCE_ID.
        ///
        /// IMPORTANT: Each event source ID string is machine specific, and each
        /// maps to one, and only one, event log, which is designated when the
        /// source is registered, as it must be before its first use.
        /// </summary>
        public string AppEventSourceID
        {
            get
            {
				lock ( s_srCriticalSection )
					return _strEventSource;
            }   // public string AppEventSourceID get method

            set
            {
				lock ( s_srCriticalSection )
                    _strEventSource = value;
            }   // public string AppEventSourceID set method
        }   // public string AppEventSourceID


        /// <summary>
        /// Set this property to cause error messages to be displayed in a
        /// distinctive pair of foreground and background colors.
        ///
        /// Unless the AppSubsystem property is Console or CUI, this property is
        /// meaningless.
        ///
        /// Unlike other properties, ErrorMessageColors must be set directly,
        /// and it may be changed at any time.
        /// </summary>
        /// <remarks>
        /// A hidden MessageInColor is maintained in tandem with this property,
        /// for use with STDERR. In this way, messages written to either STDOUT
        /// or STDERR use the same color scheme.
        /// </remarks>
        public ErrorMessagesInColor ErrorMessageColors
        {
            get { return _emsgColorsStdErr; }      // A null reference is OK, and operates as a degenerate case.

            set
            {   //  ------------------------------------------------------------
                //  The two console output channels, standard output (STDOUT)
                //  and standard error (STDERR) have functionally identical sets
                //  of output routines that write to the appropriate channel.
                //  Since the active output channel is subject to change without
                //  notice, the simplest way to implement color output is to
                //  maintain an object of each class, ready for use if needed.
                //  ------------------------------------------------------------

                if ( value != null )
                {
                    if ( value.MessageForegroundColor == value.MessageBackgroundColor )
                    {   // Turn alternate colors off if both are same.
                        _emsgColorsStdErr = null;
                        _emsgColorsStdOut = null;
                    }   // TRUE block, if ( value.MessageForegroundColor == value.MessageBackgroundColor )
                    else
                    {
                        _emsgColorsStdErr = value;

                        _emsgColorsStdOut = new MessageInColor (
                            _emsgColorsStdErr.MessageForegroundColor ,
                            _emsgColorsStdErr.MessageBackgroundColor );
                    }   // FALSE block, if ( value.MessageForegroundColor == value.MessageBackgroundColor )
                }   //  TRUE block, if ( value != null )
                else
                {   // Set both color pairs to null.
                    _emsgColorsStdErr = null;
                    _emsgColorsStdOut = null;
                }   // FALSE block, if ( value != null )
            }     // Allow object to be destroyed by setting to NULL.
        }   // public ErrorMessagesInColor ErrorMessageColors


        /// <summary>
        /// Gets or sets the new OutputOptions enumeration, which supersedes the
        /// obsolete individual flags.
        /// </summary>
        public OutputOptions OptionFlags
        {
            get
            {
                return _enmOutputOptions;
            }   // The initial value causes the console output to include everything but the stack.

            set
            {   // Synchronize the legacy flags with the OutputOptions.
                _enmOutputOptions = value;

                if ( OptionIsOn ( OutputOptions.StandardError ) || OptionIsOn ( OutputOptions.StandardOutput ) )
                    _enmSubsystem = Subsystem.Console;
                else
                    _enmSubsystem = Subsystem.Graphical;

                if ( OptionIsOn ( OutputOptions.Stack ) )
                    _enmStackTraceDisposition = StackTraceDisposition.Include;
                else
                    _enmStackTraceDisposition = StackTraceDisposition.Omit;

                if ( OptionIsOn ( OutputOptions.EventLog ) )
                    _enmEventLoggingState = RecordinEventLog.Disabled;
                else
                    _enmEventLoggingState = RecordinEventLog.Disabled;
            }  // The setter takes the input at face value, and keeps the legacy enumerations in sync.
        }   // public OutputOptions OptionFlags


        /// <summary>
        /// Along with the AppEventSourceID property, this property governs
        /// recording of events in a Windows Event Log.
        ///
        /// Since event logging is not always permissible for some application
        /// domains, such as ASP.NET applications, event logging is disabled by
        /// default.
        ///
        /// If logging is enabled, exceptions are recorded in the Application
        /// Event Log, using the event source ID string stored in the
        /// AppEventSourceID property.
        /// </summary>
        [Obsolete ( "Use OutputOptions.EventLog." )]
        public RecordinEventLog EventLoggingState
        {
            get { return _enmEventLoggingState; }
            set
            {   // Synchronize the legacy flags with the OutputOptions.
                _enmEventLoggingState = value;

                if ( _enmEventLoggingState == RecordinEventLog.Enabled )
                    _enmOutputOptions |= OutputOptions.EventLog;
                else
                    _enmOutputOptions = _enmOutputOptions & ( ~OutputOptions.EventLog );
            }  // The setter takes the input at face value, and keeps the OptionFlags in sync.
        }   // public RecordinEventLog EventLoggingState


        /// <summary>
        /// The value of this property determines whether the stack trace is
        /// included in the exception report or omitted from it.
        /// </summary>
        [Obsolete ( "Use OutputOptions.Stack." )]
        public  StackTraceDisposition AppStackTraceDisposition
        {
            get { return _enmStackTraceDisposition; }
            set
            {   // Synchronize the legacy flags with the OutputOptions.
                _enmStackTraceDisposition = value;

                if ( _enmStackTraceDisposition == StackTraceDisposition.Include )
                    _enmOutputOptions |= OutputOptions.Stack;
                else
                    _enmOutputOptions = _enmOutputOptions & ( ~OutputOptions.Stack );
            }  // The setter takes the input at face value, and keeps the OptionFlags in sync.
        }   // public  StackTraceDisposition AppStackTraceDisposition


        /// <summary>
        /// The value of this property determines whether the exception
        /// report is automatically output on the application console.
        ///
		/// Its default value suppresses output.
        /// </summary>
        [Obsolete ( "Use OutputOptions.StandardOutput and/or OutputOptions.StandardError." )]
        public Subsystem AppSubsystem
        {
            get { return _enmSubsystem; }
            set
            {   // Synchronize the legacy flags with the OutputOptions.
                _enmSubsystem = value;

                if ( _enmSubsystem == Subsystem.Console )
                    _enmOutputOptions |= OutputOptions.StandardError;
                else
                    _enmOutputOptions = _enmOutputOptions & ( ~OutputOptions.StandardError );
            }  // The setter takes the input at face value, and keeps the OptionFlags in sync.
        }   // public Subsystem AppSubsystem


        /// <summary>
        /// The value of this property is the actual subsystem ID reported by a
        /// native ("unmanaged" Win32 function, GetProcessSubsystem_WW, through
        /// P/Invoke wrapper method IP6CUtilLib1.GetProcessSubsystem.
        /// </summary>
        /// <remarks>
        /// This property uses GetExeSubsystem in lieu of GetProcessSubsystem to
        /// circumvent an anomaly in the Visual Studio debugging engine, which
        /// causes that function to return 2 (Windows GUI subsystem) when you
        /// use the Visual Studio Hosting Process, which runs in the Windows
        /// subsystem. The cause of this behavior is that the Visual Studio
        /// Hosting Process trades places with the process under study, becoming
        /// the first executable file loaded into the active process.
        /// </remarks>
        public ProcessSubsystem ApplicationSubsystem
        {
            get { return _enmProcessSubsystem; }
        }   // public ProcessSubsystem ApplicationSubsystem (Read only)
        #endregion  // Public Properties


        #region Singleton Access Methods
        /// <summary>
        /// Call this static method from anywhere to get a reference to the
        /// ExceptionLogger singleton.
        /// </summary>
        /// <returns>
        /// The return value is a reference to the singleton, which is created
        /// the first time the method is called. Subsequent calls return a
        /// reference to the singleton.
        /// </returns>
        /// <remarks>
        /// All four overloads call this method, caching the returned reference
        /// in a local variable, before they override one or more of its default
        /// property values. When all overrides have been processed, the cached
        /// reference is returned through the overload that took the call.
        ///
        /// This roundabout procedure is necessary because the properties cannot
        /// be set until the object has been created. The most straightforward
        /// way to do this is to call the default method, which performs a task
        /// usually performed by a default constructor in this implementation of
        /// the singleton design pattern.
		/// </remarks>
		public static ExceptionLogger GetTheSingleInstance ( )
		{
			if ( s_fSynchronizeFlagsNow )
				InitializeInstance ( s_genTheOnlyInstance ,
									  s_genTheOnlyInstance._enmProcessSubsystem ,
									  s_genTheOnlyInstance._enmSubsystem ,
									  s_genTheOnlyInstance._enmEventLoggingState ,
									  s_genTheOnlyInstance._enmStackTraceDisposition );
			else
				s_fSynchronizeFlagsNow = true;

			return s_genTheOnlyInstance;
		}   // public static ApplicationInstance GetTheSingleInstance (1 of 10)


		/// <summary>
        /// Call this static method from anywhere to get a reference to the
        /// ExceptionLogger singleton and set its AppSubsystem property.
        /// </summary>
        /// <param name="penmSubsystem">
        /// Use this member of the Subsystem enumeration to modify the behavior
        /// of the exception logging methods, by enabling console output if the
        /// calling application has one.
        /// </param>
        /// <returns>
        /// The return value is a reference to the singleton, which is created
        /// the first time the method is called. Subsequent calls return a
        /// reference to the singleton.
        /// </returns>
        [Obsolete ( "The Subsystem property has been superseded by the ProcessSubsystem property, and the functions formerly goverened by it have been transferred to the OutputOptions bit mask. Use OutputOptions.StandardOutput and/or OutputOptions.StandardError." )]
        public static ExceptionLogger GetTheSingleInstance ( Subsystem penmSubsystem )
        {
			s_fSynchronizeFlagsNow = false;	// Defer the call to SyncNewFlagsWithOld, so that it can accept the Subsystem.
            ExceptionLogger rTheInstance = GetTheSingleInstance ( );
            InitializeInstance ( rTheInstance ,
                                  rTheInstance._enmProcessSubsystem ,
                                  penmSubsystem ,
                                  rTheInstance._enmEventLoggingState ,
                                  rTheInstance._enmStackTraceDisposition );
            return rTheInstance;
        }   // public static ExceptionLogger GetTheSingleInstance (2 of 10)


        /// <summary>
        /// Call this static method from anywhere in your code to get a
        /// reference to the ExceptionLogger singleton and set its AppSubsystem
        /// and EventLoggingState properties.
        /// </summary>
        /// <param name="penmSubsystem">
        /// Use this member of the Subsystem enumeration to modify the behavior
        /// of the exception logging methods, by enabling console output if the
        /// calling application has one.
        /// </param>
        /// <param name="penmEventLoggingState">
        /// Use this member of the RecordinEventLog enumeration to modify the
        /// behavior of the exception logging methods by enabling or disabling
        /// recording of exceptions in the application event log.
        /// </param>
        /// <returns>
        /// The return value is a reference to the singleton, which is created
        /// the first time the method is called. Subsequent calls return a
        /// reference to the singleton.
        /// </returns>
        [Obsolete ( "The Subsystem property has been superseded by the ProcessSubsystem property, and the functions formerly goverened by it have been transferred to the OutputOptions bit mask. Use OutputOptions.StandardOutput and/or OutputOptions.StandardError." )]
        public static ExceptionLogger GetTheSingleInstance (
            Subsystem penmSubsystem ,
            RecordinEventLog penmEventLoggingState )
        {
			s_fSynchronizeFlagsNow = false;	// Defer the call to SyncNewFlagsWithOld, so that it can accept the Subsystem.
            ExceptionLogger rTheInstance = GetTheSingleInstance ( );
            InitializeInstance ( rTheInstance ,
                                  rTheInstance._enmProcessSubsystem ,
                                  penmSubsystem ,
                                  penmEventLoggingState ,
                                  rTheInstance._enmStackTraceDisposition );
            return rTheInstance;
        }   // public static ExceptionLogger GetTheSingleInstance (3 of 10)


        /// <summary>
        /// Call this static method from anywhere to get a reference to the
        /// ExceptionLogger singleton and set its AppSubsystem,
        /// EventLoggingState, AppStackTraceDisposition properties.
        /// </summary>
        /// <param name="penmSubsystem">
        /// Use this member of the Subsystem enumeration to modify the behavior
        /// of the exception logging methods, by enabling console output if the
        /// calling application has one.
        /// </param>
        /// <param name="penmEventLoggingState">
        /// Use this member of the RecordinEventLog enumeration to modify the
        /// behavior of the exception logging methods by enabling or disabling
        /// recording of exceptions in the application event log.
        /// </param>
        /// <param name="penmStackTraceDisposition">
        /// Use this member4 of the StackTraceDisposition enumeration to modify
        /// the behavior of the exception logging methods, by causing the stack
        /// trace to be either included or omitted.
        /// </param>
        /// <returns>
        /// The return value is a reference to the singleton, which is created
        /// the first time the method is called. Subsequent calls return a
        /// reference to the singleton.
        /// </returns>
        [Obsolete ( "The Subsystem property has been superseded by the ProcessSubsystem property, and the functions formerly goverened by it have been transferred to the OutputOptions bit mask. Use OutputOptions.StandardOutput and/or OutputOptions.StandardError." )]
        public static ExceptionLogger GetTheSingleInstance (
            Subsystem penmSubsystem ,
            RecordinEventLog penmEventLoggingState ,
            StackTraceDisposition penmStackTraceDisposition )
        {
			s_fSynchronizeFlagsNow = false;	// Defer the call to SyncNewFlagsWithOld, so that it can accept the Subsystem.
            ExceptionLogger rTheInstance = GetTheSingleInstance ( );
            InitializeInstance ( rTheInstance ,
                                  rTheInstance._enmProcessSubsystem ,
                                  penmSubsystem ,
                                  penmEventLoggingState ,
                                  penmStackTraceDisposition );
            return rTheInstance;
        }   // public static ExceptionLogger GetTheSingleInstance (4 of 10)


        /// <summary>
        /// Call this static method from anywhere to get a reference to the
        /// ExceptionLogger singleton and set its AppSubsystem,
        /// EventLoggingState, AppStackTraceDisposition, and AppEventSourceID
        /// properties
        /// </summary>
        /// <param name="penmSubsystem">
        /// Use this member of the Subsystem enumeration to modify the behavior
        /// of the exception logging methods, by enabling console output if the
        /// calling application has one.
        /// </param>
        /// <param name="penmEventLoggingState">
        /// Use this member of the RecordinEventLog enumeration to modify the
        /// behavior of the exception logging methods by enabling or disabling
        /// recording of exceptions in the application event log.
        /// </param>
        /// <param name="penmStackTraceDisposition">
        /// Use this member4 of the StackTraceDisposition enumeration to modify
        /// the behavior of the exception logging methods, by causing the stack
        /// trace to be either included or omitted.
        /// </param>
        /// <param name="pstrEventSourceID">
        /// Use this string to override the default event source ID,  which is
        /// WIZARDWRX_EVENT_SOURCE_ID.
        /// </param>
        /// <returns>
        /// The return value is a reference to the singleton, which is created
        /// the first time the method is called. Subsequent calls return a
        /// reference to the singleton.
        /// </returns>
		[Obsolete ( "The Subsystem property has been superseded by the ProcessSubsystem property, and the functions formerly governed by the RecordinEventLog and StackTraceDisposition have been transferred to the OutputOptions bit mask. Use OutputOptions.StandardOutput and/or OutputOptions.StandardError." )]
        public static ExceptionLogger GetTheSingleInstance (
            Subsystem penmSubsystem ,
            RecordinEventLog penmEventLoggingState ,
            StackTraceDisposition penmStackTraceDisposition ,
            string pstrEventSourceID )
        {
			s_fSynchronizeFlagsNow = false;	// Defer the call to SyncNewFlagsWithOld, so that it can accept the Subsystem.
            ExceptionLogger rTheInstance = GetTheSingleInstance ( );
            InitializeInstance ( rTheInstance ,
                                  rTheInstance._enmProcessSubsystem ,
                                  penmSubsystem ,
                                  penmEventLoggingState ,
                                  penmStackTraceDisposition );
            return rTheInstance;
        }   // public static ExceptionLogger GetTheSingleInstance (5 of 10)


        /// <summary>
        /// Call this static method from anywhere to get a reference to the
        /// ExceptionLogger singleton and set its AppSubsystem
        /// property.
        /// </summary>
        /// <param name="penmProcessSubsystem">
        /// Use this member of the ProcessSubsystem enumeration to modify the
        /// default behavior of the exception logging methods, by enabling
        /// console output if the calling application has one.
        /// </param>
        /// <returns>
        /// The return value is a reference to the singleton, which is created
        /// the first time the method is called. Subsequent calls return a
        /// reference to the singleton.
        /// </returns>
        public static ExceptionLogger GetTheSingleInstance (
            ProcessSubsystem penmProcessSubsystem )
        {
			s_fSynchronizeFlagsNow = false;	// Defer the call to SyncNewFlagsWithOld, so that it can accept the Subsystem.
            ExceptionLogger rTheInstance = GetTheSingleInstance ( );
            InitializeInstance ( rTheInstance ,
                                  penmProcessSubsystem ,
                                  rTheInstance._enmSubsystem ,
                                  rTheInstance._enmEventLoggingState ,
                                  rTheInstance._enmStackTraceDisposition );
            return rTheInstance;
        }   // public static ExceptionLogger GetTheSingleInstance (6 of 10)


        /// <summary>
        /// Call this static method from anywhere to get a reference to the
        /// ExceptionLogger singleton and set its AppSubsystem
        /// and EventLoggingState properties.
        /// </summary>
        /// <param name="penmProcessSubsystem">
        /// Use this member of the ProcessSubsystem enumeration to modify the
        /// default behavior of the exception logging methods, by enabling
        /// console output if the calling application has one.
        /// </param>
        /// <param name="penmEventLoggingState">
        /// Use this member of the RecordinEventLog enumeration to modify the
        /// behavior of the exception logging methods by enabling or disabling
        /// recording of exceptions in the application event log.
        /// </param>
        /// <returns>
        /// The return value is a reference to the singleton, which is created
        /// the first time the method is called. Subsequent calls return a
        /// reference to the singleton.
        /// </returns>
		[Obsolete ( "The Subsystem property has been superseded by the ProcessSubsystem property, and the functions formerly governed by the RecordinEventLog have been transferred to the OutputOptions bit mask. Use OutputOptions.StandardOutput and/or OutputOptions.StandardError." )]
		public static ExceptionLogger GetTheSingleInstance (
            ProcessSubsystem penmProcessSubsystem ,
            RecordinEventLog penmEventLoggingState )
        {
			s_fSynchronizeFlagsNow = false;	// Defer the call to SyncNewFlagsWithOld, so that it can accept the Subsystem.
            ExceptionLogger rTheInstance = GetTheSingleInstance ( );
            InitializeInstance ( rTheInstance ,
                                  penmProcessSubsystem ,
                                  rTheInstance._enmSubsystem ,
                                  penmEventLoggingState ,
                                  rTheInstance._enmStackTraceDisposition );
            return rTheInstance;
        }   // public static ExceptionLogger GetTheSingleInstance (7 of 10)


        /// <summary>
        /// Call this static method from anywhere to get a reference to the
        /// ExceptionLogger singleton and set its AppSubsystem,
        /// EventLoggingState, AppStackTraceDisposition properties.
        /// </summary>
        /// <param name="penmProcessSubsystem">
        /// Use this member of the ProcessSubsystem enumeration to modify the
        /// default behavior of the exception logging methods, by enabling
        /// console output if the calling application has one.
        /// </param>
        /// <param name="penmEventLoggingState">
        /// Use this member of the RecordinEventLog enumeration to modify the
        /// behavior of the exception logging methods by enabling or disabling
        /// recording of exceptions in the application event log.
        /// </param>
        /// <param name="penmStackTraceDisposition">
        /// Use this member4 of the StackTraceDisposition enumeration to modify
        /// the behavior of the exception logging methods, by causing the stack
        /// trace to be either included or omitted.
        /// </param>
        /// <returns>
        /// The return value is a reference to the singleton, which is created
        /// the first time the method is called. Subsequent calls return a
        /// reference to the singleton.
        /// </returns>
		[Obsolete ( "The functions formerly governed by the RecordinEventLog and StackTraceDisposition have been transferred to the OutputOptions bit mask. Use OutputOptions.StandardOutput and/or OutputOptions.StandardError." )]
		public static ExceptionLogger GetTheSingleInstance (
            ProcessSubsystem penmProcessSubsystem ,
            RecordinEventLog penmEventLoggingState ,
            StackTraceDisposition penmStackTraceDisposition )
        {
			s_fSynchronizeFlagsNow = false;	// Defer the call to SyncNewFlagsWithOld, so that it can accept the Subsystem.
            ExceptionLogger rTheInstance = GetTheSingleInstance ( );
            InitializeInstance ( rTheInstance ,
                                  penmProcessSubsystem ,
                                  rTheInstance._enmSubsystem ,
                                  penmEventLoggingState ,
                                  penmStackTraceDisposition );
            return rTheInstance;
        }   // public static ExceptionLogger GetTheSingleInstance (8 of 10)


        /// <summary>
        /// Call this static method from anywhere to get a reference to the
        /// ExceptionLogger singleton and set its AppSubsystem,
        /// EventLoggingState, AppStackTraceDisposition, and AppEventSourceID
        /// properties
        /// </summary>
        /// <param name="penmProcessSubsystem">
        /// Use this member of the ProcessSubsystem enumeration to modify the
        /// default behavior of the exception logging methods, by enabling
        /// console output if the calling application has one.
        /// </param>
        /// <param name="penmEventLoggingState">
        /// Use this member of the RecordinEventLog enumeration to modify the
        /// behavior of the exception logging methods by enabling or disabling
        /// recording of exceptions in the application event log.
        /// </param>
        /// <param name="penmStackTraceDisposition">
        /// Use this member4 of the StackTraceDisposition enumeration to modify
        /// the behavior of the exception logging methods, by causing the stack
        /// trace to be either included or omitted.
        /// </param>
        /// <param name="pstrEventSourceID">
        /// Use this string to override the default event source ID,  which is
        /// WIZARDWRX_EVENT_SOURCE_ID.
        /// </param>
        /// <returns>
        /// The return value is a reference to the singleton, which is created
        /// the first time the method is called. Subsequent calls return a
        /// reference to the singleton.
        /// </returns>
		[Obsolete ( "The functions formerly governed by the RecordinEventLog and StackTraceDisposition have been transferred to the OutputOptions bit mask. Use OutputOptions.StandardOutput and/or OutputOptions.StandardError." )]
		public static ExceptionLogger GetTheSingleInstance (
            ProcessSubsystem penmProcessSubsystem ,
            RecordinEventLog penmEventLoggingState ,
            StackTraceDisposition penmStackTraceDisposition ,
            string pstrEventSourceID )
        {
			s_fSynchronizeFlagsNow = false;	// Defer the call to SyncNewFlagsWithOld, so that it can accept the Subsystem.
            ExceptionLogger rTheInstance = GetTheSingleInstance ( );
            InitializeInstance ( rTheInstance ,
                                  penmProcessSubsystem ,
                                  rTheInstance._enmSubsystem ,
                                  penmEventLoggingState ,
                                  penmStackTraceDisposition );
            rTheInstance._strEventSource = pstrEventSourceID;
            return rTheInstance;
        }   // public static ExceptionLogger GetTheSingleInstance (9 of 10)


        /// <summary>
        /// Call this static method from anywhere to get a reference to the
        /// ExceptionLogger singleton and set its ProcessSubsystem,
        /// OptionFlags, and AppEventSourceID properties.
        /// </summary>
        /// <param name="penmProcessSubsystem">
        /// Use this member of the ProcessSubsystem enumeration to modify the
        /// default behavior of the exception logging methods, by enabling
        /// console output if the calling application has one.
        /// </param>
        /// <param name="penmOutputOptions">
        /// The OutputOptions enumeration is organized for use as a bit mask.
        /// Set its flags as desired to control the format and content of output
        /// generated by the ReportException methods.
        /// </param>
        /// <param name="pstrEventSourceID">
        /// Use this string to override the default event source ID,  which is
        /// WIZARDWRX_EVENT_SOURCE_ID.
        /// </param>
        /// <returns>
        /// The return value is a reference to the singleton, which is created
        /// the first time the method is called. Subsequent calls return a
        /// reference to the singleton.
        /// </returns>
        /// <remarks>
        /// This method looks entirely forward, which means that it doesn't
        /// bother with the obsolete properties.
        /// </remarks>
        public static ExceptionLogger GetTheSingleInstance (
            ProcessSubsystem penmProcessSubsystem ,
            OutputOptions penmOutputOptions ,
            string pstrEventSourceID )
        {
			s_fSynchronizeFlagsNow = false;	// Defer the call to SyncNewFlagsWithOld, so that it can accept the Subsystem.
            ExceptionLogger rTheInstance = GetTheSingleInstance ( );
            InitializeInstance ( rTheInstance ,
                                  penmProcessSubsystem ,
                                  rTheInstance._enmSubsystem ,
                                  rTheInstance._enmEventLoggingState ,
                                  rTheInstance._enmStackTraceDisposition );
            return rTheInstance;
        }   // public static ExceptionLogger GetTheSingleInstance (10 of 10)


		#if SHOW_TYPE_HANDLES
		/// <summary>
		/// The purpose of this static constructor is to force the enclosed code to run once only.
		/// </summary>
		static ExceptionLogger ( )
		{
			foreach ( Type typThisException in s_atypKnowExceptionTypes )
			{
				Console.WriteLine ( "{0}Exception type, as seen by static ExceptionLogger constructor:{0}" , Environment.NewLine );
				Console.WriteLine ( "    FullName   = {0}" , typThisException.FullName );
				Console.WriteLine ( "    Name       = {0}" , typThisException.Name );
				Console.WriteLine ( "    GUID       = {0}" , typThisException.GUID );
				Console.WriteLine ( "    TypeHandle = {0}" , typThisException.TypeHandle.Value.ToString ( NumericFormats.HEXADECIMAL_8 ) );
			}	// foreach ( Type typThisException in s_atypKnowExceptionTypes )
		}	// static ExceptionLogger
		#endif	// SHOW_TYPE_HANDLES
        #endregion  // Singleton Access Methods


        #region Public Utility Methods
        /// <summary>
        /// Restore the default exception message colors.
        /// </summary>
        /// <param name="pfWipeSavedColors">
        /// To have the colors saved by the last call to SaveCurrentColors
        /// discarded, set this argument to TRUE. Otherwise, the saved colors
        /// are preserved.
        /// </param>
        /// <returns>
        /// The return value is the reinstated default exception message colors,
        /// which may be NULL if the color scheme is invalid (both colors set to
        /// the same value) or missing (no color scheme is configured).
        /// </returns>
        public ErrorMessagesInColor RestoreDefaultColors ( bool pfWipeSavedColors )
        {
            _emsgColorsStdErr = null;
            _emsgColorsStdOut = null;

            if ( pfWipeSavedColors )
                _emsgSpecialColorsSave = null;

            SetMessageColors ( );
            return _emsgColorsStdErr;
        }   // public ErrorMessagesInColor RestoreDefaultColors


		/// <summary>
		/// Return a message suitable for display on a console to accompany a
		/// status code of ERROR_SUCCESS (zero) or ERROR_RUNTIME (one), both
		/// defined in the MagicNumbers class of standard constant definitions.
		/// </summary>
		/// <returns>
		/// Since its operation is self contained, this method should always
		/// succeed in returning an appropriate message.
		/// </returns>
		/// <seealso cref="GetSpecifiedReservedErrorMessage"/>
		public string GetReservedErrorMessage ( )
		{	// The purpose of this substitution is to discard the bits that govern what gets included in the message, leaving only the destination flags.
			return GetSpecifiedReservedErrorMessage ( ( ErrorExitOptions ) ( this._enmOutputOptions & OutputOptions.AllDestiations ) );
		}	// public string GetReservedErrorMessage (1 of 2)


		/// <summary>
		/// Return a message suitable for display on a console to accompany a
		/// status code of ERROR_SUCCESS (zero) or ERROR_RUNTIME (one), both
		/// defined in the MagicNumbers class of standard constant definitions.
		/// </summary>
		/// <param name="penmErrorExitOptions">
		/// A member of the ErrorExitOptions specifies the desired action. This
		/// value overrides the corresponding bits in the OptionFlags bit mask.
		/// 
		/// If an invalid value is specified, the returned string is an error 
		/// message that starts with "An internal error has occurred." If this
		/// happens, it should be treated as a coding error.
		/// </param>
		/// <returns>
		/// If the function succeeds, the returned message is ready to use; 
		/// appropriate substitutions have already been made. Otherwise, the
		/// return value is the error message described in the documentation
		/// of argument penmErrorExitOptions.
		/// </returns>
		public string GetReservedErrorMessage (
			ErrorExitOptions penmErrorExitOptions )
		{	// The static method does all of the work.
			return GetSpecifiedReservedErrorMessage ( penmErrorExitOptions );
		}	// public string GetReservedErrorMessage (2 of 2)


		/// <summary>
		/// Return a labeled string representation of the current OptionFlags,
		/// along with decimal and hexadecimal representations of the bit mask.
		/// </summary>
		/// <param name="pstrLabel">
		/// Specify a label to be inserted into the message. This may be the
		/// empty string, or even a null reference, to omit the label.
		/// </param>
		/// <returns>
		/// The returned string is ready to display via Console.WriteLine or
		/// MessageBox.Show.
		/// </returns>
		public string OutputOptionsDisplay ( string pstrLabel )
		{
			return string.Format (
				Properties.Resources.OUTPUT_OPTIONS_DISPLAY_FORMAT ,								// Format control string.
				new object [ ] {
					string.IsNullOrEmpty ( pstrLabel )												// Test for null or empty pstrLabel
						? SpecialStrings.EMPTY_STRING :												// If so, Format Item 0 = the empty string
						string.Concat ( pstrLabel ,													// Otherwise, Format Item 0 = descriptive text, followed by a single space
						                SpecialCharacters.SPACE_CHAR ) ,							// Format Item 0 = Optional descriptive text, which may be the empty string or a null reference, which is treated the empty string
					( ( int ) _enmOutputOptions ).ToString ( NumericFormats.HEXADECIMAL_2 ) ,		// Format Item 1 = hexadecimal representation of the value
					( int ) _enmOutputOptions ,														// Format Item 2 = decimal representation of the value
					_enmOutputOptions.ToString ( )													// Format Item 3 = enumeration representation
				} );
		}	// public string OutputOptionsDisplay


		/// <summary>
		/// Turn the specified bit in the OutputOptions bit mask OFF.
		/// </summary>
		/// <param name="penmOutputOptions">
		/// Specify the member of the OutputOptions enumerated type to turn OFF.
		/// </param>
		/// <returns>
		/// The return value is the updated OutputOptions enumerated type, which
		/// is organized as a bit mask.
		/// </returns>
		public OutputOptions OutputOptionTurnOff ( OutputOptions penmOutputOptions )
		{
			_enmOutputOptions = _enmOutputOptions & ( ~penmOutputOptions );
			return _enmOutputOptions;
		}	// public OutputOptions OutputOptionTurnOff


		/// <summary>
		/// Turn the specified bit in the OutputOptions bit mask ON.
		/// </summary>
		/// <param name="penmOutputOptions">
		/// Specify the member of the OutputOptions enumerated type to turn ON.
		/// </param>
		/// <returns>
		/// The return value is the updated OutputOptions enumerated type, which
		/// is organized as a bit mask.
		/// </returns>
		public OutputOptions OutputOptionTurnOn ( OutputOptions penmOutputOptions )
		{
			_enmOutputOptions = _enmOutputOptions | penmOutputOptions;
			return _enmOutputOptions;
		}	// public OutputOptions OutputOptionTurnOn


        /// <summary>
        /// Restore the state of the OutputOptions flags to their initial
        /// (default) values.
        /// </summary>
        /// <returns>
        /// The return value is the reinstated property value.
        /// </returns>
        /// <remarks>
        /// This routine calls the same static SetDefaultOptions method used by
        /// the static initializer, so that the defaults can be changed by
        /// visiting just one routine.
        /// </remarks>
        public OutputOptions RestoreDefaultOptions ( )
        {
            _enmOutputOptions = SetDefaultOptions ( );
            return _enmOutputOptions;
        }   // public OutputOptions RestoreDefaultOptions


        /// <summary>
        /// Restore the state of the OutputOptions flags from the copy saved by
        /// the last SaveCurrentOptions method call.
        /// </summary>
		/// <returns>
		/// This method returns the options that were just restored, so that
		/// callers can sanity check them against the expected settings.
		/// </returns>
        /// <remarks>
        /// CAUTION: Unless this method is preceded by a call to
        /// SaveCurrentOptions, this call clears all flags, not just back to
        /// their initial state, but truly clear - all flags OFF.
        /// </remarks>
        public OutputOptions RestoreSavedOptions ( )
        {
            _enmOutputOptions = _enmSavedOptions;
			return _enmOutputOptions;
        }   // public void RestoreSavedOptions


        /// <summary>
        /// Restore the ErrorMessageColors from the copy saved by the last
        /// SaveCurrentColors method call.
        /// </summary>
		/// <returns>
		/// This method returns the restored message colors, so that callers may
		/// sanity check them against the expected values.
		/// </returns>
        /// <remarks>
        /// CAUTION: Unless this method is preceded by a call to
        /// SaveCurrentColors, this call completely disables color error
        /// messages, unless the static initializer set default colors from a
        /// configuration file.
        /// </remarks>
        public ErrorMessagesInColor RestoreSavedColors ( )
        {   // Let the property setter do the work.
            ErrorMessageColors = _emsgSpecialColorsSave;
			return _emsgSpecialColorsSave;
        }   // public void RestoreSavedColors


        /// <summary>
        /// Save a copy of the current colors defined by the ErrorMessageColors
        /// property into a private area reserved for the purpose.
        /// </summary>
        /// <returns>
        /// The current settings (the settings just saved) are returned.
        /// </returns>
        public ErrorMessagesInColor SaveCurrentColors ( )
        {
            _emsgSpecialColorsSave = _emsgColorsStdErr;
            return _emsgColorsStdErr;
        }   // public ErrorMessagesInColor SaveCurrentColors


        /// <summary>
        /// Save a copy of the current state of the OutputOptions flags into a
        /// private area reserved for the purpose.
        /// </summary>
        /// <returns>
        /// The current settings (the settings just saved) are returned.
        /// </returns>
        public OutputOptions SaveCurrentOptions ( )
        {
            _enmSavedOptions = _enmOutputOptions;
            return _enmOutputOptions;
        }   // public OutputOptions SaveCurrentOptions
        #endregion  // Public Utility Methods


        #region Public Static Utility Methods
		/// <summary>
		/// Return a message suitable for display on a console to accompany a
		/// status code of ERROR_SUCCESS (zero) or ERROR_RUNTIME (one), both
		/// defined in the MagicNumbers class of standard constant definitions.
		/// 
		/// Call this method with penmErrorExitOptions equal to Succeeded to get
		/// the ERROR_SUCCESS placeholder string for your error message table.
		/// </summary>
		/// <param name="penmErrorExitOptions">
		/// A member of the ErrorExitOptions specifies the desired action. Since
		/// this is a static method, and doesn't have access to the instance
		/// properties, this value substitutes for the corresponding bits in the
		/// OptionFlags bit mask.
		/// 
		/// If an invalid value is specified, the returned string is an error 
		/// message that starts with "An internal error has occurred." If this
		/// happens, it should be treated as a coding error.
		/// </param>
		/// <returns>
		/// If the function succeeds, the returned message is ready to use; 
		/// appropriate substitutions have already been made. Otherwise, the
		/// return value is the error message described in the documentation
		/// of argument penmErrorExitOptions.
		/// </returns>
		public static string GetSpecifiedReservedErrorMessage (
			ErrorExitOptions penmErrorExitOptions )
		{
			switch ( penmErrorExitOptions )
			{ 
				case ErrorExitOptions.Succeeded:
					return SpecialStrings.ERRMSG_SUCCESS_PLACEHOLDER;
				case ErrorExitOptions.RecordedInEventLog:
					return Properties.Resources.ERRMSG_RUNTIME_SEE_EVENT_LOG;

				case ErrorExitOptions.RecordedInStandardError:
				case ErrorExitOptions.RecordedInStandardOutput:
					return string.Format (
						Properties.Resources.ERRMSG_RUNTIME_SEE_MESSAGE_ABOVE ,	// Format Control String
						Environment.NewLine );									// Format Item 0 = Embedded Newline

				default:
					return string.Format (
						Properties.Resources.ERRMSG_INVALID_ERROREXITOPTIONS ,	// Format Control String
						( int ) penmErrorExitOptions ,							// Format Item 0 = Integer value of penmErrorExitOptions
						Environment.NewLine );									// Format Item 1 = Embedded Newline
			}	// switch ( penmErrorExitOptions )
		}	// public static string GetSpecifiedReservedErrorMessage


        /// <summary>
        /// Append a message to a standard ISO-8601 time stamp.
        /// </summary>
        /// <param name="pstrMessage">
        /// Specify the message to record.
        /// </param>
        /// <remarks>
        /// Though written ostensibly for internal use, I marked this method as
        /// public because it will quickly find employment outside this library.
        /// </remarks>
        public static void TimeStampedTraceWrite ( string pstrMessage )
        {
            System.Diagnostics.Trace.WriteLine (
                string.Format (
                    "{0}: {1}" ,
                    DisplayFormats.FormatDateTimeForShow ( DateTime.Now ) ,
                pstrMessage ) );
            System.Diagnostics.Trace.Flush ( );
        }   // public static void TimeStampedTraceWrite
        #endregion  // Public Static Utility Methods


		#region Public ReportException Methods, Simplified
		/// <summary>
		/// Format and report the properties of a generic Exception on a console
		/// in a thread-safe manner.
		/// </summary>
		/// <param name="perrAny">
		/// The instance of the base Exception class to process. See Remarks.
		/// </param>
		/// <remarks>
		/// This can be ANY exception type, although the intent is to limit its
		/// use to reporting exceptions thrown by the base class,
		/// System.Exception.
		///
		/// Other overloads exist for reporting exceptions thrown by types
		/// derived from System.Exception.
		///
		/// The TargetSite property, exposed by descendants of System.Exception,
		/// is the name of the method in which the exception was thrown.
		/// </remarks>
		public string ReportException ( Exception perrAny )
		{
			StringBuilder sbMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );
			StringBuilder sbLogMsg = CreateForEventLog ( _enmOutputOptions & OutputOptions.EventLog );

			sbMsg.Append (
				ReformatExceptionMessage (
					perrAny ,
					perrAny.TargetSite.Name ,
					Properties.Resources.ERRMSG_EX_MSG_TPL ) );

			if ( UseEventLog ( _enmOutputOptions ) )
				sbLogMsg.Append (
					ReformatExceptionMessage (
						perrAny ,
						perrAny.TargetSite.Name ,
						WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_EVTMSG_TPL ) );

			return AddCommonElementsReportAndReturn (
				perrAny ,
				sbMsg , 
				sbLogMsg );
		}   // ReportException method (1 of 7 - Exception)


		/// <summary>
		/// Format and report the properties of an ArgumentException exception on
		/// a console in a thread-safe manner.
		/// </summary>
		/// <param name="perrBadArg">
		/// The instance of the ArgumentException exception to process. See
		/// Remarks.
		/// </param>
		/// <remarks>
		/// Although this method can process objects of ANY class which derives
		/// from ArgumentException, other methods of this class specialize in
		/// processing objects of the commonly used ArgumentOutOfRangeException
		/// and ArgumentNullException derived classes.
		///
		/// The TargetSite property, exposed by descendants of System.Exception,
		/// is the name of the method in which the exception was thrown.
		/// </remarks>
		public string ReportException ( ArgumentException perrBadArg )
		{
			StringBuilder sbMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );
			StringBuilder sbLogMsg = CreateForEventLog ( _enmOutputOptions & OutputOptions.EventLog );

			sbMsg.Append (
				ReformatExceptionMessage (
					perrBadArg ,
					perrBadArg.TargetSite.Name ,
					WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_MSG_TPL ) );
			sbMsg.AppendFormat (
				WizardWrx.DLLServices2.Properties.Resources.ERRMSG_ARGNAME_TPL ,        // Format String
				perrBadArg.ParamName ,                                                  // Token Index 0
				Environment.NewLine );                                                  // Token Index 1

			if ( UseEventLog ( _enmOutputOptions ) )
			{
				sbLogMsg.Append (
					ReformatExceptionMessage (
						perrBadArg ,
						perrBadArg.TargetSite.Name ,
						WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_EVTMSG_TPL ) );
				sbLogMsg.AppendFormat (
					WizardWrx.DLLServices2.Properties.Resources.ERRMSG_ARGNAME_TPL ,    // Format String
					perrBadArg.ParamName ,                                              // Token Index 0
					Environment.NewLine );                                              // Token Index 1
			}   // if ( UseEventLog ( _enmOutputOptions ) )

			return AddCommonElementsReportAndReturn (
				perrBadArg ,
				sbMsg ,
				sbLogMsg );
		}   // ReportException method (2 of 7 - Exception)


		/// <summary>
		/// Format and report the properties of an ArgumentOutOfRangeException
		/// exception on a console in a thread-safe manner.
		/// </summary>
		/// <param name="perrBadArg">
		/// The instance of the ArgumentOutOfRangeException class to process.
		/// </param>
		/// <remarks>
		/// The TargetSite property, exposed by descendants of System.Exception,
		/// is the name of the method in which the exception was thrown.
		/// </remarks>
		public string ReportException ( ArgumentOutOfRangeException perrBadArg )
		{
			StringBuilder sbMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );
			StringBuilder sbLogMsg = CreateForEventLog ( _enmOutputOptions & OutputOptions.EventLog );

			sbMsg.Append (
				ReformatExceptionMessage (
					perrBadArg ,
					perrBadArg.TargetSite.Name ,
					WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_MSG_TPL ) );
			sbMsg.AppendFormat (
				WizardWrx.DLLServices2.Properties.Resources.ERRMSG_ARGNAME_TPL ,        // Format String
				perrBadArg.ParamName ,                                                  // Token Index 0
				Environment.NewLine );                                                  // Token Index 1
			sbMsg.AppendFormat (
				WizardWrx.DLLServices2.Properties.Resources.ERRMSG_ARGVALUE_TPL ,       // Format String
				perrBadArg.ActualValue ,                                                // Token Index 0
				Environment.NewLine );                                                  // Taken Index 1

			if ( UseEventLog ( _enmOutputOptions ) )
			{
				sbLogMsg.Append (
					ReformatExceptionMessage (
						perrBadArg ,
						perrBadArg.TargetSite.Name ,
						WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_EVTMSG_TPL ) );
				sbLogMsg.AppendFormat (
					WizardWrx.DLLServices2.Properties.Resources.ERRMSG_ARGNAME_TPL ,    // Format String
					perrBadArg.ParamName ,                                              // Token Index 0
					Environment.NewLine );                                              // Token Index 1
				sbLogMsg.AppendFormat (
					WizardWrx.DLLServices2.Properties.Resources.ERRMSG_ARGVALUE_TPL ,   // Format String
					perrBadArg.ActualValue ,                                            // Token Index 0
					Environment.NewLine );                                              // Taken Index 1
			}   // if ( UseEventLog ( _enmOutputOptions ) )

			return AddCommonElementsReportAndReturn (
				perrBadArg ,
				sbMsg ,
				sbLogMsg );
		}   // ReportException method (3 of 7 - ArgumentOutOfRangeException)


		/// <summary>
		/// Format and report the properties of an ArgumentNullException
		/// exception on a console in a thread-safe manner. See Remarks.
		/// </summary>
		/// <param name="perrNullArg">
		/// The instance of an ArgumentNullException exception to process.
		/// </param>
		/// <remarks>
		/// The TargetSite property, exposed by descendants of System.Exception,
		/// is the name of the method in which the exception was thrown.
		/// </remarks>
		public string ReportException ( ArgumentNullException perrNullArg )
		{
			StringBuilder sbMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );
			StringBuilder sbLogMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );

			sbMsg.Append (
				ReformatExceptionMessage (
					perrNullArg ,
					perrNullArg.TargetSite.Name ,
					WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_MSG_TPL ) );
			sbMsg.AppendFormat (
				WizardWrx.DLLServices2.Properties.Resources.ERRMSG_ARGNAME_TPL ,        // Format String
				perrNullArg.ParamName ,                                                 // Token Index 0
				Environment.NewLine );                                                  // Token Index 1

			if ( UseEventLog ( _enmOutputOptions ) )
			{   // Generate the separate message for the event log, which always includes the stack trace.
				sbLogMsg.Append (
					ReformatExceptionMessage (
						perrNullArg ,
						perrNullArg.TargetSite.Name ,
						WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_EVTMSG_TPL ) );
				sbLogMsg.AppendFormat (
					WizardWrx.DLLServices2.Properties.Resources.ERRMSG_ARGNAME_TPL ,    // Format String
					perrNullArg.ParamName ,                                             // Token Index 0
					Environment.NewLine );                                              // Token Index 1
			}   // if ( UseEventLog ( _enmOutputOptions ) )

			return AddCommonElementsReportAndReturn (
				perrNullArg ,
				sbMsg ,
				sbLogMsg );
		}   // ReportException method (4 of 7 - ArgumentNullException)


		/// <summary>
		/// Format and report the properties of an ObjectDisposedException
		/// exception on a console in a thread-safe manner. See Remarks.
		/// </summary>
		/// <param name="perrDisposed">
		/// The instance of the ObjectDisposedException Exception class to
		/// process.
		/// </param>
		/// <remarks>
		/// Any process that throws an ObjectDisposedException exception is in
		/// serious trouble, and deserves to crash, and be investigated, because
		/// this type of exception is almost invariably caused by a programming
		/// logic error.
		///
		/// The TargetSite property, exposed by descendants of System.Exception,
		/// is the name of the method in which the exception was thrown.
		/// </remarks>
		public string ReportException ( ObjectDisposedException perrDisposed )
		{
			StringBuilder sbMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );
			StringBuilder sbLogMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );

			sbMsg.Append (
				ReformatExceptionMessage (
					perrDisposed ,
					perrDisposed.TargetSite.Name ,
					WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_MSG_TPL ) );
			sbMsg.AppendFormat (
				WizardWrx.DLLServices2.Properties.Resources.ERRMSG_OBJNAME_TPL ,
				perrDisposed.ObjectName ,
				Environment.NewLine );

			if ( UseEventLog ( _enmOutputOptions ) )
			{   // Generate the separate message for the event log, which always includes the stack trace.
				sbLogMsg.Append (
					ReformatExceptionMessage (
						perrDisposed ,
						perrDisposed.TargetSite.Name ,
						WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_EVTMSG_TPL ) );
				sbLogMsg.AppendFormat (
					WizardWrx.DLLServices2.Properties.Resources.ERRMSG_OBJNAME_TPL ,
					perrDisposed.ObjectName ,
					Environment.NewLine );
			}   // if ( UseEventLog ( _enmOutputOptions ) )

			return AddCommonElementsReportAndReturn (
				perrDisposed ,
				sbMsg ,
				sbLogMsg );
		}   // ReportException method (5 of 7 - ObjectDisposedException)


		/// <summary>
		/// Format and report the properties of an IOException exception on a
		/// console in a thread-safe manner.
		/// </summary>
		/// <param name="perrIOError">
		/// The instance of the IOException class to process. See Remarks.
		/// </param>
		/// <param name="pfi">
		/// The FileInfo object makes available much more than the file name,
		/// allowing for the possibility of an override to deliver more detailed
		/// information about the file being processed.
		/// </param>
		/// <remarks>
		/// This routine processes ANY exception of the IOException class and
		/// its derivatives. Methods for parsing published derived classes are
		/// somewhere on my ToDo list.
		///
		/// The TargetSite property, exposed by descendants of System.Exception,
		/// is the name of the method in which the exception was thrown.
		/// </remarks>
		public string ReportException (
			IOException perrIOError ,
			FileInfo pfi )
		{
			StringBuilder sbMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );
			StringBuilder sbLogMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );

			sbMsg.Append (
				ReformatExceptionMessage (
					perrIOError ,
					perrIOError.TargetSite.Name ,
					WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_MSG_TPL ) );

			{	// Keep the EnhancedIOException exception just long enough to grab the HRESULT from it.
				EnhancedIOException enhIOException = EnhancedIOException.SubclassIOException ( perrIOError );
				sbMsg.AppendFormat (
					Properties.Resources.ERRMSG_HRESULT ,											// This message template displays hexadecimal and decimal representations of an HRESULT.
					enhIOException.HRESULT.ToString ( NumericFormats.HEXADECIMAL_8 ) ,				// Format Item 0 renders a hexadecimal representation of the HRESULT.
					enhIOException.HRESULT.ToString ( NumericFormats.DECIMAL ) , 					// Format Item 1 renders a decimal representation of the HRESULT.
					Environment.NewLine );															// My method of rendering a newline keeps it out of the managed resources.
			}	// Let the enhIOException go out of scope.

			sbMsg.AppendFormat (
				WizardWrx.DLLServices2.Properties.Resources.ERRMSG_FILENAME_TPL ,
				pfi.Name ,
				Environment.NewLine );

			if ( UseEventLog ( _enmOutputOptions ) )
			{   // Generate the separate message for the event log, which always includes the stack trace.
				sbLogMsg.Append (
					ReformatExceptionMessage (
						perrIOError ,
						perrIOError.TargetSite.Name ,
						WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_EVTMSG_TPL ) );
				sbLogMsg.AppendFormat (
					WizardWrx.DLLServices2.Properties.Resources.ERRMSG_FILENAME_TPL ,
					pfi.FullName ,
					Environment.NewLine );
			}   // if ( UseEventLog ( _enmOutputOptions ) )

			return AddCommonElementsReportAndReturn (
				perrIOError ,
				sbMsg ,
				sbLogMsg );
		}   // ReportException method (6 of 7 - IOException)


		/// <summary>
		/// Format and report the properties of an FormatException exception on
		/// a console in a thread-safe manner.
		/// </summary>
		/// <param name="perrrBadFormat">
		/// The instance of the FormatException class to process.
		/// </param>
		/// <param name="pstrFormatString">
		/// This should be the format string which caused the exception. There
		/// should be a way to feed this to the exception, or recover it, but I
		/// have yet to find it.
		/// </param>
		/// <remarks>
		/// The TargetSite property, exposed by descendants of System.Exception,
		/// is the name of the method in which the exception was thrown.
		/// </remarks>
		public string ReportException (
			FormatException perrrBadFormat ,
			string pstrFormatString )
		{
			StringBuilder sbMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );
			StringBuilder sbLogMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );

			sbMsg.Append (
				ReformatExceptionMessage (
					perrrBadFormat ,
					perrrBadFormat.TargetSite.Name ,
					WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_MSG_TPL ) );
			sbMsg.AppendFormat (
				WizardWrx.DLLServices2.Properties.Resources.ERRMSG_FORMATSTRING_TPL ,
				perrrBadFormat.TargetSite.Name ,
				Environment.NewLine );

			if ( UseEventLog ( _enmOutputOptions ) )
			{   // Generate the separate message for the event log, which always includes the stack trace.
				sbLogMsg.Append (
					ReformatExceptionMessage (
						perrrBadFormat ,
						perrrBadFormat.TargetSite.Name ,
						WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_EVTMSG_TPL ) );
				sbLogMsg.AppendFormat (
					WizardWrx.DLLServices2.Properties.Resources.ERRMSG_FORMATSTRING_TPL ,
					perrrBadFormat.TargetSite.Name ,
					Environment.NewLine );
			}   // if ( UseEventLog ( _enmOutputOptions ) )

			return AddCommonElementsReportAndReturn (
				perrrBadFormat ,
				sbMsg ,
				sbLogMsg );
		}   // ReportException method (7 of 7 - FormatException)
		#endregion	// Public ReportException Methods, Simplified


		#region Public ReportException Methods, Deprecated
		/// <summary>
        /// Format and report the properties of a generic Exception on a console
        /// in a thread-safe manner.
        /// </summary>
        /// <param name="perrAny">
        /// The instance of the base Exception class to process. See Remarks.
        /// </param>
        /// <param name="pstrCurrMethodName">
        /// This should be the name of the method in which the error is caught
        /// and reported. See Remarks.
        /// </param>
        /// <remarks>
        /// This can be ANY exception type, although the intent is to limit its
        /// use to reporting exceptions thrown by the base class,
        /// System.Exception.
        ///
        /// Other overloads exist for reporting exceptions thrown by types
        /// derived from System.Exception.
        ///
        /// The TargetSite property, exposed by descendants of System.Exception,
        /// is the name of the method in which the exception was thrown.
        /// </remarks>
		[Obsolete ( "ReportException methods that take string pstrCurrMethodName are being retired in favor of a simplified method signature that the same information from the TargetSite property of the exception. These methods will eventually be removed." )]
		public string ReportException (
            Exception perrAny ,
            string pstrCurrMethodName )
        {
            StringBuilder sbMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );
            StringBuilder sbLogMsg = CreateForEventLog ( _enmOutputOptions & OutputOptions.EventLog );

            sbMsg.Append (
                ReformatExceptionMessage (
                    perrAny ,
                    pstrCurrMethodName ,
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_MSG_TPL) );

            if ( UseEventLog ( _enmOutputOptions ) )
                sbLogMsg.Append (
                    ReformatExceptionMessage (
                        perrAny ,
                        pstrCurrMethodName ,
                        WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_EVTMSG_TPL ) );

			return AddCommonElementsReportAndReturn (
				perrAny ,
				sbMsg ,
				sbLogMsg );
		}   // ReportException method (1 of 7 - Exception)


        /// <summary>
        /// Format and report the properties of an ArgumentException exception on
        /// a console in a thread-safe manner.
        /// </summary>
        /// <param name="perrBadArg">
        /// The instance of the ArgumentException exception to process. See
        /// Remarks.
        /// </param>
        /// <param name="pstrCurrMethodName">
        /// This should be the name of the method in which the error is caught
        /// and reported. See Remarks.
        /// </param>
        /// <remarks>
        /// Although this method can process objects of ANY class which derives
        /// from ArgumentException, other methods of this class specialize in
        /// processing objects of the commonly used ArgumentOutOfRangeException
        /// and ArgumentNullException derived classes.
        ///
        /// The TargetSite property, exposed by descendants of System.Exception,
        /// is the name of the method in which the exception was thrown.
        /// </remarks>
		[Obsolete ( "ReportException methods that take string pstrCurrMethodName are being retired in favor of a simplified method signature that the same information from the TargetSite property of the exception. These methods will eventually be removed." )]
		public string ReportException (
            ArgumentException  perrBadArg ,
            string pstrCurrMethodName )
        {
            StringBuilder sbMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );
            StringBuilder sbLogMsg = CreateForEventLog ( _enmOutputOptions & OutputOptions.EventLog );

            sbMsg.Append (
                ReformatExceptionMessage (
                    perrBadArg ,
                    pstrCurrMethodName ,
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_MSG_TPL ) );
            sbMsg.AppendFormat (
                WizardWrx.DLLServices2.Properties.Resources.ERRMSG_ARGNAME_TPL ,        // Format String
                perrBadArg.ParamName ,                                                  // Token Index 0
                Environment.NewLine );                                                  // Token Index 1

            if ( UseEventLog ( _enmOutputOptions ) )
            {
                sbLogMsg.Append (
                    ReformatExceptionMessage (
                        perrBadArg ,
                        pstrCurrMethodName ,
                        WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_EVTMSG_TPL ) );
                sbLogMsg.AppendFormat (
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_ARGNAME_TPL ,    // Format String
                    perrBadArg.ParamName ,                                              // Token Index 0
                    Environment.NewLine );                                              // Token Index 1
            }   // if ( UseEventLog ( _enmOutputOptions ) )

			return AddCommonElementsReportAndReturn (
				perrBadArg ,
				sbMsg ,
				sbLogMsg );
		}   // ReportException method (2 of 7 - Exception)


        /// <summary>
        /// Format and report the properties of an ArgumentOutOfRangeException
        /// exception on a console in a thread-safe manner.
        /// </summary>
        /// <param name="perrBadArg">
        /// The instance of the ArgumentOutOfRangeException class to process.
        /// </param>
        /// <param name="pstrCurrMethodName">
        /// This should be the name of the method in which the error is caught
        /// and reported. See Remarks.
        /// </param>
        /// <remarks>
        /// The TargetSite property, exposed by descendants of System.Exception,
        /// is the name of the method in which the exception was thrown.
        /// </remarks>
		[Obsolete ( "ReportException methods that take string pstrCurrMethodName are being retired in favor of a simplified method signature that the same information from the TargetSite property of the exception. These methods will eventually be removed." )]
		public string ReportException (
            ArgumentOutOfRangeException perrBadArg ,
            string pstrCurrMethodName )
        {
            StringBuilder sbMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );
            StringBuilder sbLogMsg = CreateForEventLog ( _enmOutputOptions & OutputOptions.EventLog );

            sbMsg.Append (
                ReformatExceptionMessage (
                    perrBadArg ,
                    pstrCurrMethodName ,
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_MSG_TPL ) );
            sbMsg.AppendFormat (
                WizardWrx.DLLServices2.Properties.Resources.ERRMSG_ARGNAME_TPL ,        // Format String
                perrBadArg.ParamName ,                                                  // Token Index 0
                Environment.NewLine );                                                  // Token Index 1
            sbMsg.AppendFormat (
                WizardWrx.DLLServices2.Properties.Resources.ERRMSG_ARGVALUE_TPL ,       // Format String
                perrBadArg.ActualValue ,                                                // Token Index 0
                Environment.NewLine );                                                  // Taken Index 1

            if ( UseEventLog ( _enmOutputOptions ) )
            {
                sbLogMsg.Append (
                    ReformatExceptionMessage (
                        perrBadArg ,
                        pstrCurrMethodName ,
                        WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_EVTMSG_TPL ) );
                sbLogMsg.AppendFormat (
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_ARGNAME_TPL ,    // Format String
                    perrBadArg.ParamName ,                                              // Token Index 0
                    Environment.NewLine );                                              // Token Index 1
                sbLogMsg.AppendFormat (
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_ARGVALUE_TPL ,   // Format String
                    perrBadArg.ActualValue ,                                            // Token Index 0
                    Environment.NewLine );                                              // Taken Index 1
            }   // if ( UseEventLog ( _enmOutputOptions ) )

			return AddCommonElementsReportAndReturn (
				perrBadArg ,
				sbMsg ,
				sbLogMsg );
		}   // ReportException method (3 of 7 - ArgumentOutOfRangeException)


        /// <summary>
        /// Format and report the properties of an ArgumentNullException
        /// exception on a console in a thread-safe manner. See Remarks.
        /// </summary>
        /// <param name="perrNullArg">
        /// The instance of an ArgumentNullException exception to process.
        /// </param>
        /// <param name="pstrCurrMethodName">
        /// This should be the name of the method in which the error is caught
        /// and reported. See Remarks.
        /// </param>
        /// <remarks>
        /// The TargetSite property, exposed by descendants of System.Exception,
        /// is the name of the method in which the exception was thrown.
        /// </remarks>
		[Obsolete ( "ReportException methods that take string pstrCurrMethodName are being retired in favor of a simplified method signature that the same information from the TargetSite property of the exception. These methods will eventually be removed." )]
		public string ReportException (
            ArgumentNullException perrNullArg ,
            string pstrCurrMethodName )
        {
            StringBuilder sbMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );
            StringBuilder sbLogMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );

            sbMsg.Append (
                ReformatExceptionMessage (
                    perrNullArg ,
                    pstrCurrMethodName ,
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_MSG_TPL ) );
            sbMsg.AppendFormat (
                WizardWrx.DLLServices2.Properties.Resources.ERRMSG_ARGNAME_TPL ,        // Format String
                perrNullArg.ParamName ,                                                 // Token Index 0
                Environment.NewLine );                                                  // Token Index 1

            if ( UseEventLog ( _enmOutputOptions ) )
            {   // Generate the separate message for the event log, which always includes the stack trace.
                sbLogMsg.Append (
                    ReformatExceptionMessage (
                        perrNullArg ,
                        pstrCurrMethodName ,
                        WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_EVTMSG_TPL ) );
                sbLogMsg.AppendFormat (
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_ARGNAME_TPL ,    // Format String
                    perrNullArg.ParamName ,                                             // Token Index 0
                    Environment.NewLine );                                              // Token Index 1
            }   // if ( UseEventLog ( _enmOutputOptions ) )

			return AddCommonElementsReportAndReturn (
				perrNullArg ,
				sbMsg ,
				sbLogMsg );
		}   // ReportException method (4 of 7 - ArgumentNullException)


        /// <summary>
        /// Format and report the properties of an ObjectDisposedException
        /// exception on a console in a thread-safe manner. See Remarks.
        /// </summary>
        /// <param name="perrDisposed">
        /// The instance of the ObjectDisposedException Exception class to
        /// process.
        /// </param>
        /// <param name="pstrCurrMethodName">
        /// This should be the name of the method in which the error is caught
        /// and reported. See Remarks.
        /// </param>
        /// <remarks>
        /// Any process that throws an ObjectDisposedException exception is in
        /// serious trouble, and deserves to crash, and be investigated, because
        /// this type of exception is almost invariably caused by a programming
        /// logic error.
        ///
        /// The TargetSite property, exposed by descendants of System.Exception,
        /// is the name of the method in which the exception was thrown.
        /// </remarks>
		[Obsolete ( "ReportException methods that take string pstrCurrMethodName are being retired in favor of a simplified method signature that the same information from the TargetSite property of the exception. These methods will eventually be removed." )]
		public string ReportException (
            ObjectDisposedException perrDisposed ,
            string pstrCurrMethodName )
        {
            StringBuilder sbMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );
            StringBuilder sbLogMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );

            sbMsg.Append (
                ReformatExceptionMessage (
                    perrDisposed ,
                    pstrCurrMethodName ,
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_MSG_TPL ) );
            sbMsg.AppendFormat (
                WizardWrx.DLLServices2.Properties.Resources.ERRMSG_OBJNAME_TPL ,
                perrDisposed.ObjectName ,
                Environment.NewLine );

            if ( UseEventLog ( _enmOutputOptions ) )
            {   // Generate the separate message for the event log, which always includes the stack trace.
                sbLogMsg.Append (
                    ReformatExceptionMessage (
                        perrDisposed ,
                        pstrCurrMethodName ,
                        WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_EVTMSG_TPL ) );
                sbLogMsg.AppendFormat (
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_OBJNAME_TPL ,
                    perrDisposed.ObjectName ,
                    Environment.NewLine );
            }   // if ( UseEventLog ( _enmOutputOptions ) )

			return AddCommonElementsReportAndReturn (
				perrDisposed ,
				sbMsg ,
				sbLogMsg );
		}   // ReportException method (5 of 7 - ObjectDisposedException)


        /// <summary>
        /// Format and report the properties of an IOException exception on a
        /// console in a thread-safe manner.
        /// </summary>
        /// <param name="perrIOError">
        /// The instance of the IOException class to process. See Remarks.
        /// </param>
        /// <param name="pstrCurrMethodName">
        /// This should be the name of the method in which the error is caught
        /// and reported. See Remarks.
        /// </param>
        /// <param name="pfi">
        /// The FileInfo object makes available much more than the file name,
        /// allowing for the possibility of an override to deliver more detailed
        /// information about the file being processed.
        /// </param>
        /// <remarks>
        /// This routine processes ANY exception of the IOException class and
        /// its derivatives. Methods for parsing published derived classes are
        /// somewhere on my ToDo list.
        ///
        /// The TargetSite property, exposed by descendants of System.Exception,
        /// is the name of the method in which the exception was thrown.
        /// </remarks>
		[Obsolete ( "ReportException methods that take string pstrCurrMethodName are being retired in favor of a simplified method signature that the same information from the TargetSite property of the exception. These methods will eventually be removed." )]
		public string ReportException (
            IOException perrIOError ,
            string pstrCurrMethodName ,
            FileInfo pfi )
        {
            StringBuilder sbMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );
            StringBuilder sbLogMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );

            sbMsg.Append (
                ReformatExceptionMessage (
                    perrIOError ,
                    pstrCurrMethodName ,
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_MSG_TPL ) );
            sbMsg.AppendFormat (
                WizardWrx.DLLServices2.Properties.Resources.ERRMSG_FILENAME_TPL ,
                pfi.Name ,
                Environment.NewLine );

            if ( UseEventLog ( _enmOutputOptions ) )
            {   // Generate the separate message for the event log, which always includes the stack trace.
                sbLogMsg.Append (
                    ReformatExceptionMessage (
                        perrIOError ,
                        pstrCurrMethodName ,
                        WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_EVTMSG_TPL ) );
                sbLogMsg.AppendFormat (
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_FILENAME_TPL ,
                    pfi.FullName ,
                    Environment.NewLine );
            }   // if ( UseEventLog ( _enmOutputOptions ) )

			return AddCommonElementsReportAndReturn (
				perrIOError ,
				sbMsg ,
				sbLogMsg );
		}   // ReportException method (6 of 7 - IOException)


        /// <summary>
        /// Format and report the properties of an FormatException exception on
        /// a console in a thread-safe manner.
        /// </summary>
        /// <param name="perrrBadFormat">
        /// The instance of the FormatException class to process.
        /// </param>
        /// <param name="pstrCurrMethodName">
        /// This should be the name of the method in which the error is caught
        /// and reported. See Remarks.
        /// </param>
        /// <param name="pstrFormatString">
        /// This should be the format string which caused the exception. There
        /// should be a way to feed this to the exception, or recover it, but I
        /// have yet to find it.
        /// </param>
        /// <remarks>
        /// The TargetSite property, exposed by descendants of System.Exception,
        /// is the name of the method in which the exception was thrown.
        /// </remarks>
		[Obsolete ( "ReportException methods that take string pstrCurrMethodName are being retired in favor of a simplified method signature that the same information from the TargetSite property of the exception. These methods will eventually be removed." )]
		public string ReportException (
            FormatException perrrBadFormat ,
            string pstrCurrMethodName ,
            string pstrFormatString )
        {
            StringBuilder sbMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );
            StringBuilder sbLogMsg = new StringBuilder ( MagicNumbers.CAPACITY_08KB );

            sbMsg.Append (
                ReformatExceptionMessage (
                    perrrBadFormat ,
                    pstrCurrMethodName ,
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_MSG_TPL ) );
            sbMsg.AppendFormat (
                WizardWrx.DLLServices2.Properties.Resources.ERRMSG_FORMATSTRING_TPL ,
                pstrFormatString ,
                Environment.NewLine );

            if ( UseEventLog ( _enmOutputOptions ) )
            {   // Generate the separate message for the event log, which always includes the stack trace.
                sbLogMsg.Append (
                    ReformatExceptionMessage (
                        perrrBadFormat ,
                        pstrCurrMethodName ,
                        WizardWrx.DLLServices2.Properties.Resources.ERRMSG_EX_EVTMSG_TPL ) );
                sbLogMsg.AppendFormat (
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_FORMATSTRING_TPL ,
                    pstrFormatString ,
                    Environment.NewLine );
            }   // if ( UseEventLog ( _enmOutputOptions ) )

			return AddCommonElementsReportAndReturn (
				perrrBadFormat ,
				sbMsg ,
				sbLogMsg );
		}   // ReportException method (7 of 7 - FormatException)
        #endregion  // Public ReportException Methods


		/// <summary>
		/// The StateManager calls this method once, immediately after both it
		/// and the ExceptionLogger exist.
		/// </summary>
		/// <param name="psmOfThisApp">
		/// To simplify matters a bit, the state manager passes a reference to
		/// itself.
		/// </param>
		internal void GetStandardHandleStates ( StateManager psmOfThisApp )
		{
			_fStdOutIsRedirected = ( psmOfThisApp.StandardHandleState ( StateManager.ShsStandardHandle.ShsStdOut ) == StateManager.ShsHandleState.ShsRedirected );
			_fStdErrIsRedirected = ( psmOfThisApp.StandardHandleState ( StateManager.ShsStandardHandle.ShsStdEror ) == StateManager.ShsHandleState.ShsRedirected );
		}	// GetStandardHandleStates


        #region Private Instance Methods
        /// <summary>
        /// Test whether an OutputOptions is ON or OFF.
        /// </summary>
        /// <param name="penmTestThisOption">
        /// Specify the OutputOptions enumeration member to test.
        /// </param>
        /// <returns>
        /// Return TRUE if the specified option is ON; otherwise, return FALSE.
        /// </returns>

        //  --------------------------------------------------------------------
        //  This routine simplifies simultaneously evaluating combinations of
        //  two or more OutputOptions. The following example illustrates my
        //  point.
        //
        //  With this routine:
        //
        //     if ( OptionIsOn ( OutputOptions.StandardError ) || OptionIsOn ( OutputOptions.StandardOutput ) )
        //
        //  Without it:
        //
        //     if ( ( ( _enmOutputOptions & OutputOptions.StandardError ) == OutputOptions.StandardError ) || ( ( _enmOutputOptions & OutputOptions.StandardOutput ) == OutputOptions.StandardOutput ) )
        //
        //  Which statement would you rather figure out a year or two after you
        //  wrote it?
        //
        //  As such, it is syntactic sugar, which I expect to be optimized away
        //  by in-lining; since I have to understand the C# code at a glance, but
        //  I may never look at the optimized IL, it doesn't matter nearly as
        //  much that it is easy to decipher.
        //  --------------------------------------------------------------------

        private bool OptionIsOn ( OutputOptions penmTestThisOption )
        {   // Return TRUE if the penmTestThisOption flag is ON.
            return ( _enmOutputOptions & penmTestThisOption ) == penmTestThisOption;
        }   // OptionIsOn


        /// <summary>
        /// Report as indicated by the flags stored in the _enmOutputOptions bit
        /// mask.
        /// </summary>
        /// <param name="pstrMsg">
        /// The message string to return to the caller.
        /// </param>
        /// <param name="pstrLogMsg">
        /// To correctly report inner exceptions, the messages for the user and
        /// the event log must be segregated and built concurrently.
        /// </param>
        /// <returns>
        /// The message to report.
        /// </returns>
        /// <remarks>
        /// This is the only private instance method. Making it static would
        /// require four additional arguments into it. I'd rather save those 128
        /// bytes of stack frame for when I really need it.
        ///
        /// The same message is recorded on the console, if so indicated, and in
        /// the application event log, EXCEPT that the copy that goes into the
        /// event log ALWAYS gets a stack trace attached for the exception and
        /// each inner exception, if any. Finally, the text is returned, so that
        /// the caller can use it, for example, in a message box.
        /// </remarks>
        private string ReportAsDirected (
            string pstrMsg ,
            string pstrLogMsg )
        {
#if ECHO_COLOURS
            Console.WriteLine ( "Default colors for fatal exception message, per ErrorMessagesInColor: Foreground = {0}" , ErrorMessagesInColor.FatalExceptionTextColor );
            Console.WriteLine ( "                                                                      Background = {0}" , ErrorMessagesInColor.FatalExceptionBackgroundColor );

            if ( _emsgColorsStdErr == null )
            {   // Console.WriteLine can handle a property being null, but a null object throws a NullReferenceException exception.
                Console.WriteLine ( "Current colors for fatal exception message, per ExceptionLogger:      Foreground = NULL" );
                Console.WriteLine ( "                                                                      Background = NULL" );
            }   // TRUE (anticipated outcome) block, if ( _emsgColorsStdErr == null )
            else
            {
                Console.WriteLine ( "Current colors for fatal exception message, per ExceptionLogger:      Foreground = {0}" , _emsgColorsStdErr.MessageForegroundColor );
                Console.WriteLine ( "                                                                      Background = {0}" , _emsgColorsStdErr.MessageBackgroundColor );
            }   // FALSE (UNexpected outcome) block, if ( _emsgColorsStdErr == null )
#endif  // #if ECHO_COLOURS

			if ( UseConsole ( _enmOutputOptions ) )
			{	// The client wants errors written onto one or both of the standard console output streams.
				SetMessageColors ( );
			}	// if ( UseConsole ( _enmOutputOptions ) )

			//	----------------------------------------------------------------
			//	Since a user might well want error messages to be displayed on
			//	the console via the standard error stream and preserved in a
			//	redirected standard output stream, this method supports this use
			//	case. However, when standard output is feeding the console, this
			//	produces duplicate error messages. Hence, the second printing is
			//	suppressed when it has already gone to standard error AND the
			//	standard output stream is redirected.
			//	----------------------------------------------------------------

			bool fWroteOnStandardError = false;

			if ( ( _enmOutputOptions & OutputOptions.StandardError ) == OutputOptions.StandardError )
			{	// The client assembly wants errors written onto the standard error stream.
				fWroteOnStandardError = true;

				if ( _emsgColorsStdOut == null )
				{	// Write directly to the standard error stream.
					Console.Error.WriteLine ( pstrMsg );                                // Use the current screen colors.
				}
				else
				{	// Write indirectly to the standard error stream through the ErrorMessagesInColor class.
					_emsgColorsStdErr.Write ( pstrMsg );                                // Use the custom screen colors.
				}	// if ( _emsgColorsStdOut == null )
			}	// if ( ( _enmOutputOptions & OutputOptions.StandardError ) == OutputOptions.StandardError )

			if ( ( _enmOutputOptions & OutputOptions.StandardOutput ) == OutputOptions.StandardOutput )
			{	// The client assembly wants errors written onto the standard output stream.
				if ( fWroteOnStandardError )
				{	// Suppress if this is a duplicate, which is true if standard error was written and standard output is attached to the console.
					if ( _fStdOutIsRedirected )
					{	// Although the message was written on standard error, since standard output is redirected, it gets a copy.
						if ( _emsgColorsStdOut == null )
						{	// Write directly to the standard output stream.
							Console.WriteLine ( pstrMsg );                                      // Use the current screen colors.
						}
						else
						{	// Write indirectly to the standard output stream through the MessageInColor class.
							_emsgColorsStdOut.Write ( pstrMsg );                                // Use the custom screen colors.
						}	// if ( _emsgColorsStdOut == null )
					}	// if ( _fStdOutIsRedirected )
				}	// TRUE (The message was written on the Standard Error stream.) block, if ( fWroteOnStandardError )
				else
				{	// If it didn't go on Standard Error, the redirection state of Standard Output is moot.
					if ( _emsgColorsStdOut == null )
					{	// Write directly to the standard output stream.
						Console.WriteLine ( pstrMsg );                                      // Use the current screen colors.
					}
					else
					{	// Write indirectly to the standard output stream through the MessageInColor class.
						_emsgColorsStdOut.Write ( pstrMsg );                                // Use the custom screen colors.
					}	// if ( _emsgColorsStdOut == null )
				}	// FALSE (The message was NOT written on the Standard Errror stream.) block, if ( fWroteOnStandardError )
			}	// if ( ( _enmOutputOptions & OutputOptions.StandardOutput ) == OutputOptions.StandardOutput )

            if ( UseEventLog ( _enmOutputOptions ) )
                System.Diagnostics.EventLog.WriteEntry (
                    string.IsNullOrEmpty ( _strEventSource )
                        ? GetDefaultEventSourceID ( )
                        : _strEventSource ,
                    pstrLogMsg ,
                    System.Diagnostics.EventLogEntryType.Error );

            return pstrMsg;                                                             // Return a copy to the caller.
        }   // private static string ReportAsDirected


        /// <summary>
        /// This method attends to default message colors, setting or
        /// reinstating them as needed.
        /// </summary>
        private void SetMessageColors ( )
        {
#if ECHO_COLOURS
            if ( _emsgColorsStdErr == null )
                Console.WriteLine ( "On entry to SetMessageColors, _emsgColorsStdErr is NULL." );
            else
                Console.WriteLine (
                    "On entry to SetMessageColors, _emsgColorsStdErr is {0} text on a {1} background." ,
                    _emsgColorsStdErr.MessageForegroundColor ,
                    _emsgColorsStdErr.MessageBackgroundColor );

            Console.WriteLine (
                "On entry to SetMessageColors, ErrorMessagesInColor specifies {0} text on a {1} background." ,
                ErrorMessagesInColor.FatalExceptionTextColor ,
                ErrorMessagesInColor.FatalExceptionBackgroundColor );

            Console.WriteLine (
                "On entry to SetMessageColors, the call stack is as follows:{1}{0}{1}END OF CALL STACK{1}" ,
                Environment.StackTrace ,
                Environment.NewLine );
#endif  // #if ECHO_COLOURS

            if ( _emsgColorsStdErr != null )
                return; // Leave it.

            if ( ErrorMessagesInColor.FatalExceptionTextColor == ErrorMessagesInColor.FatalExceptionBackgroundColor )
                return; // Can't use. Treat as if null, so messages stay visible.

            _emsgColorsStdErr = new ErrorMessagesInColor (
                ErrorMessagesInColor.FatalExceptionTextColor ,
                ErrorMessagesInColor.FatalExceptionBackgroundColor );
            _emsgColorsStdOut = new MessageInColor (
                ErrorMessagesInColor.FatalExceptionTextColor ,
                ErrorMessagesInColor.FatalExceptionBackgroundColor );
        }   // private void SetMessageColors
        #endregion  // Private Instance Methods


        #region Private Static Methods
		/// <summary>
		/// The last two blocks of every ReportException method are identical,
		/// and are extracted to reduce the code size.
		/// </summary>
		/// <param name="perrAny">
		/// Pass in a reference to the exception being reported, from which
		/// private method AddCommonElements, which may get folded into it,
		/// extracts the TargetSite, StackTrace, and other common properties,
		/// depending on the current state of the option flags.
		/// </param>
		/// <param name="psbMsg">
		/// Pass in a reference to the partially constructed message, which has
		/// the raw or parsed message, along with other properties that vary by
		/// exception type.
		/// 
		/// This StringBuilder is eventually sent to the console if the option
		/// flags so indicate, and becomes the value returned by the method.
		/// </param>
		/// <param name="psbLogMsg">
		/// Pass in a reference to the partially constructed message, which has
		/// the raw or parsed message, along with other properties that vary by
		/// exception type.
		/// 
		/// This StringBuilder is eventually sent to a Windows Event Log, if the
		/// option flags so indicate; otherwise, it is discarded.
		/// </param>
		/// <returns>
		/// The completed sbLogMsg is always returned to the calling routine,
		/// which may dispose of it as it sees fit, and usually discards it if
		/// the calling routine is a console program, or displays it in a
		/// message box if the program is running in the graphical subsystem.
		/// </returns>
		private string AddCommonElementsReportAndReturn (
			Exception perrAny ,
			StringBuilder psbMsg ,
			StringBuilder psbLogMsg )
		{
			AddCommonElements (
				psbMsg ,
				psbLogMsg ,
				perrAny ,
				_enmOutputOptions );

			if ( UseEventLog ( _enmOutputOptions ) )
				return ReportAsDirected (
					psbMsg.ToString ( ) ,
					psbLogMsg.ToString ( ) );
			else
				return ReportAsDirected (
					psbMsg.ToString ( ) ,
					string.Empty );
		}	// AddCommonElementsReportAndReturn
		
		
		/// <summary>
        /// Add the Source, TargetSite, and StackTrace properties to the
        /// exception report. See Remarks.
        /// </summary>
        /// <param name="psbMsg">
        /// Append the report items to this StringBuilder.
        /// </param>
        /// <param name="psbLogMsg">
        /// Since the stack trace is always included, the message for the event
        /// log must be assembled separately. If event logging is disabled, this
        /// argument is a null reference, so we don't waste effort if it would
        /// be discarded.
        /// </param>
        /// <param name="perrAnyKind">
        /// This is an instance of the System.Exception class, or one of its
        /// derivatives. See Remarks.
        /// </param>
        /// <param name="penmOutputOptions">
        /// Combine members of the OutputOptions enumeration to specify items to
        /// include in the report, and how to log the error. (The enumeration is
        /// a bit mask.)
        /// </param>
        /// <remarks>
        /// This method is called recursively to process inner exceptions.
        ///
        /// By default, all exceptions which derive from System.Exception expose
        /// these three properties, and any of them can be cast to this type.
        ///
        /// The TargetSite string contains the name of the method that threw the
        /// exception.
        ///
        /// The Source string contains the name of the class to which the method
        /// named in the TargetSite string belongs.
        /// </remarks>
        private static void AddCommonElements (
            StringBuilder psbMsg ,
            StringBuilder psbLogMsg ,
            Exception perrAnyKind ,
            OutputOptions penmOutputOptions )
        {
            if ( ( penmOutputOptions & OutputOptions.Method ) == OutputOptions.Method )
                psbMsg.AppendFormat (
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_METHOD ,         // Format String
                    perrAnyKind.TargetSite ,                                            // Token Index 0
                    Environment.NewLine );                                              // Token Index 1

            if ( ( penmOutputOptions & OutputOptions.Source ) == OutputOptions.Source )
                psbMsg.AppendFormat (
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_SOURCE ,         // Format String
                    perrAnyKind.Source ,                                                // Token Index 0
                    Environment.NewLine );                                              // Token Index 1

            string strPrettyStackTrace = FormatStackTrace ( perrAnyKind );

            if ( ( penmOutputOptions & OutputOptions.Stack ) == OutputOptions.Stack )
                psbMsg.Append ( strPrettyStackTrace );

            if ( psbLogMsg != null )
            {   // The event log always gets a copy of the stack trace.
                psbLogMsg.AppendFormat (
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_METHOD ,         // Format String
                    perrAnyKind.TargetSite ,                                            // Token Index 0
                    Environment.NewLine );                                              // Token Index 1
                psbLogMsg.AppendFormat (
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_SOURCE ,         // Format String
                    perrAnyKind.Source ,                                                // Token Index 0
                    Environment.NewLine );                                              // Token Index 1
                psbLogMsg.Append ( strPrettyStackTrace );
            }   // if ( psbLogMsg != null )

            //  ----------------------------------------------------------------
            //  Check for an inner exception. If there is none, say so and exit.
            //  Otherwise, do the following.
            //
            //  1)  Add the message from the InnerException to the report.
            //
            //  2)  Call AddCommonElements recursively, passing the inner
            //      exception.
            //  ----------------------------------------------------------------

            if ( perrAnyKind.InnerException == null )
            {   // Either the original exception had none, or this is the end of the chain.
                psbMsg.AppendFormat (
                    WizardWrx.DLLServices2.Properties.Resources.THIS_IS_THE_LAST ,      // Format String
                    Environment.NewLine );                                              // Token Index 0

                if ( psbLogMsg != null )
                {   // Since event logging is enabled, it gets a copy.
                    psbLogMsg.AppendFormat (
                        WizardWrx.DLLServices2.Properties.Resources.THIS_IS_THE_LAST ,  // Format String
                        Environment.NewLine );                                          // Token Index 0
                }   // if ( psbLogMsg != null )
            }   // TRUE block, if ( perrAnyKind.InnerException == null )
            else
            {   // There is at least one inner exception. Process them recursively.
                psbMsg.AppendFormat (
                    WizardWrx.DLLServices2.Properties.Resources.ERRMSG_INNER ,          // Format String
                    perrAnyKind.InnerException.Message ,                                // Token Index 0
                    Environment.NewLine );                                              // Token Index 1
                AddCommonElements (
                    psbMsg ,
                    psbLogMsg ,
                    perrAnyKind.InnerException ,
                    penmOutputOptions );
            }   // FALSE block, if ( perrAnyKind.InnerException == null )
        }   // private static void AddCommonElements method


        /// <summary>
        /// Return a new empty StringBuilder if event logging is enabled.
        /// Otherwise, return a null reference, which signals the exception
        /// reporting routines to skip building a message for it.
        /// </summary>
        /// <param name="penmOutputOptions">
        /// Since some of the methods with which it works are static because
        /// they are called recursively, this routine must also be static, and
        /// it must receive a copy of the OutputOptions bit mask, successor to
        /// the RecordinEventLog flag.
        /// </param>
        /// <returns>
        /// If a copy of the report is bound for a Windows event log, it is
        /// constructed in the StringBuilder returned by this method. Otherwise,
        /// the null reference signals the message formatters not to bother.
        /// </returns>
        private static StringBuilder CreateForEventLog ( OutputOptions penmOutputOptions )
        {
            if ( UseEventLog ( penmOutputOptions ) )
                return new StringBuilder ( MagicNumbers.CAPACITY_08KB );
            else
                return null;
        }   // private static StringBuilder CreateForeventLog


        /// <summary>
        /// Format the stack trace to make it (hopefully) a tad easier to read.
        /// </summary>
        /// <param name="perrAnyKind">
        /// A reference to the entire exception is passed into the method, from
        /// which this routine extracts the stack trace.
        /// </param>
        /// <returns>
        /// The returned string contains the formatted stack trace.
        /// </returns>
        private static string FormatStackTrace ( Exception perrAnyKind )
        {
            if ( perrAnyKind == null )
                return string.Empty;
            else
                return string.Format (
                    WizardWrx.DLLServices2.Properties.Resources.STACKTRACE_TPL ,        // Format String
                    perrAnyKind.StackTrace ,                                            // Token index 0
                    Environment.NewLine );                                              // Token index 1
        }   // FormatStackTrace


        /// <summary>
        /// Read the default event source ID string from the DLL configuration.
        /// </summary>
        /// <returns>
        /// If the function succeeds, the return value is the event source ID
        /// string stored in the configuration file that comes along for the
        /// ride whenever this DLL is imported into a project. Otherwise, the
        /// default event source ID defined in WIZARDWRX_EVENT_SOURCE_ID is
        /// returned.
        ///
        /// To save trips to the disk or its cache, once read, the event source
        /// ID is cached in static string s_strDefaultEventSource.
        /// </returns>
        private static string GetDefaultEventSourceID ( )
        {
            if ( string.IsNullOrEmpty ( s_strDefaultEventSource ) )
            {   // The first request initializes s_strDefaultEventSource.
				lock ( s_srCriticalSection )
                {
                    try
                    {
                        PropertyDefaults DLLProperties = new PropertyDefaults ( );
                        System.Configuration.KeyValueConfigurationCollection DLLConfigurationSettings = DLLProperties.ValuesCollection;
                        s_strDefaultEventSource = DLLConfigurationSettings [ Properties.Resources.DEFAULT_EVENT_SOURCE_ID ].Value;
                    }
                    catch ( Exception exAll )
                    {   // Tell on yourself in a way that avoids creating a recursion loop that never converges.
                        ExceptionLogger TattleTale = ExceptionLogger.GetTheSingleInstance ( );          // Get a local reference to the singleton, just like everybody else does.
                        TattleTale.AppEventSourceID = WIZARDWRX_EVENT_SOURCE_ID;                        // To prevent a recursion loop, set the event source property to the hard coded default.
                        TimeStampedTraceWrite ( TattleTale.ReportException ( exAll ) );                 // Log the error with the event viewer, if configured, and the trace listener(s), if any.
                        s_strDefaultEventSource = WIZARDWRX_EVENT_SOURCE_ID;                            // Return the fail-safe default event source ID.
                    }
				}   // lock ( s_srCriticalSection )
            }   // TRUE (first time in the lifetime of this application) block, if ( string.IsNullOrEmpty ( s_strDefaultEventSource ) )

            return s_strDefaultEventSource;
        }   // private static string GetDefaultEventSourceID


        /// <summary>
        /// Look up the exception in the list of known exceptions and, if found,
        /// return a string that marks the point where the displayed message is
        /// to be truncated.
        /// </summary>
        /// <param name="pguidExceptionTypeName">
        /// This string contains the fully qualified type name of the exception,
        /// which is the key to an public Dictionary of strings that mark the
        /// point where the message should be truncated for display. Please see
        /// the Remarks section.
        /// </param>
        /// <returns>
        /// The return value, which may be the empty string, is text, such as a
        /// fixed label, that marks a point where the message supplied by the
        /// exception is truncated.
        /// </returns>
        /// <remarks>
        /// Typically, a message is truncated because we present the information
        /// in a more visually appealing and/or accessible format. Of the myriad
        /// exceptions exposed by the Base Class Library, not to mention custom
        /// exceptions derived from System.Exception, only a handful are "known"
        /// types that require attention.
        ///
        /// Messages from types that are unknown to this class (i. e., they have
        /// no entry in the s_dctKnowExceptionTypes dictionary, are preserved.
        ///
        /// This private method hides the processing required to cover for the
        /// unknown exception type.
        /// </remarks>
        private static string GetMessageTruncationStart ( Guid pguidExceptionTypeName )
        {
            if ( s_dctKnowExceptionTypes == null )
                return null;

            string rstrTruncateFromThisString = null;

            if ( s_dctKnowExceptionTypes.TryGetValue ( pguidExceptionTypeName , out rstrTruncateFromThisString ) )
                return rstrTruncateFromThisString;
            else
                return null;
        }   // private static string GetMessageTruncationStart


        /// <summary>
        /// This private method returns a stack trace or empty string, depending
        /// on the value of the StackTraceDisposition flag.
        /// </summary>
        /// <param name="pstrPrettyStackTrace">
        /// Since a beautified stack trace is always added to the event log, one
        /// always created, although this method may ignore it.
        /// </param>
        /// <param name="penmStackTraceDisposition">
        /// This flag determines whether this method returns the stack trace or
        /// empty string.
        /// </param>
        /// <returns>
        /// The return value is whatever goes into the message that will be
        /// returned to the calling method.
        /// </returns>
        /// <remarks>
        /// Although this method could be coded inline, I chose this approach to
        /// minimize the overall amount of code in the many overloads that would
        /// otherwise each need IF statements and pairs of almost identical
        /// format statements.
        /// </remarks>
        private static string IncludeStackTraceIfRequested (
            string pstrPrettyStackTrace ,
            StackTraceDisposition penmStackTraceDisposition )
        {
            if ( penmStackTraceDisposition == StackTraceDisposition.Include )
                return pstrPrettyStackTrace;
            else
                return string.Empty;
        }   // private static string IncludeStackTraceIfRequested


        /// <summary>
        /// This private method beautifies the format of invalid argument
        /// exception reports.
        /// </summary>
        /// <param name="pexAnyKind">
        /// Reference to exception from which to extract and format its Message
        /// property.
        /// </param>
        /// <param name="pstrRoutineLabel">
        /// This string identifies the place in the source code where the
        /// exception was thrown.
        /// </param>
        /// <param name="pstrMessageTemplate">
        /// Format string, suitable for use with String.Format, from which the
        /// beautified message is constructed.
        /// </param>
        /// <returns>
        /// Beautified string, suitable for presentation on a console.
        /// </returns>
		private static string ReformatExceptionMessage (
			Exception pexAnyKind ,
			string pstrRoutineLabel ,
			string pstrMessageTemplate )
        {
            const string MESSAGE_PADDING = @"{0}               ";

#if SHOW_TYPE_HANDLES
			Type typThisException = pexAnyKind.GetType ( );
			Console.WriteLine ( "{0}Exception type, as seen by ReformatExceptionMessage:{0}" , Environment.NewLine);
			Console.WriteLine ( "    Type FullName   = {0}" , typThisException.FullName );
			Console.WriteLine ( "    Type Name       = {0}" , typThisException.Name );
			Console.WriteLine ( "    Type GUID       = {0}" , typThisException.GUID );
			Console.WriteLine ( "    TypeHandle      = {0}" , typThisException.TypeHandle.Value.ToString ( NumericFormats.HEXADECIMAL_8 ) );
#endif	// SHOW_TYPE_HANDLES

			string strRawMsg = pexAnyKind.Message;

            Guid guidExceptionTypeName = pexAnyKind.GetType ( ).GUID;
            string strTrimStartPoint = GetMessageTruncationStart ( guidExceptionTypeName );
            string strTrimmedMsg = null;

            if ( string.IsNullOrEmpty ( strTrimStartPoint ) )
                strTrimmedMsg = strRawMsg;
            else
                if ( strRawMsg.IndexOf ( strTrimStartPoint ) == MagicNumbers.STRING_INDEXOF_NOT_FOUND )
                    strTrimmedMsg = strRawMsg;
                else
                    strTrimmedMsg = strRawMsg.Substring (
                        MagicNumbers.STRING_SUBSTR_BEGINNING ,
                        strRawMsg.IndexOf ( strTrimStartPoint ) );

            return string.Format (
                pstrMessageTemplate ,
                new string [ ]
                    {
                        pexAnyKind.GetType().FullName ,
                        pstrRoutineLabel ,
                        WizardWrx.StringTricks.ReplaceToken (
                            strTrimmedMsg ,
                            Environment.NewLine ,
                            string.Format (
                                MESSAGE_PADDING ,
                                Environment.NewLine ) ) ,
                        Environment.NewLine
                    } );
        }   // private static string ReformatExceptionMessage method


        /// <summary>
        /// The purpose of this routine is to keep the code that sets the
        /// default option flags in one place only.
        /// </summary>
        /// <returns>
        /// The return value is the OutputOptions bit mask with all flags set to
        /// their initial default values.
        /// </returns>
        /// <remarks>
        /// I expect this one-line syntactic candy to be optimized away in the
		/// Release build.
        /// </remarks>
        private static OutputOptions SetDefaultOptions ( )
        {
			return OutputOptions.Method | OutputOptions.Source;
        }   // SetDefaultOptions


        /// <summary>
        /// Synchronize old and new flags and set default message colors if
        /// necessary.
        /// </summary>
        /// <param name="pTheInstance">
        /// Since this method must be static, a reference to the ExceptionLogger
        /// singleton must be passed into it.
        /// </param>
        /// <param name="penmProcessSubsystem">
        /// Use this member of the ProcessSubsystem enumeration to modify the
        /// default behavior of the exception logging methods, by enabling
        /// console output if the calling application has one.
        /// </param>
        /// <param name="penmSubsystem">
        /// Use this member of the Subsystem enumeration to modify the behavior
        /// of the exception logging methods, by enabling console output if the
        /// calling application has one.
        /// </param>
        /// <param name="penmEventLoggingState">
        /// Use this member of the RecordinEventLog enumeration to modify the
        /// behavior of the exception logging methods by enabling or disabling
        /// recording of exceptions in the application event log.
        /// </param>
        /// <param name="penmStackTraceDisposition">
        /// Use this member4 of the StackTraceDisposition enumeration to modify
        /// the behavior of the exception logging methods, by causing the stack
        /// trace to be either included or omitted.
        /// </param>
        private static void InitializeInstance (
            ExceptionLogger pTheInstance ,
            ProcessSubsystem penmProcessSubsystem ,
            Subsystem penmSubsystem ,
            RecordinEventLog penmEventLoggingState ,
            StackTraceDisposition penmStackTraceDisposition )
        {
            pTheInstance._enmProcessSubsystem = penmProcessSubsystem;

            //  ----------------------------------------------------------------
            //  For backward compatibility, set the old flags.
            //  ----------------------------------------------------------------

            pTheInstance._enmSubsystem = penmSubsystem;
            pTheInstance._enmEventLoggingState = penmEventLoggingState;
            pTheInstance._enmStackTraceDisposition = penmStackTraceDisposition;

            //  ----------------------------------------------------------------
            //  These are the flags that will go forward.
            //  ----------------------------------------------------------------

			switch ( penmProcessSubsystem )
            {
                case ProcessSubsystem.Console:
                    pTheInstance._enmOutputOptions = pTheInstance._enmOutputOptions | OutputOptions.StandardError;
                    break;

                case ProcessSubsystem.Windows:
                case ProcessSubsystem.Unknown:
                default:
                    pTheInstance._enmOutputOptions = pTheInstance._enmOutputOptions & ( ~OutputOptions.StandardError );
                    break;
            }   // switch ( penmSubsystem )

			if ( ( pTheInstance.OptionFlags & OutputOptions.EventLog ) != OutputOptions.EventLog )
				if ( penmEventLoggingState == RecordinEventLog.Enabled )
					pTheInstance._enmOutputOptions = pTheInstance._enmOutputOptions | OutputOptions.EventLog;
				else
					pTheInstance._enmOutputOptions = pTheInstance._enmOutputOptions & ( ~OutputOptions.EventLog );

			if ( ( pTheInstance.OptionFlags & OutputOptions.Stack ) != OutputOptions.Stack )
				if ( penmStackTraceDisposition == StackTraceDisposition.Include )
					pTheInstance._enmOutputOptions = pTheInstance._enmOutputOptions | OutputOptions.Stack;
				else
					pTheInstance._enmOutputOptions = pTheInstance._enmOutputOptions & ( ~OutputOptions.Stack );

            if ( UseConsole ( pTheInstance._enmOutputOptions ) )
                pTheInstance.SetMessageColors ( );

			//  ----------------------------------------------------------------
			//  Since the options can be changed without notice, though this is
			//	unlikely to occur sufficiently often to warrant implementing an
			//	event, go ahead and cache the handle states, which should be set
			//	for the life of the program.
			//  ----------------------------------------------------------------
		}   // InitializeInstance


        private static bool UseConsole ( OutputOptions penmOutputOptions )
        {
            bool fStdErrEnabled = ( penmOutputOptions & OutputOptions.StandardError ) == OutputOptions.StandardError;
            bool fStdOutEnabled = ( penmOutputOptions & OutputOptions.StandardOutput ) == OutputOptions.StandardOutput;

            return fStdErrEnabled | fStdOutEnabled;
        }   // UseConsole


        /// <summary>
        /// Hide the complexity of bit mask testing from cursory scanning of the
        /// code, so that the reader doesn't feel compelled to slow down to
        /// study the bit test. At compile time, this routine is optimized away,
        /// replaced by inline code.
        /// </summary>
        /// <param name="penmOutputOptions">
        /// The OutputOptions enumeration is organized for use as a bit mask.
        /// Set its flags as desired to control the format and content of output
        /// generated by the ReportException methods.
        /// </param>
        /// <returns>
        /// Return TRUE if event logging is enabled.
        /// </returns>
        private static bool UseEventLog ( OutputOptions penmOutputOptions )
        {
            return ( penmOutputOptions & OutputOptions.EventLog ) == OutputOptions.EventLog ;
        }   // UseEventLog
        #endregion  // Private Static Methods
    }   // public sealed class ExceptionLogger
}   // partial namespace WizardWrx.DLLServices2