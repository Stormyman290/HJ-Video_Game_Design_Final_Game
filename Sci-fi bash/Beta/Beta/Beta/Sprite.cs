using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Beta
{
    public class Sprite
    {
        //public Vector2 Position { get; set; }
        public Vector2 Position;
        public Texture2D Texture { set; get; }
        public SpriteBatch SpriteBatch { get; set; }
        public Rectangle Source;
        public int Width, Height;
        public Rectangle Destination;
        Vector2 _speed;
        Vector2 _direction;

        public Rectangle Bounds
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height); }
        }

        public Sprite(Texture2D texture, Vector2 position, SpriteBatch batch)
        {
            Texture = texture;
            Position = position;
            SpriteBatch = batch;
            Width = Texture.Width;
            Height = Texture.Height;
        }
        public Sprite(Texture2D texture, Vector2 position, SpriteBatch batch, Rectangle source)
        {
            Source = source;
            Texture = texture;
            Position = position;
            SpriteBatch = batch;
        }

        public virtual void Draw()
        {
            SpriteBatch.Draw(Texture, Position, Color.White);
        }
        public virtual void SecondDraw(SpriteBatch SpriteBatch)
        {
            SpriteBatch.Draw(Texture, Position, Source, Color.White);
        }
        public virtual void Update(GameTime gameTime)
        {

            
        }

        public void LoadContent(ContentManager theContentManager, String assetName)
        {
        }

        public void setVector(Vector2 setVectorTo)
        {
            Position.X = setVectorTo.X;
            Position.Y = setVectorTo.Y;
        }

        public void setWidthHeight(int w, int h)
        {
            Width = w;
            Height = h;
        }

        public void setBounds()
        {
            Destination = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }
        public void setSpeed(Vector2 aSpeed)
        {
            _speed = aSpeed;
        }
        public void setDirection(Vector2 aDirection)
        {
            _direction = aDirection;
        }
    }
}