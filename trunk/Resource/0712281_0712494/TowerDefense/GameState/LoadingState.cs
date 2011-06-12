using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefense.GameState
{
    public class LoadingState:GameState
    {
        #region GameState Members

        public void NextState(ref Game1 context)
        {
            throw new NotImplementedException();
        }

        public void PreviousState(ref Game1 context)
        {
            throw new NotImplementedException();
        }

        public void Clean()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region BaseGameTemplate Members

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            throw new NotImplementedException();
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
