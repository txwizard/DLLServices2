using System;


namespace BigNumberExperiments
{
	class Program
	{
		static string s_strProgramName = System.IO.Path.GetFileNameWithoutExtension (
			System.Reflection.Assembly.GetExecutingAssembly ( ).Location );
		static DateTime s_dtmStartupTime = System.Diagnostics.Process.GetCurrentProcess ( ).StartTime.ToUniversalTime ( );

		const int SCREEN_MAX_HEIGHT = 900;
		const int SCREEN_MAX_WIDTH = 1600;

		const int POSITION_TAB_STOP = 0;
		const int POSITION_ROW = 1;
		const int POSITION_COLUMN = 2;

		//	TabStop					= 50  ,
		//	Row (Y coordinate)		= 486 ,
		//	Column (X coordinate)	= 130 ,
		//	Name					= cboEncodingEngine
		const int SAMPLE_1_TABSTOP = 50;
		const int SAMPLE_1_X_COORD = 486;
		const int SAMPLE_1_Y_COORD = 130;

		// TabStop					= 100 ,
		//	Row (Y coordinate)		= -2  , (adjusted to zero)
		//	Column (X coordinate)	= 265 ,
		//	Name					= PropertyStringDetails
		const int SAMPLE_2_TABSTOP = 100;
		const int SAMPLE_2_X_COORD = 0;
		const int SAMPLE_2_Y_COORD = 265;

		//	TabStop					= 110 ,
		//	Row (Y coordinate)		= 0   ,
		//	Column (X coordinate)	= 8   ,
		//	Name					= PropertyStringNames
		const int SAMPLE_3_TABSTOP = 110;
		const int SAMPLE_3_X_COORD = 0;
		const int SAMPLE_3_Y_COORD = 8;

		//	TabStop					= 10  ,
		//	Row (Y coordinate)		= 483 ,
		//	Column (X coordinate)	= 4   ,
		//	Name					= txtWorkingDirectoryName
		const int SAMPLE_4_TABSTOP = 10;
		const int SAMPLE_4_X_COORD = 483;
		const int SAMPLE_4_Y_COORD = 4;

		//	TabStop					= 30  ,
		//	Row (Y coordinate)		= 485 ,
		//	Column (X coordinate)	= 46  ,
		//	Name					= txtProfileName
		const int SAMPLE_5_TABSTOP = 30;
		const int SAMPLE_5_X_COORD = 485;
		const int SAMPLE_5_Y_COORD = 46;

		struct TabStopInfo
		{
			public int TabStop;
			public int XCoord;
			public int YCoord;

			public TabStopInfo(int pintTabStop,int pintXCoordinate,int pintYCoordinate)
			{
				this.TabStop=pintTabStop;
				this.XCoord=pintXCoordinate;
				this.YCoord=pintYCoordinate;
			}
		};	// TabStopInfo

		static readonly TabStopInfo [ ] s_atsiThisStop = new TabStopInfo [ ]
		{
			new TabStopInfo (
				SAMPLE_1_TABSTOP ,
				SAMPLE_1_X_COORD ,
				SAMPLE_1_Y_COORD ) ,
			new TabStopInfo (
				SAMPLE_2_TABSTOP ,
				SAMPLE_2_X_COORD ,
				SAMPLE_2_Y_COORD ) ,
			new TabStopInfo (
				SAMPLE_3_TABSTOP ,
				SAMPLE_3_X_COORD ,
				SAMPLE_3_Y_COORD ) ,
			new TabStopInfo (
				SAMPLE_4_TABSTOP ,
				SAMPLE_4_X_COORD ,
				SAMPLE_4_Y_COORD ) ,
			new TabStopInfo (
				SAMPLE_5_TABSTOP ,
				SAMPLE_5_X_COORD ,
				SAMPLE_5_Y_COORD )
		};	// s_atsiThisStop

