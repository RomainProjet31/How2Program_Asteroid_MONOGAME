
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace How2Program_Asteroid_Desktop.Game_manager.Sprites
{
    public class Ship : Sprite
    {
        private static float ANGLE_INTERVAL = 5f;
        private static float MAX_SPEED = 5f;
        private static float SHIP_THRUST = 2f;
        private static float FRICTION = 0.7f;
        private static float FPS = 60f;
        private static int SHOOT_INTERVAL = 200;

        public bool IsAlive { get => _alive; }

        private bool _alive;
        private Vector2 _thrust;
        private bool _thrustOrder;
        private float _thrustAngle;
        private float _shipAngle;
        private int _shootCounter;
        private List<Missile> _missiles;

        public Ship(int _x, int _y, int _w, int _h, Rectangle screen, ContentManager content) : base(_x, _y, _w, _h, screen, content)
        {
            Content = content;
            _alive = true;
            _shootCounter = 0;
            _missiles = new List<Missile>();
            Origin = new Vector2(_w / 2f, _h / 2f);
            Texture = Content.Load<Texture2D>("Sprites/ship_3");
        }

        public override void Update(GameTime gameTime)
        {
            _thrustOrder = false;
            _shootCounter += gameTime.ElapsedGameTime.Milliseconds;
            HandleKeyEvent();
            HandleBehavior();
            Position.X += _thrust.X;
            Position.Y += _thrust.Y;
            base.Update(gameTime);
            HandleCollisions();
            HandleMissiles(gameTime);
        }

        private void HandleCollisions()
        {
            foreach(Sprite sprite in EXISTING_SPRITES)
            {
                if(sprite.GetType() == typeof(Asteroid))
                {
                    Asteroid asteroid = sprite as Asteroid;
                    if (Collides(asteroid))
                    {
                        _alive = false;
                        break;
                    }
                }
            }
        }

        private void HandleMissiles(GameTime gameTime)
        {
            int i = 0;
            while (i < _missiles.Count)
            {
                Missile missile = _missiles[i];
                if (missile.RemoveTrigger)
                {
                    _missiles.RemoveAt(i);
                }
                else
                {
                    missile.Update(gameTime);
                    i++;
                }
            }
        }

        private void HandleKeyEvent()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Left))
            {
                _shipAngle -= ANGLE_INTERVAL;
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                _shipAngle += ANGLE_INTERVAL;
            }

            if (state.IsKeyDown(Keys.Up))
            {
                _thrustOrder = true;
                _thrustAngle = _shipAngle;
            }
            else
            {
                _thrustOrder = false;
            }

            if (state.IsKeyDown(Keys.Space))
            {
                Shoot();
            }
        }

        public void HandleBehavior()
        {
            if (_thrustOrder)
            {
                float rad = MathHelper.ToRadians(_thrustAngle);
                _thrust.X += SHIP_THRUST * MathF.Cos(rad) / FPS;
                _thrust.Y += SHIP_THRUST * MathF.Sin(rad) / FPS;

                if (_thrust.X < -MAX_SPEED) _thrust.X = -MAX_SPEED;
                else if (_thrust.X > MAX_SPEED) _thrust.X = MAX_SPEED;

                if (_thrust.Y > MAX_SPEED) _thrust.Y = MAX_SPEED;
                else if (_thrust.Y < -MAX_SPEED) _thrust.Y = -MAX_SPEED;
            }
            else
            {
                float frictionX = FRICTION * _thrust.X / FPS;
                float frictionY = FRICTION * _thrust.Y / FPS;
                _thrust.X -= frictionX;
                _thrust.Y -= frictionY;
            }
        }

        public override void Draw(SpriteBatch sprite)
        {
            _missiles.ForEach(missile => missile.Draw(sprite));
            sprite.Draw(Texture, Rectangle, null, Color.White, MathHelper.ToRadians(_shipAngle), Origin, SpriteEffects.None, 0f);
        }

        private void Shoot()
        {
            if (_shootCounter >= SHOOT_INTERVAL)
            {
                double x = Position.X + Rectangle.Width * MathF.Cos(MathHelper.ToRadians(_shipAngle));
                double y = Position.Y + Rectangle.Height * MathF.Sin(MathHelper.ToRadians(_shipAngle));
                Missile missile = new Missile((int)x, (int)y, 15, 15, Screen, Content, _shipAngle);
                _missiles.Add(missile);
                _shootCounter = 0;
            }
        }
    }
}
