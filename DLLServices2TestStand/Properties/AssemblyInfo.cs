using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle ( "DLLServicesTestStand" )]
[assembly: AssemblyDescription ( "" )]
[assembly: AssemblyConfiguration ( "" )]
[assembly: AssemblyCompany ( "David A. Gray" )]
[assembly: AssemblyProduct ( "DLLServices" )]
[assembly: AssemblyCopyright ( "Copyright 2013-2016, David A. Gray" )]
[assembly: AssemblyTrademark ( "This library is distributed under a three-clause BSD license." )]
[assembly: AssemblyCulture ( "" )]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible ( false )]

// The following GUID is for the ID of the type library if this project is exposed to COM.
[assembly: Guid ( "b381d490-4cf9-42b8-b14e-79bd07e248a0" )]

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
//  ----------------------------------------------------------------------------
//  Revision History
//  ----------------------------------------------------------------------------
//
//  2015/06/04 5.4 Incorporate P6CUtilLib1.dll into the DLL project, marked as
//                 Content to be copied if newer, so that any project that
//                 consumes this class is guaranteed access to it. Since the
//                 effects of this change are confined to the configuration of
//                 the DLL project, this is the only source file of this project
//                 that is changed.
//
[assembly: AssemblyVersion ( "6.3.621.*" )]