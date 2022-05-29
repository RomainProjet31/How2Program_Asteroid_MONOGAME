using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace How2Program_Asteroid_Desktop.Game_manager
{
    public class ScoreWindow
    {
        public bool IsGameOver { get => _gameOver; }

        private SpriteBatch _spriteBatch;
        private SpriteFont _basicFont;
        private Vector2 _scorePosition;
        private Vector2 _gameOverPosition;
        private int _score;
        private string _scoreMessage;
        private string _gameOverMessage;
        private bool _gameOver;


        public ScoreWindow(SpriteBatch spriteBatch, ContentManager content, Rectangle screen)
        {
            _spriteBatch = spriteBatch;
            _score = 0;
            _gameOver = false;
            _basicFont = content.Load<SpriteFont>("File");

            _scoreMessage = "Score : ";
            _gameOverMessage = "                GAME OVER \n Appuyez sur ESPACE pour recommencer";

            _scorePosition = new Vector2(10,5);
            _gameOverPosition = new Vector2(screen.Width / 3f, screen.Height / 2f);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw()
        {
            _spriteBatch.DrawString(_basicFont, _scoreMessage + _score, _scorePosition, Color.White);
            if (_gameOver)
            {
                _spriteBatch.DrawString(_basicFont, _gameOverMessage, _gameOverPosition, Color.White);
            }
        }


        // On passe par des méthodes et non pas les get;set de C# pour forcer l'utilisation de Restart.
        public void Restart()
        {
            _gameOver = false;
            _score = 0;
        }

        public void GameOver()
        {
            _gameOver = true;
        }

        public void IncrementScore()
        {
            _score++;
        }
    }
}
