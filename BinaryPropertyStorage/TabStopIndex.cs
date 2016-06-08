using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace BinaryPropertyStorage
{
	/// <summary>
	/// The constructor of this class creates a sorted index of 
	/// ControlIndexItem items containing the name and tab index of all controls
	/// that have their TabStop property set to True.
	/// </summary>
	public class TabStopIndex
	{
		#region TabStopIndex Constructors
		/// <summary>
		/// Hide the default constructor to force all instances to be created as
		/// fully initialized instances.
		/// </summary>
		private TabStopIndex ( )
		{
		}	// private TabStopIndex constructor (1 of 3)


		/// <summary>
		/// The default public constructor initializes itself from all controls
		/// of the input Form that are tab stops.
		/// </summary>
		/// <param name="pfrmInThisForm">
		/// Specify the form from which to populate the instance.
		/// </param>
		/// <param name="ptswDedicatedTimer">
		/// Since the form may contain other timers, as does the form for which
		/// this object was created, this constructor needs a reference to its
		/// dedicated timer.
		/// </param>
		public TabStopIndex (
			Form pfrmInThisForm ,
			Timer ptswDedicatedTimer )
		{
			InitializeInstance (
				pfrmInThisForm ,
				ptswDedicatedTimer );
		}	// public TabStopIndex constructor (2 of 3)


		/// <summary>
		/// The default public constructor initializes itself from all controls
		/// of the input Form that are tab stops.
		/// </summary>
		/// <param name="pfrmInThisForm">
		/// Specify the form from which to populate the instance.
		/// <param name="ptswDedicatedTimer">
		/// Since the form may contain other timers, as does the form for which
		/// this object was created, this constructor needs a reference to its
		/// dedicated timer.
		/// </param>
		/// </param>
		/// <param name="patypExclude">
		/// Specify a list of types to exclude (e. g., Button controls).
		/// </param>
		public TabStopIndex (
			Form pfrmInThisForm ,
			Timer ptswDedicatedTimer ,
			Type [ ] patypExclude )
		{
			LoadExcludedTypeNames ( patypExclude );
			InitializeInstance (
				pfrmInThisForm ,
				ptswDedicatedTimer );
		}	// public TabStopIndex constructor (3 of 3)
		#endregion	// TabStopIndex Constructors


		#region TabStopIndex Properties
		/// <summary>
		/// Get the internal names, represented by their Name properties, of the
		/// controls to exclude from the list of allowed tab stops.
		/// </summary>
		public List<string> ExcludedTypesByName
		{
			get
			{
				return _lstExcludedTypeNames;
			}	// ExcludedTypesByName property getter
		}	// Read-only ExcludedTypesByName property


		/// <summary>
		/// Get the TabIndex of the control that just lost focus.
		/// </summary>
		/// <remarks>
		/// The onLeave event delegate of every indexed control must set this
		/// property. However, since a timer event must call a companion method,
		/// ChangeFocusIfExcluded, consumers should never need to test its
		/// value. Moreover, utility method, AlertNextControl, is available for
		/// use by the onLeave event delegate, which performs the remainder of
		/// the setup, initializes this property, through this routine, as one
		/// of its tasks.
		/// 
		/// Of greater significance to consumers of this class is the onTick
		/// event delegate, ChangeFocusIfExcluded_Tick, provided to simplify the
		/// programming required to set up the task. Since this timer must use a
		/// very short interval, and the timer turns itself off when its work is
		/// done, this task warrants a dedicated Timer control.
		/// </remarks>
		public int TabStopJustLeft
		{
			get
			{
				return _intTabStopJustLeft;
			}	// TabStopJustLeft property getter

			set
			{
				if ( value > Util.INVALID_ORDINAL)
				{
					_intTabStopJustLeft = value;
				}	// TRUE (anticipated outcome) block, if ( value > Util.INVALID_ORDINAL)
				else
				{	// Use of English words, "property name," ParamName deserves its own string resource.
					throw new ArgumentOutOfRangeException (
						Properties.Resources.ERRMSG_TABSTOPLEFT_VALUE_NAME ,
						value ,
						Properties.Resources.ERRMSG_TABSTOPLEFT_VALUE_DETAIL );
				}	// FALSE (UNanticipated outcome) block, if ( value > Util.INVALID_ORDINAL)
			}	// TabStopJustLeft property setter
		}	// TabStopJustLeft property
		#endregion	// TabStopIndex Properties


		#region TabStopIndex Public Methods
		/// <summary>
		/// The last task performed by the onLeave event of any control that has
		/// a valid (nonzero) TabIndex property is a call to this method, which
		/// paves the way for the Tick event of a dedicated Timer to skip over
		/// excluded controls.
		/// </summary>
		/// <param name="pctrlIsLosingFocus">
		/// Since the sender argument of an onLeave event is a reference to the
		/// control that is losing focus, it is cast to Control, and passed, so
		/// that this method can query its TabIndex and Location properties.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// In the unlikely event that the control fed into the method has an
		/// invalid TabIndex value (a value less than 1), this method throws an
		/// ArgumentOutOfRangeException exception, killing the method before it
		/// does any serious work.
		/// </exception>
		internal void AlertTheNextControl ( Control pctrlIsLosingFocus )
		{	// First things first, the TabIndex is saved for reference.
			const int MINIMUM_TIMER_INTERVAL = 1;

			this.TabStopJustLeft = pctrlIsLosingFocus.TabIndex;

			//	----------------------------------------------------------------
			//	The single input to BinarySearch must be an instance of the type
			//	of object of which it is composed; its properties are queried to
			//	construct the actual search term. Since the ControlIndexItem
			//	constructor takes a reference to the Control from which to set
			//	properties of the ControlIndexItem object, and this method just
			//	happens to have access to such a control, setting up the binary
			//	search is straightforward, if not entirely obvious at first. The
			//	binary search returns the index where the match was found, which
			//	is evaluated against the minimum value of a valid array index.
			//	Hence, the assignment statement is wrapped in parentheses, which
			//	makes its value equal to _intIndexOfLastControlExited which must
			//	be greater than Util.ARRAY_ELEMENT_INVALID (-1). If not, the
			//	method fails without prejudice.
			//	----------------------------------------------------------------

			if ( ( _intIndexOfLastControlExited = _lstControlsInTabOrder.BinarySearch ( new ControlIndexItem ( pctrlIsLosingFocus ) ) ) > Util.ARRAY_ELEMENT_INVALID )
			{
				//	------------------------------------------------------------
				//	Since its essential properties are in the ControlIndexItem 
				//	to which _intIndexOfLastControlExited points and the 
				//	instances on either side of it, this object has all the
				//	information it needs to figure out what to do when the Tick
				//	event on its dedicated timer fires. All that remains is
				//	enforce a minimum interval of one second, and start it.
				//	------------------------------------------------------------

				_tswDedicatedTimer.Interval = MINIMUM_TIMER_INTERVAL;
				_tswDedicatedTimer.Start ( );
			}	// if ( ( _intIndexOfLastControlExited = _lstControlsInTabOrder.BinarySearch ( new ControlIndexItem ( pctrlIsLosingFocus ) ) ) > Util.ARRAY_ELEMENT_INVALID )
		}	// AlertTheNextControl


		/// <summary>
		/// It's much simpler to delegate the work of the dedicated timer to the
		/// instance on behalf of which it was created on the parent form.
		/// </summary>
		/// <param name="ptswDedicatedTimer">
		/// Since the form may contain other timers, as does the form for which
		/// this object was created, this method needs a reference to its
		/// dedicated timer.
		/// </param>
		internal void ChangeFocusIfExcluded_Tick ( )
		{
			_tswDedicatedTimer.Stop ( );
			ChangeFocusIfExcluded ( );
		}	// ChangeFocusIfExcluded_Tick


		/// <summary>
		/// Rather than put all the work into ChangeFocusIfExcluded_Tick, it got
		/// its own protected method, giving inheritors the option of overriding
		/// it without needing to bother with the timer, which is hidden from view.
		/// </summary>
		protected void ChangeFocusIfExcluded ( )
		{
			const int ADJACENT_ITEM_IN_LIST = 1;

			ControlIndexItem cxiOverrideStop = null;

			int intTabStopOfActiveCtrl = _frmInThisForm.ActiveControl.TabIndex;

			if ( intTabStopOfActiveCtrl > _intTabStopJustLeft )
			{
				cxiOverrideStop = _lstControlsInTabOrder [ _intIndexOfLastControlExited + ADJACENT_ITEM_IN_LIST ];
			}	// if ( intTabStopOfActiveCtrl > _intTabStopJustLeft )
			else if ( intTabStopOfActiveCtrl < _intTabStopJustLeft )
			{
				cxiOverrideStop = _lstControlsInTabOrder [ _intIndexOfLastControlExited - ADJACENT_ITEM_IN_LIST ];
			}	// else if ( intTabStopOfActiveCtrl < _intTabStopJustLeft )
			else
			{
				ControlIndexItem cxiActive = new ControlIndexItem ( _frmInThisForm.ActiveControl );
				Int64 intActiveControlRank = cxiActive.UniqueKey;
				cxiActive = null;	// Discard this ControlIndexItem instance, and let the garbage collector reclaim its memory. 
				Int64 intLastControlRank = _lstControlsInTabOrder [ _intIndexOfLastControlExited ].UniqueKey;

				if ( intActiveControlRank > intLastControlRank )
				{
					cxiOverrideStop = _lstControlsInTabOrder [ _intIndexOfLastControlExited + ADJACENT_ITEM_IN_LIST ];
				}	// if ( intActiveControlRank > _lstControlsInTabOrder [ intLastControlRank )
				else if ( intActiveControlRank > intLastControlRank )
				{
					cxiOverrideStop = _lstControlsInTabOrder [ _intIndexOfLastControlExited - ADJACENT_ITEM_IN_LIST ];
				}	// else if ( intActiveControlRank > _lstControlsInTabOrder [ intLastControlRank )
				else
				{
					// Leave cxiOverrideStop null, so that the next step does nothing.
				}	// else neither ( intActiveControlRank > _lstControlsInTabOrder [ intLastControlRank ), nor ( intActiveControlRank > _lstControlsInTabOrder [ intLastControlRank )
			}	// else neither ( intTabStopOfActiveCtrl > _intTabStopJustLeft ), nor intTabStopOfActiveCtrl < _intTabStopJustLeft

			//	----------------------------------------------------------------
			//	If the preceding block found a need to change the focus, it sets
			//	cxiOverrideStop to the next control either just above or just
			//	below the item that just lost focus, indicating that the focus
			//	should shift to that control. Conversely, when cxiOverrideStop
			//	is NULL, the current control retains the focus, and the method
			//	returns.
			//	----------------------------------------------------------------

			if ( cxiOverrideStop != null )
			{
				_frmInThisForm.ActiveControl = _frmInThisForm.Controls [ cxiOverrideStop.Name ];
			}	// if ( cxiOverrideStop != null )
		}	// ChangeFocusIfExcluded

	
		public void LoadExcludedTypeNames ( Type [ ] patypExclude )
		{
			if ( _lstExcludedTypeNames == null )
			{
				if ( patypExclude == null )
				{
					return;							// There is nothing left to do.
				}	// TRUE (degenerate case) block, if ( patypExclude == null )
				else
				{
					_lstExcludedTypeNames = new List<string> ( patypExclude.Length );
				}	// FALSE (standard case) block, if ( patypExclude == null )
			}	// TRUE (initial invocation) block, if ( _lstExcludedTypeNames == null )
			else
			{
				_lstExcludedTypeNames.Clear ( );

				if ( patypExclude == null )
				{
					_lstExcludedTypeNames = null;	// Destroy the list altogether.
					return;							// There is nothing left to do.
				}	// TRUE (degenerate case) block, if ( patypExclude == null )
				else
				{
					_lstExcludedTypeNames.Capacity = patypExclude.Length;
				}	// FALSE (standard case) block, if ( patypExclude == null )				
			}	// FALSE (subsequent invocation) block, if ( _lstExcludedTypeNames == null )

			//	----------------------------------------------------------------
			//	If execution gets this far, patypExclude is not null, and list
			//	_lstExcludedTypeNames is initialized with sufficient capacity to
			//	hold an entry for each type in patypExclude.
			//	----------------------------------------------------------------

			foreach ( Type ptypThisType in patypExclude )
			{	// Store only the names; all the details just complicate searching.
				_lstExcludedTypeNames.Add ( ptypThisType.FullName );
			}	// foreach ( Type ptypThisType in patypExclude )

			_lstExcludedTypeNames.Sort ( );		// Pave the way for binary searches.
		}	// LoadExcludedTypeNames
		#endregion	// TabStopIndex Public Methods


		#region TabStopIndex Private Methods
		private void InitializeInstance (
			Form pfrmInThisForm ,
			Timer ptswDedicatedTimer )
		{
			//	----------------------------------------------------------------
			//	Make this happen ASAP, to ensure that the list is populated
			//	before the first reference to the UniqueKey property on an
			//	instance of ControlIndexItem.
			//	----------------------------------------------------------------

			ControlIndexItem.SetMaxTabIndex ( pfrmInThisForm );

			if ( _lstControlsInTabOrder == null )
			{	// Accept the default initial capacity, since there is no practical way to guess how many we need.
				_lstControlsInTabOrder = new List<ControlIndexItem> ( );
			}	// if ( _lstControlsInTabOrder == null )

			foreach ( Control ctrlThis in pfrmInThisForm.Controls )
			{
				if ( ctrlThis.TabStop )
				{	// Only controls that are tab stops count.
					if ( _lstExcludedTypeNames == null )
					{
						_lstControlsInTabOrder.Add ( new ControlIndexItem ( ctrlThis ) );
					}	// TRUE (Include all tab stops.) block, if ( _lstExcludedTypeNames == null )
					else
					{
						if ( _lstExcludedTypeNames.BinarySearch ( ctrlThis.GetType ( ).FullName ) < Util.ARRAY_FIRST_ELEMENT )
						{
							_lstControlsInTabOrder.Add ( new ControlIndexItem ( ctrlThis ) );
						}	// if ( _lstExcludedTypeNames.BinarySearch ( ctrlThis.GetType ( ).FullName ) < Util.ARRAY_FIRST_ELEMENT )
					}	// FALSE (Exclude some tab stops.) block, if ( _lstExcludedTypeNames == null )
				}	// if ( ctrlThis.TabStop )
			}	// foreach ( Control ctrlThis in pfrmInThisForm.Controls )

			if ( _lstControlsInTabOrder.Count > Util.ARRAY_SECOND_ELEMENT )
			{
				_lstControlsInTabOrder.Sort ( );
			}	// if ( _lstControlsInTabOrder.Count > Util.ARRAY_SECOND_ELEMENT )

			//	----------------------------------------------------------------
			//	Finally, save the references to the form on behalf of which this
			//	instance sprang into existence, and the timer that is dedicated
			//	for its use, so that public methods ChangeFocusIfExcluded and
			//	ChangeFocusIfExcluded_Tick can be called without arguments, and
			//	is guaranteed to interact with the intended form and timer.
			//	----------------------------------------------------------------

			_frmInThisForm = pfrmInThisForm;
			_tswDedicatedTimer = ptswDedicatedTimer;
		}	// InitializeInstance
		#endregion	// TabStopIndex Private Methods


		#region TabStopIndex Storage
		protected Form _frmInThisForm;
		private Timer _tswDedicatedTimer;
		private List<string> _lstExcludedTypeNames;
		protected List<ControlIndexItem> _lstControlsInTabOrder;
		protected int _intIndexOfLastControlExited = Util.ARRAY_ELEMENT_INVALID;
		protected int _intTabStopJustLeft = Util.INVALID_ORDINAL;
		#endregion	// TabStopIndex Storage


		/// <summary>
		/// One of these is created for each control in the input form whose
		/// TabStop property is set to True.
		/// </summary>
		public class ControlIndexItem : IComparable<ControlIndexItem>
		{
			#region ControlIndexItem Constructors
			/// <summary>
			/// Gather the dimensions of the largest screen, from which a single
			/// large number that represents the logical order of a tab stop can
			/// be derived.
			/// </summary>
			static ControlIndexItem ( )
			{
				foreach ( Screen scrThisScreen in Screen.AllScreens )
				{
					if ( scrThisScreen.Bounds.Height > s_intMaxScreenHeight )
					{
						s_intMaxScreenHeight = scrThisScreen.Bounds.Height;
					}	// if ( scrThisScreen.Bounds.Height > s_intMaxScreenHeight )

					if ( scrThisScreen.Bounds.Width > s_intMaxScreenWidth )
					{
						s_intMaxScreenWidth = scrThisScreen.Bounds.Width;
					}	// if ( scrThisScreen.Bounds.Width > s_intMaxScreenWidth )
				}	// foreach ( Screen scrThisScreen in Screen.AllScreens )
			}	// static ControlIndexItem constructor ( of 3)


			/// <summary>
			/// This constructor satisfies a
			/// </summary>
			private ControlIndexItem ( )
			{
			}	// private ControlIndexItem constructor (2 of 3)


			public ControlIndexItem ( Control pctrlIndexThis )
			{
				_strName = pctrlIndexThis.Name;
				_strControlTypeName = pctrlIndexThis.GetType ( ).FullName;
				_intColumn = pctrlIndexThis.Location.Y;
				_intRow = pctrlIndexThis.Location.X;
				_intTabOrder = pctrlIndexThis.TabIndex;
			}	// public ControlIndexItem constructor (3 of 3)
			#endregion	// ControlIndexItem Constructors


			#region ControlIndexItem Properties
			/// <summary>
			/// Gets the row (Y coordinate) of the upper left corner of the control.
			/// </summary>
			public int ControlColumn
			{
				get
				{
					return _intColumn;
				}	// ControlColumn property getter
			}	// Read-only ControlColumn property


			/// <summary>
			/// Gets the column (X coordinate) of the upper left corner of the control.
			/// </summary>
			public int ControlRow
			{
				get
				{
					return _intRow;
				}	// ControlRow property getter
			}	// Read-only ControlRow property


			/// <summary>
			/// Gets the Type of the ControlIndexItem.
			/// </summary>
			public string ControlTypeName
			{
				get
				{
					return _strControlTypeName;
				}	// ControlType property getter
			}	// Read-only ControlType property


			/// <summary>
			/// Gets the Name of the ControlIndexItem.
			/// </summary>
			public string Name
			{
				get
				{
					return _strName;
				}	// Name property getter
			}	// Read-only Name property


			/// <summary>
			/// Gets a reference to the array of rank part multipliers which are
			/// used with static method Util.DeriveUnikeKeyForSearching to
			/// create a search term that matches exactly one element in the
			/// sorted list of controls that are allowed as targets of the tab
			/// key.
			/// </summary>
			public static Int32 [ ] RankPartMultipliers
			{
				get
				{
					return s_aintKeyPartMultipliers;
				}	// Static RankPartMultipliers property getter
			}	// Read-only static RankPartMultipliers array property


			/// <summary>
			/// Gets the TabOrder of the ControlIndexItem.
			/// </summary>
			public int TabOrder
			{
				get
				{
					return _intTabOrder;
				}	// TabOrder property getter
			}	// Read-only TabOrder property


			/// <summary>
			/// Specify this property as the single search term of a
			/// BinarySearch against a collection of ControlIndexItem objects to
			/// ensure that the search finds exactly one match.
			/// </summary>
			/// <remarks>
			/// This property also drives the IComparable interface, a
			/// prerequisite for using BinarySearch, and greatly simplifies the
			/// implementations of the Equals and GetHashCode methods overrides.
			/// </remarks>
			public Int64 UniqueKey
			{
				get
				{
					return Util.DeriveUnikeKeyForSearching (
						new int [ ]	{										// Int32 [ ] paintInputValueSet							
							_intTabOrder ,										// Segment 1 of 3 = Tab Index
							_intRow ,											// Segment 2 of 3 = Row (Y Coordinate)
							_intColumn } ,										// Segment 3 of 3 = Column (X Coordinate)
						s_aintKeyPartMultipliers ,							// Int32 [ ] paintMultiplierSet
						Util.NegativeMaxValueTreatment.SubstituteZero );	// Substitute zero for any negative number in the paintInputValueSet array.
				}	// UniqueKey property getter				
			}	// Read-only UniqueKey property
			#endregion	// ControlIndexItem Properties


			#region ControlIndexItem Static Methods
			/// <summary>
			/// This method must be called before the first instance of
			/// ControlIndexItem is created.
			/// </summary>
			/// <param name="pfrmInThisForm">
			/// Pass the Form that was fed to the TabStopIndex constructor.
			/// </param>
			/// <remarks>
			/// This code cannot be implemented as a static constructor because
			/// the Form argument is required, and static constructors cannot
			/// take arguments.
			/// </remarks>
			public static void SetMaxTabIndex ( Form pfrmInThisForm )
			{
				const int KEYPART_TAB = 0;
				const int KEYPART_ROW = 1;
				const int KEYPART_COL = 2;
				const int KEYPARTS = 3;
				const int TABINDEX_IS_UNDEFINED = 0;

				s_aintKeyPartMultipliers = new int [ KEYPARTS ];

				s_aintKeyPartMultipliers [ KEYPART_ROW ] = Util.MaxWidth ( s_intMaxScreenHeight );
				s_aintKeyPartMultipliers [ KEYPART_COL ] = Util.MaxWidth ( s_intMaxScreenWidth );

				int intMaxTabIndex = TABINDEX_IS_UNDEFINED;

				foreach ( Control ctrl in pfrmInThisForm.Controls )
					if ( ctrl.TabIndex > intMaxTabIndex )
						intMaxTabIndex = ctrl.TabIndex;

				s_aintKeyPartMultipliers [ KEYPART_TAB ] = Util.MaxWidth ( intMaxTabIndex );

				//	------------------------------------------------------------
				//	IMPORTANT:	This method constructs a brand new array, fills
				//				it, and returns it to the caller. Since the
				//				return value is a reference to the new array, it
				//				replaces the original array s_aintKeyPartMaxima.
				//	------------------------------------------------------------

				s_aintKeyPartMultipliers = Util.ComputeMultiplersForSearching (
					s_aintKeyPartMultipliers ,
					Util.NegativeMaxValueTreatment.SubstituteZero );
			}	// SetMaxTabIndex
			#endregion	//  ControlIndexItem Static Methods


			#region ControlIndexItem Property Storage
			private static readonly int s_intMaxScreenHeight = Util.INVALID_ORDINAL;
			private static readonly int s_intMaxScreenWidth = Util.INVALID_ORDINAL;
			private static Int32 [ ] s_aintKeyPartMultipliers;

			private string _strName;
			private int _intColumn;
			private int _intRow;
			private int _intTabOrder;
			private string _strControlTypeName;
			#endregion	// ControlIndexItem Property Storage


			#region IComparable<ControlIndexItem> Members
			int IComparable<ControlIndexItem>.CompareTo ( ControlIndexItem pcixOther )
			{
				return this.UniqueKey.CompareTo ( pcixOther.UniqueKey );
			}	// CompareTo method
			#endregion	// IComparable<ControlIndexItem> Members


			#region Overridden Methods Inherited from Object
			/// <summary>
			/// Returns TRUE if the objects are equal with respect to all three
			/// of their integer properties.
			/// </summary>
			/// <param name="pobjOther">
			/// Object to compare for equality. If its type differs, this method
			/// returns FALSE, as would the base method that it overrides.
			/// </param>
			/// <returns>
			/// Returns TRUE if both objects are of the same type and all three
			/// of the two objects' integer properties are equal. Otherwise, the
			/// return value is FALSE.
			/// </returns>
			public override bool Equals ( object pobjOther )
			{
				if ( pobjOther.GetType ( ) == this.GetType ( ) )
				{
					ControlIndexItem cixOther = ( ControlIndexItem ) pobjOther;
					return this.UniqueKey.Equals ( cixOther.UniqueKey );
				}	// TRUE (Comparands are of like kind.) block, if ( obj.GetType ( ) == this.GetType ( ) )
				else
				{
					return false;
				}	// (Comparands are of UNlike kind.) block, if ( obj.GetType ( ) == this.GetType ( ) )
			}	// Equals method override


			/// <summary>
			/// Returns a string containing text representations of the four properties.
			/// </summary>
			/// <returns>
			/// The return value is a string that displays the property values
			/// in a neat summary, so that you can forgo expanding the
			/// properties in a watch window.
			/// </returns>
			public override string ToString ( )
			{
				const string TEMPLATE = @"TabStop = {0}, Row (Y coordinate) = {1}, Column (X coordinate) = {2}, Name = {3}";

				return string.Format (
					TEMPLATE ,					// Format control string
					new object [ ]				// Array of format items, cast to object
					{
						_intTabOrder ,			// Format Item 0 = TabStop
						_intRow ,				// Format Item 1 = Row
						_intColumn ,			// Format Item 2 = Column
						_strName				// Format Item 3 = Name
					} );
			}	// ToString method override

			public override int GetHashCode ( )
			{
				return this.UniqueKey.GetHashCode ( );
			}	// GetHashCode
			#endregion	// Overridden Methods Inherited from Object
		}	// Protected class ControlIndexItem
	}	// public class TabStopIndex
}	// partial namespace BinaryPropertyStorage