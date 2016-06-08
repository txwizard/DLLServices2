/*
    ============================================================================

    File Name:			AssemblyLocatorBase.cs

    Namespace:			WizardWrx.DLLServices

    Class Name:			AssemblyLocatorBase

    Synopsis:			Derive a class from this when you need the location from
						which the assembly in which it is defined was loaded.

    Important:			I have discovered that any assembly that defines a class
						derived from AssemblyLocatorBase depends upon this
						assembly, although the build engine may not make it so,
						especially when the dependency is indirect.

						Likewise, I have yet to find a way to force the build 
						engine to bring along a configuration file that is 
						paired with a DLL.

						However, since I have discovered how easy it is to embed
						a text file in an assembly and read it at run time, some
						of the situations that I once thought required a
						mechanism for finding a related text file can be solved
						by embedding the text file in the assembly, so long as
						its data is truly static.

    Remarks:			The motivation for this class was a need to initialize a
						static property from information stored in a
						configuration file that is associated with, and 
						accompanies, a DLL.

						Since much of its desired behavior must be evaluated in
						the context of a working DLL, its initial test stand was
						the DataEase DLL that motivated its creation.

						Not surprisingly, the DataEase DLL that motivated the
						creation of this abstract base class was among the first
						beneficiaries of resources stored as text files that are
						embedded into it as custom resources.

    References:			1)  "XML Reserved Characters"
							http://msdn.microsoft.com/en-us/library/ms145315(v=sql.90).aspx

						2)  "Naming Files, Paths, and Namespaces (Windows)"
							http://msdn.microsoft.com/en-us/library/windows/desktop/aa365247(v=vs.85).aspx

    Author:				David A. Gray

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
    2012/12/26 2.91   DAG    This class makes its first appearance.

    2012/12/26 2.92   DAG    Extend this class with virtual methods for reading
                             its associated configuration file.

    2013/02/21 2.96   DAG    Move the class into a new subsidiary namespace of
                             WizardWrx.ApplicationHelpers, DLLServices, which is
                             housed in a new DLL.

                             Make the following improvements, identified during
                             use in class library DataEase.dll.

                             1) Exposing the name of the directory from which
                                the DLL loaded expedites creating a fully
                                qualified name from the unqualified name of a
                                file that is expected to inhabit the same
                                directory.

                             2) If the configuration reader supported a token
                                for the AssemblyDataPath, it could fix up a
                                configuration value before returning it.

                             3) To accommodate assemblies loaded from the GAC,
                                it should interrogate its meta-data, and
                                substitute the name of the directory from which
                                the first assembly loaded into the process was
                                loaded.

                             4) At a minimum, the assembly should provide a
                                simple means of reporting that its configuration
                                file is missing.

                             The text above came, verbatim, from notes that I
                             made in preparation for this update.

                             Demote the DLLConfiguration and DLLettingsSection
                             properties to protected access.

                             To test its ability to find the application when it
                             is loaded from the GAC, the assembly that houses it
                             requires a strong name. I've put this off for as
                             long as I could.

    2014/06/08 5.1    DAG    BREAKING CHANGE

                             Promote the DLLServices namespace to the first rank
                             of namespaces defined under namespace WizardWrx.

                             Since there are only about three assemblies that
                             refer in any way to this library, its impact is
                             pretty limited.

    2014/09/08 5.2    DAG    1) While I was cleaning up the XML documentation, I
                                noticed that two methods, GetAssemblyVersion and
                                GetAssemblyVersionString, were calling
                                GetEntryAssembly, which returns a reference to
                                the assembly that contains the application entry
                                point, rather than GetExecutingAssembly, which
                                gets the assembly in which the calling code is
                                executing.

                             2) This update began as a cosmetic cleanup of the
                                XML documentation.

    2014/10/14 5.3    DAG    Promote to WizardWrx.DLLServices2, so that I can
                             break the link with WizardWrx.DLLServices and break
                             free of its strong name and associated version.

	2015/06/20 5.5    DAG    Incorporate my three-clause BSD license.

	2016/04/02 6.0    DAG    Correct typographical errors, and mark the ends of
                             the conditional compilation blocks.
    ============================================================================
*/


