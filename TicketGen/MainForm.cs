using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace TicketGen
{
    public partial class MainForm : Form
    {
        int w = 800, h = 1000, x = 0, y = 0;

        List<Bitmap> bitmapsToPrint;
        PrintDocument printDoc;
        PrintPreviewDialog printPreviewDialog;
        PrintDialog printDialog;

        Dictionary<string, string> tickets;

        public MainForm()
        {
            InitializeComponent();

            bitmapsToPrint = new List<Bitmap>();
            //printDoc
            printDoc = new PrintDocument()
            {
                //DocumentName = ""
            };
            printDoc.PrintPage += printDoc_PrintPage;

            //printPreviewDialog
            printPreviewDialog = new PrintPreviewDialog()
            {
                Document = printDoc
            };

            //printDialog
            printDialog = new PrintDialog()
            {
                Document = printDoc
            };

            tickets = new Dictionary<string, string>();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPages();
        }
        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            PrintPages(true);
        }
        int i = 0;
        void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (bitmapsToPrint.Count > 0)
            {
                Bitmap bitmap = bitmapsToPrint[0];
                bitmapsToPrint.RemoveAt(0);

                e.Graphics.DrawImage(bitmap, 0, 0);
                bitmap.Save(Application.StartupPath + "\\images\\img-" +
                    DateTime.Now.ToString().Replace(" ", "").Replace("/", "").Replace(":", "") +
                    "-" + ++i + ".jpg");

                if (tickets.Count > 0)
                {
                    //load tickets log file
                    XmlDocument logDocument = new XmlDocument();
                    string logDir = Application.StartupPath + "\\log";
                    string logPath = Application.StartupPath + "\\log\\tickets.xml";
                    if (File.Exists(logPath))
                    {
                        try
                        {
                            logDocument.Load(logPath);
                        }
                        catch
                        {
                            var root = logDocument.CreateElement("Tickets");
                            logDocument.AppendChild(root);
                        }
                    }
                    else
                    {
                        var root = logDocument.CreateElement("Tickets");
                        logDocument.AppendChild(root);
                    }

                    //store
                    foreach (string key in tickets.Keys)
                    {
                        var currentNode = logDocument.CreateElement("Ticket");
                        currentNode.SetAttribute("TicketNumber", key);
                        currentNode.SetAttribute("Vendor", tickets[key]);
                        logDocument.LastChild.AppendChild(currentNode);
                    }

                    //save
                    if (!File.Exists(logPath))
                    {
                        if (!Directory.Exists(logDir))
                            Directory.CreateDirectory(logDir);

                        FileStream fs = null;
                        try
                        {
                            fs = new FileStream(logPath, FileMode.Create, FileAccess.Write, FileShare.None);
                            byte[] byteText = System.Text.Encoding.ASCII.GetBytes("");
                            fs.Write(byteText, 0, byteText.Length);
                        }
                        catch
                        {
                            throw;
                        }
                        finally
                        {
                            if (fs != null)
                                fs.Close();
                        }
                    }
                    logDocument.Save(logPath);

                    tickets.Clear();
                }

                if (bitmapsToPrint.Count > 0)
                    e.HasMorePages = true;
                else
                    e.HasMorePages = false;
            }
            else
            { }
        }
        void PrintPages(bool preview = false, bool printBack = false)
        {
            try
            {
                int pageCount = (int)nudPages.Value;
                if (pageCount > 0)
                {
                    bitmapsToPrint = new List<Bitmap>(pageCount);
                    for (int index = 0; index < pageCount; index++)
                    {
                        Bitmap bitmap = null;

                        if (printBack)
                            bitmap = DrawBackPage();
                        else
                            bitmap = DrawFrontPage();

                        bitmapsToPrint.Add(bitmap);
                    }
                }

                //printDoc.DocumentName = Path.GetFileName(FilePath);
                if (preview)
                {
                    if (printPreviewDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        printDoc.Print();
                    }
                    tickets.Clear();
                }
                else
                {
                    if (printDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        printDoc.Print();
                    }
                    tickets.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        Bitmap DrawFrontPage()
        {
            Bitmap bitmap = new Bitmap(w + x, h + y, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bitmap);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, 1000, 1200);

            Bitmap ticketBitmap = new Bitmap(Properties.Resources.PlayerTicket, new Size(w + x, h / 5));
            
            //Graphics g2 = Graphics.FromImage(ticketBitmap);
            ////vendor
            //if (cboxVendors.Text != null)
            //    g2.DrawString(cboxVendors.Text, new Font(this.Font.FontFamily, 16.0F, FontStyle.Bold), Brushes.Black,
            //    new PointF(x + (11.0F * ticketBitmap.Width / 20.0F), y + (6.0F * ticketBitmap.Height / 20.0F)));
            //g2.Dispose();

            for (int index = 0; index < 5; index++) 
            {
                int top = y + (index * ticketBitmap.Height);

                g.DrawImage(ticketBitmap, new Point(x, top));
                g.DrawLine(new Pen(Brushes.Black, 1.0F), new Point(x, y + (index * ticketBitmap.Height)),
                    new Point(x + ticketBitmap.Width, top));

                //vendor
                if (cboxVendors.Text != null)
                    g.DrawString(cboxVendors.Text, new Font(this.Font.FontFamily, 16.0F, FontStyle.Bold), Brushes.Black,
                    new PointF(x + (11.0F * ticketBitmap.Width / 20.0F), top + (7.8F * ticketBitmap.Height / 20.0F)));//6.0

                //generate ticket number
                string dateTime = DateTime.Now.ToString() + DateTime.Now.Millisecond + ":";
                string ticketNum = dateTime
                    .Replace(" ", "").Replace("201", "")
                    .Replace("AM", "n").Replace("PM", "m")
                    .Replace("31/", "q").Replace("30/", "w").Replace("29/", "e").Replace("28/", "r").Replace("27/", "t")
                    .Replace("26/", "y").Replace("25/", "u").Replace("24/", "p").Replace("23/", "a").Replace("22/", "d")
                    .Replace("21/", "f").Replace("20/", "h").Replace("19/", "j").Replace("18/", "k").Replace("17/", "l")
                    .Replace("16/", "z").Replace("15/", "x").Replace("14/", "c").Replace("13/", "v")
                    .Replace("12/", "Q").Replace("11/", "W").Replace("10/", "E").Replace("9/", "R").Replace("8/", "T").Replace("7/", "Y")
                    .Replace("6/", "U").Replace("5/", "P").Replace("4/", "A").Replace("3/", "D").Replace("2/", "F").Replace("1/", "H")
                    .Replace("9:", "J").Replace("8:", "K").Replace("7:", "L").Replace("6:", "Z").Replace("5:", "X").Replace("4:", "C")
                    .Replace("3:", "V").Replace("2:", "N").Replace("1:", "M").Replace("0:", "_");
                //slip
                g.DrawString(ticketNum, new Font(this.Font.FontFamily, 10.0F, FontStyle.Bold), Brushes.Black,
                        new PointF(x + (1.5F * ticketBitmap.Width / 20.0F), top + (2.8F * ticketBitmap.Height / 20.0F)));//2.2
                //ticket
                g.DrawString(ticketNum, new Font(this.Font.FontFamily, 16.0F, FontStyle.Bold), Brushes.Black,
                        new PointF(x + (11.0F * ticketBitmap.Width / 20.0F), top + (12.2F * ticketBitmap.Height / 20.0F)));//9.5

                //record ticket number and vendor
                tickets.Add(ticketNum, cboxVendors.Text);

                //just to ensure ticket nums (which are based on time) are unique
                System.Threading.Thread.Sleep((int)nudWait.Value);
            }

            g.Dispose();

            return bitmap;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            PrintPages(false, true);
        }
        private void btnPrintBackPreview_Click(object sender, EventArgs e)
        {
            PrintPages(true, true);
        }
        Bitmap DrawBackPage()
        {
            Bitmap bitmap = new Bitmap(w + x, h + y, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bitmap);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, 1000, 1200);

            Bitmap ticketBackBitmap = new Bitmap(Properties.Resources.PlayerTicketBack, new Size(w + x, h / 5));

            for (int index = 0; index < 5; index++)
            {
                int top = y + (index * ticketBackBitmap.Height);

                g.DrawImage(ticketBackBitmap, new Point(x, top));
                g.DrawLine(new Pen(Brushes.Black, 1.0F), new Point(x, y + (index * ticketBackBitmap.Height)),
                    new Point(x + ticketBackBitmap.Width, top));
            }

            g.Dispose();

            return bitmap;
        }
    }
}
