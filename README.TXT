                              DLLServices2 ReadMe

================================================================================
2018-09-04 01:54:00 - NOTICE: Effective immediately, this repository is retired.
                              Please use its successor,
                              https://github.com/txwizard/WizardWrx_NET_API,
                              which consolidates numerous libraries int one
                              coherent constellation of class libraries.
================================================================================

================================================================================
2016-06-07 12:07:00
================================================================================

The purpose of this class library is to expedite development of new applications
that target any version of the Microsoft .NET Framework, from version 2.0 up.
The classes in this library define numerous groups of constants, most assigned
to the base WizardWrx namespace, and utility classes, organized into subsidiary
namespace DLLServices2. The following table briefly summarizes the classes.

Class Name           Namespace (Full Name)  Abstract
-------------------- ---------------------- -----------------------------------
ArrayInfo            WizardWrx              This class organizes constants and
                                            routines for working with arrays.
                                            The constants are mostly synonyms
                                            for constants defined in other
                                            classes.

AssemblyLocatorBase  WizardWrx.DLLServices2 Given the location from which an
                                            assembly was loaded, you can learn
                                            almost anything else you need to
                                            know about that file, such as its
                                            size, age, and directory. Given the
                                            directory, you can locate satellite
                                            files, such as custom configuration
                                            files.

CmdLneArgsBasic      WizardWrx.DLLServices2 An instance efficiently processes
                                            command line switches, named
                                            arguments, and positional arguments,
                                            in such a way that they are  easily
                                            accessible. The command line is
                                            completely parsed in a single pass.

CSVFileInfo          WizardWrx.DLLServices2 Use these constants and service
                                            routines to simplify working with
                                            CSV type files.

DisplayFormats       WizardWrx.DLLServices2 Pass these constants to the
                                            ToString method on an object of the
                                            appropriate type to render the
                                            object for printing.

                                            The comment associated with each
                                            constant identifies the types for
                                            which it is appropriate.

                                            There are service methods to
                                            facilitate using the Date/Time
                                            formatting constants, which cannot
                                            be fed to ToString, except, perhaps,
                                            with a custom formatting engine. Use
                                            these service methods, or call the
                                            ReformatSysDate function, which also
                                            belongs to this library, directly.

                                            NOTE: The time formatting strings
                                                  and routines in this class are
                                                  time zone agnostic. If you
                                                  want or need the time zone,
                                                  use the companion method,
                                                  GetDisplayTimeZone, defined in
                                                  sibling class Util, when it is
                                                  built against version 3.5 or
                                                  higher of the .NET Framework.

EnhancedIOException  WizardWrx              This class is a thin veneer over its
                                            base class, System.IO.IOException,
                                            which exists solely to expose its
                                            HRESULT property, so that it can be
                                            tested, rather than being hostage to
                                            a message. Breaking free of the
                                            message makes this class locale
                                            agnostic.

                                            This class is a hack, for use with
                                            version 2.0 of the .NET Framework,
                                            which doesn't expose the HRESULT in
                                            base class, System.IO.IOException.
                                            Doing so permits differentiation of
                                            I/O exceptions based on HRESULT, a
                                            numeric value that is unaffected by
                                            the current locale settings.

ErrorMessagesInColor WizardWrx.DLLServices2 System.Console.Error.Write and
                                            System.Console.Error.WriteLine methods
                                            that write in living color.

                                            Static methods take extra arguments
                                            that specify the desired foreground
                                            and background colors, while the
                                            instance methods are drop-in
                                            replacements; substitute the name of
                                            your instance for Console.

ExceptionLogger   WizardWrx.DLLServices2    This Singleton class exposes methods
                                            for formatting data from instances
                                            of the System.Exception class, and
                                            commonly used derived classes, and
                                            displaying the formatted data on a
                                            console (strictly speaking, STDOUT,
                                            which can be redirected to a file),
                                            and recording them in a Windows
                                            event log.

                                            The disposition of the messages you
                                            generate through the ReportException
                                            method is completely up to you; you
                                            can have them delivered to Standard
                                            Output (Console.Out), Standard Error
                                            (Console.Error), both, or neither,
                                            in addition to the Windows event log
                                            of your choice (By default, this is
                                            disabled.) and, finally, to your
                                            program, for display on message
                                            boxes or other uses. To that end,
                                            all overloads return the final
                                            message that would have gone to the
                                            console.

                                            Finally, if you ask for output to
                                            both Console.Out and Console.Error,
                                            these methods are smart enough to
                                            give you just one message, unless
                                            Console.Out is redirected, in which
                                            case, the message goes to both
                                            Console.Out and Console.Error.

                                            Unlike most of the classes defined
                                            in this library, this class may be
                                            inherited.

                                            All methods of this class are
                                            thread-safe.