using System;
using System.Configuration;

/* Added by DAG */

using System.IO;
using System.Reflection;


namespace WizardWrx.DLLServices2
{
    /// <summary>
    /// Use a class derived from this class to get the fully qualified name of
    /// the file from which the assembly in which the derived class is defined
    /// was loaded. See Remarks.
    /// </summary>
    /// <remarks>
    /// Given the location from which an assembly was loaded, you can learn
    /// almost anything else you need to know about that file, such as its size,
    /// age, and directory. Given the directory, you can locate satellite files,
    /// such as custom configuration files.
    /// </remarks>
	/// <seealso cref="PropertyDefaults"/>
    public abstract class AssemblyLocatorBase
    {
        #region Public Constants
        //  --------------------------------------------------------------------
        //  The following table lists the four XML Reserved Characters and the
        //  entity reference with which they must be replaced.
        //
        //      --------------------------------------------
        //      Character   Meaning         Entity reference
        //      ---------   ------------    ----------------
        //      >           Greater than    &gt;
        //      <           Less than       &lt;
        //      &           Ampersand       &amp;
        //      %           Percent         &#37;
        //      --------------------------------------------
        //
        //  Rather than use these entity references in my substitution tokens, I
        //  selected a different token delimiter.
        //
        //  See reference 1.
        //
        //  The following characters, which are reserved by either the command
        //  line parser or the file mask parser, are also best avoided.
        //
        //      < (less than)
        //      > (greater than)
        //      : (colon)
        //      " (double quote)
        //      / (forward slash)
        //      \ (backslash)
        //      | (vertical bar or pipe)
        //      ? (question mark)
        //      * (asterisk)
        //
        //  Astute observers will recognize some overlap in the above two lists.
        //  Although not listed in the table of XML reserved characters, I
        //  suspect that a forward slash in the InnerText of an element is also
        //  problematic.
        //
        //  See reference 2.
        //  --------------------------------------------------------------------

        /// <summary>
        /// Use this token in file names stored in DLL configuration files to
        /// explicitly state that the file is expected to inhabit the directory
        /// from which the assembly is loaded, unless the assembly was loaded
        /// from the Global Assembly Cache (GAC). In that case, substitute the
        /// application directory.
        /// </summary>
        public const string ASSEMBLYDATAPATH_TOKEN = @"$$AssemblyDataPath$$";
        #endregion  // Public Constants


        #region Private Constants and Instance Storage
        const int ALL_VERSION_PARTS = 4;

#if DEBUG_MESSAGES_WW
        const string DEBUG_MESSAGES_WW_TPL = @"DEBUG_MESSAGES_WW, in class {0} (derived from AssemblyLocatorBase): {1} = {2}";
#endif	// #if DEBUG_MESSAGES_WW

		const string APPSETTINGS = @"appSettings";

        const string DLL_CONFIG_FILE_NAME_TEMPLATE = @"{0}{1}";
        const string DLL_CONFIG_FILE_SUFFIX = @".config";

        const int EMPTY_CONFIG = 0;

        const bool FOR_SHOW = true;
        const bool FOR_USE = false;

        /// <summary>
        /// Once the energy required to gather the location has been expended,
        /// save it for future use.
        /// </summary>
        string _strAssemblyLocation = null;

        /// <summary>
        /// Likewise, hang onto the AssemblyDataPath.
        /// </summary>
        string _strAssemblyDataPath = null;

        /// <summary>
        /// The assembly configuration file has the same name as does the
		/// assembly, with an extension of .config appended. However, if the DLL
		/// loaded from the GAC, its configuration file must live in the
		/// application directory.
        /// </summary>
        string _strAssemblyConfigPath = null;

        string _strConfigMessage = string.Empty;

        string _strStartupAssemblyLocation = null;

