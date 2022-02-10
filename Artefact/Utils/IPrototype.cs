using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Utils
{
    internal interface IPrototype<T>
    {
        T Clone();
    }
}
