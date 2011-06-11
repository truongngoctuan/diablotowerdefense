using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TowerDefense.Menu
{
    public class MenuItemLeaf:MenuItemBaseNode
    {
        public MenuItemBaseNode _parent;

        public MenuItemLeaf(GameStage gameState, string strName)
            : base(gameState, strName)
        {
            
        }
    }
}
