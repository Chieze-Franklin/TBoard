using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TBoard.UI
{
    public class ClickablePictureBox : PictureBox
    {
        OpenFileDialog openDialog;

        public ClickablePictureBox()
        {
            //openDialog
            openDialog = new OpenFileDialog();
            openDialog.Filter = "PNG|*.png|JPG|*.jpg|JPEG|*.jpeg|All|*.*";
            openDialog.Multiselect = false;

            //this
            //this.BackgroundImage = Properties.Resources.ProfilePic;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Click += delegate
            {
                try
                {
                    if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        this.BackgroundImage = System.Drawing.Image.FromFile(openDialog.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            };
        }
    }
}
