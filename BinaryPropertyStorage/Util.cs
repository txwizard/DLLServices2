using System;

namespace BinaryPropertyStorage
{
	public static class Util
	{
		private const string ARGNAME_NEGATIVE_VALUE_TREATMENT = "penmNegativeMaxValueTreatment";
		/// <summary>
		/// Use these to set the penmPathStringSemantics argument of the SanitizePathString method.
		/// </summary>
		public enum PathStringSemantics
		{
			/// <summary>
			/// The specified string represents a file name.
			/// </summary>
			File ,

			/// <summary>
			/// The specified string represents a path name, which may conclude with a file name.
			/// </summary>
			Path
		};	// PathStringSemantics


		/// <summary>
		/// Specify the value of ComputeMultiplersForSearching argument
		/// penmNegativeMaxValueTreatment.
		/// </summary>
		public enum NegativeMaxValueTreatment
		{
			/// <summary>
			/// Replace negative integers with zero.
			/// </summary>
			SubstituteZero ,

			/// <summary>
			/// Replace negative integers with their absolute values.
			/// </summary>
			SubstituteAbsoluteValue
		};	// NegativeMaxValueTreatment


		public const int ARRAY_ELEMENT_INVALID = -1;
		public const int ARRAY_FIRST_ELEMENT = 0;
		public const int ARRAY_SECOND_ELEMENT = 1;
		public const int BUFFER_BEGINNING = ARRAY_FIRST_ELEMENT;
		public const int DECIMAL_POWER = 10;
		public const int EMPTY_STRING_LENGTH = 0;
		public const int FIRST_ORDINAL = 1;
		public const int INDEXOF_NOT_FOUND = -1;
		public const int INVALID_ORDINAL = 0;
		public const int LAST_INDEX_FROM_COUNT = 1;
		public const int ONE_ONLY = 1;
		public const int UNBALANCED_DERIVEUNIKEKEYFORSEARCHING_ARRAYS = -1;

		public const char LEFT_BRACE = '{';
		public const char RIGHT_BRACE = '}';


