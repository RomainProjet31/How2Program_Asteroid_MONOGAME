using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace How2Program_Asteroid_Desktop.Game_manager.Sprites
{
    public class Missile : Sprite
    {
        private static float MISSILE_SPEED = 5f;
        private static int LIFETIME_MISSILE = 3000;
        private static int DESTROY_INTERVAL = 500;

        public bool IsDestroyed { get => _destroyed; set { if (value) _destroyed = value; } }
        public bool RemoveTrigger { get => _removeTrigger; }

        private bool _removeTrigger;
        private bool _destroyed;
        private int _counterLifeTime;
        private int _counterDestroy;
        private float _angle;

        public Missile(int _x, int _y, int _w, int _h, Rectangle screen, ContentManager content, float angle) : base(_x, _y, _w, _h, screen, content)
        {
            _angle = angle;
            _counterDestroy = 0;
            _destroyed = _removeTrigger = false;
            Texture = Content.Load<Texture2D>("Sprites/laser");
        }

        public override void Update(GameTime deltaTime)
        {
            if (!_destroyed && !_removeTrigger)
            {
                _counterLifeTime += deltaTime.ElapsedGameTime.Milliseconds;
                if (_counterLifeTime >= LIFETIME_MISSILE)
                {
                    _removeTrigger = true;
                }
                else
                {
                    float rad = MathHelper.ToRadians(_angle);
                    Position.X += MISSILE_SPEED * MathF.Cos(rad);
                    Position.Y += MISSILE_SPEED * MathF.Sin(rad);
                    // On ne veut pas que les missiles reviennent à l'écran lors des dépassements
                    Rectangle.X = (int)Position.X;
                    Rectangle.Y = (int)Position.Y;
                }
            }
            else
            {
                if (_counterDestroy == 0)
                {
                    Texture = Content.Load<Texture2D>("Sprites/explosion");
                    Rectangle.Width = Rectangle.Height = 64;
                }
                _counterDestroy += deltaTime.ElapsedGameTime.Milliseconds;
                if(_counterDestroy >= DESTROY_INTERVAL)
                {
                    _removeTrigger = true;
                    DeleteExistence();
                }
            }
        }

        public override void Draw(SpriteBatch sprite)
        {
            sprite.Draw(Texture, Rectangle, null, Color.White, MathHelper.ToRadians(_angle), Origin, SpriteEffects.None, 0f);
        }
    }
}
