using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TowerDefense.Menu
{
    public class MenuItemNode:MenuItemBaseNode
    {
        private List<MenuItemBaseNode> _Children;

        public List<MenuItemBaseNode> Children
        {
            get { return _Children; }
            set { _Children = value; }
        }

        public MenuItemNode(GameStage gameState, string strName)
            : base(gameState, strName)
        {
            Children = new List<MenuItemBaseNode>();
        }
    }
}
