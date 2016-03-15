using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;

namespace TBoard.UI
{
    [Serializable]
    public class TournamentState
    {
        static TournamentState state;

        private TournamentState() 
        {
            CoinActions = new List<CoinAction>();
        }

        public static TournamentState GetSingleton(string filepath = null) 
        {
            if (state != null)
                return state;

            if (filepath == null)
            {
                state = new TournamentState();
            }

            if (state == null)
            {
                try
                {
                    state = Load(filepath);
                }
                catch
                {
                    state = new TournamentState();
                }
                state.FilePath = filepath;
            }

            return state;
        }

        public bool IsValid() 
        {
            if (this == null || this.MatchBoardBackImage == null || this.MatchBoardForeImage == null
                //|| this.Name == null
                || this.PlayersFolder == null
                //|| this.SoundFile == null
                || this.SponsorsFolder == null || this.TournamentBoardImage == null)
                return false;

            return true;
        }
        public static TournamentState Load(string filePath)
        {
            IFormatter serializer = new BinaryFormatter();
            Stream stream = null;

            try
            {
                stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                TournamentState state = (TournamentState)serializer.Deserialize(stream);

                return state;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }
        public List<Player> LoadPlayers()
        {
            int num = this.NumberOfPlayers;
            List<Player> players = new List<Player>();
            DirectoryInfo playersDir = new DirectoryInfo(this.PlayersFolder);
            if (playersDir.Exists)
            {
                var playerFiles = playersDir.GetFiles("*.xml", SearchOption.TopDirectoryOnly);
                if (playerFiles.Length >= num)
                {
                    for (int index = 0; index < num; index++)
                    {
                        XDocument xDoc = XDocument.Load(playerFiles[index].FullName, LoadOptions.SetLineInfo);
                        Player player = new Player();
                        player.Name = xDoc.Root.Attribute("Name").Value;
                        player.Avatar = Image.FromFile(playersDir.FullName + "\\" + xDoc.Root.Attribute("Avatar").Value);
                        player.Clip = playersDir.FullName + "\\" + xDoc.Root.Attribute("Clip").Value;
                        player.HappyFace = Image.FromFile(playersDir.FullName + "\\" + xDoc.Root.Attribute("HappyFace").Value);
                        player.NormalFace = Image.FromFile(playersDir.FullName + "\\" + xDoc.Root.Attribute("NormalFace").Value);
                        player.SadFace = Image.FromFile(playersDir.FullName + "\\" + xDoc.Root.Attribute("SadFace").Value);
                        players.Add(player);
                    }
                }
                else
                {
                    throw new ArgumentException(string.Format("Could not find {0} player(s).", num));
                    //MessageBox.Show(string.Format("Could not find {0} player(s).", num));
                }
            }
            return players;
        }
        public void Save()
        {
            IFormatter serializer = new BinaryFormatter();
            Stream stream = null;

            try
            {
                stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None);

                serializer.Serialize(stream, this);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }
        public void SaveAs(string filePath) 
        {
            this.FilePath = filePath;
            Save();
        }

        public string FilePath { get; private set; }

        public List<CoinAction> CoinActions { get; set; }
        public Image MatchBoardBackImage { get; set; }
        public Image MatchBoardForeImage { get; set; }
        public string Name { get; set; }
        public int NumberOfPlayers { get; set; }
        public string PlayersFolder { get; set; }
        public string SoundFile { get; set; }
        public string SponsorsFolder { get; set; }
        public Image TournamentBoardImage { get; set; }
    }

    [Serializable]
    public struct CoinAction 
    {
        public CoinAction(int coinIndex, Action action) : this()
        {
            this.Action = action;
            this.CoinIndex = coinIndex;
        }
        public Action Action { get; set; }
        public int CoinIndex { get; set; }
    }

    [Serializable]
    public enum Action 
    {
        CoinMoveBack,
        CoinMoveForward
    }
}
