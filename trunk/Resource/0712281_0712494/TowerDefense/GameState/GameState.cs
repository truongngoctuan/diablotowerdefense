using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TowerDefense.GameState
{
    public interface GameState:Base.BaseGameTemplate
    {
        void NextState(ref Game1 context);
        void PreviousState(ref Game1 context);
        //void Update(GameTime gameTime);
        //void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        //void Initialize();
        //void LoadContent(ContentManager content);
        void Clean();
    }
}
