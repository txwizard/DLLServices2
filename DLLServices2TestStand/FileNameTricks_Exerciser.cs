/*
    ============================================================================

	Name:				FileNameTricks_Exerciser

	Synopsis:			Put the FileNameTricks class through its paces.

	----------------------------------------------------------------------------
    Revision History
	----------------------------------------------------------------------------

    Date       Version Author Synopsis
    ---------- ------- ------ --------------------------------------------------
	2008/06/12 2.0.7.0 DAG/WW Add code to exercise the new FileNameTricks class.

    2009/12/15 2.4.15  DAG/WW Add text graphics to draw attention to internal
                              comments, and to separate them from the code.

    2010/10/22 2.52    DAG/WW Replace "Visual Studio 2005" with "Visual Studio
                              2010" in all string literals.

                              This fix is required to compensate for relocation
                              of this project, due to the upgrade to Visual
                              Studio 2010. Otherwise, the test suite crashes
                              with an unhandled exception.

    2011/11/28 2.63    DAG/WW Add PathMakeRelativeToPath method.

    ============================================================================
*/

using System;
using System.Collections.Generic;
using System.Text;

using WizardWrx;

namespace DLLServices2TestStand
{
    class FileNameTricks_Exerciser
    {
        const string DFLT_DIR_1 = @"C:\Documents and Settings\David\My Documents\Visual Studio 2010\Projects\WizardWrx_Libs\SharedUtl2\SharedUtl2_Test_Harness\";
        const string DFLT_DIR_2 = @"C:\Documents and Settings\David\My Documents\Visual Studio 2010\Projects\WizardWrx_Libs\SharedUtl2\SharedUtl2_Test_Harness";
        const string DFLT_DIR_3 = "";
        const string DFLT_DIR_4 = null;

        const string FQFN_1 = @"C:\Documents and Settings\David\My Documents\Visual Studio 2010\Projects\WizardWrx_Libs\SharedUtl2\SharedUtl2_Test_Harness\Directions_to_My_Place_Massage_DATA.htm";
        const string FQFN_2 = @"C:\Documents and Settings\David\My Documents\Visual Studio 2010\Projects\WizardWrx_Libs\SharedUtl2\SharedUtl2_Test_Harness\SharedUtl2_Test_Harness.CMD";

        const string PATH_1 = @"C:\Documents and Settings\David\My Documents\Visual Studio 2010\Projects\WizardWrx_Libs\SharedUtl2\SharedUtl2_Test_Harness\";
        const string PATH_2 = @"C:\Documents and Settings\David\My Documents\Visual Studio 2010\Projects\WizardWrx_Libs\SharedUtl2\SharedUtl2_Test_Harness";
        const string PATH_3 = @"SharedUtl2\SharedUtl2_Test_Harness";
        const string PATH_4 = @"\SharedUtl2\SharedUtl2_Test_Harness";

        const string UQFN_1 = @"Directions_to_My_Place_Massage_DATA.htm";
        const string UQFN_2 = @"SharedUtl2_Test_Harness.CMD";
        const string UQFN_3 = @"Directions_to_My_Place_Massage_DATA.";
        const string UQFN_4 = @"SharedUtl2_Test_Harness";

        const string TEST_REPORT_1 = "        Input String  = {0}\r\n        Output String = {1}\r\n";
        const string TEST_REPORT_2 = "        Input String  = {0}\r\n        Incl/Excl     = {1}\r\n        Output String = {2}\r\n";
        const string TEST_REPORT_3 = "        Input String  = {0}\r\n        Default Dir   = {1}\r\n        Output String = {2}\r\n";
        const string TEST_REPORT_4 = "        Resource Path = {0}\r\n        Working Dir   = {1}\r\n        Relative Path = {2}\r\n";

