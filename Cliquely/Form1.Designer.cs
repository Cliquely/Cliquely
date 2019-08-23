namespace Cliquely
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
			this.textBoxFasta = new System.Windows.Forms.TextBox();
			this.buttonSearchFasta = new System.Windows.Forms.Button();
			this.CliquesDGV = new System.Windows.Forms.DataGridView();
			this.textBoxTreshold = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBoxGeneType = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.geneLbl = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxMaxCliqueSize = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textBoxMaxCliques = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.CliquesDGV)).BeginInit();
			this.SuspendLayout();
			// 
			// textBoxFasta
			// 
			this.textBoxFasta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxFasta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxFasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.textBoxFasta.Location = new System.Drawing.Point(27, 62);
			this.textBoxFasta.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxFasta.Multiline = true;
			this.textBoxFasta.Name = "textBoxFasta";
			this.textBoxFasta.Size = new System.Drawing.Size(1130, 169);
			this.textBoxFasta.TabIndex = 0;
			// 
			// buttonSearchFasta
			// 
			this.buttonSearchFasta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSearchFasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.buttonSearchFasta.Location = new System.Drawing.Point(963, 242);
			this.buttonSearchFasta.Margin = new System.Windows.Forms.Padding(4);
			this.buttonSearchFasta.Name = "buttonSearchFasta";
			this.buttonSearchFasta.Size = new System.Drawing.Size(193, 42);
			this.buttonSearchFasta.TabIndex = 5;
			this.buttonSearchFasta.Text = "Search cliques";
			this.buttonSearchFasta.UseVisualStyleBackColor = true;
			this.buttonSearchFasta.Click += new System.EventHandler(this.buttonSearchFasta_Click);
			// 
			// CliquesDGV
			// 
			this.CliquesDGV.AllowUserToAddRows = false;
			this.CliquesDGV.AllowUserToDeleteRows = false;
			this.CliquesDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.CliquesDGV.Location = new System.Drawing.Point(27, 475);
			this.CliquesDGV.Margin = new System.Windows.Forms.Padding(4);
			this.CliquesDGV.Name = "CliquesDGV";
			this.CliquesDGV.ReadOnly = true;
			this.CliquesDGV.Size = new System.Drawing.Size(1131, 302);
			this.CliquesDGV.TabIndex = 10;
			this.CliquesDGV.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.CliquesDGV_ColumnAdded);
			// 
			// textBoxTreshold
			// 
			this.textBoxTreshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.textBoxTreshold.Location = new System.Drawing.Point(235, 251);
			this.textBoxTreshold.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxTreshold.Name = "textBoxTreshold";
			this.textBoxTreshold.Size = new System.Drawing.Size(320, 26);
			this.textBoxTreshold.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label1.Location = new System.Drawing.Point(23, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(217, 25);
			this.label1.TabIndex = 4;
			this.label1.Text = "Fasta format sequence:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label2.Location = new System.Drawing.Point(23, 252);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(108, 25);
			this.label2.TabIndex = 5;
			this.label2.Text = "Probability:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboBoxGeneType
			// 
			this.comboBoxGeneType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxGeneType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.comboBoxGeneType.FormattingEnabled = true;
			this.comboBoxGeneType.Items.AddRange(new object[] {
            "Homology",
            "Orthology"});
			this.comboBoxGeneType.Location = new System.Drawing.Point(235, 362);
			this.comboBoxGeneType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.comboBoxGeneType.Name = "comboBoxGeneType";
			this.comboBoxGeneType.Size = new System.Drawing.Size(320, 28);
			this.comboBoxGeneType.TabIndex = 4;
			this.comboBoxGeneType.Visible = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label3.Location = new System.Drawing.Point(23, 365);
			this.label3.Margin = new System.Windows.Forms.Padding(0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(116, 25);
			this.label3.TabIndex = 8;
			this.label3.Text = "Gene Type:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label3.Visible = false;
			// 
			// geneLbl
			// 
			this.geneLbl.AutoSize = true;
			this.geneLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.geneLbl.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.geneLbl.Location = new System.Drawing.Point(24, 376);
			this.geneLbl.MaximumSize = new System.Drawing.Size(1133, 0);
			this.geneLbl.Name = "geneLbl";
			this.geneLbl.Size = new System.Drawing.Size(36, 20);
			this.geneLbl.TabIndex = 6;
			this.geneLbl.Text = "Info";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label4.Location = new System.Drawing.Point(23, 290);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(161, 25);
			this.label4.TabIndex = 10;
			this.label4.Text = "Max Clique Size:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBoxMaxCliqueSize
			// 
			this.textBoxMaxCliqueSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.textBoxMaxCliqueSize.Location = new System.Drawing.Point(235, 289);
			this.textBoxMaxCliqueSize.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxMaxCliqueSize.Name = "textBoxMaxCliqueSize";
			this.textBoxMaxCliqueSize.Size = new System.Drawing.Size(320, 26);
			this.textBoxMaxCliqueSize.TabIndex = 2;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label5.Location = new System.Drawing.Point(23, 326);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(127, 25);
			this.label5.TabIndex = 12;
			this.label5.Text = "Max Cliques:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBoxMaxCliques
			// 
			this.textBoxMaxCliques.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.textBoxMaxCliques.Location = new System.Drawing.Point(235, 325);
			this.textBoxMaxCliques.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxMaxCliques.Name = "textBoxMaxCliques";
			this.textBoxMaxCliques.Size = new System.Drawing.Size(320, 26);
			this.textBoxMaxCliques.TabIndex = 3;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1184, 801);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textBoxMaxCliques);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBoxMaxCliqueSize);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.comboBoxGeneType);
			this.Controls.Add(this.geneLbl);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxTreshold);
			this.Controls.Add(this.CliquesDGV);
			this.Controls.Add(this.buttonSearchFasta);
			this.Controls.Add(this.textBoxFasta);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MinimumSize = new System.Drawing.Size(1198, 756);
			this.Name = "Form1";
			this.Text = "Cliquely";
			((System.ComponentModel.ISupportInitialize)(this.CliquesDGV)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFasta;
        private System.Windows.Forms.Button buttonSearchFasta;
        private System.Windows.Forms.DataGridView CliquesDGV;
        private System.Windows.Forms.TextBox textBoxTreshold;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboBoxGeneType;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label geneLbl;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxMaxCliqueSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxMaxCliques;
    }
}

