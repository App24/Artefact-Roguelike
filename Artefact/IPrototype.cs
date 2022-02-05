using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact
{
    interface IPrototype<T>
    {
        T Clone();
    }
}
