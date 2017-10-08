using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Beta.Animation
{
    class Animator_Controller
    {
        private static List<Animator> Animator_list = new List<Animator>();

        public enum AnimationType { Normal, Normal_Reversed, Reversed }
        public enum OtherAnimations_enum { NULL, SplashScreen }
        public struct animation_list_struc
        {
            public Animator animator;
            public Player.Player_Manager.PlayerState Player_State_Animation;
            public OtherAnimations_enum otherAnimationType;
        }
        private static List<animation_list_struc> Animation_list
            = new List<animation_list_struc>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture_list"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Window_related"></param>
        /// <returns></returns>

        public static void LoadAnimator(List<Texture2D> texture_list,
            int X, int Y, int Width,
            int Height,
            Game1.Var.CurrentWindow Window_related,
            int speed_milisecs,
            AnimationType Anim_type,
            Player.Player Player_related_to = null,
            Player.Player_Manager.PlayerState Anime_name = Player.Player_Manager.PlayerState.NULL,
            OtherAnimations_enum otherAnimationType = OtherAnimations_enum.NULL)
        {


            Animator anie = new Animator();
            anie.textures = texture_list;
            anie.X = X;
            anie.Y = Y;
            anie.Width = Width;
            anie.Height = Height;
            anie.Window = Window_related;
            anie.SpeedInMilisecs = speed_milisecs;
            anie.Anim_type = Anim_type;

            anie.Is_Animating = false;
            animation_list_struc stru = new animation_list_struc();
            stru.animator = anie;
            stru.Player_State_Animation = Anime_name;
            stru.otherAnimationType = otherAnimationType;
            Animation_list.Add(stru);
        }
        public static void PlayAnimation(Player.Player_Manager.PlayerState anime_name)
        {
            foreach (animation_list_struc s in Animation_list)
            {
                if (s.Player_State_Animation == anime_name)
                    s.animator.Is_Animating = true;
                else
                    s.animator.Is_Animating = false;
            }
        }
        public static void PlayAnimation(OtherAnimations_enum anime_name)
        {
            foreach (animation_list_struc s in Animation_list)
            {
                if (s.otherAnimationType == anime_name)
                    s.animator.Is_Animating = true;
                else
                    s.animator.Is_Animating = false;
            }
        }
        public static void UpdateAll(GameTime time)
        {
            lock (Animation_list)
                foreach (animation_list_struc s in Animation_list)
                    s.animator.update(time);
        }
        public static void DrawAll(SpriteBatch sprite)
        {
            lock (Animation_list)
                foreach (animation_list_struc s in Animation_list)
                    s.animator.Draw(sprite);
        }
    }
}