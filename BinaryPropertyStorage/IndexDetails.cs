using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace BinaryPropertyStorage
{
	internal class IndexDetails : IComparable<IndexDetails>
	{
		//	--------------------------------------------------------------------
		//	This is a first class object; it overrides all three of the methods
		//	of its base class, Object, that can be overridden.
		//	--------------------------------------------------------------------

		#region Public Reference Constants
		public const int BYTES_PER_GUID = 16;
		public const int BYTES_PER_INT32 = 4;
		public const int BYTES_PER_INDEX_ENTRY = 24;
		#endregion // Public Reference Constants

		#region Main object implementation
		const char DOUBLE_QUOTE = '"';
		const int EMPTY_PROPERTY_LIST = 0;
		const int FIRST_CELL = 0;
		const string LEFT_BRACE = "{";
		const string RIGHT_BRACE = "}";
		const string SUBSEQUENT_PROPERTIES = "\t{0}";
		const char TAB_CHARACTER = '\t';

		internal IndexDetails ( )
		{
		}	// internal IndexDetail constructor (1 of 2)

		internal IndexDetails (
			Int32 pintOrder ,
			Int32 pintOffset ,
			DataGridViewRow pdgvThisRow )
		{
			StringBuilder sbProperties = new StringBuilder ( 256 );
			Guid gidRowGUID = Guid.Empty;

			_intOrder = pintOrder;
			_intInitialOffset = pintOffset;

			for ( int intCellIndex = FIRST_CELL ; intCellIndex < pdgvThisRow.Cells.Count ; intCellIndex++ )
			{
				string strCellValue = pdgvThisRow.Cells [ intCellIndex ].Value == null
					? string.Empty
					: pdgvThisRow.Cells [ intCellIndex ].Value.ToString ( );

				if ( intCellIndex == FIRST_CELL )
				{	// The first cell is special.
					if ( strCellValue.Length == EMPTY_PROPERTY_LIST )
					{
						continue;	// Advance to the next row.
					}	// if ( strCellValue.Length == EMPTY_PROPERTY_LIST )
					else if ( strCellValue.StartsWith ( LEFT_BRACE ) && strCellValue.EndsWith ( RIGHT_BRACE ) )
					{
						try
						{
							_abytRowGUIDAsBytes = Util.GUIDBytesFromGuidString ( strCellValue );
						}
						catch ( Exception exAll )
						{
							throw new Exception (
								string.Format (
									Properties.Resources.FRMLBL_PRF_ERRMSG_MUST_BE_GUID_STRING ,
									strCellValue ,
									DOUBLE_QUOTE ) ,
								exAll );
						}
					}	// TRUE (anticipated outcome) block, if ( strCellValue.StartsWith ( LEFT_BRACE ) && strCellValue.EndsWith ( RIGHT_BRACE ) )
					else
					{
						throw new Exception (
							string.Format (
								Properties.Resources.FRMLBL_PRF_ERRMSG_MUST_BE_GUID_STRING ,
								strCellValue ,
								DOUBLE_QUOTE ) );
					}	// FALSE (UNanticipated outcome) block, if ( strCellValue.StartsWith ( LEFT_BRACE ) && strCellValue.EndsWith ( RIGHT_BRACE ) )
				}	// TRUE block, if ( intCellIndex == FIRST_CELL )
				else
				{	// All other cells get identical treatment that differs significantly from that accorded to the first cell.
					if ( strCellValue.Length > EMPTY_PROPERTY_LIST )
					{
						if ( sbProperties.Length > EMPTY_PROPERTY_LIST )
						{	// Subsequent properties are preceded by a TAB character.
							sbProperties.AppendFormat (
								SUBSEQUENT_PROPERTIES ,
								strCellValue );
						}	// TRUE (subsequent properties) block, if ( sbProperties.Length > EMPTY_PROPERTY_LIST )
						else
						{	// The first property is inserted as is
							sbProperties.Append ( strCellValue );
						}	// FALSE (first property) block, if ( sbProperties.Length > EMPTY_PROPERTY_LIST )
					}	// if ( strCellValue.Length > EMPTY_PROPERTY_LIST )
				}	// FALSE block, if ( intCellIndex == FIRST_CELL )
			}	// for ( int intCellIndex = 0 ; intCellIndex < pdgvThisRow.Cells.Count ; intCellIndex++ )

			if ( sbProperties.Length > EMPTY_PROPERTY_LIST )
			{
				_strAllPropertyValues = sbProperties.ToString ( );
				Encoding encEngine = ( Encoding ) Activator.CreateInstance ( s_enmSelectedEncodingEngine.EncodingEngine );
				_abytStringPropertyBytes = encEngine.GetBytes ( StringHelper.StringFromCSharpLiteral ( _strAllPropertyValues ) );
			}	// TRUE (anticipated outcome) block, if ( sbProperties.Length > EMPTY_PROPERTY_LIST )
			else
			{
				throw new Exception (
					string.Format (
						Properties.Resources.FRMLBL_PRF_ERRMSG_NO_PROPERTIES ,
						pintOrder ,
						gidRowGUID.ToString ( ) ) );
			}	// FALSE (UNanticipated outcome) block, if ( sbProperties.Length > EMPTY_PROPERTY_LIST )
		}	// internal IndexDetail constructor (2 of 2)

		internal byte [ ] CreateEntryForIndex ( )
		{
			const int OFFSET_START = Util.ARRAY_FIRST_ELEMENT;
			const int OFFSET_POINTER = OFFSET_START;
			const int OFFSET_LENGTH = OFFSET_POINTER + BYTES_PER_INT32;

			byte [ ] abytOffset = BitConverter.GetBytes ( _intInitialOffset );
			byte [ ] abyDataLength = BitConverter.GetBytes ( _abytStringPropertyBytes.Length );

			byte [ ] rabyteIndexEntry = new byte [ BYTES_PER_INDEX_ENTRY ];

			_abytRowGUIDAsBytes.CopyTo (
				rabyteIndexEntry ,
				OFFSET_START );
			abytOffset.CopyTo (
				rabyteIndexEntry ,
				OFFSET_POINTER );
			abyDataLength.CopyTo (
				rabyteIndexEntry ,
				OFFSET_LENGTH );

			return rabyteIndexEntry;
		}	// internal byte [ ] CreateEntryForIndex

		internal static void AssignEncodingEngine ( SupportedEncoders penmSupportedEncoders )
		{
			s_enmSelectedEncodingEngine = penmSupportedEncoders;
		}	// internal static void AssignEncodingEngine

		internal string AllPropertyValues
		{
			get
			{
				return _strAllPropertyValues;
			}	// AllPropertyValues property getter
		}	// AllPropertyValues property

		internal Int32 InitialOffset
		{
			get
			{
				return _intInitialOffset;
			}	// InitialOffset property getter
		}	// InitialOffset property

		internal Int32 Order
		{
			get
			{
				return _intOrder;
			}	// Order property getter
		}	// Order property

		internal byte [ ] RowGUIDAsBytes
		{
			get
			{
				return _abytRowGUIDAsBytes;
			}	// RowGUIDAsBytes property getter
		}	// RowGUIDAsBytes property

		internal byte [ ] StringPropertyBytes
		{
			get
			{
				return _abytStringPropertyBytes;
			}	// BinaryProperties property getter
		}	// BinaryProperties property


		private Int32 _intOrder;
		private Int32 _intInitialOffset;
		private string _strAllPropertyValues;
		private byte [ ] _abytRowGUIDAsBytes;
		private byte [ ] _abytStringPropertyBytes;

		private static SupportedEncoders s_enmSelectedEncodingEngine;
		#endregion	// Main object implementation


		#region IComparable<IndexDetails> Members
		int IComparable<IndexDetails>.CompareTo ( IndexDetails other )
		{
			return _intOrder.CompareTo ( other._intOrder );
		}	// CompareTo
		#endregion	// IComparable<IndexDetails> Interface


		#region Overridden Methods
		public override string ToString ( )
		{
			return string.Concat ( 
				new Guid ( _abytRowGUIDAsBytes ) ,
				TAB_CHARACTER ,
				_strAllPropertyValues );
		}	// ToString

		public override bool Equals ( object obj )
		{
			if ( obj.GetType ( ) == typeof ( IndexDetails ) )
			{
				IndexDetails objCastToMyType = ( IndexDetails ) obj;
				return _abytRowGUIDAsBytes.Equals ( objCastToMyType._abytRowGUIDAsBytes );
			}	// TRUE (Both objects are of the same type.) block, if ( obj.GetType ( ) == typeof ( IndexDetails ) )
			else
			{
				return false;
			}	// FALSE (Type of the other object is different.) block, if ( obj.GetType ( ) == typeof ( IndexDetails ) )
		}	// Equals

		public override int GetHashCode ( )
		{
			return _abytRowGUIDAsBytes.GetHashCode ( );
		}	// GetHashCode
		#endregion	// Overridden Methods
	}	// internal class IndexDetails
}	// namespace BinaryPropertyStorage