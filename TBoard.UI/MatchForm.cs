using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TBoard.UI
{
    public partial class MatchForm : Form
    {
        public MatchForm()
        {
            InitializeComponent();

            this.BackgroundImage = Properties.Resources.PlainBackground;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.KeyUp += MatchForm_KeyUp;

            //mediaPlayer1.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(player_PlayStateChange);
            mediaPlayer1.uiMode = "none";
            mediaPlayer1.settings.setMode("loop", true);
            //mediaPlayer2.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(player_PlayStateChange);
            mediaPlayer2.uiMode = "none";
            mediaPlayer2.settings.setMode("loop", true);

            this.Shown += delegate
            {
                Label lblStage = new Label() 
                {
                    AutoSize = false,
                    BackColor = Color.Transparent,
                    ForeColor = Color.Red,
                    Font = new System.Drawing.Font(this.Font.FontFamily, 30.0F, FontStyle.Bold),
                    Location = new Point(this.Width / 3, 0),
                    Size = new System.Drawing.Size(this.Width / 3, this.Height / 18),
                    TextAlign = ContentAlignment.TopCenter
                };
                this.Controls.Add(lblStage);

                Label lblP1 = new Label()
                {
                    AutoSize = false,
                    BackColor = Color.Transparent,
                    ForeColor = Color.White,
                    Font = new System.Drawing.Font(this.Font.FontFamily, 30.0F, FontStyle.Bold),
                    Location = new Point(this.Width / 9, this.Height / 18),
                    Size = new System.Drawing.Size(this.Width / 3, this.Height / 18),
                    TextAlign = ContentAlignment.TopCenter
                };
                this.Controls.Add(lblP1);

                Label lblP2 = new Label()
                {
                    AutoSize = false,
                    BackColor = Color.Transparent,
                    ForeColor = Color.White,
                    Font = new System.Drawing.Font(this.Font.FontFamily, 30.0F, FontStyle.Bold),
                    Location = new Point(5 * this.Width / 9, this.Height / 18),
                    Size = new System.Drawing.Size(this.Width / 3, this.Height / 18),
                    TextAlign = ContentAlignment.TopCenter
                };
                this.Controls.Add(lblP2);

                Label lblVS = new Label()
                {
                    AutoSize = false,
                    BackColor = Color.Transparent,
                    ForeColor = Color.OrangeRed,
                    Font = new System.Drawing.Font("AR BLANCA", 65.0F, FontStyle.Bold),//orignal: 70.0F
                    Location = new Point(4 * this.Width / 9, this.Height / 18),
                    Size = new System.Drawing.Size(this.Width / 9, 7 * this.Height / 9),
                    Text = "VS",
                    TextAlign = ContentAlignment.MiddleCenter
                };
                this.Controls.Add(lblVS);

                mediaPlayer1.Size = mediaPlayer2.Size = new System.Drawing.Size(this.Width / 3, 15 * this.Height / 18);
                mediaPlayer1.Location = new Point(this.Width / 9, this.Height / 9);
                mediaPlayer2.Location = new Point(5 * this.Width / 9, this.Height / 9);
                if (Player1 != null && Player2 != null)
                {
                    if (Stage != null && Stage.Name != null)
                        lblStage.Text = Stage.Name;
                    if (Player1.Name != null)
                        lblP1.Text = Player1.Name;
                    if (Player2.Name != null)
                        lblP2.Text = Player2.Name;

                    if (Player1.Clip != null)
                        mediaPlayer1.URL = Player1.Clip;
                    //mediaPlayer1.Ctlcontrols.play(); //no need to call play, autostart is true

                    if (Player2.Clip != null)
                        mediaPlayer2.URL = Player2.Clip;
                    //mediaPlayer2.Ctlcontrols.play();
                }
            };
        }

        void MatchForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4) 
            {
                this.Hide();
            }
        }

        private void player_MediaError(object pMediaObject)
        {
            //
        }

        private void player_PlayStateChange(int NewState)
        {
            //if ((WMPLib.WMPPlayState)NewState == WMPLib.WMPPlayState.wmppsStopped)
            //{
            //    //
            //}
        }

        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Stage Stage { get; set; }

        private void mediaPlayer_MediaError(object sender, AxWMPLib._WMPOCXEvents_MediaErrorEvent e)
        {
            //handle error here
        }
    }
}
