/*
    ============================================================================

    Namespace:			RedirectedStreamTester

    Class Name:			StreamStateInfo

	File Name:			StreamStateInfo.cs

    Synopsis:			Test native (to the BCL) methods of detecting a
                        redirected standard stream.

    Remarks:			I have a sneaking suspicion that the new "IsRedirected"
						properties addeded to the Console class in version 4 of
						the Base Class Library won't cut it.

    Author:				David A. Gray

	License:            Copyright (C) 2011-2015, David A. Gray. 
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
    2015/06/23 5.5     DAG    This class, and its project, make their first 
                              appearance.
    ============================================================================
*/

using System;

//	============================================================================
//	NOTICE: The root namespace is required, because the MagicNumbers and other
//			key classes are defined in the namespace.
//	============================================================================

using WizardWrx;

namespace RedirectedStreamTester
{
	internal enum StreamID
	{
		Unspecified ,
		STDERR ,
		STDIN ,
		STDOUT
	}	// internal enum StreamID

	internal enum StreamState
	{
		Unspecified ,
		Default = 1 ,
		D = 1 ,
		Redirected = 2 ,
		R = 2
	}	// internal enum StreamState

	internal class StreamStateInfo
	{
		private StreamID _enmStreamID		= StreamID.Unspecified;
		private StreamState _enmStreamState = StreamState.Unspecified;
		private string _strStreamFile		= null;

		public StreamID ID
		{
			get
			{
				return _enmStreamID;
			}	// public StreamID ID Property Get Nethod
		}	// public StreamID ID Property

		public StreamState State
		{
			get
			{
				return _enmStreamState;
			}	// public StreamState State Property Get Nethod
		}	// public StreamState State Property

		public string TargetFileName
		{
			get
			{
				return _strStreamFile;
			}		// public string TargetFileName Property Get Method
		}	// public string TargetFileName Property

		private StreamStateInfo ( )
		{
		}	// private StreamState (Constructor 1 of 2)

		public StreamStateInfo (
			string pstrStreamID ,
			string pstrStreamState )
		{
			const string ARGNAME_STREAMID = @"pstrStreamID";
			const string ARGNAME_STREAMSTATE = @"pstrStreamState";

			const int STREAMSTATE_MAX_SUBSTRINGS = MagicNumbers.PLUS_TWO;
			const int STREAMSTATE_FIXED_VALUE = ArrayInfo.ARRAY_FIRST_ELEMENT;
			const int STREAMSTATE_VARIABLE_VALUE = ArrayInfo.ARRAY_SECOND_ELEMENT;

			string strCurrentArgName = null;

			try
			{
				strCurrentArgName = ARGNAME_STREAMID;
				_enmStreamID = ( StreamID ) Enum.Parse (
					typeof ( StreamID ) ,
					pstrStreamID ,
					MagicBooleans.ENUM_PARSE_CASE_INSENSITIVE );

				strCurrentArgName = ARGNAME_STREAMSTATE;

				if ( pstrStreamState.IndexOf ( SpecialCharacters.COMMA ) == WizardWrx.ListInfo.INDEXOF_NOT_FOUND )
				{	// The file name was omitted.
					_enmStreamState = ( StreamState ) Enum.Parse (
						typeof ( StreamState ) ,
						pstrStreamState ,
						MagicBooleans.ENUM_PARSE_CASE_INSENSITIVE );
				}	// TRUE (degenerate case) block, if ( pstrStreamState.IndexOf ( SpecialCharacters.COMMA ) == WizardWrx.ListInfo.INDEXOF_NOT_FOUND )
				else
				{	// The file name is included.
					string [ ] astrParts = pstrStreamState.Split ( new char [ ] { SpecialCharacters.COMMA } );

					if ( astrParts.Length == STREAMSTATE_MAX_SUBSTRINGS )
					{
						_enmStreamState = ( StreamState ) Enum.Parse (
							typeof ( StreamState ) ,
							astrParts [ STREAMSTATE_FIXED_VALUE ] ,
							MagicBooleans.ENUM_PARSE_CASE_INSENSITIVE );
						_strStreamFile = astrParts [ STREAMSTATE_VARIABLE_VALUE ];
					}	// TRUE (anticipated outcome) block, if ( astrParts.Length == STREAMSTATE_MAX_SUBSTRINGS )
					else
					{	// The format of the input string is invalid.
						throw new ArgumentException (
							string.Format (
								Properties.Resources.ERRMSG_STREAMSTATE_FORMAT ,	// Format string
								pstrStreamID ,										// Format Item 0 = Stream ID
								pstrStreamState ,									// Format Item 1 = Stream State
								Environment.NewLine ) );							// Format Item 2 = Newline, My Way
					}	// FALSE (UNanticipated outcome) block, if ( astrParts.Length == STREAMSTATE_MAX_SUBSTRINGS )
				}	// FALSE (anticipated outcome) block, if ( pstrStreamState.IndexOf ( SpecialCharacters.COMMA ) == WizardWrx.ListInfo.INDEXOF_NOT_FOUND )
			}	// Normal flow of the constructor ends here.
			catch ( ArgumentNullException errNullArg )
			{
				ReportConstructionAccident (
					errNullArg ,
					strCurrentArgName );
			}
			catch ( ArgumentException errBadArg )
			{
				ReportConstructionAccident (
					errBadArg ,
					strCurrentArgName );
			}
			catch ( OverflowException errOverflow )
			{
				ReportConstructionAccident (
					errOverflow ,
					strCurrentArgName );
			}
			catch ( Exception errAllOthers )
			{
				ReportConstructionAccident (
					errAllOthers ,
					strCurrentArgName );
			}
		}	// public StreamState (Constructor 2 of 2)

		public string DisplayState ( )
		{
			const string INCLUDE_FILENAME = @"{0} to {1}";

			if ( string.IsNullOrEmpty ( _strStreamFile ) )
				return _enmStreamState.ToString ( );
			else
				return string.Format (
					INCLUDE_FILENAME ,
					_enmStreamState ,
					_strStreamFile );
		}	// public string DisplayState

		private void ReportConstructionAccident (
			Exception errAnyKind ,
			string pstrCurrentArgName )
		{
			throw new Exception (
				string.Format (													// Message
					Properties.Resources.ERRMSG_STREAMSTATE_CTOR ,				// 1) Format String
					new object [ ]
					{
						errAnyKind.GetType ( ) ,								//		Format String 0 = Exception type
						errAnyKind.TargetSite.Name ,							//		Format String 1 = Method Name
						pstrCurrentArgName ,									//		Format String 2 = Argument Name
						errAnyKind.Message ,									//		Format String 3 = Message from exception
						Environment.NewLine										//		Format String 4 = Newline, my way
					} ) ,
				errAnyKind );													// 2) Inner exception
		}	// private void ReportConstructionAccident
	}	// internal class StreamStateInfo
}	// partial namespace RedirectedStreamTester