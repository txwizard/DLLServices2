/*
    ============================================================================

    File Name:          program.cs

    Class Name:         Program

    Namespace Name:     ApplicationHelpersTestHarness

    Synopsis:           Exercise the WizardWrx.ApplicationHelpers class library.

    Remarks:            Beginning with version 3.3, the end of job routine stops
                        and waits for a carbon unit if a debug build of the 
                        program is attached to a debugger. Otherwise, it behaves
                        like a release build. See the reference cited below.

                        Beginning with version 4.1, the MessageInColor class is
                        loaded from WizardWrx.ApplicationHelpers.DLLServices,
                        its new home. To resolve ambiguous references, until I
                        have a stable release build of its original home,
                        WizardWrx.ConsoleAppAids, that no longer exports it,
                        since both namespaces are imported, these references
                        must be explicit.

                        Beginning with version 5.0, this test stand is limited
                        to facilities available through the library under test.

    Reference:          "Check If Application Was Started From Within Visual Studio,"
                        http://stackoverflow.com/questions/101806/check-if-application-was-started-from-within-visual-studio

    Author:             David A. Gray

    Copyright:          Copyright 2011-2014, David A. Gray.
                        All rights reserved world wide.

    Created:            Saturday, 25 June 2011 - Monday, 27 June 2011

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version Author Description
    ---------- ------- ------ --------------------------------------------------
    2011/06/27 1.0     DAG/WW Initial version.

    2011/07/11 1.2     DAG/WW Exercise the like named version of the renamed
                              CmdLneArgsBasic class and th eFormattedLocalTime
                              class.

    2011/07/12 1.3     DAG/WW 1) Test t5he change of default value set for an
                                 unmodified switch from SWITCH_IS_OFF to
                                 SWITCH_IS_ON.

                              2) Correct coding errors in two cath blocks, and
                                 add exception handling to the main routine.

                              3) Correct the unbalanced table error that caused
                                 the exception in the first place.

    2011/07/12 1.4     DAG/WW Test integration of the FormattedLocalTime object.

    2011/07/16 1.5     DAG/WW Change namespace name from ConsoleHelpers to
                              ApplicationHelpers, to reflect its broadened range
                              of applications (pun intended).

    2011/10/08 1.6     DAG/WW Exercise the exposed reflected assembly.

    2012/02/26 1.7     DAG/WW Exercise the new SysDateFormatters class.

    2012/03/03 1.8     DAG/WW Test corrections in the processing of default
                              values for missing named arguments and switches by
                              the CmdLneArgsBasic class.

    2012/03/11 2.00    DAG/WW Test the Corrected way that default values of 
                              switches arguments are treated, by applying the
                              same change to switch processing that was
                              implemented for named arguments in version 1.8.

    2012/09/03 2.6     DAG/WW Complete the constellation by adding ReformatNow
                              and ReformatUtcNow.

    2012/09/06 2.6     DAG/WW Add exercises for the new INIReader class.

    2012/11/12 2.8     DAG/WW Add exercises for the new ExceptionLogger class.

    2012/11/12 2.9     DAG/WW Test the smarter EOJ banner writer and the new
                              aliased argument lookup.

    2012/12/26 2.91   DAG/WW Test the static, read-only AssemblyLocation
                             property of the ApplicationInstance class.

    2013/01/21 2.93   DAG/WW Test new methods.

                             1) FirstCharFromString (static, 2 overloads)

                             2) GetArgByNameAsChar (instance, 3 overloads)

    2013/01/22 2.94   DAG/WW Correct an inverted logic error in the second
                             overload of FirstCharFromString.

                             This change inspired a method to format special
                             characters in the output, specifically, the NULL
                             and TAB characters.

    2013/02/15 2.95   DAG/WW Convert the FirstCharFromString method from an
                             instance method to a static method, to conform to
                             its documented binding, which has no direct effect
                             on this module.

    2013/02/20 2.96   DAG/WW Add a new class, GACAssemblyLocatorTest, and call
                             its single method (Go) to exercise a class derived
                             from AssemblyLocatorBase and exported by an
                             assembly that loads from the Global Assembly Cache.

    2013/02/22 2.97   DAG/WW Remove all tests of classes that inherit from base
                             class AssemblyLocatorBase, which has been moved to
                             a dedicated namespace and class library, and has a
                             test stand of its own.

    2013/11/19 3.0    DAG/WW Implement a new NormalExitAction type, Silent, for
                             which this routine provides no test case, because
                             another project, EOJTest, does so.
 
    2013/11/24 3.1    DAG/WW 1) Add the following overlooked, but often required
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

                             Since the format tester is table driven, the 
                             executable code is unaffected.

    2014/02/05 3.2    DAG/WW Add support for the new MessageInColors class, for
                             use in character mode applications.

    2014/03/10 3.3    DAG/WW Build against the correspondingly numbered version
                             of the library under test, which covers correction
                             of a long standing bug, first noticed when I began
                             actually turning the stack trace listing OFF.

    2014/03/28 4.0    DAG/WW Test the successor to the ApplicationInstance 
                             class, StateManager, along with further refinements
                             of ExceptionLogger, with repsect to its
                             behavior in Console mode.

    2014/05/22 4.1    DAG/WW 1) Test the new integer parsing methods and the 
                                improved duplicate key handling in CmdArgsBasic.

                             2) Test new GetExeSubsystem and GetProcessSubsystem
                                routines.

                             3) Move class MessageInColor from class library
                                WizardWrx.ConsoleAppAids to class library
                                WizardWrx.ApplicationHelpers.DLLServices. See
                                Remarks.

    2014/06/02 4.2    DAG/WW 1) Replace symbol names and text that suggests that
                                ApplicationHelpersTestHarness does anything with
                                the ApplicationInstance object.

                             2) Since I had to visit this routine anyway, I took
                                the opportunity to fix a typographical error in
                                a literal that appears in dozens of messages in
                                the output of every test run.

                             3) Scanning through the test report today, I found
                                a tiny formatting error in the date formatter
                                test routine, in which the "Formatted Date/Time
                                String" label was misaligned by 1 character.

    2014/06/21 5.0     DAG    Major namespace reorganization.

    2014/06/23 5.1     DAG    Documentation corrections.

    2014/09/14 5.2     DAG    Exercise the new utility classes.

    ============================================================================
*/

using System;
using System.Collections.Generic;
using System.Text;

/*  Added by DAG */

using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

using WizardWrx;
using WizardWrx.ApplicationHelpers3;
using WizardWrx.DLLServices2;
using WizardWrx.Logging;


namespace ApplicationHelpers3_TestStand
{
    class Program
    {
        #region Enumerations and Constants
        enum BannerType
        {
            BOJ,
            EOJ
        }   // enum BannerType

        enum DateFormatter
        {
            ReformatSysDate ,
            ReformatNow ,
            ReformatUtcNow
        }   // enum DateFormatter

        const byte OUTPUT_OPTIONS_ALL_OFF = 0;

        const int BREAKPOINT = 49;

        const string FORMATTED_DATE_REPORT_FQFN = @"C:\Documents and Settings\DAG\My Documents\Visual Studio 2010\Projects\WizardWrx_Libs\ApplicationHelpers2\_Notes_and_References\ApplicationHelpers2TestHarness_FormattedDateTests.TXT";
        const string TEST_PRIVATE_PROFILE_FILE = @"C:\Documents and Settings\DAG\My Documents\Visual Studio 2010\Projects\WizardWrx_Libs\ApplicationHelpers2\_Notes_and_References\EZXFER.INI";

        const string ERRMSG_ABANDONED_MUTEX_HANDLE_TPL = @"SafeWaitHandle = {0}";
        const string ERRMSG_BADARG_PARAMINFO_TPL = @"Argument Name = {0} - Argument Value = {1}";
        const string ERRMSG_INTERNAL_ERROR_TPL = @"INTERNAL ERROR: {0}{1}";
        const string ERRMSG_INTERNAL_INNER_MSG_TPL = @"Inner Exception: Message = {0}";
        const string ERRMSG_INTERNAL_OBJNAME_TPL = @"Name of disposed object = {0}";
        const string ERRMSG_INTERNAL_SOURCE_TPL = @"Source = {0}{1}";
        const string ERRMSG_INTERNAL_STACKTRACE_TPL = @"{1}Stack Trace:{1}{0}{1}{1}";
        const string ERRMSG_INTERNAL_TARGETSITE_TPL = @"TargetSite = {0}{1}";
        const string ERRMSG_GENERIC_TPL = @"Error writing to log file {0}. Message = {1}{5}Source = {2}{5}TargetSite = {3}{5}StackTrace Begin:{5}{4}{5}StackTrace End";

        const string LBL_DTMTESTTIME = @"dtmTestTime";
        const string LBL_CALENDARDATEFORMAT = @"CalendarDateFormat";
        const string LBL_HOURSFORMAT = @"HoursFormat";
        const string LBL_SHOWAMPMASUC = @"ShowAmPmAsUC";
        const string LBL_SHOWTIMEZONE = @"ShowTimeZone";
        const string LBL_SHOWWEEKDAY = @"ShowWeekday";
        const string LBL_TIMEPRECISION = @"TimePrecision";
        const string LBL_COMMENT = @"Comment";
        const string LBL_FORMATTEDDATETIMESTRING = @"FormattedDateTimeString";

        const string LIST_IS_EMPTY = @"empty";
        const string LIST_IS_NULL = @"null";

        const string NAMED_ARG_UC_1 = @"NamedArg1";
        const string NAMED_ARG_UC_2 = @"NamedArg2";
        const string NAMED_ARG_UC_3 = @"NamedArg3";

        const string NAMED_ARG_1_DEFAULT = @"NamedArg1DefaultValue";
        const string NAMED_ARG_2_DEFAULT = @"NamedArg2DefaultValue";
        const string NAMED_ARG_3_DEFAULT = @"NamedArg3DefaultValue";

        const char SWITCH_UC_Q = 'Q';
        const char SWITCH_UC_R = 'R';
        const char SWITCH_UC_S = 'S';
        const char SWITCH_UC_T = 'T';

        const string SWITCH_1_DEFAULT = @"Switch1Default";
        const string SWITCH_2_DEFAULT = @"Switch2Default";
        const string SWITCH_3_DEFAULT = @"Switch3Default";

        const string TIMPRECISION_IGNORED = @"TimePrecision property is ignored.";
        #endregion  // Enumerations and Constants


        #region Static Arrays of Test Data
        enum ExceptionTypes
        {
            ExceptionTest ,
            ArgumentExceptionTest ,
            ArgumentOutOfRangeExceptionTest ,
            ArgumentNullExceptionTest ,
            ObjectDisposedExceptionTest ,
            IOExceptionTest ,
            FormatExceptionTest ,
        };  // enum ExceptionTypes

        static readonly ExceptionTypes [ ] s_aenmExceptionTypes =
        {
            ExceptionTypes.ExceptionTest ,
            ExceptionTypes.ArgumentExceptionTest ,
            ExceptionTypes.ArgumentOutOfRangeExceptionTest ,
            ExceptionTypes.ArgumentNullExceptionTest ,
            ExceptionTypes.ObjectDisposedExceptionTest ,
            ExceptionTypes.IOExceptionTest ,
            ExceptionTypes.FormatExceptionTest ,
        };

        static readonly string [ ] s_astrFormattedDateLabels =
        {
            LBL_DTMTESTTIME,
            LBL_CALENDARDATEFORMAT,
            LBL_HOURSFORMAT,
            LBL_SHOWAMPMASUC,
            LBL_SHOWTIMEZONE,
            LBL_SHOWWEEKDAY,
            LBL_TIMEPRECISION,
            LBL_COMMENT,
            LBL_FORMATTEDDATETIMESTRING
        };  // static readonly string [ ] s_astrFormattedDateLabels

        static readonly string [ ] s_astrPortableExeCollection =
        {
            @"C:\Documents and Settings\DAG\My Documents\Programming\Visual Studio 6\DLL\P6CUtilLib1\ANSI_Test_Harness\Release\ANSI_Test_Harness.exe",
            @"C:\Documents and Settings\DAG\My Documents\Programming\Visual Studio 6\EXE\Console\MLPTCopy\Release\MLPTCopy.exe",
            @"C:\Documents and Settings\DAG\My Documents\Programming\Visual Studio 6\EXE\GUI\MD5WIN\Release\MD5WIN.exe",
            @"C:\Documents and Settings\DAG\My Documents\Visual Studio 2010\Projects\WizardWrx_Libs\ApplicationHelpers2\ApplicationHelpers2TestHarness\ApplicationHelpers2TestHarness\bin\Debug\ApplicationHelpers2TestHarness.exe",
            @"C:\Documents and Settings\DAG\My Documents\Visual Studio 2010\Projects\GUI_Apps\CountDownClock\CountDownClock\bin\Release\CountDownClock.exe"
        };  // static readonly string [ ] s_astrPortableExeCollection

        static readonly char [ ] s_achrSwitchSetAllUC =
        {
            SWITCH_UC_Q ,
            SWITCH_UC_R ,
            SWITCH_UC_S ,
            SWITCH_UC_T
        };  // static readonly char [ ] s_achrSwitchSetAllUC

        static readonly char [ ] s_achrSwitchesWithDupicates =
        {
            SWITCH_UC_Q,
            SWITCH_UC_R ,
            SWITCH_UC_S ,
            SWITCH_UC_Q
        };  // static readonly char [ ] s_achrSwitchesWithDupicates =

        static readonly string [ ] s_astrValidArgumentNames =
        {
            NAMED_ARG_UC_1 ,
            NAMED_ARG_UC_2 ,
            NAMED_ARG_UC_3
        };  // static readonly string [ ] astrValidArgumentNames

