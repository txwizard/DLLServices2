using System;
using System.Globalization;
using System.Text;

namespace CSharpParser.Utility
{
	// ==================================================================================
	/// <summary>
	/// This static class involves helper methods that use strings.
	/// </summary>
	// ==================================================================================
	public static class StringHelper
	{
		// --------------------------------------------------------------------------------
		/// <summary>
		/// Converts a C# literal string into a normal string.
		/// </summary>
		/// <param name="pstrsource">Source C# literal string.</param>
		/// <returns>
		/// Normal string representation.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// An ArgumentException is thrown if an escape sequence is malformed. Messages
		/// vary, reflecting the precise issue that caused the exception.
		/// </exception>
		// --------------------------------------------------------------------------------
		public static string StringFromCSharpLiteral ( string pstrsource )
		{
			StringBuilder sbWork = new StringBuilder ( pstrsource.Length );
			int intPos = 0;

			while ( intPos < pstrsource.Length )
			{
				char chrThisChar = pstrsource [ intPos ];

				if ( chrThisChar == '\\' )
				{
					// --- Handle escape sequences.
					intPos++;

					if ( intPos >= pstrsource.Length )
						throw new ArgumentException ( "Missing escape sequence" );

					switch ( pstrsource [ intPos ] )
					{
						// --- Simple character escapes
						case '\'':
							chrThisChar = '\'';
							break;
						case '\"':
							chrThisChar = '\"';
							break;
						case '\\':
							chrThisChar = '\\';
							break;
						case '0':
							chrThisChar = '\0';
							break;
						case 'a':
							chrThisChar = '\a';
							break;
						case 'b':
							chrThisChar = '\b';
							break;
						case 'f':
							chrThisChar = '\f';
							break;
						case 'n':
							chrThisChar = '\n';
							break;
						case 'r':
							chrThisChar = '\r';
							break;
						case 't':
							chrThisChar = '\t';
							break;
						case 'v':
							chrThisChar = '\v';
							break;
						case 'x':
							// --- Hexadecimal escape (1-4 digits)
							StringBuilder hexa = new StringBuilder ( 10 );
							intPos++;

							if ( intPos >= pstrsource.Length )
								throw new ArgumentException ( "Missing escape sequence" );

							chrThisChar = pstrsource [ intPos ];
							
							if ( Char.IsDigit ( chrThisChar )
								|| ( chrThisChar >= 'a' && chrThisChar <= 'f' )
								|| ( chrThisChar >= 'A' && chrThisChar <= 'F' ) )
							{
								hexa.Append ( chrThisChar );
								intPos++;

								if ( intPos < pstrsource.Length )
								{
									chrThisChar = pstrsource [ intPos ];

									if ( Char.IsDigit ( chrThisChar )
										|| ( chrThisChar >= 'a' && chrThisChar <= 'f' )
										|| ( chrThisChar >= 'A' && chrThisChar <= 'F' ) )
									{
										hexa.Append ( chrThisChar );
										intPos++;

										if ( intPos < pstrsource.Length )
										{
											chrThisChar = pstrsource [ intPos ];

											if ( Char.IsDigit ( chrThisChar )
												|| ( chrThisChar >= 'a' && chrThisChar <= 'f' )
												|| ( chrThisChar >= 'A' && chrThisChar <= 'F' ) )
											{
												hexa.Append ( chrThisChar );
												intPos++;

												if ( intPos < pstrsource.Length )
												{
													chrThisChar = pstrsource [ intPos ];

													if ( Char.IsDigit ( chrThisChar )
														|| ( chrThisChar >= 'a' && chrThisChar <= 'f' )
														|| ( chrThisChar >= 'A' && chrThisChar <= 'F' ) )
													{
														hexa.Append ( chrThisChar );
														intPos++;
													}	// if ( Char.IsDigit ( chrThisChar )
												}	// if ( intPos < source.Length ) || ( chrThisChar >= 'a' && chrThisChar <= 'f' ) || ( chrThisChar >= 'A' && chrThisChar <= 'F' ) )
											}	// if ( Char.IsDigit ( chrThisChar ) || ( chrThisChar >= 'a' && chrThisChar <= 'f' ) || ( chrThisChar >= 'A' && chrThisChar <= 'F' ) )
										}	// if ( intPos < source.Length )
									}	// if ( Char.IsDigit ( chrThisChar ) || ( chrThisChar >= 'a' && chrThisChar <= 'f' ) || ( chrThisChar >= 'A' && chrThisChar <= 'F' ) )
								}	// if ( intPos < source.Length )
							}	// if ( Char.IsDigit ( chrThisChar ) || ( chrThisChar >= 'a' && chrThisChar <= 'f' ) || ( chrThisChar >= 'A' && chrThisChar <= 'F' ) )

							chrThisChar = ( char ) Int32.Parse (
								hexa.ToString ( ) ,
								NumberStyles.HexNumber );
							intPos--;
							break;	// case 'x':

						case 'u':
							// Unicode hexadecimal escape (exactly 4 digits)
							intPos++;

							if ( intPos + 3 >= pstrsource.Length )
								throw new ArgumentException ( "Unrecognized escape sequence" );

							try
							{
								uint charValue = UInt32.Parse (
									pstrsource.Substring ( intPos , 4 ) ,
								    NumberStyles.HexNumber );
								chrThisChar = ( char ) charValue;
								intPos += 3;
							}
							catch ( SystemException )
							{
								throw new ArgumentException ( "Unrecognized escape sequence" );
							}
							break;	// case 'u':

						case 'U':
							// Unicode hexadecimal escape (exactly 8 digits, first four must be 0000)
							intPos++;

							if ( intPos + 7 >= pstrsource.Length )
								throw new ArgumentException ( "Unrecognized escape sequence" );

							try
							{
								uint charValue = UInt32.Parse ( pstrsource.Substring ( intPos , 8 ) ,
								  NumberStyles.HexNumber );

								if ( charValue > 0xffff )
									throw new ArgumentException ( "Unrecognized escape sequence" );

								chrThisChar = ( char ) charValue;
								intPos += 7;
							}
							catch ( SystemException )
							{
								throw new ArgumentException ( "Unrecognized escape sequence" );
							}
							break;	// case 'U':

						default:
							throw new ArgumentException ( "Unrecognized escape sequence" );
					}	// switch ( pstrSource [ intPos ] )
				}	// if ( c == '\\' )

				intPos++;
				sbWork.Append ( chrThisChar );
			}	// while ( intPos < pstrSource.Length )

			return sbWork.ToString ( );
		}	// public static string StringFromCSharpLiteral


