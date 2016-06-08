using System;
using CSharpParser.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
 
namespace CSharpParserTest
{
 /// <summary>
 /// Summary description for StringHelperTest
 /// </summary>
	public static class StringHelperTest
	{
		const string BARE_NEWLINE = @"\r\\n";
		const string PARAMETER_NAME = @"\r\\nParameter name: ";
		const string OBJECT_NAME = @"\r\\nObject name: ";

		public enum Stage
		{
			Beginning ,
			Ending
		};	// Stage

		const char DOUBLE_QUOTE = '"';

		[STAThread]
		public static void Main ( string [ ] pastrArgs )
		{
			System.Reflection.Assembly asmExec = System.Reflection.Assembly.GetExecutingAssembly ( );
			Console.WriteLine (
				"{0} BOJ{2}{2}Target .NET Framework version: {1}{2}" ,
				asmExec.FullName ,
				asmExec.ImageRuntimeVersion ,
				Environment.NewLine );

			if ( pastrArgs.Length == 0 || pastrArgs [ 0 ] != "SpecificStringTests" )
			{
				StringLiteralsAreOK ( );
				StringLiteralExceptionsAreOK ( );
				CharLiteralsAreOK ( );
				CharLiteralExceptionsAreOK ( );
				VerbatimLiteralsAreOK ( );
			}	// if ( pastrArgs.Length == 0 || pastrArgs [ 0 ] != "SpecificStringTests" )

			SpecificStringTests ( );

			Console.WriteLine (
				"{1}{0} EOJ{1}<*>" ,
				System.Reflection.Assembly.GetExecutingAssembly ( ).FullName ,
				Environment.NewLine );
		}	// public static void Main

		[TestMethod]
		public static string QuoteString ( string pstrQuoteThis )
		{
			return string.Concat (
				DOUBLE_QUOTE ,
				pstrQuoteThis ,
				DOUBLE_QUOTE );
		}	// public static string QuoteString

		[TestMethod]
		public static void SpecificStringTests ( )
		{
			ReportTaskAndStage (
				System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
				Stage.Beginning );

			int intCaseNumber = 0;

			foreach ( string strThisCase in System.IO.File.ReadAllLines ( "SpecificStringTests.TXT" , System.Text.Encoding.ASCII ) )
			{
				string strTreatedAsVerbatimLiteral = StringHelper.StringFromVerbatimLiteral ( strThisCase );
				string strTreatedAsCSharpLiteral = StringHelper.StringFromCSharpLiteral ( strThisCase );

				ReportSpecificStringTests (
					++intCaseNumber ,
					strThisCase ,
					strTreatedAsVerbatimLiteral ,
					strTreatedAsCSharpLiteral );
			}	// foreach ( string strThisCase in System.IO.File.ReadAllLines ( "SpecificStringTests.TXT" , System.Text.Encoding.ASCII ) )

			ReportTaskAndStage (
				System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
				Stage.Ending );
		}	// public static void SpecificStringTests

		[TestMethod]
		private static void ReportSpecificStringTests (
			int pintCaseNumber ,
			string pstrThisCase ,
			string pstrTreatedAsVerbatimLiteral ,
			string pstrTreatedAsCSharpLiteral )
		{
			Console.WriteLine (
				"Case # {0}: pstrThisCase                 = {1}, length = {2}{7}          pstrTreatedAsVerbatimLiteral = {3}, length = {4}{7}          pstrTreatedAsCSharpLiteral   = {5}, length = {6}{7}" ,
				new string [ ]
				{
					pintCaseNumber.ToString ( ) ,								// Format Item 0: Case #
					QuoteString ( pstrThisCase ) ,								// Format Item 1: Input string pstrThisCase
					pstrThisCase.Length.ToString ( ) ,							// Format Item 2: Length of string pstrThisCase
					QuoteString ( pstrTreatedAsVerbatimLiteral ) ,				// Format Item 3: Output string pstrTreatedAsCSharpLiteral
					pstrTreatedAsVerbatimLiteral.Length.ToString ( ) ,			// Format Item 4: Length of string pstrTreatedAsCSharpLiteral
					QuoteString ( pstrTreatedAsCSharpLiteral ) ,				// Format Item 5: Output string pstrTreatedAsVerbatimLiteral
					pstrTreatedAsCSharpLiteral.Length.ToString ( ) ,			// Format Item 6: Length of string pstrTreatedAsVerbatimLiteral
					Environment.NewLine											// Format Item 7: Newline, my way
				} );
		}	// private static void ReportSpecificStringTests

