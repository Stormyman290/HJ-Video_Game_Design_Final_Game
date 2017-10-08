using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Beta
{
    public class Tile : Sprite
    {
        public bool IsBlocked { get; set; }

        public new Rectangle Bounds
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Source.Width, Source.Height); }
        }
        public Tile(Texture2D texture, Vector2 position, SpriteBatch batch, bool isBlocked)
            : base(texture, position, batch)
        {
            IsBlocked = isBlocked;
        }

        public Tile(Texture2D texture, Vector2 position, SpriteBatch batch, Rectangle source, bool isBlocked)
            : base(texture, position, batch, source)
        {
            IsBlocked = isBlocked;
        }

        public override void Draw()
        {
            if (IsBlocked)
            {
                base.Draw();
            }
        }

        public override void SecondDraw(SpriteBatch SpriteBatch)
        {
            if (IsBlocked)
            {
                base.SecondDraw(SpriteBatch);
            }
        }

    }
}