        const string SAMPLE_RESOURCE_PATH_1 = @"C:\Documents and Settings\David\My Documents\Visual Studio 2010\Projects\WizardWrx_Libs\SharedUtl2\SharedUtl2\bin\Release\WizardWrx.SharedUtl2.dll";
        const string SAMPLE_RESOURCE_PATH_2 = @"C:\Documents and Settings\David\My Documents\Visual Studio 2010\Projects\_TESTING WizardWrx Libs\SharedUtl2\SharedUtl2\bin\Release\WizardWrx.SharedUtl2.dll";
        
        const string SAMPLE_WORKING_PATH_FILE = @"C:\Documents and Settings\David\My Documents\Visual Studio 2010\Projects\Tools_for_Programmers\AssemblyReferenceFixup\_Notes\WebApp.csproj";
        const string SAMPLE_WORKING_PATH_DIR1 = @"C:\Documents and Settings\David\My Documents\Visual Studio 2010\Projects\Tools_for_Programmers\AssemblyReferenceFixup\_Notes\";
        const string SAMPLE_WORKING_PATH_DIR2 = @"C:\Documents and Settings\David\My Documents\Visual Studio 2010\Projects\Tools_for_Programmers\AssemblyReferenceFixup\_Notes";

        static string [ ] astrDfltDirNames =
		{
            DFLT_DIR_1 ,
            DFLT_DIR_2 ,
            DFLT_DIR_3 ,
            DFLT_DIR_4
		};	// astrDfltDirNames

        static string [ ] astrTestPathsAll =
		{
            FQFN_1 ,
            FQFN_2 ,
            PATH_1 ,
            PATH_2 ,
            PATH_3 ,
            PATH_4 ,
            UQFN_1 ,
            UQFN_2 ,
            UQFN_3 ,
            UQFN_4
        };	// astrTestPathsAll

        static string [ ] astrTestPathStrings =
		{
            PATH_1 ,
            PATH_2 ,
            PATH_3 ,
            PATH_4
		};	// astrTestPathStrings

		static string [ ] astrTestFileNameStrings =
		{
            FQFN_1 ,
            FQFN_2 ,
            UQFN_1 ,
            UQFN_2 ,
            UQFN_3 ,
            UQFN_4
        };	// astrTestFileNameStrings

        static string [ ] astreResources =
        {
            SAMPLE_RESOURCE_PATH_1 ,
            SAMPLE_RESOURCE_PATH_2
        };	// astreResources

        static string [ ] astrWorkingDirs =
        {
            SAMPLE_WORKING_PATH_FILE ,
            SAMPLE_WORKING_PATH_DIR1 ,
            SAMPLE_WORKING_PATH_DIR2
        };	// astrWorkingDirs

        private FileNameTricks_Exerciser ( ) { }  // Prohibit an instance.