		[TestMethod]
		public static void StringLiteralsAreOK ( )
		{
			ReportTaskAndStage (
				System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
				Stage.Beginning );

			string csharp = "Hello, world";
			string normal = "Hello, world";
			try
			{
				Assert.AreEqual ( StringHelper.StringFromCSharpLiteral ( csharp ) , normal );
			}
			catch ( ArgumentException exArg )
			{
				ReportArgException (
					csharp ,
					exArg );
			}

			csharp = "\\\\Hello, world";
			normal = "\\Hello, world";
			try
			{
				Assert.AreEqual ( StringHelper.StringFromCSharpLiteral ( csharp ) , normal );
			}
			catch ( ArgumentException exArg )
			{
				ReportArgException (
					csharp ,
					exArg );
			}

			//      csharp = "\\\\\\a\\b\\'\\\"\\0\\f\ \ \\t\\v";		This is the original shown on the Web site.
			csharp = "\\\\\\a\\b\\'\\\"\\0\\f \\t\\v";		//	This is my amended version, which the parser accepts.
			normal = "\\\a\b\'\"\0\f \t\v";
			try
			{
				Assert.AreEqual ( StringHelper.StringFromCSharpLiteral ( csharp ) , normal );
			}
			catch ( ArgumentException exArg )
			{
				ReportArgException (
					csharp ,
					exArg );
			}

			csharp = "Hello\\x1\\x12\\x123\\x1234";
			normal = "Hello\x1\x12\x123\x1234";
			try
			{
				Assert.AreEqual ( StringHelper.StringFromCSharpLiteral ( csharp ) , normal );
			}
			catch ( ArgumentException exArg )
			{
				ReportArgException (
					csharp ,
					exArg );
			}

			csharp = "Hello\\x1\\x12\\x123\\x123456";
			normal = "Hello\x1\x12\x123\x123456";
			try
			{
				Assert.AreEqual ( StringHelper.StringFromCSharpLiteral ( csharp ) , normal );
			}
			catch ( ArgumentException exArg )
			{
				ReportArgException (
					csharp ,
					exArg );
			}

			csharp = "\\u1234\\u2345\\u3456";
			normal = "\u1234\u2345\u3456";
			try
			{
				Assert.AreEqual ( StringHelper.StringFromCSharpLiteral ( csharp ) , normal );
			}
			catch ( ArgumentException exArg )
			{
				ReportArgException (
					csharp ,
					exArg );
			}

			csharp = "\\U00001234\\U00002345\\U00003456";
			normal = "\U00001234\U00002345\U00003456";
			try
			{
				Assert.AreEqual ( StringHelper.StringFromCSharpLiteral ( csharp ) , normal );
			}
			catch ( ArgumentException exArg )
			{
				ReportArgException (
					csharp ,
					exArg );
			}

			ReportTaskAndStage (
				System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
				Stage.Ending);
		}	// public static void StringLiteralsAreOK

		[TestMethod]
		public static void StringLiteralExceptionsAreOK ( )
		{
			ReportTaskAndStage (
				System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
				Stage.Beginning );

			int count = 0;
			string csharp = "\\";
			try
			{
				StringHelper.StringFromCSharpLiteral ( csharp );
			}
			catch ( ArgumentException )
			{
				count++;
			}

			csharp = "\\q";
			try
			{
				StringHelper.StringFromCSharpLiteral ( csharp );
			}
			catch ( ArgumentException )
			{
				count++;
			}

			csharp = "\\x";
			try
			{
				StringHelper.StringFromCSharpLiteral ( csharp );
			}
			catch ( ArgumentException )
			{
				count++;
			}

			csharp = "\\u123";
			try
			{
				StringHelper.StringFromCSharpLiteral ( csharp );
			}
			catch ( ArgumentException )
			{
				count++;
			}
			Assert.AreEqual ( count , 4 );

			csharp = "\\U123345";
			try
			{
				StringHelper.StringFromCSharpLiteral ( csharp );
			}
			catch ( ArgumentException )
			{
				count++;
			}

			csharp = "\\U00012345";
			try
			{
				StringHelper.StringFromCSharpLiteral ( csharp );
			}
			catch ( ArgumentException )
			{
				count++;
			}

			csharp = "\\U00002345";
			try
			{
				StringHelper.StringFromCSharpLiteral ( csharp );
			}
			catch ( ArgumentException )
			{
				count++;
			}
			Assert.AreEqual ( count , 6 );

			ReportTaskAndStage (
				System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
				Stage.Ending );
		}	// public static void StringLiteralExceptionsAreOK

