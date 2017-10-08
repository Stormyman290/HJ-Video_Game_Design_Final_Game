using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Beta.Menu
{
    class Splash
    {
        static Boolean isSplashing = true;
        public static List<Texture2D> Texture = new List<Texture2D>(); //sifiBash <--title

        public static void ShowSplashScreen()
        {
            Game1.Var.Currentwindow = Game1.Var.CurrentWindow.SplashScreen;
            Animation.Animator_Controller.PlayAnimation(Animation.Animator_Controller.OtherAnimations_enum.SplashScreen);
        }
        public static void Update(KeyboardState new_state, GraphicsDeviceManager graphics, Song[] songs)
        {
            if (Game1.Var.Currentwindow != Game1.Var.CurrentWindow.SplashScreen)
                return;
            Keys[] array_kerys = new_state.GetPressedKeys();
            if (array_kerys.Length == 0)

                return;
            Game1.Var.Currentwindow = Game1.Var.CurrentWindow.Game;


            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 800;

            graphics.ApplyChanges();
           Game1.Var.Currentwindow = Game1.Var.CurrentWindow.Game;
           isSplashing = false;
        }
        public static Boolean isSplasingNow()
        {
            return isSplashing;
        }
    }
}
