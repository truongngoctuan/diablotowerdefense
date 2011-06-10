using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefense.Option
{
    public abstract class AbstractSubject
    {
        private AbstractObserver _observer = null;

        public AbstractObserver Observer
        {
            get { return _observer; }
            set { _observer = value; }
        }
        public void Atach(AbstractObserver objectIn)
        {
            if (_observer == null)
            {
                _observer = objectIn;
            }
            else
            {
                _observer = null;
                _observer = objectIn;
            }
        }

        public void Detach(AbstractSubject objectOut)
        {
            if (_observer.Equals(objectOut))
            {
                _observer = null;
            }
        }

        public void Notify()
        {
            if (Active != null)
            {
                Active();
            }
        }

        public delegate void ActiveFunction();
        public ActiveFunction Active = null;
    }
}