		[TestMethod]
		public static void CharLiteralsAreOK ( )
		{
			ReportTaskAndStage (
				System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
				Stage.Beginning );

			string csharp = "\\x020";
			char normal = '\x020';
			try
			{
				Assert.AreEqual ( StringHelper.CharFromCSharpLiteral ( csharp ) , normal );
			}
			catch ( ArgumentException exArg )
			{
				ReportArgException (
					csharp ,
					exArg );
			}

			csharp = @"\ ";
			normal = ' ';
			try
			{
				Assert.AreEqual ( StringHelper.CharFromCSharpLiteral ( csharp ) , normal );
			}
			catch ( ArgumentException exArg )
			{
				ReportArgException (
					csharp ,
					exArg );
			}

			ReportTaskAndStage (
				System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
				Stage.Ending );
		}	// public static void CharLiteralsAreOK

		[TestMethod]
		public static void CharLiteralExceptionsAreOK ( )
		{
			ReportTaskAndStage (
				System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
				Stage.Beginning );
			int count = 0;
			string csharp = "\\";

			try
			{
				StringHelper.CharFromCSharpLiteral ( csharp );
			}
			catch ( ArgumentException )
			{
				count++;
			}

			csharp = "\\q";
			try
			{
				StringHelper.CharFromCSharpLiteral ( csharp );
			}
			catch ( ArgumentException )
			{
				count++;
			}

			csharp = "\\x";
			try
			{
				StringHelper.CharFromCSharpLiteral ( csharp );
			}
			catch ( ArgumentException )
			{
				count++;
			}

			csharp = "\\x020ABC";
			try
			{
				StringHelper.CharFromCSharpLiteral ( csharp );
			}
			catch ( ArgumentException )
			{
				count++;
			}

			Assert.AreEqual ( count , 4 );

			ReportTaskAndStage (
				System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
				Stage.Ending );
		}	// public static void CharLiteralExceptionsAreOK

		[TestMethod]
		public static void VerbatimLiteralsAreOK ( )
		{
			ReportTaskAndStage (
				System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
				Stage.Beginning );

			string csharp = @"Hello, world";
			string normal = "Hello, world";
			Assert.AreEqual ( StringHelper.StringFromVerbatimLiteral ( csharp ) , normal );
			csharp = "\"\"Hello, world\"\"";
			normal = @"""Hello, world""";
			Assert.AreEqual ( StringHelper.StringFromVerbatimLiteral ( csharp ) , normal );

			ReportTaskAndStage (
				System.Reflection.MethodBase.GetCurrentMethod ( ).Name ,
				Stage.Ending );
		}	// public static void VerbatimLiteralsAreOK

		private static void ReportArgException (
			string pstrCsharp , 
			ArgumentException pexArg )
		{
			Console.WriteLine (
				"An {0} occurred in routine {1},{5}    while processing string: {4}{2}{4}{5}    Message = {3}" ,
				new string [ ]
					{
						pexArg.GetType ( ).Name ,									// Substitution token 0 = Exception type
						pexArg.TargetSite.Name ,									// Substitution Token 1 = Name of reporting routine
						pstrCsharp ,												// Substitution token 2 = String being processed
						pexArg.Message ,											// Substitution token 3 = Exception message
						DOUBLE_QUOTE.ToString ( ) ,									// Substitution token 4 = String quoting, my way
						Environment.NewLine											// Substitution token 5 = NewLine, my way
					} );
		}	// ReportArgExceiption

		public static void ReportTaskAndStage (
			string pstrTaskName ,
			Stage penmStage )
		{
			switch ( penmStage )
			{
				case Stage.Beginning:
					Console.WriteLine (
						"{0} Begin:{1}" ,
						pstrTaskName ,
						Environment.NewLine );
					break;
				case Stage.Ending:
					Console.WriteLine (
						"{1}{0} Done!{1}" ,
						pstrTaskName ,
						Environment.NewLine );
					break;
			}	// switch ( penmStage )
		}	// public static void ReportTaskAndStage
	}	// public static class StringHelperTest
}	// namespace CSharpParserTest