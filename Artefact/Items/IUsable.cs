using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Items
{
    internal interface IUsable
    {
        string UseText { get; }

        bool OnUse();
    }
}
