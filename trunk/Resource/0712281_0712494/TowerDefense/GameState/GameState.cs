using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TowerDefense.GameState
{
    public abstract class GameState
    {
        public abstract void ChangeGameState(ref Game1 context);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Initialize();
        public abstract void LoadContent(ContentManager content);
        public abstract void Clean();
    }
}
