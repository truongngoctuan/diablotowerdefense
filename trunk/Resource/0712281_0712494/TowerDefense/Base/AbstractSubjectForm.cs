using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TowerDefense.Option;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TowerDefense.Menu
{
    public class AbstractSubjectForm
    {
        private List<AbstractObserver> _observer = null;

        public List<AbstractObserver> Observer
        {
            get { return _observer; }
            set { _observer = value; }
        }

        public AbstractSubjectForm()
        {
            Observer = new List<AbstractObserver>();
        }

        public void Atach(AbstractObserver objectIn)
        {
            Observer.Add(objectIn);
        }

        public void Detach(AbstractObserver objectOut)
        {
            Observer.Remove(objectOut);
        }

        public void UpdateAllObserver(GameTime gametime)
        {
            for (int i = 0; i < Observer.Count; i++)
            {
                Observer[i].Update(gametime);
            }
        }
    }
}