        static readonly string [ ] s_astrNamedArgDefaults =
        {
            NAMED_ARG_1_DEFAULT ,
            NAMED_ARG_2_DEFAULT ,
            NAMED_ARG_3_DEFAULT
        };  // static readonly string [ ] s_astrNamedArgDefaults

        static readonly string [ ] s_astrSwitchDefaults =
        {
            WizardWrx.ApplicationHelpers3.CmdLneArgsBasic.SWITCH_IS_ON ,
            SWITCH_1_DEFAULT ,
            SWITCH_2_DEFAULT ,
            SWITCH_3_DEFAULT
        };  // static readonly string [ ] s_astrSwitchDefaults

        static readonly TimeDisplayFormatter.DateFieldOrder [ ] s_aenmDateFieldOrder =
        {
            TimeDisplayFormatter.DateFieldOrder.CultureInfoShortDate,
            TimeDisplayFormatter.DateFieldOrder.DDMMYY,
            TimeDisplayFormatter.DateFieldOrder.DDMMYYYY,
            TimeDisplayFormatter.DateFieldOrder.MMDDYY,
            TimeDisplayFormatter.DateFieldOrder.MMDDYYYY,
            TimeDisplayFormatter.DateFieldOrder.YYMMDD,
            TimeDisplayFormatter.DateFieldOrder.YYYYMMDD
        };  // static readonly TimeDisplayFormatter.DateFieldOrder [ ] s_aenmDateFieldOrder

        static readonly TimeDisplayFormatter.HoursFormatType [ ] s_aenmHoursFormatType =
        {
            TimeDisplayFormatter.HoursFormatType.CultureInfoShortTime,
            TimeDisplayFormatter.HoursFormatType.Civilian,
            TimeDisplayFormatter.HoursFormatType.Military
        };  // static readonly TimeDisplayFormatter.HoursFormatType [ ] s_aenmHoursFormatType

        static readonly string [ ] s_astrComment =
        {
            TIMPRECISION_IGNORED,
            StandardConstants.EMPTY_STRING,
            StandardConstants.EMPTY_STRING
        };  // static readonly string [ ] s_astrComment

        static readonly TimeDisplayFormatter.TimePrecisionType [ ] s_aenmTimePrecisionType =
        {
            TimeDisplayFormatter.TimePrecisionType.HM,
            TimeDisplayFormatter.TimePrecisionType.HMS,
            TimeDisplayFormatter.TimePrecisionType.HMST,
            TimeDisplayFormatter.TimePrecisionType.HMSTN
        };  // static readonly TimeDisplayFormatter.TimePrecisionType [ ] s_aenmTimePrecisionType

        static readonly bool [ ] s_afShowAmPmAsUC =
        {
            false,
            true
        };  // static readonly bool [ ] s_afShowAmPmAsUC

        static readonly bool [ ] s_afShowTimeZone =
        {
            false ,
            true
        };  // static readonly bool [ ] s_afShowTimeZone

        static readonly bool [ ] s_afShowWeekdayName =
        {
            false ,
            true
        };  // static readonly bool [ ] s_afShowWeekdayName

        static readonly bool [ ] _afBlankAsDefault =
        {
            CmdLneArgsBasic.BLANK_AS_DEFAULT_FORBIDDEN ,
            CmdLneArgsBasic.BLANK_AS_DEFAULT_ALLOWED
        };  // static readonly bool [ ] _afBlankAsDefault

        static ExceptionLogger.OutputOptions [ ] s_aenmStackTraceDisp =
        {
             ExceptionLogger.OutputOptions.Stack ,
            ExceptionLogger.OutputOptions.Stack & OUTPUT_OPTIONS_ALL_OFF
        };  // static ExceptionLogger.OutputOptions [ ] s_aenmStackTraceDisp

        static ExceptionLogger.OutputOptions [ ] s_aenmAppSubsystem =
        {
            ExceptionLogger.OutputOptions.StandardError & OUTPUT_OPTIONS_ALL_OFF ,
            ExceptionLogger.OutputOptions.StandardError ,
            ExceptionLogger.OutputOptions.StandardError ,
            ExceptionLogger.OutputOptions.StandardError & OUTPUT_OPTIONS_ALL_OFF ,
            ExceptionLogger.OutputOptions.StandardError & OUTPUT_OPTIONS_ALL_OFF
        };  // static ExceptionLogger.OutputOptions [ ] s_aenmAppSubsystem

        static readonly string [ ] s_astrAppSubSystemDisp =
        {
            "Unknown" ,
            "Console" ,
            "CUI" ,
            "Graphical" ,
            "GUI"
        };  // statuc readonly string s_astrAppSubSystemDisp

        static readonly ExceptionLogger.OutputOptions [ ] s_aenmLoggingState =
        {
            ExceptionLogger.OutputOptions.EventLog ,
            ExceptionLogger.OutputOptions.EventLog & OUTPUT_OPTIONS_ALL_OFF
        };  // static readonly ExceptionLogger.OutputOptions [ ] s_aenmLoggingState

        static readonly ErrorMessagesInColor [ ] s_aclrs =
        {
            null ,
            new ErrorMessagesInColor (
                ConsoleColor.White ,
                ConsoleColor.DarkRed ) ,
            new ErrorMessagesInColor (
                ConsoleColor.Black ,
                ConsoleColor.Yellow )
        };  // static readonly ErrorMessagesInColor [ ] s_aclrs
        #endregion  // Static Arrays of Test Data


        #region Entry Point Routine and Its Storage
        static ApplicationMutex s_mutAppGuard = ApplicationMutex.GetTheSingleInstance ( FORMATTED_DATE_REPORT_FQFN );

        static Mutex s_mutGlobalMutex = s_mutAppGuard.AppMutex;