FileInfoExtension    WizardWrx              Extend the System.IO.FileInfo class
                                            with methods for testing, setting,
                                            and clearing file attribute flags,
                                            including the ability to save and
                                            restore flags to their initial
                                            states.

FileIOFlags          WizardWrx.DLLServices2 Use these flags to document stream
                                            I/O method calls.

FileNameTricks       WizardWrx              This class exposes numerous static
                                            methods for manipulating file names.
                                            Unlike the objects in the
                                            System.IO.File namespace, these
                                            methods don't need a real file
                                            object. All work on strings that
                                            represent file names.

GenericSingletonBase WizardWrx.DLLServices2 Abstract class GenericSingletonBase
                                            is a complete implementation of the
                                            Singleton design pattern that takes
                                            full advantage of the Microsoft .NET
                                            Framework.

                                            Optimizations in this implementation
                                            take advantage of a guarantee made
                                            by the framework that it won't
                                            bother to call a static constructor
                                            on a class until its first use.
                                            Moreover, a static constructor is
                                            never called more than once, no
                                            matter how many subsequent
                                            references to the class occur.

                                            Taking advantage of these
                                            features of the framework
                                            eliminates the need for
                                            synchronization, and replaces a
                                            method call with a direct reference
                                            to the static read only property
                                            that returns a reference to the one
                                            and only instance.

IniFileReader        WizardWrx.DLLServices2 Provide a managed interface to
                                            GetPrivateProfileString in the
                                            Windows API, with methods to
                                            retrieve the values of individual
                                            keys and lists of the keys in a
                                            section or the sections in a file.

IP6CUtilLib1         WizardWrx.DLLServices2 This class exposes static methods
                                            imported from its namesake library
                                            of unmanaged native routines for
                                            getting information not otherwise
                                            easily retrievable by managed code.

ListInfo             WizardWrx.DLLServices2 This static class exposes handy
                                            constants for working with buffers,
                                            lists, and substrings.

Logic                WizardWrx.DLLServices2 This sealed class exposes methods
                                            that encapsulate many common, tricky
                                            loop state tests.

MagicBooleans        WizardWrx              This class defines frequently used
                                            Boolean values whose correct values
                                            are easier to remember with the help
                                            of a mnemonic.

                                            This class is one of a constellation
                                            of static classes that define a wide
                                            variety of symbolic constants that I
                                            use to make my code easier to
                                            understand when I need a refresher or
                                            am about to change it.

MagicNumbers         WizardWrx              This class defines constants for
                                            commonly used magic numbers. Others
                                            are defined in companion class
                                            ArrayInfo, while SpecialCharacters
                                            defines character representations of
                                            the visually ambiguous numbers and
                                            letters, for use in place of
                                            literals.

NumericFormats       WizardWrx              This class defines standard numeric
                                            format strings, for use with the
                                            string.format method and its
                                            derivatives and relatives.

PathPositions        WizardWrx              Use these constants to document path
                                            string parsing.

                                            This class consists entirely of
                                            synonymous constants.

PropertyDefaults     WizardWrx.DLLServices2 Expose the AppSettingsSection
                                            associated with this DLL. The
                                            configuration settings come from the
                                            base class, AssemblyLocatorBase;
                                            hence, this class also serves as the
                                            concrete example that is recommended
                                            to accompany distribution of any
                                            abstract base class.

                                            The settings exposed by this class
                                            inhabit a configuration file that is
                                            associated with the DLL, itself, as
                                            opposed to the application
                                            configuration. Though its format is
                                            similar to that of the application
                                            configuration file, the DLL
                                            configuration lives in its own
                                            configuration file that has the same
                                            name as the DLL, with an additional
                                            suffix of .config.

                                            The rationale behind segregating
                                            these settings is that the affected
                                            application properties are ones that
                                            you would be well served to keep
                                            consistent, or nearly so, across
                                            large groups of applications.
                                            Keeping them in a dedicated
                                            configuration file that travels with
                                            the DLL that implements them
                                            eliminates the thankless task of
                                            adding them to every application
                                            configuration file.

