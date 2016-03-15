using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBoard.UI
{
    [Serializable]
    public class Player
    {
        public Player() 
        {
            Wins = new Stack<Player>();
            Draws = new Stack<Player>();
            Losses = new Stack<Player>();
        }

        public Image Avatar { get; set; }
        public string Clip { get; set; }
        public string Name { get; set; }
        public Image HappyFace { get; set; }
        public Image NormalFace { get; set; }
        public Image SadFace { get; set; }

        public Stack<Player> Wins { get; set; }
        public Stack<Player> Draws { get; set; }
        public Stack<Player> Losses { get; set; }
    }
}