        static void Main ( string [ ] args )
        {
            const string ERRMSG_CALLER_NAME = @"Main";

            const string ERRMSG_INTERNAL_ERROR_TPL = @"INTERNAL ERROR: {0}{1}";
            const string ERRMSG_INTERNAL_INNER_MSG_TPL = @"Inner Exception: Message = {0}";
            const string ERRMSG_INTERNAL_SOURCE_TPL = @"Source = {0}{1}";
            const string ERRMSG_INTERNAL_STACKTRACE_TPL = @"{1}Stack Trace:{1}{0}{1}{1}";
            const string ERRMSG_INTERNAL_TARGETSITE_TPL = @"TargetSite = {0}{1}";
            const string ERRMSG_NEW_CLASS_TESTS_20140914 = @"ABORTING: Method NewClassTests_20140914.{0} failed, returning a status code of {1}";

            const string MSG_PROGRAM_BOJ = @"{0}, version {1}{2}Begin @ {3} ({4} UTC){2}";
            const string MSG_PROGRAM_EOJ = @"{0}Done{0}";

            //  ----------------------------------------------------------------
            //  Display a startup message on the console.
            //  ----------------------------------------------------------------

            int intTestNumber = StandardConstants.ZERO;

            WizardWrx.My me = new My ( );     // Get a reference to myself.

            DateTime dtmStart = DateTime.Now;
            Console.WriteLine (
                MSG_PROGRAM_BOJ ,
                new string [ ] {
                    me.NameVersionString ( My.HowManyFullNameParts.NameOnly ) ,
                    me.VersionString ,
                    Environment.NewLine ,
                    dtmStart.ToString ( ) ,
                    dtmStart.ToUniversalTime ( ).ToString ( ) } );

            uint uintSubsystemID = IP6CUtilLib1.GetProcessSubsystem ( );

            Console.WriteLine (
                "PE Subsystem = {0} ({1})" ,
                uintSubsystemID ,
                IP6CUtilLib1.XlateProcessSubsystem ( uintSubsystemID ) );

            StateManager appThisInstance = StateManager.GetTheSingleInstance ( );

            //  ----------------------------------------------------------------
            //  Initialize the new StateManager singleton, which is replacing 
            //  ApplicationInstance, which is too tighly coupled to classes in
            //  the ConsoleAppAids library.
            //  ----------------------------------------------------------------

            StateManager appStateManager = StateManager.GetTheSingleInstance ( );

            ExerciseTheStateManager ( appStateManager );

            //  ----------------------------------------------------------------
            //  Display information about the library under test.
            //  ----------------------------------------------------------------

            {   // Set scope boundaries around myLibUnderTest and the anonymous CmdLneArgsBasic object fed to its constructor.
                My myLibUnderTest = new My ( new CmdLneArgsBasic ( ) );
                ShowKeAssemblyProperties ( myLibUnderTest );
            }   // Allow myLibUnderTest to go out of scope, taking the anonymous CmdLneArgsBasic object with it.

            int intRetCode = StandardConstants.ERROR_SUCCESS;

            try
            {
                //  ------------------------------------------------------------
                //  Exercise the new utility classes. To simplify the backport,
                //  these tests get a dedicated class, NewClassTests_20140914.
                //  ------------------------------------------------------------

                intRetCode = NewClassTests_20140914.ArrayInfoExercises ( ref intTestNumber );

                if ( intRetCode != StandardConstants.ERROR_SUCCESS )
                    throw new Exception ( string.Format (
                        ERRMSG_NEW_CLASS_TESTS_20140914 ,
                        "ArrayInfoExercises" ,
                        intRetCode ) );

                intRetCode = NewClassTests_20140914.CSVFileInfoExercises ( ref intTestNumber );

                if ( intRetCode != StandardConstants.ERROR_SUCCESS )
                    throw new Exception ( string.Format (
                        ERRMSG_NEW_CLASS_TESTS_20140914 ,
                        "CSVFileInfoExercises" ,
                        intRetCode ) );

                intRetCode = NewClassTests_20140914.DisplayFormatsExercises ( ref intTestNumber );

                if ( intRetCode != StandardConstants.ERROR_SUCCESS )
                    throw new Exception ( string.Format (
                        ERRMSG_NEW_CLASS_TESTS_20140914 ,
                        "DisplayFormats" ,
                        intRetCode ) );

                intRetCode = NewClassTests_20140914.FileIOFlagsExercises ( ref intTestNumber );

                if ( intRetCode != StandardConstants.ERROR_SUCCESS )
                    throw new Exception ( string.Format (
                        ERRMSG_NEW_CLASS_TESTS_20140914 ,
                        "FileIOFlags" ,
                        intRetCode ) );

                intRetCode = NewClassTests_20140914.ListInfoExercises ( ref intTestNumber );

                if ( intRetCode != StandardConstants.ERROR_SUCCESS )
                    throw new Exception ( string.Format (
                        ERRMSG_NEW_CLASS_TESTS_20140914 ,
                        "ListInfo" ,
                        intRetCode ) );

                intRetCode = NewClassTests_20140914.PathPositionsExercises ( ref intTestNumber );

                if ( intRetCode != StandardConstants.ERROR_SUCCESS )
                    throw new Exception ( string.Format (
                        ERRMSG_NEW_CLASS_TESTS_20140914 ,
                        "PathPositions" ,
                        intRetCode ) );

                intRetCode = NewClassTests_20140914.SpecialCharactersExercises ( ref intTestNumber );

                if ( intRetCode != StandardConstants.ERROR_SUCCESS )
                    throw new Exception ( string.Format (
                        ERRMSG_NEW_CLASS_TESTS_20140914 ,
                        "SpecialCharacters" ,
                        intRetCode ) );

                intRetCode = NewClassTests_20140914.UtilsExercises ( ref intTestNumber );

                if ( intRetCode != StandardConstants.ERROR_SUCCESS )
                    throw new Exception ( string.Format (
                        ERRMSG_NEW_CLASS_TESTS_20140914 ,
                        "Util" ,
                        intRetCode ) );
            }
            catch ( Exception exUtilityClasses )
            {
                Console.WriteLine ( exUtilityClasses.Message );
                Environment.Exit ( intRetCode );
            }

            ExerciseExceptionLogger ( appStateManager );

            //  ----------------------------------------------------------------
            //  Display the command line arguments, as seen by the runtime.
            //  ----------------------------------------------------------------

            //-R1 /S2 -tLowerCase NamedArg1=NamedValue1 NamedArg2=NamedValue2 NAMEDARG3=NamedValue3

            DisplayRawArgs ( args );

            DisplayPublicConstants ( );

            //  ----------------------------------------------------------------
            //  Beginning with version 1.3, the entire test block is enclosed in
            //  a Try/Catch/Finally block.
            //  ----------------------------------------------------------------

            try
            {
                //  ------------------------------------------------------------
                //  Test a CmdLneArgsBasic object that supports ONLY positional
                //  arguments. This is the simplest kind, and the least useful.
                //  ------------------------------------------------------------

                {   // Set scope boundaries around CmdLneArgsBasic SimpleParser.
                    CmdLneArgsBasic SimpleParser = new CmdLneArgsBasic ( );
                    DisplayParsedArgs (
                        "Supports positional arguments only, and does case sensitive parsing" ,
                        SimpleParser ,
                        ref intTestNumber );
                }   // CmdLneArgsBasic object SimpleParser goes out of scope.

                //  ------------------------------------------------------------
                //  Test a CmdLneArgsBasic object that supports positional
                //  arguments and switches. This is probably the most useful
                //  implementation.
                //  ------------------------------------------------------------

                {   // Set scope boundaries around CmdLneArgsBasic SwitchParser.
                    CmdLneArgsBasic SwitchParser = new CmdLneArgsBasic ( s_achrSwitchSetAllUC );
                    DisplayParsedArgs (
                        "Supports positional arguments and switches, and does case sensitive parsing" ,
                        SwitchParser ,
                        ref intTestNumber );
                }   // CmdLneArgsBasic SwitchParser goes out of scope.

                //  ------------------------------------------------------------
                //  Test a CmdLneArgsBasic object that supports positional
                //  arguments and switches, including an invalid switch.
                //  ------------------------------------------------------------

                {   // Set scope boundaries around InvalidSwitchParser SwitchParser.
                    try
                    {
                        CmdLneArgsBasic InvalidSwitchParser = new CmdLneArgsBasic ( s_achrSwitchesWithDupicates );

                        DisplayParsedArgs (
                            "Supports positional arguments and switches, and does case sensitive parsing" ,
                            InvalidSwitchParser ,
                            ref intTestNumber );
                    }
                    catch ( Exception exAll )
                    {
                        Console.WriteLine ( 
                            "Exception caught while constructing InvalidSwitchParser{2}    Message = {0}{2}    Method = {1}{2}" ,
                            exAll.Message ,
                            exAll.TargetSite , 
                            Environment.NewLine );
                    }
                }   // CmdLneArgsBasic InvalidSwitchParser goes out of scope.

                //  ------------------------------------------------------------
                //  Test a CmdLneArgsBasic object that supports positional
                //  arguments and switches, including an invalid switch.
                //  ------------------------------------------------------------

                {   // Set scope boundaries around IntegerSwitchParser SwitchParser.
                    try
                    {
                        CmdLneArgsBasic IntegerSwitchParser = new CmdLneArgsBasic ( s_achrSwitchSetAllUC );

                        DisplayParsedArgs (
                            "Supports positional arguments and switches, and does case sensitive parsing" ,
                            IntegerSwitchParser ,
                            ref intTestNumber );
                        Console.WriteLine (
                            "    Querying switch Q with a default value of -1 returns {0}" ,
                            IntegerSwitchParser.GetSwitchByNameAsInt (
                                'Q' ,
                                -1 ) );
                        Console.WriteLine (
                            "    Querying switch R with a default value of -1 returns {0}" ,
                            IntegerSwitchParser.GetSwitchByNameAsInt (
                                'S' ,
                                -1 ) );
                    }
                    catch ( Exception exAll )
                    {
                        Console.WriteLine (
                            "Exception caught while constructing InvalidSwitchParser{2}    Message = {0}{2}    Method = {1}{2}" ,
                            exAll.Message ,
                            exAll.TargetSite ,
                            Environment.NewLine );
                    }
                }   // CmdLneArgsBasic IntegerSwitchParser goes out of scope.


                //  ------------------------------------------------------------
                //  Test a CmdLneArgsBasic object that supports positional
                //  arguments and switches. This is probably the most useful
                //  implementation for die-hard Windows programmers, because its
                //  parsing rules are case inssensitive.
                //  ------------------------------------------------------------

                {   // Set scope boundaries around CmdLneArgsBasic CaseInsensitiveSwitchParser.
                    CmdLneArgsBasic CaseInsensitiveSwitchParser = new CmdLneArgsBasic (
                        s_achrSwitchSetAllUC ,
                        CmdLneArgsBasic.ArgMatching.CaseInsensitive );
                    DisplayParsedArgs (
                        "Supports positional arguments and switches, and does case INsensitive parsing" ,
                        CaseInsensitiveSwitchParser ,
                        ref intTestNumber );
                }   // CmdLneArgsBasic SwitchParser goes out of scope.

                //  ------------------------------------------------------------
                //  Test a CmdLneArgsBasic object that supports all three kinds
                //  of arguments. This is the most versatile parser, period.
                //  ------------------------------------------------------------

                {   // Set scope boundaries around CmdLneArgsBasic FullyLoadedParser.
                    CmdLneArgsBasic FullyLoadedParser1 = new CmdLneArgsBasic (
                        s_achrSwitchSetAllUC ,
                        s_astrValidArgumentNames );
                    DisplayParsedArgs (
                        "Supports named and positional arguments and switches, and does case sensitive parsing" ,
                        FullyLoadedParser1 ,
                        ref intTestNumber );

                    Console.WriteLine (
                        "     GetArgByMultipleAliases, NAMED_ARG_UC_2 first = {0}" ,
                        FullyLoadedParser1.GetArgByMultipleAliases (
                            new string [ ]
                            {
                                NAMED_ARG_UC_2 ,
                                NAMED_ARG_UC_3
                            } ) );

                    //  ---------------------------------------------------------
                    //  Swap preferences, and repeat.
                    //  ---------------------------------------------------------

                    Console.WriteLine (
                        "     GetArgByMultipleAliases, NAMED_ARG_UC_3 first = {0}" ,
                        FullyLoadedParser1.GetArgByMultipleAliases (
                            new string [ ]
                            {
                                NAMED_ARG_UC_3 ,
                                NAMED_ARG_UC_2
                            } ) );
                }   // CmdLneArgsBasic FullyLoadedParser goes out of scope.

                //  ------------------------------------------------------------
                //  Test a CmdLneArgsBasic object that supports all three kinds
                //  of arguments, and implements case insensitive parsing. This
                //  is the most versatile parser for die-hard Window
                //  programmers.
                //  ------------------------------------------------------------

                {   // Set scope boundaries around CmdLneArgsBasic FullyLoadedWinCmdParser.
                    CmdLneArgsBasic FullyLoadedWinCmdParser = new CmdLneArgsBasic (
                        s_achrSwitchSetAllUC ,
                        s_astrValidArgumentNames ,
                        CmdLneArgsBasic.ArgMatching.CaseInsensitive );
                    DisplayParsedArgs (
                        "Supports named and positional arguments and switches, and does case INsensitive parsing" ,
                        FullyLoadedWinCmdParser ,
                        ref intTestNumber );
                }   // CmdLneArgsBasic FullyLoadedWinCmdParser goes out of scope.

                //  ------------------------------------------------------------
                //  Finally, test a CmdLineArgs object that supports all three
                //  kinds of arguments, and takes its lists of supported named
                //  arguments and switches from dictionaries of names and
                //  defaults. By making this object do  case sensitive parsing
                //  we get the default for the third named argument, as was the
                //  case in the next to last test above.
                //  ------------------------------------------------------------

                {   // Set scope boundaries around CmdLneArgsBasic FullyLoadedParser2.
                    Dictionary<string , string> dctNamedArgsWithDefaults = CreateNamedArgListWithDefaults ( );
                    Dictionary<char , string> dctSwitchesWithDefaults = CreateSwitchListWithDefaults ( );

                    CmdLneArgsBasic FullyLoadedParser2 = new CmdLneArgsBasic (
                        dctSwitchesWithDefaults ,
                        dctNamedArgsWithDefaults );
                    DisplayParsedArgs (
                        "Supports named and positional arguments and switches WITH DEFAULTS, and does case sensitive parsing" ,
                        FullyLoadedParser2 ,
                        ref intTestNumber );
                }   // CmdLneArgsBasic FullyLoadedParser2 goes out of scope.

                //  ------------------------------------------------------------
                //  Put the StateManager through its paces. This routine was
                //  created to do the same for ApplicationInstance, which the
                //  new StateManager class replaces.
                //  ------------------------------------------------------------

                DisplayStateManagerProperties ( appThisInstance );

                //  ------------------------------------------------------------
                //  Start with the method that returns a string for us to
                //  display or write somewhere.
                //  ------------------------------------------------------------

                appThisInstance.ConsoleMessageTimeFormat.CalendarDateFormat = TimeDisplayFormatter.DateFieldOrder.YYMMDD;
                appThisInstance.ConsoleMessageTimeFormat.HoursFormat = TimeDisplayFormatter.HoursFormatType.Civilian;
                appThisInstance.ShowUTCTime = false;

                //  ------------------------------------------------------------
                //  These methods moved out. Their new address is in the 
                //  ConsoleAppAids library.
                //  ------------------------------------------------------------

                //DisplayBannerString (
                //    BannerType.BOJ ,
                //    appThisInstance.GetBOJMessage ( ) );
                //DisplayBannerString (
                //    BannerType.EOJ ,
                //    appThisInstance.GetEOJMessage ( ) );

                //  ------------------------------------------------------------
                //  Repeat with the void method that generates the message and
                //  shows it on the console.
                //  ------------------------------------------------------------

                //DisplayBannerString (
                //    BannerType.BOJ ,
                //    string.Empty );
                //DisplayBannerString (
                //    BannerType.EOJ ,
                //    string.Empty );

                //  ------------------------------------------------------------
                //  Suppress the UTC flag, and redisplay the message. This
                //  time, use the void method, which calls the other method
                //  internally.
                //  ------------------------------------------------------------

                appThisInstance.ShowUTCTime = false;

                //DisplayBannerString (
                //    BannerType.BOJ ,
                //    string.Empty );
                //DisplayBannerString (
                //    BannerType.EOJ ,
                //    string.Empty );
            }
            catch ( Exception exAny )
            {
                StringBuilder sbMsg = new StringBuilder ( StandardConstants.SB_CAPACITY_08KB );

                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_ERROR_TPL ,
                    exAny.Message ,
                    Environment.NewLine );

                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_SOURCE_TPL ,
                    exAny.Source ,
                    Environment.NewLine );
                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_TARGETSITE_TPL ,
                    exAny.TargetSite ,
                    Environment.NewLine );