		// --------------------------------------------------------------------------------
		/// <summary>
		/// Converts a C# verbatim literal string into a normal string.
		/// </summary>
		/// <param name="pstrSource">Source C# literal string.</param>
		/// <returns>
		/// Normal string representation.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// An ArgumentException is thrown if an escape sequence is malformed. Messages
		/// vary, reflecting the precise issue that caused the exception.
		/// </exception>
		// --------------------------------------------------------------------------------
		public static string StringFromVerbatimLiteral ( string pstrSource )
		{
			StringBuilder sbWork = new StringBuilder ( pstrSource.Length );
			int intPos = 0;

			while ( intPos < pstrSource.Length )
			{
				char chrThisChar = pstrSource [ intPos ];

				if ( chrThisChar == '\"' )
				{
					// --- Handle escape sequences.
					intPos++;

					if ( intPos >= pstrSource.Length )
						throw new ArgumentException ( "Missing escape sequence" );

					if ( pstrSource [ intPos ] == '\"' )
						chrThisChar = '\"';
					else
						throw new ArgumentException ( "Unrecognized escape sequence" );
				}	// if ( chrThisChar == '\"' )

				intPos++;
				sbWork.Append ( chrThisChar );
			}	// while ( intPos < pstrSource.Length )

			return sbWork.ToString ( );
		}	// public static string StringFromVerbatimLiteral


		// --------------------------------------------------------------------------------
		/// <summary>
		/// Converts a C# literal string into a normal character..
		/// </summary>
		/// <param name="pstrSource">Source C# literal string.</param>
		/// <returns>
		/// Normal char representation.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// An ArgumentException is thrown if an escape sequence is malformed. Messages
		/// vary, reflecting the precise issue that caused the exception.
		/// </exception>
		// --------------------------------------------------------------------------------
		public static char CharFromCSharpLiteral ( string pstrSource )
		{
			string result = StringFromCSharpLiteral ( pstrSource );

			if ( result.Length != 1 )
				throw new ArgumentException ( "Invalid char literal" );

			return result [ 0 ];
		}	// public static char CharFromCSharpLiteral
	}	// public static class StringHelper
}	// CSharpParser.Utility