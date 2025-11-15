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
		SuspendLayout();
		// 
		// checkedListBoxFacts
		// 
		checkedListBoxFacts.FormattingEnabled = true;
		checkedListBoxFacts.Location = new System.Drawing.Point(10, 37);
		checkedListBoxFacts.Name = "checkedListBoxFacts";
		checkedListBoxFacts.Size = new System.Drawing.Size(424, 130);
		checkedListBoxFacts.TabIndex = 0;
		// 
		// comboBoxGoal
		// 
		comboBoxGoal.FormattingEnabled = true;
		comboBoxGoal.Location = new System.Drawing.Point(10, 8);
		comboBoxGoal.Name = "comboBoxGoal";
		comboBoxGoal.Size = new System.Drawing.Size(424, 23);
		comboBoxGoal.TabIndex = 1;
		// 
		// textBoxOutput
		// 
		textBoxOutput.AcceptsTab = true;
		textBoxOutput.Location = new System.Drawing.Point(10, 193);
		textBoxOutput.Multiline = true;
		textBoxOutput.Name = "textBoxOutput";
		textBoxOutput.ReadOnly = true;
		textBoxOutput.Size = new System.Drawing.Size(772, 248);
		textBoxOutput.TabIndex = 2;
		// 
		// buttonSelectFacts
		// 
		buttonSelectFacts.Location = new System.Drawing.Point(501, 8);
		buttonSelectFacts.Name = "buttonSelectFacts";
		buttonSelectFacts.Size = new System.Drawing.Size(102, 43);
		buttonSelectFacts.TabIndex = 3;
		buttonSelectFacts.Text = "Load Facts";
		buttonSelectFacts.UseVisualStyleBackColor = true;
		buttonSelectFacts.Click += buttonSelectFacts_Click;
		// 
		// buttonSelectRules
		// 
		buttonSelectRules.Location = new System.Drawing.Point(647, 8);
		buttonSelectRules.Name = "buttonSelectRules";
		buttonSelectRules.Size = new System.Drawing.Size(102, 43);
		buttonSelectRules.TabIndex = 4;
		buttonSelectRules.Text = "Load Rules";
		buttonSelectRules.UseVisualStyleBackColor = true;
		buttonSelectRules.Click += buttonSelectRules_Click;
		// 
		// buttonForward
		// 
		buttonForward.Location = new System.Drawing.Point(501, 124);
		buttonForward.Name = "buttonForward";
		buttonForward.Size = new System.Drawing.Size(102, 43);
		buttonForward.TabIndex = 5;
		buttonForward.Text = "Forward";
		buttonForward.UseVisualStyleBackColor = true;
		buttonForward.Click += buttonForward_Click;
		// 
		// buttonBackward
		// 
		buttonBackward.Location = new System.Drawing.Point(647, 124);
		buttonBackward.Name = "buttonBackward";
		buttonBackward.Size = new System.Drawing.Size(102, 43);
		buttonBackward.TabIndex = 6;
		buttonBackward.Text = "Backward";
		buttonBackward.UseVisualStyleBackColor = true;
		buttonBackward.Click += buttonBackward_Click;
		// 
		// MainForm
		// 
		AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
		AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		ClientSize = new System.Drawing.Size(800, 450);
		Controls.Add(buttonBackward);
		Controls.Add(buttonForward);
		Controls.Add(buttonSelectRules);
		Controls.Add(buttonSelectFacts);
		Controls.Add(textBoxOutput);
		Controls.Add(comboBoxGoal);
		Controls.Add(checkedListBoxFacts);
		Text = "MainForm";
		ResumeLayout(false);
		PerformLayout();
	}

	private System.Windows.Forms.Button buttonForward;

	private System.Windows.Forms.Button buttonBackward;

	private System.Windows.Forms.Button buttonSelectRules;

	private System.Windows.Forms.Button buttonSelectFacts;

	#endregion

    private System.Windows.Forms.CheckedListBox checkedListBoxFacts;
    private System.Windows.Forms.ComboBox comboBoxGoal;
    private System.Windows.Forms.TextBox textBoxOutput;
}