using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact
{
    internal interface IPrototype<T>
    {
        T Clone();
    }
}
