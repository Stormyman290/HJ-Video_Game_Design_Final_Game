using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Beta.Player
{
    class Player_Manager
    {

        public enum PlayerState { NULL, None, walk };

        public static Player[] Player_Array = new Player[0];

        public static void AddPlayer()
        {
            Array.Resize(ref Player_Array, Player_Array.Length + 2);

            Player_Array[0] = new Player(false, 200);


        }

        public static void generations()
        {

        }
        public static void Update(KeyboardState prev_state, KeyboardState actual_state)
        {
            if (Game1.Var.Currentwindow != Game1.Var.CurrentWindow.Game)
                return;
            if (actual_state.IsKeyDown(Keys.D))
            {
                Player_Array[0].state = PlayerState.walk;
                Animation.Animator_Controller.PlayAnimation(PlayerState.walk);
            }
            else
            {
                Player_Array[0].state = PlayerState.None;

                Animation.Animator_Controller.PlayAnimation(PlayerState.None);
            }
        }
    }
}
