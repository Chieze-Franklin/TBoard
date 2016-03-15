using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TBoard.UI
{
    public class Coin : MyUserControl
    {
        public Stack<Path> Paths = new Stack<Path>();
        CustomToolTip tooltip = new CustomToolTip();

        public Coin(Player player, int index) 
        {
            this.Index = index;
            this.Player = player;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = this.Player.NormalFace;

            this.BackColor = Color.Transparent;

            this.DoubleClick += delegate 
            {
                if (!IgnoreClick)
                    MoveForward();
            };
            this.MouseClick += delegate (object sender, MouseEventArgs e)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right && !IgnoreClick)
                    MoveBack();
            };
            this.MouseEnter += delegate 
            {
                if (this.Player != null)
                    tooltip.Show(this.Player.Name, this, 6000);
            };
            this.MouseLeave += delegate
            {
                tooltip.Hide(this);
            };
        }

        public void MoveForward() 
        {
            if (Spot != null && Spot.Next != null && Spot.NextPath != null && Spot.Twin != null && Spot.Twin.Coin != null) 
            {
                Spot prvSpot = Spot;
                Path nxtPath = Spot.NextPath;

                prvSpot.DrawHappyFace();
                if (prvSpot.Twin != null)
                {
                    if (prvSpot.Twin.Coin != null)
                    {
                        prvSpot.Twin.Coin.IgnoreClick = true;
                        //prvSpot.Twin.Coin.Enabled = false;
                        //prvSpot.Twin.Coin.Visible = false;
                        prvSpot.Twin.Coin.BackgroundImage = prvSpot.Twin.Coin.Player.SadFace;
                    }
                    prvSpot.Twin.DrawSadFace();
                }
                nxtPath.DrawActive();
                //forwardTimer.Enabled = true;
                //move coin along the path
                foreach (var route in prvSpot.NextPath.Routes)
                {
                    for (int i = 0; i < (route.Distance + (prvSpot.Width / 2)); i++)
                    {
                        if (route.Axis == RouteAxis.X)
                        {
                            int x = this.Location.X + 1;
                            this.Location = new System.Drawing.Point(x, this.Location.Y);
                        }
                        else if (route.Axis == RouteAxis.MinusX)
                        {
                            int x = this.Location.X - 1;
                            this.Location = new System.Drawing.Point(x, this.Location.Y);
                        }
                        else if (route.Axis == RouteAxis.Y)
                        {
                            int y = this.Location.Y + 1;
                            this.Location = new System.Drawing.Point(this.Location.X, y);
                        }
                        else if (route.Axis == RouteAxis.MinusY)
                        {
                            int y = this.Location.Y - 1;
                            this.Location = new System.Drawing.Point(this.Location.X, y);
                        }

                        //Thread.Sleep(1);
                    }

                    //record action
                    TournamentState.GetSingleton().CoinActions.Add(new CoinAction(Index, Action.CoinMoveForward));
                }

                //add path to Paths
                Paths.Push(nxtPath);

                //send coin forward
                prvSpot.SendCoin();
            }
        }
        public void MoveBack() 
        {
            //see if there is a path to navigate
            if (Paths.Count > 0) 
            {
                Spot prvSpot = Spot;
                prvSpot.DrawNormal();

                var path = Paths.Pop();

                //get the routes and reverse them
                var routes = path.Routes.ToArray();
                routes = routes.Reverse().ToArray();

                path.DrawNormal();
                foreach (var route in routes)
                {
                    for (int i = 0; i < (route.Distance + (prvSpot.Width / 2)); i++)
                    {
                        if (route.Axis == RouteAxis.X)
                        {
                            int x = this.Location.X - 1;
                            this.Location = new System.Drawing.Point(x, this.Location.Y);
                        }
                        else if (route.Axis == RouteAxis.MinusX)
                        {
                            int x = this.Location.X + 1;
                            this.Location = new System.Drawing.Point(x, this.Location.Y);
                        }
                        else if (route.Axis == RouteAxis.Y)
                        {
                            int y = this.Location.Y - 1;
                            this.Location = new System.Drawing.Point(this.Location.X, y);
                        }
                        else if (route.Axis == RouteAxis.MinusY)
                        {
                            int y = this.Location.Y + 1;
                            this.Location = new System.Drawing.Point(this.Location.X, y);
                        }

                        //Thread.Sleep(1);
                    }
                }

                path.From.ReceiveCoinBack(this);

                //record action
                TournamentState.GetSingleton().CoinActions.Add(new CoinAction(Index, Action.CoinMoveBack));
            }
        }

        public bool IgnoreClick { get; set; }
        public int Index { get; set; }
        public Player Player { get; private set; }
        public Spot Spot { get; set; }
    }

    internal class CustomToolTip : ToolTip //rough work
    {
        public CustomToolTip()
        {
            this.OwnerDraw = true;
            this.Popup += new PopupEventHandler(this.OnPopup);
            this.Draw += new DrawToolTipEventHandler(this.OnDraw);
        }

        private void OnPopup(object sender, PopupEventArgs e) // use this event to set the size of the tool tip
        {
            Coin coin = (Coin)e.AssociatedControl;
            Player player = coin.Player;
            int height = 150 + (player.Wins.Count * 16) + (player.Losses.Count * 16) + 32;
            e.ToolTipSize = new Size(200, height);
        }

        private void OnDraw(object sender, DrawToolTipEventArgs e) // use this event to customise the tool tip
        {
            Graphics g = e.Graphics;

            //fill rect
            System.Drawing.Drawing2D.LinearGradientBrush b = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds,
                Color.GreenYellow, Color.MintCream, 45f);
            g.FillRectangle(b, e.Bounds);

            //draw border
            g.DrawRectangle(new Pen(Brushes.Blue, 1), new Rectangle(e.Bounds.X, e.Bounds.Y,
                e.Bounds.Width - 1, e.Bounds.Height - 1));

            Coin coinSender = (Coin)e.AssociatedControl;
            Player player = coinSender.Player;

            //draw face
            int faceWidth = e.Bounds.Width / 2;
            float currentY = 10;
            g.DrawImage(new Bitmap(player.NormalFace, faceWidth, faceWidth), e.Bounds.Width / 4, currentY);

            //draw name
            Font nameFont = new Font(e.Font.FontFamily.Name, 14.0F, FontStyle.Bold);
            float nameWidth = g.MeasureString(e.ToolTipText, nameFont).Width;
            currentY += faceWidth;
            float nameX = (e.Bounds.Width - nameWidth) / 2;
            if (nameX < 0)
                nameX = 0;
            g.DrawString(e.ToolTipText, nameFont, Brushes.Black, new PointF(nameX, currentY));

            //draw line
            currentY += g.MeasureString(e.ToolTipText, nameFont).Height;
            Pen p = new Pen(Brushes.Black, 3.0F);
            g.DrawLine(p, new PointF(0, currentY), new PointF(e.Bounds.Width, currentY));

            //write WINS
            currentY += 3;
            g.DrawString(string.Format("WINS: {0}", player.Wins.Count), e.Font, Brushes.Blue, new PointF(2, currentY));
            var wins = player.Wins.ToArray();
            foreach (Player pl in wins)
            {
                currentY += 16;
                g.DrawString(pl.Name, e.Font, Brushes.Blue, new PointF(10, currentY));
            }

            //write LOSSES
            currentY += 16;
            g.DrawString(string.Format("LOSSES: {0}", player.Losses.Count), e.Font, Brushes.Red, new PointF(2, currentY));
            var losses = player.Losses.ToArray();
            foreach (Player pl in losses)
            {
                currentY += 16;
                g.DrawString(pl.Name, e.Font, Brushes.Red, new PointF(10, currentY));
            }

            b.Dispose();
        }
    }
}