		/// <summary>
		/// Given a list of maximum expected 32 bit integer values, return an
		/// array of 32 bit integer multipliers for deriving a single 64 bit
		/// integer to represent one of each in the single sort key required to
		/// ensure that the BinarySearch method on a generic list always finds
		/// exactly one match.
		/// </summary>
		/// <param name="paintMaxValues">
		/// The specified array must list maximum values in their intended order
		/// of appearance in the key derived by feeding this array, along with a
		/// separate array of actual values, to a companion static method,
		/// DeriveUnikeKeyForSearching.
		/// 
		/// All values are expected to be positive integers.
		/// </param>
		/// <param name="penmNegativeMaxValueTreatment">
		/// Use a member of the NegativeMaxValueTreatment enumeration to specify
		/// how to treat negative integers in the pintMaxValues array.
		/// </param>
		/// <returns>
		/// Unless the input array is empty, the return value is an array of
		/// positive 32 bit integers. If the input array is either empty or a
		/// null reference, the returned array is empty.
		/// </returns>
		/// <exception cref="System.ComponentModel.InvalidEnumArgumentException">
		/// An System.ComponentModel.InvalidEnumArgumentException exception is
		/// thrown when the value of argument penmNegativeMaxValueTreatment is
		/// invalid.
		/// </exception>
		public static Int32 [ ] ComputeMultiplersForSearching (
			Int32 [ ] paintMaxValues ,
			NegativeMaxValueTreatment penmNegativeMaxValueTreatment )
		{
			const int MINIMUM_PERMITTED_MAX_VALUE = 0;
			const int LAST_INDEX_RELATIVE_TO_THIS_INDEX = 1;
			const Int32 MULTIPLIER_FOR_RIGHTMOST_ITEM = 1;
			const Int32 WIDTH_FOR_ZERO = 1;

			Int32 [ ] raInt32;

			if ( paintMaxValues == null || paintMaxValues.Length == ARRAY_FIRST_ELEMENT )
			{
				raInt32 = new Int32 [ 0 ];
				return raInt32;
			}	// TRUE (degenerate case) block, if ( paintMaxValues == null || paintMaxValues.Length == ARRAY_FIRST_ELEMENT )
			else
			{
				//	------------------------------------------------------------
				//	Integer variable intLastMultipler is initialized with the
				//	constant value of the multiplier for the far right column,
				//	which is multiplied by 1, which has no effect. Subsequent
				//	columns multiply the result for the column to their right by
				//	a new factor, which is the power of 10 needed to cover the
				//	biggest number that is expected to appear in the column to
				//	its right.
				//
				//	Negative values are always replaced with either zero, or the
				//	absolute value.
				//
				//	Multipliers are made explicitly 32 bit integers, so that the
				//	outcome can be reasonably expected to fit into a 64 bit
				//	integer, which the .NET Framework classifies as a Long.
				//	------------------------------------------------------------

				int intNItems = paintMaxValues.Length;							// Integer variable intNItems is used twice.
				int intLastItem = intNItems - LAST_INDEX_FROM_COUNT;			// Integer variable intLastItem is used once in an expression and again in a tes5t.
				Int32 intLastMultipler = MULTIPLIER_FOR_RIGHTMOST_ITEM;			// See comment above.

				raInt32 = new Int32 [ intNItems ];

				for ( int intThisPart = intLastItem ;
					      intThisPart >= ARRAY_FIRST_ELEMENT ;
						  intThisPart-- )
				{
					if ( intThisPart < intLastItem )
					{
						int intLastPart = intThisPart + LAST_INDEX_RELATIVE_TO_THIS_INDEX;

						if ( paintMaxValues [ intLastPart ] > MINIMUM_PERMITTED_MAX_VALUE )
						{	// I expect most values to be positive integers.
							intLastMultipler = intLastMultipler * ( ( int ) Math.Pow ( DECIMAL_POWER , paintMaxValues [ intLastPart ] ) );
						}	// TRUE (Positive integers are always used as is.) block, if ( paintMaxValues [ intLastPart ] > MINIMUM_PERMITTED_MAX_VALUE )
						else
						{
							switch ( penmNegativeMaxValueTreatment )
							{
								case NegativeMaxValueTreatment.SubstituteZero:
									intLastMultipler = intLastMultipler * ( ( int ) Math.Pow ( DECIMAL_POWER , WIDTH_FOR_ZERO ) );
									break;
								case NegativeMaxValueTreatment.SubstituteAbsoluteValue:
									intLastMultipler = intLastMultipler * ( ( int ) Math.Pow ( DECIMAL_POWER , Math.Abs ( paintMaxValues [ intLastPart ] ) ) );
									break;
								default:
									throw new System.ComponentModel.InvalidEnumArgumentException (
										ARGNAME_NEGATIVE_VALUE_TREATMENT ,
										( int ) penmNegativeMaxValueTreatment ,
										typeof ( NegativeMaxValueTreatment ) );
							}	// switch ( penmNegativeMaxValueTreatment )
						}	// FALSE (Negative integers require a bit more effort.) block, if ( paintMaxValues [ intLastPart ] > MINIMUM_PERMITTED_MAX_VALUE )						
					}	// if ( intThisPart < intLastItem )

					//	--------------------------------------------------------
					//	Multiplier insertion follows the IF block because the
					//	multiplier for the first (rightmost) integer, being a
					//	constant, is used to initialize intLastMultipler, which
					//	happens before execution enters this loop.
					//	--------------------------------------------------------

					raInt32 [ intThisPart ] = intLastMultipler;
				}	// for ( int intThisPart = intLastItem ; intThisPart >= ARRAY_FIRST_ELEMENT ; intThisPart-- )

				return raInt32;
			}	// FALSE (anticipated case) block, if ( paintMaxValues == null || paintMaxValues.Length == ARRAY_FIRST_ELEMENT )
		}	// ComputeMultiplersForSearching


		/// <summary>
		/// Use an array of values and the corresponding array of multipliers to
		/// derive a single 64 bit unsigned integer representation of the input
		/// values that can be used to ensure that a Binary Search returns 
		/// exactly one match.
		/// </summary>
		/// <param name="paintInputValueSet">
		/// </param>
		/// <param name="paintMultiplierSet">
		/// Specify the array returned by calling ComputeMultiplersForSearching
		/// once before you begin processing details.
		/// </param>
		/// <param name="penmNegativeMaxValueTreatment">
		/// Use a member of the NegativeMaxValueTreatment enumeration to specify
		/// how to treat negative integers in the pintMaxValues array.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is a single unsigned 64
		/// bit integer that represents the rank of the inputs in array
		/// paintInputValueSet.
		/// 
		/// However, if arrays paintInputValueSet and paintMultiplierSet contain
		/// unequal numbers of elements, the method signals the error by returning
		/// UNBALANCED_DERIVEUNIKEKEYFORSEARCHING_ARRAYS.
		/// </returns>
		public static Int64 DeriveUnikeKeyForSearching (
			Int32 [ ] paintInputValueSet ,
			Int32 [ ] paintMultiplierSet ,
			NegativeMaxValueTreatment penmNegativeMaxValueTreatment )
		{
			if ( paintInputValueSet.Length == paintMultiplierSet.Length )
			{
				Int64 rintUniqueBinarySearchKey = 0;

				int intArrayLengths = paintMultiplierSet.Length;

				for ( int intCurrValue = ARRAY_FIRST_ELEMENT ;
					      intCurrValue < intArrayLengths ;
						  intCurrValue++ )
				{
					if ( paintInputValueSet [ intCurrValue ] >= 0 )
					{
						rintUniqueBinarySearchKey += ( paintMultiplierSet [ intCurrValue ] * paintInputValueSet [ intCurrValue ] );
					}
					else
					{
						switch ( penmNegativeMaxValueTreatment )
						{
							case NegativeMaxValueTreatment.SubstituteZero:
								break;						// Since the product of anything and zero is always zero, this case has no effect on the outcome.
							case NegativeMaxValueTreatment.SubstituteAbsoluteValue:
								rintUniqueBinarySearchKey += ( paintMultiplierSet [ intCurrValue ] * Math.Abs ( paintInputValueSet [ intCurrValue ] ) );
								break;
							default:
								throw new System.ComponentModel.InvalidEnumArgumentException (
									ARGNAME_NEGATIVE_VALUE_TREATMENT ,
									( int ) penmNegativeMaxValueTreatment ,
									typeof ( NegativeMaxValueTreatment ) );
						}	// switch ( penmNegativeMaxValueTreatment )
					}
				}	// for ( int intCurrValue = ARRAY_FIRST_ELEMENT ; intCurrValue < intArrayLengths ; intCurrValue++ )

				return rintUniqueBinarySearchKey;
			}	// TRUE (anticipated outcome) block, if ( paintInputValueSet.Length == paintMultiplierSet.Length )
			else
			{
				return UNBALANCED_DERIVEUNIKEKEYFORSEARCHING_ARRAYS;
			}	// FALSE (UNanticipated outcome) block, if ( paintInputValueSet.Length == paintMultiplierSet.Length )
		}	// DeriveUnikeKeyForSearching


