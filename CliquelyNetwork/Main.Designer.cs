namespace CliquelyNetwork
{
	partial class Main
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
			this.label5 = new System.Windows.Forms.Label();
			this.textBoxMaxCliques = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxMaxCliqueSize = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxTreshold = new System.Windows.Forms.TextBox();
			this.CliquesDGV = new System.Windows.Forms.DataGridView();
			this.buttonSearchFasta = new System.Windows.Forms.Button();
			this.geneLbl = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.CliquesDGV)).BeginInit();
			this.SuspendLayout();
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label5.Location = new System.Drawing.Point(25, 109);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(127, 25);
			this.label5.TabIndex = 20;
			this.label5.Text = "Max Cliques:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBoxMaxCliques
			// 
			this.textBoxMaxCliques.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.textBoxMaxCliques.Location = new System.Drawing.Point(237, 108);
			this.textBoxMaxCliques.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxMaxCliques.Name = "textBoxMaxCliques";
			this.textBoxMaxCliques.Size = new System.Drawing.Size(320, 26);
			this.textBoxMaxCliques.TabIndex = 15;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label4.Location = new System.Drawing.Point(25, 73);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(161, 25);
			this.label4.TabIndex = 18;
			this.label4.Text = "Max Clique Size:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBoxMaxCliqueSize
			// 
			this.textBoxMaxCliqueSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.textBoxMaxCliqueSize.Location = new System.Drawing.Point(237, 72);
			this.textBoxMaxCliqueSize.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxMaxCliqueSize.Name = "textBoxMaxCliqueSize";
			this.textBoxMaxCliqueSize.Size = new System.Drawing.Size(320, 26);
			this.textBoxMaxCliqueSize.TabIndex = 14;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label2.Location = new System.Drawing.Point(25, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(108, 25);
			this.label2.TabIndex = 16;
			this.label2.Text = "Probability:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBoxTreshold
			// 
			this.textBoxTreshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.textBoxTreshold.Location = new System.Drawing.Point(237, 34);
			this.textBoxTreshold.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxTreshold.Name = "textBoxTreshold";
			this.textBoxTreshold.Size = new System.Drawing.Size(320, 26);
			this.textBoxTreshold.TabIndex = 13;
			// 
			// CliquesDGV
			// 
			this.CliquesDGV.AllowUserToAddRows = false;
			this.CliquesDGV.AllowUserToDeleteRows = false;
			this.CliquesDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.CliquesDGV.Location = new System.Drawing.Point(30, 216);
			this.CliquesDGV.Margin = new System.Windows.Forms.Padding(4);
			this.CliquesDGV.Name = "CliquesDGV";
			this.CliquesDGV.ReadOnly = true;
			this.CliquesDGV.Size = new System.Drawing.Size(1131, 302);
			this.CliquesDGV.TabIndex = 19;
			this.CliquesDGV.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.CliquesDGV_ColumnAdded);
			// 
			// buttonSearchFasta
			// 
			this.buttonSearchFasta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSearchFasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.buttonSearchFasta.Location = new System.Drawing.Point(965, 25);
			this.buttonSearchFasta.Margin = new System.Windows.Forms.Padding(4);
			this.buttonSearchFasta.Name = "buttonSearchFasta";
			this.buttonSearchFasta.Size = new System.Drawing.Size(193, 42);
			this.buttonSearchFasta.TabIndex = 17;
			this.buttonSearchFasta.Text = "Search cliques";
			this.buttonSearchFasta.UseVisualStyleBackColor = true;
			this.buttonSearchFasta.Click += new System.EventHandler(this.buttonSearchFasta_Click);
			// 
			// geneLbl
			// 
			this.geneLbl.AutoSize = true;
			this.geneLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.geneLbl.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.geneLbl.Location = new System.Drawing.Point(26, 158);
			this.geneLbl.MaximumSize = new System.Drawing.Size(1133, 0);
			this.geneLbl.Name = "geneLbl";
			this.geneLbl.Size = new System.Drawing.Size(36, 20);
			this.geneLbl.TabIndex = 21;
			this.geneLbl.Text = "Info";
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1184, 546);
			this.Controls.Add(this.geneLbl);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textBoxMaxCliques);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBoxMaxCliqueSize);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBoxTreshold);
			this.Controls.Add(this.CliquesDGV);
			this.Controls.Add(this.buttonSearchFasta);
			this.Name = "Main";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.CliquesDGV)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBoxMaxCliques;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxMaxCliqueSize;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxTreshold;
		private System.Windows.Forms.DataGridView CliquesDGV;
		private System.Windows.Forms.Button buttonSearchFasta;
		private System.Windows.Forms.Label geneLbl;
	}
}