                // -------------------------------------------------------------
                // 2012/07/07 - DAG/WW - Add environment.NewLine to the argument
                //                       list.
                //
                //                       This same change should be applied to
                //                       the routine in TextLog.cs from which
                //                       this code is derived.
                // -------------------------------------------------------------

                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_STACKTRACE_TPL ,
                    exAny.StackTrace ,
                    Environment.NewLine );

                if ( exAny.InnerException != null )
                    sbMsg.AppendFormat (
                        ERRMSG_INTERNAL_INNER_MSG_TPL ,
                        exAny.InnerException.Message );

                string strMsg = sbMsg.ToString ( );

                Console.WriteLine ( strMsg );
                WinEventLogging.LogAppEvent (
                    ERRMSG_CALLER_NAME ,
                    strMsg );
            }
            finally
            {
                DateTime dtmTestTime = DateTime.Now;    // Use the same time for all tests.

                try
                {
                    //  ------------------------------------------------------------
                    //  Put the SysDateFormatters class through its paces.
                    //  ------------------------------------------------------------

                    ExerciseFormattedLocalTime ( dtmTestTime );
                }
                catch ( Exception errAllKinds )
                {
                    appThisInstance.AppExceptionLogger.ReportException (
                        errAllKinds ,
                        ERRMSG_CALLER_NAME );
                }

                try
                {
                    //  ------------------------------------------------------------
                    //  Put the ExerciseSysDateFormatters class through its paces.
                    //  ------------------------------------------------------------

                    ExerciseSysDateFormatters (
                        dtmTestTime ,
                        DateFormatter.ReformatSysDate );
                    ExerciseSysDateFormatters (
                        dtmTestTime ,
                        DateFormatter.ReformatNow );
                    ExerciseSysDateFormatters (
                        dtmTestTime ,
                        DateFormatter.ReformatUtcNow );
                }
                catch ( Exception errAllKinds )
                {
                    appThisInstance.AppExceptionLogger.ReportException (
                        errAllKinds ,
                        ERRMSG_CALLER_NAME );
                }

                //  ------------------------------------------------------------
                //  Exercise GetExeSubsystem.
                //  ------------------------------------------------------------

                ExerciseGetExeSubsystem ( );

                //  ------------------------------------------------------------
                //  Exercise the INI file reader.
                //  ------------------------------------------------------------

                ExerciseIniReader ( TEST_PRIVATE_PROFILE_FILE );

                //  ------------------------------------------------------------
                //  Exercise the colorized exception reporting. 
                //
                //  Since the ConsoleAppAids class tests this feature more 
                //  exhaustively, this test is pretty limited, though it should
                //  be entirely adequate.
                //  ------------------------------------------------------------

                ExerciseExceptionsInColor ( );

                //  ------------------------------------------------------------
                //  Make a final report, including elapsed time for the job.
                //  ------------------------------------------------------------

                DateTime dtmFinish = DateTime.Now;
                string strElapsedTime = WizardWrx.DateMath.ElapsedTime.ElapsedTimeDisp (
                    dtmStart ,
                    dtmFinish ,
                    WizardWrx.DateMath.ElapsedTime.TIME_SPAN_FORMAT_HOURS_PRECISE );
                Console.WriteLine (
                    "{0}{1} End{0}{2} ({3} UTC){0}Elapsed time: {4}" ,
                    new string [ ]
                    {
                        Environment.NewLine ,                                                   // Token 0
                        me.NameVersionString ( WizardWrx.My.HowManyFullNameParts.NameOnly ) ,   // Token 1
                        dtmFinish.ToString ( ) ,                                                // Token 2
                        dtmFinish.ToUniversalTime ( ).ToString ( ) ,                            // Token 3
                        strElapsedTime                                                          // Token 4
                    } );

                //  ------------------------------------------------------------
                //  Since the argumentless overload calls the overload that
                //  takes a string, passing a null reference (which is legal),
                //  calling the simpler method tests both.
                //  ------------------------------------------------------------

                //if ( System.Diagnostics.Debugger.IsAttached )
                //    DisplayAids.WaitForCarbonUnit ( );
                //else
                Console.WriteLine (
                    MSG_PROGRAM_EOJ ,
                    Environment.NewLine );
                Environment.Exit ( WizardWrx.StandardConstants.ERROR_SUCCESS );
            }   // finally block
        }   // private static void Main
        #endregion  // Entry Point Routine and Its Storage


        #region Top Level Subroutines
        private static void DisplayStateManagerProperties ( StateManager pappThisInstance )
        {
            Console.WriteLine ( @"{0}Enumerate the initial values of the StateManager{0}" , Environment.NewLine );

            Console.WriteLine ( @"     AppReturnCode               = {0}" , pappThisInstance.AppReturnCode );

            //  ----------------------------------------------------------------
            //  The BoJMessageGenerated property moved out with the rest of the 
            //  stuff that applies exclusively to character mode applications.
            //  ----------------------------------------------------------------

            //            Console.WriteLine ( @"     BoJMessageGenerated         = {0}" , pappThisInstance.BoJMessageGenerated );
            Console.WriteLine ( @"     InitialWorkingDirectoryName = {0}" , pappThisInstance.InitialWorkingDirectoryName );
            Console.WriteLine ( @"     ShowUTCTime                 = {0}" , pappThisInstance.ShowUTCTime );

            //  ----------------------------------------------------------------
            //  2011/10/08 - New properties expose more of the underlying My
            //               object.
            //  ----------------------------------------------------------------

            Console.WriteLine ( @"     AppRootAssemblyFileBaseName = {0}" , pappThisInstance.AppRootAssemblyFileBaseName );
            Console.WriteLine ( @"     AppRootAssemblyFileDirName  = {0}" , pappThisInstance.AppRootAssemblyFileDirName );
            Console.WriteLine ( @"     AppRootAssemblyFileName     = {0}" , pappThisInstance.AppRootAssemblyFileName );
            Console.WriteLine ( @"     AppStartupTimeLocal         = {0}" , pappThisInstance.AppStartupTimeLocal );
            Console.WriteLine ( @"     AppStartupTimeUtc           = {0}" , pappThisInstance.AppStartupTimeUtc );
        }   // private static void DisplayStateManagerProperties


        //private static void DisplayBannerString (
        //    BannerType penmBannerType ,
        //    string pstrBannerText )
        //{
        //    const string BOJ_BANNER_BEGIN = @"{1}{0}BOJ Banner Begin:{1}{0}";
        //    const string EOJ_BANNER_BEGIN = @"{1}{0}EOJ Banner Begin:{1}{0}";

        //    const string END_OF_BANNER = @"{0}End of Banner{1}{0}";
        //    const string INTERNAL_ERROR_MSG_TPL = @"{2}{1}{2}Internal error: Function DisplayBannerString argument penmBannerType is invalid.{2}penmBannerType = {0}{1}{2}";
        //    const string RULE_EIGHTY_CHARS = @"--------------------------------------------------------------------------------";

        //    bool fValidBannerType = true;

        //    switch ( penmBannerType )
        //    {
        //        case BannerType.BOJ:
        //            Console.WriteLine (
        //                BOJ_BANNER_BEGIN ,          // Message Template
        //                RULE_EIGHTY_CHARS ,         // Token 0
        //                Environment.NewLine );      // Token 1
        //            break;

        //        case BannerType.EOJ:
        //            Console.WriteLine (
        //                EOJ_BANNER_BEGIN ,          // Message Template
        //                RULE_EIGHTY_CHARS ,         // Token 0
        //                Environment.NewLine );      // Token 1
        //            break;

        //        default:
        //            Console.WriteLine (
        //                INTERNAL_ERROR_MSG_TPL ,    // Message Template
        //                penmBannerType ,            // Token 0
        //                RULE_EIGHTY_CHARS ,         // Token 1
        //                Environment.NewLine );      // Token 2
        //            fValidBannerType = false;
        //            break;
        //    }   // switch ( penmBannerType )

        //    if ( fValidBannerType )
        //    {   //  The routine could just exit, but that would give it two exits.
        //        if ( string.IsNullOrEmpty ( pstrBannerText ) )
        //        {
        //            ApplicationInstance appThisInstance = ApplicationInstance.GetTheSingleInstance ( );

        //            switch ( penmBannerType )
        //            {   // Using the BannerType with a switch block gives me the flexibility to add messages to the test.
        //                case BannerType.BOJ:
        //                    appThisInstance.DisplayBOJMessage ( );
        //                    break;
        //                case BannerType.EOJ:
        //                    appThisInstance.DisplayEOJMessage ( );
        //                    break;
        //            }   // switch ( penmBannerType )
        //        }   // TRUE block, if ( string.IsNullOrEmpty ( pstrBannerText ) )
        //        else
        //        {   // Caller already got the message.
        //            Console.WriteLine ( pstrBannerText );
        //        }   // FALSE block, if ( string.IsNullOrEmpty ( pstrBannerText ) )

        //        Console.WriteLine (
        //            END_OF_BANNER ,                     // Message Template
        //            RULE_EIGHTY_CHARS ,                 // Token 0
        //            Environment.NewLine );              // Token 1
        //    }   // if ( fValidBannerType )
        //}   // private static void DisplayBannerString


        private static void DisplayParsedArgs (
            string pstrTestLabel ,
            CmdLneArgsBasic pCmdLineArgs ,
            ref int pintTestNumber )
        {
            const string EXERCISE_TPL_BEGIN = @"Test {0}: Exercising A CmdLneArgsBasic object with the following capabilities: {1}{2}";
            const string EXERCISE_TPL_END = @"{1}Finished exercising CmdLneArgsBasic object {0}{1}";

            Console.WriteLine (
                EXERCISE_TPL_BEGIN ,        // Format string (template)
                ++pintTestNumber ,          // Substituted for token 0
                pstrTestLabel ,             // Substituted for token 1
                Environment.NewLine );      // Substituted for token 2

            DisplayReadOnlyProperties ( pCmdLineArgs );
            EmumerateDefinedSwitches (
                pCmdLineArgs ,
                s_achrSwitchSetAllUC );
            EnumerateDefinedNamedArguments (
                pCmdLineArgs ,
                s_astrValidArgumentNames );
            EnumeratePositionalArgs ( pCmdLineArgs );

            Console.WriteLine (             // Format string (template)
                EXERCISE_TPL_END ,          // Substituted for token 0
                pCmdLineArgs ,              // Substituted for token 1
                Environment.NewLine );      // Substituted for token 2
        }   // private static void DisplayParsedArgs method


        private static void DisplayPublicConstants ( )
        {
            const string MSG_BEGIN = "\r\nBegin display of public constants exposed by a CmdLneArgsBasic object:\r\n";
            const string MSG_END = "\r\nEnd of public constants\r\n";

            Console.WriteLine ( MSG_BEGIN );

            Console.WriteLine ( @"     ARG_LIST_HAS_ARGS           = {0}" , CmdLneArgsBasic.ARG_LIST_HAS_ARGS );
            Console.WriteLine ( @"     ARG_LIST_IS_EMPTY           = {0}" , CmdLneArgsBasic.ARG_LIST_IS_EMPTY );
            Console.WriteLine ( @"     FIRST_POSITIONAL_ARG        = {0}" , CmdLneArgsBasic.FIRST_POSITIONAL_ARG );
            Console.WriteLine ( @"     NAME_VALUE_DELIMITER        = {0}" , CmdLneArgsBasic.NAME_VALUE_DELIMITER );
            Console.WriteLine ( @"     NONE                        = {0}" , CmdLneArgsBasic.NONE );
            Console.WriteLine ( @"     POSITIONAL_ARGS_COUNT_LIMIT = {0}" , CmdLneArgsBasic.POSITIONAL_ARGS_COUNT_LIMIT );
            Console.WriteLine ( @"     SWITCH_IS_OFF               = {0}" , CmdLneArgsBasic.SWITCH_IS_OFF );
            Console.WriteLine ( @"     SWITCH_IS_ON                = {0}" , CmdLneArgsBasic.SWITCH_IS_ON );
            Console.WriteLine ( @"     VALUE_NOT_SET               = {0}" , CmdLneArgsBasic.VALUE_NOT_SET );

            Console.WriteLine ( MSG_END );
        }   // private static void DisplayPublicConstants


        private static void DisplayRawArgs ( string [ ] pastrArgs )
        {
            const string ARG_COUNT_MSG_TPL = @"     Command line Argument Count = {0}";
            const string RAW_ARGS_DISPLAY_BEGIN = @"{0}Command line arguments, as seen by the Common Language Runtime:{0}";
            const string RAW_ARGS_DISPLAY_END = @"{0}End of report on raw command line arguments.{0}";
            const string RAW_ARGS_EMPTY_MSG = @"{0}     The command line argument list is empty.{0}";
            const string RAW_ARGS_DETAIL_HEADING = @"{0}     Arguments, in order of appearance, are as follows:{0}";
            const string RAW_ARGS_DETAIL_ITEM = @"          Argument # {0} = {1}";

            Console.WriteLine (
                RAW_ARGS_DISPLAY_BEGIN ,
                Environment.NewLine );

            int intNArgs = pastrArgs.Length;

            Console.WriteLine (
                ARG_COUNT_MSG_TPL ,
                intNArgs );

            if ( intNArgs > StandardConstants.ZERO )
            {   // Command line contains at least one argument.
                Console.WriteLine (
                    RAW_ARGS_DETAIL_HEADING ,
                    Environment.NewLine );

                int intArgOrdinal = StandardConstants.ZERO;

                foreach ( string strThisArg in pastrArgs )
                {   // Microsoft .NET arrays implement an IEnumerable interface.
                    Console.WriteLine (
                        RAW_ARGS_DETAIL_ITEM ,
                        ++intArgOrdinal ,
                        strThisArg );
                }   // foreach ( string strThisArg in pastrArgs )
            }   // TRUE (normal case) block, if ( intNArgs > StandardConstants.ZERO )
            else
            {   // Cmmand line is minimal.
                Console.WriteLine (
                    RAW_ARGS_EMPTY_MSG ,
                    Environment.NewLine );
            }   // FALSE (degenerate case) block, if ( intNArgs > StandardConstants.ZERO )

            Console.WriteLine (
                RAW_ARGS_DISPLAY_END ,
                Environment.NewLine );
        }   // private static void DisplayRawArgs method


        private static void ExerciseExceptionLogger ( StateManager pappStateManager )
        {
            const string DUMMY_ARGNAME = @"DummyArgName";
            const int DUMMY_HRESULT = -2147467259;    // E_FAIL = 0x80004005
            const string DUMMY_OBJECTNAME = @"DummyObjectName";
            const string DUMMY_VALUE = @"ActualValue";

            const string METHOD_BOJ = "Starting";
            const string METHOD_EOJ = @"Completed";
            const string METHOD_MSG = @"{2}{0} {1}{2}";
            const string METHOD_NAME = @"ExerciseExceptionLogger";

            const string STANDARD_ERROR_MESSAGE = @"Test {0}: This is a test of the ExceptionLogger class. using an exception of type {1}.";
            const string STANDARD_REFRAIN_MESSAGE = @"{1}Test {0} completed{1}";

            const string MSG_STACK_DISPOSITION = @"enmStackDisp     = {0} ({1})";
            const string MSG_APP_SUBSYSTEM = @"enmSubsystem     = {0} ({1})";
            const string MSG_EVENTLOG_USAGE = @"enmEventLogUsage = {0} ({1})";
            const string MSG_OPTION_FLAGS_DISP = @"OptionFlags {0} = {1}";
            const string MSG_ANTE = @"Ante";
            const string MSG_POST = @"Post";

            int intTestNumber = StandardConstants.ZERO;

            ExceptionLogger theLogger = pappStateManager.AppExceptionLogger;

            Console.WriteLine (
                METHOD_MSG ,
                METHOD_NAME ,
                METHOD_BOJ ,
                Environment.NewLine );

            foreach ( ExceptionTypes enmExType in s_aenmExceptionTypes )
            {
                string strExceptionTypeMsg = WriteExceptionGroupBanner ( enmExType );

                foreach ( ExceptionLogger.OutputOptions enmStackDisp in s_aenmStackTraceDisp )
                {   // To keep its value in bounds, this index must be reinitialized on each loop.
                    int intSubsystemIndex = ArrayInfo.ARRAY_INVALID_INDEX;

                    foreach ( ExceptionLogger.OutputOptions enmSubsystem in s_aenmAppSubsystem )
                    {   // ExceptionLogger.Subsystem collapses to one of only three values.
                        intSubsystemIndex++;    // This index points to a literal representation for the report.

                        foreach ( ExceptionLogger.OutputOptions enmEventLogUsage in s_aenmLoggingState )
                        {
                            Console.WriteLine (
                                MSG_STACK_DISPOSITION ,
                                enmStackDisp ,
                                ( int ) enmStackDisp );
                            Console.WriteLine (
                                MSG_APP_SUBSYSTEM ,
                                s_astrAppSubSystemDisp [ intSubsystemIndex ] ,
                                ( int ) enmSubsystem );
                            Console.WriteLine (
                                MSG_EVENTLOG_USAGE ,
                                enmEventLogUsage ,
                                ( int ) enmEventLogUsage );

                            Console.WriteLine (
                                MSG_OPTION_FLAGS_DISP ,
                                MSG_ANTE ,
                                theLogger.OptionFlags );

                            BitArray32 b32Flags = new BitArray32 ( ( UInt32 ) theLogger.OptionFlags );

                            if ( ( enmSubsystem & ExceptionLogger.OutputOptions.StandardError ) == ExceptionLogger.OutputOptions.StandardError )
                                b32Flags.BitOn ( ( int ) BitArray32.BitNumber ( ( UInt32 ) ExceptionLogger.OutputOptions.StandardError ) );
                            else
                                b32Flags.BitOff ( ( int ) BitArray32.BitNumber ( ( UInt32 ) ExceptionLogger.OutputOptions.StandardError ) );

                            if ( ( enmStackDisp & ExceptionLogger.OutputOptions.Stack ) == ExceptionLogger.OutputOptions.Stack )
                                b32Flags.BitOn ( ( int ) BitArray32.BitNumber ( ( UInt32 ) ExceptionLogger.OutputOptions.Stack ) );
                            else
                                b32Flags.BitOff ( ( int ) BitArray32.BitNumber ( ( UInt32 ) ExceptionLogger.OutputOptions.Stack ) );

                            if ( ( enmEventLogUsage & ExceptionLogger.OutputOptions.EventLog ) == ExceptionLogger.OutputOptions.EventLog )
                                b32Flags.BitOn ( ( int ) BitArray32.BitNumber ( ( UInt32 ) ExceptionLogger.OutputOptions.EventLog ) );
                            else
                                b32Flags.BitOff ( ( int ) BitArray32.BitNumber ( ( UInt32 ) ExceptionLogger.OutputOptions.EventLog ) );

                            theLogger.OptionFlags = ( ExceptionLogger.OutputOptions ) ( ( UInt32 ) b32Flags );

                            Console.WriteLine (
                                MSG_OPTION_FLAGS_DISP ,
                                MSG_POST ,
                                theLogger.OptionFlags );

                            try
                            {
                                string strMsg = string.Format (
                                    STANDARD_ERROR_MESSAGE ,
                                    ++intTestNumber ,
                                    strExceptionTypeMsg );

                                switch ( enmExType )
                                {
                                    case ExceptionTypes.ArgumentExceptionTest:
                                        throw new ArgumentException (
                                            strMsg ,            // message
                                            DUMMY_ARGNAME );    // paranMane

                                    case ExceptionTypes.ArgumentNullExceptionTest:
                                        throw new ArgumentNullException (
                                            DUMMY_ARGNAME ,     // paranMane
                                            strMsg );           // message

                                    case ExceptionTypes.ArgumentOutOfRangeExceptionTest:
                                        throw new ArgumentOutOfRangeException (
                                            DUMMY_ARGNAME ,     // paranMane
                                            DUMMY_VALUE ,       // actualValue
                                            strMsg );           // message

                                    case ExceptionTypes.ExceptionTest:
                                        throw new Exception ( strMsg );

                                    case ExceptionTypes.FormatExceptionTest:
                                        throw new FormatException ( strMsg );

                                    case ExceptionTypes.IOExceptionTest:
                                        throw new IOException (
                                            strMsg ,            // message
                                            DUMMY_HRESULT );    // HRESULT

                                    case ExceptionTypes.ObjectDisposedExceptionTest:
                                        throw new ObjectDisposedException (
                                            DUMMY_OBJECTNAME ,  // objectName
                                            strMsg );           // message

                                    default:
                                        throw new Exception ( string.Format (
                                            Properties.Resources.EekMsg ,
                                            enmExType ) );
                                }   // switch ( enmExType )
                            }   // The normal flow of execution resumes at the Finally block.
                            catch ( ArgumentOutOfRangeException exBadArg )
                            {
                                Console.WriteLine (
                                    theLogger.ReportException (
                                        exBadArg ,
                                        METHOD_NAME ) );
                            }
                            catch ( ArgumentNullException exNullArg )
                            {
                                Console.WriteLine (
                                    theLogger.ReportException (
                                        exNullArg ,
                                        METHOD_NAME ) );
                            }
                            catch ( ArgumentException exArg )
                            {
                                Console.WriteLine (
                                    theLogger.ReportException (
                                        exArg ,
                                        METHOD_NAME ) );
                            }
                            catch ( ObjectDisposedException exDisposed )
                            {
                                Console.WriteLine (
                                    theLogger.ReportException (
                                        exDisposed ,
                                        METHOD_NAME ) );
                            }
                            catch ( IOException exIO )
                            {
                                Console.WriteLine (
                                    theLogger.ReportException (
                                        exIO ,
                                        METHOD_NAME ,
                                        new FileInfo ( Properties.Settings.Default.ReadOnlyTestFile ) ) );
                            }
                            catch ( FormatException exFormat )
                            {
                                Console.WriteLine (
                                    theLogger.ReportException (
                                        exFormat ,
                                        METHOD_NAME ,
                                        Properties.Settings.Default.DeliberatelyBadFormatString ) );
                            }
                            catch ( Exception exGeneric )
                            {
                                Console.WriteLine (
                                    theLogger.ReportException (
                                        exGeneric ,
                                        METHOD_NAME ) );
                            }
                            finally
                            {   // Report completion of the test.
                                Console.WriteLine (
                                    STANDARD_REFRAIN_MESSAGE ,
                                    intTestNumber ,
                                    Environment.NewLine );
                            }   // Try/Catch/Finally block
                        }   // foreach ( ExceptionLogger.RecordinEventLog enmEventLogUsage in _aenmLoggingState )
                    }   // foreach ( ExceptionLogger.Subsystem enmSubsystem in _aenmAppSubsystem )
                }   // foreach ( ExceptionLogger.StackTraceDisposition enmStackDisp in _aenmStackTraceDisp )
            }   // foreach ( ExceptionTypes enmExType in s_aenmExceptionTypes )

            Console.WriteLine (
                METHOD_MSG ,
                METHOD_NAME ,
                METHOD_EOJ ,
                Environment.NewLine );
        }   // private static void ExerciseExceptionLogger

        
        private static void ExerciseExceptionsInColor ( )
        {
            const string DEFAULT_COLOR = @"default";
            const string MSG_BEGIN = @"Begin";
            const string MSG_END = @"End";
            const string MSG_TEST_INFO = @"{3}Full Color Exception Reporting Test {0} of {1} {2}{3}";
            const string MSG_TEXT_TPL = @"This message is displayed in {0} text on a {1} background.";

            string strMethodName = System.Reflection.MethodBase.GetCurrentMethod ( ).Name;

            StateManager theApp = StateManager.GetTheSingleInstance ( );
            ExceptionLogger theLogger = theApp.AppExceptionLogger;

            theLogger.OptionFlags = theLogger.OptionFlags | ExceptionLogger.OutputOptions.StandardOutput;
            theLogger.OptionFlags = theLogger.OptionFlags | ( ExceptionLogger.OutputOptions.Stack & OUTPUT_OPTIONS_ALL_OFF );

            int intCase = StandardConstants.ZERO;

            foreach ( ErrorMessagesInColor mc in s_aclrs )
            {
                theLogger.ErrorMessageColors = mc;
                StringBuilder sbMsg = new StringBuilder ( StandardConstants.SB_CAPACITY_01KB );

                if ( mc != null )
                {
                    sbMsg.AppendFormat (
                        MSG_TEXT_TPL ,
                        mc.MessageForegroundColor ,
                        mc.MessageBackgroundColor );
                }   // TRUE (normal case) block, if ( mc != null )
                else
                {
                    sbMsg.AppendFormat (
                        MSG_TEXT_TPL ,
                        DEFAULT_COLOR ,
                        DEFAULT_COLOR );
                }   // FALSE (degenerate case) block, if ( mc != null )

                Console.WriteLine (
                    MSG_TEST_INFO ,
                    new object [ ]
                        {
                            ++intCase ,
                            s_aclrs.Length ,
                            MSG_BEGIN ,
                            Environment.NewLine
                        } );

                try
                {
                    throw new Exception ( sbMsg.ToString ( ) );
                }
                catch ( Exception exGeneral )
                {
                    theLogger.ReportException (
                        exGeneral ,
                        strMethodName );
                }
                finally
                {
                    Console.WriteLine (
                        MSG_TEST_INFO ,
                        new object [ ]
                        {
                            intCase ,
                            s_aclrs.Length ,
                            MSG_END ,
                            Environment.NewLine
                        } );
                }   // try/catch/finally block
            }   // foreach ( ErrorMessagesInColor mc in s_aclrs )
        }   // private static void ExerciseExceptionsInColor
        #endregion  // Top Level Subroutines


        #region Helper Routines
        private static Dictionary<string , string> CreateNamedArgListWithDefaults ( )
        {
            const string ERRMSG_OUT_OF_BALANCE = @"The arrays of argument names and their default values are unbalanced.{2}     Size of names array, astrValidArgumentNames = {0}{2}     Size of defaults array, astrNamedArgDefaults = {1}";

            Dictionary<string , string> rdctOfNamedArgsWithDefaults = new Dictionary<string , string> ( );

            if ( s_astrValidArgumentNames.Length != s_astrNamedArgDefaults.Length )
            {   // This is a serious coding error.
                string strErrorMsg = string.Format (
                    ERRMSG_OUT_OF_BALANCE ,
                    s_astrValidArgumentNames.Length ,
                    s_astrNamedArgDefaults.Length ,
                    Environment.NewLine );
                throw new Exception ( strErrorMsg );
            }   // if ( astrValidArgumentNames.Length != astrNamedArgDefaults.Length )

            int intArgCount = s_astrValidArgumentNames.Length;

            for (
                int intCurrArgIndex = StandardConstants.ARRAY_FIRST_ELEMENT ;
                intCurrArgIndex < intArgCount ;
                intCurrArgIndex++
                )
            {
                rdctOfNamedArgsWithDefaults.Add (
                    s_astrValidArgumentNames [ intCurrArgIndex ] ,
                    s_astrNamedArgDefaults [ intCurrArgIndex ] );
            }   // for ( int intCurrArgIndex = StandardConstants.ARRAY_FIRST_ELEMENT ; ...

            return rdctOfNamedArgsWithDefaults;
        }   // private static Dictionary<string , string> CreateNamedArgListWithDefaults method


        private static Dictionary<char , string> CreateSwitchListWithDefaults ( )
        {
            const string ERRMSG_OUT_OF_BALANCE = @"The arrays of switch names and their default values are unbalanced.{2}     Size of names array, achrSwitchSetAllUC = {0}{2}     Size of defaults array, astrSwitchDefaults = {1}";

            Dictionary<char , string> rdctOfSwitchesWithDefaults = new Dictionary<char , string> ( );

            if ( s_achrSwitchSetAllUC.Length != s_astrSwitchDefaults.Length )
            {   // This is a serious coding error.
                string strErrorMsg = string.Format (
                    ERRMSG_OUT_OF_BALANCE ,
                    s_achrSwitchSetAllUC.Length ,
                    s_astrSwitchDefaults.Length ,
                    Environment.NewLine );
                throw new Exception ( strErrorMsg );
            }   // if ( astrValidArgumentNames.Length != astrNamedArgDefaults.Length )

            int intSwitchCount = s_achrSwitchSetAllUC.Length;

            for ( int intCurrSwitchIndex = StandardConstants.ARRAY_FIRST_ELEMENT ;
                      intCurrSwitchIndex < intSwitchCount ;
                      intCurrSwitchIndex++
                )
            {
                rdctOfSwitchesWithDefaults.Add (
                    s_achrSwitchSetAllUC [ intCurrSwitchIndex ] ,
                    s_astrSwitchDefaults [ intCurrSwitchIndex ] );
            }   // for ( int intCurrSwitchIndex = StandardConstants.ARRAY_FIRST_ELEMENT ; ...

            return rdctOfSwitchesWithDefaults;
        }   // private static Dictionary<char , string> CreateSwitchListWithDefaults method


        private static void DisplayReadOnlyProperties ( CmdLneArgsBasic pcmdLineArgs )
        {
            const string ARG_IS_NULL_MSG = @"     Method DisplayReadOnlyProperties was called with a null reference.";
            const string PROP_MSG_BEGIN = @"     Begin Read-Only Properties of object {0}{1}";
            const string PROP_MSG_END = @"{0}     End of Read-Only Properties{0}";

            const string PROP_NAME_COUNT = @"Count";

            const string PROP_NAME_ARG_LIST_IS_EMPTY = @"ArgListIsEmpty";
            const string PROP_NAME_ARG_MATCHING_METHOD = @"ArgumentMatching";
            const string PROP_NAME_ARGUMENT_TYPE_ARRAY = @"ArgumentTypeArray";

            const string PROP_NAME_DEFINED_NAMED_ARGS = @"DefinedNamedArgs";
            const string PROP_NAME_DEFINED_SWITCHES_ARGS = @"DefinedSwitches";

            const string PROP_NAME_INVALID_NAMED_ARGS = @"InvalidNamedArgsInCmd";
            const string PROP_NAME_INVALID_SWITCHES_ARGS = @"InvalidSwitchesInCmd";

            const string PROP_NAME_POSITIONAL_ARGS_IN_CMD = @"PositionalArgsInCmdLine";

            const string PROP_NAME_VALID_NAMED_ARGS = @"ValidNamedArgsInCmd";
            const string PROP_NAME_VALID_SWITCHES_ARGS = @"ValidSwitchesInCmd";

            const string PROP_VALUE_TPL = @"          {0} = {1}";
            const string PROP_ARRAY_SIZE_TPL = @"{2}          {0} is an array of {1} elements, as follows.{2}";
            const string PROP_ARRAY_ELEMENT_TPL = @"               Element {0} = {1}";

            if ( pcmdLineArgs == null )
            {   // Save the NullReferenceException for when you really need it.
                Console.Write ( ARG_IS_NULL_MSG );
            }   // TRUE (abnormal) block, if ( pcmdLineArgs == null )
            else
            {   // Display the properties in a formatted listing.
                Console.WriteLine (
                    PROP_MSG_BEGIN ,
                    pcmdLineArgs ,
                    Environment.NewLine );

                Console.WriteLine (
                    PROP_VALUE_TPL ,
                    PROP_NAME_COUNT ,
                    pcmdLineArgs.Count );

                Console.WriteLine (
                    PROP_VALUE_TPL ,
                    PROP_NAME_ARG_LIST_IS_EMPTY ,
                    pcmdLineArgs.ArgListIsEmpty );
                Console.WriteLine (
                    PROP_VALUE_TPL ,
                    PROP_NAME_ARG_MATCHING_METHOD ,
                    pcmdLineArgs.ArgumentMatching );

                Console.WriteLine (
                    PROP_VALUE_TPL ,
                    PROP_NAME_DEFINED_NAMED_ARGS ,
                    pcmdLineArgs.DefinedNamedArgs );
                Console.WriteLine (
                    PROP_VALUE_TPL ,
                    PROP_NAME_DEFINED_SWITCHES_ARGS ,
                    pcmdLineArgs.DefinedSwitches );

                Console.WriteLine (
                    PROP_VALUE_TPL ,
                    PROP_NAME_INVALID_NAMED_ARGS ,
                    pcmdLineArgs.InvalidNamedArgsInCmd );
                Console.WriteLine (
                    PROP_VALUE_TPL ,
                    PROP_NAME_INVALID_SWITCHES_ARGS ,
                    pcmdLineArgs.InvalidSwitchesInCmd );

                Console.WriteLine (
                    PROP_VALUE_TPL ,
                    PROP_NAME_POSITIONAL_ARGS_IN_CMD ,
                    pcmdLineArgs.PositionalArgsInCmdLine );

                Console.WriteLine (
                    PROP_VALUE_TPL ,
                    PROP_NAME_VALID_NAMED_ARGS ,
                    pcmdLineArgs.ValidNamedArgsInCmdLine );
                Console.WriteLine (
                    PROP_VALUE_TPL ,
                    PROP_NAME_VALID_SWITCHES_ARGS ,
                    pcmdLineArgs.ValidSwitchesInCmdLine );

                int intArgPosition = StandardConstants.MINUS_ONE;

                try
                {
                    Console.WriteLine (
                        PROP_ARRAY_SIZE_TPL ,                       // Template string
                        PROP_NAME_ARGUMENT_TYPE_ARRAY ,             // Token 0
                        pcmdLineArgs.ArgumentTypeArray.Length ,     // Token 1
                        Environment.NewLine );                      // Token 2
                    intArgPosition = StandardConstants.ZERO;
                }
                catch ( Exception errAllKinds )
                {
                    Console.WriteLine ( @"          {0}" , errAllKinds.Message );
                }

                if ( pcmdLineArgs.ArgListIsEmpty == CmdLneArgsBasic.ARG_LIST_HAS_ARGS )
                {   // Display a summary, followed by an enumeration.
                    if ( intArgPosition == StandardConstants.MINUS_ONE )
                    {
                        Console.WriteLine (
                            PROP_ARRAY_SIZE_TPL ,                       // Template string
                            PROP_NAME_ARGUMENT_TYPE_ARRAY ,             // Token 0
                            pcmdLineArgs.ArgumentTypeArray.Length ,     // Token 1
                            Environment.NewLine );                      // Token 2
                        intArgPosition = StandardConstants.ZERO;
                    }   // if ( intArgPosition == StandardConstants.MINUS_ONE )

                    foreach ( CmdLneArgsBasic.ArgType enmArgType in pcmdLineArgs.ArgumentTypeArray )
                    {   // Use the implicit array enumerator.
                        Console.WriteLine (
                            PROP_ARRAY_ELEMENT_TPL ,                // Template string
                            ++intArgPosition ,                      // Token 0
                            enmArgType );                           // Token 1
                    }   // foreach ( CmdLneArgsBasic.ArgType enmArgType in pcmdLineArgs.ArgumentTypeArray )
                }   // TRUE (normal) case, if ( pcmdLineArgs.ArgListIsEmpty == CmdLneArgsBasic.ARG_LIST_HAS_ARGS )
                else
                {
                    Console.WriteLine ( @"          The command line is devoid of arguments." );
                }   // FALSE (degenerate) case, if ( pcmdLineArgs.ArgListIsEmpty == CmdLneArgsBasic.ARG_LIST_HAS_ARGS )

                Console.WriteLine (
                    PROP_MSG_END ,
                    Environment.NewLine );
            }   // FALSE (normal) block, if ( pcmdLineArgs == null )
        }   // private static void DisplayReadOnlyProperties


        private static void EnumeratePositionalArgs ( CmdLneArgsBasic pcmdLineArgs )
        {
            const string ARG_DISP_TPL_COUNT = @"{1}     Count of positional arguments = {0}{1}";
            const string ARG_DISP_TPL_END = @"{0}     End of positional argument list{0}";
            const string ARG_DISP_TPL_POSITIONAL = @"          Positional argument # {0} = {1}";
            const string MSG_NO_POSITIONAL_ARGS = @"     The command line contains no positional arguments.";

            //bool fMoreArgs = true;
            int intArgOrdinal = StandardConstants.ZERO;

            //  ----------------------------------------------------------------
            //  This is the old way, before I had these properties to query.
            //  ----------------------------------------------------------------

            //while ( fMoreArgs )
            //{
            //    string strThisArg = pcmdLineArgs.GetArgByPosition ( ++intArgOrdinal );

            //    if ( strThisArg == CmdLneArgsBasic.VALUE_NOT_SET )
            //        fMoreArgs = false;
            //    else
            //        Console.WriteLine (
            //            ARG_DISP_TPL_POSITIONAL ,
            //            intArgOrdinal ,
            //            strThisArg );
            //}   // while ( fMoreArgs )

            //if ( intArgOrdinal == StandardConstants.PLUS_ONE )
            //{   // No positional arguments found.
            //    Console.WriteLine ( MSG_NO_POSITIONAL_ARGS );
            //}   // if ( intArgOrdinal == StandardConstants.PLUS_ONE )

            //  ----------------------------------------------------------------
            //  This is the new way, which leverages the new properties.
            //  ----------------------------------------------------------------

            int intNPositionalArgsFound = pcmdLineArgs.PositionalArgsInCmdLine;

            if ( intNPositionalArgsFound == StandardConstants.ZERO )
            {   // Report the absence of command line arguments.
                Console.WriteLine ( MSG_NO_POSITIONAL_ARGS );
            }   // TRUE block (the degenerate case), if ( intNPositionalArgsFound == StandardConstants.ZERO )
            else
            {   // List arguments in order by position.
                Console.WriteLine (
                    ARG_DISP_TPL_COUNT ,
                    intNPositionalArgsFound ,
                    Environment.NewLine );

                for ( intArgOrdinal = StandardConstants.PLUS_ONE ;
                      intArgOrdinal <= intNPositionalArgsFound ;
                      intArgOrdinal++
                    )
                {
                    Console.WriteLine (
                        ARG_DISP_TPL_POSITIONAL ,
                        intArgOrdinal ,
                        pcmdLineArgs.GetArgByPosition ( intArgOrdinal ) );
                }   // for ( intArgOrdinal = StandardConstants.PLUS_ONE ; ...

                Console.WriteLine (
                    ARG_DISP_TPL_END ,
                    Environment.NewLine );
            }   // FALSE block (the normal case), if ( intNPositionalArgsFound == StandardConstants.ZERO )
        }   // private static void method EnumeratePositionalArgs


        private static void EnumerateDefinedNamedArguments (
            CmdLneArgsBasic pcmdLineArgs ,
            string [ ] pastrValidArgumentNames )
        {
            const string ARG_DISP_TPL_NAMED_ARG = @"     Argument {0} = {1} - First character = {2}";
            const string MSG_NO_NAMED_ARGS_DEFINED = @"     No named arguments are defined. Input {0} is {1}.";
            const string ERRMSG_CALLER_NAME = @"EnumerateDefinedNamedArguments";

            if ( pcmdLineArgs == null )
            {
                Console.WriteLine (
                    MSG_NO_NAMED_ARGS_DEFINED ,
                    pcmdLineArgs ,
                    LIST_IS_NULL );
            }   // TRUE block, if ( pcmdLineArgs == null )
            else if ( pcmdLineArgs.DefinedNamedArgs == StandardConstants.ZERO )
            {
                Console.WriteLine (
                    MSG_NO_NAMED_ARGS_DEFINED ,
                    pcmdLineArgs ,
                    LIST_IS_EMPTY );
            }   // TRUE block, else if ( pcmdLineArgs.DefinedNamedArgs == StandardConstants.ZERO )
            else
            {
                foreach ( string strArgName in pastrValidArgumentNames )
                {
                    try
                    {
                        Console.WriteLine (
                            ARG_DISP_TPL_NAMED_ARG ,
                            strArgName ,
                            pcmdLineArgs.GetArgByName ( strArgName ) ,
                            FormatSpecialCharacters ( pcmdLineArgs.GetArgByNameAsChar ( strArgName ) ) );
                    }
                    catch ( Exception exAny )
                    {
                        if ( exAny.Message.StartsWith ( StandardConstants.ERRMSG_ARG_IS_NULL_OR_EMPTY ) && exAny.TargetSite.Name == "FirstCharFromString" )
                        {
                            Console.WriteLine (
                                ARG_DISP_TPL_NAMED_ARG ,
                                strArgName ,
                                pcmdLineArgs.GetArgByName ( strArgName ) ,
                                FormatSpecialCharacters (
                                    pcmdLineArgs.GetArgByNameAsChar (
                                        strArgName ,
                                        StandardConstants.NULL_CHAR ) ) );
                        }   // TRUE block, if ( exAny.Message.StartsWith(StandardConstants.ERRMSG_ARG_IS_NULL_OR_EMPTY) && exAny.TargetSite.Name == "FirstCharFromString" )
                        else
                        {
                            StringBuilder sbMsg = new StringBuilder ( StandardConstants.SB_CAPACITY_08KB );

                            sbMsg.AppendFormat (
                                ERRMSG_INTERNAL_ERROR_TPL ,
                                exAny.Message ,
                                Environment.NewLine );

                            sbMsg.AppendFormat (
                                ERRMSG_INTERNAL_SOURCE_TPL ,
                                exAny.Source ,
                                Environment.NewLine );
                            sbMsg.AppendFormat (
                                ERRMSG_INTERNAL_TARGETSITE_TPL ,
                                exAny.TargetSite ,
                                Environment.NewLine );

                            // ---------------------------------------------------------
                            // 2012/07/07 - DAG/WW - Added environment.NewLine to the
                            //                       argument list. This same change
                            //                       should be applied to the routine in
                            //                       TextLog.cs from which this code is
                            //                       derived.
                            // ---------------------------------------------------------

                            sbMsg.AppendFormat (
                                ERRMSG_INTERNAL_STACKTRACE_TPL ,
                                exAny.StackTrace ,
                                Environment.NewLine );

                            if ( exAny.InnerException != null )
                                sbMsg.AppendFormat (
                                    ERRMSG_INTERNAL_INNER_MSG_TPL ,
                                    exAny.InnerException.Message );

                            string strMsg = sbMsg.ToString ( );

                            Console.WriteLine ( strMsg );
                            WinEventLogging.LogAppEvent (
                                ERRMSG_CALLER_NAME ,
                                strMsg );
                        }   // FALSE block, if ( exAny.Message.StartsWith(StandardConstants.ERRMSG_ARG_IS_NULL_OR_EMPTY) && exAny.TargetSite.Name == "FirstCharFromString" )
                    }   // catch block
                }   // foreach ( string strArgName in pastrValidArgumentNames )
            }   // FALSE block, else if ( pcmdLineArgs.DefinedNamedArgs == StandardConstants.ZERO )
        }   // private static void EnumerateDefinedNamedArguments


        /// <summary>
        /// Return a printable representation of a nonprintable character.
        /// </summary>
        /// <param name="pchr">
        /// Character to be formatted.
        /// </param>
        /// <returns>
        /// String representation of special character, or string representation
        /// of pchr, if it isn't special.
        /// </returns>
        /// <remarks>
        /// As written, this routine handles only a handful of characters (NULL,
        /// TAB, CR, and LF). A complete version that handles more nonprintable
        /// characters would make a useful addition to a library, unless, of 
        /// course, I can find a suitable routine online.
        /// </remarks>
        private static string FormatSpecialCharacters ( char pchr )
        {
            const char CARRIAGE_RETURN = '\r';
            const char LINE_FEED = '\n';

            switch ( pchr )
            {
                case StandardConstants.NULL_CHAR:
                    return @"[NULL]";
                case StandardConstants.TAB_CHAR:
                    return @"[TAB]";
                case CARRIAGE_RETURN:
                    return @"[CR]";
                case LINE_FEED:
                    return @"[LF]";
                default :
                    return pchr.ToString ( );
            }   // switch ( pchr )
        }   // private static string FormatSpecialCharacters


        private static void EmumerateDefinedSwitches (
            CmdLneArgsBasic pcmdLineArgs ,
            char [ ] pachrSwitchSetAllDefined )
        {
            const string MSG_DISP_BLANK_AS_DEFAULT = @"   BlankAsDefault Property = {0}{1}";
            const string ARG_DISP_TPL_SWITCH   = @"     Switch {0} = {1}";
            const string MSG_NO_SWITCHES_DEFINED = @"     No switches are defined. Input {0} is {1}.";
            const string MSG_SWITCH_OMITTED = @"** OMITTED **";

            if ( pachrSwitchSetAllDefined == null )
            {
                Console.WriteLine (
                    MSG_NO_SWITCHES_DEFINED ,
                    pcmdLineArgs ,
                    LIST_IS_NULL );
            }   // TRUE block, if ( pachrSwitchSetAllDefined == null )
            else
            {
                if ( pachrSwitchSetAllDefined.Length == StandardConstants.ZERO )
                {
                    Console.WriteLine (
                        MSG_NO_SWITCHES_DEFINED ,
                        pcmdLineArgs ,
                        LIST_IS_EMPTY );
                }   // TRUE block, if ( pachrSwitchSetAllDefined.Length == StandardConstants.ZERO )
                else
                {
                    foreach ( bool fBlankAsDefault in _afBlankAsDefault )
                    {
                        pcmdLineArgs.AllowEmptyStringAsDefault = fBlankAsDefault;
                        Console.WriteLine (
                            MSG_DISP_BLANK_AS_DEFAULT ,
                            pcmdLineArgs.AllowEmptyStringAsDefault ,
                            Environment.NewLine );

                        foreach ( char chrSwitch in pachrSwitchSetAllDefined )
                        {
                            Console.WriteLine (
                                ARG_DISP_TPL_SWITCH ,
                                chrSwitch ,
                                pcmdLineArgs.GetSwitchByName ( chrSwitch ) );
                            Console.WriteLine (
                                ARG_DISP_TPL_SWITCH ,
                                chrSwitch ,
                                pcmdLineArgs.GetSwitchByName (
                                    chrSwitch ,
                                    MSG_SWITCH_OMITTED ) );
                            Console.WriteLine (
                                ARG_DISP_TPL_SWITCH ,
                                chrSwitch ,
                                pcmdLineArgs.GetSwitchByName (
                                    chrSwitch ,
                                    StandardConstants.EMPTY_STRING ,
                                    CmdLneArgsBasic.BLANK_AS_DEFAULT_FORBIDDEN ) );
                            Console.WriteLine (
                                ARG_DISP_TPL_SWITCH ,
                                chrSwitch ,
                                pcmdLineArgs.GetSwitchByName (
                                    chrSwitch ,
                                    StandardConstants.EMPTY_STRING ,
                                    CmdLneArgsBasic.BLANK_AS_DEFAULT_ALLOWED ) );
                            Console.WriteLine (
                                ARG_DISP_TPL_SWITCH ,
                                chrSwitch ,
                                pcmdLineArgs.GetBooleanSwitchByName ( chrSwitch ) );
                        }   // foreach ( char chrSwitch in pachrSwitchSetAllDefined )
                    }   // foreach ( bool fBlankAsDefault in _afBlankAsDefault )
                }   // FALSE block, if ( pachrSwitchSetAllDefined.Length == StandardConstants.ZERO )
            }   // FALSE block, if ( pachrSwitchSetAllDefined == null )
        }   // private static void EmumerateDefinedSwitches method


        private static void ExerciseFormattedLocalTime ( DateTime pdtmTestTime )
        {
            //  ----------------------------------------------------------------
            //  Use the same object, iterating over all valid values of each of
            //  its properties.
            //  ----------------------------------------------------------------

            string strComment = StandardConstants.EMPTY_STRING;
            int intTestCase = StandardConstants.ZERO;
            TimeDisplayFormatter flt = new TimeDisplayFormatter ( );

            foreach ( TimeDisplayFormatter.DateFieldOrder enmDateFieldOrder in s_aenmDateFieldOrder )
            {
                flt.CalendarDateFormat = enmDateFieldOrder;

                foreach ( TimeDisplayFormatter.HoursFormatType enmHoursFormatType in s_aenmHoursFormatType )
                {
                    flt.HoursFormat = enmHoursFormatType;
                    strComment = s_astrComment [ ( int ) enmHoursFormatType ];

                    foreach ( TimeDisplayFormatter.TimePrecisionType enmTimePrecisionType in s_aenmTimePrecisionType )
                    {
                        flt.TimePrecision = enmTimePrecisionType;

                        foreach ( bool fShowAmPmAsUC in s_afShowAmPmAsUC )
                        {
                            flt.ShowAmPmAsUC = fShowAmPmAsUC;

                            foreach ( bool fShowTimeZone in s_afShowTimeZone )
                            {
                                flt.ShowTimeZone = fShowTimeZone;

                                foreach ( bool fShowWeekdayMane in s_afShowWeekdayName )
                                {
                                    flt.ShowWeekday = fShowWeekdayMane;

                                    //  --------------------------------------------
                                    //  At this point, every property has been set.
                                    //  Working from arrays of valid values, these
                                    //  nested loops put the formatters through
                                    //  their paces.
                                    //  --------------------------------------------

                                    ++intTestCase;

                                    Console.WriteLine ( @"Test date dtmTestTime        = {0}" , pdtmTestTime );

                                    Console.WriteLine ( @"Property CalendarDateFormat  = {0}" , flt.CalendarDateFormat );
                                    Console.WriteLine ( @"Property HoursFormat         = {0}" , flt.HoursFormat );
                                    Console.WriteLine ( @"Property ShowAmPmAsUC        = {0}" , flt.ShowAmPmAsUC );
                                    Console.WriteLine ( @"Property ShowTimeZone        = {0}" , flt.ShowTimeZone );
                                    Console.WriteLine ( @"Property ShowWeekday         = {0}" , flt.ShowWeekday );
                                    Console.WriteLine ( @"Property TimePrecision       = {0}" , flt.TimePrecision );

                                    Console.WriteLine ( @"Comment about property usage = {0}," , strComment );

                                    Console.WriteLine ( @"Formatted Date/Time String   = {0}{1}" , flt.FormatThisTime ( pdtmTestTime ) , Environment.NewLine );
                                    RecordFormattedLocalTimes (
                                        FORMATTED_DATE_REPORT_FQFN ,
                                        pdtmTestTime ,
                                        flt ,
                                        strComment );
                                }   // foreach (bool fShowWeekdayMane = _afShowWeekdayName)
                            }   // foreach ( bool fShowTimeZone in _afShowTimeZone )
                        }   // foreach ( bool fShowAmPmAsUC in _afShowAmPmAsUC )
                    }   // foreach ( FormattedLocalTime.TimePrecisionType enmTimePrecisionType in _aenmTimePrecisionType )
                }   // foreach ( FormattedLocalTime.HoursFormatType enmHoursFormatType in _aenmHoursFormatType )
            }   // foreach ( FormattedLocalTime.DateFieldOrder enmDateFieldOrder in _aenmDateFieldOrder )
        }   //  private static void ExerciseFormattedLocalTime method


        private static void ExerciseGetExeSubsystem ( )
        {
            //  ----------------------------------------------------------------
            //  IMPORTANT: This routine imports from System.ComponentModel and 
            //  System.Runtime.InteropServices.
            //  ----------------------------------------------------------------

            Console.WriteLine ( 
                "{0}ExerciseGetExeSubsystem Begin{0}" ,
                Environment.NewLine );

            int intCaseNumber = 0;

            foreach ( string strFQFN in s_astrPortableExeCollection )
            {
                uint uintPESubsystem = IP6CUtilLib1.GetExeSubsystem ( strFQFN );

                if ( uintPESubsystem > 0 )
                {
                    string strSubsystemTypeDesc = IP6CUtilLib1.XlateProcessSubsystem ( uintPESubsystem );
                    Console.WriteLine (
                        "    Case {0} of {1}: File = {2}{5}                 Subystem = {3} ({4}){5}" ,
                        new object [ ]
                    {
                        ++intCaseNumber ,                       // Token 0
                        s_astrPortableExeCollection.Length ,    // Token 1
                        strFQFN ,                               // Token 2
                        uintPESubsystem ,                       // Token 3
                        strSubsystemTypeDesc ,                  // Token 4
                        Environment.NewLine                     // Token 5
                    } );
                }   // TRUE (expected outcome) block, if ( uintPESubsystem > 0 )
                else
                {
                    int dwLastError = Marshal.GetLastWin32Error ( );
                    Win32Exception w32Exception = new Win32Exception ( dwLastError );
                    Console.WriteLine ( "    While attempting to identify the subsystem of {0}, GetExeSubsystem returned exit code {1}: {2}" ,
                        strFQFN ,
                        dwLastError ,
                        w32Exception.Message );
                }   // FALSE (UNexpected outcome) block, if ( uintPESubsystem > 0 )
            }   // foreach ( string strFQFN in s_astrPortableExeCollection )

            Console.WriteLine (
                "{0}ExerciseGetExeSubsystem Done{0}" ,
                Environment.NewLine );
        }   // private static void ExerciseGetExeSubsystem


        /// <summary>
        /// In addition to being a thorough, self contained exerciser for the
        /// IniFileReader class, this could become a handy general purpose INI
        /// file documenter.
        /// </summary>
        /// <param name="pstrFQFN">
        /// This string must be the fully qualified name of the INI file to
        /// enumerate.
        /// </param>
        private static void ExerciseIniReader ( string pstrFQFN )
        {
            Console.WriteLine (
                "Test INI File = {0}" ,
                pstrFQFN );

            IniFileReader pvtProfile = new IniFileReader ( pstrFQFN );

            string [ ] astrSections = pvtProfile.GetSectionNames ( );
            Console.WriteLine (
                "\nSections in INI file = {0}" ,
                astrSections.Length );

            foreach ( string strSection in astrSections )
            {
                string [ ] astrKeyNames = pvtProfile.GetEntryNames ( strSection );
                Console.WriteLine (
                    "\n    Section {0}: # of keys = {1}\n" ,
                    strSection ,
                    astrKeyNames.Length );

                foreach ( string strKeyName in astrKeyNames )
                {
                    Console.WriteLine (
                        "        Key {0} = {1}" ,
                        strKeyName ,
                        pvtProfile.GetEntryValue (
                            strSection ,
                            strKeyName ) );
              }   // foreach ( string strKeyName in astrKeyNames )
            }   // foreach ( string strSection in astrSections )
        }   // private static void ExerciseIniReader


        private static void ExerciseSysDateFormatters (
            DateTime pdtmTestTime ,
            DateFormatter penmDateFormatter )
        {
            const string MSG_TEST_BEGIN = "\r\nBegin testing of static method {0}:\r\n";
            const string MSG_TEST_DONE = "\r\nFinished testing ReformatSysDate:\r\n";
            const string MSG_TPL = @"    Format string = {0} - Formatted Date = {1}";
            const string PLAIN_DATE = @"^YYYY/^MM/^DD";

            Console.WriteLine (
                MSG_TEST_BEGIN ,
                penmDateFormatter );

            string strFormattedDate = FormatThisDate (
                pdtmTestTime ,
                PLAIN_DATE ,
                penmDateFormatter );
            Console.WriteLine (
                MSG_TPL ,
                PLAIN_DATE ,
                strFormattedDate );

            string [ ] astrFormatStrings = new string [ ]
            {
                SysDateFormatters.RFD_DD_MM_YYYY ,

                SysDateFormatters.RFD_HH_MM ,
                SysDateFormatters.RFD_HH_MM_SS ,
                SysDateFormatters.RFD_HH_MM_SS_TTT ,

                SysDateFormatters.RFD_MM_DD_YYYY , 
                SysDateFormatters.RFD_YYYY_MM_DD ,

                SysDateFormatters.RFD_WWW_DD_MM_YYYY ,
                SysDateFormatters.RFD_WWW_MM_DD_YYYY ,

                SysDateFormatters.RFD_WW_DD_MM_YYYY ,
                SysDateFormatters.RFD_WW_MM_DD_YYYY ,

                SysDateFormatters.RFD_WWWW_DD_MM_YYYY ,
                SysDateFormatters.RFD_WWWW_MM_DD_YYYY ,

                SysDateFormatters.RFDYYYYMMDD_HHMMSS ,
                SysDateFormatters.RFDYYYYMMDD ,
                SysDateFormatters.RFDHHMMSS
            };  // string [ ] astrFormatStrings

            foreach ( string strCurrFormat in astrFormatStrings )
            {
                Console.WriteLine (
                    MSG_TPL ,
                    strCurrFormat ,
                    FormatThisDate (
                        pdtmTestTime ,
                        strCurrFormat ,
                        penmDateFormatter ) );
            }   // foreach ( string strCurrFormat in astrFormatStrings )

            Console.WriteLine ( MSG_TEST_DONE );
        }   // private static void ExerciseSysDateFormatters


        private static void ExerciseTheStateManager ( StateManager pappStateManager )
        {
            Console.WriteLine ( 
                "ExerciseTheStateManager Begin{0}" , 
                Environment.NewLine );

            Console.WriteLine ( "    AppRootAssemblyFileDirName  = {0}" , pappStateManager.AppRootAssemblyFileDirName );
            Console.WriteLine ( "    AppRootAssemblyFileName     = {0}" , pappStateManager.AppRootAssemblyFileName );
            Console.WriteLine ( "    AppStartupTimeLocal         = {0}" , pappStateManager.AppStartupTimeLocal );
            Console.WriteLine ( "    AppStartupTimeUtc           = {0}" , pappStateManager.AppStartupTimeUtc );
            Console.WriteLine ( "    AppSubsystemID              = {0} ({1})" , pappStateManager.AppSubsystemID , pappStateManager.GetAppSubsystemString ( ) );
            Console.WriteLine ( "    AppSubsystemID              = {0}" , pappStateManager.GetAssemblyFQFN ( ) );

            Console.WriteLine ( "    InitialWorkingDirectoryName = {0}" , pappStateManager.InitialWorkingDirectoryName );

            Console.WriteLine (
                "{0}ExerciseTheStateManager Done{0}" ,
                Environment.NewLine );
        }   // private static void ExerciseTheStateManager


        private static string FormatThisDate (
            DateTime pdtmThisDate ,
            string pstrThisFormat ,
            DateFormatter penmThisFormatter )
        {
            switch ( penmThisFormatter )
            {
                case DateFormatter.ReformatSysDate:
                    return WizardWrx.ApplicationHelpers3.SysDateFormatters.ReformatSysDate ( pdtmThisDate , pstrThisFormat );
                case DateFormatter.ReformatNow:
                    return WizardWrx.ApplicationHelpers3.SysDateFormatters.ReformatNow ( pstrThisFormat );
                case DateFormatter.ReformatUtcNow:
                    return WizardWrx.ApplicationHelpers3.SysDateFormatters.ReformatUtcNow ( pstrThisFormat );
                default:
                    throw new ArgumentOutOfRangeException ( string.Format ( @"Error in method FormatADate: DateFormatter argument value of {0} is invalid." , penmThisFormatter ) );
            }   // switch ( penmThisFormatter )
        }   // private static string FormatThisDate


        private static void RecordFormattedLocalTimes (
            string pstrFQFN ,
            DateTime pdtmTestTime ,
            TimeDisplayFormatter pflt ,
            string pstrComment )
        {
            const string ERRMSG_CALLER_NAME = @"CreateMsg";
            const string MESSAGE_TEMPLATE = @"{0} {1}";
            const string MESSAGE_TEMPLATE_LAST_CHANCE = @"An attempt to gain exclusive use of the log file timed out, most likely due to extremely heavy contention.{1}To preserve the message, it is being recorded in the Application Event Log.{1}The message text follows.{1}{1}{0}";
            const string REPORT_TEMPLATE = "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}";

            object [ ] objFields =
            {
                pdtmTestTime,
                pflt.CalendarDateFormat,
                pflt.HoursFormat,
                pflt.ShowAmPmAsUC,
                pflt.ShowTimeZone,
                pflt.ShowWeekday,
                pflt.TimePrecision,
                pstrComment,
                pflt.FormatThisTime ( pdtmTestTime )
            };

            bool fMutexAcquired = false;
            bool fWriteHeadings = false;

            FileStream stmLog = null;
            StreamWriter swLog = null;

            string strCompleteHeading = string.Empty;
            string strMsgDetails = string.Format (
                REPORT_TEMPLATE ,
                objFields );

            DateTime dtmLogDate = DateTime.Now;     // We'll eventually do more.
            string strCompleteMessage = string.Format (
                MESSAGE_TEMPLATE ,
                dtmLogDate ,
                strMsgDetails );

            if ( File.Exists ( pstrFQFN ) )
            {   // If the file exists, so does the heading row.
                fWriteHeadings = false;
            }
            else
            {   // File will be created, with its one and only heading row.
                strCompleteHeading = string.Format (
                    REPORT_TEMPLATE ,
                    s_astrFormattedDateLabels );
                fWriteHeadings = true;
            }

            try
            {
                fMutexAcquired = s_mutGlobalMutex.WaitOne ( ( int ) WizardWrx.DateMath.TimeStampFactory.TSF_ONE_SECOND );
            }
            catch ( ObjectDisposedException errDisposed )
            {
                StringBuilder sbMsg = new StringBuilder ( StandardConstants.SB_CAPACITY_08KB );

                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_ERROR_TPL ,
                    errDisposed.Message ,
                    Environment.NewLine );

                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_OBJNAME_TPL ,
                    errDisposed.ObjectName );

                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_SOURCE_TPL ,
                    errDisposed.Source ,
                    Environment.NewLine );
                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_TARGETSITE_TPL ,
                    errDisposed.TargetSite ,
                    Environment.NewLine );

                // -------------------------------------------------------------
                // 2012/07/07 - DAG/WW - Added environment.NewLine to the
                //                       argument list. This same change should
                //                       be applied to the routine in TextLog.cs
                //                       from which this code is derived.
                // -------------------------------------------------------------

                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_STACKTRACE_TPL ,
                    errDisposed.StackTrace ,
                    Environment.NewLine );

                WinEventLogging.LogAppEvent (
                    ERRMSG_CALLER_NAME ,
                    sbMsg.ToString ( ) );
            }   // catch block for ObjectDisposedException exception
            catch ( ArgumentOutOfRangeException errBadArg )
            {
                StringBuilder sbMsg = new StringBuilder ( StandardConstants.SB_CAPACITY_08KB );

                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_ERROR_TPL ,
                    errBadArg.Message ,
                    Environment.NewLine );

                sbMsg.AppendFormat (
                    ERRMSG_BADARG_PARAMINFO_TPL ,
                    errBadArg.ParamName , errBadArg.ActualValue );

                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_SOURCE_TPL ,
                    errBadArg.Source ,
                    Environment.NewLine );
                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_TARGETSITE_TPL ,
                    errBadArg.TargetSite ,
                    Environment.NewLine );

                // -------------------------------------------------------------
                // 2012/07/07 - DAG/WW - Added environment.NewLine to the
                //                       argument list. This same change should
                //                       be applied to the routine in TextLog.cs
                //                       from which this code is derived.
                // -------------------------------------------------------------

                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_STACKTRACE_TPL ,
                    errBadArg.StackTrace ,
                    Environment.NewLine );

                WinEventLogging.LogAppEvent (
                    ERRMSG_CALLER_NAME ,
                    sbMsg.ToString ( ) );
            }   // catch block for ArgumentOutOfRangeException exception
            catch ( AbandonedMutexException errAbandonedMutex )
            {
                StringBuilder sbMsg = new StringBuilder ( StandardConstants.SB_CAPACITY_08KB );

                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_ERROR_TPL ,
                    errAbandonedMutex.Message ,
                    Environment.NewLine );

                sbMsg.AppendFormat (
                    ERRMSG_ABANDONED_MUTEX_HANDLE_TPL ,
                    errAbandonedMutex.Mutex.SafeWaitHandle );

                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_SOURCE_TPL ,
                    errAbandonedMutex.Source ,
                    Environment.NewLine );
                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_TARGETSITE_TPL ,
                    errAbandonedMutex.TargetSite ,
                    Environment.NewLine );

                // -------------------------------------------------------------
                // 2012/07/07 - DAG/WW - Added environment.NewLine to the
                //                       argument list. This same change should
                //                       be applied to the routine in TextLog.cs
                //                       from which this code is derived.
                // -------------------------------------------------------------

                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_STACKTRACE_TPL ,
                    errAbandonedMutex.StackTrace ,
                    Environment.NewLine );

                WinEventLogging.LogAppEvent (
                    ERRMSG_CALLER_NAME ,
                    sbMsg.ToString ( ) );
            }   // catch block for AbandonedMutexException exception
            catch ( Exception errMisc )
            {
                StringBuilder sbMsg = new StringBuilder ( StandardConstants.SB_CAPACITY_08KB );

                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_ERROR_TPL ,
                    errMisc.Message ,
                    Environment.NewLine );

                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_SOURCE_TPL ,
                    errMisc.Source ,
                    Environment.NewLine );
                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_TARGETSITE_TPL ,
                    errMisc.TargetSite ,
                    Environment.NewLine );

                // -------------------------------------------------------------
                // 2012/07/07 - DAG/WW - Added environment.NewLine to the
                //                       argument list. This same change should
                //                       be applied to the routine in TextLog.cs
                //                       from which this code is derived.
                // -------------------------------------------------------------

                sbMsg.AppendFormat (
                    ERRMSG_INTERNAL_STACKTRACE_TPL ,
                    errMisc.StackTrace ,
                    Environment.NewLine );

                if ( errMisc.InnerException != null )
                    sbMsg.AppendFormat (
                        ERRMSG_INTERNAL_INNER_MSG_TPL ,
                        errMisc.InnerException.Message );

                WinEventLogging.LogAppEvent (
                    ERRMSG_CALLER_NAME ,
                    sbMsg.ToString ( ) );
            }   // catch-all block for Exception (catch-all) exception
            finally
            {   // If the mutex was acquired, even by abandonment, the abandonment has been recorded, and it is safe to continue.
                if ( fMutexAcquired )
                {
                    try
                    {
                        using ( stmLog = new FileStream (
                            pstrFQFN ,
                            FileMode.Append ,
                            FileAccess.Write ,
                            FileShare.Read ,
                            StandardConstants.SB_CAPACITY_08KB ,
                            true ) )
                        {
                            //  --------------------------------------------------------
                            //  This is the original streamwriter, which is neither
                            //  thread-safe, nor asynchronous.
                            //  --------------------------------------------------------

                            //swLog = new StreamWriter(
                            //    pstrFQFN,
                            //    StandardConstants.FILE_OUT_APPEND,
                            //    Encoding.ASCII);

                            using ( swLog = new StreamWriter (
                                stmLog ,
                                Encoding.ASCII ) )
                            {   // The cleanup code for a using block calls dispose.
                                if ( fWriteHeadings )
                                {
                                    StreamWriter.Synchronized ( swLog ).WriteLine ( strCompleteHeading );
                                }   // if ( fWriteHeadings )

                                StreamWriter.Synchronized ( swLog ).WriteLine ( strCompleteMessage );
                                StreamWriter.Synchronized ( swLog ).Close ( );
                            }   // using (swLog = new StreamWriter block
                        }   // using (stmLog = new FileStream block

                        //  ------------------------------------------------------------
                        //  Signal the CLR that both using blocks completed normally.
                        //  ------------------------------------------------------------

                        swLog = null;
                        stmLog = null;
                    }   // Try block
                    catch ( PathTooLongException errPath )
                    {
                        throw new Exception ( string.Format (
                            ERRMSG_GENERIC_TPL ,
                            new string [ ] {
                        pstrFQFN ,
                        errPath.Message,
                        errPath.Source,
                        errPath.TargetSite.ToString(),
                        errPath.StackTrace,
                        Environment.NewLine} ) );
                    }
                    catch ( DirectoryNotFoundException errDirNotFnd )
                    {
                        throw new Exception ( string.Format (
                            ERRMSG_GENERIC_TPL ,
                            new string [ ] {
                        pstrFQFN ,
                        errDirNotFnd.Message,
                        errDirNotFnd.Source,
                        errDirNotFnd.TargetSite.ToString(),
                        errDirNotFnd.StackTrace,
                        Environment.NewLine} ) );
                    }
                    catch ( Exception err )
                    {
                        throw new Exception ( string.Format (
                            ERRMSG_GENERIC_TPL ,
                            new string [ ] {
                        pstrFQFN ,
                        err.Message,
                        err.Source,
                        err.TargetSite.ToString(),
                        err.StackTrace,
                        Environment.NewLine} ) );
                    }   // Catch block.
                    finally
                    {   // If anything is open, close it, and discard its resources.
                        if ( swLog != null )
                        {
                            swLog.Close ( );
                            swLog.Dispose ( );
                        }   // if (swLog != null)

                        if ( stmLog != null )
                        {
                            stmLog.Close ( );
                            stmLog.Dispose ( );
                        }   // if (stmLog != null)

                        s_mutGlobalMutex.ReleaseMutex ( );         // Relinquish ownership.
                    }   // Try/Catch/Finally block
                }   // TRUE block, if (fMutexAcquired)
                else
                {   // As a last resort, record the message in the Application Event Log.

                    // -------------------------------------------------------------
                    // 2012/07/07 - DAG/WW - Added environment.NewLine to the
                    //                       argument list. This same change should
                    //                       be applied to the routine in TextLog.cs
                    //                       from which this code is derived.
                    // -------------------------------------------------------------

                    string strLastChanceMessage = string.Format (
                        MESSAGE_TEMPLATE_LAST_CHANCE ,
                        strCompleteMessage ,
                        Environment.NewLine );
                    WinEventLogging.LogAppEvent (
                        ERRMSG_CALLER_NAME ,
                        strLastChanceMessage );
                }   // FALSE block, if (fMutexAcquired)
            }   // outer Try/Catch/Finally block, surrounding mutex acquisition.
        }   // private static void RecordFormattedLocalTimes method


        private static void ShowKeAssemblyProperties ( My pmyLib )
        {
            Console.WriteLine ( @"Selected properties of the library under test:" );

            Console.WriteLine ( @"    AssemblyFileBaseName = {0}" , pmyLib.AssemblyFileBaseName );
            Console.WriteLine ( @"    VersionString        = {0}" , pmyLib.VersionString );
            Console.WriteLine ( @"    Culture              = {0}" , pmyLib.Culture );
            Console.WriteLine ( @"    PublicKeyToken       = {0}{1}" , pmyLib.PublicKeyToken , Environment.NewLine );

            Console.WriteLine ( @"    AssemblyGuidString   = {0}{1}" , pmyLib.AssemblyGuidString , Environment.NewLine );

            Console.WriteLine ( @"    ProductName          = {0}" , pmyLib.MiscVersionInfo.ProductName );
            Console.WriteLine ( @"    LegalCopyright       = {0}" , pmyLib.LegalCopyright );
            Console.WriteLine ( @"    LegalTrademarks      = {0}" , pmyLib.LegalTrademarks );
            Console.WriteLine ( @"    CompanyName          = {0}{1}" , pmyLib.CompanyName , Environment.NewLine );

            Console.WriteLine ( @"    Description          = {0}" , pmyLib.Description );
            Console.WriteLine ( @"    Comments             = {0}" , pmyLib.Comments );

            Console.WriteLine ( @"    AssembyDirName       = {0}{1}" , pmyLib.AssembyDirName , Environment.NewLine );
            Console.WriteLine ( @"    AssemblyFileName     = {0}{1}" , pmyLib.AssemblyFileName , Environment.NewLine );

            string strAssemblyFileFQFN = WizardWrx.FileNameTricks.MakeFQFN ( pmyLib.AssemblyFileName , pmyLib.AssembyDirName );
            FileInfo fiLibraryFile = new FileInfo ( strAssemblyFileFQFN );

            Console.WriteLine ( @"    File CreatION Date   = {0} ({1} UTC)" , fiLibraryFile.CreationTime , fiLibraryFile.CreationTimeUtc );
            Console.WriteLine ( @"    File Modified Date   = {0} ({1} UTC)" , fiLibraryFile.LastWriteTime , fiLibraryFile.LastWriteTimeUtc );

            Console.WriteLine ( @"{0}End of selected properties{0}" , Environment.NewLine );
        }   // private static void ShowKeAssemblyProperties method


        private static string WriteExceptionGroupBanner ( ExceptionTypes penmExType )
        {
            const string SUFFIX = @"Test";
            string strExceptionTypeString = penmExType.ToString ( );
            string rstrGroupLabel = strExceptionTypeString.Substring (
                StandardConstants.STRING_SUBSTR_BEGINNING ,
                strExceptionTypeString.Length - SUFFIX.Length );
            Console.WriteLine (
                Properties.Resources.ExceptionLoggerTestGroup ,
                rstrGroupLabel ,
                Environment.NewLine );
            return rstrGroupLabel;
        }   // private static void WriteExceptionGroupBanner
        #endregion  // Helper Routines
    }   // private class Program
}   // partial namespace ApplicationHelpers3_TestStand