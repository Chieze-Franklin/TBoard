using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TBoard.UI
{
    public partial class MyUserControl : UserControl
    {
        public MyUserControl()
        {
            InitializeComponent();
        }
        public Graphics CreatePermanentGraphics()
        {
            Bitmap bmp = new Bitmap(this.DisplayRectangle.Width,
                    this.DisplayRectangle.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb); //Bitmap(this.Width, this.Height);
            this.BackgroundImage = bmp;
            Graphics g = Graphics.FromImage(bmp);
            return g;
        }
    }
}
