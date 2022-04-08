using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.States
{
    internal abstract class State
    {
        public abstract void Init();
        public abstract void Update();

        public virtual void Pause() { }
        public virtual void Resume() { }
        public virtual void Remove() { }
    }
}
