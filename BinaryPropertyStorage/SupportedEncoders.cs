using System;
using System.Text;

namespace BinaryPropertyStorage
{
	internal class SupportedEncoders : IComparable<SupportedEncoders>
	{
		#region Public Enumerations and Constants
		internal enum Encoder : byte
		{
			ASCII   = 1,
			Unicode = 2 ,
			UTF7    = 3 ,
			UTF8    = 4 ,
			UTF32   = 5
		}	// Encoder
		#endregion	// Public Enumerations and Constants


		#region Constructors
		internal SupportedEncoders ( )
		{
		}	// Default constructor (1 of 2)

		internal SupportedEncoders ( Encoder penmEncoder , int pintDisplayOrder )
		{
			switch ( penmEncoder )
			{
				case Encoder.ASCII:
					_strDisplayLabel = Properties.Resources.FRMLBL_ENC_CBO_ASCII;
					_typEncodingEngine = typeof ( ASCIIEncoding );
					break;
				case Encoder.Unicode:
					_strDisplayLabel = Properties.Resources.FRMLBL_ENC_CBO_UNICODE;
					_typEncodingEngine = typeof ( UnicodeEncoding );
					break;
				case Encoder.UTF32:
					_strDisplayLabel = Properties.Resources.FRMLBL_ENC_CBO_UTF_32;
					_typEncodingEngine = typeof ( UTF32Encoding );
					break;
				case Encoder.UTF7:
					_strDisplayLabel = Properties.Resources.FRMLBL_ENC_CBO_UTF_7;
					_typEncodingEngine = typeof ( UTF7Encoding );
					break;
				case Encoder.UTF8:
					_strDisplayLabel = Properties.Resources.FRMLBL_ENC_CBO_UTF_8;
					_typEncodingEngine = typeof ( UTF8Encoding );
					break;
				default:
					throw new System.ComponentModel.InvalidEnumArgumentException (
						"penmEncoder" ,
						( int ) penmEncoder ,
						typeof ( Encoder ) );
			}	// switch ( penmEncoder )

			_enmEncoder = penmEncoder;
			_intDisplayOrder = pintDisplayOrder;
		}	// Initializing constructor (2 of 2)
		#endregion	// Constructors


		#region Properties, all Read Only
		internal Encoder DefinedEncoder
		{
			get
			{
				return _enmEncoder;
			}	// DefinedEncoder propert6y getter
		}	// DefinedEncoder property

		internal string DisplayLabel
		{
			get
			{
				return _strDisplayLabel;
			}	// DisplayLabel property getter
		}	// DisplayLabel property

		internal int DisplayOrder
		{
			get
			{
				return _intDisplayOrder;
			}	// DisplayOrder property getter
		}	// DisplayOrder property

		internal Type EncodingEngine
		{
			get
			{
				return _typEncodingEngine;
			}	// EncodingEngine property getter
		}	// EncodingEngine property
		#endregion	// Properties, all Read Only


		#region Private Instance Storage
		private Encoder _enmEncoder;
		private string _strDisplayLabel;
		private int _intDisplayOrder;
		private Type _typEncodingEngine;
		#endregion	// Private Instance Storage


		#region IComparable<SupportedEncoders> Members
		int IComparable<SupportedEncoders>.CompareTo ( SupportedEncoders other )
		{
			return this._intDisplayOrder.CompareTo ( other._intDisplayOrder );
		}	// CompareTo
		#endregion	// IComparable<SupportedEncoders> Members


		#region Overridden ToString method
		public override string ToString ( )
		{
			return _strDisplayLabel;
		}	// ToString
		#endregion	// Overridden ToString method
	}	// internal class SupportedEncoders
}	// namespace BinaryPropertyStorage