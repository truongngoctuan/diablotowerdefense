using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TowerDefense.Option
{
    public abstract class AbstractObserver
    {
        //public abstract void Update(string strCommand);

        //public abstract void Update(Microsoft.Xna.Framework.Input.MouseState mouseState, Vector2 pos);
        public abstract void Update(GameTime gametime);
    }
}
