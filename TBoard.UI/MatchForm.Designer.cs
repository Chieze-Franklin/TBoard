namespace TBoard.UI
{
    partial class MatchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatchForm));
            this.mediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.mediaPlayer2 = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.mediaPlayer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaPlayer2)).BeginInit();
            this.SuspendLayout();
            // 
            // mediaPlayer1
            // 
            this.mediaPlayer1.Enabled = true;
            this.mediaPlayer1.Location = new System.Drawing.Point(65, 61);
            this.mediaPlayer1.Name = "mediaPlayer1";
            this.mediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("mediaPlayer1.OcxState")));
            this.mediaPlayer1.Size = new System.Drawing.Size(233, 201);
            this.mediaPlayer1.TabIndex = 0;
            this.mediaPlayer1.MediaError += new AxWMPLib._WMPOCXEvents_MediaErrorEventHandler(this.mediaPlayer_MediaError);
            // 
            // mediaPlayer2
            // 
            this.mediaPlayer2.Enabled = true;
            this.mediaPlayer2.Location = new System.Drawing.Point(359, 61);
            this.mediaPlayer2.Name = "mediaPlayer2";
            this.mediaPlayer2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("mediaPlayer2.OcxState")));
            this.mediaPlayer2.Size = new System.Drawing.Size(233, 201);
            this.mediaPlayer2.TabIndex = 1;
            this.mediaPlayer2.MediaError += new AxWMPLib._WMPOCXEvents_MediaErrorEventHandler(this.mediaPlayer_MediaError);
            // 
            // MatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 453);
            this.Controls.Add(this.mediaPlayer2);
            this.Controls.Add(this.mediaPlayer1);
            this.Name = "MatchForm";
            this.Text = "MatchForm";
            ((System.ComponentModel.ISupportInitialize)(this.mediaPlayer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaPlayer2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer mediaPlayer1;
        private AxWMPLib.AxWindowsMediaPlayer mediaPlayer2;

    }
}