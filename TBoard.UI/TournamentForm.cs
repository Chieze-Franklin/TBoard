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
    public partial class TournamentForm : Form
    {
        TournamentBoard tournamentBoard;
        OpenFileDialog openDialog;

        public TournamentForm()
        {
            InitializeComponent();

            //openDialog
            openDialog = new OpenFileDialog();
            openDialog.Filter = "Tournament|*.tournament|All|*.*";
            openDialog.Multiselect = false;

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.BackgroundImage = Properties.Resources.HomeImage;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            this.KeyDown += Form_KeyDown;
        }

        void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.N)
            {
                TournamentState state = TournamentState.GetSingleton();

                SettingsForm settingsForm = new SettingsForm(state);
                if (settingsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (!state.IsValid())
                    {
                        MessageBox.Show("Tournament is in an invalid state.\nEnsure you filled all fields.");
                        return;
                    }

                    tournamentBoard = new TournamentBoard();
                    this.Controls.Add(tournamentBoard);
                    tournamentBoard.InitializeBoard();
                }
                //else MessageBox.Show("result: Cancel");
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.O)
            {
                try
                {
                    if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        TournamentState state = TournamentState.GetSingleton(openDialog.FileName);
                        if (!state.IsValid()) 
                        {
                            MessageBox.Show("Tournament is in an invalid state.\nEnsure you filled all fields.");
                            return;
                        }

                        tournamentBoard = new TournamentBoard();
                        this.Controls.Add(tournamentBoard);
                        tournamentBoard.InitializeBoard();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
