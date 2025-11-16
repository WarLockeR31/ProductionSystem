using System.ComponentModel;

namespace ProductionSystem;

partial class GraphForm
{
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private IContainer components = null;

	/// <summary>
	/// Clean up any resources being used.
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
		panelGraph = new System.Windows.Forms.Panel();
		SuspendLayout();
		// 
		// panelGraph
		// 
		panelGraph.Dock = System.Windows.Forms.DockStyle.Fill;
		panelGraph.Location = new System.Drawing.Point(0, 0);
		panelGraph.Margin = new System.Windows.Forms.Padding(0);
		panelGraph.Name = "panelGraph";
		panelGraph.Size = new System.Drawing.Size(784, 411);
		panelGraph.TabIndex = 0;
		panelGraph.Paint += panelGraph_Paint;
		// 
		// GraphForm
		// 
		AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
		AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		ClientSize = new System.Drawing.Size(784, 411);
		Controls.Add(panelGraph);
		Text = "GraphForm";
		ResumeLayout(false);
	}

	private System.Windows.Forms.Panel panelGraph;

	#endregion
}