RegExpSupport        WizardWrx              Constants, some built from others by
                                            static methods, to expedite common
                                            tasks that use regular expressions.

RegistryValues       WizardWrx.DLLServices2 This sealed class exposes static
                                            methods for efficiently testing for
                                            the presence of named values in a
                                            Registry key that behave more like
                                            the Item property of a collection,
                                            and retrieving Registry value types
                                            that require a transformation of one
                                            sort or another to get them into the
                                            appropriate native type.

SpecialCharacters    WizardWrx              Use these constants when you want or
                                            need your listings to be crystal
                                            clear about certain ambiguous
                                            literals.

SpecialStrings       WizardWrx              This class defines special purpose
                                            strings that are either difficult to
                                            get right in the first place, or
                                            display ambiguously in text editors
                                            and printed source code listings.

StateManager         WizardWrx.DLLServices2 This class maintains run-time
                                            information about the executing
                                            assembly, assumed to be a desktop
                                            application, that calls it into
                                            being. Since it implements the
                                            Singleton design pattern, there is
                                            never more than one instance.

StringTricks         WizardWrx              This is a class of static functions
                                            for performing common tasks not
                                            provided by the System.String class.

SyncRoot             WizardWrx              Use instances of this class to
                                            provide classes that must be made
                                            thread-safe with locks over which
                                            the class has complete control.

SysDateFormatters    WizardWrx.DLLServices2 This class implements my stalwart
                                            date formatter, ReformatSysDateP6C,
                                            which I created initially as a
                                            Windows Interface Language (WIL,
                                            a. k. a. WinBatch) library function,
                                            Reformat_Date_YmdHms_P6C, in October
                                            2001, though its roots go back much
                                            further in my script development.

TextBlocks           WizardWrx              Methods for creating and
                                            manipulating test blocks.

TimeDisplayFormatter WizardWrx.DLLServices2 Use instances of this class to
                                            return dates and times, uniformly
                                            formatted by rules set by way of its
                                            properties.

Util                 WizardWrx.DLLServices2 This static class exposes utility
                                            constants and methods that run the
                                            gamut from syntactic sugar to
                                            functions that hide useful, but
                                            somewhat obscure capabilities of
                                            the Microsoft Base Class Library,
                                            and don't fit neatly into another
                                            class.

To maximize compatibility with client code, the library targets version 2.0 of
the Microsoft .NET Framework, enabling it to support projects that target that
version, or any later version, of the framework. Since its implementation needs
only core features of the Base Class Library, I have yet to discover an issue in
using it with any of the newer frameworks.

The class belongs to the WizardWrx namespace, which I created to organize the
helper libraries that I use in virtually every production assembly, regardless
of what framework version is its target, and whether its surface is a Windows
console, the Windows desktop, or the ASP.NET Web server. To date, I have used
classes and methods in these libraries in all three environments.

The next several sections cover special considerations of which you must be
aware if you incorporate this package into your code as is or if you want to
modify it.


--------------------------
NuGet Packages
--------------------------

The EnhancedIOException class uses a method exposed by the Newtonsoft.Json class
library. To that end, the repository includes Newtonsoft.Json.8.0.3, and the
build script is configured to pull the latest version from the NuGet repository.

--------------------------
Unmanaged Native Code DLLs
--------------------------

There are a number of native code dynamic link libraries in the output
directories. Only two, P6CUtilLib1.dll and WWConAid.dll, are directly invoked.
Though the routines in those two libraries that are used, via Platform/Invoke,
use a handful of routines from the other libraries, most are there to satisfy
the loader, which expects to find every DLL mentioned in the Import Address
Table, regardless of whether that routine is actually invoked. After all, the
loader has no way to know that you intend to use just one of possibly dozens or
hundreds of exported routines.

All of these DLLs are my own work, and were created using the Microsoft Visual
Studio compilers and tools. The newer libraries were built with Visual Studio
2013, and link to MSVCR120.dll. The rest were built with Visual Studio 6, and
link to MSVCRT.dll. Although I made significant use of the CRT libraries, even
the routines that write to streams, no resources created by one version of the
CRT DLL is ever processed by the other, and I have run these routines in dozens
of applications, many of which run daily, for over two years, without incident.

--------------------------
Required External Tools
--------------------------

