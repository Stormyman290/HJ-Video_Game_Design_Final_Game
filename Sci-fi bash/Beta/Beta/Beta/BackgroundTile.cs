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
using System.Text;

namespace Beta
{
    class BackgroundTile : Sprite
    {
        public new Rectangle Source;
        public new Rectangle Destination;

        public BackgroundTile(Texture2D texture, Vector2 position, SpriteBatch batch, Rectangle source, Rectangle bounds)
            : base(texture, position, batch)
        {
            Source = source;
            Destination = bounds;
        }

        public Rectangle getSource()
        {
            return Source;
        }

        public override void Draw()
        {
            SpriteBatch.Draw(Texture, Destination, Source, Color.White);
        }
    }
}
