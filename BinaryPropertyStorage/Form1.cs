using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace BinaryPropertyStorage
{
	public partial class Form1 : Form
	{
		#region Private Enumerations and Constants that need form scope
		const string TOKEN_PROFILE_DIRNAME = @"$$WorkingDirectoryName$$";
		const string TOKEN_PROFILE_NAME = @"$$ProfileName$$";

	
		enum PromptPriority
		{
			Normal ,
			Important ,
			Urgent ,
			Yay
		}	// PromptPriority
		#endregion	// Private Enumerations and Constants that need form scope


		#region Constructor
		public Form1 ( )
		{
			const int SKIP_FIRST_COLUMN = 1;

			InitializeComponent ( );

			//	----------------------------------------------------------------
			//	Initialize the index of labels, which enables users to assign
			//	their own labels to the column headers.
			//	----------------------------------------------------------------

			int intNSubstrings = this.PropertyStringDetails.ColumnCount;

			for ( int intCurrentColumn = SKIP_FIRST_COLUMN ; intCurrentColumn < intNSubstrings ; intCurrentColumn++ )
			{
				this.PropertyStringNames.Rows.Add ( new string [ ]
				{
					this.PropertyStringDetails.Columns [ intCurrentColumn ].HeaderText ,
					this.PropertyStringDetails.Columns [ intCurrentColumn ].Index.ToString ( ) ,
					string.Empty
				} );
			}	// for ( int intCurrentColumn = SKIP_FIRST_COLUMN ; intCurrentColumn < intNSubstrings ; intCurrentColumn++ )

			//	----------------------------------------------------------------
			//	Initialize the list box with the supported encoders.
			//	----------------------------------------------------------------

			cboEncodingEngine.BeginUpdate ( );
			s_lstSupportedEncoders.Sort ( );
			cboEncodingEngine.Items.AddRange ( s_lstSupportedEncoders.ToArray ( ) );
			cboEncodingEngine.SelectedIndex = 1;
			cboEncodingEngine.Update ( );

			InitializeFormText ( );

			InitializeProfileFileName ( );
		}	// public Form1 ( ) constructor


		private void InitializeFormText ( )
		{
			//	----------------------------------------------------------------
			//	Initialize all text on the form from string resources.
			//	----------------------------------------------------------------

			this.Text = Application.ProductName;

			this.ItemGuid.HeaderText = Properties.Resources.FRMLBL_PSD_ITEMGUID;

			this.PropertyString1.HeaderText = Properties.Resources.FRMLBL_PSD_PROPERTYSTRING1;
			this.PropertyString2.HeaderText = Properties.Resources.FRMLBL_PSD_PROPERTYSTRING2;
			this.PropertyString3.HeaderText = Properties.Resources.FRMLBL_PSD_PROPERTYSTRING3;
			this.PropertyString4.HeaderText = Properties.Resources.FRMLBL_PSD_PROPERTYSTRING4;
			this.PropertyString5.HeaderText = Properties.Resources.FRMLBL_PSD_PROPERTYSTRING5;
			this.PropertyString6.HeaderText = Properties.Resources.FRMLBL_PSD_PROPERTYSTRING6;
			this.PropertyString7.HeaderText = Properties.Resources.FRMLBL_PSD_PROPERTYSTRING7;
			this.PropertyString8.HeaderText = Properties.Resources.FRMLBL_PSD_PROPERTYSTRING8;

			this.PropertyStringLabel.HeaderText = Properties.Resources.FRMLBL_PSN_PROPERTYSTRINGLABEL;
			this.PropertyStringOrder.HeaderText = Properties.Resources.FRMLBL_PSN_PROPERTYSTRINGORDER;
			this.PropertyStringName.HeaderText = Properties.Resources.FRMLBL_PSN_PROPERTYSTRINGNAME;
			this.lblProfileName.Text = Properties.Resources.FRMLBL_PRF_LBLPROFILENAME;
			this.txtProfileName.Text = Properties.Resources.FRMLBL_PRF_TXTPROFILENAME;

			this.lblFileName.Text = Properties.Resources.FRMLBL_PRF_LBLFILENAME;
			this.cmdProfileFileBrowser.Text = Properties.Resources.FRMLBL_PRF_CMDPROFILEFILEBROWSER;
			this.cmdSaveProfile.Text = Properties.Resources.FRMLBL_PRF_CMDSAVEPROFILE;
			this.cmdNewProfile.Text = Properties.Resources.FRMLBL_PRF_CMDNEWPROFILE;
			this.lblWorkingDirectory.Text = Properties.Resources.FRMLBL_PRD_LBLWORKINGDIRECTORY;
			this.cmdBrowseForDirectory.Text = Properties.Resources.FRMLBL_PRD_CMDBROWSEFORDIRECTORY;

			this.lblFileGUID.Text = Properties.Resources.FRMLBL_TFG_TXTFILEGUID;
			this.txtFileGUID.Text = string.Concat (
				Util.LEFT_BRACE ,
				new System.Guid ( GetAssemblyGuidString ( ) ).ToString ( ) ,
				Util.RIGHT_BRACE );
			this.cmdNewFileGUID.Text = Properties.Resources.FRMLBL_TFG_CMDNEWFILEGUID;
			this.cmdNameYourOwnFileGUID.Text = Properties.Resources.FRMLBL_TFG_CMDNAMEYOUROWNFILEGUID;

			this.cmdApplyNewLabels.Text = Properties.Resources.FRMLBL_PSD_CMDAPPLYNEWLABELS;

			_tsi = new TabStopIndex ( 
				this ,
				timerTabStopMonitor ,
				new Type [ ] { typeof ( Button ) } );
		}	// private void InitializeFormText
		#endregion	// Constructor


		#region Event Delegates and other Private Methods
		private void cmdSaveProfile_Click ( object sender , EventArgs e )
		{
			if ( SaveProfile ( ) )
			{
				_fProfileIsDirty = false;
				_fCheckDirtyFlag = false;
				cmdSaveProfile.Enabled = false;
				timer1.Stop ( );
			}	// TRUE (anticipated outcome, Profile save succeeded) block, if ( SaveProfile ( ) )
			else
			{
				_fProfileIsDirty = true;
				_fCheckDirtyFlag = true;
			}	// FALSE (UNanticipated outcome, Profile save succeeded) block, if ( SaveProfile ( ) )
		}	// private void cmdSaveProfile_Click event delegate


		private int CountUsedRows ( DataGridView pdgvPropertyStringDetails )
		{
			int rintNRowsUsed = Util.INVALID_ORDINAL;

			foreach ( DataGridViewRow dgvThisRow in pdgvPropertyStringDetails.Rows )
				if ( !IsRowEmpty ( dgvThisRow ) )
					rintNRowsUsed++;

			return rintNRowsUsed;
		}	// private int CountUsedRows


		private void Form1_FormClosing ( object sender , FormClosingEventArgs e )
		{
			if ( !_fRuntimeErrorStop )
			{
				switch ( e.CloseReason )
				{
					case CloseReason.ApplicationExitCall:
					case CloseReason.FormOwnerClosing:
					case CloseReason.MdiFormClosing:
					case CloseReason.None:
					case CloseReason.UserClosing:
					case CloseReason.WindowsShutDown:
						if ( !PromptIfProfileIsDirty ( ) )
						{
							e.Cancel = true;
						}	// if ( !PromptIfProfileIsDirty ( ) )
						break;
					case CloseReason.TaskManagerClosing:	// Task Manager always gets its way. If you're killing it via task manager, who am I to argue?
						break;
				}	// switch ( e.CloseReason )
			}	// if ( !_fRuntimeErrorStop )
		}	// Form1_FormClosing


		private static string GetAssemblyGuidString ( )
		{
			object [ ] objAttribs = System.Reflection.Assembly.GetExecutingAssembly ( ).GetCustomAttributes (
				typeof ( System.Runtime.InteropServices.GuidAttribute ) ,
				false );

			if ( objAttribs.Length > Util.EMPTY_STRING_LENGTH )
			{
				System.Runtime.InteropServices.GuidAttribute oMyGuid = ( System.Runtime.InteropServices.GuidAttribute ) objAttribs [ Util.ARRAY_FIRST_ELEMENT ];
				return oMyGuid.Value.ToString ( );
			}	// TRUE (anticipated outcome) block, if ( objAttribs.Length > ListInfo.EMPTY_STRING_LENGTH )
			else
			{
				return string.Empty;
			}	// FALSE (UNanticipated outcome) block, if ( objAttribs.Length > ListInfo.EMPTY_STRING_LENGTH )
		}	// GetAssemblyGuidString


		private void InitializeProfileFileName ( )
		{
			this.txtProfileFileName.Text = Properties.Resources.FRMLBL_PRF_TXTPROFILEFILENAME.Replace (
				TOKEN_PROFILE_NAME ,
				this.txtProfileName.Text ).Replace (
					TOKEN_PROFILE_DIRNAME ,
					this.txtWorkingDirectoryName.Text );

			if ( this.txtProfileFileName.Text.StartsWith ( Path.DirectorySeparatorChar.ToString ( ) ) )
			{
				UpdatePromptMessage (
					PromptPriority.Urgent ,
					Properties.Resources.FRMLBL_PRF_TXTPROFILEUNSPECIFIEDCWD );
			}	// if ( this.txtProfileFileName.Text.StartsWith ( Path.DirectorySeparatorChar.ToString ( ) ) )

			txtWorkingDirectoryName.Tag = txtWorkingDirectoryName.Text;
			timer1.Stop ( );
		}	// private void InitializeProfileFileName


		private void IsProfileDirty ( )
		{
			if ( txtProfileName.Text == Properties.Resources.FRMLBL_PRF_TXTPROFILENAME )
			{	// The profile name is unspecified, indicated by the text "untitled" in the text box.
				cmdSaveProfile.Enabled = false;
			}	// TRUE (degenerate case) block, if ( txtProfileName.Text == Properties.Resources.FRMLBL_PRF_TXTPROFILENAME )
			else if ( !_fCWDIsValid )
			{	// The CWD is either unspecified (the text box is empty) or the specified directory doesn't exist.
				cmdSaveProfile.Enabled = false;
			}	// TRUE (degenerate case) block, else if ( !_fCWDIsValid )
			else if ( !_fProfileNameIsValid )
			{	// This double-check on the first test evaluates a flag.
				cmdSaveProfile.Enabled = false;
			}	// TRUE (degenerate case) block, else if ( !_fProfileNameIsValid )
			else if ( _fProfileIsDirty )
			{	// All prerequisites for saving a profile are met.
				cmdSaveProfile.Enabled = true;
				timer1.Stop ( );
			}	// TRUE (Profile is dirty) block, if ( _fProfileIsDirty )
			else
			{	// Unless the profile is dirty and all prerequisites for saving one are met, the save button is disabled.
				cmdSaveProfile.Enabled = false;
			}	// FALSE (anticipated case) block, if ( txtProfileName.Text == Properties.Resources.FRMLBL_PRF_TXTPROFILENAME )
		}	// private void IsProfileDirty


		private bool IsRowEmpty ( DataGridViewRow pdgvThisRow )
		{
			return ( pdgvThisRow.Cells [ Util.ARRAY_FIRST_ELEMENT ].Value == null || pdgvThisRow.Cells [ Util.ARRAY_SECOND_ELEMENT ].Value == null );
		}	// private void IsRowEmpty


		/// <summary>
		/// This event saves the KeyEventArgs of each key pressed into a private
		/// field for subsequent use by the form's onLeave events.
		/// </summary>
		/// <param name="objSenderIgnored">
		/// This event delegate ignores its sender argument.
		/// </param>
		/// <param name="pkeaKeyEventsForForm">
		/// Though it's probably overkill, the entire KeyEventArgs object goes
		/// into a private field, easily accessible to other event delegates.
		/// </param>
		private void Form1_KeyDown (
			object objSenderIgnored ,
			KeyEventArgs pkeaKeyEventsForForm )
		{
			_keaLastKeyPressed = pkeaKeyEventsForForm;
		}	// private void Form1_KeyDown


		private void MarkWorkingDirectoryAsValid (
			object pobjSender ,
			EventArgs pevtaEventArgs ,
			string pstrMessageFroStatusLine )
		{
			UpdatePromptMessage (
				PromptPriority.Normal ,
				pstrMessageFroStatusLine );
			txtWorkingDirectoryName.Tag = txtWorkingDirectoryName.Text;
			_fCWDIsValid = true;
			txtProfileFileName_TextChanged ( pobjSender , pevtaEventArgs );
		}	// private void MarkWorkingDirectoryAsValid


		private void PropertyySt5ringDetails_RowLeave ( object sender , DataGridViewCellEventArgs e )
		{
			_fProfileIsDirty = true;

			if ( !cmdSaveProfile.Enabled )
			{
				_fCheckDirtyFlag = true;
				timer1.Start ( );
			}	// if ( !cmdSaveProfile.Enabled )
		}	// private void PropertyySt5ringDetails_RowLeave event delegate


		/// <summary>
		/// This event occurs when the focus leaves any cell in the
		/// PropertyStringNames grid control.
		/// </summary>
		/// <param name="objSendingOnwerGridAsObject">
		/// Since all events are raised on behalf of the DataGridView that owns
		/// the cell, it is cast to Object, and becomes the sender, giving this
		/// routine indirect access to the entire grid.
		/// 
		/// Since the companion DataGridViewCellEventArgs argument supplies only
		/// the row and column indices of the cell, you also need access to the
		/// grid to which it belongs.
		/// </param>
		/// <param name="eaCellLosingFocusDetails">
		/// Since you probably need to know all about the cell that just lost
		/// the focus, the designers thoughtfully derived a special class,
		/// DataGridViewCellEventArgs, from EventArgs, and populated it with the
		/// row and column indices of the cell that is losing the focus.
		/// </param>
		/// <see cref="https://msdn.microsoft.com/en-us/library/system.windows.forms.datagridview.cellleave(v=vs.80).aspx"/>
		/// <see cref="https://msdn.microsoft.com/query/dev12.query?appId=Dev12IDEF1&l=EN-US&k=k(System.Windows.Forms.DataGridViewCellEventArgs);k(DataGridViewCellEventArgs);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv2.0);k(DevLang-csharp)&rd=true"/>
		/// <see cref="https://msdn.microsoft.com/en-us/library/system.windows.forms.datagridviewcell(v=vs.80).aspx"/>
		private void PropertyStringNames_CellLeave (
			object pobjSendingOnwerGridAsObject ,
			DataGridViewCellEventArgs peaCellLosingFocusDetails )
		{
			DataGridView dgvOwnerGrid = ( DataGridView ) pobjSendingOnwerGridAsObject;

			if ( dgvOwnerGrid.Columns [ peaCellLosingFocusDetails.ColumnIndex ].Name == NEW_PROPERTY_NAME )
			{	// Cells in other columns are read only, and are, therefore, ignored.
				DataGridViewCell dgrcTestCell = dgvOwnerGrid.Rows [ peaCellLosingFocusDetails.RowIndex ].Cells [ peaCellLosingFocusDetails.ColumnIndex ];

				if ( dgrcTestCell.EditedFormattedValue.ToString ( ).Length > Util.EMPTY_STRING_LENGTH )
				{	// Unless the active cell is populated, there is no point in continuing.
					int intNRowsFilled = Util.FIRST_ORDINAL;

					//	--------------------------------------------------------
					//	Count the other cells in this column that have a value.
					//	Since the current cell is, a priori, populated, the
					//	counter starts at one, and the following loop skips the
					//	current row. Getting a reference to the cell of interest
					//	decreases the computational cost of the next statement,
					//	and substantially shortens its length, rendering the
					//	nested IF that I initially coded unnecessary, since the
					//	C# logical OR operator (&&) short circuits, meaning that
					//	the second test is skipped when the first test fails.
					//	--------------------------------------------------------

					for ( int intCurrentRow = Util.ARRAY_FIRST_ELEMENT ;
						      intCurrentRow < dgvOwnerGrid.RowCount ;
							  intCurrentRow++ )
					{
						if ( intCurrentRow != peaCellLosingFocusDetails.RowIndex )
						{
							dgrcTestCell = dgvOwnerGrid.Rows [ intCurrentRow ].Cells [ peaCellLosingFocusDetails.ColumnIndex ];

							if ( dgrcTestCell.Value != null && dgrcTestCell.EditedFormattedValue.ToString ( ).Length > Util.EMPTY_STRING_LENGTH )
							{
								intNRowsFilled++;
							}	// if ( dgrcTestCell.Value != null && if ( dgrcTestCell.EditedFormattedValue.ToString ( ).Length > Util.EMPTY_STRING_LENGTH )
						}	// if ( intCurrentRow != peaCellLosingFocusDetails.RowIndex )
					}	// for ( int intCurrentRow = Util.ARRAY_FIRST_ELEMENT ; intCurrentRow < dgrLeaving.RowCount ; intCurrentRow++ )

					//	------------------------------------------------------------
					//	Enable the button if every cell in this column is populated.
					//	Otherwise, disable it.
					//	------------------------------------------------------------

					cmdApplyNewLabels.Enabled = ( intNRowsFilled == dgvOwnerGrid.RowCount );
				}	// TRUE (The cell that just lost focus contains text.) block, if ( dgrcTestCell.EditedFormattedValue.ToString ( ).Length > Util.EMPTY_STRING_LENGTH )
				else
				{	// Disable the button if the cell that just lost focus is empty.
					cmdApplyNewLabels.Enabled = false;
				}	// FALSE (The cell that just lost focus is empty.) block, if ( dgrcTestCell.EditedFormattedValue.ToString ( ).Length > Util.EMPTY_STRING_LENGTH )
			}	// if ( dgvOwnerGrid.Columns [ peaCellLosingFocusDetails.ColumnIndex ].Name == NEW_PROPERTY_NAME )
		}	// private void PropertyStringNames_CellLeave event delegate


		private bool PromptIfProfileIsDirty ( )
		{
			if ( cmdSaveProfile.Enabled )
			{	// When the button is enabled, the profile is dirty. Otherwise, it is disabled.
				switch ( MessageBox.Show (
					 Properties.Resources.FRMLBL_PRF_MSG_PROFILE_SAVE_PROMPT ,
					 Application.ProductName ,
					 MessageBoxButtons.YesNoCancel ,
					 MessageBoxIcon.Question ) )
				{
					case System.Windows.Forms.DialogResult.Yes:
						return SaveProfile ( );									// SaveProfile returns true unless it encounters a problem.
					case System.Windows.Forms.DialogResult.No:
						return true;											// The unsaved profile is abandoned, and the program exits.
					case System.Windows.Forms.DialogResult.Cancel:
						return false;											// The unsaved profile is preserved, and execution resumes.
					default:
						return true;											// An unexpected button response arrived. The program silently exits, abandoning the profile.
				}	// switch ( MessageBox.Show ( Properties.Resources.FRMLBL_PRF_MSG_PROFILE_SAVE_PROMPT , Application.ProductName , MessageBoxButtons.YesNoCancel , MessageBoxIcon.Exclamation ) )
			}	// TRUE block, if ( cmdSaveProfile.Enabled )
			else
			{
				return true;
			}	// FALSE block, if ( cmdSaveProfile.Enabled )
		}	// PromptIfProfileIsDirty

		private void RevertWorkingDirectoryName ( )
		{	// Since the Undo event does nothing when the Leave event of its control fires, the previous value must be restored from the Tag property.
			txtWorkingDirectoryName.Text = txtWorkingDirectoryName.Tag.ToString ( );
		}	// private void Revert


		private bool SaveProfile ( )
		{
			try
			{
				byte [ ] abytMagicTokenGUID = Util.GUIDBytesFromGuidString ( txtFileGUID.Text );
				byte [ ] abytNEntries = null;
				int intOffset = Util.BUFFER_BEGINNING;
				List<IndexDetails> lstDetails = new List<IndexDetails> ( );
				SupportedEncoders seSelected = ( SupportedEncoders ) cboEncodingEngine.SelectedItem;	// The explicit cast is unavoidable.

				if ( abytMagicTokenGUID.Length == IndexDetails.BYTES_PER_GUID )
				{
					int intNEntries = CountUsedRows ( PropertyStringDetails );							// Count populated rows in the grid.

					intOffset += IndexDetails.BYTES_PER_GUID;											// Account for the GUID.
					intOffset++;																		// Account for the encoding engine.
					intOffset += IndexDetails.BYTES_PER_INT32;											// Account for the entry count.
					intOffset += ( IndexDetails.BYTES_PER_INDEX_ENTRY * intNEntries );					// Account for the index at 24 bytes per entry.

					abytNEntries = BitConverter.GetBytes ( intNEntries );

					if ( abytNEntries.Length != IndexDetails.BYTES_PER_INT32 )
					{
						System.Diagnostics.Debug.Fail (
							string.Format (
								Properties.Resources.FRMLBL_PRF_ERRMSG_INVALID_INT32_FORMAT ,
								abytNEntries.Length ) );
					}	// FALSE (UNanticipated outcome) block, if ( bytNEntries.Length != IndexDetails.BYTE_PER_INT32 )

					int intOrder = Util.INVALID_ORDINAL;
					IndexDetails.AssignEncodingEngine ( seSelected );

					//	----------------------------------------------------
					//	Run through all of these, so that no more file I/O
					//	happens until all are been parsed.
					//	----------------------------------------------------

					foreach ( DataGridViewRow dgvThisRow in PropertyStringDetails.Rows )
					{
						if ( !IsRowEmpty ( dgvThisRow ) )
						{	// The cleanest way to catch this is before the constructor gets involved, so it doesn't have to throw an exception.
							IndexDetails idxItem = new IndexDetails (
								++intOrder ,
								intOffset ,
								dgvThisRow );
							intOffset += idxItem.StringPropertyBytes.Length;
							lstDetails.Add ( idxItem );
						}	// if ( !IsRowEmpty ( dgvThisRow ) )
					}	// foreach ( DataGridViewRow dgvThisRow in PropertyStringDetails.Rows )
				}	// TRUE (anticipated outcome) block, if ( gidMagicToken.Length == BYTES_PER_GUID )
				else
				{
					System.Diagnostics.Debug.Fail (
						string.Format (
							Properties.Resources.FRMLBL_PRF_ERRMSG_INVALID_GUID_FORMAT ,
							abytMagicTokenGUID.Length ) );
				}	// FALSE (UNanticipated outcome) block, if ( gidMagicToken.Length == BYTES_PER_GUID )

				using ( FileStream fotProfileOut = File.Create ( txtProfileFileName.Text , 8192 , FileOptions.RandomAccess ) )
				{
					fotProfileOut.Write (
						abytMagicTokenGUID ,
						Util.BUFFER_BEGINNING ,
						IndexDetails.BYTES_PER_GUID );
					fotProfileOut.WriteByte ( Convert.ToByte ( seSelected.DefinedEncoder ) );				// This works because the enumeration IS a byte.
					fotProfileOut.Write (
						abytNEntries ,
						Util.BUFFER_BEGINNING ,
						IndexDetails.BYTES_PER_INT32 );

					//	----------------------------------------------------
					//	If execution reaches this point, the inputs passed
					//	all edits, and are good to go. Theoretically, the
					//	sort should be unnecessary.
					//	----------------------------------------------------

					lstDetails.Sort ( );

					//	----------------------------------------------------
					//	Write the index entries.
					//	----------------------------------------------------

					foreach ( IndexDetails idCurrentItem in lstDetails )
					{
						fotProfileOut.Write (
							idCurrentItem.CreateEntryForIndex ( ) ,
							Util.BUFFER_BEGINNING ,
							IndexDetails.BYTES_PER_INDEX_ENTRY );
					}	// foreach ( IndexDetails idCurrentItem in lstDetails )

					//	----------------------------------------------------
					//	Write the string entries in the same order.
					//	----------------------------------------------------

					foreach ( IndexDetails idCurrentItem in lstDetails )
					{
						fotProfileOut.Write (
							idCurrentItem.StringPropertyBytes ,
							Util.BUFFER_BEGINNING ,
							idCurrentItem.StringPropertyBytes.Length );
					}	// foreach ( IndexDetails idCurrentItem in lstDetails )

					//	----------------------------------------------------
					//	Finally, put another copy of the GUID at the end of the file.
					//	----------------------------------------------------

					fotProfileOut.Write (
						abytMagicTokenGUID ,
						Util.BUFFER_BEGINNING ,
						IndexDetails.BYTES_PER_GUID );
				}	// using ( FileStream fotProfileOut = File.Create ( txtProfileFileName.Text , 8192 , FileOptions.RandomAccess ) )

				ProfileMetaData pmd = new ProfileMetaData (
					txtProfileName.Text ,
					txtWorkingDirectoryName.Text ,
					txtProfileFileName.Text );

				pmd.SelectedEncoder = seSelected;
				List<string> alstNewNames = new List<string> ( );

				if ( cmdApplyNewLabels.Enabled )
				{
					foreach ( DataGridViewRow dgrNewNames in PropertyStringNames.Rows )
					{
						int intNColumns = dgrNewNames.Cells.Count;

						for ( int intCurrCell = Util.ARRAY_FIRST_ELEMENT ; intCurrCell < intNColumns ; intCurrCell++ )
						{
							DataGridViewTextBoxCell dgt = ( DataGridViewTextBoxCell ) dgrNewNames.Cells [ intCurrCell ];

							if ( dgt.OwningColumn.Name == NEW_PROPERTY_NAME )
							{
								if ( !ProfileMetaData.HideThisLabel ( dgt.EditedFormattedValue.ToString ( ) ) )
								{
									alstNewNames.Add ( dgt.EditedFormattedValue.ToString ( ) );
								}	// if ( ProfileMetaData.HideThisLabel ( dgt.EditedFormattedValue.ToString ( ) ) )
							}	// if ( dgt.OwningColumn.Name == NEW_PROPERTY_NAME )
						}	// for ( int intCurrCell = Util.ARRAY_FIRST_ELEMENT ; intCurrCell < intNColumns ; intCurrCell++ )
					}	// foreach ( DataGridViewRow dgrNewName in PropertyStringNames.Rows )
				}	// TRUE (new names assigned) block, if ( cmdApplyNewLabels.Enabled )
				else
				{
					foreach ( DataGridViewColumn dgCol in PropertyStringDetails.Columns )
					{
						if ( dgCol.Name != IDENTITY_COLUMN_NAME )
						{	// Skip this column, since its name cannot be changed.
							alstNewNames.Add ( dgCol.HeaderText );
						}	// if ( dgCol.Name != IDENTITY_COLUMN_NAME )
					}	// foreach ( DataGridViewColumn dgCol in PropertyStringDetails.Columns )
				}	// FALSE (using existing names) block, if ( cmdApplyNewLabels.Enabled )

				pmd.ColumnLabels = alstNewNames.ToArray ( );

				if ( pmd.SaveState ( ) )
				{
					FileInfo finewProfile = new FileInfo ( txtProfileFileName.Text );
					UpdatePromptMessage (
						PromptPriority.Yay ,
						string.Format (
							Properties.Resources.FRMLBL_PRF_MSG_PROFILE_SAVED ,
							finewProfile.Length.ToString ( "N0" ) ) );
					return true;
				}	// TRUE (anticipated outcome) block, if ( pmd.SaveState ( ) )
				else
				{
					UpdatePromptMessage (
						PromptPriority.Yay ,
						Properties.Resources.FRMLBL_PRF_METADATA_SAVE_FAILED );
					return false;
				}	// FALSE (anticipated outcome) block, if ( pmd.SaveState ( ) )
			}
			catch ( IOException exIOAll )
			{
				MessageBox.Show (
					exIOAll.Message ,
					Application.ProductName ,
					MessageBoxButtons.OK ,
					MessageBoxIcon.Warning );
				UpdatePromptMessage (
					PromptPriority.Urgent ,
					string.Format (
						Properties.Resources.FRMLBL_PRF_MSG_PROFILE_SAVE_FAILED ,
						exIOAll.Message ) );
				return false;
			}
			catch ( Exception exAllOthers )
			{
				MessageBox.Show (
					exAllOthers.Message ,
					Application.ProductName ,
					MessageBoxButtons.OK ,
					MessageBoxIcon.Warning );
				UpdatePromptMessage (
					PromptPriority.Urgent ,
					string.Format (
						Properties.Resources.FRMLBL_PRF_MSG_PROFILE_SAVE_FAILED ,
						exAllOthers.Message ) );
				_fRuntimeErrorStop = true;
				return false;
			}
		}	// SaveProfile


		/// <summary>
		/// This event delegate is wired up to two TextBox controls, hence the
		/// generic name. Its goal is to change the background color of such a
		/// control if its Text property is the empty string.
		/// </summary>
		/// <param name="pobjSender">
		/// Sender is the control that raised the event, cast to Object. To gain
		/// access to its properties, it must be cast back to TextBox.
		/// </param>
		/// <param name="peaEventArgsIgnored">
		/// This generic EventArgs parameter is part of the method signature of
		/// any event delegate; this particular method ignores it.
		/// </param>
		private void TextBox_Enter (
			object pobjSender ,
			EventArgs peaEventArgsIgnored )
		{
			if ( pobjSender.GetType ( ) == typeof ( TextBox ) )
			{	// This sanity check paves the way for using this event delegate when I have less control over the types of controls to which it is wired up.
				TextBox tbSender = ( TextBox ) pobjSender;

				//	------------------------------------------------------------
				//	The presence of text in a control causes it to be selected
				//	(highlighted). However, the i-beam cursor can easily seem to
				//	disappear when the text box is empty. Hence, if it's empty,
				//	we change the background color of the text box.
				//
				//	Since there is a good chance that the tag is preempted, the
				//	simplest way to undo the change is to have the Leave event
				//	unconditionally do so.
				//	------------------------------------------------------------

				if ( tbSender.Text.Length == Util.EMPTY_STRING_LENGTH )
				{
					tbSender.BackColor = Color.LightGreen;
				}	// if ( tbSender.Text.Length == EMPTY_STRING_LENGTH )
			}	// if ( sender.GetType ( ) == typeof ( TextBox ) )
		}	// private void TextBox_Enter


		private void timer1_Tick ( object sender , EventArgs e )
		{
			if ( _fValidateCWD )
			{
				if ( Directory.Exists ( txtWorkingDirectoryName.Text ) )
				{
					_fValidateCWD = false;
					timer1.Stop ( );
					MarkWorkingDirectoryAsValid (
						sender ,
						e ,
						Properties.Resources.FRMLBL_PRF_MSG_DIRECTORY_CREATED );
				}	// if ( Directory.Exists ( txtWorkingDirectoryName.Text ) )
			}	// if ( _fValidateCWD )

			if ( _fCheckDirtyFlag )
			{
				IsProfileDirty ( );
			}	// if ( _fCheckDirtyFlag )
		}	// timer1_Tick


		/// <summary>
		/// This event processes timer events on behalf of timerTabStopMonitor,
		/// a dedicated Timer controls set aside for the exclusive use of the
		/// TabStopIndex instance, _tsi.
		/// </summary>
		/// <param name="pobjThisTimer">
		/// The Timer Tick event sends a reference to itself (i. e., the Timer),
		///	which isn't much use, considering that the parent form holds a
		///	reference to it, and is visible to this method.
		/// </param>
		/// <param name="e">
		/// The EventArgs event is even less useful; it is essentially empty,
		/// and is ignored.
		/// </param>
		/// <remarks>
		/// This method is essentially a stub; the real work happens in method
		/// ChangeFocusIfExcluded_Tick, an instance method on the TabStopIndex
		/// instance, _tsi, which the main form created and owns. Accordingly,
		/// it receives a reference to this form, which gives it access to 
		/// everything it needs to do its job.
		/// </remarks>
		private void timerTabStopMonitor_TickEventStub ( object pobjThisTimer , EventArgs e )
		{
			_tsi.ChangeFocusIfExcluded_Tick ( );
		}	// timerTabStopMonitor_TickEventStub


		private void txtProfileFileName_TextChanged ( object sender , EventArgs e )
		{
			if ( this.txtPromptMessage.Text == Properties.Resources.FRMLBL_PRF_TXTPROFILEUNSPECIFIEDCWD )
			{
				txtProfileFileName.Text = Properties.Resources.FRMLBL_PRF_TXTPROFILEFILENAME.Replace (
					TOKEN_PROFILE_DIRNAME ,
					txtWorkingDirectoryName.Text ).Replace (
						TOKEN_PROFILE_NAME ,
						txtProfileName.Text );
			}	// TRUE (degenerate case) block, if ( this.txtPromptMessage.Text == Properties.Resources.FRMLBL_PRF_TXTPROFILEUNSPECIFIEDCWD )
			else
			{
				txtWorkingDirectoryName.Text = Util.SanitizePathString (
					txtWorkingDirectoryName.Text ,
					Util.PathStringSemantics.Path );

				if ( Directory.Exists ( txtWorkingDirectoryName.Text ) )
				{
					if ( !txtProfileFileName.Text.StartsWith ( txtWorkingDirectoryName.Text ) )
					{
						txtProfileFileName.Text = Path.Combine (
							txtWorkingDirectoryName.Text ,
							txtProfileFileName.Text.StartsWith ( Path.DirectorySeparatorChar.ToString ( ) )
								? txtProfileFileName.Text.Substring ( 1 )
								: txtProfileFileName.Text );
						txtProfileName_Leave ( sender , e );
					}	// if ( !txtProfileFileName.Text.StartsWith ( txtWorkingDirectoryName.Text ) )
				}	// if ( Directory.Exists ( txtWorkingDirectoryName.Text ) )
			}	// FALSE (anticipated outcome) block, if ( this.txtPromptMessage.Text == Properties.Resources.FRMLBL_PRF_TXTPROFILEUNSPECIFIEDCWD )
		}	// private void txtProfileFileName_TextChanged


		private void txtProfileName_Leave ( object sender , EventArgs e )
		{
			txtProfileName.BackColor = SystemColors.Window;
			txtProfileName.Text = Util.SanitizePathString (
				txtProfileName.Text ,
				Util.PathStringSemantics.File );

			if ( txtProfileName.Text == Properties.Resources.FRMLBL_PRF_TXTPROFILENAME )
			{	// Go no further.
				_fProfileNameIsValid = false;
			}	// TRUE (degenerate case, uninitialized) block, if ( txtProfileName.Text == Properties.Resources.FRMLBL_PRF_TXTPROFILENAME )
			else
			{	// Mark the profile as valid, and fix up the derived fields.
				_fProfileNameIsValid = true;
				IsProfileDirty ( );

				if ( txtProfileFileName.Text.StartsWith ( Path.DirectorySeparatorChar.ToString ( ) ) )
				{
					txtProfileFileName.Text = Properties.Resources.FRMLBL_PRF_TXTPROFILEFILENAME.Replace (
						TOKEN_PROFILE_DIRNAME ,
						string.Empty ).Replace (
							TOKEN_PROFILE_NAME ,
							txtProfileName.Text );
				}	// TRUE (degenerate case) block, if ( txtProfileFileName.Text.StartsWith ( Path.DirectorySeparatorChar.ToString ( ) ) )
				else
				{
					txtProfileFileName.Text = Properties.Resources.FRMLBL_PRF_TXTPROFILEFILENAME.Replace (
						TOKEN_PROFILE_DIRNAME ,
						txtWorkingDirectoryName.Text ).Replace (
							TOKEN_PROFILE_NAME ,
							txtProfileName.Text );

					if ( File.Exists ( txtProfileFileName.Text ) )
					{
						UpdatePromptMessage (
							PromptPriority.Important ,
							Properties.Resources.FRMLBL_PRF_MSG_PROFILE_EXISTS );
					}	// TRUE (error or update scenario) block, if ( File.Exists ( txtProfileFileName.Text ) )
					else
					{
						UpdatePromptMessage (
							PromptPriority.Normal ,
							Properties.Resources.FRMLBL_PRF_MSG_PROFILE_IS_BRAND_NEW );
					}	// FALSE (brand new profile scenario) block, if ( File.Exists ( txtProfileFileName.Text ) )
				}	// FALSE (anticipated case) block, if ( txtProfileFileName.Text.StartsWith ( Path.DirectorySeparatorChar.ToString ( ) ) )
			}	// FALSE (desired outcome) block, if ( txtProfileName.Text == Properties.Resources.FRMLBL_PRF_TXTPROFILENAME )

			//	----------------------------------------------------------------
			//	Finally, call the AlertNextControl method on TabStopIndex object
			//	_tsi, so that it can prepare itself and the main form to change
			//	the focus to the next control in the list, skipping controls
			//	that have tab stops, but are excluded from normal sequencing.
			//	----------------------------------------------------------------

			_tsi.AlertTheNextControl ( ( Control ) sender );
		}	// private void txtProfileName_Leave event delegate


		private void txtWorkingDirectoryName_Leave ( object sender , EventArgs e )
		{
			txtWorkingDirectoryName.BackColor = SystemColors.Window;

			if ( txtWorkingDirectoryName.Text.Length > Util.EMPTY_STRING_LENGTH )
			{
				if ( txtWorkingDirectoryName.Text.EndsWith ( Path.DirectorySeparatorChar.ToString ( ) ) )
				{
					txtWorkingDirectoryName.Text = txtWorkingDirectoryName.Text.Substring (
						Util.ARRAY_FIRST_ELEMENT ,
						txtWorkingDirectoryName.Text.Length - 1 );
				}	// if ( txtWorkingDirectoryName.Text.EndsWith ( Path.DirectorySeparatorChar.ToString ( ) ) )

				if ( Directory.Exists ( txtWorkingDirectoryName.Text ) )
				{
					MarkWorkingDirectoryAsValid (
						sender ,
						e ,
						Properties.Resources.FRMLBL_PRF_MSG_DIRECTORY_FOUND );
				}	// TRUE block, if ( Directory.Exists ( txtWorkingDirectoryName.Text ) )
				else
				{
					switch ( MessageBox.Show (
						string.Format (
							Properties.Resources.FRMLBL_PRF_MSG_DIRECTORY_NOT_FOUND ,
							txtWorkingDirectoryName.Text ,
							Environment.NewLine ) ,
						Application.ProductName ,
						MessageBoxButtons.YesNoCancel ,
						MessageBoxIcon.Warning ) )
					{
						case DialogResult.Yes:
							try
							{
								Directory.CreateDirectory ( txtWorkingDirectoryName.Text );
								MarkWorkingDirectoryAsValid (
									sender ,
									e ,
									Properties.Resources.FRMLBL_PRF_MSG_DIRECTORY_CREATED );
							}
							catch ( IOException exIOAllKinds )
							{
								switch ( MessageBox.Show (
									exIOAllKinds.Message ,
									Application.ProductName ,
									MessageBoxButtons.OKCancel ,
									MessageBoxIcon.Warning ) )
								{
									case DialogResult.OK:
										UpdatePromptMessage (
											PromptPriority.Urgent ,
											Properties.Resources.FRMLBL_PRF_MSG_DIRECTORY_NOT_CREATED );
										txtWorkingDirectoryName.Tag = txtWorkingDirectoryName.Text;
										_fValidateCWD = true;
										timer1.Start ( );
										break;	// case DialogResult.OK

									case DialogResult.Cancel:
										RevertWorkingDirectoryName ( );
										break;	// case System.Windows.Forms.DialogResult.Cancel:
								}	// switch ( MessageBox.Show ( exIOAllKinds.Message , Application.ProductName , MessageBoxButtons.OKCancel , MessageBoxIcon.Warning ) )
							}
							break;	// case DialogResult.Yes

						case DialogResult.No:
							UpdatePromptMessage (
								PromptPriority.Urgent ,
								Properties.Resources.FRMLBL_PRF_MSG_DIRECTORY_NOT_CREATED.Replace (
									TOKEN_PROFILE_DIRNAME ,
									txtWorkingDirectoryName.Text ) );
							txtWorkingDirectoryName.Tag = txtWorkingDirectoryName.Text;
							_fValidateCWD = true;
							timer1.Start ( );
							break;	// case DialogResult.No

						case DialogResult.Cancel:
							RevertWorkingDirectoryName ( );
							break;	// case Forms.DialogResult.Cancel
					}	// switch ( System.Windows.Forms.MessageBox.Show ( Properties.Resources.FRMLBL_PRF_MSG_DIRECTORY_NOT_FOUND , Application.ProductName , MessageBoxButtons.YesNoCancel , MessageBoxIcon.Warning ) )
				}	// FALSE block, if ( Directory.Exists ( txtWorkingDirectoryName.Text ) )
			}	// TRUE block, if ( txtWorkingDirectoryName.Text.Length > EMPTY_STRING_LENGTH )
			else
			{
				UpdatePromptMessage (
					PromptPriority.Urgent ,
					Properties.Resources.FRMLBL_PRF_TXTPROFILEUNSPECIFIEDCWD );
			}	// FALSE block, if ( txtWorkingDirectoryName.Text.Length > EMPTY_STRING_LENGTH )

			//	----------------------------------------------------------------
			//	Finally, call the AlertNextControl method on TabStopIndex object
			//	_tsi, so that it can prepare itself and the main form to change
			//	the focus to the next control in the list, skipping controls
			//	that have tab stops, but are excluded from normal sequencing.
			//	----------------------------------------------------------------

			_tsi.AlertTheNextControl ( ( Control ) sender );
		}	// private void txtWorkingDirectoryName_Leave event delegate


		private void UpdatePromptMessage (
			PromptPriority penmPromptPriority ,
			string pstrMessage )
		{
			this.txtPromptMessage.Text = pstrMessage;

			switch ( penmPromptPriority )
			{
				case PromptPriority.Normal:
					this.txtPromptMessage.ForeColor = SystemColors.WindowText;
					this.txtPromptMessage.BackColor = SystemColors.Control;
					break;
				case PromptPriority.Important:
					this.txtPromptMessage.ForeColor = Color.Black;
					this.txtPromptMessage.BackColor = Color.Yellow;
					break;
				case PromptPriority.Urgent:
					this.txtPromptMessage.ForeColor = Color.White;
					this.txtPromptMessage.BackColor = Color.Red;
					break;
				case PromptPriority.Yay:
					this.txtPromptMessage.ForeColor = Color.White;
					this.txtPromptMessage.BackColor = Color.Green;
					break;
			}	// switch ( penmPromptPriority )
		}	// private void UpdatePromptMessage
		#endregion	// Event Delegates and other Private Methods


		#region Private Storage for state info
		private bool _fCheckDirtyFlag = false;
		private bool _fCWDIsValid = false;
		private bool _fProfileNameIsValid = false;
		private bool _fRuntimeErrorStop = false;
		private bool _fProfileIsDirty = false;
		private bool _fValidateCWD = false;					// Set this flag to TRUE when a new working directory that doesn't exist was specified, but not created.
		private TabStopIndex _tsi = null;
		private KeyEventArgs _keaLastKeyPressed;
		#endregion	// Private Storage for state info


		#region Static Storage
		private List<SupportedEncoders> s_lstSupportedEncoders = new List<SupportedEncoders> ( )
		{
			{ new SupportedEncoders ( SupportedEncoders.Encoder.ASCII   , 10 ) } ,
			{ new SupportedEncoders ( SupportedEncoders.Encoder.Unicode , 20 ) } ,
			{ new SupportedEncoders ( SupportedEncoders.Encoder.UTF32   , 50 ) } ,
			{ new SupportedEncoders ( SupportedEncoders.Encoder.UTF7    , 40 ) } ,
			{ new SupportedEncoders ( SupportedEncoders.Encoder.UTF8    , 30 ) }
		};	// s_lstSupportedEncoders
		#endregion	// Static Storage
	}	// public partial class Form1 : Form
}	// partial namespace BinaryPropertyStorage