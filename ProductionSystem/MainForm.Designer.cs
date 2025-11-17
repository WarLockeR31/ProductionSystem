namespace ProductionSystem;

partial class MainForm
{
	/// <summary>
	///  Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	///  Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}

		base.Dispose(disposing);
	}

    #region Windows Form Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		checkedListBoxFacts = new System.Windows.Forms.CheckedListBox();
		comboBoxGoal = new System.Windows.Forms.ComboBox();
		textBoxOutput = new System.Windows.Forms.TextBox();
		buttonSelectFacts = new System.Windows.Forms.Button();
		buttonSelectRules = new System.Windows.Forms.Button();
		buttonForward = new System.Windows.Forms.Button();
		buttonBackward = new System.Windows.Forms.Button();
		buttonClearSelection = new System.Windows.Forms.Button();
		buttonGraph = new System.Windows.Forms.Button();
		buttonLoadPreset = new System.Windows.Forms.Button();
		SuspendLayout();
		// 
		// checkedListBoxFacts
		// 
		checkedListBoxFacts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
		checkedListBoxFacts.FormattingEnabled = true;
		checkedListBoxFacts.Location = new System.Drawing.Point(11, 37);
		checkedListBoxFacts.Name = "checkedListBoxFacts";
		checkedListBoxFacts.ScrollAlwaysVisible = true;
		checkedListBoxFacts.Size = new System.Drawing.Size(534, 130);
		checkedListBoxFacts.TabIndex = 0;
		// 
		// comboBoxGoal
		// 
		comboBoxGoal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
		comboBoxGoal.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		comboBoxGoal.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		comboBoxGoal.FormattingEnabled = true;
		comboBoxGoal.Location = new System.Drawing.Point(11, 8);
		comboBoxGoal.MaxDropDownItems = 12;
		comboBoxGoal.Name = "comboBoxGoal";
		comboBoxGoal.Size = new System.Drawing.Size(534, 23);
		comboBoxGoal.TabIndex = 1;
		// 
		// textBoxOutput
		// 
		textBoxOutput.AcceptsTab = true;
		textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
		textBoxOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		textBoxOutput.Location = new System.Drawing.Point(10, 173);
		textBoxOutput.Multiline = true;
		textBoxOutput.Name = "textBoxOutput";
		textBoxOutput.ReadOnly = true;
		textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		textBoxOutput.Size = new System.Drawing.Size(785, 229);
		textBoxOutput.TabIndex = 2;
		// 
		// buttonSelectFacts
		// 
		buttonSelectFacts.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
		buttonSelectFacts.FlatAppearance.BorderColor = System.Drawing.Color.Black;
		buttonSelectFacts.FlatAppearance.BorderSize = 2;
		buttonSelectFacts.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
		buttonSelectFacts.Location = new System.Drawing.Point(550, 8);
		buttonSelectFacts.Name = "buttonSelectFacts";
		buttonSelectFacts.Size = new System.Drawing.Size(119, 34);
		buttonSelectFacts.TabIndex = 3;
		buttonSelectFacts.Text = "Load Facts";
		buttonSelectFacts.UseVisualStyleBackColor = true;
		buttonSelectFacts.Click += buttonSelectFacts_Click;
		// 
		// buttonSelectRules
		// 
		buttonSelectRules.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
		buttonSelectRules.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
		buttonSelectRules.Location = new System.Drawing.Point(676, 8);
		buttonSelectRules.Name = "buttonSelectRules";
		buttonSelectRules.Size = new System.Drawing.Size(119, 34);
		buttonSelectRules.TabIndex = 4;
		buttonSelectRules.Text = "Load Rules";
		buttonSelectRules.UseVisualStyleBackColor = true;
		buttonSelectRules.Click += buttonSelectRules_Click;
		// 
		// buttonForward
		// 
		buttonForward.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
		buttonForward.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
		buttonForward.Location = new System.Drawing.Point(551, 133);
		buttonForward.Name = "buttonForward";
		buttonForward.Size = new System.Drawing.Size(119, 34);
		buttonForward.TabIndex = 5;
		buttonForward.Text = "Forward";
		buttonForward.UseVisualStyleBackColor = true;
		buttonForward.Click += buttonForward_Click;
		// 
		// buttonBackward
		// 
		buttonBackward.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
		buttonBackward.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
		buttonBackward.Location = new System.Drawing.Point(676, 133);
		buttonBackward.Name = "buttonBackward";
		buttonBackward.Size = new System.Drawing.Size(119, 34);
		buttonBackward.TabIndex = 6;
		buttonBackward.Text = "Backward";
		buttonBackward.UseVisualStyleBackColor = true;
		buttonBackward.Click += buttonBackward_Click;
		// 
		// buttonClearSelection
		// 
		buttonClearSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
		buttonClearSelection.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
		buttonClearSelection.Location = new System.Drawing.Point(549, 48);
		buttonClearSelection.Name = "buttonClearSelection";
		buttonClearSelection.Size = new System.Drawing.Size(120, 34);
		buttonClearSelection.TabIndex = 7;
		buttonClearSelection.Text = "Clear Selection";
		buttonClearSelection.UseVisualStyleBackColor = true;
		buttonClearSelection.Click += buttonClearSelection_Click;
		// 
		// buttonGraph
		// 
		buttonGraph.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
		buttonGraph.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
		buttonGraph.Location = new System.Drawing.Point(551, 88);
		buttonGraph.Name = "buttonGraph";
		buttonGraph.Size = new System.Drawing.Size(244, 34);
		buttonGraph.TabIndex = 8;
		buttonGraph.Text = "Open Graph";
		buttonGraph.UseVisualStyleBackColor = true;
		buttonGraph.Click += buttonGraph_Click;
		// 
		// buttonLoadPreset
		// 
		buttonLoadPreset.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
		buttonLoadPreset.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
		buttonLoadPreset.Location = new System.Drawing.Point(675, 48);
		buttonLoadPreset.Name = "buttonLoadPreset";
		buttonLoadPreset.Size = new System.Drawing.Size(120, 34);
		buttonLoadPreset.TabIndex = 9;
		buttonLoadPreset.Text = "Load Preset";
		buttonLoadPreset.UseVisualStyleBackColor = true;
		buttonLoadPreset.Click += buttonLoadPreset_Click;
		// 
		// MainForm
		// 
		AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
		AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		ClientSize = new System.Drawing.Size(813, 411);
		Controls.Add(buttonLoadPreset);
		Controls.Add(buttonGraph);
		Controls.Add(buttonClearSelection);
		Controls.Add(buttonBackward);
		Controls.Add(buttonForward);
		Controls.Add(buttonSelectRules);
		Controls.Add(buttonSelectFacts);
		Controls.Add(textBoxOutput);
		Controls.Add(comboBoxGoal);
		Controls.Add(checkedListBoxFacts);
		MinimumSize = new System.Drawing.Size(800, 450);
		Text = "ProductionSystem";
		ResumeLayout(false);
		PerformLayout();
	}

	private System.Windows.Forms.Button buttonLoadPreset;

	private System.Windows.Forms.Button buttonGraph;

	private System.Windows.Forms.Button buttonClearSelection;

	private System.Windows.Forms.Button buttonForward;

	private System.Windows.Forms.Button buttonBackward;

	private System.Windows.Forms.Button buttonSelectRules;

	private System.Windows.Forms.Button buttonSelectFacts;

	#endregion

    private System.Windows.Forms.CheckedListBox checkedListBoxFacts;
    private System.Windows.Forms.ComboBox comboBoxGoal;
    private System.Windows.Forms.TextBox textBoxOutput;
}