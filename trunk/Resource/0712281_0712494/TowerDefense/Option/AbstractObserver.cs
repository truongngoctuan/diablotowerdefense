using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefense.Option
{
    public abstract class AbstractObserver
    {
        public abstract void Update(string strCommand);
    }
}