        public static void Drill()
        {
            int intTestNumber = WizardWrx.MagicNumbers.ZERO;

			Console.WriteLine ( @"{0}Begin exercising the WizardWrx.FileNameTricks class{0}" , Environment.NewLine );

			//	----------------------------------------------------------------
			//	Display all the enumeration values and other constants.
			//	----------------------------------------------------------------

            Console.WriteLine("    Test {0} - Exercising enumerations and constants:\r\n", ++intTestNumber);
            Console.WriteLine("        Enumeration TerminaBackslash.Include = {0}", WizardWrx.FileNameTricks.TerminaBackslash.Include);
            Console.WriteLine("        Enumeration TerminaBackslash.Omit    = {0}", WizardWrx.FileNameTricks.TerminaBackslash.Omit);
            Console.WriteLine("\r\n    Finished exercising enumerations and constants\r\n");

			//	----------------------------------------------------------------
			//	Exercise EnsureHasTerminalBackslash
			//	----------------------------------------------------------------

            Console.WriteLine("    Test {0} - Testing method EnsureHasTerminalBackslash:\r\n", ++intTestNumber);

            foreach (string strTestString in astrTestPathStrings)
            {
                string strOutputString = WizardWrx.FileNameTricks.EnsureHasTerminalBackslash(
                    strTestString);
                Console.WriteLine(
                    TEST_REPORT_1,
                    strTestString,
                    strOutputString);
			}	// foreach (string strTestString in astrTestPathStrings)

            Console.WriteLine("    Finished testing method EnsureHasTerminalBackslash\r\n");

			//	----------------------------------------------------------------
			//	Exercise EnsureNoTerminalBackslash
			//	----------------------------------------------------------------

            Console.WriteLine("    Test {0} - Testing method EnsureNoTerminalBackslash:\r\n", ++intTestNumber);

            foreach (string strTestString in astrTestPathStrings)
            {
                string strOutputString = WizardWrx.FileNameTricks.EnsureNoTerminalBackslash(
                    strTestString);
                Console.WriteLine(
                    TEST_REPORT_1,
                    strTestString,
                    strOutputString);
			}	// foreach (string strTestString in astrTestPathStrings)

            Console.WriteLine("    Finished testing method EnsureNoTerminalBackslash\r\n");


			//	----------------------------------------------------------------
			//	Exercise FileDirName
			//	----------------------------------------------------------------

            Console.WriteLine("    Test {0} - Testing method FileDirName:\r\n", ++intTestNumber);

            foreach (string strTestString in astrTestFileNameStrings)
            {	//	------------------------------------------------------------
				//	Include trailing backslash.
				//	------------------------------------------------------------

                WizardWrx.FileNameTricks.TerminaBackslash enmIncludeOrOmit = WizardWrx.FileNameTricks.TerminaBackslash.Include;
				{	//	Constrain the scope of string variable strOutputString.
                    string strOutputString = WizardWrx.FileNameTricks.FileDirName(
                        strTestString,
                        enmIncludeOrOmit);
                    Console.WriteLine(
                        TEST_REPORT_2,
                        strTestString,
                        enmIncludeOrOmit,
                        strOutputString);
				}	// String strOutputString goes out of scope.


				//	------------------------------------------------------------
				//	Omit the trailing backslash.
				//	------------------------------------------------------------

                enmIncludeOrOmit = WizardWrx.FileNameTricks.TerminaBackslash.Omit;
                {
                    string strOutputString = WizardWrx.FileNameTricks.FileDirName(
                        strTestString,
                        enmIncludeOrOmit);
                    Console.WriteLine(
                        TEST_REPORT_2,
                        strTestString,
                        enmIncludeOrOmit,
                        strOutputString);
                }
			}	// foreach (string strTestString in astrTestFileNameStrings)

            Console.WriteLine("    Finished testing method FileDirName\r\n");

			//	----------------------------------------------------------------
			//	Exercise FileExtn
			//	----------------------------------------------------------------

            Console.WriteLine("    Test {0} - Testing method FileExtn:\r\n", ++intTestNumber);

            foreach (string strTestString in astrTestFileNameStrings)
            {
                string strOutputString = WizardWrx.FileNameTricks.FileExtn(
                    strTestString);
                Console.WriteLine(
                    TEST_REPORT_1,
                    strTestString,
                    strOutputString);
			}	// foreach (string strTestString in astrTestFileNameStrings)

            Console.WriteLine("    Finished testing method FileExtn\r\n");

			//	----------------------------------------------------------------
			//	Exercise FQFBasename
			//	----------------------------------------------------------------

            Console.WriteLine("    Test {0} - Testing method FQFBasename:\r\n", ++intTestNumber);

            foreach (string strTestString in astrTestFileNameStrings)
            {
                string strOutputString = WizardWrx.FileNameTricks.FQFBasename(
                    strTestString);
                Console.WriteLine(
                    TEST_REPORT_1,
                    strTestString,
                    strOutputString);
			}	// foreach (string strTestString in astrTestFileNameStrings)

            Console.WriteLine("    Finished testing method FQFBasename\r\n");

			//	----------------------------------------------------------------
			//	Exercise MakeFQFN
			//	----------------------------------------------------------------

            Console.WriteLine("    Test {0} - Testing method MakeFQFN:\r\n", ++intTestNumber);

            foreach (string strDefaultDir in astrDfltDirNames)
            {
                foreach (string strTestString in astrTestFileNameStrings)
                {
                    string strOutputString = WizardWrx.FileNameTricks.MakeFQFN(
                        strTestString,
                        strDefaultDir);
                    Console.WriteLine(
                        TEST_REPORT_3,
                        strTestString,
                        strDefaultDir,
                        strOutputString);
                }
			}	// foreach (string strDefaultDir in astrDfltDirNames)

            Console.WriteLine("    Finished testing method MakeFQFN\r\n");

			//	----------------------------------------------------------------
			//	Exercise PathFixup
			//	----------------------------------------------------------------

            Console.WriteLine("    Test {0} - Testing method PathFixup:\r\n", ++intTestNumber);

            foreach (string strTestString in astrTestPathStrings)
            {	//	------------------------------------------------------------
                //	Include trailing backslash.
				//	------------------------------------------------------------

                WizardWrx.FileNameTricks.TerminaBackslash enmIncludeOrOmit = WizardWrx.FileNameTricks.TerminaBackslash.Include;
                {
                    string strOutputString = WizardWrx.FileNameTricks.PathFixup(
                        strTestString,
                        enmIncludeOrOmit);
                    Console.WriteLine(
                        TEST_REPORT_2,
                        strTestString,
                        enmIncludeOrOmit,
                        strOutputString);
                }

				//	------------------------------------------------------------
				//	Omit the trailing backslash.
				//	------------------------------------------------------------

                enmIncludeOrOmit = WizardWrx.FileNameTricks.TerminaBackslash.Omit;
                {
                    string strOutputString = WizardWrx.FileNameTricks.PathFixup(
                        strTestString,
                        enmIncludeOrOmit);
                    Console.WriteLine(
                        TEST_REPORT_2,
                        strTestString,
                        enmIncludeOrOmit,
                        strOutputString);
                }
			}	// foreach (string strTestString in astrTestPathStrings)

            Console.WriteLine("    Finished testing method PathFixup\r\n");

			//	----------------------------------------------------------------
			//	Exercise UQFBasename
			//	----------------------------------------------------------------

            Console.WriteLine("    Test {0} - Testing method UQFBasename:\r\n", ++intTestNumber);

            foreach (string strTestString in astrTestPathsAll)
            {
                string strOutputString = WizardWrx.FileNameTricks.UQFBasename(
                    strTestString);
                Console.WriteLine(
                    TEST_REPORT_1,
                    strTestString,
                    strOutputString);
            }

            Console.WriteLine("    Finished testing method UQFBasename\r\n");

            //	----------------------------------------------------------------
            //	Exercise PathMakeRelativeToPath
            //	----------------------------------------------------------------

            Console.WriteLine ( "    Test {0} - Testing method PathMakeRelativeToPath:\r\n" , ++intTestNumber );

            foreach ( string strReource in astreResources )
            {
                foreach ( string strWorkingDir in astrWorkingDirs )
                {
                    string strOutputString = WizardWrx.FileNameTricks.PathMakeRelative (
                        strReource ,
                        strWorkingDir );
                    Console.WriteLine (
                        TEST_REPORT_4 ,
                        strReource ,
                        strWorkingDir ,
                        strOutputString );
                }   // foreach ( string strWorkingDir in astrWorkingDirs )
            }   // foreach ( string strReource in astreResources )

            Console.WriteLine ( "    Finished testing method PathMakeRelativeToPath\r\n" );

            Console.WriteLine(@"Finished exercising the WizardWrx.FileNameTricks class");
        }   // public static method Drill
    }   // class FileNameTricks_Exerciser
}   // partial namespace DLLServices2TestStand