using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;


namespace BinaryPropertyStorage
{
	internal class ProfileMetaData
	{
		#region Private Constants
		private const int VALID_HIDE_TOKEN_1_OF_2 = 0;
		private const int VALID_HIDE_TOKEN_2_OF_2 = 1;
		private const int VALID_HIDE_TOKEN_COUNT = 2;
		private const char EXTENSION_DELIMITER_CHAR = '.';
		#endregion	// Private Constants


		#region Constructors
		static ProfileMetaData ( )
		{
			s_astrHideSignalStrings = new string [ VALID_HIDE_TOKEN_COUNT ];

			s_astrHideSignalStrings [ VALID_HIDE_TOKEN_1_OF_2 ] = Properties.Resources.FRMLBL_PRF_HIDECOLUMTOKEN_1_OF_2;
			s_astrHideSignalStrings [ VALID_HIDE_TOKEN_2_OF_2 ] = Properties.Resources.FRMLBL_PRF_HIDECOLUMTOKEN_2_OF_2;

			s_ProfileBinaryPropertyMetaDataSuffix = Properties.Resources.FRMLBL_PRF_PROFILEFILENAMESUFFIX;
		}	// static ActiveProfile constructor, always hidden and implicitly private

		private ProfileMetaData ( )
		{
		}	// private ActiveProfile constructor (1 of 2), hidden

		internal ProfileMetaData (
			string pstrProfileName ,
			string pstrProfileDirectoryName ,
			string pstrProfileFileName )
		{
			_strProfileName = pstrProfileName;
			_strProfileDirectoryName = pstrProfileDirectoryName;
			_strProfileFileName = pstrProfileFileName;

			_fIsDirty = true;
			_fIsInitialized = false;
		}	// public ActiveProfile constructor (2 of 2), internal scope
		#endregion	// Constructors


		#region Properties
		internal string [ ] ColumnLabels
		{
			get
			{
				return _astrColumnLabels;
			}	// ColumnLabels property getter
			set
			{
				_astrColumnLabels = value;
				CheckState ( );
			}	// ColumnLabels property setter
		}	// ColumnLabels (string array) Property

		internal string ProfileDirectoryName
		{
			get
			{
				return _strProfileDirectoryName;
			}	// ProfileDirectoryName Property getter
		}	// ProfileDirectoryName Property

		internal string ProfileName
		{
			get
			{
				return _strProfileName;
			}	// ProfileName Property getter
		}	// ProfileName Property

		internal string ProfileFileName
		{
			get
			{
				return _strProfileFileName;
			}	// ProfileFileName Property Setter
		}	// ProfileFileName Property

		internal SupportedEncoders SelectedEncoder
		{
			get
			{
				return _enmSelectedEncoding;
			}	// SelectedEncoder property setter
			set
			{
				_enmSelectedEncoding = value;
				CheckState ( );
			}	// SelectedEncoder property setter
		}	// SelectedEncoder Property
		#endregion	// _enmSelectedEncoding


		#region Public Methods
		internal static bool HideThisLabel ( string pstrName )
		{
			foreach ( string strToken in s_astrHideSignalStrings )
				if ( strToken == pstrName )
					return true;

			return false;
		}	// HideThisLabel