        bool _fLoadedFromGAC = false;
        #endregion  // Private Constants and Instance Storage


        #region Constructors
        /// <summary>
        /// Initialize the one and only property of this class, which holds the
        /// fully qualified path from which the containing assembly was loaded.
        /// </summary>
        public AssemblyLocatorBase ( )
        {
            _strAssemblyLocation = this.GetType ( ).Assembly.Location;

#if DEBUG_MESSAGES_WW
            Console.WriteLine (
                DEBUG_MESSAGES_WW_TPL ,
                System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                "_strAssemblyLocation" ,
                _strAssemblyLocation );
#endif	// #if DEBUG_MESSAGES_WW

			if ( this.GetType ( ).Module.Assembly.GlobalAssemblyCache )
            {   // Assembly loaded from GAC. Use application path.
                Assembly asmStartup = Assembly.GetEntryAssembly ( );
                _strStartupAssemblyLocation = asmStartup.Location;
                _strAssemblyDataPath = Path.GetDirectoryName ( _strStartupAssemblyLocation );
                _strAssemblyConfigPath = Path.Combine (
                    _strAssemblyDataPath ,
                    Path.GetFileName ( _strAssemblyLocation ) );
                _fLoadedFromGAC = true;      // Remember the answer.

#if DEBUG_MESSAGES_WW
                Console.WriteLine (
                    DEBUG_MESSAGES_WW_TPL ,
                    System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                    "_strStartupAssemblyLocation" ,
                    _strStartupAssemblyLocation );
                Console.WriteLine (
                    DEBUG_MESSAGES_WW_TPL ,
                    System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                    "_strAssemblyDataPath" ,
                    _strAssemblyDataPath );
                Console.WriteLine (
                    DEBUG_MESSAGES_WW_TPL ,
                    System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                    "_strAssemblyConfigPath" ,
                    _strAssemblyConfigPath );
                Console.WriteLine (
                    DEBUG_MESSAGES_WW_TPL ,
                    System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                    "_fLoadedFromGAC" ,
                    _fLoadedFromGAC );
#endif	// #if DEBUG_MESSAGES_WW
			}   // TRUE block, if ( this.GetType ( ).Module.Assembly.GlobalAssemblyCache )
            else
            {   // Assembly loaded from application directory. Use its path.
                _strAssemblyDataPath = Path.GetDirectoryName ( _strAssemblyLocation );
                _strAssemblyConfigPath = _strAssemblyLocation;
                _fLoadedFromGAC = false;     // Set it anyway, to be clear.
#if DEBUG_MESSAGES_WW
                Console.WriteLine (
                    DEBUG_MESSAGES_WW_TPL ,
                    System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                    "_strAssemblyDataPath" ,
                    _strAssemblyDataPath );
                Console.WriteLine (
                    DEBUG_MESSAGES_WW_TPL ,
                    System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                    "_strAssemblyConfigPath" ,
                    _strAssemblyConfigPath );
                Console.WriteLine (
                    DEBUG_MESSAGES_WW_TPL ,
                    System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                    "_fLoadedFromGAC" ,
                    _fLoadedFromGAC );
#endif	// #if DEBUG_MESSAGES_WW
			}   // FALSE block, if ( this.GetType ( ).Module.Assembly.GlobalAssemblyCache )
        }   // public AssemblyLocatorBase (constructor 1 of 1)
        #endregion  // Constructors


        #region Protected and Public ReadOnly Properties
        /// <summary>
        /// Gets a string containing the fully qualified path of the directory
        /// from which the assembly was loaded, unless it was loaded from the
        /// Global Assembly Cache (GAC). In that case, the return value is the
        /// fully qualified name of the directory from which the first assembly
        /// was loaded into the current process. See Remarks.
        /// </summary>
        /// <remarks>
        /// So far as I know, assemblies must load from one of two locations.
        /// Unsigned assemblies MUST load from the application directory. If the
        /// assembly is signed with a strong name, it MAY  be loaded from either
        /// the application directory or the Global Assembly Cache. If a signed
        /// assembly is in the local GAC, it loads from there, even if a copy is
        /// also stored in the application directory.
        /// </remarks>
        public string AssemblyDataPath { get { return _strAssemblyDataPath; } }


