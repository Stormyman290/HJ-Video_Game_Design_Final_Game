using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beta.Player
{
    class Player
    {
        int Health = 100;
        bool isAI;

        int X_loc, Y_loc;

        public int X
        {
            get { return X_loc; }
        }
        public int Y
        {
            get { return Y_loc; }
        }

        public Player_Manager.PlayerState state = Player_Manager.PlayerState.None;
        public Player(bool isAi, int X_ang, int Y = -1)
        {
            X_loc = X_ang;
            if (Y == -1)
            {
                Y_loc = Game1.Var.GameRec.Y +
                    Game1.Var.GameRec.Height - Game1.Var.CharacterSize.Height;
            }
            isAI = isAi;
        }
    }
}
