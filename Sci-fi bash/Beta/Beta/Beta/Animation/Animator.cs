using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Beta.Animation
{
    class Animator
    {
        public Player.Player PlayerRelatedTo;

        public List<Texture2D> textures = new List<Texture2D>();
        public int X, Y, Width, Height;

        private bool Is_AnimationBackward = false;

        public bool Is_Animating;

        public Animator_Controller.AnimationType Anim_type;

        public int current_frame_index = 0;
        //When a splash screen is shown in game1 , to draw only the
        //nimations that are correspomding to the splah screen
        public Game1.Var.CurrentWindow Window;

        public int Time_counter = 0;
        public int SpeedInMilisecs;

        public void update(GameTime time)
        {
            if (!Is_Animating)
                return;

            if (PlayerRelatedTo != null)
            {
                X = PlayerRelatedTo.X;
                Y = PlayerRelatedTo.Y;
            }

            Time_counter += time.ElapsedGameTime.Milliseconds;
            if (Time_counter > SpeedInMilisecs)
            {// frame switch 
                Time_counter = 0;

                if (Is_AnimationBackward)
                    current_frame_index--;
                else
                    current_frame_index++;

                if (current_frame_index + 1 == textures.Count)
                {
                    switch (Anim_type)
                    {
                        case Animator_Controller.AnimationType.Normal:
                            current_frame_index = 0;
                            break;
                        case Animator_Controller.AnimationType.Normal_Reversed:
                            Is_AnimationBackward = true;
                            break;
                        case Animator_Controller.AnimationType.Reversed:


                            break;
                    }


                }
                if (current_frame_index == 0)
                {
                    switch (Anim_type)
                    {
                        case Animator_Controller.AnimationType.Normal_Reversed:
                            Is_AnimationBackward = false;
                            break;
                        case Animator_Controller.AnimationType.Reversed:
                            current_frame_index = textures.Count;
                            break;
                    }


                }

            }
        }

        public void Draw(SpriteBatch sprite)
        {
            if (!Is_Animating)
                return;
            if (Game1.Var.Currentwindow != Window)
                return;

            foreach (Texture2D t in textures)
            {
                sprite.Draw(textures[current_frame_index],
                    new Rectangle(X, Y, Width, Height), Color.White);
            }
        }
    }
}