        /// <summary>
        /// Gets a string containing the fully qualfied file name from which
        /// the assembly in which the derived class is defined was loaded.
        /// </summary>
        public string AssemblyLocation
        { get { return _strAssemblyLocation; } }


        /// <summary>
        /// This property returns a message when the configuration file is
        /// missing or empty. Under normal conditions, it returns the empty
        /// string.
        /// </summary>
        public string ConfigMessage
        { get { return _strConfigMessage; } }


        /// <summary>
        /// Gets a reference to the entire Configuration object tied to the
        /// assembly in which the derived class is defined.
        /// </summary>
        protected Configuration DLLConfiguration
        {
            get
            {
                string strAssemblyConfigFleFQFN = FullyQualifiedDLLConfigFileName ( FOR_USE );

#if DEBUG_MESSAGES_WW
                Console.WriteLine (
                    DEBUG_MESSAGES_WW_TPL ,
                    System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                    "strAssemblyConfigFleFQFN" ,
                    strAssemblyConfigFleFQFN );
#endif	// #if DEBUG_MESSAGES_WW

				if ( File.Exists ( strAssemblyConfigFleFQFN ) )
                {   // Configuration file found. Load it.
                    return ConfigurationManager.OpenExeConfiguration ( strAssemblyConfigFleFQFN );
                }   // TRUE (normal) block, if ( File.Exists ( strAssemblyConfigFleFQFN ) )
                else
				{   // The configuration file is missing. Report by way of the ConfigMessage property, and return a null reference.
                    _strConfigMessage = string.Format (
                        Properties.Resources.ERRMSG_MISSING_CONFIG_FILE ,
                        strAssemblyConfigFleFQFN );
                    return null;
                }   // FALSE (missing configuration file) block, if ( File.Exists ( strAssemblyConfigFleFQFN ) )
            }   // protected Configuration DLLConfiguration Get method
        }   // protected Configuration DLLConfiguration property


        /// <summary>
        /// Gets a reference to the entire AppSettingsSection object tied to
        /// the assembly in which the derived class is defined.
        /// </summary>
        /// <remarks>
        /// Since this property starts from the ConfigurationManager object
        /// returned by its DLLConfiguration sibling, it requires only a single
        /// statement, with a little help from an explicit cast.
        /// </remarks>
        protected AppSettingsSection DLLettingsSection
        {
            get
            {
                Configuration cfgSection = this.DLLConfiguration;

                if ( cfgSection != null )
                {
                    AppSettingsSection rcfgAppSettings = ( AppSettingsSection ) cfgSection.GetSection ( APPSETTINGS );

                    if ( rcfgAppSettings.Settings.Count == EMPTY_CONFIG )
                    {
                        _strConfigMessage = string.Format (
							Properties.Resources.ERRMSG_CONFIG_FILE_IS_EMPTY ,
                            FullyQualifiedDLLConfigFileName ( FOR_SHOW ) );

#if DEBUG_MESSAGES_WW
                        Console.WriteLine (
                            DEBUG_MESSAGES_WW_TPL ,
                            System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
                            "_strConfigMessage" ,
                            _strConfigMessage );
#endif	// #if DEBUG_MESSAGES_WW
					}   // Notify the caller if the file is empty.

                    return rcfgAppSettings;
                }   // TRUE (normal case) block, if ( cfgSection != null )
                else
                {   // The absent file has already been noted.
                    return null;
                }   // FALSE (null case) block, if ( cfgSection != null )
            }   // protected AppSettingsSection DLLettingsSection Get method
        }   // protected AppSettingsSection DLLettingsSection property


