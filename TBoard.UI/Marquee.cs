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
    public partial class Marquee : UserControl
    {
        int scrollAmount = 10, position = 0, visImgs, unitImgWidth, SPACE = 5;
        System.Windows.Forms.Timer tmrScroll;
        Image[] imgList;

        public Marquee(Image[] imageList, int visibleImages = 5, int unitImageWidth = 30)
        {
            InitializeComponent();

            tmrScroll = new Timer(this.components);
            tmrScroll.Interval = 200;
            tmrScroll.Tick += tmrScroll_Tick;

            this.imgList = new Image[imageList.Length];
            for (int index = 0; index < imageList.Length; index++)
                this.imgList[index] = new Bitmap(imageList[index], new Size(unitImageWidth, unitImageWidth));
            this.visImgs = visibleImages;
            this.unitImgWidth = unitImageWidth;
            this.Size = new Size((unitImgWidth * visibleImages) + (SPACE * (visibleImages - 1)), unitImgWidth);

            this.Load += Marquee_Load;
        }

        void Marquee_Load(object sender, EventArgs e)
        {
            this.ResizeRedraw = true;
            if (!this.DesignMode) 
            {
                tmrScroll.Enabled = true;
            }
        }
        void tmrScroll_Tick(object sender, EventArgs e)
        {
            if (imgList.Length > visImgs)
            {
                position -= scrollAmount;

                //force refresh
                this.Invalidate();
            }

            ////force refresh
            //this.Invalidate();
        }

        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        {
            // Do nothing.
            // To prevent flicker, we will draw both the background and the text
            // to a buffered image, and draw it to the control all at once.
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            // The following line avoids a design-time error that would
            // otherwise occur when the control is first loaded (but does not yet
            // have a defined size).
            if (e.ClipRectangle.Width == 0)
            {
                return;
            }

            base.OnPaint(e);
            int totalWidth = (unitImgWidth * imgList.Length) + (SPACE * (imgList.Length - 1));
            if (position < -totalWidth)
            {
                // Reset the text to scroll back onto the control.
                position = this.Width;
            }

            // Create the drawing area in memory.
            // Double buffering is used to prevent flicker.
            Bitmap blt = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height);
            Graphics g = Graphics.FromImage(blt);

            g.FillRectangle(new SolidBrush(Color.Black), e.ClipRectangle);
            for (int index = 0; index < imgList.Length; index++)
                g.DrawImage(imgList[index], new PointF(position + (index * unitImgWidth) + SPACE, 0));

            // Render the finished image on the form.
            e.Graphics.DrawImageUnscaled(blt, 0, 0);

            g.Dispose();
        }
    }
}
