using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TBoard.UI
{
    public class TournamentBoard : MyUserControl
    {

        List<Spot> spots = new List<Spot>();
        List<Coin> coins = new List<Coin>();
        SaveFileDialog saveDialog;

        public TournamentBoard() 
        {
            //openDialog
            saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Tournament|*.tournament|All|*.*";

            Dock = DockStyle.Fill;

            int count = 0;
            Timer tmr = new Timer();
            tmr.Enabled = false;
            tmr.Interval = 1000;
            tmr.Tick += delegate 
            {
                count++;
                if (count == 5)
                {
                    Cursor.Hide();
                    tmr.Enabled = false;
                }
            };

            //this.MouseClick += delegate { Cursor.Show(); tmr.Enabled = false; };
            //this.MouseMove += delegate { count = 0; tmr.Enabled = true; };
            //this.MouseHover += delegate { count = 0; tmr.Enabled = true; };
            this.MouseDoubleClick += TournamentBoard_MouseDoubleClick;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(TournamentState.GetSingleton().FilePath))
                TournamentState.GetSingleton().Save();
            else
                MessageBox.Show("This tournament has no file location. Use 'Save As' instead.");
        }
        void btnSaveAs_Click(object sender, EventArgs e)
        {
            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TournamentState.GetSingleton().SaveAs(saveDialog.FileName);
            }
        }
        void TournamentBoard_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (Spot spot in spots) 
            {
                if (spot.Rectangle.Contains(e.Location)) 
                {
                    if (spot.Previous.Length == 2 && spot.Player == null) 
                    {
                        Spot prev1 = spot.Previous[0];
                        Spot prev2 = spot.Previous[1];

                        if (prev1.Player != null && prev2.Player != null) 
                        {
                            //MatchForm matchForm = new MatchForm();
                            MatchForm2 matchForm = new MatchForm2();
                            matchForm.Player1 = prev1.Player;
                            matchForm.Player2 = prev2.Player;
                            matchForm.Stage = prev1.Stage;
                            matchForm.Show();
                        }
                    }
                    break;
                }
            }
        }

        public void InitializeBoard()
        {
            float[] verticalLines;
            float[] horizontalLines;
            float halfRow;
            int rows = 33, cols = 12;
            int pIndex = 0;
            List<Player> players;
            Coin coin;
            Spot spot = null;
            List<Spot> leftSpots = new List<Spot>(), rightSpots = new List<Spot>();
            Stage round32 = new Stage("ROUND OF 32");
            Stage round16 = new Stage("ROUND OF 16");
            Stage qtrFinal = new Stage("QUARTER FINALS");
            Stage semiFinal = new Stage("SEMI FINALS");
            Stage final = new Stage("FINAL");
            Stage winner = new Stage("WINNER");
            Tournament tournament = new Tournament();
            TournamentState state = TournamentState.GetSingleton();
            if (!state.IsValid())
                throw new InvalidOperationException("Tournament is in an invalid state.");

            int num = state.NumberOfPlayers;

            //accepted values for num are powers of 2, for now limited between 2 and 32: 2, 4, 8, 16, 32
            if (num < 2 || num > 32)
                throw new ArgumentException("Invalid number of players.");
            if (num > 2 && num <= 4)
                num = 4;
            else if (num > 4 && num <= 8)
                num = 8;
            else if (num > 8 && num <= 16)
                num = 16;
            else if (num > 16 && num <= 32)
                num = 32;
            //var log = Math.Log(2, 2);
            //var c = Math.Ceiling(log);
            //var fl = Math.Floor(log);
            try
            {
                players = state.LoadPlayers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return; 
            }

            float deltaY = this.Height / rows;
            float y = 0;
            //but we ignore the first and last rows
            horizontalLines = new float[rows - 2];
            for (int i = 0; i < horizontalLines.Length; i++)
            {
                y += deltaY;
                horizontalLines[i] = y;
            }
            if (num == 32)
                halfRow = deltaY / 2;
            else
                halfRow = deltaY;

            float deltaX = this.Width / cols;
            float x = 0;
            verticalLines = new float[cols - 1];
            for (int i = 0; i < verticalLines.Length; i++)
            {
                x += deltaX;
                verticalLines[i] = x - halfRow;
            }

            Graphics g = CreatePermanentGraphics();

            //draw background
            g.DrawImage(state.TournamentBoardImage, this.ClientRectangle);

            RectangleF r;
            Font f = new Font(this.Font.FontFamily, 13.0F, FontStyle.Regular);

            //draw first and last (11th) circles
            if (num > 16)
            {
                for (int i = 0; i < horizontalLines.Length; i += 2)
                {
                    //first circles
                    x = verticalLines[0];
                    y = horizontalLines[i];
                    r = new RectangleF(x, y, (halfRow * 2), (halfRow * 2));
                    spot = new Spot(r, g);
                    spot.Direction = Direction.Right;
                    leftSpots.Add(spot);
                    spot.DrawNormal();
                    coin = new Coin(players[pIndex], pIndex); pIndex++;
                    coins.Add(coin);
                    spot.ReceiveCoin(coin);
                    //draw names
                    if (spot.Player.Name != null)
                    {
                        string name = spot.Player.Name;
                        float nameWidth = g.MeasureString(name, f).Width;
                        #region OLD CODE
                        //g.DrawString(name, f, Brushes.Black,
                        //    new RectangleF(spot.Left - (nameWidth + 2), y + (spot.Height / 3) + 1, nameWidth, spot.Height));
                        //g.DrawString(name, f, Brushes.White,
                        //    new RectangleF(spot.Left - (nameWidth + 3), y + (spot.Height / 3), nameWidth, spot.Height));
                        #endregion
                        #region NEW CODE
                        //get the mid point of the spot
                        float midX = (spot.Left + spot.Right) / 2;
                        g.DrawString(name, f, Brushes.Black,
                            new RectangleF(midX - (nameWidth / 2) + 1, spot.Bottom + 2, nameWidth, spot.Height));
                        g.DrawString(name, f, Brushes.White,
                            new RectangleF(midX - (nameWidth / 2), spot.Bottom + 1, nameWidth, spot.Height));
                        #endregion
                    }

                    //last circles
                    x = verticalLines[10];
                    y = horizontalLines[i];
                    r = new RectangleF(x, y, (halfRow * 2), (halfRow * 2));
                    spot = new Spot(r, g);
                    spot.Direction = Direction.Left;
                    rightSpots.Add(spot);
                    spot.DrawNormal();
                    coin = new Coin(players[pIndex], pIndex); pIndex++;
                    coins.Add(coin);
                    spot.ReceiveCoin(coin);
                    //draw names
                    if (spot.Player.Name != null)
                    {
                        string name = spot.Player.Name;
                        float nameWidth = g.MeasureString(name, f).Width;
                        #region OLD CODE
                        //g.DrawString(name, f, Brushes.Black,
                        //    new RectangleF(spot.Right + 8, y + (spot.Height / 3) + 1, nameWidth, spot.Height));
                        //g.DrawString(name, f, Brushes.White,
                        //    new RectangleF(spot.Right + 7, y + (spot.Height / 3), nameWidth, spot.Height));
                        #endregion
                        #region NEW CODE
                        float midX = (spot.Left + spot.Right) / 2;
                        g.DrawString(name, f, Brushes.Black,
                            new RectangleF(midX - (nameWidth / 2) + 1, spot.Bottom + 2, nameWidth, spot.Height));
                        g.DrawString(name, f, Brushes.White,
                            new RectangleF(midX - (nameWidth / 2), spot.Bottom + 1, nameWidth, spot.Height));
                        #endregion
                    }
                }
                round32.AddRange(leftSpots);
                round32.AddRange(rightSpots);
                spots.AddRange(leftSpots);
                spots.AddRange(rightSpots);
            }

            //draw 2nd and 10th circles
            if (num > 8)
            {
                halfRow = deltaY;
                leftSpots.Clear();
                rightSpots.Clear();
                for (int i = 1; i < horizontalLines.Length; i += 4)
                {
                    //2nd circles
                    x = verticalLines[1];
                    y = horizontalLines[i];
                    r = new RectangleF(x, y, (halfRow * 2), (halfRow * 2));
                    spot = new Spot(r, g);
                    spot.Direction = Direction.Right;
                    leftSpots.Add(spot);
                    spot.DrawNormal();
                    if (num == 16)
                    {
                        coin = new Coin(players[pIndex], pIndex); pIndex++;
                        coins.Add(coin);
                        spot.ReceiveCoin(coin);
                        //draw names
                        if (spot.Player.Name != null)
                        {
                            string name = spot.Player.Name;
                            float nameWidth = g.MeasureString(name, f).Width;
                            //get the mid point of the spot
                            float midX = (spot.Left + spot.Right) / 2;
                            g.DrawString(name, f, Brushes.Black,
                                new RectangleF(midX - (nameWidth / 2) + 1, spot.Bottom + 2, nameWidth, spot.Height));
                            g.DrawString(name, f, Brushes.White,
                                new RectangleF(midX - (nameWidth / 2), spot.Bottom + 1, nameWidth, spot.Height));
                        }
                    }

                    //10th circles
                    x = verticalLines[9];
                    y = horizontalLines[i];
                    r = new RectangleF(x, y, (halfRow * 2), (halfRow * 2));
                    spot = new Spot(r, g);
                    spot.Direction = Direction.Left;
                    rightSpots.Add(spot);
                    spot.DrawNormal();
                    if (num == 16)
                    {
                        coin = new Coin(players[pIndex], pIndex); pIndex++;
                        coins.Add(coin);
                        spot.ReceiveCoin(coin);
                        //draw names
                        if (spot.Player.Name != null)
                        {
                            string name = spot.Player.Name;
                            float nameWidth = g.MeasureString(name, f).Width;
                            float midX = (spot.Left + spot.Right) / 2;
                            g.DrawString(name, f, Brushes.Black,
                                new RectangleF(midX - (nameWidth / 2) + 1, spot.Bottom + 2, nameWidth, spot.Height));
                            g.DrawString(name, f, Brushes.White,
                                new RectangleF(midX - (nameWidth / 2), spot.Bottom + 1, nameWidth, spot.Height));
                        }
                    }
                }
                round16.AddRange(leftSpots);
                round16.AddRange(rightSpots);
                spots.AddRange(leftSpots);
                spots.AddRange(rightSpots);
            }

            //draw 3rd and 9th circles
            if (num > 4)
            {
                leftSpots.Clear();
                rightSpots.Clear();
                for (int i = 3; i < horizontalLines.Length - 1; i += 8)
                {
                    //3rd circles
                    x = verticalLines[2];
                    y = horizontalLines[i];
                    r = new RectangleF(x, y, (halfRow * 2), (halfRow * 2));
                    spot = new Spot(r, g);
                    spot.Direction = Direction.Right;
                    leftSpots.Add(spot);
                    spot.DrawNormal(); 
                    if (num == 8)
                    {
                        coin = new Coin(players[pIndex], pIndex); pIndex++;
                        coins.Add(coin);
                        spot.ReceiveCoin(coin);
                        //draw names
                        if (spot.Player.Name != null)
                        {
                            string name = spot.Player.Name;
                            float nameWidth = g.MeasureString(name, f).Width;
                            //get the mid point of the spot
                            float midX = (spot.Left + spot.Right) / 2;
                            g.DrawString(name, f, Brushes.Black,
                                new RectangleF(midX - (nameWidth / 2) + 1, spot.Bottom + 2, nameWidth, spot.Height));
                            g.DrawString(name, f, Brushes.White,
                                new RectangleF(midX - (nameWidth / 2), spot.Bottom + 1, nameWidth, spot.Height));
                        }
                    }

                    //9th circles
                    x = verticalLines[8];
                    y = horizontalLines[i];
                    r = new RectangleF(x, y, (halfRow * 2), (halfRow * 2));
                    spot = new Spot(r, g);
                    spot.Direction = Direction.Left;
                    rightSpots.Add(spot);
                    spot.DrawNormal();
                    if (num == 8)
                    {
                        coin = new Coin(players[pIndex], pIndex); pIndex++;
                        coins.Add(coin);
                        spot.ReceiveCoin(coin);
                        //draw names
                        if (spot.Player.Name != null)
                        {
                            string name = spot.Player.Name;
                            float nameWidth = g.MeasureString(name, f).Width;
                            float midX = (spot.Left + spot.Right) / 2;
                            g.DrawString(name, f, Brushes.Black,
                                new RectangleF(midX - (nameWidth / 2) + 1, spot.Bottom + 2, nameWidth, spot.Height));
                            g.DrawString(name, f, Brushes.White,
                                new RectangleF(midX - (nameWidth / 2), spot.Bottom + 1, nameWidth, spot.Height));
                        }
                    }
                }
                qtrFinal.AddRange(leftSpots);
                qtrFinal.AddRange(rightSpots);
                spots.AddRange(leftSpots);
                spots.AddRange(rightSpots);
            }

            //draw 4th and 8th circles
            if (num > 2)
            {
                leftSpots.Clear();
                rightSpots.Clear();
                for (int i = 7; i < horizontalLines.Length - 3; i += 16)
                {
                    //4th circles
                    x = verticalLines[3];
                    y = horizontalLines[i];
                    r = new RectangleF(x, y, (halfRow * 2), (halfRow * 2));
                    spot = new Spot(r, g);
                    spot.Direction = Direction.Right;
                    leftSpots.Add(spot);
                    spot.DrawNormal();
                    if (num == 4)
                    {
                        coin = new Coin(players[pIndex], pIndex); pIndex++;
                        coins.Add(coin);
                        spot.ReceiveCoin(coin);
                        //draw names
                        if (spot.Player.Name != null)
                        {
                            string name = spot.Player.Name;
                            float nameWidth = g.MeasureString(name, f).Width;
                            //get the mid point of the spot
                            float midX = (spot.Left + spot.Right) / 2;
                            g.DrawString(name, f, Brushes.Black,
                                new RectangleF(midX - (nameWidth / 2) + 1, spot.Bottom + 2, nameWidth, spot.Height));
                            g.DrawString(name, f, Brushes.White,
                                new RectangleF(midX - (nameWidth / 2), spot.Bottom + 1, nameWidth, spot.Height));
                        }
                    }

                    //8th circles
                    x = verticalLines[7];
                    y = horizontalLines[i];
                    r = new RectangleF(x, y, (halfRow * 2), (halfRow * 2));
                    spot = new Spot(r, g);
                    spot.Direction = Direction.Left;
                    rightSpots.Add(spot);
                    spot.DrawNormal();
                    if (num == 4)
                    {
                        coin = new Coin(players[pIndex], pIndex); pIndex++;
                        coins.Add(coin);
                        spot.ReceiveCoin(coin);
                        //draw names
                        if (spot.Player.Name != null)
                        {
                            string name = spot.Player.Name;
                            float nameWidth = g.MeasureString(name, f).Width;
                            float midX = (spot.Left + spot.Right) / 2;
                            g.DrawString(name, f, Brushes.Black,
                                new RectangleF(midX - (nameWidth / 2) + 1, spot.Bottom + 2, nameWidth, spot.Height));
                            g.DrawString(name, f, Brushes.White,
                                new RectangleF(midX - (nameWidth / 2), spot.Bottom + 1, nameWidth, spot.Height));
                        }
                    }
                }
                semiFinal.AddRange(leftSpots);
                semiFinal.AddRange(rightSpots);
                spots.AddRange(leftSpots);
                spots.AddRange(rightSpots);
            }

            //draw 5th and 7th circles
            //5th circle
            x = verticalLines[4];
            y = horizontalLines[15];
            r = new RectangleF(x, y, (halfRow * 2), (halfRow * 2));
            spot = new Spot(r, g);
            spot.Direction = Direction.Right;
            final.Add(spot);
            spots.Add(spot);
            spot.DrawNormal();
            if (num == 2)
            {
                coin = new Coin(players[pIndex], pIndex); pIndex++;
                coins.Add(coin);
                spot.ReceiveCoin(coin);
                //draw names
                if (spot.Player.Name != null)
                {
                    string name = spot.Player.Name;
                    float nameWidth = g.MeasureString(name, f).Width;
                    //get the mid point of the spot
                    float midX = (spot.Left + spot.Right) / 2;
                    g.DrawString(name, f, Brushes.Black,
                        new RectangleF(midX - (nameWidth / 2) + 1, spot.Bottom + 2, nameWidth, spot.Height));
                    g.DrawString(name, f, Brushes.White,
                        new RectangleF(midX - (nameWidth / 2), spot.Bottom + 1, nameWidth, spot.Height));
                }
            }

            //7th circle
            x = verticalLines[6];
            y = horizontalLines[15];
            r = new RectangleF(x, y, (halfRow * 2), (halfRow * 2));
            spot = new Spot(r, g);
            spot.Direction = Direction.Left;
            final.Add(spot);
            spots.Add(spot);
            spot.DrawNormal();
            if (num == 2)
            {
                coin = new Coin(players[pIndex], pIndex); pIndex++;
                coins.Add(coin);
                spot.ReceiveCoin(coin);
                //draw names
                if (spot.Player.Name != null)
                {
                    string name = spot.Player.Name;
                    float nameWidth = g.MeasureString(name, f).Width;
                    float midX = (spot.Left + spot.Right) / 2;
                    g.DrawString(name, f, Brushes.Black,
                        new RectangleF(midX - (nameWidth / 2) + 1, spot.Bottom + 2, nameWidth, spot.Height));
                    g.DrawString(name, f, Brushes.White,
                        new RectangleF(midX - (nameWidth / 2), spot.Bottom + 1, nameWidth, spot.Height));
                }
            }

            //draw 6th (winner's) circle
            x = verticalLines[5];
            y = horizontalLines[7];
            r = new RectangleF(x - halfRow, y - halfRow, (halfRow * 4), (halfRow * 4));
            spot = new FinalSpot(r, g);
            winner.Add(spot);
            spots.Add(spot);
            spot.DrawNormal();

            if (num > 16)
                tournament.Add(round32);
            if (num > 8)
                tournament.Add(round16);
            if (num > 4)
                tournament.Add(qtrFinal);
            if (num > 2)
                tournament.Add(semiFinal);
            tournament.Add(final);
            tournament.Add(winner);

            //paint sponsors
            DirectoryInfo sponsorsDir = new DirectoryInfo(state.SponsorsFolder);
            if (sponsorsDir.Exists) 
            {
                var imageFiles = sponsorsDir.GetFiles("*.png", SearchOption.TopDirectoryOnly);
                if (imageFiles.Length > 0)
                {
                    float sponsorY = horizontalLines[28];

                    string poweredBy = "POWERED BY";
                    var ff = new Font(this.Font.FontFamily, 15.0F, FontStyle.Bold);
                    var szF = g.MeasureString(poweredBy, ff);
                    float strW = szF.Width;
                    float strH = szF.Height;
                    x = (this.Width / 2) - (strW / 2);
                    g.DrawString(poweredBy, ff, Brushes.Yellow, new PointF(x, sponsorY - (strH + 5)));

                    //int space = 5;
                    float w = halfRow * 3;
                    //float sponsorW = (w + space) * imageFiles.Length;
                    //x = (this.Width / 2) - (sponsorW / 2);
                    //foreach (var imageFile in imageFiles) 
                    //{
                    //    Bitmap image = new Bitmap(Image.FromFile(imageFile.FullName), new Size((int)w, (int)w));
                    //    g.DrawImage(image, new PointF(x, sponsorY));
                    //    x += w + space;
                    //    image.Dispose();
                    //}

                    Image[] imageList = new Image[imageFiles.Length];
                    for (int index = 0; index < imageFiles.Length; index++)
                    {
                        Image image = Image.FromFile(imageFiles[index].FullName);
                        imageList[index] = new Bitmap(image);
                        image.Dispose();
                    }
                    Marquee marquee = new Marquee(imageList, 4, (int)w);
                    x = (this.Width / 2) - (marquee.Width / 2);
                    marquee.Location = new Point((int)x, (int)sponsorY);
                    this.Controls.Add(marquee);
                }
            }

            this.Controls.AddRange(coins.ToArray());

            Button btnSaveAs = new Button()
            {
                BackColor = Color.FromArgb(30, 144, 255),
                ForeColor = Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font(this.Font.FontFamily, 11.0F),
                Text = "Save As",
                TextAlign = ContentAlignment.TopCenter,
                Height = 30,
                Width = 80,
            };
            btnSaveAs.Location = new Point(this.Width - (btnSaveAs.Width + 2), this.Height - (btnSaveAs.Height + 2));
            btnSaveAs.Click += btnSaveAs_Click;
            this.Controls.Add(btnSaveAs);

            Button btnSave = new Button()
            {
                BackColor = btnSaveAs.BackColor,
                ForeColor = btnSaveAs.ForeColor,
                FlatStyle = btnSaveAs.FlatStyle,
                Font = btnSaveAs.Font,
                Text = "Save",
                TextAlign = btnSaveAs.TextAlign,
                Size = btnSaveAs.Size
            };
            btnSave.Location = new Point(btnSaveAs.Left - (btnSave.Width + 2), btnSaveAs.Top);
            btnSave.Click += btnSave_Click;
            this.Controls.Add(btnSave);

            //********replay
            if (state.CoinActions.Any()) 
            {
                CoinAction[] coinActions = state.CoinActions.ToArray();
                state.CoinActions.Clear();

                foreach (var action in coinActions) 
                {
                    if (action.Action == Action.CoinMoveBack)
                        coins[action.CoinIndex].MoveBack();
                    else if (action.Action == Action.CoinMoveForward)
                        coins[action.CoinIndex].MoveForward();
                }
            }
        }
    }
}