        /// <summary>
        /// Gets the DLL Settings section as a KeyValueConfigurationCollection.
        /// </summary>
        protected KeyValueConfigurationCollection DLLSettings
        {
            get
            {
                if ( this.DLLettingsSection != null )
                    return this.DLLettingsSection.Settings;
                else
                    return null;
            }   // // protected KeyValueConfigurationCollection DLLSettings Get method
        }   // protected KeyValueConfigurationCollection DLLSettings
        #endregion  // Protected and Public ReadOnly Properties


        #region Protected Methods
        /// <summary>
        /// Return the LastWriteTime of the file that contains the executing
        /// assembly. For all practical purposes, that is the date on which the
        /// assembly was built.
        /// </summary>
        /// <param name="pdtmKind">
        /// This DateTimeKind enumeration member specifies whether to report the
        /// LastWriteTime or the LastWriteTimeUtc.
        /// </param>
        /// <returns>
        /// The return value is a fully initialized DateTime structure, which
        /// contains the requested LastWriteTime (Local or UTC) of the file that
        /// contains the code of the executing assembly.
        /// </returns>
        protected DateTime GetAssemblyBuildDate ( DateTimeKind pdtmKind )
        {
            FileInfo fiThisAssembly = new FileInfo ( _strAssemblyLocation );

            switch ( pdtmKind )
            {
                case DateTimeKind.Unspecified:
                case DateTimeKind.Local:
                    return fiThisAssembly.LastWriteTime;
                case DateTimeKind.Utc:
                    return fiThisAssembly.LastWriteTimeUtc;
                default:
                    return fiThisAssembly.LastWriteTime;
            }   // switch ( pdtmKind )
        }   // protected DateTime GetAssemblyBuildDate


        /// <summary>
        /// Return the Version structure, to expedite parsing its parts.
        /// </summary>
        /// <returns>
        /// The return value is the version component of the fully qualified
        /// assembly name.
        /// </returns>
        protected Version GetAssemblyVersion ( )
        {
            return Assembly.GetExecutingAssembly ( ).GetName ( ).Version;
        }   // protected Version GetAssemblyVersion


        /// <summary>
        /// Return the complete version of the executing assembly.
        /// </summary>
        /// <returns>
        /// The return value is a string representation of all version number
        /// "octets" - Major, Minor, Build, and Revision.
        /// </returns>
        protected string GetAssemblyVersionString ( )
        {
            return Assembly.GetExecutingAssembly ( ).GetName ( ).Version.ToString ( ALL_VERSION_PARTS );
        }   // protected string GetAssemblyVersionString
        #endregion  // Protected Methods


        #region Public Methods
        /// <summary>
        /// Return the specified setting value, as a string.
        /// </summary>
        /// <param name="pstrSettingsKey">
        /// This string is the name (key) of the desired setting.
        /// </param>
        /// <returns>
        /// The return value is a string representation of the value stored in
        /// the named key.
        /// </returns>
        public string GetDLLSetting ( string pstrSettingsKey )
        {
            if ( string.IsNullOrEmpty ( pstrSettingsKey ) )
                return string.Empty;
            else
                return this.DLLSettings [ pstrSettingsKey ].Value.Replace (
                    ASSEMBLYDATAPATH_TOKEN ,
                    _strAssemblyDataPath );
        }   // public string GetDLLSetting
        #endregion  // Public Methods


        #region Private Instance Methods
        private string FullyQualifiedDLLConfigFileName ( bool pfIsForShow )
        {
            if ( pfIsForShow )
                return string.Format (
                    DLL_CONFIG_FILE_NAME_TEMPLATE ,
                    _strAssemblyConfigPath ,
                    DLL_CONFIG_FILE_SUFFIX );
            else
                if ( _fLoadedFromGAC )
                    return _strStartupAssemblyLocation;
                else
                    return _strAssemblyConfigPath;
        }   // private string FullyQualifiedDLLConfigFileName
        #endregion  // Private Instance Methods
    }   // public abstract class AssemblyLocatorBase
}   // namespace WizardWrx.DLLServices2