		static void Main ( string [ ] args )
		{
			Console.WriteLine (
				"{0} BOJ{2}{1}{2}" ,
				s_strProgramName ,
				s_dtmStartupTime ,
				Environment.NewLine );

			//	----------------------------------------------------------------
			//	Compute the minimum screen width, in characters, required to
			//	represent each of the components. The highest tab index must be
			//	derived by scanning the list.
			//	----------------------------------------------------------------

			int intHighestTabIndex = GetHighestTabIndex ( s_atsiThisStop );
			int intRequiredCharsForMaxTabIndex = MaxWidth ( intHighestTabIndex );

			int intRequiredCharsForMaxHeight = MaxWidth ( SCREEN_MAX_HEIGHT );
			int intRequiredCharsForMaxWidth = MaxWidth ( SCREEN_MAX_WIDTH );

			//	----------------------------------------------------------------
			//	Display the highest tab stop, along with the screen width and
			//	height, and the lengths of all three.
			//	----------------------------------------------------------------

			Console.WriteLine ( "Highest Tab Index = {0,4}, characters required to represent tab index = {1}" , intHighestTabIndex , intRequiredCharsForMaxTabIndex );
			Console.WriteLine ( "Screen Height     = {0,4}, characters required to represent height    = {1}" , SCREEN_MAX_HEIGHT , intRequiredCharsForMaxHeight );
			Console.WriteLine ( "Screen Width      = {0,4}, characters required to represent width     = {1}" , SCREEN_MAX_WIDTH , intRequiredCharsForMaxWidth );
			Console.WriteLine ( );

			//	----------------------------------------------------------------
			//	Generate format items for the three components.
			//	----------------------------------------------------------------

			string strTabComponentFormatItem = MinimumFormatitem (
				POSITION_TAB_STOP ,
				intHighestTabIndex );
			string strRowComponentFormatitem = MinimumFormatitem (
				POSITION_ROW ,
				SCREEN_MAX_HEIGHT );
			string strColComponentFormatItem = MinimumFormatitem (
				POSITION_COLUMN ,
				SCREEN_MAX_WIDTH );

			//	----------------------------------------------------------------
			//	Display the format items generated for the three components.
			//	----------------------------------------------------------------

			Console.WriteLine ( "Format item for Tab Index    = {0}" , strTabComponentFormatItem );
			Console.WriteLine ( "Format item for Row Index    = {0}" , strRowComponentFormatitem );
			Console.WriteLine ( "Format item for Column Index = {0}" , strColComponentFormatItem );
			Console.WriteLine ( );

			//	----------------------------------------------------------------
			//	Generate multipliers for the three components.
			//	----------------------------------------------------------------

			int intColMultiplier = 1;
			int intRowMultiplier = ( int ) ( intColMultiplier * Math.Pow ( 10 , intRequiredCharsForMaxWidth ) );
			int intTabMultiplier = ( int ) ( intRowMultiplier * Math.Pow ( 10 , intRequiredCharsForMaxHeight ) );

			//	----------------------------------------------------------------
			//	Display the multipliers.
			//	----------------------------------------------------------------

			Console.WriteLine ( "Tab Index Multiplier      = {0,10}" , intTabMultiplier );
			Console.WriteLine ( "Control Row Multiplier    = {0,10}" , intRowMultiplier );
			Console.WriteLine ( "Control Column Multiplier = {0,10}" , intColMultiplier );

			//	----------------------------------------------------------------
			//	Construct a format control string from the above format items,
			//	and use it to list the contents of the TabStopInfo array,
			//	s_atsiThisStop.
			//	----------------------------------------------------------------

			Console.WriteLine (
				"{1}Count of items in TabStopInfo array, s_atsiThisStop = {0}{1}" ,
				s_atsiThisStop.Length ,
				Environment.NewLine );

			string strTabStopInfoMessage = string.Concat ( new string [ ]
			{
				"    TabIndex = " ,
				strTabComponentFormatItem ,
				", Control Row = " ,
				strRowComponentFormatitem ,
				", Control Column = " ,
				strColComponentFormatItem ,
				", Mathematical Composite value = {3,10}"
			} );

			foreach ( TabStopInfo tsiThisStop in s_atsiThisStop )
			{
				Console.WriteLine (
					strTabStopInfoMessage ,					// Dynamically generated format control string
					new object [ ]							// Array of format items, which will eventually exceed three
					{
						tsiThisStop.TabStop ,				// Format item 0 = Tab Index
						tsiThisStop.XCoord ,				// Format Item 1 = Row (Y coordinate) of upper left corner of control)
						tsiThisStop.YCoord ,				// Format Item 2 = Column (X coordinate) of upper left corner of control)
						ComputeCompositeRank (
							tsiThisStop.TabStop ,
							intTabMultiplier ,
							tsiThisStop.YCoord ,
							intRowMultiplier ,
							tsiThisStop.XCoord ,
							intColMultiplier )
					} );
			}	// foreach ( TabStopInfo tsiThisStop in s_atsiThisStop )

			Console.WriteLine (
				"{2}{0} EOJ{2}Running Time = {1}{2}" ,
				s_strProgramName ,
				new TimeSpan (
					System.DateTime.UtcNow.Ticks
					- s_dtmStartupTime.Ticks ) ,				Environment.NewLine );
			Environment.Exit ( 0 );
		}	// static void Main


