namespace TicketGen
{
    partial class MainForm
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
            this.cboxVendors = new System.Windows.Forms.ComboBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudPages = new System.Windows.Forms.NumericUpDown();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.nudWait = new System.Windows.Forms.NumericUpDown();
            this.btnPrintBackPreview = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudPages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWait)).BeginInit();
            this.SuspendLayout();
            // 
            // cboxVendors
            // 
            this.cboxVendors.FormattingEnabled = true;
            this.cboxVendors.Location = new System.Drawing.Point(81, 42);
            this.cboxVendors.Name = "cboxVendors";
            this.cboxVendors.Size = new System.Drawing.Size(338, 21);
            this.cboxVendors.TabIndex = 0;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(182, 101);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(96, 23);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "Print...";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Vendor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Pages";
            // 
            // nudPages
            // 
            this.nudPages.Location = new System.Drawing.Point(80, 75);
            this.nudPages.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPages.Name = "nudPages";
            this.nudPages.Size = new System.Drawing.Size(46, 20);
            this.nudPages.TabIndex = 5;
            this.nudPages.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Location = new System.Drawing.Point(40, 101);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(114, 23);
            this.btnPrintPreview.TabIndex = 6;
            this.btnPrintPreview.Text = "Print Preview...";
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(150, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "wait";
            // 
            // nudWait
            // 
            this.nudWait.Location = new System.Drawing.Point(182, 74);
            this.nudWait.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudWait.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudWait.Name = "nudWait";
            this.nudWait.Size = new System.Drawing.Size(46, 20);
            this.nudWait.TabIndex = 8;
            this.nudWait.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnPrintBackPreview
            // 
            this.btnPrintBackPreview.Location = new System.Drawing.Point(40, 130);
            this.btnPrintBackPreview.Name = "btnPrintBackPreview";
            this.btnPrintBackPreview.Size = new System.Drawing.Size(114, 23);
            this.btnPrintBackPreview.TabIndex = 10;
            this.btnPrintBackPreview.Text = "Print Back Preview...";
            this.btnPrintBackPreview.UseVisualStyleBackColor = true;
            this.btnPrintBackPreview.Click += new System.EventHandler(this.btnPrintBackPreview_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(182, 130);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(96, 23);
            this.btnBack.TabIndex = 9;
            this.btnBack.Text = "Print Back...";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 176);
            this.Controls.Add(this.btnPrintBackPreview);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.nudWait);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnPrintPreview);
            this.Controls.Add(this.nudPages);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.cboxVendors);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MainForm";
            this.Text = "Ticket Generator";
            ((System.ComponentModel.ISupportInitialize)(this.nudPages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWait)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboxVendors;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudPages;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudWait;
        private System.Windows.Forms.Button btnPrintBackPreview;
        private System.Windows.Forms.Button btnBack;
    }
}

