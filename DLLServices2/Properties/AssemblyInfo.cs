using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle ( "WizardWrx.DLLServices2" )]
[assembly: AssemblyDescription ( "" )]
[assembly: AssemblyConfiguration ( "" )]
[assembly: AssemblyCompany ( "David A. Gray" )]
[assembly: AssemblyProduct ( "DLLServices" )]
[assembly: AssemblyCopyright ( "Copyright © 2013-2017, David A. Gray" )]
[assembly: AssemblyTrademark ( "This library is distributed under a three-clause BSD license." )]
[assembly: AssemblyCulture ( "" )]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible ( false )]

// The following GUID is for the ID of the type library if this project is exposed to COM.
[assembly: Guid ( "c1190387-03c3-430a-8ec4-d22173620201" )]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
//
//	============================================================================
//
//  ------------------------------------
//  Special Note about Assembly Version:
//  ------------------------------------
//
//  2014/05/19 4.1 Version promoted from 1.0 to 4.1, to match the version number
//                 listed in the revision history of ExceptionLogger.cs when it
//                 was moved into this library.
//
//  2015/06/04 5.4 Incorporate P6CUtilLib1.dll into the project, and mark it as
//                 Content to be copied if newer, so that any project that
//                 consumes this class is guaranteed access to it. Since the
//                 effects of this change are confined to the project 
//                 configuration, this is the only source file that is changed.
//
//  2015/06/09 5.5 Implement  the Unless() idiom that I've applied for years to
//                 my C and C++ projects. I don't know why I didn't think of
//                 this a long time ago. In the course of doing so, bring along
//                 the following classes, which I am moving from the 
//                 ApplicationHelpers namespace to this one.
//
//						ArrayInfo
//						ListInfo
//						SpecialCharacters
//						Util
//
//                 This is a breaking change, but I think it is acceptable,
//                 since pretty much everything I have already uses this DLL,
//                 yet relatively few, if any, production programs actually use
//                 the relocated properties and methods. In any case, I think 
//                 they can stay in their current locations for the time being.
//
//  2016/03/29 6.0 This version makes significant changes to several classes,
//                 which are summarized below. Affected classes are listed
//                 alphabetically.
//
//                 CmdLneArgsBasic:      Define methods to return named and
//                                       positional arguments as integers.
//
//                 ErrorMessagesInColor: Add methods to report their respective 
//                                       redirection states, both of which are
//                                       wrappers around an existing static
//                                       StandardHandleState method on the
//                                       StateManager class.
//
//                 ExceptionLogger:      BREAKING CHANGE ALERT
//                                       =====================
//
//                                       Simplify the ReportException
//                                       methods to get the name of the failing
//                                       method from the TargetSite property.
//
//                                       The break eliminates the second
//                                       argument from all method signatures.
//
//                                       To prevent breaking existing code, the
//                                       old methods are left, and are marked as
//                                       obsolete, to elicit a compiler warning
//                                       the next time a dependent assembly is
//                                       compiled.
//
//                 MessageInColor:       Add methods to report their respective
//                                       redirection states, just as I did in
//                                       ErrorMessagesInColor.
//
//                 StateManager:         Replace the reflected assembly with
//                                       System.Diagnostics.Process, leaving the
//                                       System.Reflection.Assembly property,
//                                       marked as obsolete, implemented as a
//                                       computed property for backwards
//                                       compatibility.
//
//                 Util:				 Define a new member, SafeConsoleClear,
//                                       for use as a drop-in replacement for
//                                       Console.Clear, which throws a trappable
//                                       exception if the standard output stream
//                                       is redirected.
//
//  2016/10/29 7.0 Break this library apart, so that smaller subsets of classes
//                 can be distributed and consumed independently.
//	============================================================================
[assembly: AssemblyVersion ( "7.0.493.*" )]