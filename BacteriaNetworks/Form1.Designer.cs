using BacteriaNetworks.Infrastructure;

namespace BacteriaNetworks
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbFilter = new System.Windows.Forms.ComboBox();
			this.btnSearchNetwork = new System.Windows.Forms.Button();
			this.cmbBacteria = new BacteriaNetworks.Infrastructure.AutoCompleteComboBox();
			this.networkViewer = new BacteriaNetworks.NetworkViewer();
			this.lblInfo = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label1.Location = new System.Drawing.Point(40, 88);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(89, 25);
			this.label1.TabIndex = 1;
			this.label1.Text = "Bacteria:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label2.Location = new System.Drawing.Point(40, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(86, 25);
			this.label2.TabIndex = 2;
			this.label2.Text = "Filter by:";
			// 
			// cmbFilter
			// 
			this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbFilter.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmbFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.cmbFilter.FormattingEnabled = true;
			this.cmbFilter.Location = new System.Drawing.Point(132, 40);
			this.cmbFilter.Name = "cmbFilter";
			this.cmbFilter.Size = new System.Drawing.Size(192, 28);
			this.cmbFilter.TabIndex = 3;
			// 
			// btnSearchNetwork
			// 
			this.btnSearchNetwork.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearchNetwork.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnSearchNetwork.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.btnSearchNetwork.Location = new System.Drawing.Point(950, 131);
			this.btnSearchNetwork.Margin = new System.Windows.Forms.Padding(4);
			this.btnSearchNetwork.Name = "btnSearchNetwork";
			this.btnSearchNetwork.Size = new System.Drawing.Size(193, 42);
			this.btnSearchNetwork.TabIndex = 4;
			this.btnSearchNetwork.Text = "Search network";
			this.btnSearchNetwork.UseVisualStyleBackColor = true;
			this.btnSearchNetwork.Click += new System.EventHandler(this.btnSearchNetwork_Click);
			// 
			// cmbBacteria
			// 
			this.cmbBacteria.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.cmbBacteria.Data = null;
			this.cmbBacteria.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.cmbBacteria.FormattingEnabled = true;
			this.cmbBacteria.Location = new System.Drawing.Point(132, 89);
			this.cmbBacteria.Name = "cmbBacteria";
			this.cmbBacteria.Size = new System.Drawing.Size(1011, 28);
			this.cmbBacteria.TabIndex = 0;
			// 
			// networkViewer
			// 
			this.networkViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.networkViewer.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.networkViewer.Location = new System.Drawing.Point(96, 219);
			this.networkViewer.Margin = new System.Windows.Forms.Padding(0);
			this.networkViewer.Name = "networkViewer";
			this.networkViewer.Size = new System.Drawing.Size(920, 381);
			this.networkViewer.TabIndex = 5;
			// 
			// lblInfo
			// 
			this.lblInfo.AutoSize = true;
			this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.lblInfo.Location = new System.Drawing.Point(41, 153);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(53, 20);
			this.lblInfo.TabIndex = 6;
			this.lblInfo.Text = "lblInfo";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1182, 753);
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.networkViewer);
			this.Controls.Add(this.btnSearchNetwork);
			this.Controls.Add(this.cmbFilter);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmbBacteria);
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private AutoCompleteComboBox cmbBacteria;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbFilter;
		private System.Windows.Forms.Button btnSearchNetwork;
		private NetworkViewer networkViewer;
		private System.Windows.Forms.Label lblInfo;
	}
}