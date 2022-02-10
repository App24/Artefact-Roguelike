using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact
{
    [Flags]
    internal enum Direction
    {
        Up=1,
        Left=2,
        Right=4,
        Down=8,
    }
}
