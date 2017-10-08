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

namespace Beta
{
    class Hazard
    {
        private int hazardDuration;
        private int radiusOfEffect;
        private Texture2D texture;
        private Vector2 location, origin;
        private Color color = Color.White;
        private Rectangle source;
        private Random rnd;

        public Hazard(Texture2D texture, Rectangle source)
        {
            this.texture = texture;
            this.source = source;
            origin = new Vector2(source.Width / 2, source.Height / 2);
            rnd = new Random();
        }

        public int getHazardDuration()
        {
            return rnd.Next(600, 1800);
        }

        public Vector2 getLocation()
        {
            return location;
        }

        public Vector2 getOrigin()
        {
            return origin;
        }

        public void happen()
        {
            hazardDuration = rnd.Next(900, 1800);
            radiusOfEffect = rnd.Next(160, 256);
            location = new Vector2(rnd.Next(0, (800 - radiusOfEffect)), rnd.Next(0, 800 - radiusOfEffect));
        }

        public void draw(SpriteBatch batch, float currentRotation)
        {
            batch.Draw(texture, location, source, color, currentRotation, origin, (float)10, SpriteEffects.None, (float)0);
        }
    }
}