		/// <summary>
		/// Private Form1 method SaveProfile calls SaveState after the binary
		/// file is created to save the other data that was stored in the form.
		/// </summary>
		/// <returns>
		/// This method is expected to return TRUE unless there is a mishap.
		/// </returns>
		/// <see cref="http://www.devx.com/tips/Tip/21168"/>
		internal bool SaveState ( )
		{
			if ( _fIsDirty )
			{
				if ( _fIsInitialized )
				{
					try
					{
						XmlDocument xmlMetaData = new XmlDocument ( );
						XmlDeclaration xmlDeclaration = xmlMetaData.CreateXmlDeclaration ( "1.0" , "UTF-8" , null );

						// Create the root element
						XmlElement xmlRootNode = xmlMetaData.CreateElement ( "BinaryPropertyStorage" );
						xmlMetaData.InsertBefore ( xmlDeclaration , xmlMetaData.DocumentElement );
						xmlMetaData.AppendChild ( xmlRootNode );

						XmlElement xmlProfileNameNode = xmlMetaData.CreateElement ( "ProfileName" );
						XmlText xmlProfileNameValue = xmlMetaData.CreateTextNode ( _strProfileName );
						xmlProfileNameNode.AppendChild ( xmlProfileNameValue );
						xmlRootNode.AppendChild ( xmlProfileNameNode );

						XmlElement xmlProfileDirectoryNameNode = xmlMetaData.CreateElement ( "ProfileDirectoryName" );
						XmlText xmlProfileDirectoryNameValue = xmlMetaData.CreateTextNode ( _strProfileDirectoryName );
						xmlProfileDirectoryNameNode.AppendChild ( xmlProfileDirectoryNameValue );
						xmlRootNode.AppendChild ( xmlProfileDirectoryNameNode );

						XmlElement xmlProfileFileNameNode = xmlMetaData.CreateElement ( "ProfileFileName" );
						XmlText xmlProfileFileNameValue = xmlMetaData.CreateTextNode ( _strProfileFileName );
						xmlProfileFileNameNode.AppendChild ( xmlProfileFileNameValue );
						xmlRootNode.AppendChild ( xmlProfileFileNameNode );

						XmlElement xmlSelectedEncodingNode = xmlMetaData.CreateElement ( "SelectedEncoding" );
						XmlText xmlSelectedEncodingValue = xmlMetaData.CreateTextNode ( _enmSelectedEncoding.DefinedEncoder.GetType ( ).Name );
						xmlSelectedEncodingNode.AppendChild ( xmlSelectedEncodingValue );
						xmlRootNode.AppendChild ( xmlSelectedEncodingNode );

						XmlElement xmlColumnLabelsNode = xmlMetaData.CreateElement ( "SelectedEncoding" );
						int intColumnNumber = Util.INVALID_ORDINAL;

						foreach ( string strPropertyName in _astrColumnLabels )
						{
							XmlElement xmlPropertyColumnNameNode = xmlMetaData.CreateElement ( string.Format ( "PropertyColumn{0}" , ++intColumnNumber ) );
							XmlText xmlPropertyColumnNameValue = xmlMetaData.CreateTextNode ( strPropertyName );
							xmlPropertyColumnNameNode.AppendChild ( xmlPropertyColumnNameValue );
							xmlColumnLabelsNode.AppendChild ( xmlPropertyColumnNameNode );
						}	// foreach ( string strPropertyName in _astrColumnLabels )

						xmlRootNode.AppendChild ( xmlColumnLabelsNode );

						xmlMetaData.Save ( System.IO.Path.Combine (
							_strProfileDirectoryName ,
							string.Concat (
								_strProfileName ,
								EXTENSION_DELIMITER_CHAR ,
								s_ProfileBinaryPropertyMetaDataSuffix ) ) );
						return true;
					}
					catch ( XmlException )
					{
						throw;
					}
				}	// TRUE (anticipated outcome) block, if ( _fIsInitialized )
				else
				{
					return false;
				}	// FALSE (UNanticipated outcome) block, if ( _fIsInitialized )
			}	// TRUE (anticipated outcome) block, if ( _fIsDirty )
			else
			{
				return true;
			}	// FALSE (UNanticipated outcome) block, if ( _fIsDirty )
		}	// SaveState
		#endregion	// Public Methods


		#region Private Methods
		private void CheckState ( )
		{
			_fIsInitialized = (  _enmSelectedEncoding != null 
				              && _astrColumnLabels != null
							  && _astrColumnLabels.Length > Util.ARRAY_FIRST_ELEMENT );
			_fIsDirty = true;
		}	// CheckState
		#endregion	// Private Methods


		#region Private Storage
		private static readonly string [ ] s_astrHideSignalStrings;
		private static readonly string s_ProfileBinaryPropertyMetaDataSuffix;

		private bool _fIsDirty;
		private bool _fIsInitialized;

		private string _strProfileName;
		private string _strProfileDirectoryName;
		private string _strProfileFileName;
		private SupportedEncoders _enmSelectedEncoding;
		private string [ ] _astrColumnLabels;
		#endregion	// Private Storage
	}	// internal class ActiveProfile
}	// partial namespace BinaryPropertyStorage