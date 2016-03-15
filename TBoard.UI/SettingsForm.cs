using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TBoard.UI
{
    public partial class SettingsForm : Form
    {
        DialogResult result = DialogResult.Cancel;
        FolderBrowserDialog folderDialog = new FolderBrowserDialog();
        OpenFileDialog fileDialog;

        Panel panorama, statusBar;
        HintedTextBox
            txtTournamentName, txtPlayersFolder, txtSponsorsFolder, txtTBoardImage, txtMatchBoardBackImage, txtMatchBoardForeImage;
        ComboBox cbNumOfPlayers;
        MyUserControl tournamentBoardImage, matchBoardImage;
        Button btnDone, btnCancel;

        TournamentState state;

        public SettingsForm(TournamentState state)
        {
            InitializeComponent();

            this.state = state;

            fileDialog = new OpenFileDialog() 
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "PNG|*.png|JPG|*.jpg;*.jpeg|All|*.*",
                Multiselect = false,
            };

            int frameHeight = 300;
            int frameWidth = 600;
            Color DarkColor = Color.FromArgb(30, 144, 255);
            Color LightColor = Color.White;
            Color CancelColor = Color.FromArgb(255, 144, 30);

            this.Height = frameHeight + 65;
            this.Width = frameWidth;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Settings";

            //------------------
            int x1 = frameWidth, x2 = frameWidth * 2;

            //-----------------------------------------
            statusBar = new Panel() 
            {
                BackColor = DarkColor,
                Dock = DockStyle.Bottom,
                Height = 26,
            };
            this.Controls.Add(statusBar);

            //-----------------------------------------
            panorama = new Panel()
            {
                BackColor = LightColor,
                Height = frameHeight,
                Width = frameWidth * 3,
            };
            this.Controls.Add(panorama);

            //------------------frame 1-------------------------
            //--------------------------------------------------
            Label lblBasicInfo = new Label()
            {
                AutoSize = false,
                BackColor = DarkColor,
                ForeColor = LightColor,
                Font = new System.Drawing.Font(this.Font.FontFamily, 16.0F, FontStyle.Regular),
                Size = new System.Drawing.Size(frameWidth / 4, frameHeight),
                Text = "Enter basic game information",
                TextAlign = ContentAlignment.TopLeft,
            };
            panorama.Controls.Add(lblBasicInfo);

            txtTournamentName = new HintedTextBox("enter tournament name")
            {
                BorderStyle = System.Windows.Forms.BorderStyle.None,
                Font = new System.Drawing.Font(this.Font.FontFamily, 11.0F),
                Width = (frameWidth * 3 / 4) - 40
            };
            txtTournamentName.Location = new Point(lblBasicInfo.Right + 10, 10);
            panorama.Controls.Add(txtTournamentName);
            UnderlineFor lineForTName = new UnderlineFor(txtTournamentName, DarkColor, SystemColors.GrayText)
            {
                BackColor = DarkColor,
            };
            lineForTName.Location = new Point(txtTournamentName.Left, txtTournamentName.Bottom);
            panorama.Controls.Add(lineForTName);

            Label lblNumOfPlayers = new Label()
            {
                AutoSize = false,
                Font = txtTournamentName.Font,
                Text = "choose number of players",
                //TextAlign = ContentAlignment.TopLeft,
                Width = txtTournamentName.Width * 7 / 8
            };
            lblNumOfPlayers.Location = new Point(txtTournamentName.Left, txtTournamentName.Bottom + 10);
            panorama.Controls.Add(lblNumOfPlayers);
            cbNumOfPlayers = new ComboBox() 
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = txtTournamentName.Font,
                Width = txtTournamentName.Width * 1 / 8
            };
            cbNumOfPlayers.Items.Add(2);
            cbNumOfPlayers.Items.Add(4);
            cbNumOfPlayers.Items.Add(8);
            cbNumOfPlayers.Items.Add(16);
            cbNumOfPlayers.Items.Add(32);
            cbNumOfPlayers.SelectedIndex = 3;
            cbNumOfPlayers.Location = new Point(lblNumOfPlayers.Right, lblNumOfPlayers.Top);
            panorama.Controls.Add(cbNumOfPlayers);

            txtPlayersFolder = new HintedTextBox("enter players' info folder")
            {
                BorderStyle = txtTournamentName.BorderStyle,
                Font = txtTournamentName.Font,
                Width = lblNumOfPlayers.Width
            };
            txtPlayersFolder.Location = new Point(txtTournamentName.Left, lblNumOfPlayers.Bottom + 10);
            panorama.Controls.Add(txtPlayersFolder);
            UnderlineFor lineForPlyrsFldr = new UnderlineFor(txtPlayersFolder, DarkColor, SystemColors.GrayText)
            {
                BackColor = SystemColors.GrayText,
            };
            lineForPlyrsFldr.Location = new Point(txtPlayersFolder.Left, txtPlayersFolder.Bottom);
            panorama.Controls.Add(lineForPlyrsFldr);
            Button btnPlayersFolder = new Button()
            {
                BackColor = LightColor,
                ForeColor = DarkColor,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font(this.Font.FontFamily, 15.0F),
                Text = "...",//Convert.ToChar(2026).ToString(),//"...",
                TextAlign = ContentAlignment.TopCenter,
                Height = 30,
                Width = cbNumOfPlayers.Width,
            };
            btnPlayersFolder.Location = new Point(txtPlayersFolder.Right, txtPlayersFolder.Top);
            btnPlayersFolder.Click += delegate 
            {
                if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
                {
                    txtPlayersFolder.HideHint();
                    txtPlayersFolder.Text = folderDialog.SelectedPath;
                }
            };
            panorama.Controls.Add(btnPlayersFolder);

            txtSponsorsFolder = new HintedTextBox("enter sponsors' info folder")
            {
                BorderStyle = txtTournamentName.BorderStyle,
                Font = txtTournamentName.Font,
                Width = lblNumOfPlayers.Width
            };
            txtSponsorsFolder.Location = new Point(txtTournamentName.Left, btnPlayersFolder.Bottom + 10);
            panorama.Controls.Add(txtSponsorsFolder);
            UnderlineFor lineForSpnsrsFldr = new UnderlineFor(txtSponsorsFolder, DarkColor, SystemColors.GrayText)
            {
                BackColor = SystemColors.GrayText,
            };
            lineForSpnsrsFldr.Location = new Point(txtSponsorsFolder.Left, txtSponsorsFolder.Bottom);
            panorama.Controls.Add(lineForSpnsrsFldr);
            Button btnSponsorsFolder = new Button()
            {
                BackColor = btnPlayersFolder.BackColor,
                ForeColor = btnPlayersFolder.ForeColor,
                FlatStyle = btnPlayersFolder.FlatStyle,
                Font = btnPlayersFolder.Font,
                Text = btnPlayersFolder.Text,
                TextAlign = btnPlayersFolder.TextAlign,
                Height = btnPlayersFolder.Height,
                Width = cbNumOfPlayers.Width,
            };
            btnSponsorsFolder.Location = new Point(txtSponsorsFolder.Right, txtSponsorsFolder.Top);
            btnSponsorsFolder.Click += delegate
            {
                if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtSponsorsFolder.HideHint();
                    txtSponsorsFolder.Text = folderDialog.SelectedPath;
                }
            };
            panorama.Controls.Add(btnSponsorsFolder);

            //------------------frame 2-------------------------
            //--------------------------------------------------
            Label lblTournamentBoard = new Label()
            {
                AutoSize = false,
                BackColor = DarkColor,
                ForeColor = LightColor,
                Font = new System.Drawing.Font(this.Font.FontFamily, 16.0F, FontStyle.Regular),
                Size = new System.Drawing.Size(frameWidth / 4, frameHeight),
                Text = "Choose the background image for the tournament board",
                TextAlign = ContentAlignment.TopLeft,
            };
            lblTournamentBoard.Location = new Point(x1, 0);
            panorama.Controls.Add(lblTournamentBoard);

            txtTBoardImage = new HintedTextBox("enter tournament board image")
            {
                BorderStyle = txtTournamentName.BorderStyle,
                Font = txtTournamentName.Font,
                Width = lblNumOfPlayers.Width
            };
            txtTBoardImage.Location = new Point(lblTournamentBoard.Right + 10, 10);
            txtTBoardImage.KeyUp += (s, e) => 
            {
                if (e.KeyCode == Keys.Enter) 
                {
                    DrawTournamentBoard();
                }
            };
            panorama.Controls.Add(txtTBoardImage);
            UnderlineFor lineForTBoardImg = new UnderlineFor(txtTBoardImage, DarkColor, SystemColors.GrayText)
            {
                BackColor = SystemColors.GrayText,
            };
            lineForTBoardImg.Location = new Point(txtTBoardImage.Left, txtTBoardImage.Bottom);
            panorama.Controls.Add(lineForTBoardImg);
            Button btnTBoardImage = new Button()
            {
                BackColor = btnPlayersFolder.BackColor,
                ForeColor = btnPlayersFolder.ForeColor,
                FlatStyle = btnPlayersFolder.FlatStyle,
                Font = btnPlayersFolder.Font,
                Text = btnPlayersFolder.Text,
                TextAlign = btnPlayersFolder.TextAlign,
                Height = btnPlayersFolder.Height,
                Width = cbNumOfPlayers.Width,
            };
            btnTBoardImage.Location = new Point(txtTBoardImage.Right, txtTBoardImage.Top);
            btnTBoardImage.Click += delegate
            {
                if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtTBoardImage.HideHint();
                    txtTBoardImage.Text = fileDialog.FileName;
                    DrawTournamentBoard();
                }
            };
            panorama.Controls.Add(btnTBoardImage);
            
            tournamentBoardImage = new MyUserControl()
            {
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Height = txtTournamentName.Width  / 2,
                Width = txtTournamentName.Width,
            };
            tournamentBoardImage.Location = new Point(txtTBoardImage.Left, btnTBoardImage.Bottom + 10);
            panorama.Controls.Add(tournamentBoardImage);

            //------------------frame 3-------------------------
            //--------------------------------------------------
            Label lblMatchBoard = new Label()
            {
                AutoSize = false,
                BackColor = DarkColor,
                ForeColor = LightColor,
                Font = new System.Drawing.Font(this.Font.FontFamily, 16.0F, FontStyle.Regular),
                Size = new System.Drawing.Size(frameWidth / 4, frameHeight),
                Text = "Choose the background and foreground images for the match board",
                TextAlign = ContentAlignment.TopLeft,
            };
            lblMatchBoard.Location = new Point(x2, 0);
            panorama.Controls.Add(lblMatchBoard);

            txtMatchBoardBackImage = new HintedTextBox("enter match board background image")
            {
                BorderStyle = txtTournamentName.BorderStyle,
                Font = txtTournamentName.Font,
                Width = lblNumOfPlayers.Width
            };
            txtMatchBoardBackImage.Location = new Point(lblMatchBoard.Right + 10, 10);
            txtMatchBoardBackImage.KeyUp += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DrawMatchBoard();
                }
            };
            panorama.Controls.Add(txtMatchBoardBackImage);
            UnderlineFor lineForMatchBoardBackImg = new UnderlineFor(txtMatchBoardBackImage, DarkColor, SystemColors.GrayText)
            {
                BackColor = SystemColors.GrayText,
            };
            lineForMatchBoardBackImg.Location = new Point(txtMatchBoardBackImage.Left, txtMatchBoardBackImage.Bottom);
            panorama.Controls.Add(lineForMatchBoardBackImg);
            Button btnMatchBoardBackImage = new Button()
            {
                BackColor = btnPlayersFolder.BackColor,
                ForeColor = btnPlayersFolder.ForeColor,
                FlatStyle = btnPlayersFolder.FlatStyle,
                Font = btnPlayersFolder.Font,
                Text = btnPlayersFolder.Text,
                TextAlign = btnPlayersFolder.TextAlign,
                Height = btnPlayersFolder.Height,
                Width = cbNumOfPlayers.Width,
            };
            btnMatchBoardBackImage.Location = new Point(txtMatchBoardBackImage.Right, txtMatchBoardBackImage.Top);
            btnMatchBoardBackImage.Click += delegate
            {
                if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtMatchBoardBackImage.HideHint();
                    txtMatchBoardBackImage.Text = fileDialog.FileName;
                    DrawMatchBoard();
                }
            };
            panorama.Controls.Add(btnMatchBoardBackImage);

            txtMatchBoardForeImage = new HintedTextBox("enter match board foreground image")
            {
                BorderStyle = txtTournamentName.BorderStyle,
                Font = txtTournamentName.Font,
                Width = lblNumOfPlayers.Width
            };
            txtMatchBoardForeImage.Location = new Point(txtMatchBoardBackImage.Left, btnMatchBoardBackImage.Bottom + 2);
            txtMatchBoardForeImage.KeyUp += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DrawMatchBoard();
                }
            };
            panorama.Controls.Add(txtMatchBoardForeImage);
            UnderlineFor lineForMatchBoardForeImg = new UnderlineFor(txtMatchBoardForeImage, DarkColor, SystemColors.GrayText)
            {
                BackColor = SystemColors.GrayText,
            };
            lineForMatchBoardForeImg.Location = new Point(txtMatchBoardForeImage.Left, txtMatchBoardForeImage.Bottom);
            panorama.Controls.Add(lineForMatchBoardForeImg);
            Button btnMatchBoardForeImage = new Button()
            {
                BackColor = btnPlayersFolder.BackColor,
                ForeColor = btnPlayersFolder.ForeColor,
                FlatStyle = btnPlayersFolder.FlatStyle,
                Font = btnPlayersFolder.Font,
                Text = btnPlayersFolder.Text,
                TextAlign = btnPlayersFolder.TextAlign,
                Height = btnPlayersFolder.Height,
                Width = cbNumOfPlayers.Width,
            };
            btnMatchBoardForeImage.Location = new Point(txtMatchBoardForeImage.Right, txtMatchBoardForeImage.Top);
            btnMatchBoardForeImage.Click += delegate
            {
                if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtMatchBoardForeImage.HideHint();
                    txtMatchBoardForeImage.Text = fileDialog.FileName;
                    DrawMatchBoard();
                }
            };
            panorama.Controls.Add(btnMatchBoardForeImage);

            matchBoardImage = new MyUserControl()
            {
                BorderStyle = tournamentBoardImage.BorderStyle,
                Height = txtTournamentName.Width / 2,
                Width = txtTournamentName.Width,
            };
            matchBoardImage.Location = new Point(txtMatchBoardForeImage.Left, txtMatchBoardForeImage.Bottom + 2);
            panorama.Controls.Add(matchBoardImage);

            //---------Done
            btnDone = new Button()
            {
                BackColor = DarkColor,
                ForeColor = LightColor,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font(this.Font.FontFamily, 11.0F),
                Text = "Done",
                TextAlign = ContentAlignment.TopCenter,
                Height = 30,
                Width = 80,
                Visible = false,
            };
            btnDone.Location = new Point((frameWidth / 2) - (btnDone.Width / 2), frameHeight - (btnDone.Height + 2));
            btnDone.Click += btnDone_Click;
            panorama.Controls.Add(btnDone);

            //---------Cancel
            btnCancel = new Button()
            {
                BackColor = CancelColor,
                ForeColor = LightColor,
                FlatStyle = btnDone.FlatStyle,
                Font = btnDone.Font,
                Text = "Cancel",
                TextAlign = btnDone.TextAlign,
                Height = btnDone.Height,
                Width = btnDone.Width,
            };
            btnCancel.Location = new Point(btnDone.Right + 2, btnDone.Top);
            btnCancel.Click += btnCancel_Click;
            panorama.Controls.Add(btnCancel);

            //---------
            RadioButton rad1 = new RadioButton()
            {
                AutoSize = false,
                Checked = true,
                Width = 12
            };
            rad1.Location = new Point((statusBar.Width / 2) - 21, rad1.Location.Y);
            rad1.CheckedChanged += delegate { if (rad1.Checked) AnimatePanorama(0); };
            RadioButton rad2 = new RadioButton()
            {
                AutoSize = false,
                Location = new Point((statusBar.Width / 2) - 6, rad1.Location.Y),
                Width = 12
            };
            rad2.CheckedChanged += delegate { if (rad2.Checked) AnimatePanorama(x1 * -1); };
            RadioButton rad3 = new RadioButton()
            {
                AutoSize = false,
                Location = new Point((statusBar.Width / 2) + 9, rad1.Location.Y),
                Width = 12
            };
            rad3.CheckedChanged += delegate { if (rad3.Checked) AnimatePanorama(x2 * -1); btnDone.Visible = true; };

            statusBar.Controls.Add(rad1);
            statusBar.Controls.Add(rad2);
            statusBar.Controls.Add(rad3);
        }

        void btnDone_Click(object sender, EventArgs e) 
        {
            try
            {
                //assign stuff to state
                state.Name = txtTournamentName.Text;
                state.NumberOfPlayers = (int)cbNumOfPlayers.SelectedItem;
                state.PlayersFolder = txtPlayersFolder.Text;
                state.SponsorsFolder = txtSponsorsFolder.Text;

                if (File.Exists(txtTBoardImage.Text))
                {
                    Image image = Image.FromFile(txtTBoardImage.Text);
                    state.TournamentBoardImage = new Bitmap(image);
                    image.Dispose();
                }

                if (File.Exists(txtMatchBoardBackImage.Text))
                {
                    Image image = Image.FromFile(txtMatchBoardBackImage.Text);
                    state.MatchBoardBackImage = new Bitmap(image);
                    image.Dispose();
                }
                if (File.Exists(txtMatchBoardForeImage.Text))
                {
                    Image image = Image.FromFile(txtMatchBoardForeImage.Text);
                    state.MatchBoardForeImage = new Bitmap(image);
                    image.Dispose();
                }

                //close window
                result = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void btnCancel_Click(object sender, EventArgs e)
        {
            //close window
            result = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        void AnimatePanorama(int targetX)
        {
            int count = 0, gap = Math.Abs(targetX - panorama.Location.X), delta = gap / 10;
            while (panorama.Location.X != targetX)
            {
                count++;
                if (count == 6)
                    delta = gap / 20;
                else if (count == 12)
                    delta = gap / 40;
                if (count == 16)
                    delta = 1;
                if (panorama.Location.X < targetX)
                {
                    panorama.Location = new Point(panorama.Left + delta, panorama.Top);

                    btnDone.Location = new Point(btnDone.Left - delta, btnDone.Top);
                    btnCancel.Location = new Point(btnCancel.Left - delta, btnCancel.Top);
                }
                else
                {
                    panorama.Location = new Point(panorama.Left - delta, panorama.Top);

                    btnDone.Location = new Point(btnDone.Left + delta, btnDone.Top);
                    btnCancel.Location = new Point(btnCancel.Left + delta, btnCancel.Top);
                }
                panorama.Refresh();
                System.Threading.Thread.Sleep(10);
            }
        }
        void DrawMatchBoard()
        {
            try
            {
                Graphics g = matchBoardImage.CreatePermanentGraphics();
                Image image = null;
                Bitmap bitmap = null;

                //background
                if (File.Exists(txtMatchBoardBackImage.Text))
                {
                    image = Image.FromFile(txtMatchBoardBackImage.Text);
                    bitmap = new Bitmap(image, matchBoardImage.Size);
                    g.DrawImage(bitmap, new Point());
                }

                //dummy players
                bitmap = new Bitmap(Properties.Resources.DummyPlayersImage, matchBoardImage.Size);
                g.DrawImage(bitmap, new Point());

                //foreground
                if (File.Exists(txtMatchBoardForeImage.Text))
                {
                    image = Image.FromFile(txtMatchBoardForeImage.Text);
                    bitmap = new Bitmap(image, matchBoardImage.Size);
                    g.DrawImage(bitmap, new Point());
                }

                if (bitmap != null) bitmap.Dispose();
                if (image != null) image.Dispose();
                g.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void DrawTournamentBoard() 
        {
            try
            {
                if (File.Exists(txtTBoardImage.Text))
                {
                    Image image = Image.FromFile(txtTBoardImage.Text);
                    Bitmap bitmap = new Bitmap(image, tournamentBoardImage.Size);
                    Graphics g = tournamentBoardImage.CreatePermanentGraphics();
                    g.DrawImage(bitmap, new Point());
                    bitmap = new Bitmap(Properties.Resources.DummyTBoardForeImage, tournamentBoardImage.Size);
                    g.DrawImage(bitmap, new Point());

                    bitmap.Dispose();
                    image.Dispose();
                    g.Dispose();
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        public new DialogResult ShowDialog() 
        {
            base.ShowDialog();
            return result;
        }
    }
}
