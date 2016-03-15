using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBoard.UI
{
    public class Path
    {
        Graphics g;
        PointF p;
        Pen orangePen = new Pen(Brushes.Orange, 3);
        Pen whitePen = new Pen(Brushes.White, 3);

        public Path(Spot from, Spot to, Graphics g) 
        {
            this.From = from;
            this.To = to;
            this.g = g;
            this.p = new PointF(from.Right, (from.Top + from.Bottom) / 2);
            Route route1 = new Route();
            Route route2 = new Route();

            if (from.Direction == Direction.Bottom || from.Direction == Direction.Top)
            {
                if (from.Direction == Direction.Bottom)
                {
                    this.p = new PointF((from.Left + from.Right) / 2, from.Bottom);

                    route1.Axis = RouteAxis.Y;
                    route1.Distance = ((to.Top + to.Bottom) / 2) - from.Bottom;
                }
                else if (from.Direction == Direction.Top)
                {
                    this.p = new PointF((from.Left + from.Right) / 2, from.Top);

                    route1.Axis = RouteAxis.MinusY;
                    route1.Distance = from.Top - ((to.Top + to.Bottom) / 2);
                }

                if (to.Left >= from.Left)
                {
                    route2.Axis = RouteAxis.X;
                    route2.Distance = to.Left - this.p.X;
                }
                else
                {
                    route2.Axis = RouteAxis.MinusX;
                    route2.Distance = this.p.X - to.Right;
                }
            }
            else if (from.Direction == Direction.Left || from.Direction == Direction.Right)
            {
                if (from.Direction == Direction.Left)
                {
                    this.p = new PointF(from.Left, (from.Top + from.Bottom) / 2);

                    route1.Axis = RouteAxis.MinusX;
                    route1.Distance = from.Left - ((to.Left + to.Right) / 2);
                }
                else if (from.Direction == Direction.Right)
                {
                    this.p = new PointF(from.Right, (from.Top + from.Bottom) / 2);

                    route1.Axis = RouteAxis.X;
                    route1.Distance = ((to.Left + to.Right) / 2) - from.Right;
                }

                if (to.Top >= from.Top)
                {
                    route2.Axis = RouteAxis.Y;
                    route2.Distance = to.Top - this.p.Y;
                }
                else
                {
                    route2.Axis = RouteAxis.MinusY;
                    route2.Distance = this.p.Y - to.Bottom;
                }
            }

            this.Routes.Add(route1);
            this.Routes.Add(route2);
        }

        public void DrawActive()
        {
            PointF currentP = p;
            foreach (var route in Routes)
            {
                if (route.Axis == RouteAxis.X)
                    g.DrawLine(orangePen, currentP.X, currentP.Y, currentP.X += route.Distance, currentP.Y);
                else if (route.Axis == RouteAxis.MinusX)
                {
                    float x = currentP.X;
                    g.DrawLine(orangePen, currentP.X -= route.Distance, currentP.Y, x, currentP.Y);
                }
                else if (route.Axis == RouteAxis.Y)
                    g.DrawLine(orangePen, currentP.X, currentP.Y, currentP.X, currentP.Y += route.Distance);
                else if (route.Axis == RouteAxis.MinusY)
                {
                    float y = currentP.Y;
                    g.DrawLine(orangePen, currentP.X, currentP.Y -= route.Distance, currentP.X, y);
                }
            }
        }
        public void DrawNormal()
        {
            PointF currentP = p;
            foreach (var route in Routes)
            {
                if (route.Axis == RouteAxis.X)
                    g.DrawLine(whitePen, currentP.X, currentP.Y, currentP.X += route.Distance, currentP.Y);
                else if (route.Axis == RouteAxis.MinusX)
                {
                    float x = currentP.X;
                    g.DrawLine(whitePen, currentP.X -= route.Distance, currentP.Y, x, currentP.Y);
                }
                else if (route.Axis == RouteAxis.Y)
                    g.DrawLine(whitePen, currentP.X, currentP.Y, currentP.X, currentP.Y += route.Distance);
                else if (route.Axis == RouteAxis.MinusY)
                {
                    float y = currentP.Y;
                    g.DrawLine(whitePen, currentP.X, currentP.Y -= route.Distance, currentP.X, y);
                }
            }
        }

        public List<Route> Routes = new List<Route>();

        public Spot From { get; set; }
        public Spot To { get; set; }
    }

    public struct Route 
    {
        public RouteAxis Axis { get; set; }
        public float Distance { get; set; }
    }

    public enum RouteAxis 
    {
        X,
        MinusX,
        Y,
        MinusY
    }
}
