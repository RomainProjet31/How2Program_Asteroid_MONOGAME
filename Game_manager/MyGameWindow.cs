using How2Program_Asteroid_Desktop.Game_manager.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace How2Program_Asteroid_Desktop.Game_manager
{
    public class MyGameWindow
    {
        private static int MIN_INTERVAL_MS = 1000;
        private static int MAX_INTERVAL_MS = 4000;

        private ScoreWindow _scoreWindow;

        private SpriteBatch _spriteBatch;
        private ContentManager _content;
        private Texture2D _bgTexture;
        private Rectangle _screen;
        private Random _random;
        private int _counter;
        private int _spawnInterval;
        private bool _paused;
        private Ship _player;
        private List<Asteroid> _asteroids;



        public MyGameWindow(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, ContentManager content)
        {
            _spriteBatch = spriteBatch;
            _content = content;
            _asteroids = new List<Asteroid>();
            _random = new Random();
            _screen = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            _bgTexture = content.Load<Texture2D>("Sprites/stars");
            _scoreWindow = new ScoreWindow(spriteBatch, content, _screen);
            InitOrRestart();
        }

        public void InitOrRestart()
        {
            // Détruire tous les sprites existants pour repartir sur une base nouvelle
            if (_player != null) _player.DeleteExistence();
            _asteroids.ForEach(asteroid => asteroid.DeleteExistence());

            _player = new Ship(100, 100, 32, 32, _screen, _content);
            _asteroids.Clear();
            _scoreWindow.Restart();
            _paused = false;
            _counter = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (!_scoreWindow.IsGameOver)
            {
                ManageAsteroidSpawn(gameTime);
                _player.Update(gameTime);
                int i = 0;
                while (i < _asteroids.Count)
                {
                    Asteroid asteroid = _asteroids[i];
                    if (asteroid.IsDestroyed)
                    {
                        _asteroids.RemoveAt(i);
                        _scoreWindow.IncrementScore();
                    }
                    else
                    {
                        asteroid.Update(gameTime);
                        i++;
                    }
                }
                if (!_player.IsAlive)
                {
                    _scoreWindow.GameOver();
                }
            }
            else
            {
                HandleKeyEvent();
            }
            _scoreWindow.Update(gameTime);
        }

        private void HandleKeyEvent()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Space))
            {
                InitOrRestart();
            }
        }

        private void ManageAsteroidSpawn(GameTime gameTime)
        {
            _counter += gameTime.ElapsedGameTime.Milliseconds;
            if (_counter >= _spawnInterval)
            {
                _counter = 0;
                _spawnInterval = _random.Next(MIN_INTERVAL_MS, MAX_INTERVAL_MS);
                AddAsteroid();
            }
        }

        private void AddAsteroid()
        {
            _asteroids.Add(new Asteroid(-1, -1, 32, 32, _screen, _content));
        }

        public void Draw()
        {
            _spriteBatch.Begin();

            _spriteBatch.Draw(_bgTexture, _screen, Color.White);
            _player.Draw(_spriteBatch);
            foreach (Asteroid asteroid in _asteroids)
            {
                asteroid.Draw(_spriteBatch);
            }
            _scoreWindow.Draw();

            _spriteBatch.End();
        }
    }
}