The pre and post build tesks and the test scripts found in the /scripts
directory use a number of tools that I have developed over many years. Since
they live in a directory that is on the PATH list on my machine, they are "just
there" when I need them, and I seldom give them a second thought. To simplify
matters for anybody who wants to run the test scripts or build the project, they
are in DAGDevTOOLS.ZIP, which can be extracted into any directory that happens
to be on your PATH list. None of them requires installation, none of the DLLs is
registered for COM, and none of them or their DLLs use the Windows Registry.

A few use MSVCR120.dll, which is not included, but you probably have it if you
have a compatible version of Microsoft Visual Studio. The rest use MSVCRT.DLL,
which ships with Microsoft Windows.

The following table lists and briefly describes the programs and scripts that
are included in the archive.

Program or Script Name      Purpose
--------------------------- ----------------------------------------------------
Date2FN.exe                 Format the local time as YYYYMMDD_HHMMSS, and append
                            it to the base name of a file. This is a simple way
                            to guarantee unique names for sets of log files and
                            test reports that are easy to sort by creation date.

wwBldNbrMgr.exe             Increment the build number in the AssemblyInfo.cs OF
                            a project. Only C# projects are supported.

WWLogger.exe                Create or append to a text long, each entry having a
                            time stamp.

                            The default format is YYYY/MM/DD hh:mm:ss, which can
                            be changed via command line options, discussed with
                            examples in WWLOGGER.pdf, which is also included.
                            The time stamp is always followed by a single space,
                            then the message, which has no pre-set length limit.

                            Additional lines can be appended to the message by
                            writing them into a text file, then piping the text
                            to standard input by way of redirection characters.

WWLOGGERD.EXE               This is a Unicode version of WWLogger.exe; it works
                            identically, except that it outputs 16-bit Unicode
                            characters. Though the files created by the current
                            version lack a Byte Order Mark of any kind, I have
                            never had a problem reading them into a text editor
                            or spreadsheet program.

WWPause.exe                 I got fed up eons ago with batch files that took off
                            when ANY key was pressed, and wrote a DOS version of
                            this program. This is its 32 bit character mode son.

WWSleep.EXE                 This program allows a batch file to be suspended for
                            a user-defined number of seconds, with a default of
                            30 seconds if unspecified, to allow time for you to
                            read its output before the window in which it ran is
                            closed when the script exits.

ExportFromEventLogNDays.CMD This script uses the SysInternals PSLogList utility
                            to export records from a Windows event log. You must
                            supply your own copy of PSLogList.exe.

ShowArgs.BAT                This script displays the command line arguments of a
                            batch file. It can display up to nine arguments, but
                            it displays only actual arguments.

ShowTime.CMD                This script displays the current date and time, so a
                            script listing is time stamped.

XCOPYD.CMD                  This wrapper around stalwart system utility XCOPY is
                            the most dependable way I know to automatically give
                            the correct answer to the prompt about whether the
                            destination is a file or a directory when the answer
                            is that it is a directory.

XcopyDestIsDir.TXT          This response file answers the prompt from XCOPY.EXE
                            when the destination is a directory. XCOPYF.CMD
                            pipes it into XCOPY.EXE to automatically supply the
                            correct answer. This not only eliminate guesswork,
                            and a risk of responding incorrectly when using
                            XCOPY interactively in a command prompt window, but
                            it permits XCOPY to be run safely in unatteended
                            scripts, including build scripts, such as the one in
                            the Visual C++ build projects for my native DLLs.

                            This file and XCOPYD.CMD inhabit the same directory,
                            where XCOPYD.CMD parses its own fully qualified path
                            to work out where to find its response file.

XcopyDestIsFile.TXT         This is the response file for XCOPYF.CMD; it works
                            exactly like XcopyDestIsDir.TXT when the correct
                            response is that the destination is a file.

XCOPYF.CMD                  This wrapper around stalwart system utility XCOPY is
                            the most dependable way I know to automatically give
                            the correct answer to the prompt about whether the
                            destination is a file or a directory when the answer
                            is that it is a file.


--------------------------
Internal Documentation
--------------------------

The source code includes comprehenisve technical documentation, including XML to
generate IntelliSense help, from which the build engine generates XML documents,
which are included herein. Argument names follow Hungarian notation, to make the
type immediately evident in most cases. A lower case "p" precedes a type prefix,
to differentiate arguments from local variables, followed by a lower case "a" to
designate arguments that are arrays. Object variables have an initial underscore
and static variables begin with "s_"; this naming scheme makes variable scope
crystal clear.

The classes are thoroughly cross referenced, and many properties and methods
have working links to relevant MSDN pages.
================================================================================
