using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    public static class ParticleSystemManager
    {
        public static List<ParticleSystem> particleSystems = new List<ParticleSystem>();
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Draw(spriteBatch);
            }
        }
    }
}