		private static Int64 ComputeCompositeRank (
			int pintTabIndex ,
			int pintTabMultiplier ,
			int pintRowCoordinate ,
			int pintRowMultiplier ,
			int pintColumnCoordinate ,
			int pintColMultiplier )
		{
			return ( pintTabIndex * pintTabMultiplier )
				+ ( pintRowCoordinate * pintRowMultiplier )
				+ ( pintColumnCoordinate * pintColMultiplier );
		}	// static void Main


		/// <summary>
		/// Determine the maximum TabIndex in the collection.
		/// </summary>
		/// <param name="patsiThisStop">
		/// Pass in a reference to the static array of TabStopInfo
		/// structures.
		/// </param>
		/// <returns>
		/// The return value is the maximum TabIndex value found on any element
		/// of the TabStopInfo array.
		/// </returns>
		/// <remarks>
		/// Strictly speaking, the array argument is unnecessary, but I used it
		/// to simplify adapting this method for use in the real program.
		/// </remarks>
		private static int GetHighestTabIndex ( TabStopInfo [ ] patsiThisStop )
		{
			int rintHighestTabIndex = 0;

			foreach ( TabStopInfo tsi in patsiThisStop )
				if ( tsi.TabStop > rintHighestTabIndex )
					rintHighestTabIndex = tsi.TabStop;

			return rintHighestTabIndex;
		}	// GetHighestTabIndex

	
		/// <summary>
		/// Given an integer, assumed to be a maximum value, report the minimum
		/// width, in characters, required to represent it.
		/// </summary>
		/// <param name="pintMaxValue">
		/// Specify the largest number that you expect to feed to the proposed
		/// format item.
		/// </param>
		/// <returns>
		/// The return value is the minimum screen width, in characters, needed
		/// to represent an integer less than or equal to pintMaxValue.
		/// </returns>
		private static int MaxWidth ( int pintMaxValue )
		{
			return pintMaxValue.ToString ( ).Length;
		}	// MaxWidth


		/// <summary>
		/// Generate a format item of the minimum width required to represent a
		/// specified maximum value at a specified position in a format control
		/// string.
		/// </summary>
		/// <param name="pintIndex">
		/// Specify the index that corresponds to the position in the list of
		/// substitution values from which the value fed to this format item
		/// will be drawn.
		/// </param>
		/// <param name="pintMaxValue">
		/// Specify the maximum value that the format item is expected to
		/// be expected to render.
		/// </param>
		/// <returns>
		/// The return value is a composite format item, ready to be substituted
		/// into a format control string.
		/// </returns>
		/// <remarks>
		/// This method calls MaxWidth to generate the alignment component of
		/// the composite format control item.
		/// </remarks>
		private static string MinimumFormatitem (
			int pintIndex ,
			int pintMaxValue )
		{
			const char FORMAT_ITEM_OPEN_CHAR = '{';
			const char FORMAT_ITEM_ALIGNMENT_CHAR = ',';
			const char FORMAT_ITEM_CLOSE_CHAR = '}';

			return string.Concat (
				FORMAT_ITEM_OPEN_CHAR ,
				pintIndex ,
				FORMAT_ITEM_ALIGNMENT_CHAR ,
				MaxWidth ( pintMaxValue ) ,
				FORMAT_ITEM_CLOSE_CHAR );
		}	// MinimumFormatitem
	}	// class Program
}	// namespace BigNumberExperiments