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
    public partial class MatchForm2 : Form //**************************PAINT EVERYTHING, DONT USE ANY CONTROL
    {
        MyUserControl matchBoard;
        public MatchForm2()
        {
            InitializeComponent();

            this.BackgroundImage = TournamentState.GetSingleton().MatchBoardBackImage; 
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            matchBoard = new MyUserControl() 
            {
                Dock = DockStyle.Fill,
            };
            this.Controls.Add(matchBoard);

            this.KeyUp += MatchForm_KeyUp;

            this.Shown += MatchForm2_Shown;
        }
        void MatchForm2_Shown(object sender, EventArgs e)
        {
            if (Player1 != null && Player2 != null)
            {
                TournamentState state = TournamentState.GetSingleton();
                Graphics g = matchBoard.CreatePermanentGraphics();

                //draw background
                g.DrawImage(state.MatchBoardBackImage, matchBoard.ClientRectangle);

                var font30 = new System.Drawing.Font(this.Font.FontFamily, 30.0F, FontStyle.Bold);
                //write stage
                if (Stage != null && Stage.Name != null) 
                {
                    var width = g.MeasureString(Stage.Name, font30).Width;
                    g.DrawString(Stage.Name, font30, Brushes.White, new PointF((matchBoard.Width - width) / 2, 5));
                }

                //write P1 name
                if (Player1.Name != null) 
                {
                    var width = g.MeasureString(Player1.Name, font30).Width;
                    g.DrawString(Player1.Name, font30, Brushes.White, new PointF(2 * matchBoard.Width / 9, this.Height / 18));
                }
                //write P2 name
                if (Player2.Name != null)
                {
                    var width = g.MeasureString(Player2.Name, font30).Width;
                    g.DrawString(Player2.Name, font30, Brushes.White, new PointF(6 * matchBoard.Width / 9, this.Height / 18));
                }

                //write VS
                var fontVS = new System.Drawing.Font("AR BLANCA", 65.0F, FontStyle.Bold);
                var widthVS = g.MeasureString("VS", fontVS).Width;
                var heightVS = g.MeasureString("VS", fontVS).Height;
                g.DrawString("VS", fontVS, Brushes.OrangeRed, new PointF((matchBoard.Width - widthVS) / 2, (matchBoard.Height - heightVS) / 2));

                //draw P1 avatar
                if (Player1.Avatar != null)
                {
                    RectangleF rect = new RectangleF(new PointF(matchBoard.Width / 9, 2 * this.Height / 18),
                        new SizeF(3 * matchBoard.Width / 9, this.Height - (2 * this.Height / 18)));
                    Bitmap bitmap = new Bitmap(Player1.Avatar, (int)rect.Width, (int)rect.Height);
                    g.DrawImage(bitmap, rect);
                }
                //draw P2 avatar
                if (Player2.Avatar != null)
                {
                    RectangleF rect = new RectangleF(new PointF(5 * matchBoard.Width / 9, 2 * this.Height / 18),
                        new SizeF(3 * matchBoard.Width / 9, this.Height - (2 * this.Height / 18)));
                    Bitmap bitmap = new Bitmap(Player2.Avatar, (int)rect.Width, (int)rect.Height);
                    g.DrawImage(bitmap, rect);
                }

                //draw foreground
                if (state.MatchBoardForeImage != null)
                    g.DrawImage(state.MatchBoardForeImage, matchBoard.ClientRectangle);

                //history
                var font20 = new System.Drawing.Font(this.Font.FontFamily, 20.0F, FontStyle.Bold);
                int deltaY = 30;

                //draw P1 history
                int rows = Player1.Wins.Count + 1;
                float currY = this.Height - (rows * deltaY);
                g.DrawString(string.Format("WINS: {0}", Player1.Wins.Count), font20, Brushes.Blue,
                    new PointF(matchBoard.Width / 9, currY));
                var wins = Player1.Wins.ToArray();
                foreach (Player pl in wins)
                {
                    currY += deltaY;
                    g.DrawString(pl.Name, font20, Brushes.Orange, new PointF((matchBoard.Width / 9) + 20, currY));
                }

                //draw P2 history
                rows = Player2.Wins.Count + 1;
                currY = this.Height - (rows * deltaY);
                g.DrawString(string.Format("WINS: {0}", Player2.Wins.Count), font20, Brushes.Blue,
                    new PointF(5 * matchBoard.Width / 9, currY));
                wins = Player2.Wins.ToArray();
                foreach (Player pl in wins)
                {
                    currY += deltaY;
                    g.DrawString(pl.Name, font20, Brushes.Orange, new PointF((5 * matchBoard.Width / 9) + 20, currY));
                }

                g.Dispose();
            }
        }
        void MatchForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                this.Hide();
            }
        }

        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Stage Stage { get; set; }
    }
}