		/// <summary>
		/// Generate an array of 16 bytes from a GUID string.
		/// </summary>
		/// <param name="pstrGuidString">
		/// This string must be generated from a valid GUID, such as by calling
		/// the ToString method on a System.Guid structure.
		/// </param>
		/// <returns>
		/// The return value is an array of exactly 16 bytes that represents the
		/// most compact format in which a GUID may be stored.
		/// </returns>
		public static byte [ ] GUIDBytesFromGuidString ( string pstrGuidString )
		{
			Guid gidFromString = new Guid ( pstrGuidString );
			return gidFromString.ToByteArray ( );
		}	// GUIDBytesFromGuidString


		/// <summary>
		/// Given an integer, assumed to be a maximum value, report the minimum
		/// width, in characters, required to represent it.
		/// </summary>
		/// <param name="pnbrMaxValue">
		/// Specify the largest number that you expect to feed to the proposed
		/// format item.
		/// </param>
		/// <returns>
		/// The return value is the minimum screen width, in characters, needed
		/// to represent an integer less than or equal to pintMaxValue.
		/// </returns>
		public static int MaxWidth<T> ( T pnbrMaxValue )
		{
			return pnbrMaxValue.ToString ( ).Length;
		}	// MaxWidth



		/// <summary>
		/// Remove invalid characters from an input string.
		/// </summary>
		/// <param name="pstrPathString">
		/// Path string from which to remove invalid characters.
		/// </param>
		/// <param name="penmPathStringSemantics">
		/// Use a member of the PathStringSemantics enumeration to specify
		/// whether to treat string pstrPathString as a path or file name.
		/// </param>
		/// <returns>
		/// The return value is the cleaned path string.
		/// </returns>
		internal static string SanitizePathString (
			string pstrPathString ,
			PathStringSemantics penmPathStringSemantics )
		{
			if ( string.IsNullOrEmpty ( pstrPathString ) )
				return string.Empty;

			char [ ] achrInvalids;

			switch ( penmPathStringSemantics )
			{
				case PathStringSemantics.File:
					achrInvalids = System.IO.Path.GetInvalidFileNameChars ( );
					break;
				case PathStringSemantics.Path:
					achrInvalids = System.IO.Path.GetInvalidPathChars ( );
					break;
				default:
					throw new System.ComponentModel.InvalidEnumArgumentException (
						"penmPathStringSemantics" ,
						( int ) penmPathStringSemantics ,
						typeof ( PathStringSemantics ) );
			}	// switch ( penmPathStringSemantics )

			string rstrCleanPathString = pstrPathString;
			int intCharPos = INDEXOF_NOT_FOUND;

			foreach ( char chrIsInvalid in achrInvalids )
			{
				while ( ( intCharPos = rstrCleanPathString.IndexOf ( chrIsInvalid ) ) > INDEXOF_NOT_FOUND )
				{
					rstrCleanPathString = rstrCleanPathString.Remove (
						intCharPos ,
						ONE_ONLY );
				}	// while ( rstrCleanPathString.IndexOf ( chrIsInvalid ) > INDEXOF_NOT_FOUND )
			}	// foreach ( char chrIsInvalid in achrInvalids )

			return rstrCleanPathString;
		}	// SanitizePathString
	}	// public static class Util
}	// partial namespace BinaryPropertyStorage