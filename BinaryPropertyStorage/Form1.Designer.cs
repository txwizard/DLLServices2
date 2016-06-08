namespace BinaryPropertyStorage
{
    partial class Form1
    {
		/// <summary>
		/// Reference strings assigned as column name and used to identify column for special handling
		/// </summary>
		const string NEW_PROPERTY_NAME = @"PropertyStringName";
		const string IDENTITY_COLUMN_NAME = "ItemGuid";

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose ( );
            }
            base.Dispose ( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( )
        {
			this.components = new System.ComponentModel.Container();
			this.PropertyStringDetails = new System.Windows.Forms.DataGridView();
			this.ItemGuid = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PropertyString1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PropertyString2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PropertyString3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PropertyString4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PropertyString5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PropertyString6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PropertyString7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PropertyString8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PropertyStringNames = new System.Windows.Forms.DataGridView();
			this.PropertyStringLabel = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PropertyStringOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PropertyStringName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.lblProfileName = new System.Windows.Forms.Label();
			this.txtProfileName = new System.Windows.Forms.TextBox();
			this.lblFileName = new System.Windows.Forms.Label();
			this.txtProfileFileName = new System.Windows.Forms.TextBox();
			this.cmdProfileFileBrowser = new System.Windows.Forms.Button();
			this.cmdSaveProfile = new System.Windows.Forms.Button();
			this.cmdNewProfile = new System.Windows.Forms.Button();
			this.lblWorkingDirectory = new System.Windows.Forms.Label();
			this.txtWorkingDirectoryName = new System.Windows.Forms.TextBox();
			this.cmdBrowseForDirectory = new System.Windows.Forms.Button();
			this.txtPromptMessage = new System.Windows.Forms.TextBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.cboEncodingEngine = new System.Windows.Forms.ComboBox();
			this.lblEncodingEngine = new System.Windows.Forms.Label();
			this.lblFileGUID = new System.Windows.Forms.Label();
			this.txtFileGUID = new System.Windows.Forms.TextBox();
			this.cmdNewFileGUID = new System.Windows.Forms.Button();
			this.cmdNameYourOwnFileGUID = new System.Windows.Forms.Button();
			this.cmdApplyNewLabels = new System.Windows.Forms.Button();
			this.timerTabStopMonitor = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.PropertyStringDetails)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PropertyStringNames)).BeginInit();
			this.SuspendLayout();
			// 
			// PropertyStringDetails
			// 
			this.PropertyStringDetails.AllowUserToOrderColumns = true;
			this.PropertyStringDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.PropertyStringDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemGuid,
            this.PropertyString1,
            this.PropertyString2,
            this.PropertyString3,
            this.PropertyString4,
            this.PropertyString5,
            this.PropertyString6,
            this.PropertyString7,
            this.PropertyString8});
			this.PropertyStringDetails.Location = new System.Drawing.Point(0, 265);
			this.PropertyStringDetails.Name = "PropertyStringDetails";
			this.PropertyStringDetails.Size = new System.Drawing.Size(1004, 261);
			this.PropertyStringDetails.TabIndex = 100;
			this.PropertyStringDetails.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.PropertyySt5ringDetails_RowLeave);
			// 
			// ItemGuid
			// 
			this.ItemGuid.HeaderText = "GUID";
			this.ItemGuid.Name = "ItemGuid";
			// 
			// PropertyString1
			// 
			this.PropertyString1.HeaderText = "Property String 1";
			this.PropertyString1.Name = "PropertyString1";
			// 
			// PropertyString2
			// 
			this.PropertyString2.HeaderText = "Property String 2";
			this.PropertyString2.Name = "PropertyString2";
			// 
			// PropertyString3
			// 
			this.PropertyString3.HeaderText = "Property String 3";
			this.PropertyString3.Name = "PropertyString3";
			// 
			// PropertyString4
			// 
			this.PropertyString4.HeaderText = "Property String 4";
			this.PropertyString4.Name = "PropertyString4";
			// 
			// PropertyString5
			// 
			this.PropertyString5.HeaderText = "Property String 5";
			this.PropertyString5.Name = "PropertyString5";
			// 
			// PropertyString6
			// 
			this.PropertyString6.HeaderText = "Property String 6";
			this.PropertyString6.Name = "PropertyString6";
			// 
			// PropertyString7
			// 
			this.PropertyString7.HeaderText = "Property String 7";
			this.PropertyString7.Name = "PropertyString7";
			// 
			// PropertyString8
			// 
			this.PropertyString8.HeaderText = "Property String 8";
			this.PropertyString8.Name = "PropertyString8";
			// 
			// PropertyStringNames
			// 
			this.PropertyStringNames.AllowUserToAddRows = false;
			this.PropertyStringNames.AllowUserToDeleteRows = false;
			this.PropertyStringNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.PropertyStringNames.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PropertyStringLabel,
            this.PropertyStringOrder,
            this.PropertyStringName});
			this.PropertyStringNames.Location = new System.Drawing.Point(0, 8);
			this.PropertyStringNames.Name = "PropertyStringNames";
			this.PropertyStringNames.Size = new System.Drawing.Size(344, 212);
			this.PropertyStringNames.TabIndex = 110;
			this.PropertyStringNames.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.PropertyStringNames_CellLeave);
			// 
			// PropertyStringLabel
			// 
			this.PropertyStringLabel.HeaderText = "Property String Label";
			this.PropertyStringLabel.Name = "PropertyStringLabel";
			this.PropertyStringLabel.ReadOnly = true;
			// 
			// PropertyStringOrder
			// 
			this.PropertyStringOrder.HeaderText = "Property String Order";
			this.PropertyStringOrder.Name = "PropertyStringOrder";
			this.PropertyStringOrder.ReadOnly = true;
			// 
			// PropertyStringName
			// 
			this.PropertyStringName.HeaderText = "PropertyStringName";
			this.PropertyStringName.Name = "PropertyStringName";
			// 
			// lblProfileName
			// 
			this.lblProfileName.AutoSize = true;
			this.lblProfileName.Location = new System.Drawing.Point(371, 47);
			this.lblProfileName.Name = "lblProfileName";
			this.lblProfileName.Size = new System.Drawing.Size(67, 13);
			this.lblProfileName.TabIndex = 0;
			this.lblProfileName.Text = "Profile Name";
			// 
			// txtProfileName
			// 
			this.txtProfileName.Location = new System.Drawing.Point(485, 46);
			this.txtProfileName.Name = "txtProfileName";
			this.txtProfileName.Size = new System.Drawing.Size(221, 20);
			this.txtProfileName.TabIndex = 30;
			this.txtProfileName.Text = "untitled";
			this.txtProfileName.Enter += new System.EventHandler(this.TextBox_Enter);
			this.txtProfileName.Leave += new System.EventHandler(this.txtProfileName_Leave);
			// 
			// lblFileName
			// 
			this.lblFileName.AutoSize = true;
			this.lblFileName.Location = new System.Drawing.Point(371, 86);
			this.lblFileName.Name = "lblFileName";
			this.lblFileName.Size = new System.Drawing.Size(86, 13);
			this.lblFileName.TabIndex = 0;
			this.lblFileName.Text = "Profile File Name";
			// 
			// txtProfileFileName
			// 
			this.txtProfileFileName.Location = new System.Drawing.Point(486, 88);
			this.txtProfileFileName.Name = "txtProfileFileName";
			this.txtProfileFileName.ReadOnly = true;
			this.txtProfileFileName.Size = new System.Drawing.Size(422, 20);
			this.txtProfileFileName.TabIndex = 0;
			this.txtProfileFileName.TabStop = false;
			this.txtProfileFileName.TextChanged += new System.EventHandler(this.txtProfileFileName_TextChanged);
			// 
			// cmdProfileFileBrowser
			// 
			this.cmdProfileFileBrowser.Location = new System.Drawing.Point(934, 87);
			this.cmdProfileFileBrowser.Name = "cmdProfileFileBrowser";
			this.cmdProfileFileBrowser.Size = new System.Drawing.Size(75, 23);
			this.cmdProfileFileBrowser.TabIndex = 40;
			this.cmdProfileFileBrowser.Text = "Browse";
			this.cmdProfileFileBrowser.UseVisualStyleBackColor = true;
			// 
			// cmdSaveProfile
			// 
			this.cmdSaveProfile.Enabled = false;
			this.cmdSaveProfile.Location = new System.Drawing.Point(371, 203);
			this.cmdSaveProfile.Name = "cmdSaveProfile";
			this.cmdSaveProfile.Size = new System.Drawing.Size(80, 19);
			this.cmdSaveProfile.TabIndex = 0;
			this.cmdSaveProfile.Text = "Save Profile";
			this.cmdSaveProfile.UseVisualStyleBackColor = true;
			this.cmdSaveProfile.Click += new System.EventHandler(this.cmdSaveProfile_Click);
			// 
			// cmdNewProfile
			// 
			this.cmdNewProfile.Location = new System.Drawing.Point(737, 43);
			this.cmdNewProfile.Name = "cmdNewProfile";
			this.cmdNewProfile.Size = new System.Drawing.Size(94, 23);
			this.cmdNewProfile.TabIndex = 0;
			this.cmdNewProfile.TabStop = false;
			this.cmdNewProfile.Text = "New Profile";
			this.cmdNewProfile.UseVisualStyleBackColor = true;
			// 
			// lblWorkingDirectory
			// 
			this.lblWorkingDirectory.AutoSize = true;
			this.lblWorkingDirectory.Location = new System.Drawing.Point(371, 8);
			this.lblWorkingDirectory.Name = "lblWorkingDirectory";
			this.lblWorkingDirectory.Size = new System.Drawing.Size(92, 13);
			this.lblWorkingDirectory.TabIndex = 0;
			this.lblWorkingDirectory.Text = "Working Directory";
			// 
			// txtWorkingDirectoryName
			// 
			this.txtWorkingDirectoryName.Location = new System.Drawing.Point(485, 4);
			this.txtWorkingDirectoryName.Name = "txtWorkingDirectoryName";
			this.txtWorkingDirectoryName.Size = new System.Drawing.Size(422, 20);
			this.txtWorkingDirectoryName.TabIndex = 10;
			this.txtWorkingDirectoryName.Enter += new System.EventHandler(this.TextBox_Enter);
			this.txtWorkingDirectoryName.Leave += new System.EventHandler(this.txtWorkingDirectoryName_Leave);
			// 
			// cmdBrowseForDirectory
			// 
			this.cmdBrowseForDirectory.Location = new System.Drawing.Point(931, 5);
			this.cmdBrowseForDirectory.Name = "cmdBrowseForDirectory";
			this.cmdBrowseForDirectory.Size = new System.Drawing.Size(75, 23);
			this.cmdBrowseForDirectory.TabIndex = 20;
			this.cmdBrowseForDirectory.Text = "Select";
			this.cmdBrowseForDirectory.UseVisualStyleBackColor = true;
			// 
			// txtPromptMessage
			// 
			this.txtPromptMessage.Location = new System.Drawing.Point(0, 485);
			this.txtPromptMessage.Name = "txtPromptMessage";
			this.txtPromptMessage.ReadOnly = true;
			this.txtPromptMessage.Size = new System.Drawing.Size(1004, 20);
			this.txtPromptMessage.TabIndex = 0;
			this.txtPromptMessage.TabStop = false;
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// cboEncodingEngine
			// 
			this.cboEncodingEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboEncodingEngine.FormattingEnabled = true;
			this.cboEncodingEngine.Location = new System.Drawing.Point(485, 130);
			this.cboEncodingEngine.Name = "cboEncodingEngine";
			this.cboEncodingEngine.Size = new System.Drawing.Size(121, 21);
			this.cboEncodingEngine.TabIndex = 50;
			// 
			// lblEncodingEngine
			// 
			this.lblEncodingEngine.AutoSize = true;
			this.lblEncodingEngine.Location = new System.Drawing.Point(371, 125);
			this.lblEncodingEngine.Name = "lblEncodingEngine";
			this.lblEncodingEngine.Size = new System.Drawing.Size(88, 13);
			this.lblEncodingEngine.TabIndex = 0;
			this.lblEncodingEngine.Text = "Encoding Engine";
			// 
			// lblFileGUID
			// 
			this.lblFileGUID.AutoSize = true;
			this.lblFileGUID.Location = new System.Drawing.Point(371, 164);
			this.lblFileGUID.Name = "lblFileGUID";
			this.lblFileGUID.Size = new System.Drawing.Size(53, 13);
			this.lblFileGUID.TabIndex = 0;
			this.lblFileGUID.Text = "File GUID";
			// 
			// txtFileGUID
			// 
			this.txtFileGUID.Location = new System.Drawing.Point(486, 172);
			this.txtFileGUID.Name = "txtFileGUID";
			this.txtFileGUID.ReadOnly = true;
			this.txtFileGUID.Size = new System.Drawing.Size(221, 20);
			this.txtFileGUID.TabIndex = 0;
			this.txtFileGUID.TabStop = false;
			// 
			// cmdNewFileGUID
			// 
			this.cmdNewFileGUID.Location = new System.Drawing.Point(737, 171);
			this.cmdNewFileGUID.Name = "cmdNewFileGUID";
			this.cmdNewFileGUID.Size = new System.Drawing.Size(94, 23);
			this.cmdNewFileGUID.TabIndex = 0;
			this.cmdNewFileGUID.TabStop = false;
			this.cmdNewFileGUID.Text = "New File GUID";
			this.cmdNewFileGUID.UseVisualStyleBackColor = true;
			// 
			// cmdNameYourOwnFileGUID
			// 
			this.cmdNameYourOwnFileGUID.Location = new System.Drawing.Point(861, 171);
			this.cmdNameYourOwnFileGUID.Name = "cmdNameYourOwnFileGUID";
			this.cmdNameYourOwnFileGUID.Size = new System.Drawing.Size(147, 23);
			this.cmdNameYourOwnFileGUID.TabIndex = 0;
			this.cmdNameYourOwnFileGUID.TabStop = false;
			this.cmdNameYourOwnFileGUID.Text = "Name Your Own File GUID";
			this.cmdNameYourOwnFileGUID.UseVisualStyleBackColor = true;
			// 
			// cmdApplyNewLabels
			// 
			this.cmdApplyNewLabels.Enabled = false;
			this.cmdApplyNewLabels.Location = new System.Drawing.Point(115, 231);
			this.cmdApplyNewLabels.Name = "cmdApplyNewLabels";
			this.cmdApplyNewLabels.Size = new System.Drawing.Size(115, 23);
			this.cmdApplyNewLabels.TabIndex = 120;
			this.cmdApplyNewLabels.Text = "Apply New Labels";
			this.cmdApplyNewLabels.UseVisualStyleBackColor = true;
			// 
			// timerTabStopMonitor
			// 
			this.timerTabStopMonitor.Interval = 1;
			this.timerTabStopMonitor.Tag = "This timer is reservewd for use by the TabStopIndex object.";
			this.timerTabStopMonitor.Tick += new System.EventHandler(this.timerTabStopMonitor_TickEventStub);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1014, 503);
			this.Controls.Add(this.cmdApplyNewLabels);
			this.Controls.Add(this.cmdNameYourOwnFileGUID);
			this.Controls.Add(this.cmdNewFileGUID);
			this.Controls.Add(this.txtFileGUID);
			this.Controls.Add(this.lblFileGUID);
			this.Controls.Add(this.lblEncodingEngine);
			this.Controls.Add(this.cboEncodingEngine);
			this.Controls.Add(this.txtPromptMessage);
			this.Controls.Add(this.cmdBrowseForDirectory);
			this.Controls.Add(this.txtWorkingDirectoryName);
			this.Controls.Add(this.lblWorkingDirectory);
			this.Controls.Add(this.cmdNewProfile);
			this.Controls.Add(this.cmdSaveProfile);
			this.Controls.Add(this.cmdProfileFileBrowser);
			this.Controls.Add(this.txtProfileFileName);
			this.Controls.Add(this.lblFileName);
			this.Controls.Add(this.txtProfileName);
			this.Controls.Add(this.lblProfileName);
			this.Controls.Add(this.PropertyStringNames);
			this.Controls.Add(this.PropertyStringDetails);
			this.KeyPreview = true;
			this.Name = "Form1";
			this.Text = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			((System.ComponentModel.ISupportInitialize)(this.PropertyStringDetails)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PropertyStringNames)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }   // private void InitializeComponent

        #endregion  // Windows Form Designer generated code

        private System.Windows.Forms.DataGridView PropertyStringDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemGuid;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyString1;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyString2;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyString3;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyString4;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyString5;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyString6;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyString7;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyString8;
        private System.Windows.Forms.DataGridView PropertyStringNames;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyStringLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyStringOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyStringName;
        private System.Windows.Forms.Label lblProfileName;
        private System.Windows.Forms.TextBox txtProfileName;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.TextBox txtProfileFileName;
        private System.Windows.Forms.Button cmdProfileFileBrowser;
        private System.Windows.Forms.Button cmdSaveProfile;
        private System.Windows.Forms.Button cmdNewProfile;
        private System.Windows.Forms.Label lblWorkingDirectory;
        private System.Windows.Forms.TextBox txtWorkingDirectoryName;
        private System.Windows.Forms.Button cmdBrowseForDirectory;
        private System.Windows.Forms.TextBox txtPromptMessage;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox cboEncodingEngine;
        private System.Windows.Forms.Label lblEncodingEngine;
        private System.Windows.Forms.Label lblFileGUID;
        private System.Windows.Forms.TextBox txtFileGUID;
        private System.Windows.Forms.Button cmdNewFileGUID;
        private System.Windows.Forms.Button cmdNameYourOwnFileGUID;
		private System.Windows.Forms.Button cmdApplyNewLabels;
		private System.Windows.Forms.Timer timerTabStopMonitor;
    }   // partial class Form1
}   // partial namespace BinaryPropertyStorage