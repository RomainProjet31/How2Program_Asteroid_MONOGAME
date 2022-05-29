using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace How2Program_Asteroid_Desktop.Game_manager.Sprites
{
    public class Asteroid : Sprite
    {
        private static float ASTEROID_SPEED = 2f;
        public bool IsDestroyed { get => _destroyed; }

        private bool _destroyed;
        private float _angle;
        private Vector2 _origin;

        public Asteroid(int _x, int _y, int _w, int _h, Rectangle screen, ContentManager content) : base(_x, _y, _w, _h, screen, content)
        {
            Texture = content.Load<Texture2D>("Sprites/meteor");
            _origin = new Vector2(_w / 2f, _h / 2f);
            InitRandomAngleAndPosition();
        }

        private void InitRandomAngleAndPosition()
        {
            Random rdm = new Random();
            _angle = rdm.Next(0, 360);

            Position.X = rdm.Next(0, Screen.Width);
            Position.Y = rdm.Next(0, Screen.Height);
        }

        public override void Update(GameTime deltaTime)
        {
            if (!_destroyed)
            {
                float rad = MathHelper.ToRadians(_angle);
                Position.X += ASTEROID_SPEED * MathF.Cos(rad);
                Position.Y += ASTEROID_SPEED * MathF.Sin(rad);
                base.Update(deltaTime);

                foreach (Sprite sprite in EXISTING_SPRITES)
                {
                    if (sprite.GetType() == typeof(Missile))
                    {
                        Missile missile = sprite as Missile;
                        if (Collides(missile))
                        {
                            missile.IsDestroyed = _destroyed = true;
                            DeleteExistence();
                            break;
                        }
                    }
                }
            }
        }

        public override void Draw(SpriteBatch sprite)
        {
            sprite.Draw(Texture, Rectangle, null, Color.White, MathHelper.ToRadians(_angle), _origin, SpriteEffects.None, 0f);
        }
    }
}
