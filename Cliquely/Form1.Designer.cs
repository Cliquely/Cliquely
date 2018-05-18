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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.textBoxTreshold = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxFasta
            // 
            this.textBoxFasta.Location = new System.Drawing.Point(12, 12);
            this.textBoxFasta.Name = "textBoxFasta";
            this.textBoxFasta.Size = new System.Drawing.Size(100, 20);
            this.textBoxFasta.TabIndex = 0;
            // 
            // buttonSearchFasta
            // 
            this.buttonSearchFasta.Location = new System.Drawing.Point(118, 10);
            this.buttonSearchFasta.Name = "buttonSearchFasta";
            this.buttonSearchFasta.Size = new System.Drawing.Size(75, 23);
            this.buttonSearchFasta.TabIndex = 1;
            this.buttonSearchFasta.Text = "Search gene";
            this.buttonSearchFasta.UseVisualStyleBackColor = true;
            this.buttonSearchFasta.Click += new System.EventHandler(this.buttonSearchFasta_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(3, 52);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(463, 150);
            this.dataGridView1.TabIndex = 2;
            // 
            // textBoxTreshold
            // 
            this.textBoxTreshold.Location = new System.Drawing.Point(323, 11);
            this.textBoxTreshold.Name = "textBoxTreshold";
            this.textBoxTreshold.Size = new System.Drawing.Size(100, 20);
            this.textBoxTreshold.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 201);
            this.Controls.Add(this.textBoxTreshold);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.buttonSearchFasta);
            this.Controls.Add(this.textBoxFasta);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFasta;
        private System.Windows.Forms.Button buttonSearchFasta;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBoxTreshold;
    }
}

