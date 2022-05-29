using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace How2Program_Asteroid_Desktop.Game_manager.Sprites
{
    public abstract class Sprite
    {
        protected static List<Sprite> EXISTING_SPRITES = new List<Sprite>();
        protected ContentManager Content;
        protected Rectangle Rectangle;
        protected Rectangle Screen;
        protected Vector2 Origin;
        protected Vector2 Position;
        protected Texture2D Texture;
        


        public Sprite(int _x, int _y, int _w, int _h, Rectangle screen, ContentManager content)
        {
            Rectangle = new Rectangle(_x, _y, _w, _h);
            Origin = new Vector2(_w / 2f, _h / 2f);
            Position = new Vector2(_x, _y);
            EXISTING_SPRITES.Add(this);
            Content = content;
            Screen = screen;
        }

        public virtual void Update(GameTime deltaTime)
        {
            ManageScreenPosition();
            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;
        }

        protected virtual void ManageScreenPosition()
        {
            if (Position.X < 0) Position.X = Screen.Width;
            else if (Position.X > Screen.Width) Position.X = 0;

            if (Position.Y < 0) Position.Y = Screen.Height;
            else if (Position.Y > Screen.Height) Position.Y = 0;
        }

        public virtual void Draw(SpriteBatch sprite)
        {
            if (Texture != null)
            {
                sprite.Draw(Texture, Position, Color.White);
            }
        }

        public bool Collides(Sprite other)
        {
            bool myPOVX = Rectangle.X <= other.Rectangle.X && Rectangle.Right >= other.Rectangle.X;
            bool ItsPOVX = other.Rectangle.X <= Rectangle.X && other.Rectangle.Right >= Rectangle.X;
            bool overlapX = myPOVX || ItsPOVX;

            bool myPOVY = Rectangle.Y <= other.Rectangle.Y && Rectangle.Bottom >= other.Rectangle.Y;
            bool ItsPOVY = other.Rectangle.Y <= Rectangle.Y && other.Rectangle.Bottom>= Rectangle.Y;
            bool overlapY = myPOVY || ItsPOVY;

            return overlapX && overlapY;
        }

        public void DeleteExistence()
        {
            EXISTING_SPRITES.Remove(this);
        }
    }
}
