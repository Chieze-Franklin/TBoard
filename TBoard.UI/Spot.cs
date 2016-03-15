using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBoard.UI
{
    public class Spot
    {
        //RectangleF Rectangle;
        Graphics g;
        Pen grayPen = new Pen(Brushes.Gray, 3);
        Pen orangePen = new Pen(Brushes.Orange, 3);
        Pen whitePen = new Pen(Brushes.White, 3);
        Brush orangeBrush = Brushes.Orange;
        Brush whiteBrush = Brushes.White;
        List<Spot> prevSpots = new List<Spot>();

        public Spot(RectangleF r, Graphics g) 
        {
            this.Rectangle = r;
            this.g = g;
        }

        public void DrawActive()
        {
            g.FillEllipse(orangeBrush, Rectangle);
            //g.DrawEllipse(orangePen, Rectangle);
        }
        public void DrawNormal() 
        {
            g.FillEllipse(whiteBrush, Rectangle);
            //g.DrawEllipse(whitePen, Rectangle);
        }

        public void DrawHappyFace()
        {
            if (this.Player != null && this.Player.HappyFace != null)
            {
                g.DrawImage(new Bitmap(this.Player.HappyFace, (int)this.Width, (int)this.Height), new PointF(this.Left, this.Top));
            }
        }
        public void DrawNormalFace()
        {
            if (this.Player != null && this.Player.NormalFace != null) 
            {
                g.DrawImage(new Bitmap(this.Player.NormalFace, (int)this.Width, (int)this.Height), new PointF(this.Left, this.Top));
            }
        }
        public void DrawSadFace()
        {
            if (this.Player != null && this.Player.SadFace != null)
            {
                g.DrawImage(new Bitmap(this.Player.SadFace, (int)this.Width, (int)this.Height), new PointF(this.Left, this.Top));
            }
            g.DrawImage(new Bitmap(Properties.Resources.Cancel, (int)this.Width, (int)this.Height), new PointF(this.Left, this.Top));
        }

        public virtual void ReceiveCoin(Coin coin) 
        {
            coin.Spot = this;
            this.Coin = coin;
            this.Player = coin.Player;
            coin.Size = new Size((int)this.Width, (int)this.Height);
            coin.Location = new Point((int)this.Left, (int)this.Top);
            this.DrawActive();
            //this.DrawNormalFace();
        }
        public void ReceiveCoinBack(Coin coin) 
        {
            if (Next != null)
            {
                Next.Coin = null;
                Next.Player = null;
            }
            coin.Spot = this;
            this.Coin = coin;
            this.Player = coin.Player;
            coin.BackgroundImage = Coin.Player.NormalFace;
            coin.Size = new Size((int)this.Width, (int)this.Height);
            coin.Location = new Point((int)this.Left, (int)this.Top);
            //this.DrawActive(); //redundant, cuz spot is already active
            if (Twin != null)
            {
                Twin.DrawActive();
                if (Twin.Coin != null)
                {
                    Twin.Coin.IgnoreClick = false;
                    //Twin.Coin.Enabled = true;
                    //Twin.Coin.Visible = true;
                    Twin.Coin.BackgroundImage = Twin.Coin.Player.NormalFace;
                }
                //------------------
                this.Player.Wins.Pop();
                Twin.Player.Losses.Pop();
            }
        }
        public void SendCoin()
        {
            if (Coin != null) 
            {
                if (Next != null) 
                {
                    Next.ReceiveCoin(Coin);
                }
                Coin = null;
                this.DrawHappyFace();
            }
            if (Twin != null)
            {
                if (Twin.Coin != null)
                {
                    Twin.Coin.IgnoreClick = true;
                    //Twin.Coin.Enabled = false;
                    //Twin.Coin.Visible = false;
                    Twin.Coin.BackgroundImage = Twin.Coin.Player.SadFace;
                }
                Twin.DrawSadFace();
                //------------
                this.Player.Wins.Push(Twin.Player);
                Twin.Player.Losses.Push(this.Player);
            }
        }

        public void MakeTwins(Spot spot) 
        {
            this.Twin = spot;
            spot.Twin = this;
        }
        public void SetNext(Spot next) 
        {
            this.Next = next;
            if (!next.prevSpots.Contains(this))
                next.prevSpots.Add(this);
            this.NextPath = new Path(this, next, g);
            this.NextPath.DrawNormal();
        }

        public Path NextPath { get; private set; }
        public Spot Next { get; private set; }
        public Spot[] Previous 
        {
            get { return prevSpots.ToArray(); }
        }
        public Spot Twin { get; private set; }
        public Coin Coin { get; private set; }
        public Player Player { get; private set; }
        public Stage Stage { get; set; }

        public Direction Direction { get; set; }

        public RectangleF Rectangle { get; private set; }
        public float Bottom { get { return Rectangle.Bottom; } }
        public float Left { get { return Rectangle.Left; } }
        public float Right { get { return Rectangle.Right; } }
        public float Top { get { return Rectangle.Top; } }
        public float Height { get { return Rectangle.Height; } }
        public float Width { get { return Rectangle.Width; } }
    }

    public class FinalSpot : Spot 
    {
        public FinalSpot(RectangleF r, Graphics g) : base(r, g) { }
        public override void ReceiveCoin(Coin coin)
        {
            base.ReceiveCoin(coin);
            //this.Coin.Visible = false;
            //this.DrawHappyFace();
            this.Coin.BackgroundImage = this.Coin.Player.HappyFace;
        }
    }

    public enum Direction 
    {
        Bottom,
        Left,
        Right,
        Top
    }
}
